using Microsoft.AspNetCore.Identity;

namespace ColdDream.Api.Models;

public class User : IdentityUser<Guid>
{
    public string? OpenId { get; set; }
    public string? NickName { get; set; }
    public int Points { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
