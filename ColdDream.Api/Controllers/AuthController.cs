using ColdDream.Api.DTOs;
using ColdDream.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ColdDream.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ApiResponse<AuthResponseDto>> Register(RegisterDto model)
    {
        var result = await _authService.RegisterAsync(model);
        if (result == null)
            return ApiResponse<AuthResponseDto>.Fail("Registration failed");
        return ApiResponse<AuthResponseDto>.Ok(result);
    }

    [HttpPost("login")]
    public async Task<ApiResponse<AuthResponseDto>> Login(LoginDto model)
    {
        var result = await _authService.LoginAsync(model);
        if (result == null)
            return ApiResponse<AuthResponseDto>.Fail("Invalid credentials");
        return ApiResponse<AuthResponseDto>.Ok(result);
    }

    [HttpPost("wechat-login")]
    public async Task<ApiResponse<AuthResponseDto>> WeChatLogin(WeChatLoginDto model)
    {
        var result = await _authService.WeChatLoginAsync(model);
        if (result == null)
            return ApiResponse<AuthResponseDto>.Fail("WeChat login failed");
        return ApiResponse<AuthResponseDto>.Ok(result);
    }

    [HttpPost("guest-login")]
    public async Task<ApiResponse<AuthResponseDto>> GuestLogin()
    {
        var result = await _authService.GuestLoginAsync();
        if (result == null)
            return ApiResponse<AuthResponseDto>.Fail("Guest login failed");
        return ApiResponse<AuthResponseDto>.Ok(result);
    }

    [HttpPost("profile")]
    public async Task<ApiResponse<object>> UpdateProfile(UpdateProfileDto model)
    {
        // For simplicity, we assume the user is authenticated and we get the ID from claims
        // But for the profile completion flow, we might need to pass the ID or use the token if already issued
        // Let's assume the token is issued and we get the ID from claims
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return ApiResponse<object>.Fail("Unauthorized");

        var result = await _authService.UpdateProfileAsync(Guid.Parse(userId), model);
        if (!result)
            return ApiResponse<object>.Fail("Profile update failed");
        return ApiResponse<object>.Ok(new { success = true });
    }

    [HttpGet("me")]
    public async Task<ApiResponse<UserProfileDto>> GetProfile()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return ApiResponse<UserProfileDto>.Fail("Unauthorized");

        var profile = await _authService.GetProfileAsync(Guid.Parse(userId));
        if (profile == null) return ApiResponse<UserProfileDto>.Fail("Profile not found");

        return ApiResponse<UserProfileDto>.Ok(profile);
    }
}
