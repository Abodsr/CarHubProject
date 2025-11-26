using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CarHubProject.Controllers
{
    public class ReviewController : Controller
    {
        readonly IReviewRepository _reviewRepository;
        readonly IContractRepository _contractRepository;
        readonly UserManager<User> _userManager;

        public ReviewController(IReviewRepository reviewRepository, UserManager<User> userManager, IContractRepository contractRepository)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
            _contractRepository = contractRepository;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var reviews = _reviewRepository.GetAll();
            return View(reviews);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int carId)
        {
            var review = new Review { CarId = carId };
            return View(review);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(Review review)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                TempData["Error"] = "You must be logged in to review a car.";
                return RedirectToAction("Details", "Car", new { id = review.CarId });
            }

            // Check contract
            var hasContract = _contractRepository.GetAll()
                .Any(c => c.CustomerId == userId &&
                          c.CarId == review.CarId &&
                          (c.Status == "Completed" || c.Status == "Active"));

            if (!hasContract)
            {
                TempData["Error"] = "You can only review cars you have rented or purchased.";
                return RedirectToAction("Details", "Car", new { id = review.CarId });
            }

            // Check if reviewed before
            var alreadyReviewed = _reviewRepository.GetAll()
                .Any(r => r.CarId == review.CarId && r.CustomerId == userId);

            if (alreadyReviewed)
            {
                TempData["Error"] = "You have already reviewed this car.";
                return RedirectToAction("Details", "Car", new { id = review.CarId });
            }

            
            review.CustomerId = userId;
            review.CreatedAt = DateTime.Now;

            if (ModelState.IsValid)
            {
                _reviewRepository.Add(review);
                _reviewRepository.Save();

                TempData["Success"] = "Review submitted successfully!";
                return RedirectToAction("Details", "Car", new { id = review.CarId });
            }

            return View(review);
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(Review review)
        {
            if (ModelState.IsValid)
            {
                _reviewRepository.Update(review);
                _reviewRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(review);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review == null)
            {
                return NotFound();
            }
            return View(review);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            var review = _reviewRepository.GetById(id);
            if (review != null)
            {
                _reviewRepository.Delete(review);
                _reviewRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
