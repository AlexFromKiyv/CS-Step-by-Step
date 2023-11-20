using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExpressions
{
    record class Car(string Manufacturer, string Name, int Year);
    record class Place(string Name);
    record class Person(string Name, int Age, List<string> Languages);
    record class Cart_item(int Id, int Product_Id, int Quantyty);
    record class Product(int Id, string Name, double Price);
    record class Driver(string Name, int Experience);
    record class Vehile(string Name, Driver Owner);
}
