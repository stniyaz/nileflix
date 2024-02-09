using Movie.Business.Helpers.Pagination;
using Movie.Core.Models;

namespace Movie.MVC.Areas.manage.ViewModels
{
    public class MovieIndexVM
    {
        public PaginatedList<Core.Models.Movie> PaginatedMovies { get; set; }
        public List<MovieGenre> MovieGenres { get; set; }
    }
}
