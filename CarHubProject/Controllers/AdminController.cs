using CarHubProject.Models;
using CarHubProject.Repositories;
using CarHubProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarHubProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ICarRepository _carRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IBrandRepository _brandRepository;

        public AdminController(ICarRepository carRepository, IContractRepository contractRepository, IBrandRepository brandRepository)
        {
            _carRepository = carRepository;
            _contractRepository = contractRepository;
            _brandRepository = brandRepository;
        }

        public IActionResult Dashboard()
        {
            var cars = _carRepository.GetAll();
            var contracts = _contractRepository.GetAll();
            var brands = _brandRepository.GetAll();

            var dashboardViewModel = new DashboardViewModel
            {
                Cars = cars,
                Contracts = contracts,
                Brands = brands
            };

            return View(dashboardViewModel);
        }

        [HttpPost]
        public IActionResult UpdateCarStatus(int carId, string status)
        {
            var car = _carRepository.GetById(carId);
            if (car == null)
            {
                return NotFound();
            }

            car.Status = status;
            _carRepository.Update(car);
            _carRepository.Save();

            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public IActionResult UpdateContractStatus(int contractId, string status)
        {
            var contract = _contractRepository.GetById(contractId);
            if (contract == null)
            {
                return NotFound();
            }

            contract.Status = status;
            _contractRepository.Update(contract);
            _contractRepository.Save();

            return RedirectToAction("Dashboard");
        }
    }
}