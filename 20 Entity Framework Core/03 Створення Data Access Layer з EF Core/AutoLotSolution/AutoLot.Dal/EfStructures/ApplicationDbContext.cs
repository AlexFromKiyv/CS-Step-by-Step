using System;
using System.Collections.Generic;
using AutoLot.Models.Entities;
using AutoLot.Models.Entities.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AutoLot.Dal.EfStructures;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CreditRisk> CreditRisks { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<CustomerOrderView> CustomerOrderViews { get; set; }
    public virtual DbSet<Car> Cars { get; set; }
    public virtual DbSet<Make> Makes { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Driver> Drivers { get; set; }
    public virtual DbSet<CarDriver> CarsToDrivers { get; set; }
    public virtual DbSet<Radio> Radios { get; set; }
    public virtual DbSet<SeriLogEntry> SeriLogEntries { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<CustomerOrderView>(entity =>
        {
            entity.ToView("CustomerOrderView");
        });

        new CarConfiguration().Configure(modelBuilder.Entity<Car>());
        new DriverConfiguration().Configure(modelBuilder.Entity<Driver>());
        new CarDriverConfiguration().Configure(modelBuilder.Entity<CarDriver>());
        new RadioConfiguration().Configure(modelBuilder.Entity<Radio>());
        new CustomerConfiguration().Configure(modelBuilder.Entity<Customer>());
        new MakeConfiguration().Configure(modelBuilder.Entity<Make>());
        new CreditRiskConfiguration().Configure(modelBuilder.Entity<CreditRisk>());
        new OrderConfiguration().Configure(modelBuilder.Entity<Order>());
        new SeriLogEntryConfiguration().Configure(modelBuilder.Entity<SeriLogEntry>());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
