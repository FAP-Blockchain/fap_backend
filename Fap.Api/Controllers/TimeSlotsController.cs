using Fap.Api.Interfaces;
using Fap.Domain.DTOs.TimeSlot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TimeSlotsController : ControllerBase
    {
        private readonly ITimeSlotService _timeSlotService;
        private readonly ILogger<TimeSlotsController> _logger;

        public TimeSlotsController(ITimeSlotService timeSlotService, ILogger<TimeSlotsController> logger)
        {
            _timeSlotService = timeSlotService;
            _logger = logger;
        }

        /// <summary>
        /// Get all time slots
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllTimeSlots()
        {
            try
            {
                var timeSlots = await _timeSlotService.GetAllTimeSlotsAsync();
                return Ok(timeSlots);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting time slots: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving time slots" });
            }
        }

        /// <summary>
        /// Get time slot by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTimeSlotById(Guid id)
        {
            try
            {
                var timeSlot = await _timeSlotService.GetTimeSlotByIdAsync(id);
                
                if (timeSlot == null)
                    return NotFound(new { message = $"Time slot with ID {id} not found" });
                
                return Ok(timeSlot);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error getting time slot {id}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while retrieving time slot" });
            }
        }

        /// <summary>
        /// Create a new time slot
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateTimeSlot([FromBody] CreateTimeSlotRequest request)
        {
            try
            {
                var result = await _timeSlotService.CreateTimeSlotAsync(request);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return CreatedAtAction(
                    nameof(GetTimeSlotById), 
                    new { id = result.TimeSlotId }, 
                    result
                );
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error creating time slot: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while creating time slot" });
            }
        }

        /// <summary>
        /// Update an existing time slot
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTimeSlot(Guid id, [FromBody] UpdateTimeSlotRequest request)
        {
            try
            {
                var result = await _timeSlotService.UpdateTimeSlotAsync(id, request);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error updating time slot {id}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while updating time slot" });
            }
        }

        /// <summary>
        /// Delete a time slot
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTimeSlot(Guid id)
        {
            try
            {
                var result = await _timeSlotService.DeleteTimeSlotAsync(id);
                
                if (!result.Success)
                    return BadRequest(result);
                
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"? Error deleting time slot {id}: {ex.Message}");
                return StatusCode(500, new { message = "An error occurred while deleting time slot" });
            }
        }
    }
}
