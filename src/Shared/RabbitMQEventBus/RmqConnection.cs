using Microsoft.Extensions.Logging;

using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace RabbitMQEventBus
{
    // Lightweight version of "DefaultRabbitMQPerssistentConnection" class from Microsoft eShopOnContainers.
    public class RmqConnection
    {
        private readonly object _locker = new object();

        private readonly IConnectionFactory _connectionFactory;
        private readonly ILogger<RmqConnection> _logger;
        private IConnection _connection;
        private bool _disposed;

        public RmqConnection(
            IConnectionFactory connectionFactory,
            ILogger<RmqConnection> logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
        }

        public bool IsConnected => _connection is not null && _connection.IsOpen && !_disposed;

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No connections are available");
            }

            return _connection.CreateModel();
        }

        public bool TryConnect()
        {
            _logger.LogInformation("Starting connect attempts");

            lock (_locker)
            {
                int retries = 0;

                while (retries < 5)
                {
                    try
                    {
                        _connection = _connectionFactory.CreateConnection();
                    }
                    catch (BrokerUnreachableException e)
                    {
                        _logger.LogWarning(e, e.Message);
                    }
                    catch (SocketException e)
                    {
                        _logger.LogWarning(e, e.Message);
                    }

                    if (IsConnected)
                    {
                        break;
                    }

                    retries++;

                    Thread.Sleep(2500);
                }

                if (IsConnected)
                {
                    _connection.ConnectionShutdown += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            _logger.LogWarning("RabbitMQ connection was shutdown. Trying to re-connect");
                            TryConnect();
                        }
                    };

                    _connection.CallbackException += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            _logger.LogWarning("RabbitMQ connection throw exception. Trying to re-connect");
                            TryConnect();
                        }
                    };

                    _connection.ConnectionBlocked += (sender, args) =>
                    {
                        if (!_disposed)
                        {
                            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

                            TryConnect();
                        }
                    };

                    _logger.LogInformation("Connected to rabbitmq");
                    return true;
                }

                _logger.LogCritical("Can't connect to rabbitmq");

                return false;
            }
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException exception)
            {
                _logger.LogCritical(exception, exception.Message);
            }
        }
    }
}
