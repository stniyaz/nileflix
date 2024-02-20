namespace Movie.Core.Models
{
    public class Earning : BaseEntity
    {
        public string AppUserId { get; set; }
        public int Amount { get; set; }

        public AppUser AppUser { get; set; }
    }
}
