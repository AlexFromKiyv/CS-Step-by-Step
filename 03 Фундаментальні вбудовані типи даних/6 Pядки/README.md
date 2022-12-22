# Рядки

System.String скорочено string допомогає працювати з рядками. Додамо проект Strings
```cs
ExplorationOfStrings_1();

static void ExplorationOfStrings_1()
{
    string myString = "Hi girl!";

    Console.WriteLine(myString);
    Console.WriteLine($"String is ValueType: {myString is ValueType}");
    Console.WriteLine($"Length:{myString.Length}");
    Console.WriteLine($"Compare with \"HI girl!\":{string.Compare(myString,"HI girl!")}");
    Console.WriteLine($"Contains \"girl\":{myString.Contains("girl")}");
    Console.WriteLine($"To uppper:{myString.ToUpper()}");
    Console.WriteLine($"Replace \" girl!\":{myString.Replace(" girl!"," !")}");
    Console.WriteLine(myString);
}
```
Як ми бачимо тип не є ValueType і тому зним ведется робота як з об'єктом. Крім того треба зазначити шо метод <em>Replace</em> не заминює <em>myString</em> а створює нову.

```cs
Concatination();
static void Concatination()
{
    string myString1 = "Hi";
    string myString2 = "everybody";
    string myString3 = myString1 + " " + myString2;
    myString3 += "!";
    Console.WriteLine(myString3);
}
``` 
Таким чином можна об'єднувати рядки.

```cs
Escapes();
static void Escapes()
{
    Console.WriteLine("Code\tName\tPrice");
    Console.WriteLine("D:\\Documents\\template.doc");
    Console.WriteLine("Text\n\n");
    Console.WriteLine("Text{0}{0}",Environment.NewLine);
    Console.WriteLine("\"New text\"");
    Console.WriteLine("\a");
}
```
Escape символи дозволяють по різному виводити текст а також додати ситемны звуки. Оскільки перехіду на нову строку відповідають різні символи в різних ОС иноді краше викорасти Environment.NewLine.


