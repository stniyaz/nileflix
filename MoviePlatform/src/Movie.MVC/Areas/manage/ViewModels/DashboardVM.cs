using Movie.Core.Models;

namespace Movie.MVC.Areas.manage.ViewModels
{
    public class DashboardVM
    {
        public List<Core.Models.Movie> Movies { get; set; }
        public int? CommentsCount { get; set; }
        public List<AppUser> Users { get; set; }
    }
}
