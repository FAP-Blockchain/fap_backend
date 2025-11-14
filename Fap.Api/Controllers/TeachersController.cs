using Fap.Api.Extensions;
using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Teacher;
using Fap.Domain.DTOs.Slot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherService _teacherService;
   private readonly ISlotService _slotService;
        private readonly ILogger<TeachersController> _logger;

        public TeachersController(
   ITeacherService teacherService,
         ISlotService slotService,
 ILogger<TeachersController> logger)
{
            _teacherService = teacherService;
    _slotService = slotService;
       _logger = logger;
}

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
                var userId = User.GetRequiredUserId();
                var teacher = await _teacherService.GetTeacherByUserIdAsync(userId);
                if (teacher == null)
                {
                    return NotFound(new { message = "Teacher profile not found for current user" });
                }

                return Ok(teacher);
            }
            catch (InvalidOperationException)
            {
                return Unauthorized(new { message = "Unable to identify current user" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting current teacher profile: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving teacher profile" });
            }
        }

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
