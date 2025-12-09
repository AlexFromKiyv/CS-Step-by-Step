# Логічні змінні

System.Boolean скорочено bool допомогає працювати з логічними змінними. Додамо проект Boolean з методом.

```cs
static void ExplorationOfBooleanType()
{
    bool myBool = default;
    Console.WriteLine($"Default: {myBool}");
    Console.WriteLine($"Type in: {myBool.GetType()}");
    Console.WriteLine($"Representation to string: {bool.TrueString}, {bool.FalseString} ");
}
ExplorationOfBooleanType();
```
```
Default: False
Type in: System.Boolean
Representation to string: True, False
```

Змінна типу <em>bool</em> може мати значення або False або True. В класі System.Boolean є строкове представлення ціх значень.    

## Логічні оператори AND &, OR |, XOR ^
```cs

AndOrXor();
void AndOrXor()
{
    bool a = true;
    bool b = false;
    Console.WriteLine($"AND  | True  | False    ");
    Console.WriteLine($"True | {a & a,-5} | {a & b,-5} ");
    Console.WriteLine($"False| {b & a,-5} | {b & b,-5} ");
    Console.WriteLine();
    Console.WriteLine($"OR   | True  | False    ");
    Console.WriteLine($"True | {a | a,-5} | {a | b,-5} ");
    Console.WriteLine($"False| {b | a,-5} | {b | b,-5} ");
    Console.WriteLine();
    Console.WriteLine($"XOR  | True  | False    ");
    Console.WriteLine($"True | {a ^ a,-5} | {a ^ b,-5} ");
    Console.WriteLine($"False| {b ^ a,-5} | {b ^ b,-5} ");
}
```
```
AND  | True  | False
True | True  | False
False| False | False

OR   | True  | False
True | True  | True
False| True  | False

XOR  | True  | False
True | False | True
False| True  | False
```
## Умовні логічні оператори AND &&, OR ||
```cs

ConditionalLogicalOperators();
void ConditionalLogicalOperators()
{
    Console.WriteLine($"True & TrueReturn :{true & TrueReturn()}");
    Console.WriteLine($"False & TrueReturn :{false & TrueReturn()}");
    Console.WriteLine($"True | TrueReturn :{true | TrueReturn()}");
    Console.WriteLine($"False | TrueReturn :{false | TrueReturn()}");

    Console.WriteLine($"True && TrueReturn :{true && TrueReturn()}");
    Console.WriteLine($"False && TrueReturn :{false && TrueReturn()}");
    Console.WriteLine($"True || TrueReturn :{true || TrueReturn()}");
    Console.WriteLine($"False || TrueReturn :{false || TrueReturn()}");

    bool TrueReturn()
    {
        Console.Write("Im Working.  ");
        return true;
    }
} 
```

```
Im Working.  True & TrueReturn :True
Im Working.  False & TrueReturn :False
Im Working.  True | TrueReturn :True
Im Working.  False | TrueReturn :True
Im Working.  True && TrueReturn :True
False && TrueReturn :False
True || TrueReturn :True
Im Working.  False || TrueReturn :True
```
Як видно оператори && || долучають другий операнд тількі тоді коли це потрібно. Це може бути кориснє коли багато поерандів.


## Метод Parse, TryParse

З строки можно отримати змінну типу.

```cs
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
UsingParse();
```
```
True
True
False
False

```

Але рядок може не відповідає типу. Для того щоб в консоль не викинувся виняток існуе метод TryParse

```cs
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
UsingTryParse();
```
Was parsing "False" well?:True False
Was parsing "Hi girl" well?:False False
Cannot convert "Hi girl" to bool.