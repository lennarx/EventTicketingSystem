namespace Shared.Result
{
    public sealed record Error(int HttpStatusCode, string? Message = null);
}
