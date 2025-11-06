using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        Task<Subject?> GetBySubjectCodeAsync(string subjectCode);
        Task<Subject?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Subject>> GetAllWithDetailsAsync();
        Task<IEnumerable<Subject>> GetBySemesterIdAsync(Guid semesterId);
    }
}
