namespace Movie.Core.Models
{
    public class Genre : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }

        public List<MovieGenre> MovieGenres { get; set; }
    }
}
