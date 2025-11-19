using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface ISubjectOfferingRepository : IGenericRepository<SubjectOffering>
    {
        Task<SubjectOffering?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<SubjectOffering>> GetBySubjectIdAsync(Guid subjectId);
        Task<IEnumerable<SubjectOffering>> GetBySemesterIdAsync(Guid semesterId);
        Task<SubjectOffering?> GetBySubjectAndSemesterAsync(Guid subjectId, Guid semesterId);
        Task<bool> ExistsAsync(Guid subjectId, Guid semesterId);
    }
}
