using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CarFleet.Common;
using CarFleet.Infrastructure.Models;
using CarFleet.REST.Models;

namespace CarFleet.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Шлях буде /api/fleets
    public class FleetsController : ControllerBase
    {
        private readonly ICrudServiceAsync<FleetModel> _crudService;

        public FleetsController(ICrudServiceAsync<FleetModel> crudService)
        {
            _crudService = crudService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var fleets = await _crudService.ReadAllAsync();
            var result = fleets.Select(f => new FleetRestModel
            {
                Id = f.Id,
                Name = f.Name,
                City = f.City
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var fleets = await _crudService.ReadAllAsync();
            var fleet = fleets.FirstOrDefault(f => f.Id == id);
            
            if (fleet == null) return NotFound();

            var result = new FleetRestModel
            {
                Id = fleet.Id,
                Name = fleet.Name,
                City = fleet.City
            };
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(FleetRestModel fleetRest)
        {
            var newFleet = new FleetModel
            {
                Name = fleetRest.Name,
                City = fleetRest.City
            };

            await _crudService.CreateAsync(newFleet);
            return CreatedAtAction(nameof(GetById), new { id = newFleet.Id }, fleetRest);
        }
    }
}