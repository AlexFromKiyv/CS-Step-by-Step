using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.Models.Configuration;

internal class RadioConfiguration : IEntityTypeConfiguration<Radio>
{
    public void Configure(EntityTypeBuilder<Radio> builder)
    {
        builder.Property(e => e.CarId).HasColumnName("InventoryId");
        builder.HasIndex(e => e.CarId, "IX_Radios_InventoryId").IsUnique();
        builder.HasOne(d => d.CarNavigation)
          .WithOne(p => p.RadioNavigation)
          .HasForeignKey<Radio>(d => d.CarId);
    }
}