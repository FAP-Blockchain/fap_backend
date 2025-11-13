using System;
using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Slot
{
public class UpdateSlotRequest
    {
        [Required(ErrorMessage = "Date is required")]
        public DateTime Date { get; set; }

        public Guid? TimeSlotId { get; set; }

        public Guid? SubstituteTeacherId { get; set; }

        [MaxLength(500, ErrorMessage = "Substitution reason cannot exceed 500 characters")]
        public string? SubstitutionReason { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Scheduled|Completed|Cancelled)$", ErrorMessage = "Status must be Scheduled, Completed, or Cancelled")]
        public string Status { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}
