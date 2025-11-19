using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface IClassRepository : IGenericRepository<Class>
    {
        Task<Class?> GetByClassCodeAsync(string classCode);
        Task<Class?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Class>> GetAllWithDetailsAsync();
        Task<(List<Class> Classes, int TotalCount)> GetPagedClassesAsync(
            int page,
            int pageSize,
            string? subjectId,
            string? teacherId,
            string? semesterId,
            string? classCode,
            string? sortBy,
            string? sortOrder
        );
        Task<List<ClassMember>> GetClassRosterAsync(Guid classId);
        Task<bool> IsClassCodeUniqueAsync(string classCode, Guid? excludeId = null);
        
        // ==================== NEW METHODS ====================
        Task<bool> IsStudentInClassAsync(Guid classId, Guid studentId);
        Task<ClassMember?> GetClassMemberAsync(Guid classId, Guid studentId);
        Task AddStudentToClassAsync(ClassMember classMember);
        Task RemoveStudentFromClassAsync(ClassMember classMember);
        Task<int> GetCurrentStudentCountAsync(Guid classId);
    }
}
