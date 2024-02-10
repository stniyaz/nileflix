using Microsoft.AspNetCore.Identity;

namespace Movie.Core.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public bool IsBanned { get; set; }
        public bool IsPremium { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
