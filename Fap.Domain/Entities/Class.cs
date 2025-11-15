using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Domain.Entities
{
    [Table("Classes")]
    public class Class
    {
        [Key] public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string ClassCode { get; set; }

        [Required]
        public Guid SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }

        // ✅ Giáo viên dạy lớp này
        [Required]
        public Guid TeacherUserId { get; set; }
        [ForeignKey(nameof(TeacherUserId))]
        public Teacher Teacher { get; set; }

        // ✅ Số lượng sinh viên tối đa
        [Range(1, 500)]
        public int MaxEnrollment { get; set; } = 40; // Mặc định 40 sinh viên

        // ✅ Sinh viên trong lớp
        public virtual ICollection<ClassMember> Members { get; set; }

        // ✅ Đăng ký lớp
        public virtual ICollection<Enroll> Enrolls { get; set; }
        // ✅ Buổi học trong lớp
        public virtual ICollection<Slot> Slots { get; set; }
    }
}
