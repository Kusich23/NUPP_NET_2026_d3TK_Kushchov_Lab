using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CarFleet.Common;

namespace CarFleet.ConsoleApp
{
    class Program
    {
        // Примітиви синхронізації згідно завдання
        private static readonly object _lockObject = new object();
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(2); // Допускаємо лише 2 потоки одночасно
        private static readonly AutoResetEvent _autoEvent = new AutoResetEvent(false);

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== Лабораторна робота 2: Асинхронність та Багатопотоковість ===");

            var carService = new CrudServiceAsync<Car>("cars_data.json");

            // 1. Паралельне створення понад 1000 об'єктів (Клас Parallel)
            Console.WriteLine("Генеруємо 1000 автомобілів паралельно...");
            Parallel.For(0, 1000, i =>
            {
                var newCar = Car.CreateNew();
                carService.CreateAsync(newCar).Wait();
            });

            var allCars = (await carService.ReadAllAsync()).ToList();
            Console.WriteLine($"Успішно створено {allCars.Count} автомобілів.\n");

            // 2. Використання LINQ (Мінімальні, максимальні та середні значення)
            int minCap = allCars.Min(c => c.PassengerCapacity);
            int maxCap = allCars.Max(c => c.PassengerCapacity);
            double avgCap = allCars.Average(c => c.PassengerCapacity);
            int oldestYear = allCars.Min(c => c.Year);

            Console.WriteLine("--- Аналіз даних (LINQ) ---");
            Console.WriteLine($"Мiнiмальна мiсткiсть: {minCap} пас.");
            Console.WriteLine($"Максимальна мiсткiсть: {maxCap} пас.");
            Console.WriteLine($"Середня мiсткiсть: {Math.Round(avgCap, 2)} пас.");
            Console.WriteLine($"Найстарший рiк випуску: {oldestYear}\n");

            // 3. Тест Пагінації
            var page2 = await carService.ReadAllAsync(page: 2, amount: 5);
            Console.WriteLine("--- Пагінація (Сторінка 2, Розмір 5) ---");
            foreach (var car in page2)
            {
                Console.WriteLine($"- {car.Brand}, Рiк: {car.Year}");
            }

            // 4. Демонстрація примітивів синхронізації
            Console.WriteLine("\n--- Тест примітивів синхронізації ---");
            Task.Run(() => TestSyncPrimitives());
            
            _autoEvent.WaitOne(); // Головний потік чекає на сигнал від AutoResetEvent
            Console.WriteLine("Головний потік отримав сигнал від AutoResetEvent!");

            // 5. Асинхронне збереження
            bool isSaved = await carService.SaveAsync();
            Console.WriteLine($"\nДані успішно збережено у файл: {isSaved}");
        }

        // Метод для демонстрації lock та Semaphore
        static void TestSyncPrimitives()
        {
            Parallel.For(0, 5, i =>
            {
                _semaphore.Wait(); // Обмежуємо доступ (до 2 потоків)
                try
                {
                    lock (_lockObject) // Блокуємо доступ для інших потоків
                    {
                        Console.WriteLine($"[Lock/Semaphore] Потік {Task.CurrentId} виконує безпечну роботу.");
                        Thread.Sleep(200); // Імітація роботи
                    }
                }
                finally
                {
                    _semaphore.Release();
                }
            });
            
            _autoEvent.Set(); // Надсилаємо сигнал головному потоку
        }
    }
}