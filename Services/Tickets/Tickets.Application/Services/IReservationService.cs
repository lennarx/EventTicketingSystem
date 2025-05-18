using Tickets.Application.Dtos;

namespace Tickets.Application.Services
{
    public interface IReservationService
    {
        IEnumerable<TicketDto> GetTicketByUserId(Guid id);
        Task<TicketDto> GetTicketById(Guid id);
        Task<IEnumerable<TicketDto>> ReserveTicketsAsync(Guid eventId, Guid userId, int quantity);
        Task<ReservationDto> ConfirmReservationAsync(Guid reservationId);
        Task<ReservationDto> CancelReservationAsync(Guid reservationId);

    }
}
