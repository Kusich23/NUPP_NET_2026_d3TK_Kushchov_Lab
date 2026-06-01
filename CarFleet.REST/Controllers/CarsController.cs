using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarFleet.Common;
using CarFleet.Infrastructure.Models;
using CarFleet.REST.Models;

namespace CarFleet.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICrudServiceAsync<CarModel> _carService;
        private readonly ICrudServiceAsync<FleetModel> _fleetService; // Додали сервіс автопарків

        public CarsController(ICrudServiceAsync<CarModel> carService, ICrudServiceAsync<FleetModel> fleetService)
        {
            _carService = carService;
            _fleetService = fleetService;
        }

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
                FleetId = c.Fleet?.Id ?? 0 // Безпечно дістаємо ID автопарку
            });
            return Ok(result);
        }

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

        [HttpPost]
        public async Task<IActionResult> Create(CarRestModel carRest)
        {
            // 1. Шукаємо автопарк за переданим FleetId
            var allFleets = await _fleetService.ReadAllAsync();
            var targetFleet = allFleets.FirstOrDefault(f => f.Id == carRest.FleetId);

            if (targetFleet == null)
            {
                return BadRequest("Автопарк із таким FleetId не знайдено! Спочатку створіть автопарк.");
            }

            // 2. Створюємо машину і прив'язуємо її до знайденого автопарку
            var newCar = new CarModel
            {
                Id = Guid.NewGuid(),
                Brand = carRest.Brand,
                Year = carRest.Year,
                PassengerCapacity = carRest.PassengerCapacity,
                BodyType = carRest.BodyType,
                Fleet = targetFleet // Прив'язуємо!
            };

            await _carService.CreateAsync(newCar);
            return CreatedAtAction(nameof(GetById), new { id = newCar.Id }, carRest);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var car = await _carService.ReadAsync(id);
            if (car == null) return NotFound();

            await _carService.RemoveAsync(car);
            return NoContent();
        }
    }
}