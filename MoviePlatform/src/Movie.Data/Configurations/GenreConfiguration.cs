using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.Property(x=> x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x=> x.ImageUrl).IsRequired().HasMaxLength(100);
        }
    }
}
