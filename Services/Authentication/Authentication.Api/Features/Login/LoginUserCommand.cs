using MediatR;
using Shared.Result;

namespace Authentication.Api.Features.Login
{
    public record LoginUserCommand(string Email, string Password) : IRequest<Result<string, Error>>;
}
