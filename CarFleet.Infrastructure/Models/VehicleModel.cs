using System;
using System.Collections.Generic;

namespace CarFleet.Infrastructure.Models
{
    public abstract class VehicleModel
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }

        // Зв'язок 1-до-1 (Один автомобіль має один двигун)
        public EngineModel Engine { get; set; }

        // Зв'язок 1-до-Багатьох (Один автопарк має багато автомобілів)
        public int FleetId { get; set; }
        public FleetModel Fleet { get; set; }

        // Зв'язок Багато-до-Багатьох (Автомобіль має багато водіїв, водій керує багатьма авто)
        public ICollection<DriverModel> Drivers { get; set; } = new List<DriverModel>();
    }
}