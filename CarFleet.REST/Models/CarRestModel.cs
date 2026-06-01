using System;

namespace CarFleet.REST.Models
{
    public class CarRestModel
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }
        public int PassengerCapacity { get; set; }
        public string BodyType { get; set; }
        public int FleetId { get; set; }
    }
}