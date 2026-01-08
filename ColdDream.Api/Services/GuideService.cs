using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Services;

public interface IGuideService
{
    Task<Guide> CreateAsync(Guide guide);
    Task<Guide?> GetByIdAsync(Guid id);
    Task<IEnumerable<Guide>> GetUserGuidesAsync(Guid userId);
    Task<IEnumerable<Guide>> GetAllGuidesAsync();
    Task<bool> UpdateAsync(Guide guide);
    Task<bool> DeleteAsync(Guid id, Guid userId);
}

public class GuideService : IGuideService
{
    private readonly AppDbContext _context;

    public GuideService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Guide> CreateAsync(Guide guide)
    {
        _context.Guides.Add(guide);
        
        // Add points to user
        var user = await _context.Users.FindAsync(guide.UserId);
        if (user != null)
        {
            user.Points += 10; // Reward 10 points per guide
        }

        await _context.SaveChangesAsync();
        return guide;
    }

    public async Task<Guide?> GetByIdAsync(Guid id)
    {
        return await _context.Guides.FindAsync(id);
    }

    public async Task<IEnumerable<Guide>> GetUserGuidesAsync(Guid userId)
    {
        return await _context.Guides
            .Where(g => g.UserId == userId)
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Guide>> GetAllGuidesAsync()
    {
        return await _context.Guides
            .Where(g => g.Status == 1) // Only Published
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    public async Task<bool> UpdateAsync(Guide guide)
    {
        var existing = await _context.Guides.FindAsync(guide.Id);
        if (existing == null) return false;

        existing.Title = guide.Title;
        existing.Description = guide.Description;
        existing.ImageUrl = guide.ImageUrl;
        existing.Itinerary = guide.Itinerary;
        existing.Tags = guide.Tags;
        existing.Duration = guide.Duration;
        existing.Budget = guide.Budget;
        // Reset status to pending on update? Or keep as is? Let's keep as is for now or reset if critical fields change.
        // For simplicity, let's say updates don't reset status in this demo, or maybe they do.
        // User said "my approved guides", so maybe updates should be re-approved.
        // Let's keep it simple: Status is managed by admin (not implemented), but for demo, let's auto-approve or keep pending.
        // Actually, let's make new guides Pending, and maybe have a "Publish" button?
        // For now, let's just update fields.
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId)
    {
        var guide = await _context.Guides.FindAsync(id);
        if (guide == null || guide.UserId != userId) return false;

        _context.Guides.Remove(guide);
        await _context.SaveChangesAsync();
        return true;
    }
}
