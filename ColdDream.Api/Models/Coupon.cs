using System.ComponentModel.DataAnnotations;

namespace ColdDream.Api.Models;

public class Coupon
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; } // Owner
    public string Name { get; set; } = string.Empty; // e.g., "New User Bonus"
    public decimal Amount { get; set; } // Discount amount
    public decimal MinSpend { get; set; } = 0; // Minimum spend to use
    public DateTime ExpiryDate { get; set; }
    public bool IsUsed { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
