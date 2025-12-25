using MediatR;
using Microsoft.AspNetCore.Mvc;
using IdentityService.Application.Commands;

namespace IdentityService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IMediator mediator, ILogger<AuthController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<RegisterUserCommandResponse>> Register(
        [FromBody] RegisterUserRequest request)
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.DateOfBirth
        );

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("User registration failed: {Message}", result.Message);
            return BadRequest(new { message = result.Message });
        }

        _logger.LogInformation("User registered successfully: {Email}", request.Email);
        return CreatedAtAction(nameof(Register), new { userId = result.UserId }, result);
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<LoginCommandResponse>> Login(
        [FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("Login failed for user: {Email}", request.Email);
            return Unauthorized(new { message = result.Message });
        }

        _logger.LogInformation("User logged in successfully: {Email}", request.Email);
        return Ok(result);
    }

    [HttpPost("refresh-token")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<RefreshTokenCommandResponse>> RefreshToken(
        [FromBody] RefreshTokenRequest request)
    {
        var command = new RefreshTokenCommand(request.AccessToken, request.RefreshToken);
        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            _logger.LogWarning("Token refresh failed");
            return Unauthorized(new { message = result.Message });
        }

        _logger.LogInformation("Token refreshed successfully");
        return Ok(result);
    }
}

public record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateTime DateOfBirth
);

public record LoginRequest(string Email, string Password);

public record RefreshTokenRequest(string AccessToken, string RefreshToken);
