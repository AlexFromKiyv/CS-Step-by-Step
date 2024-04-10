using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AttributeForValidation;

internal class AgeValidationAttribute : Attribute
{
    public int From { get; set; } = 0;
    public int To { get; set; } = 130;

    public AgeValidationAttribute(int from, int to)
    {
        From = from;
        To = to;
    }
    public AgeValidationAttribute()
    {
    }
}
