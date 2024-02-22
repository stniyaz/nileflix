namespace Movie.Business.Services.Interfaces
{
    public interface IUserWatcedMovieService
    {
        Task ViewCounterAsync(int movieId);
    }
}
