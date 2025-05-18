using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Orchestration.Api.Features
{
    public static class StartSagaEndpoint
    {
        public static IEndpointRouteBuilder MapStartSaga(this IEndpointRouteBuilder app)
        {
            app.MapPost("",
                [Authorize] async ([FromBody] StartSagaCommand command, ISender sender) =>
            {
                await sender.Send(command);
                return Results.Accepted("Saga started.");
            });

            return app;
        }
    }
}
