using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class Booking
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid TourRouteId { get; set; }
    public TourRoute? TourRoute { get; set; }
    public Guid? ButlerId { get; set; }
    public Butler? Butler { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Paid, Completed, Cancelled
    
    [Required]
    public string OrderNumber { get; set; } = string.Empty;
    public int PeopleCount { get; set; }
    public TimeSlot TimeSlot { get; set; }
    public string? ContactName { get; set; }
    public string? ContactPhone { get; set; }

    public decimal TotalAmount { get; set; }
    public DateTime TravelDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
