using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging.Events.Users;
using Shared.Messaging.Interfaces;
using Shared.Result;
using Users.Api.Data;

namespace Users.Api.Features.Register
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<Guid, Error>>
    {
        private readonly UsersDbContext _db;
        private readonly IEventPublisher _eventPublisher;
        public RegisterUserHandler(UsersDbContext db, IEventPublisher eventPublisher)
        {
            _db = db;
            _eventPublisher = eventPublisher;
        }

        public async Task<Result<Guid, Error>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _db.Users.AnyAsync(u => u.Email == request.Email, cancellationToken))
                return Result<Guid, Error>.Failure(new Error(400, "Email already exists"));

            var hasher = new PasswordHasher();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                PasswordHash = hasher.HashPassword(request.Password)
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);

            await _eventPublisher.PublishAsync(
                new UserRegisteredEvent(user.Id, user.Email, user.PasswordHash)
            );

            return Result<Guid, Error>.Success(user.Id);
        }
    }
}
