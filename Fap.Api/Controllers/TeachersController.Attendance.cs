using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Attendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fap.Api.Controllers
{
    /// <summary>
    /// Teacher Attendance Endpoints - Add to TeachersController
    /// </summary>
    public partial class TeachersController
    {
        // Note: Add IAttendanceService to TeachersController constructor if not already there

        // ==================== TEACHER ATTENDANCE VIEW ====================

        /// <summary>
        /// GET /api/teachers/me/slots/pending-attendance - Get slots that need attendance
        /// </summary>
        [HttpGet("me/slots/pending-attendance")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetPendingAttendanceSlots()
        {
            try
            {
                var teacherId = await GetCurrentTeacherIdAsync();
                var pendingSlots = await _attendanceService.GetPendingAttendanceSlotsAsync(teacherId);

                return Ok(new
                {
                    success = true,
                    message = $"Found {pendingSlots.Count} slot(s) needing attendance",
                    data = pendingSlots
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting pending attendance slots: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// GET /api/teachers/me/attendance/statistics - Get attendance statistics for my classes
        /// </summary>
        [HttpGet("me/attendance/statistics")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMyAttendanceStatistics([FromQuery] Guid? classId = null)
        {
            try
            {
                var teacherId = await GetCurrentTeacherIdAsync();
                var stats = await _attendanceService.GetTeacherAttendanceStatisticsAsync(teacherId, classId);

                return Ok(new
                {
                    success = true,
                    data = stats
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting attendance statistics: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }
    }
}
