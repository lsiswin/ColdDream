using ColdDream.Api.Models;
using ColdDream.Api.Services;
using ColdDream.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using AutoMapper;

namespace ColdDream.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CouponController : ControllerBase
{
    private readonly ICouponService _couponService;
    private readonly IMapper _mapper;

    public CouponController(ICouponService couponService, IMapper mapper)
    {
        _couponService = couponService;
        _mapper = mapper;
    }

    [HttpGet("my")]
    public async Task<ApiResponse<IEnumerable<CouponDto>>> GetMyCoupons()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return ApiResponse<IEnumerable<CouponDto>>.Fail("Unauthorized");

        var coupons = await _couponService.GetMyCouponsAsync(Guid.Parse(userId));
        var couponDtos = _mapper.Map<IEnumerable<CouponDto>>(coupons);
        return ApiResponse<IEnumerable<CouponDto>>.Ok(couponDtos);
    }

    // Endpoint to give a test coupon (for demo purposes)
    [HttpPost("test-issue")]
    public async Task<ApiResponse<object>> IssueTestCoupon()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        await _couponService.CreateCouponAsync(Guid.Parse(userId), 50, "新用户专享红包");
        return ApiResponse<object>.Ok(new { message = "Coupon issued" });
    }
}
