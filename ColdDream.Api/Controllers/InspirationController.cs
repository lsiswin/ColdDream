using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InspirationController : ControllerBase
{
    private readonly IInspirationService _inspirationService;

    public InspirationController(IInspirationService inspirationService)
    {
        _inspirationService = inspirationService;
    }

    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        Guid? userId = null;
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim != null)
        {
            userId = Guid.Parse(userIdClaim);
        }

        var list = await _inspirationService.GetInspirationsAsync(userId);
        return Ok(list.OrderByDescending(x => x.CreatedAt));
    }

    [HttpGet("collected")]
    [Authorize]
    public async Task<IActionResult> GetCollected()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var list = await _inspirationService.GetCollectedInspirationsAsync(Guid.Parse(userId));
        return Ok(list.OrderByDescending(x => x.CreatedAt));
    }

    [HttpPost("{id}/like")]
    [Authorize]
    public async Task<IActionResult> Like(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _inspirationService.ToggleLikeAsync(id, Guid.Parse(userId));
        if (result == null) return NotFound();
        
        return Ok(new { likes = result.Likes, isLiked = result.IsLiked });
    }

    [HttpPost("{id}/collect")]
    [Authorize]
    public async Task<IActionResult> Collect(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _inspirationService.ToggleCollectAsync(id, Guid.Parse(userId));
        if (result == null) return NotFound();

        return Ok(new { collects = result.Collects, isCollected = result.IsCollected });
    }
}
