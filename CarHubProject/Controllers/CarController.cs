using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Drawing2D;

namespace CarHubProject.Controllers
{
    public class CarController : Controller
    {
        readonly ICarRepository _carRepository;
        readonly IBrandRepository _brandRepository;

        public CarController(ICarRepository carRepository, IBrandRepository brandRepository)
        {
            _carRepository = carRepository;
            _brandRepository = brandRepository;
        }
        public IActionResult Index(string? brand, decimal? minPrice, decimal? maxPrice, string? fuel, int? year)
        {
            
            
            ViewBag.Brands = _brandRepository.GetAll();

            
            var brandId = _brandRepository.GetAll()
                            .FirstOrDefault(b => b.BrandName.ToLower() == brand?.ToLower())?.BrandId;

            var cars = _carRepository.Search(
                brandId: brandId,
                year: year,
                fuelType: fuel,
                minPrice: minPrice,
                maxPrice: maxPrice
            );
            return View(cars);
            
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["brands"] = _brandRepository.GetAll();
            return View();
        }
        [HttpPost]
        public IActionResult Create(Car car)
        {
            ViewData["brands"] = _brandRepository.GetAll();

            if (ModelState.IsValid)
            {
                _carRepository.Add(car);
                _carRepository.Save();
                return RedirectToAction("Index", "Home");
            }
            return View(car);
        }
        [HttpGet]
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
        public IActionResult Edit(Car car)
        {
            if (ModelState.IsValid)
            {
                _carRepository.Update(car);
                _carRepository.Save();
         
                return RedirectToAction("Index");
            }

            ViewData["brands"] = _brandRepository.GetAll();
            return View(car);
        }
        public IActionResult Delete(int id)
        {
            var delCar= _carRepository.GetById(id);
           _carRepository.Delete(delCar);
            _carRepository.Save();
            return RedirectToAction("Index");
        }

        

    }
}
