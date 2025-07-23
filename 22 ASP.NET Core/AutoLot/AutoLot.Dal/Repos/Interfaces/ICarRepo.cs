using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Repos.Interfaces;

public interface ICarRepo : ITemporalTableBaseRepo<Car>
{
    IEnumerable<Car> GetAllBy(int makeId);
    IEnumerable<Car> GetAllInclludeMake();
    string GetPetName(int id);
}