using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fap.Domain.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key] public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string ClassCode { get; set; }

        // ✅ CHANGED: Link to SubjectOffering instead of Subject directly
        [Required]
        public Guid SubjectOfferingId { get; set; }

        [ForeignKey(nameof(SubjectOfferingId))]
        public virtual SubjectOffering SubjectOffering { get; set; }


        // ✅ Giáo viên dạy lớp này
        [Required]
        public Guid TeacherUserId { get; set; }

        [ForeignKey(nameof(TeacherUserId))]
        public virtual Teacher Teacher { get; set; }

        // ✅ Số lượng sinh viên tối đa
        [Range(1, 500)]
        public int MaxEnrollment { get; set; } = 40; // Mặc định 40 sinh viên

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ✅ Navigation properties
        public virtual ICollection<ClassMember> Members { get; set; } = new List<ClassMember>();
        public virtual ICollection<Enroll> Enrolls { get; set; } = new List<Enroll>();
        public virtual ICollection<Slot> Slots { get; set; } = new List<Slot>();
    }
}
