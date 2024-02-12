using Movie.Core.Models;

namespace Movie.MVC.Areas.manage.ViewModels
{
    public class MovieIndexVM
    {
        public List<Core.Models.Movie> Movies { get; set; }
        public List<MovieGenre> MovieGenres { get; set; }
    }
}
