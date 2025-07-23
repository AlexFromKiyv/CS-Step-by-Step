using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Repos.Base;

public interface IBaseViewRepo<T> : IDisposable where T : class, new()
{
    ApplicationDbContext Context { get; }
    IEnumerable<T> ExecuteSqlString(string sql);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAllIgnoreQueryFilters();
}