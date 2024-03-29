# Простий консольний проект
Створимо простий проект.
1. Створіть папку. (Наприклад D:\Manual\Chapter02)  
2. Запустити Visual Studio.
3. Create new project
4. Вибрати Console App. Next.
5. Location згідно з тим яку папку зробили в п.1.
3. Project name : ConsoleAppWithoutTopLevel
4. Solution name : SimpleConsoleSolution
5. Next
6. Встановіть флажок Do not use top-level stattamens
7. Create

Подимивось файл ConsoleAppWithoutTopLevel\obj\Debug\netX.0\SimpleCSharpApp.GlobalUsings.g.cs. 

```cs
// <auto-generated/>
global using global::System;
global using global::System.Collections.Generic;
global using global::System.IO;
global using global::System.Linq;
global using global::System.Net.Http;
global using global::System.Threading;
global using global::System.Threading.Tasks;
```
Як бачимо вказане глобальне використаня різних просторів імен. Тобто ми маємо змогу звертатися до класів з простору імен System і тд.

Код файлу Program.cs виглядае так:

```cs
namespace ConsoleAppWithoutTopLevel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
```

Для того аби компілятор міг перетворити код програми в код виконання всі конструкції повинні відповідати конструкціям язика. Крім того C# потребуе щоб все логіка программи  було визначено в середині типу(клас, структура, ...) увигляді визначених данних та методів. В данному випадку визиваеться метод WriteLine статичного класу Console який находиться в System.  Метод Main точка входу для виконання програми. Объект классу в якому находиться метод Main називаеться application object. Може бути більше одного об`екта виконання. Компілятору можна вказати який метод використовувати як точку входу. Цю установку можна зробити в властивостях проекту в розділі Startup object.

## Top-level statamens

Додамо в рішення ше один проект.

1. Правий-клік на Solution "SimpleConsoleSolution"
2. Add
3. New project
4. Console App
5. Next
6. Project name: ConsoleAppWithTopLevel
7. Next
8. Не встановлюйте флажок Do not use top-level stattamens
9. Create

Файл Program.cs виглядає так:

```cs
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
```

Хоча тут немае визначення класу Program і методу Main при компіляції строки коди обгортаються в метод і клас неявно системою. Тут залишили тілки самі важливи дії викинувши рутинну навкого визначення повторюючихся речей. Працюючи з опреторами Top-level треба притримуватися правил:

- Top-level statamens модна використовувати в одному файлі
- Програма не може мати точки входу
- Їх не можна вкладати в простір імен
- Вони можуть обробляти вхідні данні (string[] args)
- Функції в класі Program локальні
- Додаткові типи можуть бути оголошені після всіх операторів верхнього рівня.
- Інструкції верхнього рівня компілюються до класу під назвою Program, дозволяючи додати частковий клас Program для зберігання звичайних методів.

Краша практика створити функції , які ви будете викликати в Program.cs, в окремому файлі та вручну визначте їх у частковому класі Program. Це об’єднає їх у автоматично створений клас Program на тому самому рівні, що й метод <Main>$, а не як локальні функції всередині методу <Main>$.

В проекті ConsoleAppWithTopLevel додамо класс myMath
1. На назві проекту правий клік 
2. Add
3. Class з назвою Program.MyMath.cs  

```cs
    partial class Program
    {
        static double RoundAdd(double a,double b) => Math.Round(a+b);
    }
```
В файлі Program.cs додайте
```cs
double result = RoundAdd(12.5434,4.34);
Console.WriteLine(result);
```
```
Hello, World!
17
```
Це крашій варіант але я для компактності небуду створювати додаткових файлів і буду створювати методи в Program.cs



## Передача программі параметрів з консолі.

Параметри отримані программою доступні в массиві string[] args

Додамо в рішеня проект з назвою GetParametersInApp

1. Правий-клік на Solution "SimpleConsoleSolution"
2. Add
3. New project
4. Console App
5. Next
6. Project name: GetParametersInApp
7. Next
8. Не встановіть флажок Do not use top-level stattamens
9. Create

Змінемо код Program.cs:

```cs
int length = args.Length;
string[] appArgs = Environment.GetCommandLineArgs();

Console.WriteLine($"The number of parameters:{length}");
for (int i = 0; i < length; i++)
{
    Console.WriteLine($"Prameter {i}:" + args[i]);
}

```
Для перевірки запустимо проект з консолі додавши параметри:
1. В Solution Explorer на проекті правий клік > Open in Terminal
2. В Terminal команда: dotnet run weight 65

Якшо потрібно щоб пробіли були частиною аргументу візміть параметр в подвійні або одинарні дужки.

В классі System.Environment є статичний метод за допомогою якого також можна отримати массив.
```cs
public static string[] GetCommandLineArgs();
```
У VS для цілей розробки можна вказати параметри запуску. На проекті правий клік > Properties > Debug > Open debug launch profiles UI

Таким чином коли ви користуетесь top-level statamens всерівно система формуе метод який отримуе string[]  

# Поверненя кода помилки після виконання програми.

Додамо в рішеня проект GetCodeErrorOutApp типу Console App.

Якшо його запустити в VS після виконанння програми ви можете помітити напис системи :

... exited with code 0.

За домовленностью якщо программа успішно відпрацювала ОС отримує код 0. В процессі розробки в деяких випадках вам треба передати код -1 шо означатиме шо шось пішло не так.

Провіримо з яким кодом виходить проект в Terminal в VS:

1. В Solution Explorer на проекті правий клік > Open in Terminal
2. В Terminal команда: dotnet run
3. В Terminal команда: $LastExitCode (повинен бути 0)
4. Змінимо код Program.cs    

```cs
Console.Write("Does this program work well?:(Y/N)");

string? enteredString = Console.ReadLine();
if (enteredString == "Y" || enteredString == "y")
{
    return 0;
}
else
{
    // Bad work 
    return -1; 
}
```
5. В Terminal команда: dotnet run
6. В Terminal команда: $LastExitCode (повинен вернути -1)

Таку можливість можна використовувати в тестуванні.

# Можливості класу Console
Доступ до класу забезпечуе global using global::System; Тобто ми можемо звертатися до класу Consol де забадаемо.

Додамо в рішеня проект UsingSystemConsole типу Console App.

Більшість методів классу статичні і їх можна визивати Console.NameMethod(...);

Додамо в проект метод який показує можливості ввода вивода тексту
```cs
UsingConsoleForInputOutputString();
static void UsingConsoleForInputOutputString()
{
    // Input string
 
    Console.Write("Enter name:");
    string? name = Console.ReadLine();
    Console.Write("What do you like?:");
    string? interests = Console.ReadLine();

    Console.Clear();

    //Output string

    Console.WriteLine("Hi {0}! {0} like {1}.", name, interests);
    Console.WriteLine("Hi {1}! {1} like {0}.", interests, name );
    Console.WriteLine($"Hi {name}! {name} like {interests}.");

}
```

Закоментуемо виклик методу.
```cs
// UsingConsoleForInputOutputString();
``` 

Додамо метод роботи з кольором.
```cs
UsingConsoleColor();
static void UsingConsoleColor()
{
    ConsoleColor beginColor = Console.ForegroundColor;
    Console.ForegroundColor = ConsoleColor.Red;
    for (int i = 0; i < 20; i++)
    {
        Console.WriteLine("DANGER!!!");
    }
    Console.Beep();
    Console.ForegroundColor = beginColor;
}

``` 
Закоментуемо виклик попереднього методу.

Метод WriteLine дозволяє форматувати числові данні для виводу в консоль. 
Додамо метод форматування чисел.
```cs
UsingNumericalFormatingForConsole();

static void UsingNumericalFormatingForConsole()
{
    Console.WriteLine($"For money C: {10000.00023:C}"); // гривня покаже ?
    Console.WriteLine($"For decimal D: {10023:D}");
    Console.WriteLine($"For decimal D9: {10023:D7}");
    Console.WriteLine($"For exponencial format E: {0.000025:E}");
    Console.WriteLine($"For fixed-poind format F: {0.025:F}");
    Console.WriteLine($"For fixed-poind format F3: {0.025:F3}");
    Console.WriteLine($"For fixed and exponencial format G: {1000000:G}");
    Console.WriteLine($"For fixed and exponencial format G: {0.00025:G}");
    Console.WriteLine($"For numerical format N: {0.025:N}");
    Console.WriteLine($"For numerical format N: {1000000:N}");
    Console.WriteLine($"Hexadecimal format X:{1000000:X}");
}
```

Форматування можна виконувати над текстом не тільки для консолі.
```cs
UsingFormattingToGetStringObject();
static void UsingFormattingToGetStringObject()
{
    string summ = string.Format($"{1000000:N} $");

    Console.WriteLine(summ);
}
```

Пошуковий запит:
```
.Net C# how formatting numeric
```


## Можливості класу System.Environment

Додамо в рішеня проект UsingSystemEnvironment типу Console App.

Змінемо код Program.cs:
```cs
Console.WriteLine("OS: "+Environment.OSVersion);
Console.WriteLine("Number of processors: " + Environment.ProcessorCount);
Console.WriteLine("Machine: " + Environment.MachineName);
Console.WriteLine(".NET: " + Environment.Version);
Console.WriteLine("Version: " + Environment.Version);
foreach (string drive in Environment.GetLogicalDrives())
{
    Console.WriteLine("Drive: " + drive);
}
Console.WriteLine("New line in this OS:" + Environment.NewLine);
Console.WriteLine("Is OS 64-bit: "+ Environment.Is64BitOperatingSystem);
Console.WriteLine("Path to the system directory: " + Environment.SystemDirectory);
Console.WriteLine("Current directory: " + Environment.CurrentDirectory);
Console.WriteLine("User name: "+Environment.UserName);
```

# Регістрозалежність

В С# Console і console це різні речі. Можете спробувати поміняти і VS допоможе вам высправити помилку. 

 - Ключові слова пишуться в нижньому регістрі: public, lock, class, dynamic
 - Простори імен, типи та імена членів починаються (за домовленістю) з великої літери:
Console.WriteLine, System.Data

VS та VSC допомогае правільно редагувати код. 
