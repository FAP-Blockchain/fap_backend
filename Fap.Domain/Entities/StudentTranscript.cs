using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Domain.Entities
{
    [Table("StudentTranscripts")]
    public class StudentTranscript
    {
        [Key] public Guid Id { get; set; }

        [Required] public Guid StudentId { get; set; }  // Liên kết với bảng Student
        [ForeignKey(nameof(StudentId))] public Student Student { get; set; }

        [Required] public Guid SubjectId { get; set; }  // Liên kết với bảng Subject
        [ForeignKey(nameof(SubjectId))] public Subject Subject { get; set; }

        [Column(TypeName = "decimal(4,2)")]
        public decimal? FinalScore { get; set; }  // Điểm cuối cùng của môn học

        [MaxLength(5)] public string FinalLetter { get; set; }  // Điểm chữ (A, B, C,...)

        public bool Passed { get; set; }  // Trạng thái vượt qua môn học

        [MaxLength(100)] public string AcademicPhase { get; set; }  // Giai đoạn học tập (Year 1 Semester 2,...)

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;  // Ngày cập nhật lộ trình học

        // Cột để lưu trữ thông tin tín chỉ môn học
        public int Credits { get; set; }

        // Cột lưu trạng thái hoàn thành chương trình học
        public bool IsGraduated { get; set; }  // Trạng thái xét tốt nghiệp
    }

}
