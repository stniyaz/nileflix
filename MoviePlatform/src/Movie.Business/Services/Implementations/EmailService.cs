using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Helpers.Mail;
using Movie.Business.Services.Interfaces;
using Movie.Core.Models;

namespace Movie.Business.Services.Implementations
{
    public class EmailService : Movie.Business.Services.Interfaces.IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAccountService _accountService;
        private readonly SignInManager<AppUser> _signInManager;
        private string unsuccessfullConfirmation = "Something went wrong.";
        public EmailService(EmailConfiguration emailConfiguration,
                            UserManager<AppUser> userManager,
                            IAccountService accountService,
                            SignInManager<AppUser> signInManager)
        {
            _emailConfiguration = emailConfiguration;
            _userManager = userManager;
            _accountService = accountService;
            _signInManager = signInManager;
        }

        public async Task ChangeEmailAsync(string userId, string newMail, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) throw new UserNotFoundException();

            var result = await _userManager.ChangeEmailAsync(user, newMail, token);
            if (!result.Succeeded) throw new UnsuccessfulConfirmationException(unsuccessfullConfirmation);
        }

        public async Task CheckConfirmationAsync(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new UserNotFoundException();

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
                throw new UnsuccessfulConfirmationException(unsuccessfullConfirmation);

            await _signInManager.SignInAsync(user, isPersistent: true);

        }

        public async Task SendMailAsync(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await Send(emailMessage);

        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("nilex Platform", _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };

            return emailMessage;
        }

        private async Task Send(MimeMessage message)
        {
            SmtpClient client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                await client.AuthenticateAsync("n.soltanov13@gmail.com", "kxnp rnuu afhe lbji");

                await client.SendAsync(message);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                await client.DisconnectAsync(true);
                client.Dispose();
            }
        }
    }
}
