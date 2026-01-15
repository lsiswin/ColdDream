using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ColdDream.Api.DTOs;
using ColdDream.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ColdDream.Api.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto model);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto model);
    Task<AuthResponseDto?> WeChatLoginAsync(WeChatLoginDto model);
    Task<AuthResponseDto?> GuestLoginAsync();
    Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileDto model);
    Task<UserProfileDto?> GetProfileAsync(Guid userId);
}

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IWeChatService _weChatService;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IConfiguration configuration,
        IWeChatService weChatService
    )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _weChatService = weChatService;
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
            return null;

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        if (!result.Succeeded)
            return null;

        return new AuthResponseDto
        {
            Token = GenerateJwtToken(user),
            Username = user.UserName!,
            Email = user.Email!,
            Points = user.Points,
        };
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto model)
    {
        var user = new User
        {
            UserName = model.Username,
            Email = model.Email,
            NickName = model.NickName,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return null;

        return new AuthResponseDto
        {
            Token = GenerateJwtToken(user),
            Username = user.UserName!,
            Email = user.Email!,
            Points = user.Points,
        };
    }

    public async Task<AuthResponseDto?> WeChatLoginAsync(WeChatLoginDto model)
    {
        // 1. Get OpenId from LoginCode
        var session = await _weChatService.Code2Session(model.LoginCode);
        if (session == null || string.IsNullOrEmpty(session.OpenId))
            return null;

        // 2. Get Phone Number from PhoneCode
        var phoneInfo = await _weChatService.GetPhoneNumber(model.PhoneCode);
        if (phoneInfo == null || phoneInfo.PhoneInfo == null)
            return null;

        var phoneNumber = phoneInfo.PhoneInfo.PhoneNumber;

        // 3. Find user by OpenId
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.OpenId == session.OpenId);
        bool isNewUser = false;

        if (user == null)
        {
            // 4. Create new user if not exists
            isNewUser = true;
            user = new User
            {
                UserName = Guid.NewGuid().ToString("N"), // Random username
                Email = $"{session.OpenId}@wechat.com", // Placeholder email
                OpenId = session.OpenId,
                PhoneNumber = phoneNumber,
                NickName = "微信用户", // Default nickname
                CreatedAt = DateTime.UtcNow,
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
                return null;
        }
        else
        {
            // Update phone number if changed
            if (user.PhoneNumber != phoneNumber)
            {
                user.PhoneNumber = phoneNumber;
                await _userManager.UpdateAsync(user);
            }
        }

        return new AuthResponseDto
        {
            Token = GenerateJwtToken(user),
            Username = user.UserName!,
            Email = user.Email!,
            Points = user.Points,
            IsNewUser = isNewUser,
        };
    }

    public async Task<AuthResponseDto?> GuestLoginAsync()
    {
        var guestUser = await _userManager.FindByNameAsync("guest_tester");
        if (guestUser == null)
        {
            guestUser = new User
            {
                UserName = "guest_tester",
                Email = "guest@colddream.com",
                NickName = "游客测试员",
                CreatedAt = DateTime.UtcNow,
                OpenId = "guest_openid_" + Guid.NewGuid().ToString("N"), // Dummy OpenId
            };
            var result = await _userManager.CreateAsync(guestUser, "Guest@123456");
            if (!result.Succeeded)
                return null;
        }

        return new AuthResponseDto
        {
            Token = GenerateJwtToken(guestUser),
            Username = guestUser.UserName!,
            Email = guestUser.Email!,
            Points = guestUser.Points,
            IsNewUser = false,
        };
    }

    public async Task<bool> UpdateProfileAsync(Guid userId, UpdateProfileDto model)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return false;

        user.NickName = model.NickName;
        // user.AvatarUrl = model.AvatarUrl; // Add AvatarUrl to User model if needed

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<UserProfileDto?> GetProfileAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return null;

        return new UserProfileDto
        {
            Id = user.Id,
            Username = user.UserName!,
            Email = user.Email!,
            NickName = user.NickName,
            Points = user.Points,
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
