using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomGenericType
{
	class Car
	{
        public string Model { get; set; }
        public int Yead { get; set; }

        public Car(string model, int yead)
        {
            Model = model;
            Yead = yead;
        }
    }

	class MyGeneticWithDefaultConstructor<T>  where T : new()  
	{
        public T? MyProperty { get; set; }
    }

    class MyGenericWithManyConstraint<T> where T : class, IEnumerable, new()
    {

    }

    class AccountDocument
    {
        public int Id { get; set; }
    }

    class MyGenericWithManyConstraintAndParameters<D,T> where D : AccountDocument,
        new() where T : struct, IComparable<T>
    {

    }    

    class myClass
    {
        public int MyFunc<T>(T x) where T: struct  
        {
            return 1;
        }
    }
}
