using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Api.Services;
using PaymentGateway.Api.Services.Db;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddHttpClient(nameof(IPaymentProviderClient), client =>
        {
            client.BaseAddress = new Uri("http://localhost:8080");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        builder.Services.AddScoped<IPaymentService, PaymentService>();
        builder.Services.AddScoped<IPaymentProviderClient, PaymentProviderClient>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}