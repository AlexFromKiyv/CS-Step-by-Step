using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Services.DataServices.Dal;

public class CarDalDataService : DalDataServiceBase<Car>,ICarDataService
{
    private readonly ICarRepo _repo;

    public CarDalDataService(ICarRepo repo) : base(repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Car>> GetAllByMakeIdAsync(int? makeId) =>
        makeId.HasValue ? _repo.GetAllBy(makeId.Value) : MainRepo.GetAllIgnoreQueryFilters();
}
