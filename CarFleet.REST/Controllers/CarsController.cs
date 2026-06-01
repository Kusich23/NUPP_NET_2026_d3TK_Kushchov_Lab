using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; // ДОДАНО: Простір імен для авторизації
using CarFleet.Common;
using CarFleet.Infrastructure.Models;
using CarFleet.REST.Models;
using Microsoft.AspNetCore.Identity;

namespace CarFleet.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // ДОДАНО: Тепер весь контролер за замовчуванням вимагає наявності токена (авторизації)
    [Authorize] 
    public class CarsController : ControllerBase
    {
        private readonly ICrudServiceAsync<CarModel> _carService;
        private readonly ICrudServiceAsync<FleetModel> _fleetService;

        public CarsController(ICrudServiceAsync<CarModel> carService, ICrudServiceAsync<FleetModel> fleetService)
        {
            _carService = carService;
            _fleetService = fleetService;
        }

        // Доступно всім авторизованим користувачам (навіть зі звичайною роллю "User", бо на рівні класу стоїть [Authorize])
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cars = await _carService.ReadAllAsync();
            var result = cars.Select(c => new CarRestModel
            {
                Id = c.Id,
                Brand = c.Brand,
                Year = c.Year,
                PassengerCapacity = c.PassengerCapacity,
                BodyType = c.BodyType,
                FleetId = c.Fleet?.Id ?? 0 
            });
            return Ok(result);
        }

        // Доступно всім авторизованим користувачам
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var car = await _carService.ReadAsync(id);
            if (car == null) return NotFound();

            var result = new CarRestModel
            {
                Id = car.Id,
                Brand = car.Brand,
                Year = car.Year,
                PassengerCapacity = car.PassengerCapacity,
                BodyType = car.BodyType,
                FleetId = car.Fleet?.Id ?? 0
            };
            return Ok(result);
        }

        // ДОДАНО: Створювати можуть тільки Адміни або Менеджери
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")] 
        public async Task<IActionResult> Create(CarRestModel carRest)
        {
            var allFleets = await _fleetService.ReadAllAsync();
            var targetFleet = allFleets.FirstOrDefault(f => f.Id == carRest.FleetId);

            if (targetFleet == null)
            {
                return BadRequest("Автопарк із таким FleetId не знайдено! Спочатку створіть автопарк.");
            }

            var newCar = new CarModel
            {
                Id = Guid.NewGuid(),
                Brand = carRest.Brand,
                Year = carRest.Year,
                PassengerCapacity = carRest.PassengerCapacity,
                BodyType = carRest.BodyType,
                Fleet = targetFleet 
            };

            await _carService.CreateAsync(newCar);
            return CreatedAtAction(nameof(GetById), new { id = newCar.Id }, carRest);
        }

        // ДОДАНО: Видаляти може ТІЛЬКИ Адмін
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var car = await _carService.ReadAsync(id);
            if (car == null) return NotFound();

            await _carService.RemoveAsync(car);
            return NoContent();
        }
    }
}