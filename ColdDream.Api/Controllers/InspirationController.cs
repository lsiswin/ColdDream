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
public class InspirationController : ControllerBase
{
    private readonly IInspirationService _inspirationService;
    private readonly IMapper _mapper;

    public InspirationController(IInspirationService inspirationService, IMapper mapper)
    {
        _inspirationService = inspirationService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<InspirationDto>>> GetList()
    {
        Guid? userId = null;
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim != null)
        {
            userId = Guid.Parse(userIdClaim);
        }

        var list = await _inspirationService.GetInspirationsAsync(userId);
        var dtos = _mapper.Map<IEnumerable<InspirationDto>>(list.OrderByDescending(x => x.CreatedAt));
        return ApiResponse<IEnumerable<InspirationDto>>.Ok(dtos);
    }

    [HttpGet("collected")]
    [Authorize]
    public async Task<ApiResponse<IEnumerable<InspirationDto>>> GetCollected()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<IEnumerable<InspirationDto>>.Fail("Unauthorized");

        var list = await _inspirationService.GetCollectedInspirationsAsync(Guid.Parse(userId));
        var dtos = _mapper.Map<IEnumerable<InspirationDto>>(list.OrderByDescending(x => x.CreatedAt));
        return ApiResponse<IEnumerable<InspirationDto>>.Ok(dtos);
    }

    [HttpPost("{id}/like")]
    [Authorize]
    public async Task<ApiResponse<object>> Like(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        var result = await _inspirationService.ToggleLikeAsync(id, Guid.Parse(userId));
        if (result == null) return ApiResponse<object>.Fail("Inspiration not found");
        
        return ApiResponse<object>.Ok(new { likes = result.Likes, isLiked = result.IsLiked });
    }

    [HttpPost("{id}/collect")]
    [Authorize]
    public async Task<ApiResponse<object>> Collect(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        var result = await _inspirationService.ToggleCollectAsync(id, Guid.Parse(userId));
        if (result == null) return ApiResponse<object>.Fail("Inspiration not found");

        return ApiResponse<object>.Ok(new { collects = result.Collects, isCollected = result.IsCollected });
    }
}
