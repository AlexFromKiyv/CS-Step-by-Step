# Dynamic тип.

Додамо проект Dynamics.

Тип dynamic дозволяє зберігати дані будь якого типу.
```cs
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
```
Це працює але за рахунок втрати продуктивності. Так само як з System.Object перед виконанням певних дій требе виконувати перетворення типу якє зменьшую продуктивність.
