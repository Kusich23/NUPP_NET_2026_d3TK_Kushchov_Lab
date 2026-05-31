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

        private static readonly Random _random = new Random();
        private static readonly string[] _brands = { "Tesla", "Toyota", "BMW", "Audi", "Ford" };

        public Car(string brand, int year, int passengerCapacity) : base(brand, year)
        {
            PassengerCapacity = passengerCapacity;
        }

        // Статичний метод для генерації випадкових об'єктів згідно завдання
        public static Car CreateNew()
        {
            string randomBrand = _brands[_random.Next(_brands.Length)];
            int randomYear = _random.Next(2000, 2024);
            int randomCapacity = _random.Next(2, 8);
            
            return new Car(randomBrand, randomYear, randomCapacity)
            {
                BodyType = "Sedan",
                IsElectric = _random.Next(0, 2) == 1
            };
        }
    }

    // Наслідування
    public class Truck : Vehicle
    {
        public double CargoCapacity { get; set; }
        public int NumberOfAxles { get; set; }
        public bool HasTrailer { get; set; }

        private static readonly Random _random = new Random();
        private static readonly string[] _brands = { "Volvo", "MAN", "Scania", "DAF" };

        public Truck(string brand, int year, double cargoCapacity) : base(brand, year)
        {
            CargoCapacity = cargoCapacity;
        }

        // Статичний метод для генерації випадкових вантажівок
        public static Truck CreateNew()
        {
            string randomBrand = _brands[_random.Next(_brands.Length)];
            int randomYear = _random.Next(2010, 2024);
            double randomCapacity = _random.NextDouble() * 20.0 + 5.0; // від 5 до 25 тонн
            
            return new Truck(randomBrand, randomYear, Math.Round(randomCapacity, 1));
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