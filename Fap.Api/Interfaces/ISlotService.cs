using Fap.Domain.DTOs.Slot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fap.Api.Interfaces
{
    public interface ISlotService
    {
        // Basic CRUD
        Task<SlotDto?> GetSlotByIdAsync(Guid id);
        Task<IEnumerable<SlotDto>> GetSlotsByClassIdAsync(Guid classId);
        Task<IEnumerable<SlotDto>> GetSlotsByTeacherIdAsync(Guid teacherId);
        Task<SlotDto> CreateSlotAsync(CreateSlotRequest request);
        Task<SlotDto?> UpdateSlotAsync(Guid id, UpdateSlotRequest request);
        Task<bool> DeleteSlotAsync(Guid id);

        // Status management
        Task<SlotDto?> UpdateSlotStatusAsync(Guid id, UpdateSlotStatusRequest request);
        Task<SlotDto?> CompleteSlotAsync(Guid id);
        Task<SlotDto?> CancelSlotAsync(Guid id, string reason);

        // Query & Filter
        Task<IEnumerable<SlotDto>> GetSlotsByFilterAsync(SlotFilterRequest filter);
        Task<IEnumerable<SlotDto>> GetUpcomingSlotsAsync(Guid teacherId);
        Task<IEnumerable<SlotDto>> GetSlotsNeedingAttendanceAsync(Guid teacherId);

        // Validation
        Task<bool> CanModifySlotAsync(Guid slotId, Guid teacherUserId);
        Task<bool> CanDeleteSlotAsync(Guid slotId);
    }
}
