using Movie.Business.ViewModels;

namespace Movie.Business.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResultVM> PaymentProcess(/*string date */string username);
    }
}
