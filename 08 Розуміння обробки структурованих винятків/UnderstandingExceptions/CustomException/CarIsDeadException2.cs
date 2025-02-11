using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomException;

public class CarIsDeadException2 :ApplicationException
{
    public DateTime ErrorTimeStamp { get; set; }
    public string CauseOfError { get; set; } = null!;

    public CarIsDeadException2(string message, DateTime errorTimeStamp, string causeOfError)
        :base(message)
    {
        ErrorTimeStamp = errorTimeStamp;
        CauseOfError = causeOfError;
    }
    public CarIsDeadException2()
    {
    }
}
