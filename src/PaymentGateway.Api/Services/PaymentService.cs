using Microsoft.EntityFrameworkCore;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Dtos;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Db;

namespace PaymentGateway.Api.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentProviderClient _client;
    private readonly ApplicationDbContext _dbContext;
    
    public PaymentService(IPaymentProviderClient client, ApplicationDbContext dbContext)
    {
        _client = client;
        _dbContext = dbContext;
    }
    
    public async Task<PaymentResponse> ProcessPaymentAsync(PaymentRequest request)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));
        ArgumentNullException.ThrowIfNull(request.PaymentId, nameof(request.PaymentId));
        ArgumentNullException.ThrowIfNull(request.CardNumber, nameof(request.CardNumber));
        ArgumentNullException.ThrowIfNull(request.Currency, nameof(request.Currency));
        ArgumentNullException.ThrowIfNull(request.Cvv, nameof(request.Cvv));
        ArgumentNullException.ThrowIfNull(request.ExpiryYear, nameof(request.ExpiryYear));
        ArgumentNullException.ThrowIfNull(request.ExpiryMonth, nameof(request.ExpiryMonth));
        
        // save payment in db
        var paymentDetails = new PaymentDetails()
        {
            PaymentId = request.PaymentId,
            Status = PaymentStatus.Unknown,
            CardNumber = request.CardNumber,
            ExpiryMonth = request.ExpiryMonth,
            ExpiryYear = request.ExpiryYear,
            Currency = request.Currency,
            Amount = request.Amount,
            PaymentInitiationTime = DateTime.UtcNow
        };
        var savePaymentDetails = await SavePaymentDetailsAsync(paymentDetails);
        
        // format month, create the paymentRequest payment for Payment Provider & send the payment
        var expiryMonth = AddZeroPrefixIfSingleDigit(request.ExpiryMonth);

        var paymentProviderRequest = new PaymentProviderRequest()
        {
            CardNumber = request.CardNumber,
            ExpiryDate = $"{expiryMonth}/{request.ExpiryYear}",
            Amount = request.Amount,
            Currency = request.Currency,
            Cvv = request.Cvv,
        };
        
        var paymentProviderResponse = await _client.SendPayment(paymentProviderRequest);
        var paymentStatus = paymentProviderResponse.Authorized ? PaymentStatus.Authorized : PaymentStatus.Declined;
        paymentDetails.Status = paymentStatus;
        var updatedPaymentDetails = await UpdatePaymentDetailsAsync(paymentDetails);
        
        ArgumentNullException.ThrowIfNull(updatedPaymentDetails);
        
        // prepare response, add payment status to it and update in db
        var serviceResponse = new PaymentResponse(
            updatedPaymentDetails.PaymentId,
            updatedPaymentDetails.Status,
            new string(request.CardNumber.AsSpan()[^4..]),
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Currency,
            request.Amount
        );
        
        return serviceResponse;
    }

    private string AddZeroPrefixIfSingleDigit(uint value)
    {
        return value < 10 ? $"0{value}" : $"{value}";
    }

    public async Task<PaymentDetails?> SavePaymentDetailsAsync(PaymentDetails paymentDetails)
    {
        if (paymentDetails == null)
        {
            throw new ArgumentNullException(nameof(paymentDetails), "Payment details cannot be null.");
        }
        
        _dbContext.PaymentDetails.Add(paymentDetails);
        await _dbContext.SaveChangesAsync();
        
        var savedObject = await _dbContext.PaymentDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(pd => pd.PaymentId == paymentDetails.PaymentId);
        
        ArgumentNullException.ThrowIfNull(savedObject);
        return savedObject;
    }
    
    public async Task<PaymentDetails?> UpdatePaymentDetailsAsync(PaymentDetails paymentDetails)
    {
        if (paymentDetails == null)
        {
            throw new ArgumentNullException(nameof(paymentDetails), "Payment details cannot be null.");
        }
        
        _dbContext.PaymentDetails.Update(paymentDetails);
        await _dbContext.SaveChangesAsync();
        
        var savedObject = await _dbContext.PaymentDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(pd => pd.PaymentId == paymentDetails.PaymentId);
        
        ArgumentNullException.ThrowIfNull(savedObject);
        return savedObject;
    }

    public async Task<PaymentResponse> GetPaymentAsync(string paymentId)
    {
        var payment = await RetrievePaymentDetailsAsync(paymentId);
        ArgumentNullException.ThrowIfNull(payment);
        
        return new PaymentResponse(
            payment.PaymentId,
            payment.Status,
            new string(payment.CardNumber.AsSpan()[^4..]),
            payment.ExpiryMonth,
            payment.ExpiryYear,
            payment.Currency,
            payment.Amount
        );
    }

    public async Task<PaymentDetails?> RetrievePaymentDetailsAsync(string id)
    {
        var paymentDetails = await _dbContext.PaymentDetails
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PaymentId == id);
        return paymentDetails;
    }
}