using MediatR;
using Microsoft.AspNetCore.Identity;
using IdentityService.Domain.Entities;
using IdentityService.Application.Commands;

namespace IdentityService.Application.Handlers;

public sealed class UnlockUserCommandHandler : IRequestHandler<UnlockUserCommand, UnlockUserCommandResponse>
{
    private readonly UserManager<AppUser> _userManager;

    public UnlockUserCommandHandler(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UnlockUserCommandResponse> Handle(UnlockUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return new UnlockUserCommandResponse(false, "User not found");
        }

        user.Unlock();
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return new UnlockUserCommandResponse(false, $"Failed to unlock user: {errors}");
        }

        return new UnlockUserCommandResponse(true, "User unlocked successfully");
    }
}
