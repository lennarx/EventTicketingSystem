using MassTransit;
using MediatR;
using Shared.Messaging.Commands.Tickets;

namespace Orchestration.Api.Features
{
    public class StartSagaHandler : IRequestHandler<StartSagaCommand>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        public StartSagaHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(StartSagaCommand request, CancellationToken cancellationToken)
        {
            await _publishEndpoint.Publish(new CreateReservationCommand
            {
                EventId = request.eventId,
                UserId = request.userId,
                Quantity = request.quantity
            });
        }
    }
}
