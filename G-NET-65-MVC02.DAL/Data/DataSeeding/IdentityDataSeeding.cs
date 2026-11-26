using G_NET_65_MVC02.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace G_NET_65_MVC02.DAL.Data.DataSeeding
{
    public static class IdentityDataSeeding
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager
            ,ILogger logger, CancellationToken ct = default)
        {
			try
			{
                bool HasUsers = userManager.Users.Any();
                bool HasRoles = roleManager.Roles.Any();
                if (HasUsers && HasRoles) return;
                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>()
                    {
                        new IdentityRole(){Name = "SuperAdmin"},
                        new IdentityRole(){Name = "Admin"}
                    };
                    foreach (var roleName in Roles.Select(r => r.Name))
                    {
                        if (!await roleManager.RoleExistsAsync(roleName!))
                        {
                            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName!));
                            if(!roleResult.Succeeded)
                            {
                                logger.LogError("Failed To Create Role:{Role},{Errors}", roleName,
                                    string.Join(";",roleResult.Errors.Select(e => e.Description)));
                            }
                        }
                    }
                }


                if(!HasUsers)
                {
                    var MainAdmin = new ApplicationUser()
                    {
                        FirstName = "Omar",
                        LastName = "Mohamed",
                        UserName = "OmarMohamed",
                        Email = "OmarMohamed@gmail.com",
                        PhoneNumber = "01225891012"
                    };
                    await userManager.CreateAsync(MainAdmin,"P@ssw0rd");
                    await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");

                    var Admin01 = new ApplicationUser()
                    {
                        FirstName = "Ali",
                        LastName = "Nasser",
                        UserName = "AliNasser",
                        Email = "AliNasser@gmail.com",
                        PhoneNumber = "01225891012"
                    };
                    var CreateResult = await userManager.CreateAsync(Admin01, "P@ssw0rd");
                    if(!CreateResult.Succeeded)
                    {
                        logger.LogError("Failed To Create Seed SuperAdmin:{Errors}",
                            string.Join(";",CreateResult.Errors.Select(e => e.Description)));
                        return;
                    }
                    logger.LogInformation($"Seeded Admin01{Admin01.Email}");
                }
                return;
            }
            catch (Exception ex)
			{
                logger.LogError(ex,"Identity Seeding Failed");
                throw;
			}
        }
    }
}
