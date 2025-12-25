using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using IdentityService.Application.Commands;
using IdentityService.Infrastructure.Repositories;
using IdentityService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.Application.Handlers;

public sealed class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenCommandResponse>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;

    public RefreshTokenCommandHandler(
        IRefreshTokenRepository refreshTokenRepository,
        UserManager<AppUser> userManager,
        IConfiguration configuration)
    {
        _refreshTokenRepository = refreshTokenRepository;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<RefreshTokenCommandResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = await _refreshTokenRepository.GetByTokenAsync(request.RefreshToken);
        if (refreshToken == null || !refreshToken.IsValid)
        {
            return new RefreshTokenCommandResponse(
                string.Empty,
                string.Empty,
                DateTime.UtcNow,
                false,
                "Invalid or expired refresh token"
            );
        }

        var user = await _userManager.FindByIdAsync(refreshToken.UserId);
        if (user == null || !user.IsActive)
        {
            return new RefreshTokenCommandResponse(
                string.Empty,
                string.Empty,
                DateTime.UtcNow,
                false,
                "User not found or inactive"
            );
        }

        var newAccessToken = GenerateAccessToken(user);
        var newRefreshToken = new RefreshToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = GenerateRefreshTokenString(),
            ExpiresAt = DateTime.UtcNow.AddDays(7),
            IssuedAt = DateTime.UtcNow
        };

        await _refreshTokenRepository.RevokeAsync(refreshToken.Id);
        await _refreshTokenRepository.AddAsync(newRefreshToken);

        var expiresAt = DateTime.UtcNow.AddMinutes(15);

        return new RefreshTokenCommandResponse(
            newAccessToken,
            newRefreshToken.Token,
            expiresAt,
            true,
            "Token refreshed successfully"
        );
    }

    private string GenerateAccessToken(AppUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey not configured")));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
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
