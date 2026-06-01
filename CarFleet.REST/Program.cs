using CarFleet.Common;
using CarFleet.Infrastructure;
using CarFleet.Infrastructure.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore; 
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models; 

var builder = WebApplication.CreateBuilder(args);

// Додаємо підтримку контролерів
builder.Services.AddControllers();

// Налаштування Swagger із "замочком"
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Вставте ваш accessToken сюди."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Впровадження залежностей (Dependency Injection)
builder.Services.AddDbContext<CarFleetContext>();
builder.Services.AddScoped<IRepository<CarModel>, Repository<CarModel>>();
builder.Services.AddScoped<IRepository<FleetModel>, Repository<FleetModel>>();
builder.Services.AddScoped<ICrudServiceAsync<CarModel>, DbCrudServiceAsync<CarModel>>();
builder.Services.AddScoped<ICrudServiceAsync<FleetModel>, DbCrudServiceAsync<FleetModel>>();

// Налаштування Identity та Авторизації
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>() 
    .AddEntityFrameworkStores<CarFleetContext>();

var app = builder.Build();

// БЛОК СТВОРЕННЯ РОЛЕЙ ТА ПРИЗНАЧЕННЯ АДМІНА
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    
    string[] roleNames = { "Admin", "Manager", "User" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Автоматично видаємо роль Admin користувачу admin@test.com
    var adminUser = await userManager.FindByEmailAsync("admin@test.com"); 
    if (adminUser != null && !await userManager.IsInRoleAsync(adminUser, "Admin"))
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }
}

// МАГІЧНИЙ БЛОК ДЛЯ СТВОРЕННЯ ТАБЛИЦЬ
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarFleetContext>();
    context.Database.Migrate(); 
}

// Вмикаємо Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Підключення аутентифікації та кінцевих точок Identity
app.UseAuthentication(); 
app.UseAuthorization();
app.MapIdentityApi<ApplicationUser>(); 

app.MapControllers();
app.Run();