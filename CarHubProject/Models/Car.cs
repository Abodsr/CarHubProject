using System.ComponentModel.DataAnnotations;
namespace CarHubProject.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Model name is required.")]
        [StringLength(100)]
        public string? Model { get; set; }

        [Required(ErrorMessage = "Please enter a year.")]
        [Range(1900, 2026, ErrorMessage = "Year must be between 1900 and 2026.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "Please set a price.")]
        [Range(100, 100000, ErrorMessage = "Price must be between 100 EGP and  100,000 EGP .")]
        public int PricePerDay { get; set; }

        [Required(ErrorMessage = "Status is required.(Available,Rented,Maintenance)")]
        [StringLength(20)]
        public string? Status { get; set; }

        [Required]
        [StringLength(50)]
        public string? Color { get; set; }

        [Required]
        [StringLength(50)]
        public string? Transmission { get; set; }

        [Required(ErrorMessage = "FuelType is required (Petrol, Diesel, Hybrid, Electric)")]
        [StringLength(100)]
        public string? FuelType { get; set; }

        [Required]
        [Range(0, 1000000, ErrorMessage = "Mileage (km) must be a positive number.")]
        public decimal Mileage { get; set; }

        [Required]
        public int BrandId { get; set; }

        public Brand ?Brand { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
