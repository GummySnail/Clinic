using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Identity;

public class User : IdentityUser
{
    public string CreatedBy { get; set; } = "Administrator";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
}