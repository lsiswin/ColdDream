using System.Security.Claims;
using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ColdDream.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuideController : ControllerBase
{
    private readonly IGuideService _guideService;

    public GuideController(IGuideService guideService)
    {
        _guideService = guideService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Guide>>> GetAll()
    {
        return Ok(await _guideService.GetAllGuidesAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Guide>> GetById(Guid id)
    {
        var guide = await _guideService.GetByIdAsync(id);
        if (guide == null) return NotFound();
        return Ok(guide);
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<Guide>>> GetMyGuides()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        return Ok(await _guideService.GetUserGuidesAsync(Guid.Parse(userId)));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Guide>> Create(Guide guide)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        guide.UserId = Guid.Parse(userId);
        var result = await _guideService.CreateAsync(guide);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Guide guide)
    {
        if (id != guide.Id) return BadRequest();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        // Verify ownership
        var existing = await _guideService.GetByIdAsync(id);
        if (existing == null) return NotFound();
        if (existing.UserId != Guid.Parse(userId)) return Forbid();

        await _guideService.UpdateAsync(guide);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var success = await _guideService.DeleteAsync(id, Guid.Parse(userId));
        if (!success) return NotFound();

        return NoContent();
    }
}
