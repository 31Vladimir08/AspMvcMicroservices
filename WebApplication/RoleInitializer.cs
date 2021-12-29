using Microsoft.AspNetCore.Identity;

namespace WebApplication
{
    public class RoleInitializer
    {
        public static void InitializeAsync(UserManager<IdentityUser> userManager)
        {
            var adminEmail = "admin@admin.com";
            var password = "$Admin12345$";

            if (userManager.FindByEmailAsync("admin@admin.com").Result != null) 
                return;
            var user = new IdentityUser
            {
                Email = adminEmail,
                UserName = adminEmail,
                NormalizedUserName = "Admin"
            };

            var result = userManager.CreateAsync(user, password).Result;

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, nameof(RoleEnum.Administrator)).Wait();
            }
        }
    }
}
