using MassTransit;
using Payments.Api.Services;
using Shared.Messaging.Commands.Payments;
using Shared.Messaging.Events.Payments;

namespace Payments.Api.Consumers
{
    public class ProcessPaymentCommandConsumer : IConsumer<ProcessPaymentCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IPaymentService _paymentService;
        public ProcessPaymentCommandConsumer (IPublishEndpoint publish, IPaymentService paymentService)
        {
            _publishEndpoint = publish;
            _paymentService = paymentService;
        }
        public async Task Consume(ConsumeContext<ProcessPaymentCommand> context)
        {
            var message = context.Message;
            // Simulate payment processing
            var isSuccess = await _paymentService.ProcessPayment(message.UserId, message.Amount);
            if (isSuccess)
            {
                await _publishEndpoint.Publish(new PaymentSucceededEvent
                {
                    ReservationId = context.Message.ReservationId,
                    UserId = context.Message.UserId,
                    Amount = context.Message.Amount
                });
            }
            else
                await _publishEndpoint.Publish(new PaymentFailedEvent
                {
                    ReservationId = context.Message.ReservationId,
                    UserId = context.Message.UserId,
                    Amount = context.Message.Amount
                });
        }
    }
}
