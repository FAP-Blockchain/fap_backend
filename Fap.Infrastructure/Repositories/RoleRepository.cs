using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _dbSet
                .FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
        }
    }
}