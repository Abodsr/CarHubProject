namespace CarHubProject.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ContractId { get; set; }
    }
}
