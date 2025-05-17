using MediatR;

namespace Authentication.Api.Features.Login
{
    public static class LoginUserEndpoint
    {
        public static IEndpointRouteBuilder MapLogin(this IEndpointRouteBuilder app)
        {
            app.MapPost("/login", async (LoginUserCommand command, ISender sender) =>
            {
                var result = (await sender.Send(command))
                    .Match(value => value, error => error);

                if (result.Error != null)
                {
                    return Results.Problem(result.Error.Message, null, result.Error.HttpStatusCode);
                }
                else
                {
                    return Results.Ok(result?.Value);
                }
            });

            return app;
        }
    }
}
