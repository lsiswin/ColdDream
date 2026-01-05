using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ButlersController : ControllerBase
{
    private readonly IButlerService _butlerService;

    public ButlersController(IButlerService butlerService)
    {
        _butlerService = butlerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllButlers()
    {
        var butlers = await _butlerService.GetAllButlersAsync();
        return Ok(butlers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetButler(Guid id)
    {
        var butler = await _butlerService.GetButlerByIdAsync(id);
        if (butler == null) return NotFound();
        return Ok(butler);
    }

    [HttpPost]
    [Authorize] // Admin only ideally
    public async Task<IActionResult> CreateButler(Butler butler)
    {
        var createdButler = await _butlerService.CreateButlerAsync(butler);
        return CreatedAtAction(nameof(GetButler), new { id = createdButler.Id }, createdButler);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateButler(Guid id, Butler butler)
    {
        if (id != butler.Id) return BadRequest();
        await _butlerService.UpdateButlerAsync(butler);
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteButler(Guid id)
    {
        await _butlerService.DeleteButlerAsync(id);
        return NoContent();
    }

    [HttpGet("route/{routeId}")]
    public async Task<IActionResult> GetButlersForRoute(Guid routeId)
    {
        var butlers = await _butlerService.GetButlersByRouteIdAsync(routeId);
        return Ok(butlers);
    }

    [HttpPost("{butlerId}/assign/{routeId}")]
    [Authorize]
    public async Task<IActionResult> AssignButlerToRoute(Guid butlerId, Guid routeId)
    {
        await _butlerService.AssignButlerToRouteAsync(butlerId, routeId);
        return Ok();
    }
}
