using Microsoft.Extensions.DependencyInjection;

using RabbitMQ.Client;

using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EventBus
{
    public class RabbitMqEventBus : IEventBus
    {
        private const string EXCHANGE = "shop_exchange";

        private readonly RabbitMqConnection _connection;
        private readonly IEventBusSubscriptionManager _subscriptionManager;
        private readonly IServiceScopeFactory _serviceScopeFactory;



        public RabbitMqEventBus(
            RabbitMqConnection connection,
            IEventBusSubscriptionManager subscriptionManager,
            IServiceScopeFactory serviceScopeFactory)
        {
            _connection = connection;
            _subscriptionManager = subscriptionManager;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public void Publish<TEvent>(TEvent integrationEvent) where TEvent : IntegrationEvent
        {
            using var model = _connection.CreateModel();

            model.ExchangeDeclare(EXCHANGE, "fanout");

            model.BasicPublish(
                exchange: EXCHANGE,
                routingKey: typeof(TEvent).FullName,
                body: Encoding.UTF8.GetBytes(JsonSerializer.Serialize(integrationEvent)));
        }

        public void Subscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            _subscriptionManager.AddSubscription<TEvent, TEventHandler>();
        }

        public void Unsubscribe<TEvent, TEventHandler>()
            where TEvent : IntegrationEvent
            where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            _subscriptionManager.RemoveSubscription<TEvent, TEventHandler>();
        }

        private IModel CreateConsumer()
        {
            var model = _connection.CreateModel();

            model.ExchangeDeclare(exchange: EXCHANGE, type: "fanout");

            model.QueueDeclare(queue: )
        }

        private async Task ProcessEvent(string eventName, string eventBody)
        {
            if (_subscriptionManager.HasAnyHandler(eventName))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    foreach (var handlerType in _subscriptionManager.GetHandlersOf(eventName))
                    {
                        var handler = scope.ServiceProvider.GetService(handlerType);

                        if (handler is null)
                        {
                            continue;
                        }

                        var eventType = Type.GetType(eventName);

                        var @event = JsonSerializer.Deserialize(eventBody, Type.GetType(eventName));

                        await Task.Yield();

                        await (Task)typeof(IIntegrationEventHandler<>)
                            .MakeGenericType(eventType)
                            .GetMethod("Handle", Array.Empty<Type>())
                            .Invoke(handler, new object[] { @event });
                    }
                }
            }
        }
    }
}
