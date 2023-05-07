
using IEnumerable_IEnumerator;

//ArrayIsEnumerable();
void ArrayIsEnumerable()
{
    int[] temperatures = { 4, 6, 8 };

    foreach (int item in temperatures)
    {
        Console.WriteLine(item);
    }
}

//IEnumerableFromArray_1();
void IEnumerableFromArray_1()
{
    Garage garage = new();

    foreach (var item in garage)
    {
        Console.WriteLine($"{((Car)item).Name}");
    }

    Console.WriteLine();

    var ie = garage.GetEnumerator();

    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}");
    Console.Write($"{((Car)ie.Current).Name}\t");
    Console.WriteLine($"The method Current returned : {ie.Current}\t");

    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}");
    ie.Reset();
    Console.WriteLine($"Reset");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
}


//IEnumerableFromArray_2();
void IEnumerableFromArray_2()
{
    Garage_v1 garage = new();

    //var enumerator = garage.GetEnumerator(); //class cannot contain definition GetEnumerator  

    foreach (Car item in garage)
    {
        Console.WriteLine($"{item.Name} {item.MaxSpeed}");
    }
}

//ExamineIEnumerableWithYield();
void ExamineIEnumerableWithYield()
{
    Garage_v2 garage = new();

    foreach (Car item in garage)
    {
        Console.WriteLine($"{item.Name} {item.MaxSpeed}");
    }

}

//ExplorationGetEnumerator();
void ExplorationGetEnumerator()
{
    Garage_v3 garage = new();

    var enumerator = garage.GetEnumerator();

    Console.WriteLine("After called GetEnumerator");

    enumerator.MoveNext();
}

//ExplorationGetEnumeratorWithFunction();
void ExplorationGetEnumeratorWithFunction()
{
    Garage_v4 garage = new();

    var enumerator = garage.GetEnumerator();

    Console.WriteLine("After called GetEnumerator");

    enumerator.MoveNext();
}



ExplorationNamedEnumerator();
void ExplorationNamedEnumerator()
{
    Garage_v5 garage = new();

    foreach (Car item in garage.GetTheCars(true))
    {
        Console.WriteLine($"{item.Name} {item.MaxSpeed}");
    }

    foreach (Car item in garage.GetTheCars())
    {
        Console.WriteLine($"{item.Name} {item.MaxSpeed}");
    }
}