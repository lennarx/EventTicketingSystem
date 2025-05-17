namespace Shared.Messaging.Events.Users
{
    public record UserRegisteredEvent(Guid Id, string Email, string PasswordHash);
}
