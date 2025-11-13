using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories;

public class AttendanceRepository : GenericRepository<Attendance>, IAttendanceRepository
{
    public AttendanceRepository(FapDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Attendance>> GetBySlotIdAsync(Guid slotId)
    {
        return await _context.Attendances
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Subject)
            .Include(a => a.Slot)
                .ThenInclude(s => s.TimeSlot)
            .Include(a => a.Slot)
                .ThenInclude(s => s.Class)
            .Where(a => a.SlotId == slotId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByClassIdAsync(Guid classId)
    {
        return await _context.Attendances
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Subject)
            .Include(a => a.Slot)
                .ThenInclude(s => s.Class)
            .Include(a => a.Slot)
                .ThenInclude(s => s.TimeSlot)
            .Where(a => a.Slot.ClassId == classId)
            .OrderByDescending(a => a.Slot.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByStudentIdAsync(Guid studentId)
    {
        return await _context.Attendances
            .Include(a => a.Subject)
            .Include(a => a.Slot)
                .ThenInclude(s => s.Class)
            .Include(a => a.Slot)
                .ThenInclude(s => s.TimeSlot)
            .Where(a => a.StudentId == studentId)
            .OrderByDescending(a => a.Slot.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetBySubjectIdAsync(Guid subjectId)
    {
        return await _context.Attendances
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Subject)
            .Include(a => a.Slot)
                .ThenInclude(s => s.Class)
            .Include(a => a.Slot)
                .ThenInclude(s => s.TimeSlot)
            .Where(a => a.SubjectId == subjectId)
            .OrderByDescending(a => a.Slot.Date)
            .ToListAsync();
    }

    public async Task<IEnumerable<Attendance>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate)
    {
        return await _context.Attendances
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Subject)
            .Include(a => a.Slot)
                .ThenInclude(s => s.Class)
            .Include(a => a.Slot)
                .ThenInclude(s => s.TimeSlot)
            .Where(a => a.Slot.Date >= fromDate && a.Slot.Date <= toDate)
            .OrderByDescending(a => a.Slot.Date)
            .ToListAsync();
    }

    public async Task<Attendance?> GetByStudentAndSlotAsync(Guid studentId, Guid slotId)
    {
        return await _context.Attendances
            .Include(a => a.Student)
                .ThenInclude(s => s.User)
            .Include(a => a.Subject)
            .Include(a => a.Slot)
                .ThenInclude(s => s.Class)
            .Include(a => a.Slot)
                .ThenInclude(s => s.TimeSlot)
            .FirstOrDefaultAsync(a => a.StudentId == studentId && a.SlotId == slotId);
    }

    public async Task<bool> HasAttendanceForSlotAsync(Guid slotId)
    {
        return await _context.Attendances
            .AnyAsync(a => a.SlotId == slotId);
    }

    public async Task<int> CountPresentByStudentAsync(Guid studentId, Guid? classId = null)
    {
        var query = _context.Attendances
            .Where(a => a.StudentId == studentId && a.IsPresent);

        if (classId.HasValue)
        {
            query = query.Where(a => a.Slot.ClassId == classId.Value);
        }

        return await query.CountAsync();
    }

    public async Task<int> CountAbsentByStudentAsync(Guid studentId, Guid? classId = null)
    {
        var query = _context.Attendances
            .Where(a => a.StudentId == studentId && !a.IsPresent);

        if (classId.HasValue)
        {
            query = query.Where(a => a.Slot.ClassId == classId.Value);
        }

        return await query.CountAsync();
    }

    public async Task<int> CountExcusedByStudentAsync(Guid studentId, Guid? classId = null)
    {
        var query = _context.Attendances
            .Where(a => a.StudentId == studentId && !a.IsPresent && a.IsExcused);

        if (classId.HasValue)
        {
            query = query.Where(a => a.Slot.ClassId == classId.Value);
        }

        return await query.CountAsync();
    }
}
