Дужки {} використовують аби декілька послідовних визначень або дій об'єднати в одне ціле.
В умовних операторах і циклах дозволено залишати один рядок коду без дужок але це може бути джерелом помилок.

```cs
BadCode();
static void BadCode()
{
    string enteredString = "";
    while (enteredString != "y")
        Console.Write("Do you want to exit ? (Y/N):");
        enteredString = Console.ReadLine();

}
```
Хоча такій код компілюеться і виконується він приводить до того шо виконання застряє в нескінченому циклі.
Добре шо видно шо шось не так. Але коли ви доопрацьовуєте код написаний в такому стилі то можете зробить небажані помилки. Тому краше в операторах for, while, if і подібних виділяти блок коду.

```cs
ItWork();
static void ItWork()
{
    string enteredString = "";
    while (enteredString.ToLower() != "y")
    {
        Console.Write("Do you want to exit ? (Y/N):");
        enteredString = Console.ReadLine();
    }
}
```



