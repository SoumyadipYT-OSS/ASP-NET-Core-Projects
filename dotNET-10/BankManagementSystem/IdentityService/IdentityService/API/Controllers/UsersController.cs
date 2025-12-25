using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Application.Commands;
using IdentityService.Application.Queries;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class UsersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<UsersController> _logger;

    public UsersController(IMediator mediator, ILogger<UsersController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetUserByIdQueryResponse>> GetUserById(string userId)
    {
        var query = new GetUserByIdQuery(userId);
        var result = await _mediator.Send(query);

        if (result == null)
        {
            _logger.LogWarning("User not found: {UserId}", userId);
            return NotFound(new { message = "User not found" });
        }

        return Ok(result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<GetAllUsersQueryResponse>> GetAllUsers(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetAllUsersQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost("{userId}/roles")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<AssignRoleCommandResponse>> AssignRole(
        string userId,
        [FromBody] AssignRoleRequest request)
    {
        var command = new AssignRoleCommand(userId, request.RoleName);
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("Failed to assign role {RoleName} to user {UserId}: {Message}", 
                request.RoleName, userId, result.Message);
            return BadRequest(new { message = result.Message });
        }

        _logger.LogInformation("Role {RoleName} assigned to user {UserId}", request.RoleName, userId);
        return Ok(result);
    }

    [HttpPost("{userId}/lock")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<LockUserCommandResponse>> LockUser(
        string userId,
        [FromBody] LockUserRequest request)
    {
        var command = new LockUserCommand(userId, request.Reason);
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("Failed to lock user {UserId}: {Message}", userId, result.Message);
            return BadRequest(new { message = result.Message });
        }

        _logger.LogInformation("User {UserId} locked with reason: {Reason}", userId, request.Reason);
        return Ok(result);
    }

    [HttpPost("{userId}/unlock")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<UnlockUserCommandResponse>> UnlockUser(string userId)
    {
        var command = new UnlockUserCommand(userId);
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("Failed to unlock user {UserId}: {Message}", userId, result.Message);
            return BadRequest(new { message = result.Message });
        }

        _logger.LogInformation("User {UserId} unlocked", userId);
        return Ok(result);
    }
}

public record AssignRoleRequest(string RoleName);
public record LockUserRequest(string Reason);
