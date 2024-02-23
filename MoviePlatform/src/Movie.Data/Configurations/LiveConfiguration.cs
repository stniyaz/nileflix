using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class LiveConfiguration : IEntityTypeConfiguration<Live>
    {
        public void Configure(EntityTypeBuilder<Live> builder)
        {
            builder.Property(x => x.Title)
                   .IsRequired()
                   .HasMaxLength(50);
                   
            builder.Property(x => x.ImageUrl)
                   .IsRequired()
                   .HasMaxLength(100);
            
            builder.Property(x=> x.Url)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
