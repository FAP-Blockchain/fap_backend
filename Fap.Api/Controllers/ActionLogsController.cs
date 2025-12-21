using Fap.Domain.DTOs.ActionLog;
using Fap.Domain.DTOs.Common;
using Fap.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fap.Api.Controllers
{
    [ApiController]
    [Route("api/action-logs")]
    [Authorize(Roles = "Admin")]
    public class ActionLogsController : ControllerBase
    {
        private readonly IActionLogService _actionLogService;
        private readonly ILogger<ActionLogsController> _logger;

        public ActionLogsController(IActionLogService actionLogService, ILogger<ActionLogsController> logger)
        {
            _actionLogService = actionLogService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/action-logs - Get action logs (Admin only)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<ActionLogDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResult<ActionLogDto>>> GetActionLogs([FromQuery] GetActionLogsRequest request)
        {
            try
            {
                var result = await _actionLogService.GetActionLogsAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting action logs");
                return StatusCode(500, new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = "An error occurred while retrieving action logs"
                });
            }
        }
    }
}
