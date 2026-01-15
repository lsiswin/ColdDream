namespace ColdDream.Api.DTOs;

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
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}

public class CreateBookingDto
{
    public Guid TourRouteId { get; set; }
    public Guid? ButlerId { get; set; }
    public DateTime BookingDate { get; set; }
    public int Travelers { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public Guid? CouponId { get; set; }
}
