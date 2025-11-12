using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CarHubProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        readonly IBrandRepository _brandRepository;
        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public IActionResult Index()
        {
            var brands = _brandRepository.GetAll();
            return View(brands);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (ModelState.IsValid)
            {
                _brandRepository.Add(brand);
                _brandRepository.Save();
                return RedirectToAction("Index", "Home");
            }
             return View();
        }

        // GET: Brand/Details/5
        public IActionResult Details(int id)
        {
            var brand = _brandRepository.GetById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // GET: Brand/Edit/5
        public IActionResult Edit(int id)
        {
            var brand = _brandRepository.GetById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brand/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Brand brand)
        {
            if (id != brand.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _brandRepository.Update(brand);
                _brandRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brand/Delete/5
        public IActionResult Delete(int id)
        {
            var brand = _brandRepository.GetById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var brand = _brandRepository.GetById(id);
            if (brand != null)
            {
                _brandRepository.Delete(brand);
                _brandRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
