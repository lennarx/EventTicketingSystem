using System.Net.Sockets;
using Tickets.Application.Dtos;
using Tickets.Domain.Entities;

namespace Tickets.Application.Mappers
{
    public static class TicketDtoMapper
    {
        public static TicketDto MapToDto(this Ticket ticket)
        {
            return new TicketDto
            {
                Id = ticket.Id,
                UserId = ticket.UserId,
                ReservationId = ticket.ReservationId,
                EventId = ticket.ReservationId,
                TicketStatus = nameof(ticket.Status)
            };
        }
        public static Ticket MapToEntity(this TicketDto ticketDto)
        {
            return new Ticket
            {
                Id = ticketDto.Id,
                UserId = ticketDto.UserId,
                ReservationId = ticketDto.ReservationId ?? Guid.Empty
            };
        }

        public static ICollection<TicketDto> MapToDtos(this IEnumerable<Ticket> tickets)
        {
            return tickets.Select(ticket => ticket.MapToDto()).ToList();
        }

        public static ICollection<Ticket> MapToEntities(this IEnumerable<TicketDto> ticketDtos)
        {
            return ticketDtos.Select(ticketDto => ticketDto.MapToEntity()).ToList();
        }
    }
}
