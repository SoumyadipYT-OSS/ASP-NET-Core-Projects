using Microsoft.AspNetCore.Identity;

namespace IdentityService.Domain.Entities;

public class AppRole : IdentityRole<string>
{
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
}
