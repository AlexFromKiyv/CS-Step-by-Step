﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.Models.Configuration;

public class CarConfiguration : IEntityTypeConfiguration<Car>
{
    public void Configure(EntityTypeBuilder<Car> builder)
    {
        builder.ToTable("Inventory", "dbo");
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.MakeId, "IX_Inventory_MakeId");
        builder.Property(e => e.Color)
        .IsRequired()
        .HasMaxLength(50);
        builder.Property(e => e.PetName)
        .IsRequired()
        .HasMaxLength(50);
        builder.Property(e => e.DateBuilt)
        .HasDefaultValueSql("getdate()");
        builder.Property(e => e.IsDrivable)
        .HasDefaultValue(true);
        builder.Property(e => e.Display)
        .HasComputedColumnSql("[PetName] + ' (' + [Color] + ')'");
        builder.HasOne(c => c.MakeNavigation)
        .WithMany(m => m.Cars)
        .HasForeignKey(c => c.MakeId)
        .OnDelete(DeleteBehavior.ClientSetNull)
        .HasConstraintName("FK_Inventory_Makes_MakeId");

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

        builder.HasQueryFilter(c => c.IsDrivable == true);

        builder.Property(c => c.Price).HasConversion(new StringToNumberConverter<decimal>());

        //CultureInfo provider = new CultureInfo("en-us");
        //NumberStyles numberStyles = NumberStyles.Number | NumberStyles.AllowCurrencySymbol;
        //builder.Property(c => c.Price).HasConversion(
        //    v => decimal.Parse(v, numberStyles, provider),
        //    v => v.ToString("C2")
        //    );

        builder.Property<bool?>("IsDeleted").IsRequired(false).HasDefaultValue(false);

        //specify table name and schema – not needed because of the Table attribute
        //builder.ToTable('Inventory', 'dbo', b=> b.IsTemporal());
        //builder.ToTable(tb => tb.IsTemporal());

        builder.ToTable(b => b.IsTemporal(t =>
        {
            t.UseHistoryTable("InventoryAudit", "audits");
        }));
    }
}