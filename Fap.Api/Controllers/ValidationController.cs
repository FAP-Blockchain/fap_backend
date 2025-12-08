using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Validation;
using Fap.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Fap.Api.Controllers
{
    [Route("api/validation")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ValidationController : ControllerBase
    {
        private readonly IValidationService _validationService;
        private readonly FapDbContext _context;
        private readonly ILogger<ValidationController> _logger;

        public ValidationController(
            IValidationService validationService, 
            FapDbContext context,
            ILogger<ValidationController> logger)
        {
            _validationService = validationService;
            _context = context;
            _logger = logger;
        }

        [HttpGet("latest_credential")]
        public async Task<IActionResult> GetLatestCredential()
        {
            var credential = await _context.Credentials
                .OrderByDescending(c => c.IssuedDate)
                .FirstOrDefaultAsync();

            if (credential == null)
            {
                return NotFound(new { success = false, message = "No credentials found." });
            }

            return Ok(new
            {
                success = true,
                data = new
                {
                    id = credential.Id,
                    studentId = credential.StudentId,
                    fileUrl = credential.FileUrl,
                    ipfsHash = credential.IPFSHash,
                    issuedDate = credential.IssuedDate,
                    isOnBlockchain = credential.IsOnBlockchain
                }
            });
        }

        [HttpPut("tamper_credential")]
        public async Task<IActionResult> TamperLatestCredential([FromBody] TamperCredentialRequest request)
        {
            var credential = await _context.Credentials
                .OrderByDescending(c => c.IssuedDate)
                .FirstOrDefaultAsync();

            if (credential == null)
            {
                return NotFound(new { success = false, message = "No credentials found to tamper." });
            }

            if (string.IsNullOrEmpty(request.FileUrl))
            {
                return BadRequest(new { success = false, message = "FileUrl is required." });
            }

            // Tamper with the data
            // We change the FileUrl and IPFSHash so that off-chain data no longer matches on-chain hash
            var originalUrl = credential.FileUrl;
            var originalHash = credential.IPFSHash;

            // Point to the user-provided file
            credential.FileUrl = request.FileUrl;
            // Use provided hash or a default fake one
            credential.IPFSHash = !string.IsNullOrEmpty(request.IPFSHash) 
                ? request.IPFSHash 
                : "QmTamperedHash1234567890TamperedHash1234567890"; 
            
            await _context.SaveChangesAsync();

            _logger.LogWarning("Tampered with credential {Id}. Original URL: {OrigUrl}, New URL: {NewUrl}", 
                credential.Id, originalUrl, credential.FileUrl);

            return Ok(new
            {
                success = true,
                message = "Latest credential has been tampered with. Verification should now fail.",
                data = new
                {
                    id = credential.Id,
                    studentId = credential.StudentId,
                    originalUrl,
                    originalHash,
                    newUrl = credential.FileUrl,
                    newHash = credential.IPFSHash
                }
            });
        }

        [HttpGet("attendance_date")]
        public IActionResult GetAttendanceDateValidation()
        {
            return Ok(new
            {
                success = true,
                data = new
                {
                    enabled = _validationService.IsAttendanceDateValidationEnabled
                }
            });
        }

        [HttpPost("attendance_date")]
        public async Task<IActionResult> SetAttendanceDateValidation([FromBody] AttendanceValidationToggleRequest request)
        {
            await _validationService.SetAttendanceDateValidationAsync(request.Enabled);
            _logger.LogInformation("Attendance date validation toggled to {Enabled}", request.Enabled);

            return Ok(new
            {
                success = true,
                message = request.Enabled
                    ? "Attendance date validation enabled"
                    : "Attendance date validation disabled",
                data = new
                {
                    enabled = request.Enabled
                }
            });
        }
    }
}
