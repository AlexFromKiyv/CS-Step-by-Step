using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples
{
    public class ApplicationDbContext : DbContext
    {
        // Properies

        public DbSet<Car> Cars { get; set; }
        public DbSet<Make> Makes { get; set; }
        public DbSet<Radio> Radios { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<CarDriver> CarsToDrivers { get; set; }


        // Constructors
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options) 
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
        private void ChangeTracker_Tracked(object? sender, EntityTrackedEventArgs e)
        {
            if (e.FromQuery)
            {
                Console.WriteLine($"An entity of type {e.Entry.Entity.GetType().Name} was loaded from the database.");
            }
        }

        private void ChangeTracker_StateChanged(object? sender, EntityStateChangedEventArgs e)
        {
            if (e.OldState == EntityState.Modified && e.NewState == EntityState.Unchanged)
            {
                Console.WriteLine($"An entity of type {e.Entry.Entity.GetType().Name} was update.");
            }
        }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API calls go here

            modelBuilder.Entity<Car>(entity => 
            {
                entity.ToTable("Invertory", "dbo");
                entity.HasKey(e => e.Id);
                //entity.HasKey(e => new { e.Id, e.OrganizationId });
                entity.HasIndex(e => e.MakeId, "IX_Inventory_MakeId").IsUnique();
                entity.Property(e => e.Color)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValue("Black");
                entity.Property(e => e.PetName)
                .IsRequired()
                .HasMaxLength(50);
                entity.Property(e => e.DateBuild)
                .HasDefaultValueSql("getdate()");
                entity.Property(e => e.IsDrivable)
                .HasField("_IsDrivable")
                .HasDefaultValue(true);
                entity.Property(e => e.TimeStamp)
                .IsRowVersion()
                .IsConcurrencyToken();
                entity.Property(e => e.Display)
                .HasComputedColumnSql("[PetName] + ' (' + [Color] + ')'");

                entity.HasOne(c => c.MakeNavigation)
                .WithMany(m => m.Cars)
                .HasForeignKey(c => c.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Makes_MakeId");

                entity.HasOne(c => c.RadioNavigation)
                .WithOne(r => r.CarNavigation)
                .HasForeignKey<Radio>(c => c.CarId);

            });

            modelBuilder.Entity<Radio>(entity => 
            {
                entity.Property(e => e.CarId).HasColumnName("InvertoryId");

                //entity.HasIndex(e => e.CarId, "IX_Radios_CarId");

                //entity.HasOne(r => r.CarNavigation)
                //.WithOne(c => c.RadioNavigation)
                //.HasForeignKey<Radio>(r => r.CarId);

                entity.HasIndex(r => r.CarId, "IX_Radios_CarId")
                .IsUnique();
            });

            //modelBuilder.Entity<Make>().HasCheckConstraint("CH_Name", "[Name]<>'Lemon'", c => c.HasName("CK_Check_Name"));

            modelBuilder.Entity<Make>(entity =>
            {
                entity.HasMany(m => m.Cars)
                .WithOne(c=>c.MakeNavigation)
                .HasForeignKey(c => c.MakeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Inventory_Makes_MakeId");
            });



        }
    }
}
