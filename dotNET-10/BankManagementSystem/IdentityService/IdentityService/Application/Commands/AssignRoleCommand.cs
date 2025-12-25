using MediatR;

namespace IdentityService.Application.Commands;

public sealed record AssignRoleCommand(
    string UserId,
    string RoleName
) : IRequest<AssignRoleCommandResponse>;

public sealed record AssignRoleCommandResponse(
    bool Success,
    string Message
);
