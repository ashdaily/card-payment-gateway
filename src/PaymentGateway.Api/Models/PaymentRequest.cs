namespace PaymentGateway.Api.Models;

public record PaymentRequest(
    string? PaymentId,
    string CardNumber,
    uint ExpiryMonth,
    uint ExpiryYear,
    string Currency,
    uint Amount,
    string Cvv
);