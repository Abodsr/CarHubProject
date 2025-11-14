using CarHubProject.Models;

namespace CarHubProject.ViewModels
{
    public class CreateContractViewModel
    {
        public int CarId { get; set; }
        public Car? CarDetails { get; set; }
        public Contract Contract { get; set; } = new Contract();
    }
}
