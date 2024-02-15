using Movie.Business.Services.Implementations;
using Movie.Core.Models;

namespace Movie.MVC.ViewModels
{
    public class HomeVM
    {
        public List<Core.Models.Movie> Movies { get; set; }
        public List<Genre> Genres { get; set; }
        public List<MovieGenre> MovieGenres { get; set; }
        public int? GenreId { get; set; }
    }
}
