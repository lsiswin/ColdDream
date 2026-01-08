using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomTourController : ControllerBase
{
    private readonly ICustomTourService _customTourService;

    public CustomTourController(ICustomTourService customTourService)
    {
        _customTourService = customTourService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomTour customTour)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        customTour.UserId = Guid.Parse(userId);
        var result = await _customTourService.CreateAsync(customTour);
        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyCustomTours()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _customTourService.GetUserCustomToursAsync(Guid.Parse(userId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _customTourService.GetByIdAsync(id);
        if (result == null) return NotFound();
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (result.UserId.ToString() != userId) return Forbid();

        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var success = await _customTourService.CancelAsync(id, Guid.Parse(userId));
        if (!success) return BadRequest("Cannot cancel this tour request.");
        
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var success = await _customTourService.DeleteAsync(id, Guid.Parse(userId));
        if (!success) return NotFound();

        return Ok();
    }
}
