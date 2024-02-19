using Microsoft.AspNetCore.Identity;
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
        public async Task<PaymentResultVM> PaymentProcess(/*string date*/ string username)
        {
            var user = await _accountService.GetUserByNameAsync(username);

            var domain = "https://localhost:7161/";

            var option = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = domain + $"home/c91d8291b1cd4893b870173f92636708?userId={user.Id}",
                CancelUrl = domain + $"home/pricingplans",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = user.Email,
                Metadata = new Dictionary<string, string>()
                {
                    {"customerEmail" , user.Email },
                    {"discription", "salam"}
                },
            };
            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = 5000,
                    Currency = "azn",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "One month"
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
