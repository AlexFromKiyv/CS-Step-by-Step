namespace AutoLot.Dal.Repos;

public class OrderRepo : TemporalTableBaseRepo<Order>, IOrderRepo
{
    public OrderRepo(ApplicationDbContext context) : base(context)
    {
    }

    public OrderRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}