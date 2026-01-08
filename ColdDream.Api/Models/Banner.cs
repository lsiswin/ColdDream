using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class Banner
{
    public Guid Id { get; set; }
    
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    
    public string Tag { get; set; } = string.Empty;
    
    public string TargetUrl { get; set; } = string.Empty;
    
    public int SortOrder { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
