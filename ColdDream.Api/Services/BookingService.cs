using ColdDream.Api.Data;
using ColdDream.Api.Models;

namespace ColdDream.Api.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Booking> CreateBookingAsync(Booking booking)
    {
        // Generate Order Number
        booking.OrderNumber = GenerateOrderNumber();
        booking.Status = "Pending";
        booking.CreatedAt = DateTime.UtcNow;

        // Calculate Total Amount (Basic logic for now)
        var route = await _unitOfWork.Repository<TourRoute>().GetByIdAsync(booking.TourRouteId);
        if (route != null)
        {
            decimal butlerPrice = 0;
            if (booking.ButlerId.HasValue)
            {
                var butler = await _unitOfWork.Repository<Butler>().GetByIdAsync(booking.ButlerId.Value);
                if (butler != null)
                {
                    butlerPrice = butler.Price;
                }
            }
            booking.TotalAmount = (route.Price * booking.PeopleCount) + butlerPrice;
        }

        await _unitOfWork.Repository<Booking>().AddAsync(booking);
        await _unitOfWork.CompleteAsync();
        return booking;
    }

    public async Task<Booking?> GetBookingByIdAsync(Guid id)
    {
        return await _unitOfWork.Repository<Booking>().GetByIdAsync(id);
    }

    public async Task<IEnumerable<Booking>> GetUserBookingsAsync(Guid userId)
    {
        return await _unitOfWork.Repository<Booking>().FindAsync(b => b.UserId == userId);
    }

    public async Task UpdateBookingAsync(Booking booking)
    {
        _unitOfWork.Repository<Booking>().Update(booking);
        await _unitOfWork.CompleteAsync();
    }

    public async Task CancelBookingAsync(Guid id)
    {
        var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(id);
        if (booking != null)
        {
            booking.Status = "Cancelled";
            _unitOfWork.Repository<Booking>().Update(booking);
            await _unitOfWork.CompleteAsync();
        }
    }

    private string GenerateOrderNumber()
    {
        return DateTime.Now.ToString("yyyyMMddHHmmss") + new Random().Next(1000, 9999);
    }
}
