using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.FirstName).IsRequired(true).HasMaxLength(50);
            builder.Property(x => x.LastName).IsRequired(true).HasMaxLength(50);

            builder.Property(x => x.CreatedDate);
            builder.Property(x => x.UpdatedDate).IsRequired(false);
        }
    }
}
