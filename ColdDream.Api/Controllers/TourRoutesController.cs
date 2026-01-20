using AutoMapper;
using ColdDream.Api.DTOs;
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
    private readonly IMapper _mapper;

    public TourRoutesController(ITourRouteService tourRouteService, IMapper mapper)
    {
        _tourRouteService = tourRouteService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<TourRouteDto>>> GetRoutes()
    {
        // For now, return public routes.
        // In real app, might want to return all but hide details for private ones.
        var routes = await _tourRouteService.GetPublicRoutesAsync();
        return ApiResponse<IEnumerable<TourRouteDto>>.Ok(
            _mapper.Map<IEnumerable<TourRouteDto>>(routes)
        );
    }

    [HttpGet("top")]
    public async Task<ApiResponse<IEnumerable<TourRouteDto>>> GetTopRoutes(
        [FromQuery] int count = 10
    )
    {
        var routes = await _tourRouteService.GetTopSellingRoutesAsync(count);
        return ApiResponse<IEnumerable<TourRouteDto>>.Ok(
            _mapper.Map<IEnumerable<TourRouteDto>>(routes)
        );
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<TourRouteDto>> GetRoute(Guid id)
    {
        var isAuthenticated = User.Identity?.IsAuthenticated ?? false;
        var route = await _tourRouteService.GetRouteDetailsAsync(id, isAuthenticated);

        if (route == null)
        {
            // Distinguish between not found and forbidden?
            // For security, maybe just 404 or 403.
            return ApiResponse<TourRouteDto>.Fail(
                "Route not found or you do not have permission to view it."
            );
        }

        return ApiResponse<TourRouteDto>.Ok(_mapper.Map<TourRouteDto>(route));
    }

    [HttpPost]
    [Authorize] // Only logged in users can create (or Admin)
    public async Task<ApiResponse<TourRouteDto>> CreateRoute(CreateTourRouteDto routeDto)
    {
        var route = _mapper.Map<TourRoute>(routeDto);

        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
        {
            route.CreatorId = Guid.Parse(userId);
        }

        var createdRoute = await _tourRouteService.CreateRouteAsync(route);
        return ApiResponse<TourRouteDto>.Ok(_mapper.Map<TourRouteDto>(createdRoute));
    }

    [HttpGet("my")]
    [Authorize]
    public async Task<ApiResponse<IEnumerable<TourRouteDto>>> GetMyRoutes()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return ApiResponse<IEnumerable<TourRouteDto>>.Fail("Unauthorized");

        var routes = await _tourRouteService.GetRoutesByCreatorAsync(Guid.Parse(userId));
        return ApiResponse<IEnumerable<TourRouteDto>>.Ok(
            _mapper.Map<IEnumerable<TourRouteDto>>(routes)
        );
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ApiResponse<TourRouteDto>> UpdateRoute(Guid id, TourRouteDto routeDto)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return ApiResponse<TourRouteDto>.Fail("Unauthorized");

        // Verify ownership
        // Since Service doesn't support GetById with owner check directly,
        // and GetRouteDetailsAsync handles IsPrivate but not Creator check for updates.
        // We should fetch first.
        var existingRoute = await _tourRouteService.GetRouteDetailsAsync(id, true);
        if (existingRoute == null)
            return ApiResponse<TourRouteDto>.Fail("Route not found");

        // Check if current user is the creator
        if (existingRoute.CreatorId != Guid.Parse(userId))
        {
            return ApiResponse<TourRouteDto>.Fail("Forbidden: You can only edit your own routes");
        }

        // Map updates
        // We only allow updating certain fields
        existingRoute.Title = routeDto.Title;
        existingRoute.Description = routeDto.Description;
        existingRoute.Price = routeDto.Price;
        existingRoute.ImageUrl = routeDto.ImageUrl;
        existingRoute.Itinerary = routeDto.Itinerary;
        existingRoute.Tags = routeDto.Tags;
        existingRoute.Duration = routeDto.Duration;
        existingRoute.IsPrivate = routeDto.IsPrivate;
        existingRoute.UpdatedAt = DateTime.UtcNow;

        var updatedRoute = await _tourRouteService.UpdateRouteAsync(existingRoute);
        return ApiResponse<TourRouteDto>.Ok(_mapper.Map<TourRouteDto>(updatedRoute));
    }
}
