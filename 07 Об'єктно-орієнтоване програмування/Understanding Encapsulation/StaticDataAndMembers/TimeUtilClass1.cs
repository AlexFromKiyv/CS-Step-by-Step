using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// Import the static members of Console and DateTime.
using static System.Console;
using static System.DateTime;

namespace StaticDataAndMembers;

static class TimeUtilClass1
{
    public static void PrintTime()
    {
        WriteLine(Now.ToShortTimeString());
    }
    public static void PrintDate()
    {
        WriteLine(Today.ToShortDateString());
    }
}
