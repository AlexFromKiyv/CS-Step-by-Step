namespace AutoLot.Dal.Repos;

public class DriverRepo : BaseRepo<Driver>, IDriverRepo
{
    public DriverRepo(ApplicationDbContext context) : base(context)
    {
    }
    internal DriverRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    internal IOrderedQueryable<Driver> BuildQuery() =>
    Table
    .OrderBy(d => d.PersonInformation.LastName)
    .OrderBy(d => d.PersonInformation.FirstName);

    public override IEnumerable<Driver> GetAll() =>
    BuildQuery();
    public override IEnumerable<Driver> GetAllIgnoreQueryFilters() =>
        BuildQuery().IgnoreQueryFilters();

}