using CourseRegistration.Models;
using CourseRegistration.Services;
using Microsoft.AspNetCore.Identity;

namespace DanceManagementSystem.Seed
{
    public static class SeedUserAndRoles
    {

        public static async Task Seed(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedRoles(roleManager);

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                await SeedUserAndGiveRoleToUser(userManager);


            }
        }



        // Seed three roles if they not exist
        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "SuperAdmin", "Admin", "User" };

            foreach (var role in roleNames)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    try
                    {
                        // Add the role
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }



        // Seed a user
        private static async Task SeedUserAndGiveRoleToUser(UserManager<IdentityUser> userManager)
        {
            var email = "SuperAdmin@DanseManagementSystem.se";
            var password = "*Qwerty123";
            var userName = "SuperAdmin";
            var phoneNumber = "";

            if (userManager.FindByEmailAsync(email).Result == null)
            {
                IdentityUser user = new()
                {
                    Email = email,
                    UserName = userName,
                    PhoneNumber = phoneNumber
                };


                // Seed the user
                IdentityResult userResult = await userManager.CreateAsync(user, password);

                if (userResult.Succeeded)
                {
                    // Add the role to that user
                    userManager.AddToRoleAsync(user, role: "SuperAdmin").Wait();
                }

            }
        }
    }
}
