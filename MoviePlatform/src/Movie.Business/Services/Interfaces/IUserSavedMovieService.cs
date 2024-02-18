namespace Movie.Business.Services.Interfaces
{
    public interface IUserSavedMovieService
    {
        Task<bool> AddOrRemoveAsync(int movieId);
        Task<List<int>> GetUserSavedMoviesIdsAsync(string username);
        Task<List<Core.Models.Movie>> GetUserSavedMoviesAsync(string username);
    }
}
