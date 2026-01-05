using ColdDream.Api.Models;

namespace ColdDream.Api.Services;

public interface ITourRouteService
{
    Task<IEnumerable<TourRoute>> GetPublicRoutesAsync();
    Task<TourRoute?> GetRouteDetailsAsync(Guid id, bool isAuthenticated);
    Task<TourRoute> CreateRouteAsync(TourRoute route);
}
