using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BannerController : ControllerBase
{
    private readonly IBannerService _bannerService;

    public BannerController(IBannerService bannerService)
    {
        _bannerService = bannerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetBanners()
    {
        var banners = await _bannerService.GetActiveBannersAsync();
        return Ok(banners.OrderBy(b => b.SortOrder));
    }
}
