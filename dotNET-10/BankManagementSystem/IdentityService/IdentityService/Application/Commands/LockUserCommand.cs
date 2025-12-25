using MediatR;

namespace IdentityService.Application.Commands;

public sealed record LockUserCommand(
    string UserId,
    string Reason
) : IRequest<LockUserCommandResponse>;

public sealed record LockUserCommandResponse(
    bool Success,
    string Message
);
