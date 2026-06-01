namespace CarFleet.Infrastructure.Models
{
    public class TruckModel : VehicleModel
    {
        public double CargoCapacity { get; set; }
        public int NumberOfAxles { get; set; }
        public bool HasTrailer { get; set; }
    }
}