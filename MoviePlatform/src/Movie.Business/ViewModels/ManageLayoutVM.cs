using Movie.Core.Models;

namespace Movie.Business.ViewModels
{
    public class ManageLayoutVM
    {
        public AppUser User { get; set; }
        public IList<string> Role { get; set; }
    }
}
