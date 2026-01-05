using ColdDream.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;

    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpGet("top-routes")]
    public async Task<IActionResult> GetTopRoutes([FromQuery] int count = 10)
    {
        var routes = await _leaderboardService.GetTopRoutesAsync(count);
        return Ok(routes);
    }

    [HttpPost("click/{routeId}")]
    public async Task<IActionResult> RecordClick(Guid routeId)
    {
        await _leaderboardService.IncrementRouteClickAsync(routeId);
        return Ok();
    }
}
