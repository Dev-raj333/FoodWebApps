using FoodDonationWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FoodDonationWebApp.Seed
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            // Get the RoleManager from the service provider
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Define the roles that you want to seed into the system
            string[] roleNames = { "Admin", "Recipient", "Doner", "Volunture" };

            // Loop over the roleNames and ensure each role exists
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Create the role without manually setting the Id (handled automatically)
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception($"Error creating role: {roleName}");
                    }
                }
            }


            // Create a default admin user
            var adminEmail = "admin1@admin.com";
            var adminPassword = "Admin@123";
            var FirstName = "Admin";
            var LastName = "Admin";

            // Check if the admin user already exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                // If the admin doesn't exist, create it
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true, // Set the email as confirmed
                    FirstName = FirstName,
                    LastName = LastName
                };

                var createAdmin = await userManager.CreateAsync(adminUser, adminPassword);
                if (createAdmin.Succeeded)
                {
                    // Add the newly created admin to the "Admin" role
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new Exception("Failed to create the admin user.");
                }
            }
        }
    }
}
