using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Teacher;
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
        private readonly ILogger<TeachersController> _logger;

        public TeachersController(ITeacherService teacherService, ILogger<TeachersController> logger)
        {
            _teacherService = teacherService;
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
                _logger.LogError($"? Error getting teachers: {ex.Message}");
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
                _logger.LogError($"? Error getting teacher {id}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving teacher" });
            }
        }
    }
}
