using Movie.Business.Helpers.Pagination;
using Movie.Core.Models;

namespace Movie.MVC.Areas.manage.ViewModels
{
    public class GenreIndexVM
    {
        public PaginatedList<Genre> Genres { get; set; }
        public int? SortBy { get; set; }
        public string? Search { get; set; }
        public string? Page { get; set; }
    }
}
