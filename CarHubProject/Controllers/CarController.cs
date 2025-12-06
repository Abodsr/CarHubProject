using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;
using Microsoft.AspNetCore.Authorization;
using CarHubProject.ViewModels;

namespace CarHubProject.Controllers
{
    public class CarController : Controller
    {
        readonly ICarRepository _carRepository;
        readonly IBrandRepository _brandRepository;
        readonly IReviewRepository _reviewRepository;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public CarController(ICarRepository carRepository, IBrandRepository brandRepository, IReviewRepository reviewRepository, IWebHostEnvironment webHostEnvironment)
        {
            _carRepository = carRepository;
            _brandRepository = brandRepository;
            _reviewRepository = reviewRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(string? brand, decimal? minPrice, decimal? maxPrice, string? fuel, int? year, string? sortOrder, string? priceType)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CurrentPriceType = priceType;
            ViewBag.ModelSortParm = String.IsNullOrEmpty(sortOrder) ? "model_desc" : "";
            ViewBag.PricePerDaySortParm = sortOrder == "priceperday_asc" ? "priceperday_desc" : "priceperday_asc";
            ViewBag.SalePriceSortParm = sortOrder == "saleprice_asc" ? "saleprice_desc" : "saleprice_asc";
            ViewBag.YearSortParm = sortOrder == "year_asc" ? "year_desc" : "year_asc";

            ViewBag.Brands = _brandRepository.GetAll();

            var brandId = _brandRepository.GetAll()
                            .FirstOrDefault(b => !string.IsNullOrEmpty(brand) && b.BrandName?.ToLower() == brand!.ToLower())?.BrandId;

            var cars = _carRepository.Search(
                brandId: brandId,
                year: year,
                fuelType: fuel,
                minPrice: minPrice,
                maxPrice: maxPrice,
                priceType: priceType
            );
            if (!User.IsInRole("Admin"))
            {
                cars = cars.Where(c => c.Status != "Sold").ToList();

            }


            switch (sortOrder)
            {
                case "model_desc":
                    cars = cars.OrderByDescending(s => s.Model).ToList();
                    break;
                case "priceperday_asc":
                    cars = cars.OrderBy(s => s.PricePerDay).ToList();
                    break;
                case "priceperday_desc":
                    cars = cars.OrderByDescending(s => s.PricePerDay).ToList();
                    break;
                case "saleprice_asc":
                    cars = cars.OrderBy(s => s.SalePrice).ToList();
                    break;
                case "saleprice_desc":
                    cars = cars.OrderByDescending(s => s.SalePrice).ToList();
                    break;
                case "year_asc":
                    cars = cars.OrderBy(s => s.Year).ToList();
                    break;
                case "year_desc":
                    cars = cars.OrderByDescending(s => s.Year).ToList();
                    break;
                default: // Model ascending
                    cars = cars.OrderBy(s => s.Model).ToList();
                    break;
            }

            return View(cars);

        }

        // GET: Car/Details/5
        public IActionResult Details(int id)
        {
            var car = _carRepository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }

            var reviews = _reviewRepository.GetAll().Where(r => r.CarId == id);

            var viewModel = new CarDetailViewModel
            {
                CarDetails = car,
                Reviews = reviews
            };

            return View(viewModel);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["brands"] = _brandRepository.GetAll();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(Car car, IFormFile imageFile)
        {
            ViewData["brands"] = _brandRepository.GetAll();

            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(fileStream);
                    }
                    car.ImageUrl = "/uploads/" + uniqueFileName;
                }

                _carRepository.Add(car);
                _carRepository.Save();
                return RedirectToAction("Index", "Home");
            }
            return View(car);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var car = _carRepository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }

            ViewData["brands"] = _brandRepository.GetAll();
            return View(car);
        }

       
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Car car, IFormFile imageFile)
        {
            ModelState.Remove("imageFile");

             if (ModelState.IsValid)
             {
                var oldCar = _carRepository.GetById(car.Id);
                if (oldCar == null) return NotFound();

                //Upadte Data
                oldCar.Model = car.Model;
                oldCar.Year = car.Year;
                oldCar.PricePerDay = car.PricePerDay;
                oldCar.Color = car.Color;
                oldCar.Transmission = car.Transmission;
                oldCar.FuelType = car.FuelType;
                oldCar.Mileage = car.Mileage;
                oldCar.BrandId = car.BrandId;
                oldCar.Status = car.Status;

                
                if (imageFile != null)
                {
                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        imageFile.CopyTo(stream);
                    }

                    oldCar.ImageUrl = "/uploads/" + uniqueFileName;
                }

                
                _carRepository.Save();

                return RedirectToAction("Index");
             }

            

            ViewData["brands"] = _brandRepository.GetAll();
            return View(car);
        }
        // GET: Car/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var car = _carRepository.GetById(id);
            if (car == null)
            {
                return NotFound();
            }
            return View(car);
        }

        // POST: Car/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var car = _carRepository.GetById(id);
            if (car != null)
            {
                _carRepository.Delete(car);
                _carRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        

    }
}
