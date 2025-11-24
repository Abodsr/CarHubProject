using CarHubProject.Models;

namespace CarHubProject.ViewModels
{
    public class UserDashboardViewModel
    {
        public IEnumerable<Contract> UserContracts { get; set; }
        public IEnumerable<Car> UserCars { get; set; }
    }
}
