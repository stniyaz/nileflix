using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;

namespace Movie.Business.Jobs
{
    public class CleanupTokenJob
    {
        private readonly IAccountService _accountService;

        public CleanupTokenJob(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public async Task Cleanup()
        {
            var usersToDelete = await _accountService.GetExpiredTokenUserAsync();
            await _accountService.DeleteUsersAsync(usersToDelete);
        }
    }
}
