using System.Security.Claims;
using ColdDream.Api.Models;
using ColdDream.Api.Services;
using ColdDream.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace ColdDream.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GuideController : ControllerBase
{
    private readonly IGuideService _guideService;
    private readonly IMapper _mapper;

    public GuideController(IGuideService guideService, IMapper mapper)
    {
        _guideService = guideService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<GuideDto>>> GetAll()
    {
        var guides = await _guideService.GetAllGuidesAsync();
        return ApiResponse<IEnumerable<GuideDto>>.Ok(_mapper.Map<IEnumerable<GuideDto>>(guides));
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<GuideDto>> GetById(Guid id)
    {
        var guide = await _guideService.GetByIdAsync(id);
        if (guide == null) return ApiResponse<GuideDto>.Fail("Guide not found");
        return ApiResponse<GuideDto>.Ok(_mapper.Map<GuideDto>(guide));
    }

    [Authorize]
    [HttpGet("my")]
    public async Task<ApiResponse<IEnumerable<GuideDto>>> GetMyGuides()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<IEnumerable<GuideDto>>.Fail("Unauthorized");

        var guides = await _guideService.GetUserGuidesAsync(Guid.Parse(userId));
        return ApiResponse<IEnumerable<GuideDto>>.Ok(_mapper.Map<IEnumerable<GuideDto>>(guides));
    }

    [Authorize]
    [HttpPost]
    public async Task<ApiResponse<GuideDto>> Create(CreateGuideDto guideDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<GuideDto>.Fail("Unauthorized");

        var guide = _mapper.Map<Guide>(guideDto);
        guide.UserId = Guid.Parse(userId);
        var result = await _guideService.CreateAsync(guide);
        return ApiResponse<GuideDto>.Ok(_mapper.Map<GuideDto>(result), "Guide created");
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ApiResponse<object>> Update(Guid id, CreateGuideDto guideDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");
        
        // Verify ownership
        var existing = await _guideService.GetByIdAsync(id);
        if (existing == null) return ApiResponse<object>.Fail("Guide not found");
        if (existing.UserId != Guid.Parse(userId)) return ApiResponse<object>.Fail("Forbidden");

        _mapper.Map(guideDto, existing);
        await _guideService.UpdateAsync(existing);
        return ApiResponse<object>.Ok(null, "Guide updated");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ApiResponse<object>> Delete(Guid id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        var success = await _guideService.DeleteAsync(id, Guid.Parse(userId));
        if (!success) return ApiResponse<object>.Fail("Guide not found or cannot delete");

        return ApiResponse<object>.Ok(null, "Guide deleted");
    }
}
