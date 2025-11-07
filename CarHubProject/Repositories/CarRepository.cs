using CarHubProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CarHubProject.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbContext _context;
        public CarRepository(AppDbContext context)
        {
            _context = context;
        }
        public void Add(Car car)
        {
            _context.Cars.Add(car);
        }

        public void Delete(Car car)
        {
            Car? DelCar = GetById(car.Id);
            if (DelCar != null && DelCar.IsDeleted == false)
            {
                DelCar.IsDeleted = true;
            }


        }

        public List<Car> GetAll()
        {
            return _context.Cars.Include(x => x.Brand)
                .Where(x => !x.IsDeleted)
                .ToList();
        }
        public List<Car> GetAvailableCars()
        {
            return _context.Cars.Include(x => x.Brand)
                           .Where(x => !x.IsDeleted && x.Status == "Available")
                           .ToList();
        }



        public Car? GetById(int carId)
        {
            return _context.Cars.Include(x => x.Brand).FirstOrDefault(x => x.Id == carId);
        }

        public void Update(Car car)
        {
            _context.Update(car);
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        public List<Car> Search(string? model = null, int? brandId = null, int? year = null, string? fuelType = null)
        {
            var query = _context.Cars.Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(model))
                query = query.Where(x => x.Model.ToLower().Contains(model.ToLower()));

            if (brandId.HasValue)
                query = query.Where(x => x.BrandId == brandId);

            if (year.HasValue)
                query = query.Where(x => x.Year == year);

            if (!string.IsNullOrEmpty(fuelType))
                query = query.Where(x => x.FuelType.ToLower() == fuelType.ToLower());

            return query.ToList();
        }

        public List<Car> FilterByPriceRange(decimal minPrice, decimal maxPrice)
        {
            return _context.Cars.Include(x => x.Brand).Where(x => !x.IsDeleted
            && x.PricePerDay >= minPrice && x.PricePerDay <= maxPrice).ToList();
        }
    }
}
