using Microsoft.Extensions.Logging;
using Tickets.Application.Dtos;
using Tickets.Application.Mappers;
using Tickets.Application.Services;
using Tickets.Domain.Entities;
using Tickets.Domain.Enums;
using Tickets.Infrastructure.Repositories;

namespace Tickets.Infrastructure.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Ticket> _repository;

        public ReservationService(IRepository<Ticket> repository)
        {
            _repository = repository;
        }

        public async Task<ReservationDto> CancelReservationAsync(Guid reservationId)
        {
            var ticketsToCancel = _repository.GetByCondition(x => x.ReservationId == reservationId);
            if (ticketsToCancel == null || !ticketsToCancel.Any())
                return new ReservationDto();

            var ticketsCancelled = ticketsToCancel.Select(x => CancelTicket(x));
            await _repository.UpdateRangeAsync(ticketsCancelled);

            return new ReservationDto
            {
                EventId = ticketsToCancel.First().EventId,
                ReservationId = reservationId,
                UserId = ticketsToCancel.First().UserId,
            };
        }

        public async Task<ReservationDto> ConfirmReservationAsync(Guid reservationId)
        {
            var ticketsToConfirm = _repository.GetByCondition(x => x.ReservationId == reservationId);
            if (ticketsToConfirm == null || ticketsToConfirm.Count() == 0 || ticketsToConfirm.Any(x => x.Status != TicketStatusEnum.Reserved))
                return new ReservationDto();

            var confirmedTickets = ticketsToConfirm.Select(ticket => ConfirmTicket(ticket));
            await _repository.UpdateRangeAsync(confirmedTickets);

            return new ReservationDto
            {
                ReservationId = reservationId,
                EventId = ticketsToConfirm.First().EventId,
                UserId = ticketsToConfirm.First().UserId,
                ReservationStatus = nameof(TicketStatusEnum.Confirmed),
            };
        }

        public async Task<TicketDto> GetTicketById(Guid id)
        {
            return (await _repository.GetByIdAsync(id))?.MapToDto();
        }

        public IEnumerable<TicketDto> GetTicketByUserId(Guid id)
        {
            return _repository.GetByCondition(x => x.UserId == id).MapToDtos();
        }

        public async Task<IEnumerable<TicketDto>> ReserveTicketsAsync(Guid eventId, Guid userId, int quantity)
        {
            ICollection<Ticket> tickets = new List<Ticket>();
            var reservationId = Guid.NewGuid();
            for (int i = 0; i < quantity; i++)
            {
                var ticket = new Ticket
                {
                    EventId = eventId,
                    UserId = userId,
                    Status = TicketStatusEnum.Reserved,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    ReservationId = reservationId
                };
                tickets.Add(ticket);
            }
            await _repository.AddRangeAsync(tickets);

            return tickets.MapToDtos();
        }

        private Ticket CancelTicket(Ticket ticket)
        {
            ticket.Status = TicketStatusEnum.Canceled;
            ticket.UpdatedAt = DateTime.UtcNow;
            return ticket;
        }

        private Ticket ConfirmTicket(Ticket ticket)
        {
            ticket.Status = TicketStatusEnum.Confirmed;
            ticket.UpdatedAt = DateTime.UtcNow;
            return ticket;
        }
    }
}
