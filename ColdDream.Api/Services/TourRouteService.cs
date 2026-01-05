using ColdDream.Api.Data;
using ColdDream.Api.Models;

namespace ColdDream.Api.Services;

public class TourRouteService : ITourRouteService
{
    private readonly IUnitOfWork _unitOfWork;

    public TourRouteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TourRoute>> GetPublicRoutesAsync()
    {
        // Only return non-private routes, or maybe all routes but mark private ones?
        // Requirement: "User without login can browse tour route details, if need to reserve or view some private routes need login"
        // So maybe list all, but details are restricted?
        // "User without login can browse tour route details" -> This implies details are public mostly.
        // "view some private routes need login" -> Some routes are private.
        
        return await _unitOfWork.Repository<TourRoute>()
            .FindAsync(r => !r.IsPrivate);
    }

    public async Task<TourRoute?> GetRouteDetailsAsync(Guid id, bool isAuthenticated)
    {
        var route = await _unitOfWork.Repository<TourRoute>().GetByIdAsync(id);
        if (route == null) return null;

        if (route.IsPrivate && !isAuthenticated)
        {
            return null; // Or throw exception
        }

        return route;
    }

    public async Task<TourRoute> CreateRouteAsync(TourRoute route)
    {
        await _unitOfWork.Repository<TourRoute>().AddAsync(route);
        await _unitOfWork.CompleteAsync();
        return route;
    }
}
