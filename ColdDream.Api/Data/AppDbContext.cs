using ColdDream.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Data;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    // Users is inherited from IdentityDbContext
    public DbSet<TourRoute> TourRoutes { get; set; }
    public DbSet<Butler> Butlers { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<CustomTourRequest> CustomTourRequests { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<Inspiration> Inspirations { get; set; }
    public DbSet<InspirationLike> InspirationLikes { get; set; }
    public DbSet<InspirationCollect> InspirationCollects { get; set; }
    public DbSet<CustomTour> CustomTours { get; set; }
    public DbSet<Guide> Guides { get; set; }
    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Booking>().Property(b => b.TotalPrice).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<TourRoute>().Property(r => r.Price).HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Butler>().Property(b => b.Price).HasColumnType("decimal(18,2)");
    }
}
