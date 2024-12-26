using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Models.Entities.Configuration;

public class CarDriverConfiguration : IEntityTypeConfiguration<CarDriver>
{
    public void Configure(EntityTypeBuilder<CarDriver> builder)
    {
        builder.HasQueryFilter(cd => cd.CarNavigation.IsDrivable);

        builder.ToTable(b => b.IsTemporal(t =>
        {
            t.UseHistoryTable("InventoryToDriversAudit", "dbo");
        }));
    }
}
