using Microsoft.AspNetCore.Identity;
using Movie.Business.Services.Interfaces;
using Movie.Business.ViewModels;
using Movie.Core.Models;

namespace Movie.Business.Services.ViewServices
{
    public class LayoutService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountService _accountService;

        public LayoutService(UserManager<AppUser> userManager,
                             IAccountService accountService)
        {
            _userManager = userManager;
            _accountService = accountService;
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
        public async Task<string> GetUserFullName(string name)
        {
            var user = await _accountService.GetUserByNameAsync(name);

            return user.FirstName + " " + user.LastName;
        }
    }
}
