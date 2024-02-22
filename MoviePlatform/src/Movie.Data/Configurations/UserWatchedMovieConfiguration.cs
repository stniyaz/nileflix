using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class UserWatchedMovieConfiguration : IEntityTypeConfiguration<UserWatchedMovie>
    {
        public void Configure(EntityTypeBuilder<UserWatchedMovie> builder)
        {
            builder.HasOne(x => x.AppUser)
                   .WithMany(x => x.UserWatchedMovies)
                   .HasForeignKey(x => x.AppUserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Movie)
                   .WithMany(x => x.UserWatchedMovies)
                   .HasForeignKey(x => x.MovieId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
