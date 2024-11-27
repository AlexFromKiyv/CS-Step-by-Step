using AutoLot.Dal.EfStructures;

static void Do()
{
    var context = new ApplicationDbContextFactory().CreateDbContext([]);

    Console.WriteLine(context);

}
Do();
