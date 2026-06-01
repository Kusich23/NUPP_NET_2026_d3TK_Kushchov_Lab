using System.Collections.Generic;

namespace CarFleet.Infrastructure.Models
{
    public class DriverModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LicenseCategory { get; set; }

        // Зворотний зв'язок Багато-до-Багатьох
        public ICollection<VehicleModel> Vehicles { get; set; } = new List<VehicleModel>();
    }
}