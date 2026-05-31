using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CarFleet.Infrastructure;
using CarFleet.Infrastructure.Models;

namespace CarFleet.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Лабораторна робота 3: Entity Framework Core (SQLite) ===");

            // Створюємо контекст БД
            using var context = new CarFleetContext();
            await context.Database.MigrateAsync();
            
            // Ініціалізуємо Репозиторій та CRUD сервіс
            var repository = new Repository<CarModel>(context);
            var dbService = new DbCrudServiceAsync<CarModel>(repository);

            Console.WriteLine("\n[1] Додаємо автомобілі в базу даних SQLite...");
            
            // СТВОРЮЄМО АВТОПАРК (Щоб спрацював зв'язок FOREIGN KEY)
            var myFleet = new FleetModel { Name = "Головний Автопарк", City = "Київ" };

            // Прив'язуємо машини до цього автопарку
            var car1 = new CarModel { Id = Guid.NewGuid(), Brand = "Porsche", Year = 2024, PassengerCapacity = 2, BodyType = "Coupe", Fleet = myFleet };
            var car2 = new CarModel { Id = Guid.NewGuid(), Brand = "Toyota", Year = 2021, PassengerCapacity = 5, BodyType = "Sedan", Fleet = myFleet };

            await dbService.CreateAsync(car1);
            await dbService.CreateAsync(car2);
            Console.WriteLine("Автомобілі успішно збережено у БД!");

            Console.WriteLine("\n[2] --- Зчитування даних із Бази Даних ---");
            var allCars = await dbService.ReadAllAsync();
            
            foreach (var car in allCars)
            {
                Console.WriteLine($"- ID: {car.Id}");
                Console.WriteLine($"  Марка: {car.Brand}, Рік: {car.Year}");
                Console.WriteLine($"  Місткість: {car.PassengerCapacity} пас., Кузов: {car.BodyType}");
            }
            
            Console.WriteLine("\nРоботу з SQLite успішно завершено!");
        }
    }
}