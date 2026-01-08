using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class Guide
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Itinerary { get; set; } // JSON string: [{day: 1, title: "", content: ""}]
    public int Status { get; set; } = 0; // 0: Pending, 1: Published, 2: Rejected
    public string? Tags { get; set; } // JSON array: ["Tag1", "Tag2"]
    public string? Duration { get; set; }
    public string? Budget { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
