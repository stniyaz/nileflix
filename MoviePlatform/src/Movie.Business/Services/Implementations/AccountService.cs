using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie.Business.CustomExceptions.CommonExceptions;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.DTOs.UserDTOs;
using Movie.Business.Services.Interfaces;
using Movie.Business.ViewModels;
using Movie.Core.Models;
using System.Linq.Expressions;

namespace Movie.Business.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IEarningService _earningService;

        public AccountService(UserManager<AppUser> userManager,
                              SignInManager<AppUser> signInManager,
                              RoleManager<IdentityRole> roleManager,
                              IMapper mapper,
                              IHttpContextAccessor httpContext,
                              IEarningService earningService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _httpContext = httpContext;
            _earningService = earningService;
        }
        private string loginFailMessage = "Sorry, login failed. Please note that if there are too many incorrect attempts, your account will be temporarily blocked.";
        private string existMailMessage = "This email address is already taken. Please enter another e-mail address.";
        private string existUsernameMessage = "This username is already taken. Please enter another username.";
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

            AppUser? user = null;

            // check
            user = await _userManager.FindByNameAsync(model.UserName.Trim());
            if (user is not null)
                throw new ExistUsernameException("UserName", existUsernameMessage);
            user = await _userManager.FindByEmailAsync(model.Email.Trim());
            if (user is not null)
                throw new ExistEmailException("Email", existMailMessage);
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
                await _userManager.UpdateSecurityStampAsync(wantedUser);
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
            var users = await _userManager.GetUsersInRoleAsync("User");
            var activeUsers = users.Where(x => !x.IsBanned).ToList();

            return await SearchUser(activeUsers, search);
        }

        public async Task<List<AppUser>> SearchByBannedUsersAsync(string? search)
        {
            var users = await _userManager.Users.Where(x => x.IsBanned).ToListAsync();

            return await SearchUser(users, search);
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
            return await _userManager.Users.Include(x => x.Comments).Include(x => x.UserSavedMovies).FirstOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<string> GetUserRoleAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var role = await _roleManager.FindByNameAsync(roles.FirstOrDefault());
            return role.Id;
        }

        public async Task UpdateAsync(UserUpdateDTO dto)
        {
            // check
            var existUser = await _userManager.FindByIdAsync(dto.Id);
            if (existUser is null)
                throw new UserNotFoundException();

            if (_userManager.Users.Any(x => x.Email == dto.Email.Trim() &&
                                            x.Id != dto.Id))
                throw new ExistEmailException("Email", existMailMessage);

            if (_userManager.Users.Any(x => x.UserName == dto.UserName.Trim() &&
                                            x.Id != dto.Id))
                throw new ExistUsernameException("UserName", existUsernameMessage);

            var role = await _roleManager.FindByIdAsync(dto.RoleId);
            if (role is null)
                throw new InvalidRoleIdException("RoleId", "Role not found.");

            // remove role
            var userRoles = await _userManager.GetRolesAsync(existUser);
            await _userManager.RemoveFromRolesAsync(existUser, userRoles);

            // add role
            await _userManager.AddToRoleAsync(existUser, role.Name);

            // trim string properties
            foreach (var property in dto.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(dto);
                    if (value != null)
                    {
                        value = value.Trim();
                        property.SetValue(dto, value);
                    }
                }
            }
            // map and update
            existUser = _mapper.Map(dto, existUser);
            var result = await _userManager.UpdateAsync(existUser);
        }

        public async Task ChangePasswordAsync(string username, ChangePasswordVM model)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user is null)
                throw new UserNotFoundException();

            var check = await _userManager.CheckPasswordAsync(user, model.OldPassword);

            if (!check) throw new UserInvalidCredentialsException("OldPassword", "The current password is incorrect.");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
                throw new UnexceptedException(result.Errors.FirstOrDefault().Description);

        }

        public async Task<ChangeMailVM> UserEditAsync(UserEditDTO dto)
        {
            var existUser = await _userManager.FindByIdAsync(dto.Id);
            if (existUser is null)
                throw new UserNotFoundException();

            if (_userManager.Users.Any(x => x.Email == dto.Email.Trim() &&
                                            x.Id != dto.Id))
                throw new ExistEmailException("Email", existMailMessage);

            if (_userManager.Users.Any(x => x.UserName == dto.UserName.Trim() &&
                                            x.Id != dto.Id))
                throw new ExistUsernameException("UserName", existUsernameMessage);

            // trim string properties
            foreach (var property in dto.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(dto);
                    if (value != null)
                    {
                        value = value.Trim();
                        property.SetValue(dto, value);
                    }
                }
            }

            existUser.UserName = dto.UserName;
            existUser.FirstName = dto.FirstName;
            existUser.LastName = dto.LastName;
            existUser.UpdatedDate = DateTime.UtcNow.AddHours(4);
            await _userManager.UpdateAsync(existUser);
            await _signInManager.SignInAsync(existUser, isPersistent: true);

            if (existUser.Email.ToLower() != dto.Email.ToLower())
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(existUser, dto.Email);
                return new ChangeMailVM { UserId = dto.Id, NewMail = dto.Email, Token = token, IsChanged = true };
            }
            return new ChangeMailVM { IsChanged = false };
        }
        public async Task<ResetPasswordVM> CheckEmailAsync(UserResetPasswordDTO dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new UserNotFoundException("Email", "Please check the e-mail address again because there is no such e-mail address in our system.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return new ResetPasswordVM { Token = token, Email = user.Email };
        }
        public async Task ResetPasswordAsync(ResetPasswordVM model)
        {
            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Token))
                throw new NullDatasException();

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user is null) throw new UserNotFoundException();

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded) throw new UnexceptedException(result?.Errors?.FirstOrDefault()?.Description);
            user.UpdatedDate = DateTime.UtcNow.AddHours(4);
            await _userManager.UpdateAsync(user);
        }

        private async Task<List<AppUser>> SearchUser(List<AppUser> users, string? search)
        {
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
            return users;
        }

        public async Task<AppUser> GetUserByEmailAsync(string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user is null) throw new UserNotFoundException();
            return user;
        }
        public async Task<List<AppUser>> GetUsersAsync(Expression<Func<AppUser, bool>>? expression = null,
                                                       params string[]? includes)
        {
            return await _userManager.Users.Where(expression).Include(expression).ToListAsync();
        }

        public async Task UserToPremiumAsync(string userId, int amount)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new UserNotFoundException();

            switch (amount)
            {
                case 1399:
                    user.PremiumStartDate = CheckStartPremium(user);
                    user.PremiumEndDate = CheckEndPremium(user, 1);
                    break;
                case 2399:
                    user.PremiumStartDate = CheckStartPremium(user);
                    user.PremiumEndDate = CheckEndPremium(user, 2);
                    break;
                case 3399:
                    user.PremiumStartDate = CheckStartPremium(user);
                    user.PremiumEndDate = CheckEndPremium(user, 3);
                    break;
                default:
                    throw new UnexceptedException();
            }
            await _earningService.CreateAsync(amount, userId);
            user.IsPremium = true;
            await _userManager.UpdateAsync(user);
        }

        private DateTime CheckEndPremium(AppUser user, int month)
        {
            DateTime sum;
            if (user.IsPremium && user.PremiumEndDate is not null)
            {
                sum = user.PremiumEndDate.Value.AddMonths(month).AddHours(4);
            }
            else
            {
                sum = DateTime.UtcNow.AddMonths(month).AddHours(4);
            }
            return sum;
        }
        private DateTime CheckStartPremium(AppUser user)
        {
            if (user.PremiumStartDate is not null)
            {
                return user.PremiumStartDate.Value;
            }
            return DateTime.UtcNow.AddHours(4);
        }
    }
}
