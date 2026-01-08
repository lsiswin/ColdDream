using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Services;

public interface ICouponService
{
    Task<IEnumerable<Coupon>> GetMyCouponsAsync(Guid userId);
    Task<Coupon?> GetValidCouponAsync(Guid couponId, Guid userId);
    Task MarkAsUsedAsync(Guid couponId);
    Task CreateCouponAsync(Guid userId, decimal amount, string name);
}

public class CouponService : ICouponService
{
    private readonly AppDbContext _context;

    public CouponService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Coupon>> GetMyCouponsAsync(Guid userId)
    {
        return await _context
            .Coupons.Where(c => c.UserId == userId && !c.IsUsed && c.ExpiryDate > DateTime.UtcNow)
            .OrderBy(c => c.ExpiryDate)
            .ToListAsync();
    }

    public async Task<Coupon?> GetValidCouponAsync(Guid couponId, Guid userId)
    {
        return await _context.Coupons.FirstOrDefaultAsync(c =>
            c.Id == couponId && c.UserId == userId && !c.IsUsed && c.ExpiryDate > DateTime.UtcNow
        );
    }

    public async Task MarkAsUsedAsync(Guid couponId)
    {
        var coupon = await _context.Coupons.FindAsync(couponId);
        if (coupon != null)
        {
            coupon.IsUsed = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task CreateCouponAsync(Guid userId, decimal amount, string name)
    {
        var coupon = new Coupon
        {
            UserId = userId,
            Amount = amount,
            Name = name,
            ExpiryDate = DateTime.UtcNow.AddDays(30), // Default 30 days validity
        };
        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();
    }
}
