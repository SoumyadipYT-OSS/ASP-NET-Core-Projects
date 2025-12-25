using System.IdentityModel.Tokens.Jwt;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using IdentityService.Domain.Entities;
using IdentityService.Application.Commands;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Infrastructure.Repositories;

namespace IdentityService.Application.Handlers;

public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IConfiguration _configuration;

    public LoginCommandHandler(
        UserManager<AppUser> userManager,
        IRefreshTokenRepository refreshTokenRepository,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _configuration = configuration;
    }

    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            user?.RecordFailedLogin();
            if (user != null)
            {
                await _userManager.UpdateAsync(user);
            }

            return new LoginCommandResponse(
                string.Empty,
                string.Empty,
                DateTime.UtcNow,
                false,
                "Invalid email or password"
            );
        }

        if (user.LockoutEnd.HasValue && user.LockoutEnd > DateTimeOffset.UtcNow)
        {
            return new LoginCommandResponse(
                string.Empty,
                string.Empty,
                DateTime.UtcNow,
                false,
                "User account is locked"
            );
        }

        if (!user.IsActive)
        {
            return new LoginCommandResponse(
                string.Empty,
                string.Empty,
                DateTime.UtcNow,
                false,
                "User account is inactive"
            );
        }

        user.RecordSuccessfulLogin();
        await _userManager.UpdateAsync(user);

        var accessToken = GenerateAccessToken(user);
        var refreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = GenerateRefreshTokenString(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IssuedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.AddAsync(refreshToken);

        var expiresAt = DateTime.UtcNow.AddMinutes(15);

        return new LoginCommandResponse(
            accessToken,
            refreshToken.Token,
            expiresAt,
            true,
            "Login successful"
        );
    }

    private string GenerateAccessToken(AppUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<System.Security.Claims.Claim>
        {
            new(System.Security.Claims.ClaimTypes.NameIdentifier, user.Id),
            new(System.Security.Claims.ClaimTypes.Email, user.Email ?? string.Empty),
            new("FirstName", user.FirstName),
            new("LastName", user.LastName)
        };

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshTokenString()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
