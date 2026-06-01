using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarFleet.Infrastructure;
using CarFleet.Infrastructure.Models;
using System.Threading.Tasks;

namespace CarFleet.MVC.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarFleetContext _context;

        public CarsController(CarFleetContext context)
        {
            _context = context;
        }

        // Відображення списку
        public async Task<IActionResult> Index()
        {
            var cars = await _context.Set<CarModel>().ToListAsync();
            return View(cars);
        }

        // Показ форми створення
        public IActionResult Create()
        {
            return View();
        }

        // Збереження даних у БД
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarModel car)
        {
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }
    }
}