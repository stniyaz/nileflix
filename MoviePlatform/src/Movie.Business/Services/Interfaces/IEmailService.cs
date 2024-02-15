using Movie.Business.Helpers.Mail;

namespace Movie.Business.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendMailAsync(Message message);
        Task CheckConfirmationAsync(string token, string email);
        Task ChangeEmailAsync(string userId, string newMail, string token);
    }
}
