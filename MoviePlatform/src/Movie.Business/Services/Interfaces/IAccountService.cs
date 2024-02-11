using Microsoft.AspNetCore.Identity;
using Movie.Business.ViewModels;

namespace Movie.Business.Services.Interfaces
{
	public interface IAccountService
	{
		Task LoginAsync(LoginVM model);
		Task LogoutAsync();
		Task<ConfirmationVM> RegisterAsync(RegisterVM model);
	}
}
