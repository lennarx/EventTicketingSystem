using Tickets.Application.Dtos;

namespace Tickets.Application.Services
{
    public interface ITicketService
    {
        IEnumerable<TicketDto> GetTicketByUserId(Guid id);
        Task<TicketDto> GetTicketById(Guid id);
        Task<IEnumerable<TicketDto>> ReserveTicketsAsync(Guid eventId, Guid userId, int quantity);
        Task<TicketDto> ConfirmReservationAsync(Guid reservationId);
        Task<TicketDto> CancelReservationAsync(Guid reservationId);
        Task<bool> MarkAsUsedAsync(Guid ticketId);

    }
}
