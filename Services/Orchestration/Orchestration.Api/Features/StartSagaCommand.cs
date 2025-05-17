using MediatR;

namespace Orchestration.Api.Features
{
    public record StartSagaCommand(Guid userId, Guid eventId, int quantity) : IRequest;
}
