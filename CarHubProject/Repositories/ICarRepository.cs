using CarHubProject.Models;

namespace CarHubProject.Repositories
{
    public interface ICarRepository
    {

        //CRUD
        List<Car> GetAll();
        public List<Car> Search(string? model = null, int? brandId = null, int? year = null, string? fuelType = null);
        public List<Car> FilterByPriceRange(decimal minPrice, decimal maxPrice);
        void Add(Car car);
        Car? GetById(int carId);
        void Update(Car car);

        void Delete(Car car);
        
        void Save();
    }
}
