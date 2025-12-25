namespace IdentityService.Domain.Events;

public sealed class RoleAssignedEvent : DomainEvent
{
    public RoleAssignedEvent(Guid userId, string roleName)
    {
        AggregateId = userId;
        UserId = userId;
        RoleName = roleName;
    }

    public Guid UserId { get; }
    public string RoleName { get; }
}
