using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication
{
    public class RoleInitializer
    {
        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@admin.com";
            var password = "$Admin12345$";
            var t = nameof(RoleEnum.Administrator);
            if (await roleManager.FindByNameAsync(RoleEnum.Administrator.ToString()) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RoleEnum.Administrator.ToString()));
            }

            if (await roleManager.FindByNameAsync(nameof(RoleEnum.User)) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(RoleEnum.User.ToString()));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new IdentityUser { Email = adminEmail, UserName = adminEmail, NormalizedUserName = "Admin"};
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RoleEnum.Administrator.ToString());
                }
            }
        }
    }
}
