using EventBus;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RabbitMQEventBus
{
    // Lightweight version of "DefaultRabbitMQPerssistentConnection" class from Microsoft eShopOnContainers.
    // Unsubscription from events does not delete queues!
    public class RmqEventBus : IEventBus
    {
        private const string BrokerName = "rmq_event_bus";

        private readonly ISubscriptionManager _subscriptionManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly RmqConnection _connection;
        private readonly ILogger<RmqEventBus> _logger;

        private string _consumerQueue;
        private IModel _consumerModel;

        public RmqEventBus(
            ISubscriptionManager subscriptionManager,
            IServiceScopeFactory serviceScopeFactory,
            RmqConnection connection,
            ILogger<RmqEventBus> logger,
            string consumerQueue)
        {
            _subscriptionManager = subscriptionManager;
            _serviceScopeFactory = serviceScopeFactory;
            _connection = connection;
            _logger = logger;
            _consumerQueue = consumerQueue;
            _consumerModel = CreateConsumerModel();
        }

        public void Publish<TEvent>(TEvent integrationEvent) where TEvent : IntegrationEvent
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            using var model = _connection.CreateModel();

            model.ExchangeDeclare(exchange: BrokerName, type: "fanout");

            model.BasicPublish(
                exchange: BrokerName,
                routingKey: integrationEvent.GetType().Name,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(integrationEvent)));
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            if (!_subscriptionManager.HasAnyHandler<TEvent>())
            {
                if (_connection.IsConnected)
                {
                    _connection.TryConnect();
                }

                using var model = _connection.CreateModel();

                model.QueueBind(
                    queue: _consumerQueue,
                    exchange: BrokerName,
                    routingKey: _subscriptionManager.GetEventName<TEvent>());
            }

            _subscriptionManager.Subscribe<TEvent, TEventHandler>();

            if (_consumerModel is not null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerModel);

                consumer.Received += OnConsumerReceived;

                _consumerModel.BasicConsume(
                    queue: _consumerQueue,
                    autoAck: false,
                    consumer: consumer);
            }
        }

        public void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            _subscriptionManager.Unsubscribe<TEvent, TEventHandler>();
        }

        private IModel CreateConsumerModel()
        {
            if (!_connection.IsConnected)
            {
                _connection.TryConnect();
            }

            var model = _connection.CreateModel();

            model.ExchangeDeclare(exchange: BrokerName, type: "fanout");

            model.QueueDeclare(
                queue: _consumerQueue,
                exclusive: false,
                autoDelete: false);

            return model;
        }

        private async Task OnConsumerReceived(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var eventBody = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                await ProcessEvent(eventName, eventBody);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, exception.Message);
            }

            _consumerModel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private async Task ProcessEvent(string eventName, string eventBody)
        {
            if (!_subscriptionManager.HasAnyHandler(eventName))
            {
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();

            var handlerTypes = _subscriptionManager.GetHandlersOf(eventName);

            foreach (var handlerType in handlerTypes)
            {
                var handlerInstance = scope.ServiceProvider.GetService(handlerType);

                if (handlerInstance is null)
                {
                    continue;
                }

                var eventType = _subscriptionManager.GetEventType(eventName);

                var eventInstance = JsonSerializer.Deserialize(eventBody, eventType);

                await Task.Yield();
                await (Task)(typeof(IIntegrationEventHandler<>).MakeGenericType(eventType))
                    .GetMethod("Handle")
                    .Invoke(handlerInstance, new object[] { eventInstance });
            }
        }
    }
}
