using System.Threading.Tasks;
using Cynosura.Studio.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cynosura.Studio.Data
{
    public class DatabaseInitializer : IDatabaseInitializer
    {
        private readonly DataContext _dataContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public DatabaseInitializer(DataContext dataContext, RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _dataContext = dataContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await _dataContext.Database.MigrateAsync();

            var administratorRoleName = "Administrator";
            if (await _roleManager.FindByNameAsync(administratorRoleName) == null)
            {
                await _roleManager.CreateAsync(new Role() { Name = administratorRoleName });
            }

            var administratorEmail = "admin@cynosura.ru";
            if (await _userManager.FindByNameAsync(administratorEmail) == null)
            {
                var user = new User()
                {
                    UserName = administratorEmail,
                    Email = administratorEmail,
                    EmailConfirmed = true
                };
                await _userManager.CreateAsync(user);
                await _userManager.AddToRoleAsync(user, administratorRoleName);
                await _userManager.AddPasswordAsync(user, "Admin123!");
            }
        }
    }
}
