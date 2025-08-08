using AutoLot.Models.Entities.Configuration;
using AutoLot.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace AutoLot.Dal.EfStructures;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        //Handling events
        SavingChanges += (sender, args) =>
        {
            string cs = ((ApplicationDbContext)sender!).Database.GetConnectionString()!;
            Console.WriteLine($"Saving changes for {cs}");
        };
        SavedChanges += (sender, args) =>
        {
            string cs = ((ApplicationDbContext)sender!).Database.GetConnectionString()!;
            Console.WriteLine($"Saved {args!.EntitiesSavedCount} changes for {cs}");
        };
        SaveChangesFailed += (sender, args) =>
        {
            Console.WriteLine($"An exception occurred! {args.Exception.Message} entities");
        };

        ChangeTracker.Tracked += ChangeTracker_Tracked;
        ChangeTracker.StateChanged += ChangeTracker_StateChanged;
    }

    private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
    {
        if (e.Entry.Entity is not Car c)
        {
            return;
        }
        var action = string.Empty;
        Console.WriteLine($"Car {c.PetName} was {e.OldState} before the state changed to {e.NewState}");
        switch (e.NewState)
        {
            case EntityState.Unchanged:
                action = e.OldState switch
                {
                    EntityState.Added => "Added",
                    EntityState.Modified => "Edited",
                    _ => action
                };
                Console.WriteLine($"The object was {action}");
                break;
        }
    }

    private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
    {
        var source = (e.FromQuery) ? "Database" : "Code";
        if (e.Entry.Entity is Car c)
        {
            Console.WriteLine($"Car entry {c.PetName} was added from {source}");
        }
    }


    public virtual DbSet<CreditRisk> CreditRisks { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<Make> Makes { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Driver> Drivers { get; set; }

    public virtual DbSet<CarDriver> CarsToDrivers { get; set; }

    public virtual DbSet<Radio> Radios { get; set; }

    public virtual DbSet<SeriLogEntry> SeriLogEntries { get; set; }

    public virtual DbSet<CustomerOrderViewModel> CustomerOrderViewModels { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        new CarConfiguration().Configure(modelBuilder.Entity<Car>());

        new DriverConfiguration().Configure(modelBuilder.Entity<Driver>());

        new CarDriverConfiguration().Configure(modelBuilder.Entity<CarDriver>());

        new RadioConfiguration().Configure(modelBuilder.Entity<Radio>());

        new CustomerConfiguration().Configure(modelBuilder.Entity<Customer>());

        new MakeConfiguration().Configure(modelBuilder.Entity<Make>());

        new CreditRiskConfiguration().Configure(modelBuilder.Entity<CreditRisk>());

        new OrderConfiguration().Configure(modelBuilder.Entity<Order>());

        new SeriLogEntryConfiguration().Configure(modelBuilder.Entity<SeriLogEntry>());

        new CustomerOrderViewModelConfiguration().Configure(modelBuilder.Entity<CustomerOrderViewModel>());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    // DB Functions
    [DbFunction("udf_CountOfMakes", Schema = "dbo")]
    public static int InventoryCountFor(int makeId)
        => throw new NotSupportedException();
    [DbFunction("udtf_GetCarsForMake", Schema = "dbo")]
    public IQueryable<Car> GetCarsFor(int makeId)
        => FromExpression(() => GetCarsFor(makeId));

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.IgnoreAny<INonPersisted>();
    }

    public override int SaveChanges()
    {
        try
        {
            return base.SaveChanges();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            //A concurrency error occurred
            //Should log and handle intelligently
            throw new CustomConcurrencyException("A concurrency error happened.", ex);
        }
        catch (RetryLimitExceededException ex)
        {
            //DbResiliency retry limit exceeded
            //Should log and handle intelligently
            throw new CustomRetryLimitExceededException("Retry Limit Exceeded Exception", ex);
        }
        catch (DbUpdateException ex)
        {
            //Should log and handle intelligently
            throw new CustomDbUpdateException("An error occurred updating the database", ex);
        }
        catch (Exception ex)
        {
            //Should log and handle intelligently
            throw new CustomException("An error occurred updating the database", ex);
        }
    }

}
