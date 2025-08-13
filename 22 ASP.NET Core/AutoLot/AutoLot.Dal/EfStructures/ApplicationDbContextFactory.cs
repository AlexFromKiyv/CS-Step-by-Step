namespace AutoLot.Dal.EfStructures;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[]? args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=AutoLot;Trusted_Connection=True;ConnectRetryCount=0";
        optionsBuilder.UseSqlServer(connectionString);
        //Console.WriteLine(connectionString);
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
