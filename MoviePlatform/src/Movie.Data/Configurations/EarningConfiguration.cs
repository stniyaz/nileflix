using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
    public class EarningConfiguration : IEntityTypeConfiguration<Earning>
    {
        public void Configure(EntityTypeBuilder<Earning> builder)
        {
            builder.HasOne(x => x.AppUser).WithMany(x => x.Earnings).HasForeignKey(x => x.AppUserId);
        }
    }
}
