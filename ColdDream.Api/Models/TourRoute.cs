using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class TourRoute
{
    public Guid Id { get; set; }
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
    public decimal Price { get; set; }
    public string? Tags { get; set; } // JSON string
    public string? ImageUrl { get; set; }
    public string? RouteMapUrl { get; set; }
    public string? Itinerary { get; set; } // JSON string: [{day: 1, title: "", content: ""}]
    public int Sales { get; set; }
    public ICollection<Butler> Butlers { get; set; } = new List<Butler>();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
