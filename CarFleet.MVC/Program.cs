using CarFleet.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; // Додано для міграцій

var builder = WebApplication.CreateBuilder(args);

// Додаємо підтримку MVC
builder.Services.AddControllersWithViews();

// Підключаємо нашу Базу Даних
builder.Services.AddDbContext<CarFleetContext>();

var app = builder.Build();

// --- МАГІЧНИЙ БЛОК ДЛЯ СТВОРЕННЯ БАЗИ ТА ТАБЛИЦЬ ---
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarFleetContext>();
    context.Database.Migrate(); 
}
// ----------------------------------------------------

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection(); // Залишаємо вимкненим для локальної роботи
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();