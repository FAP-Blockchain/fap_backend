using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Auth
{
    public class VerifyOtpRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "OTP code is required")]
        [StringLength(10, MinimumLength = 4)]
        public string Code { get; set; }

        [Required(ErrorMessage = "Purpose is required")]
        public string Purpose { get; set; }
    }
}