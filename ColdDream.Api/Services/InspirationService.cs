using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Services;

public interface IInspirationService
{
    Task<IEnumerable<Inspiration>> GetInspirationsAsync(Guid? userId = null);
    Task<Inspiration?> ToggleLikeAsync(Guid id, Guid userId);
    Task<Inspiration?> ToggleCollectAsync(Guid id, Guid userId);
    Task<IEnumerable<Inspiration>> GetCollectedInspirationsAsync(Guid userId);
    Task<Inspiration> CreateAsync(Guid userId, string description, string imageUrl);
}

public class InspirationService : IInspirationService
{
    private readonly AppDbContext _context;

    public InspirationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Inspiration> CreateAsync(Guid userId, string description, string imageUrl)
    {
        var inspiration = new Inspiration
        {
            UserId = userId,
            Description = description,
            ImageUrl = imageUrl,
            CreatedAt = DateTime.UtcNow
        };

        _context.Inspirations.Add(inspiration);
        await _context.SaveChangesAsync();
        return inspiration;
    }

    public async Task<IEnumerable<Inspiration>> GetInspirationsAsync(Guid? userId = null)
    {
        var inspirations = await _context.Inspirations.ToListAsync();
        
        if (userId.HasValue)
        {
            var likedIds = await _context.InspirationLikes
                .Where(l => l.UserId == userId.Value)
                .Select(l => l.InspirationId)
                .ToListAsync();
                
            var collectedIds = await _context.InspirationCollects
                .Where(c => c.UserId == userId.Value)
                .Select(c => c.InspirationId)
                .ToListAsync();
                
            foreach (var ins in inspirations)
            {
                ins.IsLiked = likedIds.Contains(ins.Id);
                ins.IsCollected = collectedIds.Contains(ins.Id);
            }
        }
        
        return inspirations;
    }

    public async Task<Inspiration?> ToggleLikeAsync(Guid id, Guid userId)
    {
        var inspiration = await _context.Inspirations.FindAsync(id);
        if (inspiration == null) return null;

        var existingLike = await _context.InspirationLikes
            .FirstOrDefaultAsync(l => l.InspirationId == id && l.UserId == userId);

        if (existingLike != null)
        {
            _context.InspirationLikes.Remove(existingLike);
            inspiration.Likes = Math.Max(0, inspiration.Likes - 1);
            inspiration.IsLiked = false;
        }
        else
        {
            _context.InspirationLikes.Add(new InspirationLike { InspirationId = id, UserId = userId });
            inspiration.Likes++;
            inspiration.IsLiked = true;
        }
        
        await _context.SaveChangesAsync();
        return inspiration;
    }

    public async Task<Inspiration?> ToggleCollectAsync(Guid id, Guid userId)
    {
        var inspiration = await _context.Inspirations.FindAsync(id);
        if (inspiration == null) return null;

        var existingCollect = await _context.InspirationCollects
            .FirstOrDefaultAsync(c => c.InspirationId == id && c.UserId == userId);

        if (existingCollect != null)
        {
            _context.InspirationCollects.Remove(existingCollect);
            inspiration.Collects = Math.Max(0, inspiration.Collects - 1);
            inspiration.IsCollected = false;
        }
        else
        {
            _context.InspirationCollects.Add(new InspirationCollect { InspirationId = id, UserId = userId });
            inspiration.Collects++;
            inspiration.IsCollected = true;
        }
        
        await _context.SaveChangesAsync();
        return inspiration;
    }

    public async Task<IEnumerable<Inspiration>> GetCollectedInspirationsAsync(Guid userId)
    {
        var collectedIds = await _context.InspirationCollects
            .Where(c => c.UserId == userId)
            .Select(c => c.InspirationId)
            .ToListAsync();
            
        var inspirations = await _context.Inspirations
            .Where(i => collectedIds.Contains(i.Id))
            .ToListAsync();

        foreach (var ins in inspirations)
        {
            ins.IsCollected = true;
            // Check if liked as well
            ins.IsLiked = await _context.InspirationLikes.AnyAsync(l => l.InspirationId == ins.Id && l.UserId == userId);
        }
            
        return inspirations;
    }
}
