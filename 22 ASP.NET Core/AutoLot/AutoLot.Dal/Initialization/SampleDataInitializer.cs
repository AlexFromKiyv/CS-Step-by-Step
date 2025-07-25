﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Initialization;

public static class SampleDataInitializer
{
    public static void DropAndCreateDatabase(ApplicationDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.Migrate();
    }

    internal static void ClearData(ApplicationDbContext context)
    {
        var entities = new[]
        {
            typeof(Order).FullName,
            typeof(Customer).FullName,
            typeof(CarDriver).FullName,
            typeof(Driver).FullName,
            typeof(Radio).FullName,
            typeof(Car).FullName,
            typeof(Make).FullName,
            typeof(CreditRisk).FullName
        };

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddDbContextDesignTimeServices(context);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        var designTimeModel = serviceProvider.GetService<IModel>();

        foreach (var entityName in entities)
        {
            var entity = context.Model.FindEntityType(entityName);
            var tableName = entity.GetTableName();
            var schemaName = entity.GetSchema();
            context.Database.ExecuteSqlRaw($"DELETE FROM {schemaName}.{tableName}");
            context.Database.ExecuteSqlRaw($"DBCC CHECKIDENT (\"{schemaName}.{tableName}\", RESEED, 0);");
            if (entity.IsTemporal())
            {
                var strategy = context.Database.CreateExecutionStrategy();
                strategy.Execute(() =>
                {
                    using var trans = context.Database.BeginTransaction();
                    var designTimeEntity = designTimeModel.FindEntityType(entityName);
                    var historySchema = designTimeEntity.GetHistoryTableSchema();
                    var historyTable = designTimeEntity.GetHistoryTableName();
                    context.Database.ExecuteSqlRaw($"ALTER TABLE {schemaName}.{tableName} SET (SYSTEM_VERSIONING = OFF)");
                    context.Database.ExecuteSqlRaw($"DELETE FROM {historySchema}.{historyTable}");
                    context.Database.ExecuteSqlRaw($"ALTER TABLE {schemaName}.{tableName} SET (SYSTEM_VERSIONING = ON (HISTORY_TABLE={historySchema}.{historyTable}))");
                    trans.Commit();
                });
            }
        }
    }

    internal static void SeedData(ApplicationDbContext context)
    {
        try
        {
            ProcessInsert(context, context.Customers, SampleData.Customers);
            ProcessInsert(context, context.Makes, SampleData.Makes);
            ProcessInsert(context, context.Drivers, SampleData.Drivers);
            ProcessInsert(context, context.Cars, SampleData.Inventory);
            ProcessInsert(context, context.Radios, SampleData.Radios);
            ProcessInsert(context, context.CarsToDrivers, SampleData.CarsAndDrivers);
            ProcessInsert(context, context.Orders, SampleData.Orders);
            ProcessInsert(context, context.CreditRisks, SampleData.CreditRisks);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            //Set a break point here to determine what the issues is
            throw;
        }

        static void ProcessInsert<TEntity>(ApplicationDbContext context,
            DbSet<TEntity> table, List<TEntity> records) where TEntity : BaseEntity
        {
            if (table.Any()) { return; }

            IExecutionStrategy strategy = context.Database.CreateExecutionStrategy();

            strategy.Execute(() =>
            {
                using var transaction = context.Database.BeginTransaction();
                try
                {
                    var metaData = context.Model.FindEntityType(typeof(TEntity).FullName);
                    string sqlON = $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} ON";
                    string sqlOFF = $"SET IDENTITY_INSERT {metaData.GetSchema()}.{metaData.GetTableName()} OFF";

                    context.Database.ExecuteSqlRaw(sqlON);
                    table.AddRange(records);
                    context.SaveChanges();
                    context.Database.ExecuteSqlRaw(sqlOFF);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            });
        }
    }

    public static void InitializeData(ApplicationDbContext context)
    {
        DropAndCreateDatabase(context);
        SeedData(context);
    }

    public static void ClearAndSeedData(ApplicationDbContext context)
    {
        ClearData(context);
        SeedData(context);
    }

}
