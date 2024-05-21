namespace PaymentGateway.Api.Models;

public enum PaymentStatus
{
    Unknown,
    Authorized, // the payment was authorized by the call to the acquiring bank
    Declined, // the payment was declined by the call to the acquiring bank
    Rejected, // No payment could be created as invalid information was supplied to the payment gateway and therefore it has rejected the request without calling the acquiring bank
}