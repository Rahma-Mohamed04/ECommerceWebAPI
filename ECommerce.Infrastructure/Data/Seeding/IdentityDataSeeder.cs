using ECommerce.Domain.Contracts;
using ECommerce.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Data.Seeding
{
    public class IdentityDataSeeder : IDataSeeder
    {
        private readonly StoreIdentityDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityDataSeeder(StoreIdentityDbContext dbContext,
                                  UserManager<ApplicationUser> userManager,
                                  RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            // 1. Check and apply pending migrations
            var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync(ct);
            if (pendingMigrations.Any())
            {
                await _dbContext.Database.MigrateAsync(ct);
            }

            // 2. Seed Roles
            if (!await _roleManager.Roles.AnyAsync(ct))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            // 3. Seed Users
            if (!await _userManager.Users.AnyAsync(ct))
            {
                var admin = new ApplicationUser
                {
                    DisplayName = "Rahma Mohamed",
                    Email = "Rahma@gmail.com",
                    UserName = "RahmaMohamed",
                    PhoneNumber = "01123456789"
                };

                var createUser = await _userManager.CreateAsync(admin, "P@ssw0rd");

                if (createUser.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "SuperAdmin");
                }
                else
                {
                    foreach (var error in createUser.Errors)
                    {
                        Console.WriteLine(error.Description);
                    }
                }
            }
        }
    }
}
