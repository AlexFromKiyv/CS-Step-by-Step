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
