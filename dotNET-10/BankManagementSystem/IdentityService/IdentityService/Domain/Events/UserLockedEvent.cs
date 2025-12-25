namespace IdentityService.Domain.Events;

public sealed class UserLockedEvent : DomainEvent
{
    public UserLockedEvent(Guid userId, string reason)
    {
        AggregateId = userId;
        UserId = userId;
        Reason = reason;
    }

    public Guid UserId { get; }
    public string Reason { get; }
}
