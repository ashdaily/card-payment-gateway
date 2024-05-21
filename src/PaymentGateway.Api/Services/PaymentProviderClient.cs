using System.Text;
using System.Text.Json;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Dtos;

namespace PaymentGateway.Api.Services;

public class PaymentProviderClient : IPaymentProviderClient
{
    private readonly HttpClient _httpClient;

    public PaymentProviderClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<PaymentProviderResponse> SendPayment(PaymentProviderRequest paymentRequest)
    {
        var url = "http://localhost:8080/payments";
        
        try
        {
            var jsonRequest = JsonSerializer.Serialize(paymentRequest);
            var content = new StringContent(jsonRequest, Encoding.UTF8);

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                try
                {
                    return JsonSerializer.Deserialize<PaymentProviderResponse>(jsonResponse)
                           ?? throw new JsonException("Failed to deserialize the payment provider response.");
                }
                catch (JsonException)
                {
                    // JSON parsing errors
                    throw new Exception("Error parsing the JSON response from the server.");
                }
            }
            else
            {
                // non-success status
                var errorResponse = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"HTTP error {response.StatusCode}: {errorResponse}");
            }
        }
        catch (HttpRequestException ex)
        {
            // network errors
            throw new Exception($"Network error occurred: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            // timeout
            throw new Exception("Request timed out.", ex);
        }
        catch (Exception ex)
        {
            // General exceptions
            throw new Exception($"An error occurred: {ex.Message}");
        }
    }
}