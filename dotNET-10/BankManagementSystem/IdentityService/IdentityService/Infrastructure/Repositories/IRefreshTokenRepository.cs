using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetByTokenAsync(string token);
    Task<IEnumerable<RefreshToken>> GetByUserIdAsync(string userId);
    Task AddAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
    Task RevokeAsync(Guid tokenId);
    Task RevokeUserTokensAsync(string userId);
}
