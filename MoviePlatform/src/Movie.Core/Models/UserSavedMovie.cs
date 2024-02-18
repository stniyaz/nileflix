namespace Movie.Core.Models
{
    public class UserSavedMovie : BaseEntity
    {
        public string AppUserId { get; set; }
        public int MovieId { get; set; }
        public AppUser AppUser { get; set; }
        public Movie Movie { get; set; }
    }
}
