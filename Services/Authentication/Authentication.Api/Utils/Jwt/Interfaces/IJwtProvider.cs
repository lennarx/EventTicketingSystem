using Authentication.Api.Data;

namespace Authentication.Api.Utils.Jwt.Interfaces
{
    public interface IJwtProvider
    {
        string GenerateToken(AuthUser user);
    }
}
