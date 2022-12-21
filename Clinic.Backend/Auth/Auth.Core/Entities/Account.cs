using Microsoft.AspNetCore.Identity;

namespace Auth.Core.Entities;

public class Account : IdentityUser
{
    public string CreatedBy { get; set; } = "Admin";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}