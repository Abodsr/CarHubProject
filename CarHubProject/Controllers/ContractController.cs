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
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            var contract = viewModel.Contract;
            contract.CarId = car.Id;

            if (contract.ContractType == "Rental")
            {
                if (!contract.StartDate.HasValue)
                {
                    ModelState.AddModelError("Contract.StartDate", "Start date is required for a rental.");
                }
                if (!contract.EndDate.HasValue)
                {
                    ModelState.AddModelError("Contract.EndDate", "End date is required for a rental.");
                }

                if (contract.StartDate.HasValue && contract.StartDate.Value < DateTime.Today)
                {
                    ModelState.AddModelError("Contract.StartDate", "Start date cannot be in the past.");
                }
                if (contract.StartDate.HasValue && contract.EndDate.HasValue && contract.EndDate.Value < contract.StartDate.Value)
                {
                    ModelState.AddModelError("Contract.EndDate", "End date must be after start date.");
                }

                if (contract.StartDate.HasValue && contract.EndDate.HasValue)
                {
                    var duration = (contract.EndDate.Value - contract.StartDate.Value).Days;
                    if (duration < 1)
                    {
                        ModelState.AddModelError("Contract.EndDate", "Rental must be for at least one day.");
                    }

                    var existingContracts = _contractRepository.GetContractsByCarId(car.Id);
                    foreach (var existingContract in existingContracts)
                    {
                        if (existingContract.ContractType == "Rental" && existingContract.Status != "Cancelled")
                        {
                            if (contract.StartDate.Value < existingContract.EndDate && contract.EndDate.Value > existingContract.StartDate)
                            {
                                ModelState.AddModelError(string.Empty, "This car is not available for the selected dates.");
                                break;
                            }
                        }
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                viewModel.CarDetails = car; // Re-populate CarDetails for view
                return View("CreateContract", viewModel);
            }

            contract.CustomerId = userId;
            contract.Status = "Pending";
            contract.PaymentStatus = "Pending";

            if (contract.ContractType == "Rental")
            {
                var duration = (contract.EndDate!.Value - contract.StartDate!.Value).Days;
                contract.TotalAmount = duration * car.PricePerDay;
                car.Status = "Rented";
            }
            else if (contract.ContractType == "Sale")
            {
                contract.TotalAmount = car.SalePrice;
                contract.StartDate = DateTime.Today;
                contract.EndDate = DateTime.Today;
                car.Status = "Sold";
            }

            _carRepository.Update(car);
            _carRepository.Save();
            _contractRepository.Add(contract);
            _contractRepository.Save();

            return RedirectToAction("CreateForContract", "Payment", new { contractId = contract.ContractId });
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
