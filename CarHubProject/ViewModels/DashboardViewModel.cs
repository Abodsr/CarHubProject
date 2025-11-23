using CarHubProject.Models;

namespace CarHubProject.ViewModels
{
    public class DashboardViewModel
    {
        public IEnumerable<Car> Cars { get; set; }
        public IEnumerable<Contract> Contracts { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
    }
}
