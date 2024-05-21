using PaymentGateway.Api.Models;

namespace PaymentGateway.Api.Services;

public interface IPaymentService
{
    Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request);

    Task<PaymentResponse> GetPaymentAsync(string id);
}
