using System;

namespace CarFleet.Infrastructure.Models
{
    public class EngineModel
    {
        public Guid Id { get; set; }
        public int HorsePower { get; set; }
        public double Volume { get; set; }
        public string FuelType { get; set; }

        // Зворотний зв'язок 1-до-1
        public Guid VehicleId { get; set; }
        public VehicleModel Vehicle { get; set; }
    }
}