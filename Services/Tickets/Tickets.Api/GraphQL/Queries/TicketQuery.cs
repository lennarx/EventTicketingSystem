using HotChocolate.Authorization;
using Tickets.Application.Dtos;
using Tickets.Application.Services;

namespace Tickets.Api.GraphQL.Queries
{
    public class TicketQuery
    {
        [Authorize]
        public async Task<TicketDto> GetTicketByIdAsync(Guid id, [Service] IReservationService ticketService)
            => await ticketService.GetTicketById(id);
        [Authorize]
        public IEnumerable<TicketDto> GetTicketsByUserIdAsync(Guid userId, [Service] IReservationService ticketService)
            => ticketService.GetTicketByUserId(userId);
    }
}
