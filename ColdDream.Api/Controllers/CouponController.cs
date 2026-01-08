using ColdDream.Api.Models;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ColdDream.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;

    public CouponController(ICouponService couponService)
    {
        _couponService = couponService;
    }

    [HttpGet("my")]
    public async Task<IActionResult> GetMyCoupons()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var coupons = await _couponService.GetMyCouponsAsync(Guid.Parse(userId));
        return Ok(coupons);
    }

    // Endpoint to give a test coupon (for demo purposes)
    [HttpPost("test-issue")]
    public async Task<IActionResult> IssueTestCoupon()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        await _couponService.CreateCouponAsync(Guid.Parse(userId), 50, "新用户专享红包");
        return Ok(new { message = "Coupon issued" });
    }
}
