using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.DataAccessLayer.BulkImport;

internal interface IMyDataReader<T> : IDataReader
{
    public List<T> Records { get; set; }
}
