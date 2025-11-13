using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface ISlotRepository : IGenericRepository<Slot>
    {
        Task<Slot?> GetByIdWithDetailsAsync(Guid id);
        Task<IEnumerable<Slot>> GetByClassIdAsync(Guid classId);
        Task<IEnumerable<Slot>> GetByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<Slot>> GetByDateRangeAsync(DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Slot>> GetByStatusAsync(string status);
        Task<IEnumerable<Slot>> GetUpcomingSlotsAsync(Guid teacherId);
        Task<bool> HasOverlappingSlotAsync(Guid classId, DateTime date, Guid? timeSlotId, Guid? excludeSlotId = null);
    }
}
