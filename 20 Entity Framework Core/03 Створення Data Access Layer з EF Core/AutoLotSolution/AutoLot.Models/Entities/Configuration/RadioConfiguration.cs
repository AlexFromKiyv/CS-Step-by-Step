using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Models.Entities.Configuration;

public class RadioConfiguration : IEntityTypeConfiguration<Radio>
{
    public void Configure(EntityTypeBuilder<Radio> builder)
    {
        builder.HasQueryFilter(r => r.CarNavigation.IsDrivable);
        builder.HasIndex(r => r.CarId, "IX_Radios_CarId")
            .IsUnique();
        builder.HasOne(r => r.CarNavigation)
            .WithOne(c => c.RadioNavigation)
            .HasForeignKey<Radio>(r => r.CarId);
        builder.ToTable(tb => tb.IsTemporal(t =>
        {
            t.UseHistoryTable("RadiosAudit");
        }));
    }
}
