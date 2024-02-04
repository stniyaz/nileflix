namespace Movie.Core.Models
{
	public class MovieImage : BaseEntity
	{
        public string ImageUrl { get; set; }
        public bool IsCover { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
