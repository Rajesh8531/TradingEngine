using Application.Common.Interface;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class RabbitMQPublisher: IEventPublisher, IAsyncDisposable
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private readonly SemaphoreSlim _connectionLock = new(1, 1);

        public RabbitMQPublisher(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
            {
                await _connection.CloseAsync();
                await _connection.DisposeAsync();
            }
        }

        private async Task<IConnection> GetConnectionAsync(CancellationToken ct)
        {
            if (_connection is not null) return _connection;
            await _connectionLock.WaitAsync(ct);
            try
            {
                _connection ??= await _factory.CreateConnectionAsync(ct);
                return _connection;
            }
            finally
            {
                _connectionLock.Release();
            }
        }

        public async Task PublishAsync(ReadOnlyMemory<byte> body, string exchangeName, string routingKey, CancellationToken ct = default)
        {
            var connection = await GetConnectionAsync(ct);

            await using var channel = await connection.CreateChannelAsync(cancellationToken:ct);

            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type : ExchangeType.Topic,
                durable : true,
                cancellationToken : ct
                );

            var properties = new BasicProperties()
            {
                Persistent = true
            };

            await channel.BasicPublishAsync(
            exchange: exchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: ct
            );
        }
    }
}
