using Microsoft.AspNetCore.Identity;
using Movie.Business.DTOs.UserDTOs;
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
        Task DeleteByNameAsync(string name);
        Task<List<IdentityRole>> GetRolesAsync();
        Task<AppUser> GetUserByNameAsync(string username);
        Task<AppUser> GetUserByEmailAsync(string mail);
        Task<string> GetUserRoleAsync(string id);
        Task<List<AppUser>> SearchByUsersAsync(string? search);
        Task<List<AppUser>> SearchByModsAsync(string? search);
        Task UpdateAsync(UserUpdateDTO dto);
        Task<ChangeMailVM> UserEditAsync(UserEditDTO dto);
        Task ChangePasswordAsync(string username, ChangePasswordVM model);
        Task<ResetPasswordVM> CheckEmailAsync(UserResetPasswordDTO dto);
        Task ResetPasswordAsync(ResetPasswordVM model);
        Task UserToPremiumAsync(string email);
    }
}
