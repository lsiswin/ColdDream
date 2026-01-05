using ColdDream.Api.Data;
using ColdDream.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ColdDream.Api.Services;

public class ButlerService : IButlerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _context; // Need direct context for many-to-many includes if repo doesn't support it easily

    public ButlerService(IUnitOfWork unitOfWork, AppDbContext context)
    {
        _unitOfWork = unitOfWork;
        _context = context;
    }

    public async Task<IEnumerable<Butler>> GetAllButlersAsync()
    {
        return await _unitOfWork.Repository<Butler>().GetAllAsync();
    }

    public async Task<Butler?> GetButlerByIdAsync(Guid id)
    {
        return await _unitOfWork.Repository<Butler>().GetByIdAsync(id);
    }

    public async Task<Butler> CreateButlerAsync(Butler butler)
    {
        await _unitOfWork.Repository<Butler>().AddAsync(butler);
        await _unitOfWork.CompleteAsync();
        return butler;
    }

    public async Task UpdateButlerAsync(Butler butler)
    {
        _unitOfWork.Repository<Butler>().Update(butler);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteButlerAsync(Guid id)
    {
        var butler = await _unitOfWork.Repository<Butler>().GetByIdAsync(id);
        if (butler != null)
        {
            _unitOfWork.Repository<Butler>().Remove(butler);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task<IEnumerable<Butler>> GetButlersByRouteIdAsync(Guid routeId)
    {
        // Using context directly to handle Include easily for Many-to-Many
        // Or extend Repository to support Include
        // For now, let's use context for this specific query
        var route = await _context.TourRoutes
            .Include(r => r.Butlers)
            .FirstOrDefaultAsync(r => r.Id == routeId);
            
        return route?.Butlers ?? Enumerable.Empty<Butler>();
    }

    public async Task AssignButlerToRouteAsync(Guid butlerId, Guid routeId)
    {
        var route = await _context.TourRoutes.Include(r => r.Butlers).FirstOrDefaultAsync(r => r.Id == routeId);
        var butler = await _context.Butlers.FindAsync(butlerId);

        if (route != null && butler != null)
        {
            if (!route.Butlers.Any(b => b.Id == butlerId))
            {
                route.Butlers.Add(butler);
                await _context.SaveChangesAsync();
            }
        }
    }
}
