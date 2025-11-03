namespace CarHubProject.Models
{
    public class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public int PricePerDay { get; set; }
        public string Status { get; set; }
        public string Color { get; set; }
        public string Transmission { get; set; }
        public string FuelType { get; set; }
        public decimal Mileage { get; set; }
        public int BrandId { get; set; }
    }
}
