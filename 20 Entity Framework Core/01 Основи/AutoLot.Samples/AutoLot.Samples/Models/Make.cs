using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Samples.Models;
[Table("Makes", Schema = "dbo")]
public class Make : BaseEntity
{
    [Required, StringLength(50)]
    public string Name { get; set; }
    [InverseProperty(nameof(Car.MakeNavigation))]
    public IEnumerable<Car> Cars { get; set; } = new List<Car>();
}
