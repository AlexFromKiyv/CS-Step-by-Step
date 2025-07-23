using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Repos;

public class CustomerRepo : BaseRepo<Customer>, ICustomerRepo
{
    public CustomerRepo(ApplicationDbContext context) : base(context)
    {
    }

    public CustomerRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public override IEnumerable<Customer> GetAll() =>
    Table.Include(c => c.Orders)
    .OrderBy(o => o.PersonInformation.LastName);

}
