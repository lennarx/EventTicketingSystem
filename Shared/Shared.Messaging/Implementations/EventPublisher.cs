using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System.Text;
using Shared.Messaging.Interfaces;
using Newtonsoft.Json;

namespace Shared.Messaging.Implementations
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IChannel _channel;

        public EventPublisher(IChannel channel)
        {
            _channel = channel;            
        }

        public async Task PublishAsync<T>(T evt) where T: class
        {
            var json = JsonConvert.SerializeObject(evt);
            var body = Encoding.UTF8.GetBytes(json);

            var exchange = $"{typeof(T).Name.ToLowerInvariant()}.exchange";

            await _channel.ExchangeDeclareAsync(
                exchange: exchange,
                type: ExchangeType.Fanout,
                durable: true
            );

            await _channel.BasicPublishAsync(
                exchange: exchange,
                routingKey: "",
                mandatory: false,
                basicProperties: new BasicProperties { ContentType = "application/json" },
                body: body
            );
        }
    }
}
