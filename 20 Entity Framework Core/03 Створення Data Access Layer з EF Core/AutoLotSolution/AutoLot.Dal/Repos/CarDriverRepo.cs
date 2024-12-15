

namespace AutoLot.Dal.Repos;

public class CarDriverRepo : TemporalTableBaseRepo<CarDriver>, ICarDriverRepo
{
    public CarDriverRepo(ApplicationDbContext context) : base(context)
    {
    }
    public CarDriverRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }


    internal IIncludableQueryable<CarDriver, Driver> BuildBaseQuery()
    => Table.Include(cd => cd.CarNavigation).Include(cd => cd.DriverNavigation);
    public override IEnumerable<CarDriver> GetAll()
    => BuildBaseQuery();
    public override IEnumerable<CarDriver> GetAllIgnoreQueryFilters()
        => BuildBaseQuery().IgnoreQueryFilters();
    public override CarDriver? Find(int id)
        => BuildBaseQuery().IgnoreQueryFilters()
        .Where(cd => cd.Id == id).FirstOrDefault();
}
