using ColdDream.Api.Models;

namespace ColdDream.Api.Services;

public interface IBookingService
{
    Task<Booking> CreateBookingAsync(Booking booking);
    Task<Booking?> GetBookingByIdAsync(Guid id);
    Task<IEnumerable<Booking>> GetUserBookingsAsync(Guid userId);
    Task UpdateBookingAsync(Booking booking);
    Task CancelBookingAsync(Guid id);
}
