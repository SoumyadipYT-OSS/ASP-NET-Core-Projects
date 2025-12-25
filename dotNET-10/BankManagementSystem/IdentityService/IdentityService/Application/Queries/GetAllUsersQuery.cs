using MediatR;

namespace IdentityService.Application.Queries;

public sealed record GetAllUsersQuery(
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<GetAllUsersQueryResponse>;

public sealed record GetAllUsersQueryResponse(
    List<UserDto> Users,
    int TotalCount,
    int PageNumber,
    int PageSize
);

public sealed record UserDto(
    string UserId,
    string Email,
    string FirstName,
    string LastName,
    bool IsActive,
    DateTime CreatedAt,
    List<string> Roles
);
