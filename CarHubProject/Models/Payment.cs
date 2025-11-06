using System.ComponentModel.DataAnnotations;
namespace CarHubProject.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        [Required(ErrorMessage = "Payment amount is required.")]
        [Range(0.01, 10000000.00, ErrorMessage = "Payment amount must be positive.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Payment Type (credit card,cash,bank)")]
        public string PaymentType { get; set; } 

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // completed,pending,failed

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Payment Date")]
        public DateTime CreatedAt { get; set; } = DateTime.Now; 

        [Required(ErrorMessage = "This payment must be linked to a contract.")]
        [Display(Name = "Contract")]
        public int ContractId { get; set; }
    }
}