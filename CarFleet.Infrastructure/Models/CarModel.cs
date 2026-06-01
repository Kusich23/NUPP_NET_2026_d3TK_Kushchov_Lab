namespace CarFleet.Infrastructure.Models
{
    public class CarModel : VehicleModel
    {
        public int PassengerCapacity { get; set; }
        public string BodyType { get; set; }
        public bool IsElectric { get; set; }
    }
}