using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CarHubProject.Controllers
{
    public class BrandController : Controller
    {
        readonly IBrandRepository _brandRepository;
        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        public IActionResult Index()
        {
            return View();
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
        public IActionResult Delete(int id)
        {
            var delBrand = _brandRepository.GetById(id);
            _brandRepository.Delete(delBrand);
            _brandRepository.Save();
            return RedirectToAction("Index");
        }
    }
}
