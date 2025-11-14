using System.ComponentModel.DataAnnotations;
namespace CarHubProject.Models
{
    public class Contract
    {
        public int ContractId { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Contract type must be (\"Rental\", \"Sale\")")]
        [Display(Name = "Contract Type")]
        public string? ContractType { get; set; } 

        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime? StartDate { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [DataType(DataType.Currency)]
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }

        [StringLength(50)]
        [Display(Name = "Payment Status")]
        public string? PaymentStatus { get; set; } // pending,paid,overdue

        [StringLength(50)]
        public string? Status { get; set; } // active,completed,cancelled

        [Display(Name = "Customer")]
        public string? CustomerId { get; set; }

        [Display(Name = "Employee")]
        public int? EmployeeId { get; set; }
        public int CarId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
