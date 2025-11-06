using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface ITeacherRepository : IGenericRepository<Teacher>
    {
        Task<Teacher?> GetByTeacherCodeAsync(string teacherCode);
        Task<Teacher?> GetByUserIdAsync(Guid userId);
        Task<Teacher?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Teacher>> GetAllWithUsersAsync();
        Task<(List<Teacher> Teachers, int TotalCount)> GetPagedTeachersAsync(
            int page,
            int pageSize,
            string? searchTerm,
            string? specialization,
            bool? isActive,
            string? sortBy,
            string? sortOrder
        );
    }
}