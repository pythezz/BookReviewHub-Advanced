using System.ComponentModel.DataAnnotations;

namespace BookReviewHub.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Display name must be between 2 and 50 characters.")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }

    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}