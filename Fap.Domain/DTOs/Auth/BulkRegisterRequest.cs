using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Auth
{
    /// <summary>
    /// Request để đăng ký nhiều tài khoản cùng lúc
    /// </summary>
    public class BulkRegisterRequest
    {
        [Required]
        [MinLength(1, ErrorMessage = "At least one user is required")]
        public List<RegisterUserRequest> Users { get; set; }
    }
}