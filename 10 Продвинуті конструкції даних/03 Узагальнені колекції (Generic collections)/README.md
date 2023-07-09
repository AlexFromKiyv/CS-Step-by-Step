# Узагальнені колекції (Generic collections)

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

## Робота з List\<T>.

Створимо допоміжні методи відобаження колекції на консоль.
```cs
void CollectionToConsole(ICollection collection)
{
    Console.WriteLine(collection);
    Console.WriteLine($"Count:{collection.Count}\n");
    // Enumerate over collection ICollection : IEnumerable
    int index = 0;
    foreach (var item in collection)
    {
        Console.Write($"\t{index}.\t");
        Console.WriteLine(item);
        index++;
    }
}

void ListToConsole<T>(List<T> list)
{
    Console.WriteLine();
    Console.WriteLine(list);
    Console.WriteLine($"Count:{list.Count}");
    Console.WriteLine($"Capacity:{list.Capacity}\n"  );
    // Enumerate over collection ICollection : IEnumerable
    int index = 0;
    foreach (var item in list)
    {
        Console.Write($"\t{index}.\t");
        Console.WriteLine(item);
        index++;
    }
}
```
Дослідимо List<Person>.
```cs
void UseGenericList()
{
    // Make a List of personages
    List<Person> personages = new()
    {
        new("Tomy","Stark",40),
        new("Sara","Connor",30),
        new("Sherlock","Holms",50),
    };
    // Print out
    ListToConsole(personages);

    //Add
    Person rembo = new("John", "Rembo", 30);
    personages.Add(rembo);

    // Insert new item
    Person bond = new("James", "Bond", 40);
    personages.Insert(2,bond);

    ListToConsole(personages);

    //Remove
    personages.Remove(bond);
    personages.Remove(rembo);

    ListToConsole(personages);

    // To array 
    Person[] arrayPersonages = personages.ToArray();

    //Array : ICollection
    CollectionToConsole(arrayPersonages);

    
}
UseGenericList();
```
```

System.Collections.Generic.List`1[GenericCollections.Person]
Count:3
Capacity:4

        0.      Stark Tomy 40   GenericCollections.Person
        1.      Connor Sara 30  GenericCollections.Person
        2.      Holms Sherlock 50       GenericCollections.Person

System.Collections.Generic.List`1[GenericCollections.Person]
Count:5
Capacity:8

        0.      Stark Tomy 40   GenericCollections.Person
        1.      Connor Sara 30  GenericCollections.Person
        2.      Bond James 40   GenericCollections.Person
        3.      Holms Sherlock 50       GenericCollections.Person
        4.      Rembo John 30   GenericCollections.Person

System.Collections.Generic.List`1[GenericCollections.Person]
Count:3
Capacity:8

        0.      Stark Tomy 40   GenericCollections.Person
        1.      Connor Sara 30  GenericCollections.Person
        2.      Holms Sherlock 50       GenericCollections.Person
GenericCollections.Person[]
Count:3

        0.      Stark Tomy 40   GenericCollections.Person
        1.      Connor Sara 30  GenericCollections.Person
        2.      Holms Sherlock 50       GenericCollections.Person
```
List<T> часто використовуваний клас який дозволяє динамічно змінювати розмір контейнера. В прикладі використовується синтаксис ініціалізації, хоча можна було використовувати метод Add декілька разів. За допомогою вказуваня індексу об'єкт додається в середину списку. Метод ToArray повертає массив об'єктів на основі вмісту списку.
Клас List<T> має багато інших методів для роботи з списком. 

## Робота з Stack\<T>.

Клас Stack\<T> репрезентую колекцію яка зберігає елеиенти за принципом "last-in, first-out".
```cs
void UseGenericStack()
{
    Stack<Person> personages = new();

    personages.Push(new("Tomy", "Stark", 40));
    CollectionToConsole(personages);

    personages.Push(new("Sara", "Connor", 30));
    CollectionToConsole(personages);

    personages.Push(new("John", "Rembo", 30));
    CollectionToConsole(personages);


    Person person = personages.Peek();
    Console.WriteLine($"\n{person}\n");
    CollectionToConsole(personages);

    person = personages.Pop();
    Console.WriteLine($"\n{person}\n");
    CollectionToConsole(personages);

    Console.WriteLine($"\n{personages.Pop()}\n");
    Console.WriteLine($"\n{personages.Pop()}\n");
    Console.WriteLine($"\n{personages.Pop()}");
}

UseGenericStack();
```
```
System.Collections.Generic.Stack`1[GenericCollections.Person]
Count:1

        0.      Stark Tomy 40   GenericCollections.Person
System.Collections.Generic.Stack`1[GenericCollections.Person]
Count:2

        0.      Connor Sara 30  GenericCollections.Person
        1.      Stark Tomy 40   GenericCollections.Person
System.Collections.Generic.Stack`1[GenericCollections.Person]
Count:3

        0.      Rembo John 30   GenericCollections.Person
        1.      Connor Sara 30  GenericCollections.Person
        2.      Stark Tomy 40   GenericCollections.Person

Rembo John 30   GenericCollections.Person

System.Collections.Generic.Stack`1[GenericCollections.Person]
Count:3

        0.      Rembo John 30   GenericCollections.Person
        1.      Connor Sara 30  GenericCollections.Person
        2.      Stark Tomy 40   GenericCollections.Person

Rembo John 30   GenericCollections.Person

System.Collections.Generic.Stack`1[GenericCollections.Person]
Count:2

        0.      Connor Sara 30  GenericCollections.Person
        1.      Stark Tomy 40   GenericCollections.Person

Connor Sara 30  GenericCollections.Person


Stark Tomy 40   GenericCollections.Person

Unhandled exception. System.InvalidOperationException: Stack empty.
```

Клас визначає члени Push, Peek, Pop для розміщеня, отриманя та отриманя та вилучення. Метод Peek заглядає в стек і дачить елемент який додао останнім. Як видно з прикладу при використанням методу Pop може виникнути виняток.
```cs
void UseGenericStackWithCheck()
{
    Person[] persons = 
    {
        new("Tomy", "Stark", 40),
        new("Sara", "Connor", 30),
        new("John", "Rembo", 30)
    };

    Stack<Person> personages = new(persons);
    CollectionToConsole(personages);

    Console.WriteLine();

    while (personages.TryPop(out Person? person))
    {
        Console.WriteLine(person);
    }
}

UseGenericStackWithCheck();
```
```
System.Collections.Generic.Stack`1[GenericCollections.Person]
Count:3

        0.      Rembo John 30   GenericCollections.Person
        1.      Connor Sara 30  GenericCollections.Person
        2.      Stark Tomy 40   GenericCollections.Person

Rembo John 30   GenericCollections.Person
Connor Sara 30  GenericCollections.Person
Stark Tomy 40   GenericCollections.Person
```

## Робота з Queue\<T>.

Черги (Queue) це контейнери яки забезпечують зберіганя єлементів за принципом "first-in, first-out". 

```cs
void UseGenericQueue()
{
    Queue<Person> sitizens = new();

    // queuing
    sitizens.Enqueue(new("Julia", "Firstenko", 25));
    sitizens.Enqueue(new("Evgeniy", "Secandenko", 28));
    sitizens.Enqueue(new("Nikolaj","Thirdenko",42));
    sitizens.Enqueue(new("Pavel", "Fourtinenko", 32));

    CollectionToConsole(sitizens);

    // peek first 
    Person person = sitizens.Peek();
    Console.WriteLine($"\nWork Peek: {person}\n"  );

    CollectionToConsole(sitizens);
    
    // dequing
    person = sitizens.Dequeue();
    Console.WriteLine($"\nWork Dequeue: {person}\n");

    CollectionToConsole(sitizens);
    
    Console.WriteLine();

    while (sitizens.TryDequeue(out Person? sitizen))
    {
        Console.WriteLine(sitizen);
    }

}

UseGenericQueue();
```
```
System.Collections.Generic.Queue`1[GenericCollections.Person]
Count:4

        0.      Firstenko Julia 25      GenericCollections.Person
        1.      Secandenko Evgeniy 28   GenericCollections.Person
        2.      Thirdenko Nikolaj 42    GenericCollections.Person
        3.      Fourtinenko Pavel 32    GenericCollections.Person

Work Peek: Firstenko Julia 25   GenericCollections.Person

System.Collections.Generic.Queue`1[GenericCollections.Person]
Count:4

        0.      Firstenko Julia 25      GenericCollections.Person
        1.      Secandenko Evgeniy 28   GenericCollections.Person
        2.      Thirdenko Nikolaj 42    GenericCollections.Person
        3.      Fourtinenko Pavel 32    GenericCollections.Person

Work Dequeue: Firstenko Julia 25        GenericCollections.Person

System.Collections.Generic.Queue`1[GenericCollections.Person]
Count:3

        0.      Secandenko Evgeniy 28   GenericCollections.Person
        1.      Thirdenko Nikolaj 42    GenericCollections.Person
        2.      Fourtinenko Pavel 32    GenericCollections.Person

Secandenko Evgeniy 28   GenericCollections.Person
Thirdenko Nikolaj 42    GenericCollections.Person
Fourtinenko Pavel 32    GenericCollections.Person
```
Особливі методи цого клаус дозволяють поставити елемент в чергу, подивитись чія черга настала та зняти з черги елемент чия черга настала. Яшо черга пуста и викликати метод Dequeue виникне виняток.