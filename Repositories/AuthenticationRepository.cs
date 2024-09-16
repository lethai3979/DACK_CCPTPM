using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.AspNetCore.Identity;

namespace GoWheels_WebAPI.Repositories
{
    public class AuthenticationRepository : IUserRepository
    {
        public readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationRepository(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task AddUserToRoleAsync(ApplicationUser user, string roleName)
             => await _userManager.AddToRoleAsync(user, roleName);

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
            => await _userManager.CreateAsync(user, password);

        public async Task EnsureRoleExistsAsync(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        public async Task<ApplicationUser?> FindByUserNameAsync(string userName) 
            => await _userManager.FindByNameAsync(userName);

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user) 
            => await _userManager.GetRolesAsync(user);

        public async Task<bool> ValidatePasswordAsync(ApplicationUser user, string password)
            => await _userManager.CheckPasswordAsync(user, password);
    }
}
