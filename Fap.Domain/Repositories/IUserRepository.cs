using Fap.Domain.Entities;
using System.Threading.Tasks;

namespace Fap.Domain.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdWithRoleAsync(Guid id);
    }
}
