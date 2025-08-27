namespace AutoLot.Services.DataServices.Api;

public class CarApiDataService : ApiDataServiceBase<Car>, ICarDataService
{
    public CarApiDataService(ICarApiServiceWrapper serviceWrapper) : base(serviceWrapper)
    {
    }

    public async Task<IEnumerable<Car>> GetAllByMakeIdAsync(int? makeId)
    {
        return makeId.HasValue
            ? await ((ICarApiServiceWrapper)ServiceWrapper).GetCarsByMakeAsync(makeId.Value)
            : await GetAllAsync();
    }
}
