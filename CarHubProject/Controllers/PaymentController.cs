using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarHubProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PaymentController : Controller
    {
        readonly IPaymentRepository _paymentRepository;
        readonly IContractRepository _contractRepository;

        public PaymentController(IPaymentRepository paymentRepository, IContractRepository contractRepository)
        {
            _paymentRepository = paymentRepository;
            _contractRepository = contractRepository;
        }

        [AllowAnonymous]
        [Authorize]
        [HttpGet]
        public IActionResult CreateForContract(int contractId)
        {
            var contract = _contractRepository.GetById(contractId);
            if (contract == null)
            {
                return NotFound();
            }

            var payment = new Payment
            {
                ContractId = contractId,
                Amount = contract.TotalAmount
            };

            return View("CreateForContract", payment);
        }

        [AllowAnonymous]
        [Authorize]
        [HttpPost]
        public IActionResult CreateForContract(Payment payment)
        {
            payment.Status = "Completed";
            _paymentRepository.Add(payment);

            var contract = _contractRepository.GetById(payment.ContractId);
            if (contract != null)
            {
                contract.PaymentStatus = "Paid";
                _contractRepository.Update(contract);
            }

            _paymentRepository.Save(); // This will save both payment and contract changes if the context is shared. It's better to call Save on each repository.
            _contractRepository.Save();


            return RedirectToAction("Index", "Car");
        }

        public IActionResult Index()
        {
            var payments = _paymentRepository.GetAll();
            return View(payments);
        }

        public IActionResult Details(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Payment payment)
        {
            if (ModelState.IsValid)
            {
                _paymentRepository.Add(payment);
                _paymentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        [HttpPost]
        public IActionResult Edit(Payment payment)
        {
            if (ModelState.IsValid)
            {
                _paymentRepository.Update(payment);
                _paymentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(payment);
        }

        public IActionResult Delete(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment == null)
            {
                return NotFound();
            }
            return View(payment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var payment = _paymentRepository.GetById(id);
            if (payment != null)
            {
                _paymentRepository.Delete(payment);
                _paymentRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
