using Fap.Domain.Entities;

namespace Fap.Domain.Repositories
{
    public interface ITimeSlotRepository : IGenericRepository<TimeSlot>
    {
        Task<TimeSlot?> GetByNameAsync(string name);
        Task<IEnumerable<TimeSlot>> GetAllWithSlotsAsync();
        Task<bool> IsTimeSlotOverlapping(TimeSpan startTime, TimeSpan endTime, Guid? excludeId = null);
    }
}
