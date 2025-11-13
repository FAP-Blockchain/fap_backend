using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Enrollment;
using Fap.Domain.DTOs.Grade;
using Fap.Domain.DTOs.Student;
using Fap.Domain.DTOs.Attendance;
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
        private readonly IEnrollmentService _enrollmentService;
        private readonly IGradeService _gradeService;
        private readonly IAttendanceService _attendanceService;
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(
     IStudentService studentService,
        IEnrollmentService enrollmentService,
   IGradeService gradeService,
   IAttendanceService attendanceService,
    ILogger<StudentsController> logger)
        {
            _studentService = studentService;
     _enrollmentService = enrollmentService;
            _gradeService = gradeService;
   _attendanceService = attendanceService;
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
                _logger.LogError($"Error getting students: {ex.Message}");
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
          _logger.LogError($"Error getting student {id}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving student" });
            }
 }

        /// <summary>
        /// GET /api/students/{id}/enrollments - Get student's enrollment history
   /// </summary>
        [HttpGet("{id}/enrollments")]
        public async Task<IActionResult> GetStudentEnrollments(Guid id, [FromQuery] GetStudentEnrollmentsRequest request)
        {
            try
            {
     var result = await _enrollmentService.GetStudentEnrollmentHistoryAsync(id, request);
    return Ok(result);
            }
    catch (Exception ex)
    {
        _logger.LogError($"Error getting enrollments for student {id}: {ex.Message}");
   return StatusCode(500, new { message = "An error occurred while retrieving student enrollments" });
      }
   }

     /// <summary>
        /// GET /api/students/{id}/grades - Get student grade transcript
  /// </summary>
        [HttpGet("{id}/grades")]
        public async Task<IActionResult> GetStudentGrades(Guid id, [FromQuery] GetStudentGradesRequest request)
        {
 try
     {
                var result = await _gradeService.GetStudentGradesAsync(id, request);

      if (result == null)
       return NotFound(new { message = $"Student with ID {id} not found" });

      return Ok(result);
            }
      catch (Exception ex)
        {
       _logger.LogError($"Error getting grades for student {id}: {ex.Message}");
     return StatusCode(500, new { message = "An error occurred while retrieving student grades" });
   }
        }

        /// <summary>
        /// GET /api/students/{id}/attendance - Get attendance history for a student
        /// </summary>
     [HttpGet("{id}/attendance")]
        public async Task<IActionResult> GetStudentAttendanceHistory(Guid id)
        {
 try
            {
       var result = await _attendanceService.GetAttendancesByStudentIdAsync(id);
     return Ok(new
        {
          success = true,
         message = $"Retrieved {result.Count()} attendance records",
   data = result
                });
            }
            catch (Exception ex)
            {
  _logger.LogError($"Error getting attendance for student {id}: {ex.Message}");
     return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
         }
 }

     /// <summary>
        /// GET /api/students/{id}/attendance/statistics - Get attendance statistics for a student
    /// </summary>
        [HttpGet("{id}/attendance/statistics")]
        public async Task<IActionResult> GetStudentAttendanceStatistics(Guid id, [FromQuery] Guid? classId = null)
        {
            try
         {
       var result = await _attendanceService.GetStudentAttendanceStatisticsAsync(id, classId);
       return Ok(new
              {
      success = true,
    data = result
    });
            }
   catch (InvalidOperationException ex)
      {
      return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
         _logger.LogError($"Error getting attendance statistics for student {id}: {ex.Message}");
return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
         }
        }
    }
}
