using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CommonSnappableTypes;

[AttributeUsage(AttributeTargets.Class)]
public class CompanyInfoAttribute : Attribute
{
    public string CompanyName { get; set; } = string.Empty;
    public string CompanyUrl { get; set; } = string.Empty;
}
