namespace ColdDream.Api.DTOs;

public class BannerDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Tag { get; set; } = string.Empty;
    public string TargetUrl { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

public class ButlerDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Tags { get; set; } = string.Empty;
    public double Rating { get; set; }
}

public class CouponDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal MinSpend { get; set; }
    public DateTime ExpiryDate { get; set; }
    public bool IsUsed { get; set; }
}

public class CustomTourDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Destination { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public int Days { get; set; }
    public int PeopleCount { get; set; }
    public string Budget { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime CreatedAt { get; set; }
}

public class CreateCustomTourDto
{
    public string Destination { get; set; } = string.Empty;
    public string StartDate { get; set; } = string.Empty;
    public int Days { get; set; }
    public int PeopleCount { get; set; }
    public string Budget { get; set; } = string.Empty;
    public string Requirements { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
}

public class GuideDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Itinerary { get; set; }
    public int Status { get; set; }
    public string? Tags { get; set; }
    public string? Duration { get; set; }
    public string? Budget { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateGuideDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Itinerary { get; set; }
    public string? Tags { get; set; }
    public string? Duration { get; set; }
    public string? Budget { get; set; }
}

public class CreateInspirationDto
{
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
}

public class InspirationDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserAvatar { get; set; } = string.Empty;
    public int Likes { get; set; }
    public int Collects { get; set; }
    public bool IsLiked { get; set; }
    public bool IsCollected { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TourRouteDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
    public decimal Price { get; set; }
    public string? Tags { get; set; }
    public string? ImageUrl { get; set; }
    public string? RouteMapUrl { get; set; }

    public string? Duration { get; set; }
    public string? Itinerary { get; set; }
    public int Sales { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateTourRouteDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsPrivate { get; set; }
    public decimal Price { get; set; }
    public string? Tags { get; set; }
    public string? ImageUrl { get; set; }
    public string? RouteMapUrl { get; set; }
    public string? Itinerary { get; set; }
}
