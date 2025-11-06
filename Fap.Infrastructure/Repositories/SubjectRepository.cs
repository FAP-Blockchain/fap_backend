using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<Subject?> GetBySubjectCodeAsync(string subjectCode)
        {
            return await _dbSet
                .Include(s => s.Semester)
                .FirstOrDefaultAsync(s => s.SubjectCode == subjectCode);
        }

        public async Task<Subject?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(s => s.Semester)
                .Include(s => s.Classes)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.Classes)
                    .ThenInclude(c => c.Members)
                .Include(s => s.SubjectCriterias)
                .Include(s => s.Roadmaps)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Subject>> GetAllWithDetailsAsync()
        {
            return await _dbSet
                .Include(s => s.Semester)
                .Include(s => s.Classes)
                .OrderBy(s => s.SubjectCode)
                .ToListAsync();
        }

        public async Task<IEnumerable<Subject>> GetBySemesterIdAsync(Guid semesterId)
        {
            return await _dbSet
                .Include(s => s.Semester)
                .Include(s => s.Classes)
                .Where(s => s.SemesterId == semesterId)
                .OrderBy(s => s.SubjectCode)
                .ToListAsync();
        }
    }
}
