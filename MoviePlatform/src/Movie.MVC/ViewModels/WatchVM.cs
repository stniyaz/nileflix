using Movie.Business.DTOs.ComentDTOs;
using Movie.Core.Models;

namespace Movie.MVC.ViewModels
{
    public class WatchVM
    {
        public Core.Models.Movie Movie { get; set; }
        public CommentCreateDTO Comment { get; set; }
    }
}
