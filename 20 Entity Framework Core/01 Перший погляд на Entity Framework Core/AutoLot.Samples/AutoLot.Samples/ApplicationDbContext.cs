using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples
{
    public class ApplicationDbContext : DbContext
    {
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
        }
    }
}
