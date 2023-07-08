# Узагальнення (Generic)

Узагальненими можуть бути класи, інтерфейси, структури і делегати. Узагальнений тип виглядає з кутавими дужками і символом.(наприклад List<T>). 
Приклади узагальнень можна побачити Visual Studio > View > Object Browser і в пошуку ввести System.Collections.Generic. 
Літера Т це параметр типу або заповнювачи. Напис IEnumerable<T> можна прочитати 
"IEnumerable of type T". T зазвичай використовується для типів, TKey для ключів, TValue для значень.
Коли вистворюєте похідну загального типу, реалізовуете загальний інтерфейс або викликаете загал ьний член треба вказати значеня параметру в вигляді типу. 

### Визначення параметрів типу узагальнених класів/структур.

Generic\Program.cs
```cs
void SpecifyingGenericParameters()
{
    List<Car> cars = new() 
    {
        new Car(1,"VW", "Beetle", 2020), 
        new Car(2,"VW", "E-Buster", 2025) 
    };

    Car car = cars[0];
    car.Year = 2016;

    foreach (Car? item in cars)
    {
        item.ToConsole();
    }
}

SpecifyingGenericParameters();
```
```
VW      Beetle  2016
VW      E-Buster        2025
```
При створенні екземпляра узагальненого класу або структури вказується параметр який є типом який буде використовувати узагальнений клас. В цьому прикладі використовується узагальнений клас List<T>. Строку List<Car> можна прочитати як список об'єктів авто (list of car objects). Тип вказується при стоврені і не залишаеться незмінним. 

Частина визначення класу List<T> виглядае так:

```cs
  public class List<T> : IList<T>, IList, IReadOnlyList<T>
  {
    ...
    public void Add(T item);
    public void AddRange(IEnumerable<T> collection);
    public ReadOnlyCollection<T> AsReadOnly();
    public int BinarySearch(T item);
    public bool Contains(T item);
    public T this[int index] { get; set; }
    ...
  }
```

При визначені зміної типу List<Car> відбуваеться створення об'єкту наче був ствоений обект наступного класу:

```cs
  public class List<Car> : IList<Car>, IList, IReadOnlyList<Car>
  {
    ...
    public void Add(Car item);
    public void AddRange(IEnumerable<Car> collection);
    public ReadOnlyCollection<Car> AsReadOnly();
    public int BinarySearch(Car item);
    public bool Contains(Car item);
    public Car this[int index] { get; set; }
    ...
  }
```
Компілятор не створює додадкового класу, а скоріше використовує члени загального типу.

### Використаня загальних членів неузагальненого класу.

Корисно шо в неузагальнених класах є узагальнені властивості.
```cs
void UseGenericMemeber()
{
    int[] ints = { 31, 22, 13, 4, 25 };

    Array.Sort<int>(ints);

    foreach (int item in ints)
    {
        Console.Write(item+"\t");
    }
}

UseGenericMemeber();
```
```
4       13      22      25      31
```
При використані узагальнених членів треба вказати тип заповнювача.

### Визначення параметрів типу узагальнених інтерфейсів.

Загальни інтерфейси реалізують аби класи і структири підтримувати необхідну поведінку( наприклад сотрування об'єктів). Розглянемо інтерфейс IComparable
```cs
    //
    // Summary:
    //     Defines a generalized type-specific comparison method that a value type or class
    //     implements to order or sort its instances.
    public interface IComparable
    {
        //
        // Summary:
        //     Compares the current instance with another object of the same type and returns
        //     an integer that indicates whether the current instance precedes, follows, or
        //     occurs in the same position in the sort order as the other object.
        //
        // Parameters:
        //   obj:
        //     An object to compare with this instance.
        //
        // Returns:
        //     A value that indicates the relative order of the objects being compared. The
        //     return value has these meanings:
        //     Value – Meaning
        //     Less than zero – This instance precedes obj in the sort order.
        //     Zero – This instance occurs in the same position in the sort order as obj.
        //     Greater than zero – This instance follows obj in the sort order.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     obj is not the same type as this instance.
        int CompareTo(object? obj);
    }
```
Реалізація інтерфейсу для Car

```cs
    public class Car : IComparable
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = "Undefined";
        public string Model { get; set; } = "Undefined";
        public int Year { get; set; }

        public Car(int id, string manufacturer, string model, int year)
        {
            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public Car()
        {
        }

        public void ToConsole() => Console.WriteLine($"{Id}\t{Manufacturer}\t{Model}\t{Year}");

        public int CompareTo(object? obj)
        {
            if (obj == null) return 1; 

            if (obj is Car tempCar)
            {
                return Id.CompareTo(tempCar.Id);
            }
            throw new ArgumentException("Parameter is not a car.");
        }
    }
```
В цій реалізації неузагальнений інтерфейс оперує з object і тому потребує перевірки типу. Крім того у випадку винятку треба викидати ArgumentException. 

Існує також узагальнений тип IComparable<T>.

Car_v1.cs
```cs
   public class Car_v1 : IComparable<Car_v1>
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; } = "Undefined";
        public string Model { get; set; } = "Undefined";
        public int Year { get; set; }

        public Car_v1(int id, string manufacturer, string model, int year)
        {
            Id = id;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public Car_v1()
        {
        }

        public void ToConsole() => Console.WriteLine($"{Manufacturer}\t{Model}\t{Year}");

        public int CompareTo(Car_v1? other)
        {
            if (other == null) 
            {
                return 1;
            }
            else
            {
                return Id.CompareTo(other.Id);
            }
        }
    }
```
В цьому прикладі використовуеться узагальнений варіант інтерфейсу. В цьому випадку код методу CompareTo значно очищено. Тут не потрібно первіряти чи є вхідний параметр типу Car_v1 до ту він може бути тільки такого типу або null. Якшо хтось захоче передати інший тип буде помилка компіляції. 

```cs
void UsingGeneticInterface()
{
    List<Car_v1?> cars = new()
    {
        new Car_v1(3,"VW","New Beetle",1998),
        new Car_v1(1,"VW", "Käfer", 1938),
        null,
        new Car_v1(2,"VW", "Golf", 1974),
        null
    };

    Console.WriteLine("\n Before sorting."  );
    PrintListCar(cars);

    cars.Sort();

    Console.WriteLine("\n After sorting.");
    PrintListCar(cars);

    void PrintListCar(List<Car_v1?> list)
    {
        foreach (var item in list)
        {
            if (item is null)
            {
                Console.WriteLine();
            }
            else
            {
                item.ToConsole();
            }
        }
    }
}

UsingGeneticInterface();
```
```
 Before sorting.
VW      New Beetle      1998
VW      Kafer   1938

VW      Golf    1974


 After sorting.


VW      Kafer   1938
VW      Golf    1974
VW      New Beetle      1998
```

## System.Collections.Generic

При створенні програм вам потрібен спосіб керування даними в пам'яті. Класи System.Collections.Generic скоріш за все відповідатимуть висогам.
Для більшості неузагальнених типів існують узагальнені заміни які більш корисні для використання.
Можна знайти ряд узагальнених інтерфейсів які розширюють свій неузагальнений аналог. Наприклад, IEnumerable<T> розширює IEnumerable. Тобто реалізація в класі буде підтримувати функціональність неузагальнених інтерфейсів.

Основні узагальнені інтерфейси простору. Всі вони public interface

ICollection<T> : IEnumerable<T>, IEnumerable : Визначає загальні характеристики типових колекції(розміри,можливості перебору та безпеки потоку).Дозворяє дії: Add, Remove, Clear, Contains, Count, тощо. 

    IComparer<in T> : Визначає спосіб порівняння об'єктів.

    IDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable : Дозволяє загальному об'єкту колекції представляти свій вміст за допомогою пар ключ-значення.

    IEnumerable<out T> : IEnumerable :Повертає об'ект який реалізовує інтерфейс IEnumerator<T>.

    IEnumerator<out T> : IEnumerator, IDisposable : Підтримує просту ітерацію в стилі foraech над узагалненою коллекцїєю.

    IList<T> : ICollection<T>, IEnumerable<T>, IEnumerable : Додає поведінку роботи з індексами. 

    ISet<T> : ICollection<T>, IEnumerable<T>, IEnumerable : Надає базовий інтерфейс для абстракції множин.

Простір імен також мае класи які реалізують ці ключові інтерфейси. Всі public class. Ось найбільш важливі. 

    List<T> : IList<T>, IList, IReadOnlyList<T> : Узагальнений типом послідовний список з можливістю дінамічної зміни кількості елементів.

    LinkedList<T> : ICollection<T>, ICollection, IReadOnlyCollection<T>, ISerializable, IDeserializationCallback : Представляє подвійно зв'язний список.

    Stack<T> : IEnumerable<T>, System.Collections.ICollection, IReadOnlyCollection<T> : Узагальнена реалізація списку з принципом "first-in, first-out".

    Queue<T> : IEnumerable<T>, System.Collections.ICollection,   IReadOnlyCollection<T> : Узагальнена реалізація списку з принципом "last-in, first-out".

    Dictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>, ISerializable, IDeserializationCallback where TKey : notnull : Словник узагальнених пар ключ-значення.

    SortedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue> where TKey : notnull : Відсортований словник пар пар ключ-значення.

    SortedSet<T> : ISet<T>, ICollection<T>, ICollection, IReadOnlyCollection<T>, IReadOnlySet<T>, ISerializable, IDeserializationCallback : Колекція об'єктів яка зберігається в порядку сортування без дублювання. 

Простір імен також визначає допоміжні класи та структури для роботи з певними контейнерами. Наприклад клас LinkedList<T> має член LinkedListNode<T>, який представляє вузол колекції. Або є допоміжний клас дла винятків KeyNotFoundException. 

## Ініціалізація колекцій.

При створені змінної об'єкта ви можете визначити його стан вказавши дані властивостей. Аналогічно можна робити з колекціями.

```cs
using System.Drawing;

void InitialiazationCollection()
{
    int[] simpleArray = { 1, 2, 3 };
    PrintCollection(simpleArray);

    List<int> simpleList = new() { 1, 2, 3 };
    PrintCollection(simpleList);

    List<Point> listOfPoint = new()
    {
        new(1,2),
        new(2,3),
        new(3,4)
    };
    PrintCollection(listOfPoint);

    List<Rectangle> listOfRectangle = new()
    {
        new(){Height =90,Width =40, Location = new(1,2) },
        new(){Height =100,Width =50, Location = new(2,4) },

    };
    PrintCollection(listOfRectangle);

    void PrintCollection(ICollection collection)
    {
        Console.WriteLine(collection);
        foreach (var item in collection)
        {
            Console.WriteLine(item);
        }
        Console.WriteLine();
    }
}

InitialiazationCollection();
```
```
System.Int32[]
1
2
3

System.Collections.Generic.List`1[System.Int32]
1
2
3

System.Collections.Generic.List`1[System.Drawing.Point]
{X=1,Y=2}
{X=2,Y=3}
{X=3,Y=4}

System.Collections.Generic.List`1[System.Drawing.Rectangle]
{X=1,Y=2,Width=40,Height=90}
{X=2,Y=4,Width=50,Height=100}
```
Використовувати такий синтаксис можна лжеш для класів яки які підтримують метод Add який формалізований в інтерфейсах ICollection<T>/ICollection. В цьому прикладі поєднується створення об'ектів з створенням колекції. 