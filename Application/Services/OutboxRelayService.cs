using Application.Common.Interface;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Unicode;

namespace Application.Services
{
    public class OutboxRelayService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OutboxRelayService> _logger;
        private readonly IEventPublisher _publisher;

        public OutboxRelayService(IServiceScopeFactory scopeFactory, ILogger<OutboxRelayService> logger, RabbitMQPublisher publisher)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting Outbox Relay Service...");

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await ProcessOutboxMessagesAsync(stoppingToken);
            }
        }

        private async Task ProcessOutboxMessagesAsync(CancellationToken stoppingToken)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedOn == null)
                .OrderBy(m => m.CreatedAt)
                .Take(100)
                .ToListAsync(stoppingToken);

            if(messages.Count == 0)
            {
                _logger.LogInformation("No unprocessed outbox messages found.");
                return;
            }

            foreach (var message in messages)
            {
                try
                {
                    var body = Encoding.UTF8.GetBytes(message.Payload);
                    await _publisher.PublishAsync(body, "trading_orders", "orders.created", stoppingToken);

                    message.ProcessedOn = DateTime.UtcNow;
                    message.Error = null;
                }
                catch (Exception ex)
                {
                    _logger.LogError($" Failed to publish message on the queue. Error -> {ex.Message} ");
                    message.Error = ex.Message;
                    break;
                }
            }

            await dbContext.SaveChangesAsync(stoppingToken);
        }
    }
}
