using HotChocolate.Authorization;
using Tickets.Application.Dtos;
using Tickets.Application.Services;

namespace Tickets.Api.GraphQL.Mutations
{
    public class TicketMutation
    {
        [Authorize]
        public async Task<IEnumerable<TicketDto>> ReserveTicketAsync(Guid eventId, Guid userId, int quantity, [Service] ITicketService service)
        => await service.ReserveTicketsAsync(eventId, userId, quantity);
        [Authorize]
        public async Task<TicketDto> ConfirmTicketAsync(Guid reservationId, [Service] ITicketService service)
            => await service.ConfirmReservationAsync(reservationId);
        [Authorize]
        public async Task<TicketDto> CancelReservationAsync(Guid reservationId, [Service] ITicketService service)
            => await service.CancelReservationAsync(reservationId);
        [Authorize]
        public async Task<bool> MarkAsUsedAsync(Guid ticketId, [Service] ITicketService service)
            => await service.MarkAsUsedAsync(ticketId);
    }
}
