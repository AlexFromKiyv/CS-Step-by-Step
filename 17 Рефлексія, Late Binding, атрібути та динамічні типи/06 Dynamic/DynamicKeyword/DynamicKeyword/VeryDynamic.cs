using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicKeyword;
internal class VeryDynamic
{
    private static dynamic _fild;
    public dynamic Property { get; set; }

    public dynamic Method(dynamic parameter)
    {
        dynamic dynamicVariable = parameter;

        if (parameter is string)
        {
            return dynamicVariable.ToUpper();
        }
        else
        {
            return dynamicVariable;
        }
    }
}

