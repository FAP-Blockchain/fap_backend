using Fap.Domain.DTOs.Attendance;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fap.Api.Interfaces
{
    public interface IAttendanceService
    {
        // Basic CRUD
        Task<AttendanceDto?> GetAttendanceByIdAsync(Guid id);
        Task<IEnumerable<AttendanceDto>> GetAttendancesBySlotIdAsync(Guid slotId);
        Task<IEnumerable<AttendanceDto>> GetAttendancesByClassIdAsync(Guid classId);
        Task<IEnumerable<AttendanceDto>> GetAttendancesByStudentIdAsync(Guid studentId);
        
        // Attendance Actions
        Task<IEnumerable<AttendanceDto>> TakeAttendanceAsync(TakeAttendanceRequest request);
        Task<AttendanceDto?> UpdateAttendanceAsync(Guid id, UpdateAttendanceRequest request);
        Task<AttendanceDto?> ExcuseAbsenceAsync(Guid id, ExcuseAbsenceRequest request);
        
        // Statistics & Reports
        Task<AttendanceStatisticsDto> GetStudentAttendanceStatisticsAsync(Guid studentId, Guid? classId = null);
        Task<ClassAttendanceReportDto> GetClassAttendanceReportAsync(Guid classId);
        Task<IEnumerable<AttendanceDto>> GetAttendancesByFilterAsync(AttendanceFilterRequest filter);
        
        // Validation
        Task<bool> CanTakeAttendanceAsync(Guid slotId, Guid teacherUserId);
        Task<bool> CanExcuseAbsenceAsync(Guid attendanceId, Guid studentUserId);
    }
}
