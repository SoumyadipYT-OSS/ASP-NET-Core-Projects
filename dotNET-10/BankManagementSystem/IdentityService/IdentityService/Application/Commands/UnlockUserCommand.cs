using MediatR;

namespace IdentityService.Application.Commands;

public sealed record UnlockUserCommand(
    string UserId
) : IRequest<UnlockUserCommandResponse>;

public sealed record UnlockUserCommandResponse(
    bool Success,
    string Message
);
