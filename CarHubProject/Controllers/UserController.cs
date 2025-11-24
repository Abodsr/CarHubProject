using CarHubProject.Models;
using CarHubProject.Repositories;
using CarHubProject.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarHubProject.Controllers
{
    [Authorize(Roles = "Admin")]

    
    public class UserController : Controller
    {

        private readonly AppDbContext _context;
        private readonly ICarRepository _carRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly UserManager<User> _userManager;

        public UserController(
        ICarRepository carRepository,
        IContractRepository contractRepository,
        IBrandRepository brandRepository,
        AppDbContext context,
        UserManager<User> userManager
          )
        {
            _context = context;
            _carRepository = carRepository;
            _contractRepository = contractRepository;
            _brandRepository = brandRepository;
            _userManager = userManager;
        }


        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users.ToListAsync());
        }



        [AllowAnonymous]
        public IActionResult UserDashboard()
        {

            string userId = _userManager.GetUserId(User);



            var userContracts = _contractRepository
               .GetAll()
               .Where(c => c.CustomerId == userId)
               .ToList();


            var userCars = _carRepository
                .GetAll()
                .Where(car => userContracts.Any(contract => contract.CarId == car.Id))
                .ToList();


            var viewModel = new UserDashboardViewModel
            {
                UserContracts = userContracts,
                UserCars = userCars
            };

            return View(viewModel);
        }


        // GET: User/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FullName,Address,NationalId,LicenseNumber,CreatedAt,IsActive,IsDeleted")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Address,NationalId,LicenseNumber,CreatedAt,IsActive,IsDeleted")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
