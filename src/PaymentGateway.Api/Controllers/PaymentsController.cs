using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services;

namespace PaymentGateway.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    [HttpPost("/api/payment")]
    public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // Process the payment
            PaymentResponse paymentResponse = await _paymentService.ProcessPaymentAsync(request with
            {
                PaymentId = Guid.NewGuid().ToString(),
            });
            return Ok(paymentResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }

    [HttpGet("/api/payment/{id}")]
    public async Task<IActionResult> GetPaymentDetails(string id)
    {
        try
        {
            var paymentDetails = await _paymentService.GetPaymentAsync(id);
            return Ok(paymentDetails);
        }
        catch (Exception ex)
        {
            // Log the exception (if logging is configured)
            return StatusCode(500, "An error occurred while processing your request. Please try again later.");
        }
    }
}