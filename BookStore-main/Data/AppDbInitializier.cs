using BookStore.Data.Static;
using BookStore.Models;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Data
{
    public class AppDbInitializier
    {
        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder builder)
        {
            using (var applicationservices = builder.ApplicationServices.CreateScope())
            {
                #region Role
                var roleManager = applicationservices.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                }
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }
                if (!await roleManager.RoleExistsAsync(UserRoles.Vendor))
                {
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Vendor));
                }
                #endregion
                #region User
                var userManager = applicationservices.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                if (await userManager.FindByEmailAsync("admin@admin.com") == null)
                {
                    var newAdminUser = new ApplicationUser()
                    {
                        Email = "admin@admin.com",
                        EmailConfirmed = true,
                        UserName = "Admin"
                    };
                    await userManager.CreateAsync(newAdminUser, "@Dmin123");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);

                    if (await userManager.FindByEmailAsync("user@user.com") == null)
                    {
                        var newOridinalUser = new ApplicationUser()
                        {
                            Email = "user@user.com",
                            EmailConfirmed = true,
                            UserName = "TestUser"

                        };
                        await userManager.CreateAsync(newOridinalUser, "@User123");
                        await userManager.AddToRoleAsync(newOridinalUser, UserRoles.User);
                    }

                    if (await userManager.FindByEmailAsync("vendor@vendor.com") == null)
                    {
                        var newOridinalUser = new ApplicationUser()
                        {
                            Email = "vendor@vendor.com",
                            EmailConfirmed = true,
                            UserName = "TestVendor"
                        };
                        await userManager.CreateAsync(newOridinalUser, "@Vendor123");
                        await userManager.AddToRoleAsync(newOridinalUser, UserRoles.Vendor);
                    }
                    #endregion
                }
            }
        }
    }
}
