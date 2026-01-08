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
    
    [Required]
    public DateTime BookingDate { get; set; }
    
    [Required]
    public int Travelers { get; set; }
    
    [Required]
    public string ContactName { get; set; } = string.Empty;
    
    [Required]
    public string ContactPhone { get; set; } = string.Empty;
    
    public decimal TotalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public Guid? CouponId { get; set; }
    
    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
