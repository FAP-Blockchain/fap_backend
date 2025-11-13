using System;
using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Slot
{
    public class CreateSlotRequest
    {
        [Required(ErrorMessage = "ClassId is required")]
         public Guid ClassId { get; set; }

        [Required(ErrorMessage = "Date is required")]
         public DateTime Date { get; set; }

        public Guid? TimeSlotId { get; set; }

        public Guid? SubstituteTeacherId { get; set; }

        [MaxLength(500, ErrorMessage = "Substitution reason cannot exceed 500 characters")]
        public string? SubstitutionReason { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }
    }
}
