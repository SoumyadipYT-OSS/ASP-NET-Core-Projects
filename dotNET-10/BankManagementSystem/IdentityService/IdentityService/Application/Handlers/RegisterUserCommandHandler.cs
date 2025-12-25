using MediatR;
using Microsoft.AspNetCore.Identity;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Events;
using IdentityService.Application.Commands;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Application.Handlers;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IdentityDbContext _context;

    public RegisterUserCommandHandler(UserManager<AppUser> userManager, IdentityDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            return new RegisterUserCommandResponse(
                Guid.Empty,
                request.Email,
                request.FirstName,
                request.LastName,
                false,
                "User with this email already exists"
            );
        }

        var user = new AppUser
        {
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth,
            EmailConfirmed = false,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new RegisterUserCommandResponse(
                Guid.Empty,
                request.Email,
                request.FirstName,
                request.LastName,
                false,
                $"Registration failed: {errors}"
            );
        }

        // Assign default User role
        await _userManager.AddToRoleAsync(user, "User");

        // Add domain event
        user.AddDomainEvent(new UserRegisteredEvent(
            Guid.Parse(user.Id),
            user.Email,
            user.FirstName,
            user.LastName
        ));

        // Log domain event
        var eventLog = new DomainEventLog
        {
            AggregateId = Guid.Parse(user.Id),
            EventType = nameof(UserRegisteredEvent),
            EventData = System.Text.Json.JsonSerializer.Serialize(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName
            })
        };
        await _context.DomainEventLogs.AddAsync(eventLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new RegisterUserCommandResponse(
            Guid.Parse(user.Id),
            user.Email,
            user.FirstName,
            user.LastName,
            true,
            "User registered successfully"
        );
    }
}
