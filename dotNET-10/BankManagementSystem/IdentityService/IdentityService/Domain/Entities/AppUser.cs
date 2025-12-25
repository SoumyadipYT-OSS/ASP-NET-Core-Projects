using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using IdentityService.Domain.Events;

namespace IdentityService.Domain.Entities;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? PostalCode { get; set; }
    public string? Country { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public int FailedLoginAttempts { get; set; }
    public DateTime? LastFailedLoginAt { get; set; }

    public ICollection<AppUserRole> UserRoles { get; set; } = new List<AppUserRole>();
    
    private readonly List<DomainEvent> _domainEvents = new();
    
    [NotMapped]
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void Lock(string reason)
    {
        LockoutEnabled = true;
        LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
        AddDomainEvent(new UserLockedEvent(Guid.Parse(Id), reason));
    }

    public void Unlock()
    {
        LockoutEnabled = false;
        LockoutEnd = null;
    }

    public void RecordFailedLogin()
    {
        FailedLoginAttempts++;
        LastFailedLoginAt = DateTime.UtcNow;

        if (FailedLoginAttempts >= 5)
        {
            Lock("Too many failed login attempts");
        }
    }

    public void ClearFailedLoginAttempts()
    {
        FailedLoginAttempts = 0;
        LastFailedLoginAt = null;
    }

    public void RecordSuccessfulLogin()
    {
        LastLoginAt = DateTime.UtcNow;
        ClearFailedLoginAttempts();
    }
}
