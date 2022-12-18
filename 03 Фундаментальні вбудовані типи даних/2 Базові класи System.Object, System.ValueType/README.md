# 2 Базові класи System.Object, System.ValueType

Всі типи в С# походять від класу System.Object і відповідно успадковують його поведінку.
Отже варто розглянути його методи. Додамо в рішення проект BaseClasses і додамо метод.  

```cs
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
```

Як бачимо <em>int</em> є скороченням типу <em>System.Int32</em>. Крім того ми бачимо шо int є нашадком System.ValueType. Це означае що змінні такогу типу при виконанні програми розміщуються в стеку.В стеку, відповідно до типу, виділяеться необхідна кількість памяті і при використяні зміної туди записуеться значення.  Коли відробила та частину коду де створено змінну частина пам'яті в стеку де була змінна звільняється.
Таким чином змінні типу ValueType швидкі і єффективні.

В той же час тип <em>string</em> не є ValueType і тому в стеку зберігаеться посиланя на об`ект строки в <em>heap</em>.













