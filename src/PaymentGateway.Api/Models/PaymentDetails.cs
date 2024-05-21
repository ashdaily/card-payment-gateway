using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Api.Models;

public class PaymentDetails
{
    [Key] 
    public required string PaymentId { get; set; }
    public required PaymentStatus Status { get; set; }
    public required string CardNumber { get; set; }
    public required uint ExpiryMonth { get; set; }
    public required uint ExpiryYear { get; set; }
    public required string Currency { get; set; }
    public required uint Amount { get; set; }
    public DateTime PaymentInitiationTime { get; set; }
}