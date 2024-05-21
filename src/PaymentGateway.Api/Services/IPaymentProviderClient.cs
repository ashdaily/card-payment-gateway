using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Dtos;

namespace PaymentGateway.Api.Services;

public interface IPaymentProviderClient
{
    Task<PaymentProviderResponse> SendPayment(PaymentProviderRequest request);
}