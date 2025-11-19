using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Attendance;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fap.Api.Controllers
{
    /// <summary>
    /// Student Attendance Endpoints - Add to StudentsController
    /// </summary>
    public partial class StudentsController
    {
        // ==================== STUDENT ATTENDANCE VIEW ====================

        /// <summary>
        /// GET /api/students/me/attendance - Get my attendance history
        /// </summary>
        [HttpGet("me/attendance")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyAttendance([FromQuery] AttendanceFilterRequest? filter)
        {
            try
            {
                var studentId = await GetCurrentStudentIdAsync();
                var attendance = await _attendanceService.GetStudentAttendanceAsync(studentId, filter);

                return Ok(new
                {
                    success = true,
                    message = $"Retrieved {attendance.Count} attendance records",
                    data = attendance
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting student attendance: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// GET /api/students/me/attendance/statistics - Get attendance statistics
        /// </summary>
        [HttpGet("me/attendance/statistics")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyAttendanceStatistics([FromQuery] Guid? classId = null)
        {
            try
            {
                var studentId = await GetCurrentStudentIdAsync();
                var stats = await _attendanceService.GetStudentAttendanceStatisticsAsync(studentId, classId);

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

        /// <summary>
        /// GET /api/students/me/attendance/class/{classId} - Get attendance for a specific class
        /// </summary>
        [HttpGet("me/attendance/class/{classId}")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> GetMyAttendanceByClass(Guid classId)
        {
            try
            {
                var studentId = await GetCurrentStudentIdAsync();
                var attendance = await _attendanceService.GetStudentAttendanceByClassAsync(studentId, classId);

                return Ok(new
                {
                    success = true,
                    data = attendance
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting class attendance: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// POST /api/students/me/attendance/{attendanceId}/excuse - Request excuse for absence
        /// </summary>
        [HttpPost("me/attendance/{attendanceId}/excuse")]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> RequestExcuse(Guid attendanceId, [FromBody] ExcuseAbsenceRequest request)
        {
            try
            {
                var userId = GetCurrentUserId();

                // Validate student can excuse this attendance
                if (!await _attendanceService.CanExcuseAbsenceAsync(attendanceId, userId))
                {
                    return Forbid();
                }

                var result = await _attendanceService.ExcuseAbsenceAsync(attendanceId, request);

                if (result == null)
                {
                    return NotFound(new { success = false, message = "Attendance record not found" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Excuse request submitted successfully",
                    data = result
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error requesting excuse: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }
    }
}
