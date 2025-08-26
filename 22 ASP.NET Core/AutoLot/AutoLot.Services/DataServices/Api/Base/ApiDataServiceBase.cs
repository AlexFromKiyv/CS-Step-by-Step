namespace AutoLot.Services.DataServices.Api.Base;

public abstract class ApiDataServiceBase<TEntity> : IDataServiceBase<TEntity>
    where TEntity : BaseEntity, new()
{
    protected ApiDataServiceBase()
    {
    }
    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
    public Task<TEntity> FindAsync(int id)
    {
        throw new NotImplementedException();
    }
    public Task<TEntity> AddAsync(TEntity entity, bool persist = true)
    {
        throw new NotImplementedException();
    }
    public Task<TEntity> UpdateAsync(TEntity entity, bool persist = true)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(TEntity entity, bool persist = true)
    {
        throw new NotImplementedException();
    }
}
