using Microsoft.AspNetCore.Mvc;
using ColdDream.Api.Data;
using ColdDream.Api.Models;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ButlersController : ControllerBase
{
    private readonly AppDbContext _context;

    public ButlersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("route/{routeId}")]
    public IActionResult GetButlersByRoute(Guid routeId)
    {
        // For simplicity, return all butlers for any route
        // In a real app, we would filter by route or location
        var butlers = _context.Butlers.ToList();
        return Ok(butlers);
    }
}
