using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Repos.Base;

public abstract class TemporalTableBaseRepo<T> : BaseRepo<T>, ITemporalTableBaseRepo<T> where T : BaseEntity, new()
{
    protected TemporalTableBaseRepo(ApplicationDbContext context) : base(context) {}
    protected TemporalTableBaseRepo(DbContextOptions<ApplicationDbContext> options) : base(options) {}


    // Helper methods
    internal static DateTime ConvertToUtc(DateTime dateTime) =>
        TimeZoneInfo.ConvertTimeToUtc(dateTime, TimeZoneInfo.Utc);
    internal static IEnumerable<TemporalViewModel<T>> ExecuteQuery(IQueryable<T> query) =>
    query.OrderBy(e => EF.Property<DateTime>(e, "PeriodStart"))
    .Select(e => new TemporalViewModel<T>
    {
        Entity = e,
        PeriodStart = EF.Property<DateTime>(e, "PeriodStart"),
        PeriodEnd = EF.Property<DateTime>(e, "PeriodEnd")
    });

    public IEnumerable<TemporalViewModel<T>> GetAllHistory() =>
    ExecuteQuery(Table.TemporalAll());
    public IEnumerable<TemporalViewModel<T>> GetAllHistoryAsOf(DateTime dateTime) =>
        ExecuteQuery(Table.TemporalAsOf(ConvertToUtc(dateTime)));
    public IEnumerable<TemporalViewModel<T>> GetHistoryBetween(DateTime startDateTime, DateTime endDateTime) =>
        ExecuteQuery(Table.TemporalBetween(ConvertToUtc(startDateTime), ConvertToUtc(endDateTime)));
    public IEnumerable<TemporalViewModel<T>> GetHistoryContainedIn(
        DateTime startDateTime, DateTime endDateTime)
        => ExecuteQuery(Table.TemporalContainedIn(ConvertToUtc(startDateTime), ConvertToUtc(endDateTime)));
    public IEnumerable<TemporalViewModel<T>> GetHistoryFromTo(
        DateTime startDateTime, DateTime endDateTime)
        => ExecuteQuery(Table.TemporalFromTo(ConvertToUtc(startDateTime), ConvertToUtc(endDateTime)));
}
