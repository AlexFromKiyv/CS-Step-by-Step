using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[]? args)
    {
        string connectionString = @"Server=(localdb)\mssqllocaldb;Database=AutoLotSamples;Trusted_Connection=True;ConnectRetryCount=0";

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        if (args != null && args.Length == 1 && args[0].Equals("lazy", StringComparison.OrdinalIgnoreCase))
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        //optionsBuilder.UseSqlServer(connectionString, 
        //    options => options.EnableRetryOnFailure());

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
