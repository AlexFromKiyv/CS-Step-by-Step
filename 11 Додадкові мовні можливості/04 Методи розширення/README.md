# Методи розширення

Концепція методів розширення дозволяє додавати нові методи або властивості до класу чи структури, не змінюючи початковий тип безпосередньо. Отже, де це може бути корисним? Розглянемо такі можливості.
По-перше, скажімо, у вас є клас, який перебуває у production. З часом стає зрозуміло, що цей клас повинен підтримувати кілька нових членів. Якщо ви змінюєте поточне визначення класу безпосередньо, ви ризикуєте порушити зворотну сумісність зі старими базами коду, які його використовують, оскільки вони могли бути не скомпільовані з найновішою та найкращою версією визначення класу. Один із способів забезпечити зворотну сумісність – створити новий похідний клас з існуючого батьківського класу; однак, тепер у вас є два класи для обслуговування. Як усім відомо, підтримка коду — це найменш приваблива частина посадових обов'язків інженера-програміста.
Тепер розглянемо таку ситуацію. Припустимо, у вас є структура (або, можливо, запечатаний клас) і ви хочете додати нові члени, щоб вона поводилася поліморфно у вашій системі. Оскільки структури та запечатані класи не можна розширювати, ваш єдиний вибір — додати члени до типу, знову ж таки ризикуючи порушити зворотну сумісність!
Використовуючи методи розширення, ви можете змінювати типи без створення підкласів та без безпосередньої зміни типу. Загвоздка полягає в тому, що нова функціональність пропонується типу, лише якщо на методи розширення було зроблено посилання для використання у вашому поточному проєкті.

## Визначення методів розширення

Перше обмеження під час визначення методів розширення полягає в тому, що вони повинні бути визначені в межах статичного класу; отже, кожен метод розширення має бути оголошений з ключовим словом static. Другий момент полягає в тому, що всі методи розширення позначаються як такі за допомогою ключового слова this як модифікатора першого (і тільки першого) параметра відповідного методу. Параметр «this кваліфікований» представляє елемент, що розширюється.
Для ілюстрації створіть новий проект консольної програми з назвою ExtensionMethods. Тепер припустимо, що ви створюєте клас з назвою MyExtensions, який визначає два методи розширення. 
Перший метод дозволяє будь-якому об'єкту використовувати новий метод з назвою DisplayDefiningAssembly(), який використовує типи з простору імен System.Reflection для відображення назви збірки, що містить відповідний тип. Ви розглянете API рефлексії в іншому розділі. Якщо ви новачок у цій темі, просто зрозумійте, що рефлексія дозволяє вам виявляти структуру збірок, типів та членів типів під час виконання. 
Другий метод розширення, названий ReverseDigits(), дозволяє будь-якому цілочисельному числу отримати нову версію самого себе, де значення перетворюється цифра за цифрою. Наприклад, якщо ціле число зі значенням 1234 викликається методом ReverseDigits(), повернене ціле число встановлюється на значення 4321. Розглянемо таку реалізацію класу (імпортуйте простір імен System.Reflection):

```cs
using System.Reflection;

namespace ExtensionMethods;

static class MyExtensions
{
    // This method allows any object to display the assembly
    // it is defined in.
    public static void DisplayDefiningAssembly(this object obj)
    {
        Console.WriteLine($"" +
            $"{obj.GetType().Name} lives here: => " +
            $"{Assembly.GetAssembly(obj.GetType()).GetName().Name}\n");
    }

    // This method allows any integer to reverse its digits.
    // For example, 56 would return 65.
    public static int ReverseDigits(this int i)
    {
        // Translate int into a string, and then
        // get all the characters.
        char[] digits = i.ToString().ToCharArray();
        // Now reverse items in the array.
        Array.Reverse(digits);
        // Put back into string.
        string newDigits = new string(digits);
        // Finally, return the modified string back as an int.
        return int.Parse(newDigits);
    }
}
```
Зверніть увагу, як перший параметр кожного методу розширення було кваліфіковано ключовим словом this перед визначенням типу параметра. Завжди перший параметр методу розширення представляє тип, який розширюється. Оскільки DisplayDefiningAssembly() було прототиповано для розширення System.Object, кожен тип тепер має цей новий член, оскільки Object є батьківським для всіх типів на платформі .NET. Однак, ReverseDigits() був прототипований для розширення лише цілочисельних типів; тому, якщо будь-що, крім цілого числа, спробує викликати цей метод, ви отримаєте помилку під час компіляції.

    Зрозумійте, що даний метод розширення може мати кілька параметрів, але лише перший параметр можна кваліфікувати за допомогою цього параметра. Додаткові параметри будуть розглядатися як звичайні вхідні параметри для використання методом.

## Виклик методів розширення

Тепер, коли у вас є ці методи розширення, розглянемо наступний приклад коду, який застосовує метод розширення до різних типів у бібліотеках базових класів:

```cs
using ExtensionMethods;

static void InvokingExtensionMethods()
{
    // The int has assumed a new identity!
    int myInt = 12345678;
    myInt.DisplayDefiningAssembly();

    // So has the DataSet!
    System.Data.DataSet d = new System.Data.DataSet();
    d.DisplayDefiningAssembly();

    // Use new integer functionality.
    Console.WriteLine($"Value of myInt: {myInt}");
    Console.WriteLine($"Reversed digits of myInt: {myInt.ReverseDigits()}");
}
InvokingExtensionMethods();
```
```
Int32 lives here: => System.Private.CoreLib

DataSet lives here: => System.Data.Common

Value of myInt: 12345678
Reversed digits of myInt: 87654321
```

## Імпорт методів розширення

Коли ви визначаєте клас, що містить методи розширення, він, безсумнівно, буде визначено в просторі імен. Якщо цей простір імен відрізняється від простору імен, що використовує методи розширення, вам потрібно буде використати очікуване ключове слово C# using. Коли ви це зробите, ваш файл коду матиме доступ до всіх методів розширення для типу, що розширюється. Це важливо пам'ятати, оскільки якщо ви явно не імпортуєте правильний простір імен, методи розширення будуть недоступні для цього файлу коду C#.
Фактично, хоча на перший погляд може здаватися, що методи розширення мають глобальний характер, насправді вони обмежені просторами імен, які їх визначають, або просторами імен, які їх імпортують. Нагадаємо, що ви обгорнули клас MyExtensions у простір імен з назвою ExtensionMethods наступним чином:

```cs
namespace ExtensionMethods;

static class MyExtensions
{
    //...
}    
```
Щоб використовувати методи розширення в класі, вам потрібно явно імпортувати простір імен ExtensionMethods, як ми це зробили в операторах верхнього рівня, що використовуються для розгляду прикладів.

```cs
using ExtensionMethods;
```

# Розширення типів, що реалізують певні інтерфейси

На цьому етапі ви побачили, як розширювати класи (і, опосередковано, структури, що дотримуються того самого синтаксису) новою функціональністю за допомогою методів розширення. Також можна визначити метод розширення, який може розширювати лише клас або структуру, що реалізує інтерфейс. Наприклад, ви можете сказати щось на кшталт: «Якщо клас або структура реалізує IEnumerable\<T\>, то цей тип отримує такі нові члени». Звичайно, можна вимагати, щоб тип підтримував будь-який інтерфейс, включаючи ваші власні інтерфейси. 
Для ілюстрації додайте новий проект консольної програми з назвою InterfaceExtensions. Мета тут — додати новий метод до будь-якого типу, що реалізує IEnumerable, який включатиме будь-який масив та багато неузагальнених класів колекцій (нагадаємо, що узагальнений інтерфейс IEnumerable\<T\> розширює неузагальнений інтерфейс IEnumerable). Додайте наступний клас до вашого нового проекту:

```cs
namespace InterfaceExtensions;

static class AnnoyingExtensions
{
    public static void PrintDataAndBeep(this System.Collections.IEnumerable collection)
    {
        foreach (var item in collection)
        {
            Console.Write($"{item} ");
            Console.Beep();
        }
    }
}
```
Враховуючи, що метод PrintDataAndBeep() може використовуватися будь-яким класом або структурою, що реалізує IEnumerable, ви можете перевірити це за допомогою наступного коду:

```cs

using InterfaceExtensions;

static void ExtendingTypesImplementingSpecificInterfaces()
{
    // System.Array implements IEnumerable!
    string[] data =
      { "Wow", "this", "is", "sort", "of", "annoying",
      "but", "in", "a", "weird", "way", "fun!"};
    data.PrintDataAndBeep();

    Console.WriteLine();
    
    // List<T> implements IEnumerable!
    List<int> myInts = new List<int>() { 10, 15, 20 };
    myInts.PrintDataAndBeep();
}
ExtendingTypesImplementingSpecificInterfaces();
```
```
Wow this is sort of annoying but in a weird way fun!
10 15 20
```
Пам’ятайте, що ця мовна функція може бути корисною, коли ви хочете розширити функціональність типу, але не хочете створювати підкласи (або не можете створювати підкласи, якщо тип запечатаний) для цілей поліморфізму. Як ви побачите пізніше, методи розширення відіграють ключову роль для LINQ API. Фактично, ви побачите, що в API LINQ одним із найпоширеніших елементів, що розширюються, є клас або структура, що реалізує узагальнену версію IEnumerable.

## Підтримка методу розширення GetEnumerator

Метод foreach перевіряє методи розширення класу та, якщо знайдено метод GetEnumerator(), використовує цей метод для отримання IEnumerator для цього класу. Щоб побачити це в дії, додайте новий проект з назвою ForEachWithExtensionMethods. Додайте класи.

```cs
namespace ForEachWithExtensionMethods;
class Car
{
    // Car properties.
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = string.Empty;

    // Constructors.
    public Car() { }

    public Car(string petName, int currentSpeed)
    {
        PetName = petName;
        CurrentSpeed = currentSpeed;
    }
}

```
```cs
class Garage
{
    public Car[] CarsInGarage { get; set; }

    // Fill with some Car objects upon startup.
    public Garage()
    {
        CarsInGarage = new Car[4];
        CarsInGarage[0] = new Car("Rusty", 30);
        CarsInGarage[1] = new Car("Clunker", 55);
        CarsInGarage[2] = new Car("Zippy", 30);
        CarsInGarage[3] = new Car("Fred", 30);
    }
}
```
Зверніть увагу, що клас Garage не реалізує IEnumerable, а також не має методу GetEnumerator(). Метод GetEnumerator() додається через клас GarageExtensions, як показано тут:

```cs
using System.Collections;

namespace ForEachWithExtensionMethods;

static class GarageExtensions
{
    public static IEnumerator GetEnumerator(this Garage g) 
        => g.CarsInGarage.GetEnumerator();
}
```

Таким чином з класом Garage можна виконати наступне:

```cs
static void UsingGatage()
{
    Garage carLot = new Garage();

    // Hand over each car in the collection?
    foreach (Car c in carLot)
    {
        Console.WriteLine($"{c.PetName} is going {c.CurrentSpeed} MPH");
    }
}
UsingGatage();
```
```
Rusty is going 30 MPH
Clunker is going 55 MPH
Zippy is going 30 MPH
Fred is going 30 MPH
```