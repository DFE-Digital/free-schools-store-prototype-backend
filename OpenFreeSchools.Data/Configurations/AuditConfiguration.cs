using OpenFreeSchools.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OpenFreeSchools.Data.Configurations;

public class AuditConfiguration : IEntityTypeConfiguration<Audit>
{
	public void Configure(EntityTypeBuilder<Audit> builder)
	{
		builder.ToTable("AuditLog", "concerns");
		
		builder.Property(e => e.ChangeType)
			.HasConversion<string>();
		
		builder.HasKey(e => e.Id)
				.HasName("PK__AuditLog");
	}
}