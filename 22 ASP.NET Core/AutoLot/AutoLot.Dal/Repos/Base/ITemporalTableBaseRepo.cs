using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Repos.Base;

public interface ITemporalTableBaseRepo<T> : IBaseRepo<T> where T : BaseEntity, new()
{
    IEnumerable<TemporalViewModel<T>> GetAllHistory();
    IEnumerable<TemporalViewModel<T>> GetAllHistoryAsOf(DateTime dateTime);
    IEnumerable<TemporalViewModel<T>> GetHistoryBetween(DateTime startDateTime, DateTime endDateTime);
    IEnumerable<TemporalViewModel<T>> GetHistoryContainedIn(DateTime startDateTime, DateTime endDateTime);
    IEnumerable<TemporalViewModel<T>> GetHistoryFromTo(DateTime startDateTime, DateTime endDateTime);
}
