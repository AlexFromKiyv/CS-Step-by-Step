ExploringSystemObject();
static void ExploringSystemObject()
{
    System.Object obj = new System.Object();

    Console.WriteLine("obj-------------------");

    Console.WriteLine(obj.ToString());      
    Console.WriteLine(obj.Equals(0));       
    Console.WriteLine(obj.GetType());       
    Console.WriteLine(obj.GetHashCode());

    Console.WriteLine();
    Console.WriteLine("int myValue = 100;----"); 

    int myValue = 100;
    Console.WriteLine(myValue.ToString());
    Console.WriteLine(myValue.Equals(100));
    Console.WriteLine(myValue.GetType());
    Console.WriteLine(myValue.GetHashCode());

    Console.WriteLine();
    Console.WriteLine("string myString = \"Hi girl\";---");

    string myString = "Hi girl";
    Console.WriteLine(myString.ToString());
    Console.WriteLine(myString.Equals(100));
    Console.WriteLine(myString.GetType());
    Console.WriteLine(myString.GetHashCode());

    Console.WriteLine(myString.Length);
}
