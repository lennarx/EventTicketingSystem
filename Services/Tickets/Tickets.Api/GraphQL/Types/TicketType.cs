using HotChocolate.Types;
using Tickets.Application.Dtos;

namespace Tickets.Api.GraphQL.Types
{
    public class TicketType : ObjectType<TicketDto>
    {
        protected override void Configure(IObjectTypeDescriptor<TicketDto> descriptor)
        {
            descriptor.Field(f => f.Id).Type<NonNullType<UuidType>>();
            descriptor.Field(f => f.UserId).Type<NonNullType<UuidType>>();
            descriptor.Field(f => f.TicketStatus).Type<NonNullType<StringType>>();
            descriptor.Field(f => f.ReservationId).Type<NonNullType<UuidType>>();
        }
    }
}
