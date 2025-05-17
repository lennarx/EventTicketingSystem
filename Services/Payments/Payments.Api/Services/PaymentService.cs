namespace Payments.Api.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _client;

        public PaymentService(HttpClient client)
        {
            _client = client;
        }

        public async Task<bool> ProcessPayment(Guid userId, decimal amount)
        {
            // Simulate payment processing
            var random = new Random();
            // Simulate success or failure
            return random.Next(1, 10) % 2 == 0; 
        }
    }
}
