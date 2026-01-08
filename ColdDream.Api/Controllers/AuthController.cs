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
    public async Task<IActionResult> Register(RegisterDto model)
    {
        var result = await _authService.RegisterAsync(model);
        if (result == null)
            return BadRequest("Registration failed");
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto model)
    {
        var result = await _authService.LoginAsync(model);
        if (result == null)
            return Unauthorized("Invalid credentials");
        return Ok(result);
    }

    [HttpPost("wechat-login")]
    public async Task<IActionResult> WeChatLogin(WeChatLoginDto model)
    {
        var result = await _authService.WeChatLoginAsync(model);
        if (result == null)
            return BadRequest("WeChat login failed");
        return Ok(result);
    }

    [HttpPost("guest-login")]
    public async Task<IActionResult> GuestLogin()
    {
        var result = await _authService.GuestLoginAsync();
        if (result == null)
            return BadRequest("Guest login failed");
        return Ok(result);
    }

    [HttpPost("profile")]
    public async Task<IActionResult> UpdateProfile(UpdateProfileDto model)
    {
        // For simplicity, we assume the user is authenticated and we get the ID from claims
        // But for the profile completion flow, we might need to pass the ID or use the token if already issued
        // Let's assume the token is issued and we get the ID from claims
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var result = await _authService.UpdateProfileAsync(Guid.Parse(userId), model);
        if (!result)
            return BadRequest("Profile update failed");
        return Ok(new { success = true });
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetProfile()
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null) return Unauthorized();

        var profile = await _authService.GetProfileAsync(Guid.Parse(userId));
        if (profile == null) return NotFound();

        return Ok(profile);
    }
}
