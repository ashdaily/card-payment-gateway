using Microsoft.AspNetCore.Mvc;

using Moq;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Tests;

public class PaymentsControllerTests
{
    private readonly PaymentsController _controller;
    private readonly Mock<IPaymentService> _paymentServiceMock;

    public PaymentsControllerTests()
    {
        _paymentServiceMock = new Mock<IPaymentService>();
        _controller = new PaymentsController(_paymentServiceMock.Object);
    }

    [Fact]
    public async Task ProcessPayment_ReturnsOk_WithValidResponse()
    {
        // Arrange
        var paymentRequest = new PaymentRequest(
            PaymentId: Guid.NewGuid().ToString(),
            Amount: 100,
            Currency: "USD",
            CardNumber: "",
            ExpiryMonth: 12,
            ExpiryYear: 2023,
            Cvv: "123"
        );
        var expectedPaymentResponse = new PaymentResponse(
            Id: "unique-transaction-id",
            Status: PaymentStatus.Authorized,
            LastFourDigits: "3456",
            ExpiryMonth: 12,
            ExpiryYear: 2023,
            Currency: "USD",
            Amount: 100
        );

        _paymentServiceMock.Setup(service => service.ProcessPaymentAsync(It.IsAny<PaymentRequest>()))
            .ReturnsAsync(expectedPaymentResponse);

        // Act
        var actionResult = await _controller.ProcessPayment(paymentRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(actionResult);
        var paymentResponse = Assert.IsType<PaymentResponse>(okResult.Value);
        Assert.Equal(paymentResponse, expectedPaymentResponse);
    }
}