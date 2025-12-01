using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Domain.Entities
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }  // Liên kết với User

        [Required, MaxLength(30)]
        public string StudentCode { get; set; } // Mã sinh viên

    public DateTime EnrollmentDate { get; set; } // Ngày nhập học
    public decimal GPA { get; set; }  // Điểm trung bình

    public bool IsGraduated { get; set; } = false;
    public DateTime? GraduationDate { get; set; }

        // Navigation
        public User User { get; set; }  // Quan hệ với User
        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Enroll> Enrolls { get; set; }
        public virtual ICollection<Attendance> Attendances { get; set; }

        public int? CurriculumId { get; set; }
        public virtual Curriculum? Curriculum { get; set; }

        public virtual ICollection<ClassMember> ClassMembers { get; set; }
        public virtual ICollection<Credential> Credentials { get; set; }
    public virtual ICollection<StudentRoadmap> Roadmaps { get; set; }
    }
}
