namespace IdentityService.Domain.Entities;

public class AppUserRole
{
    public string UserId { get; set; } = string.Empty;
    public string RoleId { get; set; } = string.Empty;
    public DateTime AssignedAt { get; set; }

    public AppUser? User { get; set; }
    public AppRole? Role { get; set; }
}
