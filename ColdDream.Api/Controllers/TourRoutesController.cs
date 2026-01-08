using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TourRoutesController : ControllerBase
{
    private readonly ITourRouteService _tourRouteService;

    public TourRoutesController(ITourRouteService tourRouteService)
    {
        _tourRouteService = tourRouteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetRoutes()
    {
        // For now, return public routes. 
        // In real app, might want to return all but hide details for private ones.
        var routes = await _tourRouteService.GetPublicRoutesAsync();
        return Ok(routes);
    }

    [HttpGet("top")]
    public async Task<IActionResult> GetTopRoutes([FromQuery] int count = 10)
    {
        var routes = await _tourRouteService.GetTopSellingRoutesAsync(count);
        return Ok(routes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRoute(Guid id)
    {
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        var route = await _tourRouteService.GetRouteDetailsAsync(id, isAuthenticated);

        if (route == null)
        {
            // Distinguish between not found and forbidden?
            // For security, maybe just 404 or 403.
            return NotFound("Route not found or you do not have permission to view it.");
        }

        return Ok(route);
    }

    [HttpPost]
    [Authorize] // Only logged in users can create (or Admin)
    public async Task<IActionResult> CreateRoute(TourRoute route)
    {
        var createdRoute = await _tourRouteService.CreateRouteAsync(route);
        return CreatedAtAction(nameof(GetRoute), new { id = createdRoute.Id }, createdRoute);
    }
}
