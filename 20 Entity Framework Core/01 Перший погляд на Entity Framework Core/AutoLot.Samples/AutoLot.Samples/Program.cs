
static void SimpleSaveCahnges()
{
    var context = new ApplicationDbContextFactory().CreateDbContext([]);
    //make some changes
    context.SaveChanges();
}
//SimpleSaveCahnges();

static void TransactedSaveChanges()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null!);
    using var transaction = context.Database.BeginTransaction();
	try
	{
        //Create, change, delete stuff
        context.SaveChanges();
        transaction.Commit();
    }
    catch (Exception ex)
	{
        Console.WriteLine(ex.Message);
        transaction.Rollback();
	}
}

static void UsingSavePoints()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null!);
    using var transaction = context.Database.BeginTransaction();
    try
    {
        //Create, change, delete stuff
        transaction.CreateSavepoint("check point 1");
        context.SaveChanges();
        transaction.Commit();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
        transaction.RollbackToSavepoint("check point 1");
    }
}

static void TransctionWithExecutionStrateies()
{
    var context = new ApplicationDbContextFactory().CreateDbContext(null);
    var strategy = context.Database.CreateExecutionStrategy();
    strategy.Execute(() =>
    {
        using var transaction = context.Database.BeginTransaction();
        try
        {
            // actionToExecute();
            transaction.Commit();
            Console.WriteLine("Succeesed.");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine(ex.Message);
        }
    });
}