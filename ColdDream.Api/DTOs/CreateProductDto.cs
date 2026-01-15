using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.DTOs;

public class CreateProductDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    [Range(0, int.MaxValue)]
    public int PointsPrice { get; set; }
    [Range(0, int.MaxValue)]
    public int Stock { get; set; }
}
