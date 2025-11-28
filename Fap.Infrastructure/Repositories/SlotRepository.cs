using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class SlotRepository : GenericRepository<Slot>, ISlotRepository
    {
        public SlotRepository(FapDbContext context) : base(context) { }

        public async Task<Slot?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _dbSet
                .Include(s => s.Class)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Subject)
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Semester)
                .Include(s => s.TimeSlot)
                .Include(s => s.SubstituteTeacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Attendances)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Slot>> GetByClassIdAsync(Guid classId)
        {
            return await _dbSet
                .Include(s => s.TimeSlot)
                .Include(s => s.SubstituteTeacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Attendances)
                .Where(s => s.ClassId == classId)
                .OrderBy(s => s.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Slot>> GetByClassIdsAsync(List<Guid> classIds)
        {
            return await _dbSet
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Subject)
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Semester)
                .Include(s => s.Class)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.TimeSlot)
                .Include(s => s.SubstituteTeacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Attendances)
                .Where(s => classIds.Contains(s.ClassId))
                .OrderBy(s => s.Date)
                .ThenBy(s => s.TimeSlot.StartTime)
                .ToListAsync();
        }

        public async Task<IEnumerable<Slot>> GetByTeacherIdAsync(Guid teacherId)
        {
            return await _dbSet
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Subject)
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Semester)
                .Include(s => s.TimeSlot)
                .Include(s => s.SubstituteTeacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Attendances)
                .Where(s =>
                    s.Class.TeacherUserId == teacherId ||
                    s.SubstituteTeacherId == teacherId)
                .OrderBy(s => s.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Slot>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            return await _dbSet
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Subject)
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Semester)
                .Include(s => s.Class)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.TimeSlot)
                .Include(s => s.SubstituteTeacher)
                    .ThenInclude(t => t.User)
                .Include(s => s.Attendances)
                .Where(s => s.Date >= fromDate && s.Date <= toDate)
                .OrderBy(s => s.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Slot>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Subject)
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Semester)
                .Include(s => s.Class)
                    .ThenInclude(c => c.Teacher)
                        .ThenInclude(t => t.User)
                .Include(s => s.TimeSlot)
                .Include(s => s.SubstituteTeacher)
                    .ThenInclude(t => t.User)
                .Where(s => s.Status == status)
                .OrderBy(s => s.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Slot>> GetUpcomingSlotsAsync(Guid teacherId)
        {
            var today = DateTime.UtcNow.Date;

            return await _dbSet
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Subject)
                .Include(s => s.Class)
                    .ThenInclude(c => c.SubjectOffering)
                        .ThenInclude(so => so.Semester)
                .Include(s => s.TimeSlot)
                .Include(s => s.SubstituteTeacher)
                    .ThenInclude(t => t.User)
                .Where(s =>
                    (s.Class.TeacherUserId == teacherId ||
                     s.SubstituteTeacherId == teacherId) &&
                    s.Date >= today &&
                    s.Status == "Scheduled")
                .OrderBy(s => s.Date)
                .ThenBy(s => s.TimeSlot.StartTime)
                .Take(10)
                .ToListAsync();
        }

        public async Task<bool> HasOverlappingSlotAsync(
            Guid classId,
            DateTime date,
            Guid? timeSlotId,
            Guid? excludeSlotId = null)
        {
            var query = _dbSet
                .Where(s =>
                    s.ClassId == classId &&
                    s.Date.Date == date.Date &&
                    s.TimeSlotId == timeSlotId &&
                    s.Status != "Cancelled");

            if (excludeSlotId.HasValue)
            {
                query = query.Where(s => s.Id != excludeSlotId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<bool> HasTeacherConflictAsync(
            Guid teacherId,
            DateTime date,
            Guid? timeSlotId,
            Guid? excludeSlotId = null)
        {
            var query = _dbSet
                .Where(s =>
                    s.Date.Date == date.Date &&
                    s.TimeSlotId == timeSlotId &&
                    s.Status != "Cancelled" &&
                    (s.Class.TeacherUserId == teacherId || s.SubstituteTeacherId == teacherId));

            if (excludeSlotId.HasValue)
            {
                query = query.Where(s => s.Id != excludeSlotId.Value);
            }

            return await query.AnyAsync();
        }
    }
}
