using Fap.Domain.Entities;

namespace Fap.Domain.Repositories;

public interface IAttendanceRepository : IGenericRepository<Attendance>
{
    Task<IEnumerable<Attendance>> GetBySlotIdAsync(Guid slotId);
    Task<IEnumerable<Attendance>> GetByClassIdAsync(Guid classId);
    Task<IEnumerable<Attendance>> GetByStudentIdAsync(Guid studentId);
    Task<IEnumerable<Attendance>> GetBySubjectIdAsync(Guid subjectId);
    Task<IEnumerable<Attendance>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
    Task<Attendance?> GetByStudentAndSlotAsync(Guid studentId, Guid slotId);
    Task<bool> HasAttendanceForSlotAsync(Guid slotId);
    Task<int> CountPresentByStudentAsync(Guid studentId, Guid? classId = null);
    Task<int> CountAbsentByStudentAsync(Guid studentId, Guid? classId = null);
    Task<int> CountExcusedByStudentAsync(Guid studentId, Guid? classId = null);
}
