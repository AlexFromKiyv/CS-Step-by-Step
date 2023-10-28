using LinqOverCollections;
using System;
using System.Collections;

List<Car> myCar = new()
{
    new Car{ PetName = "Henry", Color = "Silver", Speed = 100, Make = "BMW"},
    new Car{ PetName = "Daisy", Color = "Tan", Speed = 90, Make = "BMW"},
    new Car{ PetName = "Mary", Color = "Black", Speed = 55, Make = "VW"},
    new Car{ PetName = "Clunker", Color = "Rust", Speed = 5, Make = "Yugo"},
    new Car{ PetName = "Melvin", Color = "White", Speed = 43, Make = "Ford"}
};

void CollectionToConsole<T>(IEnumerable<T>? collection)
{
    if (collection == null) return;

    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
}

void UseLinqForComplexObject()
{
    GetFastCars(myCar);

    void GetFastCars(List<Car> cars)
    {
        var queryFastCars = from c in cars where c.Speed > 55 select c;

        CollectionToConsole(queryFastCars);
    }
}

//UseLinqForComplexObject();

void UseLinqForComplexObjectWithComplexCriteria()
{
    GetFastCars(myCar);

    void GetFastCars(List<Car> cars)
    {
        var queryFastCars = from c in cars where c.Speed > 90 && c.Make == "BMW" select c;

        CollectionToConsole(queryFastCars);
    }
}

//UseLinqForComplexObjectWithComplexCriteria();

void LinqOverArrayList()
{
    // Here is a nongeneric collection of cars.
    ArrayList myCars = new ArrayList() 
    {
        new Car{ PetName = "Henry", Color = "Silver", Speed = 100, Make = "BMW"},
        new Car{ PetName = "Daisy", Color = "Tan", Speed = 90, Make = "BMW"},
        new Car{ PetName = "Mary", Color = "Black", Speed = 55, Make = "VW"},
        new Car{ PetName = "Clunker", Color = "Rust", Speed = 5, Make = "Yugo"},
        new Car{ PetName = "Melvin", Color = "White", Speed = 43, Make = "Ford"}
    };

    // Transform ArrayList into an IEnumerable<Car>-compatible type.
    var myCarsGeneric = myCar.OfType<Car>();

    var queryFastCars = from c in myCarsGeneric where c.Speed > 55 select c;

    CollectionToConsole(queryFastCars);

}

//LinqOverArrayList();

void FilteringNoGenericCollection()
{
    ArrayList myStuff = new();

    myStuff.AddRange(new object[]
    {
        10, 400, 8, false, new Car(), "Hi girl"
    }); 

    var myInts = myStuff.OfType<int>();

    CollectionToConsole(myInts);
}

//FilteringNoGenericCollection();

void FilteringByType()
{
    List<Exception> exceptions = new()
    {
      new ArgumentException(),
      new SystemException(),
      new IndexOutOfRangeException(),
      new InvalidOperationException(),
      new NullReferenceException(),
      new InvalidCastException(),
      new OverflowException(),
      new DivideByZeroException(),
      new ApplicationException()
    };

var queryArithmeticException = exceptions.OfType<ArithmeticException>();

CollectionToConsole(queryArithmeticException);
}
FilteringByType();