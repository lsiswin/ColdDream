using ColdDream.Api.Models;
using ColdDream.Api.Services;
using ColdDream.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

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
        return ApiResponse<IEnumerable<TourRouteDto>>.Ok(_mapper.Map<IEnumerable<TourRouteDto>>(routes));
    }

    [HttpGet("top")]
    public async Task<ApiResponse<IEnumerable<TourRouteDto>>> GetTopRoutes([FromQuery] int count = 10)
    {
        var routes = await _tourRouteService.GetTopSellingRoutesAsync(count);
        return ApiResponse<IEnumerable<TourRouteDto>>.Ok(_mapper.Map<IEnumerable<TourRouteDto>>(routes));
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
            return ApiResponse<TourRouteDto>.Fail("Route not found or you do not have permission to view it.");
        }

        return ApiResponse<TourRouteDto>.Ok(_mapper.Map<TourRouteDto>(route));
    }

    [HttpPost]
    [Authorize] // Only logged in users can create (or Admin)
    public async Task<ApiResponse<TourRouteDto>> CreateRoute(CreateTourRouteDto routeDto)
    {
        var route = _mapper.Map<TourRoute>(routeDto);
        var createdRoute = await _tourRouteService.CreateRouteAsync(route);
        return ApiResponse<TourRouteDto>.Ok(_mapper.Map<TourRouteDto>(createdRoute));
    }
}
