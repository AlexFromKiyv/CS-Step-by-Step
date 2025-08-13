namespace AutoLot.Models.Entities.Configuration;

public class MakeConfiguration : IEntityTypeConfiguration<Make>
{
    public void Configure(EntityTypeBuilder<Make> builder)
    {
        builder.ToTable(tb => tb.IsTemporal(t =>
        {
            t.UseHistoryTable("MakesAudit", "dbo");
        }));
    }
}