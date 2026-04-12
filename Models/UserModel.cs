using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNetDashboard.Models
{
    /// <summary>
    /// Represents a managed user record in the system (not an auth user).
    /// </summary>
    public class UserModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100, MinimumLength = 2,
            ErrorMessage = "Name must be between 2 and 100 characters.")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email address is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address (e.g. user@example.com).")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Age is required.")]
        [Range(18, 60, ErrorMessage = "Age must be between 18 and 60 years old.")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, MinimumLength = 6,
            ErrorMessage = "Password must be at least 6 characters long.")]
        [DataType(DataType.Password)]
        [NotMapped]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match. Please try again.")]
        [Display(Name = "Confirm Password")]
        [NotMapped]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        [Display(Name = "Department")]
        [StringLength(80)]
        public string Department { get; set; } = string.Empty;

        [Display(Name = "Status")]
        [StringLength(20)]
        public string Status { get; set; } = "Active"; // Active | Inactive | Suspended

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
