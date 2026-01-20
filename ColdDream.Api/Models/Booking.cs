using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ColdDream.Api.Constants;

namespace ColdDream.Api.Models;

public class Booking
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    [Required]
    public Guid TourRouteId { get; set; }
    
    [ForeignKey(nameof(TourRouteId))]
    public TourRoute? TourRoute { get; set; }

    public Guid? ButlerId { get; set; }
    
    [ForeignKey(nameof(ButlerId))]
    public Butler? Butler { get; set; }

    public DateTime BookingDate { get; set; }

    public int Travelers { get; set; }

    [Required]
    [MaxLength(50)]
    public string ContactName { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountAmount { get; set; }

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = BookingStatus.Pending;

    // Coupon used
    public Guid? CouponId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
