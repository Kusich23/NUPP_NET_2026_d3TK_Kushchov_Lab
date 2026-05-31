using System;
using CarFleet.Common;

namespace CarFleet.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Автопарк: Демонстрація роботи ===");

            var carService = new CrudService<Vehicle>();

            var myCar = new Car("Tesla Model 3", 2022, 5);
            var myTruck = new Truck("Volvo FH16", 2020, 20.5);

            carService.Create(myCar);
            carService.Create(myTruck);
            Console.WriteLine("Автомобілі додані у сервіс.\n");

            Console.WriteLine("Список транспорту (ReadAll):");
            foreach (var vehicle in carService.ReadAll())
            {
                vehicle.PrintVehicleInfo();
            }

            Console.WriteLine("\nТест двигуна (Делегати та Події):");
            var engine = new Engine(500, 4.0, "Diesel");
            engine.EngineStarted += (msg) => Console.WriteLine($"[Подія] {msg}");
            engine.StartEngine();

            string filePath = "fleet_data.json";
            carService.Save(filePath);
            Console.WriteLine($"\nДані збережено у файл: {filePath}");

            Console.WriteLine();
            Vehicle.ShowTotalVehicles();
        }
    }
}