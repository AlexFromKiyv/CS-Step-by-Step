static void SimpleSaveCahnges()
{
    var context = new ApplicationDbContextFactory().CreateDbContext([]);
    //make some changes
    context.SaveChanges();
}
//SimpleSaveCahnges();


