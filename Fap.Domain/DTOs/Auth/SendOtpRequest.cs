using System.ComponentModel.DataAnnotations;

namespace Fap.Domain.DTOs.Auth
{
    public class SendOtpRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Purpose is required")]
        public string Purpose { get; set; } // "Registration" or "PasswordReset"
    }
}