using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using CarHubProject.ViewModels;

namespace CarHubProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ContractController : Controller
    {
        readonly IContractRepository _contractRepository;
        readonly ICarRepository _carRepository;
        readonly UserManager<User> _userManager;

        public ContractController(IContractRepository contractRepository, ICarRepository carRepository, UserManager<User> userManager)
        {
            _contractRepository = contractRepository;
            _carRepository = carRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [Authorize]
        [HttpGet]
        public IActionResult CreateContract(int carId)
        {
            var car = _carRepository.GetById(carId);
            if (car == null)
            {
                return NotFound();
            }

            
            var status = car.Status?.Trim().ToLower();
            if (status != "available")
            {
                TempData["Error"] = $"This car is not available. Current status: {car.Status}";
                return RedirectToAction("Details", "Car", new { id = carId });
            }

            var viewModel = new CreateContractViewModel
            {
                CarId = carId,
                CarDetails = car,
                Contract = new Contract
                {
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1)
                }
            };

            return View("CreateContract", viewModel);
        }

        [AllowAnonymous]
        [Authorize]
        [HttpPost]
        public IActionResult CreateContract(CreateContractViewModel viewModel)
        {
            var car = _carRepository.GetById(viewModel.CarId);
            if (car == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Challenge();

            var status = car.Status?.Trim().ToLower();
            if (status != "available")
            {
                TempData["Error"] = $"This car is not available. Current status: {car.Status}";
                return RedirectToAction("Details", "Car", new { id = viewModel.CarId });
            }

            var contract = viewModel.Contract;
            contract.CarId = car.Id;
            contract.CustomerId = userId;

            if (contract.ContractType == "Rental")
            {
                if (!contract.StartDate.HasValue)
                    ModelState.AddModelError("Contract.StartDate", "Start date is required.");

                if (!contract.EndDate.HasValue)
                    ModelState.AddModelError("Contract.EndDate", "End date is required.");

                if (contract.StartDate.HasValue && contract.StartDate.Value < DateTime.Today)
                    ModelState.AddModelError("Contract.StartDate", "Start date cannot be in the past.");

                if (contract.StartDate.HasValue &&
                    contract.EndDate.HasValue &&
                    contract.EndDate < contract.StartDate)
                    ModelState.AddModelError("Contract.EndDate", "End date must be after start date.");

                if (contract.StartDate.HasValue && contract.EndDate.HasValue)
                {
                    int duration = (contract.EndDate.Value - contract.StartDate.Value).Days;
                    if (duration < 1)
                        ModelState.AddModelError("Contract.EndDate", "Minimum rental is one day.");

                    var existingContracts = _contractRepository.GetContractsByCarId(car.Id);
                    foreach (var existingContract in existingContracts)
                    {
                        if (existingContract.ContractType == "Rental" &&
                            existingContract.Status != "Cancelled")
                        {
                            if (contract.StartDate < existingContract.EndDate &&
                                contract.EndDate > existingContract.StartDate)
                            {
                                ModelState.AddModelError(string.Empty, "This car is not available for the selected dates.");
                                break;
                            }
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
                return View("CreateContract", viewModel);

            _contractRepository.Add(contract);
            _contractRepository.Save();

            if (contract.ContractType == "Purchase")
                car.Status = "Sold";

            if (contract.ContractType == "Rental")
                car.Status = "Rented";

            _carRepository.Update(car);
            _carRepository.Save();

            TempData["Success"] = "Contract created successfully!";
            return RedirectToAction("Index", "Car");
        }

        public IActionResult Index()
        {
            var contracts = _contractRepository.GetAll();
            return View(contracts);
        }


        public IActionResult Details(int id)
        {
            var contract = _contractRepository.GetById(id);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Contract contract)
        {
            if (ModelState.IsValid)
            {
                _contractRepository.Add(contract);
                _contractRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contract = _contractRepository.GetById(id);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        [HttpPost]
        public IActionResult Edit(Contract contract)
        {
            if (ModelState.IsValid)
            {
                _contractRepository.Update(contract);
                _contractRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(contract);
        }

        public IActionResult Delete(int id)
        {
            var contract = _contractRepository.GetById(id);
            if (contract == null)
            {
                return NotFound();
            }
            return View(contract);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var contract = _contractRepository.GetById(id);
            if (contract != null)
            {
                _contractRepository.Delete(contract);
                _contractRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
