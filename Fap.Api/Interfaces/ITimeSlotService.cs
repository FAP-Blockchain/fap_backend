using Fap.Domain.DTOs.TimeSlot;

namespace Fap.Api.Interfaces
{
    public interface ITimeSlotService
    {
        Task<List<TimeSlotDto>> GetAllTimeSlotsAsync();
        Task<TimeSlotDto?> GetTimeSlotByIdAsync(Guid id);
        Task<TimeSlotResponse> CreateTimeSlotAsync(CreateTimeSlotRequest request);
        Task<TimeSlotResponse> UpdateTimeSlotAsync(Guid id, UpdateTimeSlotRequest request);
        Task<TimeSlotResponse> DeleteTimeSlotAsync(Guid id);
    }
}
