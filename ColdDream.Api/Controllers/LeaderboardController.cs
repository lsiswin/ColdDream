using ColdDream.Api.Services;
using ColdDream.Api.Models;
using ColdDream.Api.DTOs;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;
    private readonly IMapper _mapper;

    public LeaderboardController(ILeaderboardService leaderboardService, IMapper mapper)
    {
        _leaderboardService = leaderboardService;
        _mapper = mapper;
    }

    [HttpGet("top-routes")]
    public async Task<ApiResponse<IEnumerable<TourRouteDto>>> GetTopRoutes([FromQuery] int count = 10)
    {
        var routes = await _leaderboardService.GetTopRoutesAsync(count);
        var routeDtos = _mapper.Map<IEnumerable<TourRouteDto>>(routes);
        return ApiResponse<IEnumerable<TourRouteDto>>.Ok(routeDtos);
    }

    [HttpPost("click/{routeId}")]
    public async Task<ApiResponse<object>> RecordClick(Guid routeId)
    {
        await _leaderboardService.IncrementRouteClickAsync(routeId);
        return ApiResponse<object>.Ok(null);
    }
}
