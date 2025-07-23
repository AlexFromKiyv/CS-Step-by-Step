using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Models.Entities.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasIndex(o => new { o.CustomerId, o.CarId })
            .IsUnique(true);
        builder.HasQueryFilter(o => o.CarNavigation.IsDrivable);
        builder.HasOne(d => d.CarNavigation)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.CarId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Orders_Inventory");

        builder.HasOne(d => d.CustomerNavigation)
            .WithMany(p => p.Orders)
            .HasForeignKey(d => d.CustomerId)
            .HasConstraintName("FK_Orders_Customers");

        builder.ToTable(b => b.IsTemporal(t =>
        {
            t.UseHistoryTable("OrdersAudit", "dbo");
        }));
    }
}