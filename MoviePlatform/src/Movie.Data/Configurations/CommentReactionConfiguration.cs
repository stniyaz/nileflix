using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class CommentReactionConfiguration : IEntityTypeConfiguration<CommentReaction>
    {
        public void Configure(EntityTypeBuilder<CommentReaction> builder)
        {
            builder.HasOne(x => x.AppUser)
                   .WithMany(x => x.CommentReactions)
                   .HasForeignKey(x => x.AppUserId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Comment)
                   .WithMany(x => x.CommentReactions)
                   .HasForeignKey(x => x.CommentId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
