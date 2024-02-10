using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Movie.Business.ViewModels;
using Movie.Core.Models;

namespace Movie.Business.Services.ViewServices
{
    public class ManageLayoutService
    {
        private readonly UserManager<AppUser> _userManager;

        public ManageLayoutService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ManageLayoutVM> GetUserAndRoleAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return new ManageLayoutVM
            {
                User = user,
                Role = await _userManager.GetRolesAsync(user),
            };
        }
    }
}
