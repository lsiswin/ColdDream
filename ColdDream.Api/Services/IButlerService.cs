using ColdDream.Api.Models;

namespace ColdDream.Api.Services;

public interface IButlerService
{
    Task<IEnumerable<Butler>> GetAllButlersAsync();
    Task<Butler?> GetButlerByIdAsync(Guid id);
    Task<Butler> CreateButlerAsync(Butler butler);
    Task UpdateButlerAsync(Butler butler);
    Task DeleteButlerAsync(Guid id);
    Task<IEnumerable<Butler>> GetButlersByRouteIdAsync(Guid routeId);
    Task AssignButlerToRouteAsync(Guid butlerId, Guid routeId);
}
