using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking(Booking booking)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        booking.UserId = Guid.Parse(userId);
        
        var createdBooking = await _bookingService.CreateBookingAsync(booking);
        return CreatedAtAction(nameof(GetBooking), new { id = createdBooking.Id }, createdBooking);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBooking(Guid id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (booking.UserId.ToString() != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        return Ok(booking);
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var bookings = await _bookingService.GetUserBookingsAsync(Guid.Parse(userId));
        return Ok(bookings);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBooking(Guid id, Booking booking)
    {
        if (id != booking.Id) return BadRequest();

        var existingBooking = await _bookingService.GetBookingByIdAsync(id);
        if (existingBooking == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (existingBooking.UserId.ToString() != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        // Allow updating specific fields
        existingBooking.PeopleCount = booking.PeopleCount;
        existingBooking.TimeSlot = booking.TimeSlot;
        existingBooking.ContactName = booking.ContactName;
        existingBooking.ContactPhone = booking.ContactPhone;
        existingBooking.ButlerId = booking.ButlerId;
        
        // Recalculate amount if needed (simplified here)
        // In real app, might need to call service to recalculate

        await _bookingService.UpdateBookingAsync(existingBooking);
        return Ok(existingBooking);
    }

    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> CancelBooking(Guid id)
    {
        var booking = await _bookingService.GetBookingByIdAsync(id);
        if (booking == null) return NotFound();

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (booking.UserId.ToString() != userId && !User.IsInRole("Admin"))
        {
            return Forbid();
        }

        await _bookingService.CancelBookingAsync(id);
        return NoContent();
    }
}
