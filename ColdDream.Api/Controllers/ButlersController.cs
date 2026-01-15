using Microsoft.AspNetCore.Mvc;
using ColdDream.Api.Data;
using ColdDream.Api.Models;
using ColdDream.Api.DTOs;
using AutoMapper;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ButlersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ButlersController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet("route/{routeId}")]
    public ApiResponse<IEnumerable<ButlerDto>> GetButlersByRoute(Guid routeId)
    {
        // For simplicity, return all butlers for any route
        // In a real app, we would filter by route or location
        var butlers = _context.Butlers.ToList();
        var butlerDtos = _mapper.Map<IEnumerable<ButlerDto>>(butlers);
        return ApiResponse<IEnumerable<ButlerDto>>.Ok(butlerDtos);
    }
}
