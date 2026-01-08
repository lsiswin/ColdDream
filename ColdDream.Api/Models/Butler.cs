using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class Butler
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; } = string.Empty;
    
    public string AvatarUrl { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    
    public string Tags { get; set; } = string.Empty; // JSON array or comma separated
    
    public double Rating { get; set; }
    
    public Guid? TourRouteId { get; set; } // Optional: if a butler is specific to a route, or null if global (simplified)
}
