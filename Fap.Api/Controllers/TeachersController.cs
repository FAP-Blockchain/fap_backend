using Fap.Api.Extensions;
using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Teacher;
using Fap.Domain.DTOs.Slot;
using Fap.Domain.DTOs.Schedule;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Fap.Api.Controllers
{
    [ApiController]
    [Route("api/teachers")]
    [Authorize]
    public partial class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
        private readonly ISlotService _slotService;
        private readonly IScheduleService _scheduleService;
        private readonly IAttendanceService _attendanceService;
        private readonly ILogger<TeachersController> _logger;

        public TeachersController(
         ITeacherService teacherService,
         ISlotService slotService,
         IScheduleService scheduleService,
         IAttendanceService attendanceService,
         ILogger<TeachersController> logger)
        {
            _teacherService = teacherService;
            _slotService = slotService;
            _scheduleService = scheduleService;
            _attendanceService = attendanceService;
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

        // ==================== BASIC CRUD ====================

        [HttpGet]
        public async Task<IActionResult> GetAllTeachers([FromQuery] GetTeachersRequest request)
        {
            try
            {
                var result = await _teacherService.GetTeachersAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting teachers: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving teachers" });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeacherById(Guid id)
        {
            try
            {
                var teacher = await _teacherService.GetTeacherByIdAsync(id);

                if (teacher == null)
                    return NotFound(new
                    {
                        success = false,
                        message = $"Teacher with ID '{id}' not found"
                    });

                return Ok(teacher);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting teacher {id}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving teacher" });
            }
        }

        [HttpGet("me")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetCurrentTeacherProfile()
        {
            try
            {
                var userId = GetCurrentUserId();
                var teacher = await _teacherService.GetTeacherByUserIdAsync(userId);

                if (teacher == null)
                {
                    return NotFound(new { message = "Teacher profile not found for current user" });
                }

                return Ok(teacher);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting current teacher profile: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving teacher profile" });
            }
        }

        // ==================== TEACHER SCHEDULE (Simplified) ====================

        /// <summary>
        /// GET /api/teachers/me/schedule - Get current teacher's weekly schedule
        /// If no date provided, returns current week
        /// </summary>
        [HttpGet("me/schedule")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetMyWeeklySchedule([FromQuery] DateTime? weekStartDate)
        {
            try
            {
                var teacherId = await GetCurrentTeacherIdAsync();

                // If no date provided, get Monday of current week
                var startDate = weekStartDate ?? GetMonday(DateTime.UtcNow);

                var request = new GetWeeklyScheduleRequest
                {
                    WeekStartDate = startDate,
                    IncludeAttendance = true
                };

                var schedule = await _scheduleService.GetTeacherWeeklyScheduleAsync(teacherId, request);

                return Ok(new
                {
                    success = true,
                    data = schedule
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting teacher schedule: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        /// <summary>
        /// GET /api/teachers/me/schedule/statistics - Get schedule statistics for current semester
        /// </summary>
        [HttpGet("me/schedule/statistics")]
        [Authorize(Roles = "Teacher")]
        public async Task<IActionResult> GetScheduleStatistics([FromQuery] Guid? semesterId = null)
        {
            try
            {
                var userId = GetCurrentUserId();
                var statistics = await _scheduleService.GetScheduleStatisticsAsync(userId, semesterId);

                return Ok(new
                {
                    success = true,
                    data = statistics
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting schedule statistics: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred" });
            }
        }

        // Helper method to get Monday of the week
        private DateTime GetMonday(DateTime date)
        {
            var dayOfWeek = (int)date.DayOfWeek;
            // If Sunday (0), go back 6 days, otherwise go back (dayOfWeek - 1) days
            var daysToSubtract = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
            return date.Date.AddDays(-daysToSubtract);
        }

        // ==================== EXISTING ENDPOINTS (keep as is) ====================

        /// <summary>
        /// GET /api/teachers/{id}/slots - Get all slots for a teacher
        /// </summary>
        [HttpGet("{id}/slots")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> GetTeacherSlots(Guid id)
        {
            try
            {
                var result = await _slotService.GetSlotsByTeacherIdAsync(id);
                return Ok(new
                {
                    success = true,
                    message = $"Retrieved {result.Count()} slots",
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting slots for teacher {id}: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }
    }
}
