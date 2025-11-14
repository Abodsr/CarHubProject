using CarHubProject.Models;

namespace CarHubProject.ViewModels
{
    public class CarDetailViewModel
    {
        public Car? CarDetails { get; set; }
        public IEnumerable<Review> Reviews { get; set; } = new List<Review>();
    }
}
