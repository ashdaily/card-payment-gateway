namespace PaymentGateway.Api.Models;

public record PaymentResponse(
    string Id,
    PaymentStatus Status,
    string LastFourDigits,
    uint ExpiryMonth,
    uint ExpiryYear,
    string Currency,
    uint Amount);
