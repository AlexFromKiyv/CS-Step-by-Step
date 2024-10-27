using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.ViewModels.Configuration;

public class CarMakeViewModelConfiguration : IEntityTypeConfiguration<CarMakeViewModel>
{
    public void Configure(EntityTypeBuilder<CarMakeViewModel> builder)
    {
        builder.HasNoKey().ToSqlQuery(@"
                SELECT m.Id MakeId, m.Name Make, i.Id CarId, i.IsDrivable, 
                   i.DisplayName, i.DateBuild, i.Color, i.PetName
                FROM dbo.Makes m 
                INNER JOIN dbo.Inventory i ON i.MakeId = m.Id");
        builder.ToTable(x => x.ExcludeFromMigrations());
    }
}