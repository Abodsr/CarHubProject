using System.ComponentModel.DataAnnotations;
namespace CarHubProject.Models

{
    public class Brand
    {
        public int BrandId { get; set; }

        [Required(ErrorMessage = "Brand name is required.")]
        [StringLength(100)]
        public string? BrandName { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<Car>? Cars { get; set; } = new List<Car>();
    }
}
