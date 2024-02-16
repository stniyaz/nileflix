namespace Movie.Core.Models
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string MovieLong { get; set; }
        public int ReleaseYear { get; set; }
        public int AgeLimit { get; set; }
        public int Views { get; set; }
        public double? Rating { get; set; }
        public bool IsPopular { get; set; }
        public bool IsNewst { get; set; }
        public List<MovieImage> MovieImages { get; set; }
        public string TrailerUrl { get; set; }
        public string Movie1080pUrl { get; set; }
        public string Movie480pUrl { get; set; }
        public string Subtitle { get; set; }


        public int CountryId { get; set; }
        public Country Country { get; set; }

        public List<MovieGenre> MovieGenres { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
