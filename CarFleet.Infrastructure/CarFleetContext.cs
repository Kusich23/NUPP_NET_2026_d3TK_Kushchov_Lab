using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CarFleet.Infrastructure.Models;

namespace CarFleet.Infrastructure
{
    // Наслідуємося від IdentityDbContext замість DbContext
    public class CarFleetContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<VehicleModel> Vehicles { get; set; }
        public DbSet<CarModel> Cars { get; set; }
        public DbSet<TruckModel> Trucks { get; set; }
        public DbSet<EngineModel> Engines { get; set; }
        public DbSet<FleetModel> Fleets { get; set; }
        public DbSet<DriverModel> Drivers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Зберігаємо твій абсолютний шлях, щоб база не занулялася
            optionsBuilder.UseSqlite(@"Data Source=/Users/maksserdiuk/NUPP_NET_2026_d3TK_Kushchov_Lab/CarFleet.Infrastructure/carfleet.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ЦЕЙ РЯДОК ОБОВ'ЯЗКОВИЙ: він ініціалізує таблиці Identity (користувачі, ролі тощо)
            base.OnModelCreating(modelBuilder);

            // Твоє налаштування зв'язків з попередніх лабораторних
            modelBuilder.Entity<VehicleModel>().ToTable("Vehicles");
            modelBuilder.Entity<CarModel>().ToTable("Cars");
            modelBuilder.Entity<TruckModel>().ToTable("Trucks");

            modelBuilder.Entity<VehicleModel>()
                .HasOne(v => v.Engine)
                .WithOne(e => e.Vehicle)
                .HasForeignKey<EngineModel>(e => e.VehicleId);

            modelBuilder.Entity<FleetModel>()
                .HasMany(f => f.Vehicles)
                .WithOne(v => v.Fleet)
                .HasForeignKey(v => v.FleetId);

            modelBuilder.Entity<VehicleModel>()
                .HasMany(v => v.Drivers)
                .WithMany(d => d.Vehicles);
        }
    }
}