using Fap.Api.Interfaces;
using Fap.Domain.DTOs.GradeComponent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Fap.Api.Controllers
{
    [ApiController]
    [Route("api/grade-components")]
    [Authorize]
    public class GradeComponentsController : ControllerBase
    {
        private readonly IGradeComponentService _gradeComponentService;
        private readonly ILogger<GradeComponentsController> _logger;

        public GradeComponentsController(
            IGradeComponentService gradeComponentService,
            ILogger<GradeComponentsController> logger)
        {
            _gradeComponentService = gradeComponentService;
            _logger = logger;
        }

        /// <summary>
        /// GET /api/grade-components - Get all grade components (optionally filter by subject)
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<GradeComponentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllGradeComponents([FromQuery] Guid? subjectId = null)
        {
            try
            {
                var components = await _gradeComponentService.GetAllGradeComponentsAsync(subjectId);
                return Ok(components);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grade components");
                return StatusCode(500, new { message = "An error occurred while retrieving grade components" });
            }
        }

        /// <summary>
        /// GET /api/grade-components/{id} - Get grade component by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GradeComponentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetGradeComponentById(Guid id)
        {
            try
            {
                var component = await _gradeComponentService.GetGradeComponentByIdAsync(id);

                if (component == null)
                    return NotFound(new { message = $"Grade component with ID {id} not found" });

                return Ok(component);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting grade component {GradeComponentId}", id);
                return StatusCode(500, new { message = "An error occurred while retrieving grade component" });
            }
        }

        /// <summary>
        /// POST /api/grade-components - Create a new grade component
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GradeComponentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateGradeComponent([FromBody] CreateGradeComponentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _gradeComponentService.CreateGradeComponentAsync(request);

                if (!result.Success)
                    return BadRequest(result);

                return CreatedAtAction(
                    nameof(GetGradeComponentById),
                    new { id = result.GradeComponentId },
                    result
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating grade component");
                return StatusCode(500, new { message = "An error occurred while creating grade component" });
            }
        }

        /// <summary>
        /// PUT /api/grade-components/{id} - Update a grade component
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GradeComponentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateGradeComponent(Guid id, [FromBody] UpdateGradeComponentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var result = await _gradeComponentService.UpdateGradeComponentAsync(id, request);

                if (!result.Success)
                {
                    if (result.Message?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                        return NotFound(result);
                    
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating grade component {GradeComponentId}", id);
                return StatusCode(500, new { message = "An error occurred while updating grade component" });
            }
        }

        /// <summary>
        /// DELETE /api/grade-components/{id} - Delete a grade component
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(GradeComponentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteGradeComponent(Guid id)
        {
            try
            {
                var result = await _gradeComponentService.DeleteGradeComponentAsync(id);

                if (!result.Success)
                {
                    if (result.Message?.Contains("not found", StringComparison.OrdinalIgnoreCase) == true)
                        return NotFound(result);
                    
                    return BadRequest(result);
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting grade component {GradeComponentId}", id);
                return StatusCode(500, new { message = "An error occurred while deleting grade component" });
            }
        }
    }
}
