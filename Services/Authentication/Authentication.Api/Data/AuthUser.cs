namespace Authentication.Api.Data
{
    public class AuthUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
}
