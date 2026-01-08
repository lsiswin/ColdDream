using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class InspirationCollect
{
    public Guid Id { get; set; }
    public Guid InspirationId { get; set; }
    public Inspiration? Inspiration { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
