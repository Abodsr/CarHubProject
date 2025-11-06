using System.ComponentModel.DataAnnotations;

namespace CarHubProject.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; } // Optional

        [StringLength(250)]
        public string? Address { get; set; } // Optional

        [StringLength(50)]
        [Display(Name = "National ID")]
        public string? NationalId { get; set; } // Optional

        [StringLength(50)]
        [Display(Name = "License Number")]
        public string LicenseNumber { get; set; } 

        [Required]
        public string PasswordHash { get; set; } 

        [Required]
        [StringLength(20)]
        public string Role { get; set; } = "Customer";

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = false; 
    }
}