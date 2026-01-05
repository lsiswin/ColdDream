using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class Butler
{
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public decimal Price { get; set; }
    public string? Tags { get; set; } // JSON string
    public double Rating { get; set; }
    public ICollection<TourRoute> TourRoutes { get; set; } = new List<TourRoute>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
