using Microsoft.AspNetCore.Identity;
using Movie.Business.CustomExceptions.UserException;
using Movie.Business.Services.Interfaces;
using Movie.Business.ViewModels;
using Movie.Core.Models;
using Stripe;
using Stripe.Checkout;

namespace Movie.Business.Services.Implementations
{
    public class PaymentService : IPaymentService
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> _userManager;

        public PaymentService(IAccountService accountService,
                              UserManager<AppUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
        }
        public async Task<PaymentResultVM> PaymentProcess(string username, string date)
        {
            var user = await _accountService.GetUserByNameAsync(username);
            if (user is null) throw new UserNotFoundException();
            int amount;
            string month;
            switch (date)
            {
                case "onemonth":
                    amount = 1399;
                    month = "One month";
                    break;
                case "twomonths":
                    amount = 2399;
                    month = "Two months";
                    break;
                case "threemonths":
                    amount = 3399;
                    month = "Three months";
                    break;
                default:
                    throw new UnexceptedException();
            }

            var domain = "https://localhost:7161/";

            var option = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"home/c91d8291b1cd4893b870173f92636708?userId={user.Id}&amount={amount}",
                CancelUrl = domain + $"home/pricingplans",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.Email,
            };
            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = amount,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = month,
                        Description = "You are about to get a premium membership. You can be sure that you made the right decision."
                    },
                },
                Quantity = 1,

            };

            option.LineItems.Add(sessionLineItem);
            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(option);

            return new PaymentResultVM { Result = true, Session = session };
        }
    }
}
