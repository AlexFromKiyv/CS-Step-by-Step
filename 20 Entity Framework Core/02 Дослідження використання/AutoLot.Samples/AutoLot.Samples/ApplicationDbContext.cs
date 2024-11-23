using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples;

public class ApplicationDbContext : DbContext
{
    //Properties
    public DbSet<Car> Cars { get; set; }
    public DbSet<Make> Makes { get; set; }
    public DbSet<Radio> Radios { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<CarDriver> CarsToDrivers { get; set; }
    public DbSet<CarMakeViewModel> CarMakeViewModels { get; set; }

    // Constructor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        SavingChanges += (sender, args) =>
        {
            Console.WriteLine($"Saving change for {((DbContext)sender!).Database.GetConnectionString()}");
        };

        SavedChanges += (sender, args) =>
        {
            Console.WriteLine($"Saved change {args.EntitiesSavedCount} entities");
        };

        SaveChangesFailed += (sender, args) =>
        {
            Console.WriteLine($"An exception orruced! {args.Exception.Message} entities ");
        };

        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
        ChangeTracker.Tracked += ChangeTracker_Tracked;
    }

    // Events
    private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.OldState == EntityState.Modified && e.NewState == EntityState.Unchanged)
        {
            Console.WriteLine($"An entity of type {e.Entry.Entity.GetType().Name} was update.");
        }
    }
    private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
    {
        if (e.FromQuery)
        {
            Console.WriteLine($"An entity of type {e.Entry.Entity.GetType().Name} was loaded from the database.");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Fluent API calls go here

        new CarConfiguration().Configure(modelBuilder.Entity<Car>());
        new RadioConfiguration().Configure(modelBuilder.Entity<Radio>());
        new DriverConfiguration().Configure(modelBuilder.Entity<Driver>());
        new CarMakeViewModelConfiguration().Configure(modelBuilder.Entity<CarMakeViewModel>());
    }

    //DB Function
    [DbFunction("udf_CountOfMakes", Schema = "dbo")]
    public static int InventoryCountFor(int makeId)
        => throw new NotSupportedException();

    [DbFunction("udtf_GetCarsForMake", Schema = "dbo")]
    public IQueryable<Car> GetCarsFor(int makeId)
    => FromExpression(() => GetCarsFor(makeId));

}
