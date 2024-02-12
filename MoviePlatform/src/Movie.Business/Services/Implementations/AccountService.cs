using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Services.Interfaces;
using Movie.Business.ViewModels;
using Movie.Core.Models;

namespace Movie.Business.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountService(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        string loginFailMessage = "Sorry, login failed. Please note that if there are too many incorrect attempts, your account will be temporarily blocked.";
        public async Task LoginAsync(LoginVM model)
        {
            AppUser? user = null;

            user = await _userManager.FindByNameAsync(model.UsernameOrMail)
                ?? await _userManager.FindByEmailAsync(model.UsernameOrMail);

            if (user is null)
                throw new UserInvalidCredentialsException(loginFailMessage);


            if (await _userManager.CheckPasswordAsync(user, model.Password) &&
                user.IsBanned)
            {
                throw new BannedUserException("Your account has been permanently banned. If you think there is an error, please contact us.");
            }

            if (await _userManager.CheckPasswordAsync(user, model.Password) &&
                user.LockoutEnd <= DateTime.UtcNow.AddHours(4))
            {
                user.LockoutEnd = null;
                await _userManager.UpdateAsync(user);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);
            string messages = string.Empty;

            if (!result.Succeeded)
                throw new UserInvalidCredentialsException(loginFailMessage);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<ConfirmationVM> RegisterAsync(RegisterVM model)
        {
            if (!model.Privacy)
                throw new UnacceptablePrivacyException("Please accept the agreement to complete registration.");

            AppUser user = null;

            // check
            user = await _userManager.FindByNameAsync(model.UserName.Trim());
            if (user is not null)
                throw new ExistUsernameException("UserName", "This username is already taken. Please enter another username.");
            user = await _userManager.FindByEmailAsync(model.Email.Trim());
            if (user is not null)
                throw new ExistEmailException("Email", "This email address is already taken. Please enter another e-mail address.");
            // create user
            AppUser newUser = new AppUser
            {
                UserName = model.UserName.Trim(),
                Email = model.Email.Trim(),
                FirstName = model.FirstName.Trim(),
                LastName = model.LastName.Trim(),
                CreatedDate = DateTime.UtcNow.AddHours(4)
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);
            if (!result.Succeeded)
            {
                throw new UnexceptedException(result.Errors?.FirstOrDefault()?.Description);
            }

            await _userManager.AddToRoleAsync(newUser, "User");

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            var email = newUser.Email;
            return new ConfirmationVM() { Token = token, Email = email };
        }

        public async Task<List<AppUser>> GetExpiredTokenUserAsync()
        {
            var now = DateTime.UtcNow.AddHours(4);
            return await _userManager.Users.Where(
                              x => now > (x.CreatedDate.AddMinutes(15)) &&
                              x.EmailConfirmed == false).ToListAsync();
        }

        public async Task DeleteUserAsync(AppUser user)
        {
            var wantedUser = await _userManager.FindByIdAsync(user.Id);
            if (wantedUser != null)
            {
                await _userManager.DeleteAsync(wantedUser);
            }
        }

        public async Task DeleteUsersAsync(List<AppUser> users)
        {
            if (users is not null && users.Count > 0)
            {
                foreach (var userId in users.Select(u => u.Id))
                {
                    var wantedUser = await _userManager.FindByIdAsync(userId);
                    if (wantedUser is not null)
                    {
                        await _userManager.DeleteAsync(wantedUser);
                    }
                }
            }
        }

        public async Task<List<AppUser>> SearchByUsersAsync(string? search)
        {
            return await GetByRoleAsync(search, "user");
        }

        public async Task<List<AppUser>> SearchByModsAsync(string? search)
        {
            return await GetByRoleAsync(search, "moderator");
        }

        public async Task DeleteByNameAsync(string name)
        {
            var wantedUser = await _userManager.FindByNameAsync(name);
            if (wantedUser is null)
                throw new UserNotFoundException();

            await _userManager.DeleteAsync(wantedUser);
        }

        public async Task<List<IdentityRole>> GetRolesAsync()
        {
            return await _roleManager.Roles.Where(x => x.Name != "Admin").ToListAsync();
        }

        public async Task<AppUser> GetUserByNameAsync(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }


        private async Task<List<AppUser>> GetByRoleAsync(string? search, string role)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);
            if (!string.IsNullOrEmpty(search))
            {
                if (search.Length >= 2)
                    users = users.Where(x => x.UserName.Trim().ToLower().Contains(search.Trim())
                                          || x.Email.Trim().ToLower().Contains(search.Trim())
                                          || x.FirstName.Trim().ToLower().Contains(search.Trim())
                                          || x.LastName.Trim().ToLower().Contains(search.Trim()))
                                          .ToList();
                else
                    throw new InvalidSearchException();
            }
            return users.ToList();
        }

    }
}
