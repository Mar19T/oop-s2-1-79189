using Microsoft.AspNetCore.Identity;

namespace Library.MVC.Data
{
    public static class IdentitySeed
    {
        public static async Task SeedAdminUser(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userSup = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            string roleName = "Admin";
            string email = "admin@library.com";
            string password = "Admin123!";

            string SuproleName = "Supervisor";
            string Supemail = "Supervisor@library.com";
            string Suppassword = "SuperVisor123!";
            

            // Create role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Create admin user if it doesn't exist
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, roleName);
            }
            // Create Supervisor user if it doesn't exist
            var SupervisorUser = await userSup.FindByEmailAsync(Supemail);

            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = Supemail,
                    Email = Supemail,
                    EmailConfirmed = true
                };

                await userSup.CreateAsync(user, Suppassword);
                await userSup.AddToRoleAsync(user, SuproleName);
            }
        }
    }
}