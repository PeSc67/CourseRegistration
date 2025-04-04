using CourseRegistration.Models;
using CourseRegistration.Services;
using Microsoft.AspNetCore.Identity;

namespace DanceManagementSystem.Seed
{
    public static class TemporarySeedPrivateUsersForPeder
    {

        public static async Task Seed(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                await SeedUsers_PederSchulze(userManager);
            }




            // Seed three users with different roles for Peder Schulze
            static async Task SeedUsers_PederSchulze(UserManager<IdentityUser> userManager)  
            {
                
                IdentityUser[] pedersUsers =
                {
                        new IdentityUser { UserName = "Peder_SuperAdmin", Email = "Peder_SuperAdmin@test.com", PhoneNumber = "073-962 83 59"},
                        new IdentityUser { UserName = "Peder_Admin", Email = "Peder_Admin@test.com", PhoneNumber = "073-962 83 59"},
                        new IdentityUser { UserName = "Peder_User", Email = "Peder_User@test.com", PhoneNumber = "073-962 83 59"},
                };

                var password = "password";

                foreach (var usr in pedersUsers)
                {
                    IdentityUser user = new()
                    {
                        Email = usr.Email,
                        UserName = usr.UserName,
                        PhoneNumber = usr.PhoneNumber,
                    };


                    // Seed the user
                    IdentityResult userResult = await userManager.CreateAsync(user, password);

                    if (userResult.Succeeded)
                    {
                        // Add the role to that user
                        if (usr.UserName.Contains("SuperAdmin"))
                        {
                            userManager.AddToRoleAsync(user, role: "SuperAdmin").Wait();
                        }
                        else if (usr.UserName.Contains("LocalAdmin"))
                        {
                            userManager.AddToRoleAsync(user, role: "LocalAdmin").Wait();
                        }
                        else if (usr.UserName.Contains("AdminForUserModule"))
                        {
                            userManager.AddToRoleAsync(user, role: "AdminForUserModule").Wait();
                        }
                        else if (usr.UserName.Contains("AdminForCoarseModule"))
                        {
                            userManager.AddToRoleAsync(user, role: "AdminForCoarseModule").Wait();
                        }
                        else if (usr.UserName.Contains("AdminForCompetitionModule"))
                        {
                            userManager.AddToRoleAsync(user, role: "AdminForCompetitionModule").Wait();
                        }
                        else if (usr.UserName.Contains("Admin"))
                        {
                            userManager.AddToRoleAsync(user, role: "Admin").Wait();
                        }
                        else
                        {
                            userManager.AddToRoleAsync(user, role: "User").Wait();
                        }

                    }
                }
            }
        }
    }
}
