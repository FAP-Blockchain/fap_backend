using System;
using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Slot
{
    public class UpdateSlotStatusRequest
    {
        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Scheduled|Completed|Cancelled)$", ErrorMessage = "Status must be Scheduled, Completed, or Cancelled")]
        public string Status { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}
