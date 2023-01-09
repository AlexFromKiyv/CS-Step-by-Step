# Array

## Створення

Масив це набір єлементів одного типу з доступом через числові індекси.

```cs

UsingSimpleArray();

static void UsingSimpleArray()
{
    int[] myIntArray = new int[3]; // declaring

    myIntArray[0] = 1;
    myIntArray[1] = 2;
    myIntArray[2] = 3;

    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine(myIntArray[i]);
    }

    //myIntArray[3] = 4; //do not work. Out of index
    int[] myNewArray = new int[10];

    foreach (int item in myNewArray)
    {
        Console.WriteLine(item);
    }
}
```
Треба зауважити new int[3] означає що загальна кількість єлементів 3. Індексація починаеться з 0. Коли ви задекларували масив память виділяеться і він заповнюється значеннями default. В цьому прикладі 0-ми. 

## Ініціалізація

Масив можна задекларувати і одразу задати значення єлементам. 

```cs

ArrayInitialization();

static void ArrayInitialization()
{
    string[] myStringArray = new string[] { "good", "better", "best" };
    bool[] myBoolArray = { true, false, false, true };
    int[] myIntArray = { 10, 15, 20 };

    Console.WriteLine(myStringArray);
    Console.WriteLine(myStringArray.Length);
    Console.WriteLine(string.Join(" < ",myStringArray));

    Console.WriteLine(myIntArray);
    Console.WriteLine(myIntArray.Length);
    Console.WriteLine(string.Join(", ", myIntArray));
}
```
При ініціалізації не потрібно вказувати розмір массиву. Також можна опустити new type[]