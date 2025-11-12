using System.ComponentModel.DataAnnotations;
namespace CarHubProject.Models
{
    public class Review
    {
        public int ReviewId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5 stars.")]
        public int Rating { get; set; }

        [StringLength(2000, ErrorMessage = "Comment cannot be longer than 2000 characters.")]
        public string? Comment { get; set; } // Comment can be optional

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "A review must be linked to a customer.")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "A review must be linked to a car.")]
        [Display(Name = "Car")]
        public int CarId { get; set; }

        [Display(Name = "Hidden")]
        public bool IsHidden { get; set; } = false; 
    }
}