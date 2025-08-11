namespace AutoLot.Dal.Repos;

public class CustomerOrderViewModelRepo : BaseViewRepo<CustomerOrderViewModel>, ICustomerOrderViewModelRepo
{
    public CustomerOrderViewModelRepo(ApplicationDbContext context) : base(context)
    {
    }

    public CustomerOrderViewModelRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}
