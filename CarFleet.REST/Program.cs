using CarFleet.Common;
using CarFleet.Infrastructure;
using CarFleet.Infrastructure.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; // Додано для роботи з базою даних

var builder = WebApplication.CreateBuilder(args);

// Додаємо підтримку контролерів
builder.Services.AddControllers();

// Налаштування Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Впровадження залежностей (Dependency Injection)
builder.Services.AddDbContext<CarFleetContext>();
builder.Services.AddScoped<IRepository<CarModel>, Repository<CarModel>>();
builder.Services.AddScoped<IRepository<FleetModel>, Repository<FleetModel>>();
builder.Services.AddScoped<ICrudServiceAsync<CarModel>, DbCrudServiceAsync<CarModel>>();
builder.Services.AddScoped<ICrudServiceAsync<FleetModel>, DbCrudServiceAsync<FleetModel>>();

var app = builder.Build();

// --- МАГІЧНИЙ БЛОК ДЛЯ СТВОРЕННЯ ТАБЛИЦЬ ---
// Цей код гарантує, що база даних і всі таблиці будуть створені при запуску сервера
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarFleetContext>();
    context.Database.Migrate(); 
}
// ---------------------------------------------

// Вмикаємо Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();