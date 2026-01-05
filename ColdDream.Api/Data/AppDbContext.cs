using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ColdDream.Api.Models;

namespace ColdDream.Api.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Users is inherited from IdentityDbContext
    public DbSet<TourRoute> TourRoutes { get; set; }
    public DbSet<Butler> Butlers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Booking>()
            .Property(b => b.TotalAmount)
            .HasColumnType("decimal(18,2)");
            
        modelBuilder.Entity<TourRoute>()
            .Property(r => r.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Butler>()
            .Property(b => b.Price)
            .HasColumnType("decimal(18,2)");
    }
}
