using MediatR;
using Microsoft.AspNetCore.Identity;
using IdentityService.Domain.Entities;
using IdentityService.Domain.Events;
using IdentityService.Application.Commands;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Application.Handlers;

public sealed class AssignRoleCommandHandler : IRequestHandler<AssignRoleCommand, AssignRoleCommandResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<AppRole> _roleManager;
    private readonly IdentityDbContext _context;

    public AssignRoleCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager,
        IdentityDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }

    public async Task<AssignRoleCommandResponse> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return new AssignRoleCommandResponse(false, "User not found");
        }

        var roleExists = await _roleManager.RoleExistsAsync(request.RoleName);
        if (!roleExists)
        {
            return new AssignRoleCommandResponse(false, "Role does not exist");
        }

        var userInRole = await _userManager.IsInRoleAsync(user, request.RoleName);
        if (userInRole)
        {
            return new AssignRoleCommandResponse(false, "User already has this role");
        }

        var result = await _userManager.AddToRoleAsync(user, request.RoleName);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new AssignRoleCommandResponse(false, $"Failed to assign role: {errors}");
        }

        // Add domain event
        user.AddDomainEvent(new RoleAssignedEvent(Guid.Parse(user.Id), request.RoleName));

        // Log domain event
        var eventLog = new DomainEventLog
        {
            AggregateId = Guid.Parse(user.Id),
            EventType = nameof(RoleAssignedEvent),
            EventData = System.Text.Json.JsonSerializer.Serialize(new
            {
                user.Id,
                request.RoleName
            })
        };
        await _context.DomainEventLogs.AddAsync(eventLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new AssignRoleCommandResponse(true, $"Role {request.RoleName} assigned successfully");
    }
}
