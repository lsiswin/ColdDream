namespace ColdDream.Api.DTOs;

using System.ComponentModel.DataAnnotations;
using ColdDream.Api.Constants;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }

    // Ideally we would return a UserDto, but for now we might not need user details in list
    // public UserDto? User { get; set; }

    public Guid TourRouteId { get; set; }
    public TourRouteDto? TourRoute { get; set; }

    public Guid? ButlerId { get; set; }
    public ButlerDto? Butler { get; set; }

    public DateTime BookingDate { get; set; }
    public int Travelers { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public decimal DiscountAmount { get; set; }
    public string Status { get; set; } = BookingStatus.Pending;
    public DateTime CreatedAt { get; set; }
}

public class CreateBookingDto
{
    [Required]
    public Guid TourRouteId { get; set; }

    public Guid? ButlerId { get; set; }

    [Required]
    public DateTime BookingDate { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Travelers must be between 1 and 100")]
    public int Travelers { get; set; }

    [Required]
    [StringLength(50)]
    public string ContactName { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string ContactPhone { get; set; } = string.Empty;

    public Guid? CouponId { get; set; }
}
