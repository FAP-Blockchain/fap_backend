using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        Task<Teacher?> GetByTeacherCodeAsync(string teacherCode);
        Task<Teacher?> GetByUserIdAsync(Guid userId);
    }
}