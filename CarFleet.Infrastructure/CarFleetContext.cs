using Microsoft.EntityFrameworkCore;
using CarFleet.Infrastructure.Models;

namespace CarFleet.Infrastructure
{
    // Наслідуємося від DbContext згідно із завданням
    public class CarFleetContext : DbContext
    {
        // Це наші майбутні таблиці в базі даних
        public DbSet<VehicleModel> Vehicles { get; set; }
        public DbSet<CarModel> Cars { get; set; }
        public DbSet<TruckModel> Trucks { get; set; }
        public DbSet<EngineModel> Engines { get; set; }
        public DbSet<FleetModel> Fleets { get; set; }
        public DbSet<DriverModel> Drivers { get; set; }

        // Налаштування підключення до SQLite
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Вказуємо АБСОЛЮТНИЙ шлях. 
            // Тепер усі проєкти (Console, REST, MVC) будуть писати і читати один і той самий файл!
            optionsBuilder.UseSqlite(@"Data Source=/Users/maksserdiuk/NUPP_NET_2026_d3TK_Kushchov_Lab/CarFleet.Infrastructure/carfleet.db");
        }

        // Використання Fluent API для налаштування зв'язків та наслідування
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Table-per-Type (TPT) - Наслідування
            modelBuilder.Entity<VehicleModel>().ToTable("Vehicles");
            modelBuilder.Entity<CarModel>().ToTable("Cars");
            modelBuilder.Entity<TruckModel>().ToTable("Trucks");

            // 2. Зв'язок 1-до-1 (Vehicle <-> Engine)
            modelBuilder.Entity<VehicleModel>()
                .HasOne(v => v.Engine)
                .WithOne(e => e.Vehicle)
                .HasForeignKey<EngineModel>(e => e.VehicleId);

            // 3. Зв'язок 1-до-Багатьох (Fleet <-> Vehicles)
            modelBuilder.Entity<FleetModel>()
                .HasMany(f => f.Vehicles)
                .WithOne(v => v.Fleet)
                .HasForeignKey(v => v.FleetId);

            // 4. Зв'язок Багато-до-Багатьох (Vehicles <-> Drivers)
            modelBuilder.Entity<VehicleModel>()
                .HasMany(v => v.Drivers)
                .WithMany(d => d.Vehicles);
        }
    }
}