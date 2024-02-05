using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class MovieGenreConfiguration : IEntityTypeConfiguration<MovieGenre>
    {
        public void Configure(EntityTypeBuilder<MovieGenre> builder)
        {
            builder.HasOne(mg => mg.Movie).WithMany(m => m.MovieGenres).HasForeignKey(mg => mg.MovieId);

            builder.HasOne(mg => mg.Genre).WithMany(g => g.MovieGenres).HasForeignKey(mg => mg.GenreId);
        }
    }
}
