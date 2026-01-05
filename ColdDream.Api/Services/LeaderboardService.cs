using ColdDream.Api.Data;
using ColdDream.Api.Models;
using StackExchange.Redis;

namespace ColdDream.Api.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IUnitOfWork _unitOfWork;
    private const string RouteClicksKey = "route:clicks";

    public LeaderboardService(IConnectionMultiplexer redis, IUnitOfWork unitOfWork)
    {
        _redis = redis;
        _unitOfWork = unitOfWork;
    }

    public async Task IncrementRouteClickAsync(Guid routeId)
    {
        var db = _redis.GetDatabase();
        await db.SortedSetIncrementAsync(RouteClicksKey, routeId.ToString(), 1);
    }

    public async Task<IEnumerable<TourRoute>> GetTopRoutesAsync(int count)
    {
        var db = _redis.GetDatabase();
        var topIds = await db.SortedSetRangeByRankAsync(RouteClicksKey, 0, count - 1, Order.Descending);

        var routes = new List<TourRoute>();
        foreach (var idStr in topIds)
        {
            if (Guid.TryParse(idStr, out Guid id))
            {
                var route = await _unitOfWork.Repository<TourRoute>().GetByIdAsync(id);
                if (route != null)
                {
                    routes.Add(route);
                }
            }
        }
        return routes;
    }
}
