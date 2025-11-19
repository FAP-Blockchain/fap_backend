using Fap.Infrastructure.Data;
using Fap.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly FapDbContext _context;
        private readonly ILogger<AdminController> _logger;

        public AdminController(FapDbContext context, ILogger<AdminController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Seed database with initial data (Protected with secret key)
        /// </summary>
        /// <remarks>
        /// Call this endpoint after deployment to populate the database with seed data.
        /// 
        /// Required header:
        /// - X-Seed-Secret: Your secret key configured in environment variables
        /// 
        /// Example:
        /// ```
        /// POST /api/admin/seed-database
        /// Headers:
        ///   X-Seed-Secret: your-secure-seed-key-2024
        /// ```
        /// </remarks>
        /// <response code="200">Database seeded successfully</response>
        /// <response code="401">Invalid or missing seed secret key</response>
        /// <response code="500">Seeding failed with error details</response>
        [HttpPost("seed-database")]
        [AllowAnonymous] // Tạm thời anonymous, bảo vệ bằng secret key
        [ProducesResponseType(typeof(SeedDatabaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SeedDatabase([FromHeader(Name = "X-Seed-Secret")] string? secret)
        {
            // 🔒 Bảo mật: Kiểm tra secret key
            var expectedSecret = Environment.GetEnvironmentVariable("SEED_SECRET_KEY")
            ?? "development-seed-key-2024"; // Default cho local dev

            if (string.IsNullOrWhiteSpace(secret) || secret != expectedSecret)
            {
                _logger.LogWarning("❌ Unauthorized seed attempt from IP: {IP}",
             HttpContext.Connection.RemoteIpAddress);
                return Unauthorized(new ErrorResponse
                {
                    Message = "Invalid or missing seed secret key",
                    Timestamp = DateTime.UtcNow
                });
            }

            try
            {
                _logger.LogInformation("==============================================");
                _logger.LogInformation("🌱 Starting database seeding via API endpoint...");
                _logger.LogInformation("📍 Triggered from IP: {IP}",
                      HttpContext.Connection.RemoteIpAddress);
                _logger.LogInformation("==============================================");

                // Gọi DataSeeder
                await DataSeeder.SeedAsync(_context);

                _logger.LogInformation("==============================================");
                _logger.LogInformation("✅ Database seeding completed successfully!");
                _logger.LogInformation("==============================================");

                return Ok(new SeedDatabaseResponse
                {
                    Message = "Database seeded successfully",
                    Timestamp = DateTime.UtcNow,
                    Status = "Success"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ ERROR during database seeding");
                _logger.LogError("Stack trace: {StackTrace}", ex.StackTrace);

                return StatusCode(500, new ErrorResponse
                {
                    Message = "Database seeding failed",
                    Error = ex.Message,
                    StackTrace = ex.StackTrace,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        /// <summary>
        /// Health check endpoint for the admin API
        /// </summary>
        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                Status = "Healthy",
                Service = "FAP Admin API",
                Timestamp = DateTime.UtcNow,
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"
            });
        }
    }

    #region Response Models

    public class SeedDatabaseResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public string? Error { get; set; }
        public string? StackTrace { get; set; }
        public DateTime Timestamp { get; set; }
    }

    #endregion
}
