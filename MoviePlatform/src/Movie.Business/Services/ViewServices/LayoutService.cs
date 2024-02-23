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
        private readonly ISettingService _settingService;

        public LayoutService(UserManager<AppUser> userManager,
                             IAccountService accountService,
                             ISettingService settingService)
        {
            _userManager = userManager;
            _accountService = accountService;
            _settingService = settingService;
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
        public async Task<List<Setting>> GetSettings()
        {
            return await _settingService.GetAllAsync();
        }
    }
}
