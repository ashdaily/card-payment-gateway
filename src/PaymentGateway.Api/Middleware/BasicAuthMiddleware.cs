namespace PaymentGateway.Api.Middleware;

using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private const string AuthorizationHeader = "Authorization";
    private const string BasicScheme = "Basic";

    public BasicAuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(AuthorizationHeader, out var authHeader))
        {
            context.Response.StatusCode = 401; // Unauthorized
            return;
        }

        var authHeaderValue = AuthenticationHeaderValue.Parse(authHeader);
        if (authHeaderValue.Scheme != BasicScheme)
        {
            context.Response.StatusCode = 401; // Unauthorized
            return;
        }

        var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty)).Split(':');
        if (credentials.Length != 2 || !IsUserValid(credentials[0], credentials[1]))
        {
            context.Response.StatusCode = 401; // Unauthorized
            return;
        }

        await _next(context);
    }

    private bool IsUserValid(string username, string password)
    {
        // Ideally, we need retrieve this from a secure place and consider hashing the password.
        return username == "admin" && password == "password";
    }
}
