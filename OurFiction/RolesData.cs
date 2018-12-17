using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OurFiction
{
    public static class RolesData
    {
        private static readonly string[] roles = new[] { "Admin", "User" };

        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (var role in roles)
            {
                var isRoleExist = await roleManager.RoleExistsAsync(role);
                if (!isRoleExist)
                {
                    var create = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!create.Succeeded)
                    {
                        throw new Exception("Failed to create role");
                    }
                }
            }
        }
    }
}
