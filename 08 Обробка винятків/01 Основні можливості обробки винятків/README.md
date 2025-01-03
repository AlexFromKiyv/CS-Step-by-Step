# Основні можливості обробки винятків

Кажучи про якість програмного забезпеченя відрізняють:

Bugs - помилка яку заклав програміст.
User errors - дії користувача які не передбачив програміст.
Exception - це ситуації які виникають в процесі роботи і вважаються неприйнятними для корректної роботи і які є можливість передбачити.
Навіть коли програміст заклав помилку та непередбачив помилку користувача среда виконання генерує відповідний виняток. В бібліотеці базових класів визначені класи винятків FormatException, IndexOutOfRangeException, FileNotFoundException, ArgumentOutOfRangeException та інші.

## Роль обробки винятків в .Net

Помилки це звічайна річ і вони виникають постійно. Підходи коли помилкам давали числові значення не ідеальні, трохи застарілі але ще працюють. Кілкість чисел-помилок може в проекті дуже збільшуватися.

Аби команди не придумували свої варіанти, .Net дає свою стандартну струкруровану обробку винятків. Це уніфікований підхід до обробки помилок. Iншою перевагою структурованої обробки винятків .Net є шо замість загадкового числа створюється об'єкт помилки в якому міститься тип , опис а також детальний знімок стеку викликів якій ініціював проблему. Також користувач може отримати додадтоку інформацію як можна вирішити проблему у вигляді спеціальних даних або посилання.

Аби обробити виняток треба використати:
- Тип класу шо представляє виняток.
- Член класу який видає виняток за обставин.
- Блок коду який викликає члена у випадку різних обставин.
- Блок коду який перехоплює виняток. 

Об'єкт шо представляє проблему яка винилка є єкземпляром класу шо розширюється від  System.Exception або його нащадків.

## System.Exception

Всі винятки ініційовані в .Net остаточно походять від System.Exception. 

```cs
    //
    // Summary:
    //     Represents errors that occur during application execution.
    public class Exception : ISerializable
    {
        public Exception();
        public Exception(string? message);
        public Exception(string? message, Exception? innerException);
        protected Exception(SerializationInfo info, StreamingContext context);

        public virtual string? StackTrace { get; }
        public virtual string? Source { get; set; }
        public virtual string Message { get; }
        public Exception? InnerException { get; }
        public int HResult { get; set; }
        public virtual IDictionary Data { get; }
        public MethodBase? TargetSite { get; }
        public virtual string? HelpLink { get; set; }
        ...
    }
```
Це основна частина визначення класу. Ці всі конструктори і методи використовуються далі.

## Виняткова ситуація.

Розглянемо ситуацію коли ми можемо створити виняток.  

Exeptions\BaseClassExeption\Car_v1.cs

```cs
    namespace BaseClassExeption;
    
    public class Car_v1
    {
        public const int MAXSPEED = 140;

        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }
        private bool _carIsDead;

        public Car_v1(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }

        public Car_v1()
        {
        }

        public void Accelerate(int delta)
        {
            if (_carIsDead)
            {
                Console.WriteLine($"{Name} is out of order ..."); 
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MAXSPEED)
                {
                    Console.WriteLine($"{Name} has overheated!");
                    CurrentSpeed = 0;
                    _carIsDead = true;
                }
                else
                {
                    Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
                }
            }
        }
    }
```
```cs

void ExplorationTheOccurationOfAnException()
{
    Car_v1 car = new("Nissan Leaf", 35);

	for (int i = 0; i < 10; i++)
	{
		car.Accelerate(20);
	}
}
ExplorationTheOccurationOfAnException();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135
Nissan Leaf has overheated!
Nissan Leaf is out of order ...
Nissan Leaf is out of order ...
Nissan Leaf is out of order ...
Nissan Leaf is out of order ...
```
В цьому прикладі ми будемо створювати структурований виняток за відомими обставинами. Логіка класу проста коли відбувається збільшення швидкості(Accelerate) відбуваеться превірка масимальної швидкості. Коли шіидкість перевишує масимально допустиму двигун прегрівається шо фіксуеться в приватній змінній.(В цому випадку краще б було замість константи використовувати властивість тільки для читаня але це не змінює суті). У цьому прикладі знаючи всі обмеженя ми самі конролюємо ситуацію. Поточна реалізація просто показує прегрів але можна можна покрашити ситуацію запобігши цьому.

## throw і загальний виняток.

Exeptions\BaseClassExeption\Car_v2.cs
```cs
    namespace BaseClassExeption;

    public class Car_v2
    {
        public const int MAXSPEED = 140;
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }

        private bool _carIsDead;

        public Car_v2(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v2()
        {
        }

        public void Accelerate(int delta)
        {
            if (_carIsDead)
            {
                Console.WriteLine($"{Name} is out of order ...");
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MAXSPEED)
                {
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new Exception($"{Name} has overheated!");
                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }
```
```cs
void ExplorationThrow()
{
    Car_v2 car = new("Nissan Leaf", 35);

	for (int i = 0; i < 10; i++)
	{
		car.Accelerate(20);
	}
}
ExplorationThrow();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135
Unhandled exception. System.Exception: Nissan Leaf has overheated!
   at BaseClassExeption.Car_v2.Accelerate(Int32 delta) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\BaseClassExeption\Classes_v2.cs:line 40
   at Program.<<Main>$>g__ExplorationThrow|0_1() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\BaseClassExeption\Program.cs:line 23
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\BaseClassExeption\Program.cs:line 16
```
В цьому випадку програма аварійно закінчує свою роботу вказуючи шо є необроблений виняток.

## Можливості Visual Studio.

В VS проект можна запустити в двух режимах. В режимі з Debug > Start Debugging (F5) проект зупинеться на містці де викидується виняток.
Коли проєкт зупиняється він показує місце де відбувся виняток. Також можна використати Show Call Stack та View Details щоб подивитись ланцюг визовів і вміст винятку.


## Конструкція try ... catch і загальний виняток.

```cs
void ExplorationTryCatch()
{
    Car_v2 car = new("Nissan Leaf", 35);

    Console.WriteLine("---The begin of try---\n");
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }

    }
    catch (Exception e)
    {
        string stringForShow = "\n\n" +
            $"Attention! Problem occured!\n\n" +
            $" Method: {e.TargetSite}\n" +
            $" Message: {e.Message}\n" +
            $" Source: {e.Source}\n";
        Console.WriteLine(stringForShow);
    }
    Console.WriteLine("---The end of try---");
}
ExplorationTryCatch();
```
```
---The begin of try---

Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135


Attention! Problem occured!

 Method: Void Accelerate(Int32)
 Message: Nissan Leaf has overheated!
 Source: BaseClassExeption

---The end of try---
```
Як ви бачите виняток створюється в результаті використовування об'єктів класу. Коли виникає виняток управління переходіть в точку де використовується метод класу тому цей метод повинет бути готовий до такої ситуації. Для цого використовуеться спроба виконати можливо метод який може викинути виняток і для цього використовується блок try. Якшо цей блок виконується без винятку блок catch пропускаеться. У видадку винятка блок catch спрацьовує і перхопляє об'єкт винятку. В цей момент можна використати вміст цього об'екту аби показати проблему. 

Як ви бачите програма не вивалюється з помилкою. Коли виникають винятки можливи ситуації коли программа продовжує працювати але не так єфективно.(наприклад немає доступу до даних). 

## Властивості об'єкта Exception детально.

### TargetSite

```cs
void ExplorationExceptionMemberTargetSite()
{
    Car_v2 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (Exception e)
    {
        string stringForShow = "\n\n" +
            $" Member Name: {e.TargetSite}\n" +
            $" Class defining member: {e.TargetSite?.DeclaringType}\n" +
            $" Memeber Type: {e.TargetSite?.MemberType}\n";
        Console.WriteLine(stringForShow);
    }
}
ExplorationExceptionMemberTargetSite();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135


 Member Name: Void Accelerate(Int32)
 Class defining member: BaseClassExeption.Car_v2
 Memeber Type: Method
```
TargetSite - властивість шо містить об'єкт System.Reflection.MethodBase який використовується для збору інформації про метод що створив виняток. 

### StackTrace

```cs
void ExplorationExceptionMemberStackTrace()
{
    Car_v2 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (Exception e)
    {
        string stringForShow = "\n\n" +
            $" Stack: {e.StackTrace}\n";
        Console.WriteLine(stringForShow);
    }
}
ExplorationExceptionMemberStackTrace();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135


 Stack:    at BaseClassExeption.Car_v2.Accelerate(Int32 delta) in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\BaseClassExeption\Classes_v2.cs:line 40
   at Program.<<Main>$>g__ExplorationExceptionMemberStackTrace|0_4() in D:\MyWork\CS-Step-by-Step\08 Обробка винятк?в\Exeptions\BaseClassExeption\Program.cs:line 90
```
System.Exception.StackTrace - показує серію викликів які привели до винятку. Ця властивість створюється середою виконання. Зверніть увагу шо метод класу в якому виникнув виняток стоїть зверху тоді як клієнт шо визвав метод знизу. Це може допомогти розібратись. 

### HelpLink
```cs
    class Car_v3
    {
        public const int MAXSPEED = 140;
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }

        private bool _carIsDead;

        public Car_v3(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v3()
        {
        }

        public void Accelerate(int delta)
        {
            if (_carIsDead)
            {
                Console.WriteLine($"{Name} is out of order ...");
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MAXSPEED)
                {
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new Exception($"{Name} has overheated!") 
                    { 
                        HelpLink = "https://www.youtube.com/results?search_query=car+engine+overhead"
                    };
                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }
```
```cs
void ExplorationExceptionMemberHelpLink()
{
    Car_v3 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (Exception e)
    {
        string stringForShow = "\n\n" +
            $" Help link: {e.HelpLink}\n";
        Console.WriteLine(stringForShow);
    }
}
ExplorationExceptionMemberHelpLink();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135


 Help link: https://www.youtube.com/results?search_query=car+engine+overhead
```
HelpLink - властивість яка може містити певну URL-адресу або файл довідки шо містить білше про часті проблеми.

### Data

```cs
    public class Car_v4
    {
        public const int MAXSPEED = 140;
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }

        private bool _carIsDead;

        public Car_v4(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v4()
        {
        }

        public void Accelerate(int delta)
        {
            if (_carIsDead)
            {
                Console.WriteLine($"{Name} is out of order ...");
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MAXSPEED)
                {   
                    int tempSpeed = CurrentSpeed;
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new Exception($"{Name} has overheated!") 
                    { 
                        Data =
                        {
                            {"TimeStamp",$"The car exploded at {DateTime.Now}" },
                            {"Clause",$"The speed is too high {tempSpeed}. Maximum speed is {MAXSPEED}" }
                        }
                    };
                }
                Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
            }
        }
    }
```
```cs
void ExplorationExceptionMemberData()
{
    Car_v4 car = new("Nissan Leaf", 35);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(20);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine();

        string stringForShow = "\nProblem:\n";
        foreach (DictionaryEntry item in e.Data)
        {
            stringForShow += $"{item.Key} : {item.Value}\n";
        }
        Console.WriteLine(stringForShow);
    }
}
ExplorationExceptionMemberData();
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135


Problem:
TimeStamp : The car exploded at 07.04.2023 14:59:17
Clause : The speed is too high 155. Maximum speed is 140
```
Data - містить об'єкт реалізує інтерфейс System.Collections.IDictionary. Такі об'єкти містять колекції ключ-значення. Ця властивість корисна тим шо дозволяє запакувати дані шо до стану в момент помилки без потреби створення додадкового типу для розширення типу Exception. Не завжди треба створювати власний тип винятків і не треба забувати властивість Data.

## Винятки можна уникнути використовуючи if

Не треба забувати шо виняток можна передбачити і обробити за допомогою іf

```cs
void UsingIf()
{
    while (true)
    {
        Console.Write("Enter whole number:");
        PrintIncrement(Console.ReadLine());
    }

    void PrintIncrement(string? enteredString)
    {
        if(int.TryParse(enteredString,out int number))
        {
            number++;
            Console.WriteLine(number);
        }
        else
        {
            Console.WriteLine("You entered not whole number!");
        }
    }
}
UsingIf();
```
```
Enter whole number:1
2
Enter whole number:2
3
Enter whole number:3
4
Enter whole number:1000
1001
Enter whole number:girl
You entered not whole number!
Enter whole number:
```
Тут в нагоді пригодився метод класу Int32 TryParse. Такий підхід уникнути виняток може бути швидше і простіше. Тут вся відповідальність перекладається на метод з сістемної бібліотеки.

Також можна реалізовувати методи використовуючи шаблон try.
```cs
static bool TryParse(string? input, out Person value)
{
  if (someFailure)
  {
    value = default(Person);
    return false;
  }
  // successfully parsed the string into a Person
  value = new Person() { ... };
  return true;
}
```
