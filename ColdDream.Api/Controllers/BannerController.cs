using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Mvc;
using ColdDream.Api.DTOs;
using AutoMapper;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BannerController : ControllerBase
{
    private readonly IBannerService _bannerService;
    private readonly IMapper _mapper;

    public BannerController(IBannerService bannerService, IMapper mapper)
    {
        _bannerService = bannerService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<BannerDto>>> GetBanners()
    {
        var banners = await _bannerService.GetActiveBannersAsync();
        var bannerDtos = _mapper.Map<IEnumerable<BannerDto>>(banners.OrderBy(b => b.SortOrder));
        return ApiResponse<IEnumerable<BannerDto>>.Ok(bannerDtos);
    }
}
