using MediatR;

namespace IdentityService.Application.Commands;

public sealed record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginCommandResponse>;

public sealed record LoginCommandResponse(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiresAt,
    bool Success,
    string Message
);
