using Microsoft.AspNetCore.Identity;

namespace RepeaterCouncil.Data
{
    // File: /Data/DbInitializer.cs
    public static class DbInitializer
    {
        public static async Task SeedExistingUsers(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var salt = context.Keys.SingleOrDefault()?.Key1;

                var existingUsers = context.Users.ToList(); 

                foreach (var oldUser in existingUsers)
                {
                    var userExists = await userManager.FindByNameAsync(oldUser.Callsign);
                    if (userExists == null)
                    {
                        var identityUser = new IdentityUser
                        {
                            UserName = oldUser.Callsign,
                            Email = oldUser.Email,
                            PhoneNumber = oldUser.PhoneCell,
                        };

                        var result = await userManager.CreateAsync(identityUser);

                        if (result.Succeeded)
                        {
                            identityUser.PasswordHash = Convert.ToBase64String(oldUser.Password);
                            await userManager.UpdateAsync(identityUser);
                        }
                        else
                        {
                            throw new Exception("Failed to create user: " + result.Errors.FirstOrDefault()?.Description);
                        }
                    }
                }
            }
        }

        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            // Resolve RoleManager
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Define the roles to be created
            string[] roleNames = { "Administrator", "Coordinator", "User" };

            // Iterate through the roles and create them if they do not exist
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }

}
