using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Entities;

public class Account : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public string CreatedBy { get; set; } = "Admin";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}