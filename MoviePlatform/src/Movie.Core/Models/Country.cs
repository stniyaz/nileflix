namespace Movie.Core.Models
{
    public class Country : BaseEntity
    {
        public string Name { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
