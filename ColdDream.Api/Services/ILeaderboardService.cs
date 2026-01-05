using ColdDream.Api.Models;

namespace ColdDream.Api.Services;

public interface ILeaderboardService
{
    Task IncrementRouteClickAsync(Guid routeId);
    Task<IEnumerable<TourRoute>> GetTopRoutesAsync(int count);
}
