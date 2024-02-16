using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.Property(x => x.Text).IsRequired().HasMaxLength(1000);
            builder.Property(x => x.Like).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Dislike).IsRequired().HasDefaultValue(0);
            builder.HasOne(x => x.AppUser).WithMany(x => x.Comments);
            builder.HasOne(x => x.Movie).WithMany(x => x.Comments);
        }
    }
}
