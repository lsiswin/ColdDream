using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class CustomTourRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    
    [Required]
    public string Destination { get; set; } = string.Empty;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public int Days { get; set; }
    
    [Required]
    public int PeopleCount { get; set; }
    
    public decimal? Budget { get; set; }
    
    public string? Requirements { get; set; } // Special requirements, e.g., "Vegetarian", "Wheelchair accessible"
    
    public string ContactName { get; set; } = string.Empty;
    
    [Required]
    public string ContactPhone { get; set; } = string.Empty;
    
    public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Cancelled
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
