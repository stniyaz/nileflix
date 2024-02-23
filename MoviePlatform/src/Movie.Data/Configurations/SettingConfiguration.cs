using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class SettingConfiguration : IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.Property(x => x.Key)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(x => x.Value)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
