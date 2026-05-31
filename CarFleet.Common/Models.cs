using System;

namespace CarFleet.Common
{
    public interface IIdentifiable
    {
        Guid Id { get; set; }
    }

    public class Vehicle : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }

        // 1. Статичні поля
        public static int TotalVehiclesCount;

        // 2. Статичні конструктори
        static Vehicle()
        {
            TotalVehiclesCount = 0;
        }

        // 3. Конструктори
        public Vehicle(string brand, int year)
        {
            Id = Guid.NewGuid();
            Brand = brand;
            Year = year;
            TotalVehiclesCount++;
        }

        // 4. Методи
        public virtual void Drive()
        {
            Console.WriteLine($"{Brand} починає рух.");
        }

        // 5. Статичні методи
        public static void ShowTotalVehicles()
        {
            Console.WriteLine($"Всього створено транспортних засобів: {TotalVehiclesCount}");
        }
    }

    // Наслідування
    public class Car : Vehicle
    {
        public int PassengerCapacity { get; set; }
        public string BodyType { get; set; }
        public bool IsElectric { get; set; }

        public Car(string brand, int year, int passengerCapacity) : base(brand, year)
        {
            PassengerCapacity = passengerCapacity;
        }
    }

    // Наслідування
    public class Truck : Vehicle
    {
        public double CargoCapacity { get; set; }
        public int NumberOfAxles { get; set; }
        public bool HasTrailer { get; set; }

        public Truck(string brand, int year, double cargoCapacity) : base(brand, year)
        {
            CargoCapacity = cargoCapacity;
        }
    }

    public class Engine
    {
        public int HorsePower { get; set; }
        public double Volume { get; set; }
        public string FuelType { get; set; }

        // 6. Делегати
        public delegate void EngineStateHandler(string message);

        // 7. Події
        public event EngineStateHandler EngineStarted;

        public Engine(int horsePower, double volume, string fuelType)
        {
            HorsePower = horsePower;
            Volume = volume;
            FuelType = fuelType;
        }

        public void StartEngine()
        {
            EngineStarted?.Invoke($"Двигун на {HorsePower} к.с. запущено!");
        }
    }

    public static class VehicleExtensions
    {
        // 8. Метод розширення
        public static void PrintVehicleInfo(this Vehicle vehicle)
        {
            Console.WriteLine($"Транспорт: {vehicle.Brand}, Рік: {vehicle.Year}, ID: {vehicle.Id}");
        }
    }
}