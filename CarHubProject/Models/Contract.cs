using System.ComponentModel.DataAnnotations;
namespace CarHubProject.Models
{
    public class Contract
    {
        public int ContractId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Contract type must be (\"Rental\", \"Sale\")")]
        [Display(Name = "Contract Type")]
        public string ContractType { get; set; } 

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0.01, 10000000.00, ErrorMessage = "Total amount must be a positive value.")]
        [DataType(DataType.Currency)]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Payment Status")]
        public string PaymentStatus { get; set; } // pending,paid,overdue

        [Required]
        [StringLength(50)]
        public string Status { get; set; } // active,completed,cancelled


        [Required(ErrorMessage = "A customer must be assigned to the contract.")]
        [Display(Name = "Customer")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "An employee must be assigned to the contract.")]
        [Display(Name = "Employee")]
        public int EmployeeId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
