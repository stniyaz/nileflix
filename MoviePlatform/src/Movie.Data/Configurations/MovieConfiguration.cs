using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Movie.Data.Configurations
{
    public class MovieConfiguration : IEntityTypeConfiguration<Movie.Core.Models.Movie>
    {
        public void Configure(EntityTypeBuilder<Core.Models.Movie> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(1024);
            builder.Property(x => x.MovieLong).IsRequired().HasMaxLength(20);
            builder.Property(x => x.ReleaseYear).IsRequired();
            builder.Property(x => x.AgeLimit).IsRequired();
            builder.Property(x => x.Views).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.IsNewst).IsRequired();
            builder.Property(x => x.IsPopular).IsRequired();
            builder.Property(x => x.MovieUrl).IsRequired().HasMaxLength(100);
            builder.Property(x => x.TrailerUrl).IsRequired().HasMaxLength(100);
            builder.HasOne(x => x.Country).WithMany(x => x.Movies);
        }
    }
}
