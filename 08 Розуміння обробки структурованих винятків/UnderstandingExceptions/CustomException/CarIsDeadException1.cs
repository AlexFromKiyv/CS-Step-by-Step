using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CustomException;
// This custom exception describes the details of the car-is-dead condition.
// (Remember, you can also simply extend Exception.)
public class CarIsDeadException1 : ApplicationException
{
    private string _messageDetails = string.Empty;
    public DateTime ErrorTimeStamp { get; set; }
    public string CauseOfError { get; set; } = null!;


    public CarIsDeadException1(string message, DateTime time, string cause)
    {
        _messageDetails = message;
        ErrorTimeStamp = time;
        CauseOfError = cause;
    }
    public CarIsDeadException1()
    {
    }

    // Override the Exception.Message property.
    public override string Message => $"Car Error Message:{_messageDetails}";
}
