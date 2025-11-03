using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fap.Domain.Entities
{
    [Table("StudentRoadmaps")]
    public class StudentRoadmap
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid StudentId { get; set; }
        [ForeignKey(nameof(StudentId))]
        public Student Student { get; set; }

        [Required]
        public Guid SubjectId { get; set; }
        [ForeignKey(nameof(SubjectId))]
        public Subject Subject { get; set; }

        // 🔹 Trạng thái môn học
        [Required, MaxLength(20)]
        public string Status { get; set; } // "Planned", "InProgress", "Completed", "Failed"

        // 🔹 Kỳ học dự kiến hoặc đã học
        [Required]
        public Guid SemesterId { get; set; }
        [ForeignKey(nameof(SemesterId))]
        public Semester Semester { get; set; }

        // 🔹 Thứ tự trong lộ trình (môn học thứ mấy)
        public int SequenceOrder { get; set; }

        // 🔹 Điểm số (nếu đã hoàn thành)
        [Column(TypeName = "decimal(4,2)")]
        public decimal? FinalScore { get; set; }

        [MaxLength(5)]
        public string LetterGrade { get; set; } // "A", "B+", "C"...

        // 🔹 Ngày bắt đầu và hoàn thành
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }

        // 🔹 Ghi chú
        [MaxLength(500)]
        public string Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}