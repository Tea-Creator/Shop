using Microsoft.Extensions.Options;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

using System;
using System.Threading;

namespace EventBus
{
    public class RabbitMqConnection : IDisposable
    {
        private readonly IConnection _connection;

        public RabbitMqConnection(IOptions<RabbitMqConnectionOptions> options)
        {
            _connection = CreateConnection(options.Value);
        }

        public IModel CreateModel() => _connection.CreateModel();

        private static IConnection CreateConnection(RabbitMqConnectionOptions options)
        {
            int wait = 0;

            var factory = new ConnectionFactory
            {
                HostName = options.Host,
                UserName = options.Username,
                Password = options.Password,
            };

            do
            {
                try
                {
                    var connection = factory.CreateConnection();

                    if (connection is not null && connection.IsOpen)
                    {
                        return connection;
                    }
                }
                catch (BrokerUnreachableException)
                {
                    Thread.Sleep(options.ConnectFailureDelay);
                    wait += options.ConnectFailureDelay;
                }
            } while (wait < options.ConnectTimeout);

            throw new TimeoutException("Can't connect to rabbitmq. Timeout");
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _connection?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
