using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Services.Db;

public abstract class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestDatabase");
            });

            // Build the service provider
            var sp = services.BuildServiceProvider();

            // Create a scope to obtain a reference to the database context
            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();

                // Seed the database with test data
                InitializeDbForTests(db);
            }
        });
    }
    
    private void InitializeDbForTests(ApplicationDbContext db)
    {
        // Add entities to the database context and save them
        db.PaymentDetails.Add(new PaymentDetails(){
            PaymentId = "123", 
            Status = PaymentStatus.Authorized,
            CardNumber= "2222405343248877",
            ExpiryMonth = 4,
            ExpiryYear = 2025,
            Currency = CurrencyCode.GBP.ToString("G"),
            Amount = 100,
            PaymentInitiationTime = DateTime.UtcNow});
        db.SaveChanges();
    }
}