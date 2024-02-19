namespace Movie.Business.ViewModels
{
    public class PaymentResultVM
    {
        public Stripe.Checkout.Session Session { get; set; }
        public bool Result { get; set; }
    }
}
