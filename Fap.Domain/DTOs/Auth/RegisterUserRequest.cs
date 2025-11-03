using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Auth
{
    /// <summary>
    /// Request để đăng ký 1 tài khoản
    /// </summary>
    public class RegisterUserRequest
    {
        [Required(ErrorMessage = "Full name is required")]
        [MaxLength(150)]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [MaxLength(150)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public string RoleName { get; set; } // "Student" hoặc "Teacher"

        // ===== STUDENT FIELDS (Optional - chỉ bắt buộc khi RoleName = "Student") =====
        [MaxLength(30)]
        public string? StudentCode { get; set; }  // ✅ Nullable

        public DateTime? EnrollmentDate { get; set; }

        // ===== TEACHER FIELDS (Optional - chỉ bắt buộc khi RoleName = "Teacher") =====
        [MaxLength(30)]
        public string? TeacherCode { get; set; }  // ✅ Nullable

        public DateTime? HireDate { get; set; }

        [MaxLength(200)]
        public string? Specialization { get; set; }  // ✅ Nullable

        [MaxLength(20)]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string? PhoneNumber { get; set; }  // ✅ Nullable
    }
}