using Microsoft.AspNetCore.Identity;

namespace CarFleet.Infrastructure.Models
{
    // Наслідуємося від IdentityUser згідно із завданням
    public class ApplicationUser : IdentityUser
    {
        // Тут можна додати додаткові поля користувача (наприклад, ім'я або дата народження)
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}