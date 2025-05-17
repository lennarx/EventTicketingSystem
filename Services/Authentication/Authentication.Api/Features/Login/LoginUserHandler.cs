using Authentication.Api.Data;
using Authentication.Api.Utils.Jwt.Interfaces;
using MediatR;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Result;

namespace Authentication.Api.Features.Login
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<string, Error>>   
    {
        private readonly AuthDbContext _db;
        private readonly IJwtProvider _jwtProvider;
        private readonly PasswordHasher _passwordHasher;

        public LoginUserHandler(AuthDbContext db, IJwtProvider jwtProvider)
        {
            _db = db;
            _jwtProvider = jwtProvider;
            _passwordHasher = new PasswordHasher();
        }

        public async Task<Result<string, Error>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
            if (user is null)
                return Result<string, Error>.Failure(new Error(404, "User not found"));

            var hashedVerificationResult = _passwordHasher.VerifyHashedPassword(user.PasswordHash, request.Password);
            if (hashedVerificationResult == Microsoft.AspNet.Identity.PasswordVerificationResult.Failed)
                return Result<string, Error>.Failure(new Error(400, "Invalid password"));

            var token = _jwtProvider.GenerateToken(user);
            return Result<string, Error>.Success(token);
        }
    }
}
