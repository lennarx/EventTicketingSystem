using HotChocolate.Authorization;
using Tickets.Application.Dtos;
using Tickets.Application.Services;

namespace Tickets.Api.GraphQL.Mutations
{
    public class TicketMutation
    {
        [Authorize]
        public async Task<IEnumerable<TicketDto>> ReserveTicketAsync(Guid eventId, Guid userId, int quantity, [Service] IReservationService service)
        => await service.ReserveTicketsAsync(eventId, userId, quantity);
        [Authorize]
        public async Task<ReservationDto> ConfirmTicketAsync(Guid reservationId, [Service] IReservationService service)
            => await service.ConfirmReservationAsync(reservationId);
        [Authorize]
        public async Task<ReservationDto> CancelReservationAsync(Guid reservationId, [Service] IReservationService service)
            => await service.CancelReservationAsync(reservationId);
    }
}
