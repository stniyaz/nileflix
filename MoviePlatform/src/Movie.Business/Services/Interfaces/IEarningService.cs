namespace Movie.Business.Services.Interfaces
{
    public interface IEarningService
    {
        Task CreateAsync(int amount, string userId);
    }
}
