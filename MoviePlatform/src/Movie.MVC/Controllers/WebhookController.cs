using Microsoft.AspNetCore.Mvc;
using Movie.Business.Services.Interfaces;
using Stripe;

namespace Movie.MVC.Controllers
{
    public class WebhookController : Controller
    {
        private readonly IAccountService _accountService;

        public WebhookController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            // Webhook isteğini işleyin
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                // Stripe'dan gelen JSON verisini işleyin
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], "whsec_34a2ca4cb8579d9bb7a4efdd0c33cd559927cce1eb83f73b66582ea3c2c3977a");

                // Ödeme ile ilgili gereken işlemleri yapın
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                    var amount = paymentIntent.Amount;
                    // Ödeme başarılı oldu, gerekli işlemleri yapın
                    await Console.Out.WriteLineAsync("salam------------------------------");

                    string jsonContent = "{\"key1\":\"value1\",\"key2\":\"value2\"}"; // Örnek JSON içerik

                    // İçeriği StringContent nesnesine dönüştür
                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                    HttpClient client = new HttpClient();
                    string url = "https://localhost:7161/home/premium";
                    HttpResponseMessage response = await client.PostAsync(url, content);
                }


                return Ok();
            }

            catch (StripeException)
            {
                // Hata durumunda loglama yapabilirsiniz
                return BadRequest();
            }
            catch (Exception)
            {
                Console.WriteLine("gozlemirdim");
                return BadRequest();
            }
        }
    }
}
