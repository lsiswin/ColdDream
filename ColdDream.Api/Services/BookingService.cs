using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Services;

public interface IBookingService
{
    Task<Booking> CreateBookingAsync(Guid userId, Booking booking);
    Task<IEnumerable<Booking>> GetUserBookingsAsync(Guid userId);
    Task<Booking?> GetBookingByIdAsync(Guid id);
    Task<bool> CancelBookingAsync(Guid id, Guid userId);
    Task<bool> DeleteBookingAsync(Guid id, Guid userId);
}

public class BookingService : IBookingService
{
    private readonly AppDbContext _context;

    public BookingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Booking> CreateBookingAsync(Guid userId, Booking booking)
    {
        booking.UserId = userId;
        booking.Status = "Confirmed"; // Auto-confirm since we skip payment
        booking.CreatedAt = DateTime.UtcNow;
        
        // Calculate price
        decimal routePrice = 0;
        decimal butlerPrice = 0;

        var route = await _context.TourRoutes.FindAsync(booking.TourRouteId);
        if (route != null)
        {
            routePrice = route.Price;
            route.Sales += booking.Travelers;
        }

        if (booking.ButlerId.HasValue)
        {
            var butler = await _context.Butlers.FindAsync(booking.ButlerId.Value);
            if (butler != null)
            {
                butlerPrice = butler.Price;
            }
        }

        // Apply Coupon
        decimal discount = 0;
        if (booking.CouponId.HasValue)
        {
            var coupon = await _context.Coupons.FindAsync(booking.CouponId.Value);
            if (coupon != null && !coupon.IsUsed && coupon.ExpiryDate > DateTime.UtcNow && coupon.UserId == userId)
            {
                // Discount applies ONLY to route price (routePrice * travelers)
                decimal routeTotal = routePrice * booking.Travelers;
                if (routeTotal >= coupon.MinSpend)
                {
                    discount = Math.Min(coupon.Amount, routeTotal);
                    booking.DiscountAmount = discount;
                    
                    // Mark coupon as used
                    coupon.IsUsed = true;
                    _context.Coupons.Update(coupon);
                }
            }
        }

        booking.TotalPrice = (routePrice * booking.Travelers) + butlerPrice - discount;

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<IEnumerable<Booking>> GetUserBookingsAsync(Guid userId)
    {
        return await _context.Bookings
            .Include(b => b.TourRoute)
            .Include(b => b.Butler)
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid id)
    {
        return await _context.Bookings
            .Include(b => b.TourRoute)
            .Include(b => b.Butler)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> CancelBookingAsync(Guid id, Guid userId)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null || booking.UserId != userId)
        {
            return false;
        }

        if (booking.Status == "Cancelled" || booking.Status == "Completed")
        {
            return false;
        }

        booking.Status = "Cancelled";
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteBookingAsync(Guid id, Guid userId)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null || booking.UserId != userId)
        {
            return false;
        }

        // Only allow deleting cancelled or completed bookings
        if (booking.Status != "Cancelled" && booking.Status != "Completed")
        {
            return false;
        }

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }
}
