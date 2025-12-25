using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Repositories;

public interface IUserRepository
{
    Task<AppUser?> GetByIdAsync(string userId);
    Task<AppUser?> GetByEmailAsync(string email);
    Task<IEnumerable<AppUser>> GetAllAsync(int pageNumber, int pageSize);
    Task<int> GetTotalCountAsync();
    Task AddAsync(AppUser user);
    Task UpdateAsync(AppUser user);
    Task DeleteAsync(string userId);
}
