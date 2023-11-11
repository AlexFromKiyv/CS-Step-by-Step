using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExpressions
{
    record class Car(string Manufacturer, string Name, int Year);

    record class Place(string Name);

    record class Person(string Name);
}
