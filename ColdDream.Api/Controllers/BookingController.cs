using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ColdDream.Api.DTOs;
using AutoMapper;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IMapper _mapper;

    public BookingController(IBookingService bookingService, IMapper mapper)
    {
        _bookingService = bookingService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ApiResponse<BookingDto>> Create(CreateBookingDto bookingDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<BookingDto>.Fail("Unauthorized");

        var booking = _mapper.Map<Booking>(bookingDto);
        var result = await _bookingService.CreateBookingAsync(Guid.Parse(userId), booking);
        var dto = _mapper.Map<BookingDto>(result);
        return ApiResponse<BookingDto>.Ok(dto);
    }

    [HttpGet("my")]
    public async Task<ApiResponse<IEnumerable<BookingDto>>> GetMyBookings()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<IEnumerable<BookingDto>>.Fail("Unauthorized");

        var result = await _bookingService.GetUserBookingsAsync(Guid.Parse(userId));
        var dtos = _mapper.Map<IEnumerable<BookingDto>>(result);
        return ApiResponse<IEnumerable<BookingDto>>.Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<BookingDto>> GetById(Guid id)
    {
        var result = await _bookingService.GetBookingByIdAsync(id);
        if (result == null) return ApiResponse<BookingDto>.Fail("Booking not found");
        return ApiResponse<BookingDto>.Ok(_mapper.Map<BookingDto>(result));
    }

    [HttpPost("{id}/cancel")]
    public async Task<ApiResponse<object>> Cancel(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        var result = await _bookingService.CancelBookingAsync(id, Guid.Parse(userId));
        if (!result) return ApiResponse<object>.Fail("Cancel failed");
        
        return ApiResponse<object>.Ok(new { success = true });
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<object>> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        var result = await _bookingService.DeleteBookingAsync(id, Guid.Parse(userId));
        if (!result) return ApiResponse<object>.Fail("Delete failed");
        
        return ApiResponse<object>.Ok(new { success = true });
    }
}
