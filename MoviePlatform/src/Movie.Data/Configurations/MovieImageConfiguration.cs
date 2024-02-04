using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movie.Core.Models;

namespace Movie.Data.Configurations
{
	public class MovieImageConfiguration : IEntityTypeConfiguration<MovieImage>
	{
		public void Configure(EntityTypeBuilder<MovieImage> builder)
		{
			builder.Property(x=> x.ImageUrl).IsRequired().HasMaxLength(100);
		}
	}
}
