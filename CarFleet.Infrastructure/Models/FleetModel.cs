using System.Collections.Generic;

namespace CarFleet.Infrastructure.Models
{
    public class FleetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }

        // Зворотний зв'язок 1-до-Багатьох
        public ICollection<VehicleModel> Vehicles { get; set; } = new List<VehicleModel>();
    }
}