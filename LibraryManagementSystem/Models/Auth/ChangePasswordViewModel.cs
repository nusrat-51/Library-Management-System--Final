using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models.Auth
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string NewPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "New Password and Confirm Password do not match.")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
