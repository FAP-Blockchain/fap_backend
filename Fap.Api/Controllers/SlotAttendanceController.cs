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
    /// Slot Attendance API - RESTful attendance management for slots
    /// </summary>
    [Route("api/slots")]
    [ApiController]
    [Authorize]
    public class SlotAttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;
        private readonly IScheduleService _scheduleService;
        private readonly ILogger<SlotAttendanceController> _logger;

        public SlotAttendanceController(
           IAttendanceService attendanceService,
           IScheduleService scheduleService,
               ILogger<SlotAttendanceController> logger)
        {
            _attendanceService = attendanceService;
            _scheduleService = scheduleService;
            _logger = logger;
        }

        // ==================== HELPER METHODS ====================

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Invalid user token");
            }
            return userId;
        }

        private async Task<Guid> GetCurrentTeacherIdAsync()
        {
            var userId = GetCurrentUserId();
            var teacherId = await _scheduleService.GetTeacherIdByUserIdAsync(userId);
            if (!teacherId.HasValue)
            {
                throw new InvalidOperationException("Current user is not a teacher");
            }
            return teacherId.Value;
        }

        // ==================== SLOT-BASED ATTENDANCE (RESTful) ====================

        /// <summary>
        /// POST /api/slots/{slotId}/attendance - Take attendance for a slot
        /// </summary>
        [HttpPost("{slotId}/attendance")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> TakeAttendance(
          Guid slotId,
       [FromBody] TakeSlotAttendanceRequest request)
        {
            try
            {
                var teacherId = await GetCurrentTeacherIdAsync();

                // Validate authorization (unless admin)
                if (!User.IsInRole("Admin"))
                {
                    if (!await _attendanceService.CanTakeAttendanceForSlotAsync(slotId, teacherId))
                    {
                        return Forbid();
                    }
                }

                var result = await _attendanceService.TakeAttendanceForSlotAsync(slotId, request);

                return Ok(new
                {
                    success = true,
                    message = $"Attendance taken for {result.StudentAttendances.Count} students",
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
                _logger.LogError($"Error taking attendance for slot {slotId}: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// GET /api/slots/{slotId}/attendance - Get attendance for a slot
        /// </summary>
        [HttpGet("{slotId}/attendance")]
        [Authorize(Roles = "Teacher,Admin,Student")]
        public async Task<IActionResult> GetSlotAttendance(Guid slotId)
        {
            try
            {
                var result = await _attendanceService.GetSlotAttendanceAsync(slotId);

                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Slot not found or no attendance taken yet"
                    });
                }

                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting attendance for slot {slotId}: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// PUT /api/slots/{slotId}/attendance - Update attendance for a slot
        /// </summary>
        [HttpPut("{slotId}/attendance")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> UpdateAttendance(
       Guid slotId,
       [FromBody] UpdateSlotAttendanceRequest request)
        {
            try
            {
                var teacherId = await GetCurrentTeacherIdAsync();

                if (!User.IsInRole("Admin"))
                {
                    if (!await _attendanceService.CanTakeAttendanceForSlotAsync(slotId, teacherId))
                    {
                        return Forbid();
                    }
                }

                var result = await _attendanceService.UpdateAttendanceForSlotAsync(slotId, request);

                return Ok(new
                {
                    success = true,
                    message = "Attendance updated successfully",
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
                _logger.LogError($"Error updating attendance for slot {slotId}: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// DELETE /api/slots/{slotId}/attendance - Delete attendance for a slot (if wrong)
        /// </summary>
        [HttpDelete("{slotId}/attendance")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> DeleteAttendance(Guid slotId)
        {
            try
            {
                var teacherId = await GetCurrentTeacherIdAsync();

                if (!User.IsInRole("Admin"))
                {
                    if (!await _attendanceService.CanTakeAttendanceForSlotAsync(slotId, teacherId))
                    {
                        return Forbid();
                    }
                }

                var result = await _attendanceService.DeleteSlotAttendanceAsync(slotId);

                if (!result)
                {
                    return NotFound(new { success = false, message = "No attendance found for this slot" });
                }

                return Ok(new
                {
                    success = true,
                    message = "Attendance deleted successfully"
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting attendance for slot {slotId}: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        // ==================== QUICK ACTIONS ====================

        /// <summary>
        /// POST /api/slots/{slotId}/attendance/mark-all-present - Mark all students as present
        /// </summary>
        [HttpPost("{slotId}/attendance/mark-all-present")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> MarkAllPresent(Guid slotId)
        {
            try
            {
                var teacherId = await GetCurrentTeacherIdAsync();

                if (!User.IsInRole("Admin"))
                {
                    if (!await _attendanceService.CanTakeAttendanceForSlotAsync(slotId, teacherId))
                    {
                        return Forbid();
                    }
                }

                var result = await _attendanceService.MarkAllPresentForSlotAsync(slotId);

                return Ok(new
                {
                    success = true,
                    message = $"Marked {result.TotalStudents} students as present",
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
                _logger.LogError($"Error marking all present for slot {slotId}: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// POST /api/slots/{slotId}/attendance/mark-all-absent - Mark all students as absent
        /// </summary>
        [HttpPost("{slotId}/attendance/mark-all-absent")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> MarkAllAbsent(Guid slotId)
        {
            try
            {
                var teacherId = await GetCurrentTeacherIdAsync();

                if (!User.IsInRole("Admin"))
                {
                    if (!await _attendanceService.CanTakeAttendanceForSlotAsync(slotId, teacherId))
                    {
                        return Forbid();
                    }
                }

                var result = await _attendanceService.MarkAllAbsentForSlotAsync(slotId);

                return Ok(new
                {
                    success = true,
                    message = $"Marked {result.TotalStudents} students as absent",
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
                _logger.LogError($"Error marking all absent for slot {slotId}: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }
    }
}
