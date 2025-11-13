using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Domain.Entities
{
    [Table("Attendances")]
    public class Attendance
    {
        [Key] public Guid Id { get; set; }

        [Required] public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))] public Student Student { get; set; }

        [Required] public Guid SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))] public Subject Subject { get; set; }

        [Required] public Guid SlotId { get; set; }
        [ForeignKey(nameof(SlotId))] public Slot Slot { get; set; }

        public bool IsPresent { get; set; }
        
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        public bool IsExcused { get; set; } = false;
    
        [MaxLength(1000)]
        public string? ExcuseReason { get; set; }
        
        public DateTime RecordedAt { get; set; } = DateTime.UtcNow;
    }
}
