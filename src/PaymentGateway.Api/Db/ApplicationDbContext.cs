using PaymentGateway.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Api.Services.Db;

public class ApplicationDbContext : DbContext
{
    public DbSet<PaymentDetails> PaymentDetails { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}