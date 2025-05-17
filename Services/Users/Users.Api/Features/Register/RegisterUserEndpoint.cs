using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Users.Api.Features.Register
{
    public static class RegisterUserEndpoint
    {
        public static IEndpointRouteBuilder MapRegisterUser(this IEndpointRouteBuilder app)
        {
            app.MapPost("/users", 
                [Authorize] async (RegisterUserCommand command, ISender sender) =>
            {
                if (string.IsNullOrWhiteSpace(command.Email) || !IsValidEmail(command.Email))
                    return Results.BadRequest(new { error = "Invalid Email." });

                if (string.IsNullOrWhiteSpace(command.Password) || command.Password.Length < 6)
                    return Results.BadRequest(new { error = "Password must have at least 6 characters." });

                var result = (await sender.Send(command))
                    .Match(resultValue => resultValue, error => error);

                if (result.Error != null)
                {
                    return Results.Problem(result.Error.Message, null, result.Error.HttpStatusCode);
                }
                else
                {
                    return Results.Created($"/users/{result.Value}", new { Id = result.Value });
                }
            });

            return app;
        }

        private static bool IsValidEmail(string email)
        {
            return System.Net.Mail.MailAddress.TryCreate(email, out _);
        }
    }
}
