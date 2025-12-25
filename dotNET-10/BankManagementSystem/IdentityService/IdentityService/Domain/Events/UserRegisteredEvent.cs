namespace IdentityService.Domain.Events;

public sealed class UserRegisteredEvent : DomainEvent
{
    public UserRegisteredEvent(Guid userId, string email, string firstName, string lastName)
    {
        AggregateId = userId;
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid UserId { get; }
    public string Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
}
