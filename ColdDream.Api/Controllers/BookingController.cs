using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(Booking booking)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _bookingService.CreateBookingAsync(Guid.Parse(userId), booking);
        return Ok(result);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _bookingService.GetUserBookingsAsync(Guid.Parse(userId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _bookingService.GetBookingByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _bookingService.CancelBookingAsync(id, Guid.Parse(userId));
        if (!result) return BadRequest("Cancel failed");
        
        return Ok(new { success = true });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var result = await _bookingService.DeleteBookingAsync(id, Guid.Parse(userId));
        if (!result) return BadRequest("Delete failed");
        
        return Ok(new { success = true });
    }
}
