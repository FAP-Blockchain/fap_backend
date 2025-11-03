using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fap.Domain.Entities
{
    [Table("Slots")]
    public class Slot
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ClassId { get; set; }
        [ForeignKey(nameof(ClassId))]
        public Class Class { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public Guid? TimeSlotId { get; set; }
        [ForeignKey(nameof(TimeSlotId))]
        public TimeSlot TimeSlot { get; set; }

        // 🆕 Giáo viên dạy thay (nếu có)
        public Guid? SubstituteTeacherId { get; set; }
        [ForeignKey(nameof(SubstituteTeacherId))]
        public Teacher SubstituteTeacher { get; set; }

        // 🆕 Lý do thay đổi giáo viên
        [MaxLength(500)]
        public string SubstitutionReason { get; set; }

        // 🆕 Trạng thái slot
        [MaxLength(20)]
        public string Status { get; set; } = "Scheduled"; // "Scheduled", "Completed", "Cancelled"

        // 🆕 Ghi chú
        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual ICollection<Attendance> Attendances { get; set; }
    }
}
