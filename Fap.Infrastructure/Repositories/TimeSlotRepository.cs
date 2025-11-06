using Fap.Domain.Entities;
using Fap.Domain.Repositories;
using Fap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fap.Infrastructure.Repositories
{
    public class TimeSlotRepository : GenericRepository<TimeSlot>, ITimeSlotRepository
    {
        public TimeSlotRepository(FapDbContext context) : base(context)
        {
        }

        public async Task<TimeSlot?> GetByNameAsync(string name)
        {
            return await _dbSet
                .Include(ts => ts.Slots)
                .FirstOrDefaultAsync(ts => ts.Name == name);
        }

        public async Task<IEnumerable<TimeSlot>> GetAllWithSlotsAsync()
        {
            return await _dbSet
                .Include(ts => ts.Slots)
                .OrderBy(ts => ts.StartTime)
                .ToListAsync();
        }

        public async Task<bool> IsTimeSlotOverlapping(TimeSpan startTime, TimeSpan endTime, Guid? excludeId = null)
        {
            var query = _dbSet.AsQueryable();

            if (excludeId.HasValue)
            {
                query = query.Where(ts => ts.Id != excludeId.Value);
            }

            // Check if there's any overlap
            var overlapping = await query.AnyAsync(ts =>
                (startTime >= ts.StartTime && startTime < ts.EndTime) ||  // Start time overlaps
                (endTime > ts.StartTime && endTime <= ts.EndTime) ||      // End time overlaps
                (startTime <= ts.StartTime && endTime >= ts.EndTime)      // Completely encompasses
            );

            return overlapping;
        }
    }
}
