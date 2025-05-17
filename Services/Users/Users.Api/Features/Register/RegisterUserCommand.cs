using MediatR;
using Shared.Result;

namespace Users.Api.Features.Register
{
    public sealed record RegisterUserCommand(string Email, string Password) : IRequest<Result<Guid, Error>> { }
}
