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
        readonly UserManager<User> _userManager;

        public ReviewController(IReviewRepository reviewRepository, UserManager<User> userManager)
        {
            _reviewRepository = reviewRepository;
            _userManager = userManager;
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
                return Challenge();
            }

            review.CustomerId = userId;

            if (ModelState.IsValid)
            {
                _reviewRepository.Add(review);
                _reviewRepository.Save();
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
