using MediatR;

namespace IdentityService.Application.Commands;

public sealed record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<RefreshTokenCommandResponse>;

public sealed record RefreshTokenCommandResponse(
    string NewAccessToken,
    string NewRefreshToken,
    DateTime ExpiresAt,
    bool Success,
    string Message
);
