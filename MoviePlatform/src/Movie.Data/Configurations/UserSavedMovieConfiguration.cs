using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class UserSavedMovieConfiguration : IEntityTypeConfiguration<UserSavedMovie>
    {
        public void Configure(EntityTypeBuilder<UserSavedMovie> builder)
        {
            builder.HasOne(x => x.Movie).WithMany(x => x.UserSavedMovies).HasForeignKey(x=> x.MovieId);
            builder.HasOne(x => x.AppUser).WithMany(x => x.UserSavedMovies).HasForeignKey(x=> x.AppUserId);
        }
    }
}
