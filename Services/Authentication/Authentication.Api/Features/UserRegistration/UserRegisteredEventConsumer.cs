using RabbitMQ.Client;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Shared.Messaging.Events.Users;
using Newtonsoft.Json;
using Authentication.Api.Data;

namespace Authentication.Api.Features.UserRegistration
{
    public class UserRegisteredEventConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;

        public UserRegisteredEventConsumer(IServiceScopeFactory scopeFactory, IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["Rabbit:HostName"] ?? "localhost"
            };

            var connection = await factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            var exchange = "userregisteredevent.exchange";
            await channel.ExchangeDeclareAsync(exchange, ExchangeType.Fanout, durable: true);

            var queueResult = await channel.QueueDeclareAsync(exclusive: true);
            await channel.QueueBindAsync(queueResult.QueueName, exchange, "");

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await channel.BasicGetAsync(queueResult.QueueName, autoAck: true);

                if (result is null)
                {
                    await Task.Delay(1000, stoppingToken);
                    continue;
                }

                var json = Encoding.UTF8.GetString(result.Body.ToArray());
                var evt = JsonConvert.DeserializeObject<UserRegisteredEvent>(json);

                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AuthDbContext>();

                var exists = await db.Users.AnyAsync(u => u.Email == evt.Email);
                if (!exists)
                {
                    db.Users.Add(new AuthUser
                    {
                        Id = evt.Id,
                        Email = evt.Email,
                        PasswordHash = evt.PasswordHash
                    });

                    await db.SaveChangesAsync();
                }
            }
        }
    }
}
