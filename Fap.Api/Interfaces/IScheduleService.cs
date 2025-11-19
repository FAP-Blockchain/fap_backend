using Fap.Domain.DTOs.Schedule;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fap.Api.Interfaces
{
    public interface IScheduleService
    {
        // ==================== HELPER METHODS ====================
 
        /// <summary>
        /// Get Teacher ID from User ID
        /// </summary>
        Task<Guid?> GetTeacherIdByUserIdAsync(Guid userId);
        
        /// <summary>
        /// Get Student ID from User ID
        /// </summary>
        Task<Guid?> GetStudentIdByUserIdAsync(Guid userId);

        // ==================== TEACHER SCHEDULE ====================

        /// <summary>
        /// Get teacher's schedule for a specific date or date range
        /// </summary>
        Task<List<ScheduleItemDto>> GetTeacherScheduleAsync(Guid teacherId, GetScheduleRequest request);

        /// <summary>
        /// Get teacher's weekly schedule
        /// </summary>
        Task<WeeklyScheduleDto> GetTeacherWeeklyScheduleAsync(Guid teacherId, GetWeeklyScheduleRequest request);

        /// <summary>
        /// Get teacher's daily schedule
        /// </summary>
        Task<DailyScheduleDto> GetTeacherDailyScheduleAsync(Guid teacherId, DateTime date, bool includeAttendance = true);

        /// <summary>
        /// Get teacher's semester schedule overview
        /// </summary>
        Task<SemesterScheduleDto> GetTeacherSemesterScheduleAsync(Guid teacherId, Guid semesterId);

        // ==================== STUDENT SCHEDULE ====================

        /// <summary>
        /// Get student's schedule for a specific date or date range
        /// </summary>
        Task<List<ScheduleItemDto>> GetStudentScheduleAsync(Guid studentId, GetScheduleRequest request);

        /// <summary>
        /// Get student's weekly schedule
        /// </summary>
        Task<WeeklyScheduleDto> GetStudentWeeklyScheduleAsync(Guid studentId, GetWeeklyScheduleRequest request);

        /// <summary>
        /// Get student's daily schedule
        /// </summary>
        Task<DailyScheduleDto> GetStudentDailyScheduleAsync(Guid studentId, DateTime date, bool includeAttendance = true);

        /// <summary>
        /// Get student's semester schedule overview
        /// </summary>
        Task<SemesterScheduleDto> GetStudentSemesterScheduleAsync(Guid studentId, Guid semesterId);

        // ==================== CURRENT USER SCHEDULE ====================

        /// <summary>
        /// Get current authenticated user's schedule
        /// </summary>
        Task<List<ScheduleItemDto>> GetMyScheduleAsync(Guid userId, GetScheduleRequest request);

        /// <summary>
        /// Get current user's weekly schedule
        /// </summary>
        Task<WeeklyScheduleDto> GetMyWeeklyScheduleAsync(Guid userId, GetWeeklyScheduleRequest request);

        /// <summary>
        /// Get current user's daily schedule
        /// </summary>
        Task<DailyScheduleDto> GetMyDailyScheduleAsync(Guid userId, DateTime date, bool includeAttendance = true);

        // ==================== SCHEDULE UTILITIES ====================

        /// <summary>
        /// Check for schedule conflicts
        /// </summary>
        Task<List<ScheduleConflictDto>> CheckScheduleConflictsAsync(Guid userId, DateTime fromDate, DateTime toDate);

        /// <summary>
        /// Get schedule statistics
        /// </summary>
        Task<ScheduleStatisticsDto> GetScheduleStatisticsAsync(Guid userId, Guid? semesterId = null);
    }
}
