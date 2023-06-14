using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enums;

class Person
{
    public string? Name { get; set; }

    public DayOfTheWeek TrainingDays;
}

[Flags]
public enum DayOfTheWeek : byte
{
    Sunday    = 0b_0000_0000, // i.e. 0
    Monday    = 0b_0000_0001, // i.e. 1
    Tuesday   = 0b_0000_0010, // i.e. 2
    Wednesday = 0b_0000_0100, // i.e. 4
    Thursday  = 0b_0000_1000, // i.e. 8
    Friday    = 0b_0001_0000, // i.e. 16
    Saturday  = 0b_0010_0000, // i.e. 32
}