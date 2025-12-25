using MediatR;

namespace IdentityService.Application.Queries;

public sealed record GetUserByIdQuery(string UserId) : IRequest<GetUserByIdQueryResponse?>;

public sealed record GetUserByIdQueryResponse(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTime CreatedAt,
    List<string> Roles
);
