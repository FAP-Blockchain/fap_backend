using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetByStudentCodeAsync(string studentCode);
        Task<Student?> GetByUserIdAsync(Guid userId);
    }
}