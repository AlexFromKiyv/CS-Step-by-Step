using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomException;

public class CarIsDeadException3 : ApplicationException
{
    public DateTime ErrorTimeStamp { get; set; }
    public string CauseOfError { get; set; } = null!;

    public CarIsDeadException3(DateTime time, string cause) 
        : this(string.Empty, time, cause, null)
    {
    }
    public CarIsDeadException3(string? message, DateTime time,
    string cause) : this(message, time, cause, null)
    {
    }
    public CarIsDeadException3(string? message, DateTime time,
    string cause, Exception? innerException) : base(message, innerException)
    {
        ErrorTimeStamp = time;
        CauseOfError = cause;
    }
    public CarIsDeadException3()
    {
    }
}
