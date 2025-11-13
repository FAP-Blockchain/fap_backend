using Fap.Api.Interfaces;
using Fap.Domain.DTOs.Slot;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Fap.Api.Controllers
{
    /// <summary>
    /// Slots Management API - Quản lý buổi học (Class Sessions)
    /// </summary>
    [Route("api/slots")]
    [ApiController]
    [Authorize]
    public class SlotsController : ControllerBase
    {
        private readonly ISlotService _slotService;

        public SlotsController(ISlotService slotService)
        {
            _slotService = slotService;
        }

        /// <summary>
        /// GET /api/slots/{id} - Get slot details by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSlotById(Guid id)
        {
            try
            {
                var result = await _slotService.GetSlotByIdAsync(id);
                if (result == null)
                {
                    return NotFound(new { success = false, message = $"Slot with ID {id} not found" });
                }
                return Ok(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/slots - Create a new slot
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CreateSlot([FromBody] CreateSlotRequest request)
        {
            try
            {
                var result = await _slotService.CreateSlotAsync(request);
                return CreatedAtAction(nameof(GetSlotById), new { id = result.Id }, new
                {
                    success = true,
                    message = "Slot created successfully",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while creating slot", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/slots/{id} - Update an existing slot
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> UpdateSlot(Guid id, [FromBody] UpdateSlotRequest request)
        {
            try
            {
                // Get teacher user ID from claims
                var teacherUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                // Check authorization (unless admin)
                if (!User.IsInRole("Admin"))
                {
                    if (!await _slotService.CanModifySlotAsync(id, teacherUserId))
                    {
                        return Forbid();
                    }
                }

                var result = await _slotService.UpdateSlotAsync(id, request);
                if (result == null)
                {
                    return NotFound(new { success = false, message = $"Slot with ID {id} not found" });
                }
                return Ok(new
                {
                    success = true,
                    message = "Slot updated successfully",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while updating slot", error = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/slots/{id} - Delete a slot
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> DeleteSlot(Guid id)
        {
            try
            {
                // Get teacher user ID from claims
                var teacherUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                // Check authorization (unless admin)
                if (!User.IsInRole("Admin"))
                {
                    if (!await _slotService.CanModifySlotAsync(id, teacherUserId))
                    {
                        return Forbid();
                    }
                }

                // Check if can delete
                if (!await _slotService.CanDeleteSlotAsync(id))
                {
                    return BadRequest(new { success = false, message = "Cannot delete this slot. It may have attendance records." });
                }

                var result = await _slotService.DeleteSlotAsync(id);
                if (!result)
                {
                    return NotFound(new { success = false, message = $"Slot with ID {id} not found" });
                }
                return Ok(new { success = true, message = "Slot deleted successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while deleting slot", error = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/slots/{id}/status - Update slot status
        /// </summary>
        [HttpPut("{id}/status")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> UpdateSlotStatus(Guid id, [FromBody] UpdateSlotStatusRequest request)
        {
            try
            {
                var teacherUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                if (!User.IsInRole("Admin"))
                {
                    if (!await _slotService.CanModifySlotAsync(id, teacherUserId))
                    {
                        return Forbid();
                    }
                }

                var result = await _slotService.UpdateSlotStatusAsync(id, request);
                if (result == null)
                {
                    return NotFound(new { success = false, message = $"Slot with ID {id} not found" });
                }
                return Ok(new
                {
                    success = true,
                    message = "Slot status updated successfully",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/slots/{id}/complete - Mark slot as completed
        /// </summary>
        [HttpPost("{id}/complete")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CompleteSlot(Guid id)
        {
            try
            {
                var teacherUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                if (!User.IsInRole("Admin"))
                {
                    if (!await _slotService.CanModifySlotAsync(id, teacherUserId))
                    {
                        return Forbid();
                    }
                }

                var result = await _slotService.CompleteSlotAsync(id);
                if (result == null)
                {
                    return NotFound(new { success = false, message = $"Slot with ID {id} not found" });
                }
                return Ok(new
                {
                    success = true,
                    message = "Slot marked as completed",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/slots/{id}/cancel - Cancel a slot
        /// </summary>
        [HttpPost("{id}/cancel")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> CancelSlot(Guid id, [FromBody] CancelSlotRequest request)
        {
            try
            {
                var teacherUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

                if (!User.IsInRole("Admin"))
                {
                    if (!await _slotService.CanModifySlotAsync(id, teacherUserId))
                    {
                        return Forbid();
                    }
                }

                var result = await _slotService.CancelSlotAsync(id, request.Reason);
                if (result == null)
                {
                    return NotFound(new { success = false, message = $"Slot with ID {id} not found" });
                }
                return Ok(new
                {
                    success = true,
                    message = "Slot cancelled successfully",
                    data = result
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/slots/filter - Get slots with filters
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetSlotsByFilter([FromQuery] SlotFilterRequest filter)
        {
            try
            {
                var result = await _slotService.GetSlotsByFilterAsync(filter);
                return Ok(new
                {
                    success = true,
                    data = result,
                    pageNumber = filter.PageNumber,
                    pageSize = filter.PageSize
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/slots/upcoming - Get upcoming slots for current teacher
        /// </summary>
        [HttpGet("upcoming")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> GetUpcomingSlots()
        {
            try
            {
                var teacherUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var result = await _slotService.GetUpcomingSlotsAsync(teacherUserId);
                return Ok(new
                {
                    success = true,
                    message = $"Retrieved {result.Count()} upcoming slots",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/slots/needing-attendance - Get slots that need attendance for current teacher
        /// </summary>
        [HttpGet("needing-attendance")]
        [Authorize(Roles = "Teacher,Admin")]
        public async Task<IActionResult> GetSlotsNeedingAttendance()
        {
            try
            {
                var teacherUserId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                var result = await _slotService.GetSlotsNeedingAttendanceAsync(teacherUserId);
                return Ok(new
                {
                    success = true,
                    message = $"Found {result.Count()} slots needing attendance",
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred", error = ex.Message });
            }
        }
    }

    // Helper DTO for cancel request
    public class CancelSlotRequest
    {
        public string Reason { get; set; }
    }
}
