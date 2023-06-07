//ExploringSystemObject();

static void ExploringSystemObject()
{
    System.Object obj = new System.Object();

    Console.WriteLine("obj-------------------");

    Console.WriteLine(obj.ToString());      
    Console.WriteLine(obj.Equals(0));       
    Console.WriteLine(obj.GetType());       
    Console.WriteLine(obj.GetHashCode());
 

    Console.WriteLine();
    Console.WriteLine("int myInt = 100;----"); 

    int myInt = 100;
    Console.WriteLine(myInt.ToString());
    Console.WriteLine(myInt.Equals(100));
    Console.WriteLine(myInt.GetType());
    Console.WriteLine(myInt.GetHashCode());
    Console.WriteLine($"int is ValueType: {myInt is ValueType}");

    Console.WriteLine();
    Console.WriteLine("string myString = \"Hi girl\";---");

    string myString = "Hi girl";
    Console.WriteLine(myString.ToString());
    Console.WriteLine(myString.Equals(100));
    Console.WriteLine(myString.GetType());
    Console.WriteLine(myString.GetHashCode());
    Console.WriteLine($"string is ValueType: {myString is ValueType}");
}

ItIsNoGoodUsingObject();

void ItIsNoGoodUsingObject()
{
    object weight = 69;
    object name = "Hanna";

    Console.WriteLine($"{name} weight {weight} kg");

    Console.WriteLine(((string)name).Length);
    Console.WriteLine((int)weight+1);
}


