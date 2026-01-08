using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class CustomTour
{
    public Guid Id { get; set; }
    
    public Guid UserId { get; set; }
    
    [Required]
    public string Destination { get; set; } = string.Empty;
    
    [Required]
    public string StartDate { get; set; } = string.Empty;
    
    [Required]
    public int Days { get; set; }
    
    [Required]
    public int PeopleCount { get; set; }
    
    [Required]
    public string Budget { get; set; } = string.Empty;
    
    public string Requirements { get; set; } = string.Empty;
    
    public string ContactName { get; set; } = string.Empty;
    
    public string ContactPhone { get; set; } = string.Empty;
    
    public string Status { get; set; } = "Pending"; // Pending, Processing, Completed
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
