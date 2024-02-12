using Microsoft.AspNetCore.Identity;
using Movie.Business.ViewModels;
using Movie.Core.Models;

namespace Movie.Business.Services.Interfaces
{
    public interface IAccountService
    {
        Task LoginAsync(LoginVM model);
        Task LogoutAsync();
        Task<ConfirmationVM> RegisterAsync(RegisterVM model);
        Task<List<AppUser>> GetExpiredTokenUserAsync();
        Task DeleteUserAsync(AppUser user);
        Task DeleteUsersAsync(List<AppUser> users);
    }
}
