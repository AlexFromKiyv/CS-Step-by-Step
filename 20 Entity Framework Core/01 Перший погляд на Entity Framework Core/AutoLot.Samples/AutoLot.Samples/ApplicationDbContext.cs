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

        //modelBuilder.Entity<Car>(entity =>
        //{
        //    entity.ToTable("Inventory", "dbo");
        //    entity.HasKey(e => e.Id);
        //    //entity.HasKey(e => new { e.Id, e.OrganizationId });
        //    //entity.HasIndex(e => e.MakeId, "IX_Inventory_MakeId").IsUnique();
        //    entity.Property(e => e.Color)
        //    .IsRequired()
        //    .HasMaxLength(50);
        //    entity.Property(e => e.PetName)
        //    .IsRequired()
        //    .HasMaxLength(50);
        //    entity.Property(e => e.DateBuilt)
        //    .HasDefaultValueSql("getdate()");
        //    entity.Property(e => e.IsDrivable)
        //    .HasDefaultValue(true);
        //    entity.Property(e => e.Display)
        //    .HasComputedColumnSql("[PetName] + ' (' + [Color] + ')'");
        //    entity.HasOne(c => c.MakeNavigation)
        //    .WithMany(m => m.Cars)
        //    .HasForeignKey(c => c.MakeId)
        //    .OnDelete(DeleteBehavior.ClientSetNull)
        //    .HasConstraintName("FK_Inventory_Makes_MakeId");
        //});

        //modelBuilder.Entity<Car>()
        //  .HasMany(p => p.Drivers)
        //  .WithMany(p => p.Cars)
        //  .UsingEntity<CarDriver>(
        //     j => j
        //         .HasOne(cd => cd.DriverNavigation)
        //         .WithMany(d => d.CarDrivers)
        //         .HasForeignKey(nameof(CarDriver.DriverId))
        //         .HasConstraintName("FK_InventoryDriver_Drivers_DriverId")
        //         .OnDelete(DeleteBehavior.Cascade),
        //     j => j
        //         .HasOne(cd => cd.CarNavigation)
        //         .WithMany(c => c.CarDrivers)
        //         .HasForeignKey(nameof(CarDriver.CarId))
        //         .HasConstraintName("FK_InventoryDriver_Inventory_InventoryId")
        //         .OnDelete(DeleteBehavior.ClientCascade),
        //     j =>
        //     {
        //         j.HasKey(cd => new { cd.CarId, cd.DriverId });
        //     });

        new CarConfiguration().Configure(modelBuilder.Entity<Car>());

        //modelBuilder.Entity<Radio>(entity =>
        //{
        //    entity.Property(e => e.CarId).HasColumnName("InventoryId");

        //    entity.HasIndex(e => e.CarId, "IX_Radios_InventoryId").IsUnique();
        //    entity.HasOne(r => r.CarNavigation)
        //    .WithOne(c => c.RadioNavigation)
        //    .HasForeignKey<Radio>(r => r.CarId);
        //});

        new RadioConfiguration().Configure(modelBuilder.Entity<Radio>());

        //modelBuilder.Entity<Make>()
        //    .ToTable(t => t.HasCheckConstraint("CH_Name", "[Name]<>'Lemon'"));

        new DriverConfiguration().Configure(modelBuilder.Entity<Driver>());
        new CarMakeViewModelConfiguration().Configure(modelBuilder.Entity<CarMakeViewModel>());
    }
}
