using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Messaging.Events.Tickets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Orchestration.Api.Features
{
    public static class StartSagaEndpoint
    {
        public static IEndpointRouteBuilder MapStartSaga(this IEndpointRouteBuilder app)
        {
            app.MapPost("", async ([FromBody] StartSagaCommand command, ISender sender) =>
            {
                await sender.Send(command);
                return Results.Ok("Saga started.");
            });

            return app;
        }
    }
}
