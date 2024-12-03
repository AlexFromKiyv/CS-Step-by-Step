using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Models.Entities.Configuration;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.HasOne(d => d.MakeNavigation)
            .WithMany(p => p.Cars)
            .HasForeignKey(c=>c.MakeId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Inventory_Makes_MakeId");
        
        builder.HasQueryFilter(c => c.IsDrivable);
        builder.Property(p => p.IsDrivable)
            .HasField("_isDrivable")
            .HasDefaultValue(true);

        builder.Property(e => e.DateBuild).HasDefaultValueSql("getdate()");

        builder.Property(e => e.Display)
        .HasComputedColumnSql("[PetName] + ' (' + [Color] + ')'", stored: true);

        builder.Property(p => p.Price).HasConversion(new StringToNumberConverter<decimal>());

        builder.ToTable(b => b.IsTemporal(t =>
        {
            t.UseHistoryTable("InventoryAudit");
        }));

        builder
    .HasMany(p => p.Drivers)
    .WithMany(p => p.Cars)
    .UsingEntity<CarDriver>(
       j => j
           .HasOne(cd => cd.DriverNavigation)
           .WithMany(d => d.CarDrivers)
           .HasForeignKey(nameof(CarDriver.DriverId))
           .HasConstraintName("FK_InventoryDriver_Drivers_DriverId")
           .OnDelete(DeleteBehavior.Cascade),
       j => j
           .HasOne(cd => cd.CarNavigation)
           .WithMany(c => c.CarDrivers)
           .HasForeignKey(nameof(CarDriver.CarId))
           .HasConstraintName("FK_InventoryDriver_Inventory_InventoryId")
           .OnDelete(DeleteBehavior.ClientCascade),
       j =>
       {
           j.HasKey(cd => new { cd.CarId, cd.DriverId });
       });
    }
}
