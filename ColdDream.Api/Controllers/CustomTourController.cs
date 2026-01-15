using ColdDream.Api.Models;
using ColdDream.Api.Services;
using ColdDream.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomTourController : ControllerBase
{
    private readonly ICustomTourService _customTourService;
    private readonly IMapper _mapper;

    public CustomTourController(ICustomTourService customTourService, IMapper mapper)
    {
        _customTourService = customTourService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ApiResponse<CustomTourDto>> Create(CreateCustomTourDto customTourDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<CustomTourDto>.Fail("Unauthorized");

        var customTour = _mapper.Map<CustomTour>(customTourDto);
        customTour.UserId = Guid.Parse(userId);
        var result = await _customTourService.CreateAsync(customTour);
        var dto = _mapper.Map<CustomTourDto>(result);
        return ApiResponse<CustomTourDto>.Ok(dto);
    }

    [HttpGet("my")]
    public async Task<ApiResponse<IEnumerable<CustomTourDto>>> GetMyCustomTours()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<IEnumerable<CustomTourDto>>.Fail("Unauthorized");

        var result = await _customTourService.GetUserCustomToursAsync(Guid.Parse(userId));
        var dtos = _mapper.Map<IEnumerable<CustomTourDto>>(result);
        return ApiResponse<IEnumerable<CustomTourDto>>.Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<CustomTourDto>> GetById(Guid id)
    {
        var result = await _customTourService.GetByIdAsync(id);
        if (result == null) return ApiResponse<CustomTourDto>.Fail("Tour not found");
        
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (result.UserId.ToString() != userId) return ApiResponse<CustomTourDto>.Fail("Forbidden");

        var dto = _mapper.Map<CustomTourDto>(result);
        return ApiResponse<CustomTourDto>.Ok(dto);
    }

    [HttpPost("{id}/cancel")]
    public async Task<ApiResponse<object>> Cancel(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        var success = await _customTourService.CancelAsync(id, Guid.Parse(userId));
        if (!success) return ApiResponse<object>.Fail("Cannot cancel this tour request.");
        
        return ApiResponse<object>.Ok(null);
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<object>> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        var success = await _customTourService.DeleteAsync(id, Guid.Parse(userId));
        if (!success) return ApiResponse<object>.Fail("Tour not found or cannot delete");

        return ApiResponse<object>.Ok(null);
    }
}
