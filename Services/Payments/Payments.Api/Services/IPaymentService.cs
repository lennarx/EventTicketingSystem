namespace Payments.Api.Services
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(Guid userId, decimal amount);
    }
}
