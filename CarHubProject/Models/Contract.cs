namespace CarHubProject.Models
{
    public class Contract
    {
        public int ContractId { get; set; }
        public string ContractType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentStatus { get; set; }
        public string Status { get; set; }
        public int CustomerId { get; set; }
        public int EmployeeId { get; set; }
    }
}
