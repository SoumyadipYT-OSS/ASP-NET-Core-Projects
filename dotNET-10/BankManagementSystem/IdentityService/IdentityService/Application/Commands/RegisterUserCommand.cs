using MediatR;

namespace IdentityService.Application.Commands;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateTime DateOfBirth
) : IRequest<RegisterUserCommandResponse>;

public sealed record RegisterUserCommandResponse(
    Guid UserId,
    string Email,
    string FirstName,
    string LastName,
    bool Success,
    string Message
);
