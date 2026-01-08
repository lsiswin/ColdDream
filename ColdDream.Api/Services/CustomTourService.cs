using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Services;

public interface ICustomTourService
{
    Task<CustomTour> CreateAsync(CustomTour customTour);
    Task<IEnumerable<CustomTour>> GetUserCustomToursAsync(Guid userId);
    Task<CustomTour?> GetByIdAsync(Guid id);
    Task<bool> CancelAsync(Guid id, Guid userId);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}

public class CustomTourService : ICustomTourService
{
    private readonly AppDbContext _context;

    public CustomTourService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CustomTour> CreateAsync(CustomTour customTour)
    {
        customTour.CreatedAt = DateTime.UtcNow;
        customTour.Status = "Pending";
        
        _context.CustomTours.Add(customTour);
        await _context.SaveChangesAsync();
        return customTour;
    }

    public async Task<IEnumerable<CustomTour>> GetUserCustomToursAsync(Guid userId)
    {
        return await _context.CustomTours
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<CustomTour?> GetByIdAsync(Guid id)
    {
        return await _context.CustomTours.FindAsync(id);
    }

    public async Task<bool> CancelAsync(Guid id, Guid userId)
    {
        var tour = await _context.CustomTours.FindAsync(id);
        if (tour == null || tour.UserId != userId) return false;
        
        if (tour.Status == "Pending" || tour.Status == "Processing")
        {
            tour.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }
        return false;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var tour = await _context.CustomTours.FindAsync(id);
        if (tour == null || tour.UserId != userId) return false;

        _context.CustomTours.Remove(tour);
        await _context.SaveChangesAsync();
        return true;
    }
}
