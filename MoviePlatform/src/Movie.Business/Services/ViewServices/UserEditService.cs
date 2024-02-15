using MailKit;
using Movie.Business.Services.Interfaces;

namespace Movie.Business.Services.ViewServices
{
    public class UserEditService
    {
        private readonly IAccountService _accountService;

        public UserEditService(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task<string> GetRoleAsync(string id)
        {
            return await _accountService.GetUserRoleAsync(id);
        }
    }
}
