using ColdDream.Api.DTOs;

namespace ColdDream.Api.Services;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto model);
    Task<AuthResponseDto?> RegisterAsync(RegisterDto model);
}
