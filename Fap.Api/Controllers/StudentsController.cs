using Fap.Api.Interfaces;
using Fap.Api.Services;
using Fap.Domain.DTOs.Student;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents([FromQuery] GetStudentsRequest request)

        { 
            try
            {
                var results = await _studentService.GetStudentsAsync(request);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting students: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving students" });
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);

                if (student == null)
                    return NotFound(new
                    {
                        success = false,
                        message = $"Student with ID '{id}' not found"
                    });

                return Ok(student);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting student {id}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving student" });
            }
        }
    }
}
