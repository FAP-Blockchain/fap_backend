using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<Role?> GetByNameAsync(string roleName);
    }
}