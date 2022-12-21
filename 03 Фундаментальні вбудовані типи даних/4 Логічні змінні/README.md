# Логічні змінні

Додамо проект Boolean з методом.

```cs
ExplorationOfBooleanType();

static void ExplorationOfBooleanType()
{
    bool myBool = default;
    Console.WriteLine($"Default: {myBool}");
    Console.WriteLine($"Type in: {myBool.GetType()}");
    Console.WriteLine($"Representation to string: {bool.TrueString}, {bool.FalseString} ");
}
```

Змінна типу <em>bool</em> може мати значення або False або True. В класі System.Boolean є строкове представлення ціх значень.    

# Метод Parse, TryParse

З строки можно отримати змінну типу.

```cs
UsingParse();

static void UsingParse()
{
    bool myBool = bool.Parse("True");
    Console.WriteLine(myBool);
    myBool = bool.Parse("true");
    Console.WriteLine(myBool);
    myBool = bool.Parse("false");
    Console.WriteLine(myBool);
    myBool = bool.Parse("False");
    Console.WriteLine(myBool);
    //myBool = bool.Parse("t");//do not work
    //myBool =bool.Parse("0"); 
    //myBool = bool.Parse("T");
    //myBool = bool.Parse("");
    //myBool =bool.Parse(null);
}

UsingTryParse();
```
Але рядок може не відповідає типу. Для того щоб в консоль не викинувся виняток існуе метод TryParse

```cs
UsingTryParse();

static void UsingTryParse()
{
    string myString = "False";
    bool resultParsing = bool.TryParse(myString, out bool myBool1);

    Console.WriteLine($"Was parsing \"{myString}\" well?:{resultParsing} {myBool1}");

    myString = "Hi girl";
    resultParsing = bool.TryParse(myString, out bool myBool2);

    Console.WriteLine($"Was parsing \"{myString}\" well?:{resultParsing} {myBool2}");

    if (resultParsing)
    {
        Console.WriteLine(myBool2);
    }
    else
    {
        Console.WriteLine($"Cannot convert \"{myString}\" to bool.");
    }
}
```