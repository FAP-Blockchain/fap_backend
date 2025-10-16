using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Domain.Entities
{
    [Table("SubjectCriteria")]
    public class SubjectCriteria
    {
        [Key] public Guid Id { get; set; }

        [Required] public Guid SubjectId { get; set; }  // Liên kết với môn học
        [ForeignKey(nameof(SubjectId))] public Subject Subject { get; set; }

        [Required, MaxLength(200)]
        public string Name { get; set; }  // Tên tiêu chí (VD: "Tham gia đầy đủ", "Điểm tối thiểu cho quiz")

        [MaxLength(500)]
        public string Description { get; set; }  // Mô tả chi tiết về tiêu chí

        public decimal MinScore { get; set; }  // Điểm tối thiểu yêu cầu (nếu có)

        public bool IsMandatory { get; set; }  // Tiêu chí này có bắt buộc không (VD: bắt buộc tham gia 75% lớp học)

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  // Ngày tạo tiêu chí
    }
}
