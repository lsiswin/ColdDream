using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.DTOs;

public class RegisterDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    public string? NickName { get; set; }
}

public class LoginDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
}

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Points { get; set; }
    public bool IsNewUser { get; set; }
}

public class WeChatLoginDto
{
    [Required]
    public string LoginCode { get; set; } = string.Empty;
    [Required]
    public string PhoneCode { get; set; } = string.Empty;
}

public class UpdateProfileDto
{
    [Required]
    public string NickName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
}

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? NickName { get; set; }
    public string? AvatarUrl { get; set; }
    public int Points { get; set; }
}
