
namespace AutoLot.Dal.Repos;

public class CarRepo : TemporalTableBaseRepo<Car>, ICarRepo
{
    public CarRepo(ApplicationDbContext context) : base(context)
    {
    }
    internal CarRepo(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    internal IOrderedQueryable<Car> BuildBaseQuery() =>
    Table.Include(c => c.MakeNavigation).OrderBy(c => c.PetName);
    public IEnumerable<Car> GetAllBy(int makeId) =>
    BuildBaseQuery().Where(c => c.MakeId == makeId);
    public override Car? Find(int id) =>
    Table.IgnoreQueryFilters()
    .Where(c => c.Id == id)
    .Include(c => c.MakeNavigation)
    .FirstOrDefault();

    public string GetPetName(int id)
    {
        var parameterId = new SqlParameter
        {
            ParameterName = "@carId",
            SqlDbType = SqlDbType.Int,
            Value = id
        };
        var parameterName = new SqlParameter
        {
            ParameterName = "@petName",
            SqlDbType = SqlDbType.NVarChar,
            Size = 50,
            Direction = ParameterDirection.Output
        };

        string sqlQuery = "EXEC [dbo].[GetPetName] @carId, @petName OUTPUT";
        ExecuteParameterizedQuery(sqlQuery, [parameterId, parameterName]);
        return (string)parameterName.Value;
    }

}
