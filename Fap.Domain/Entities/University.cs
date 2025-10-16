using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fap.Domain.Entities
{
    [Table("Universities")]
    public class University
    {
        [Key]
        public Guid Id { get; set; }  // Khóa chính (Id của trường đại học)

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }  // Tên trường đại học

        [MaxLength(100)]
        public string ShortName { get; set; }  // Tên viết tắt (VD: FPT, HUST)

        [MaxLength(255)]
        public string Email { get; set; }  // Email chính thức của trường

        [MaxLength(20)]
        public string Phone { get; set; }  // Số điện thoại

        [MaxLength(500)]
        public string Address { get; set; }  // Địa chỉ trường

        [MaxLength(255)]
        public string Website { get; set; }  // Website của trường

        public DateTime CreatedAt { get; set; }  // Ngày tạo

        public DateTime? UpdatedAt { get; set; }  // Ngày cập nhật (nullable)

        // Các bảng liên quan
        public virtual ICollection<Class> Classes { get; set; }  // Một trường có nhiều lớp học
        public virtual ICollection<Subject> Subjects { get; set; }  // Một trường có nhiều môn học
        public virtual ICollection<User> Users { get; set; }  // Người dùng của trường
    }

}
