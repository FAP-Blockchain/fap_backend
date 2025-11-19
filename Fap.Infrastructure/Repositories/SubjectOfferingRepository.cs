using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class SubjectOfferingRepository : GenericRepository<SubjectOffering>, ISubjectOfferingRepository
    {
        public SubjectOfferingRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<SubjectOffering?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _context.SubjectOfferings
          .Include(so => so.Subject)
             .Include(so => so.Semester)
       .Include(so => so.Classes)
    .ThenInclude(c => c.Teacher)
  .ThenInclude(t => t.User)
         .Include(so => so.Classes)
            .ThenInclude(c => c.Members)
.FirstOrDefaultAsync(so => so.Id == id);
        }

        public async Task<IEnumerable<SubjectOffering>> GetBySubjectIdAsync(Guid subjectId)
     {
       return await _context.SubjectOfferings
     .Include(so => so.Subject)
          .Include(so => so.Semester)
    .Include(so => so.Classes)
      .Where(so => so.SubjectId == subjectId)
      .ToListAsync();
        }

      public async Task<IEnumerable<SubjectOffering>> GetBySemesterIdAsync(Guid semesterId)
        {
            return await _context.SubjectOfferings
      .Include(so => so.Subject)
          .Include(so => so.Semester)
                .Include(so => so.Classes)
   .Where(so => so.SemesterId == semesterId)
                .ToListAsync();
        }

        public async Task<SubjectOffering?> GetBySubjectAndSemesterAsync(Guid subjectId, Guid semesterId)
  {
            return await _context.SubjectOfferings
            .Include(so => so.Subject)
           .Include(so => so.Semester)
              .Include(so => so.Classes)
         .FirstOrDefaultAsync(so => so.SubjectId == subjectId && so.SemesterId == semesterId);
        }

        public async Task<bool> ExistsAsync(Guid subjectId, Guid semesterId)
        {
            return await _context.SubjectOfferings
                .AnyAsync(so => so.SubjectId == subjectId && so.SemesterId == semesterId);
     }
    }
}
