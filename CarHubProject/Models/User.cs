using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarHubProject.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        [StringLength(250)]
        public string? Address { get; set; } // Optional

        [StringLength(50)]
        [Display(Name = "National ID")]
        public string? NationalId { get; set; } // Optional

        [StringLength(50)]
        [Display(Name = "License Number")]
        public string? LicenseNumber { get; set; } 

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsActive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}