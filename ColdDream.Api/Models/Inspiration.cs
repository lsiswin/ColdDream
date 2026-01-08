using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class Inspiration
{
    public Guid Id { get; set; }
    
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    
    [Required]
    public string Description { get; set; } = string.Empty;
    
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    
    public int Likes { get; set; }
    public int Collects { get; set; }
    
    public bool IsLiked { get; set; } // Not mapped to DB, used for DTO
    public bool IsCollected { get; set; } // Not mapped to DB, used for DTO
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
