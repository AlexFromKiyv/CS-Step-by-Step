UsingDynamic();

void UsingDynamic()
{
    dynamic something = "Ira";
    System.Console.WriteLine(something);
    System.Console.WriteLine(something.Length);
    something = 35;
    System.Console.WriteLine(something);
    //System.Console.WriteLine(something.Length); //don't work. It don't get compiler error.
    something = new[] { 90, 60, 90 };
    System.Console.WriteLine(something);
    System.Console.WriteLine(something.Length);
}


