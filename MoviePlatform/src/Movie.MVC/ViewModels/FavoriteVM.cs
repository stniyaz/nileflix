using Movie.Core.Models;

namespace Movie.MVC.ViewModels
{
    public class FavoriteVM
    {
        public AppUser ActiveUser { get; set; }
        public List<Core.Models.Movie> Movies { get; set; }
        public List<MovieGenre> MovieGenres { get; set; }
    }
}
