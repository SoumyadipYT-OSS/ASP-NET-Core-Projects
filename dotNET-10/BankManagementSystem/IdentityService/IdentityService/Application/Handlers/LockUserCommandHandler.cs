using MediatR;
using Microsoft.AspNetCore.Identity;
using IdentityService.Domain.Entities;
using IdentityService.Application.Commands;
using IdentityService.Infrastructure.Persistence;

namespace IdentityService.Application.Handlers;

public sealed class LockUserCommandHandler : IRequestHandler<LockUserCommand, LockUserCommandResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IdentityDbContext _context;

    public LockUserCommandHandler(UserManager<AppUser> userManager, IdentityDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<LockUserCommandResponse> Handle(LockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return new LockUserCommandResponse(false, "User not found");
        }

        user.Lock(request.Reason);
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new LockUserCommandResponse(false, $"Failed to lock user: {errors}");
        }

        // Log domain event
        var eventLog = new DomainEventLog
        {
            AggregateId = Guid.Parse(user.Id),
            EventType = "UserLockedEvent",
            EventData = System.Text.Json.JsonSerializer.Serialize(new
            {
                user.Id,
                request.Reason
            })
        };
        await _context.DomainEventLogs.AddAsync(eventLog, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new LockUserCommandResponse(true, "User locked successfully");
    }
}
