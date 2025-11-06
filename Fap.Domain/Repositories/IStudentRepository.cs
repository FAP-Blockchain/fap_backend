using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface IStudentRepository : IGenericRepository<Student>
    {
        Task<Student?> GetByStudentCodeAsync(string studentCode);
        Task<Student?> GetByUserIdAsync(Guid userId);
        Task<Student?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Student>> GetAllWithUsersAsync();
        Task<(List<Student> Students, int TotalCount)> GetPagedStudentsAsync(
            int page,
            int pageSize,
            string? searchTerm,
            bool? isGraduated,
            bool? isActive,
            decimal? minGPA,
            decimal? maxGPA,
            string? sortBy,
            string? sortOrder
        );
    }
}