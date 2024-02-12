using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie.Core.Models;

namespace Movie.Business.Jobs
{
    public class CleanupTokenJob
    {
        private readonly UserManager<AppUser> _userManager;

        public CleanupTokenJob(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task Cleanup()
        {
            var now = DateTime.UtcNow.AddHours(4);
            var usersToDelete = await _userManager.Users.Where(
                              x => now > (x.CreatedDate.AddMinutes(1)) &&
                              x.EmailConfirmed == false).ToListAsync();
            foreach (var user in usersToDelete)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}
