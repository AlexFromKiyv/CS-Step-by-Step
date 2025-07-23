using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Models.ViewModels;

public class TemporalViewModel<T> where T : BaseEntity, new()
{
    public required T Entity { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}