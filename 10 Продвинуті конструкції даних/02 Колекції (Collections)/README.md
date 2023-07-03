# Колекції. Collections

Будь-які програми створені на .Net підтримують і оброблають набори данних в пам'яті. Ці дані можуть надходит з БД, текстового файлу, XML документу, виклику веб-служби, датчиків, вноситись користувачем. Для роботи з колекціями є простіри імен System.Collection, System.Collection.Generic.
Найпростіший контейнер для набору даних є массив. Він дозволяє визначити набор однотипних данних з обмеженою верхньою межою. System.Array надає массивам фунціональності.   

```cs
void UsingArray()
{
    string[] strings = { "First", "Second", "Third" };

    ArrayToConsole(strings);

    Array.Reverse(strings);

    ArrayToConsole(strings);


    void ArrayToConsole(string[] array)
    {
        Console.WriteLine();
        Console.WriteLine($"Type:{array}"  );
        Console.WriteLine($"Length:{array.Length}");
        foreach (string item in array)
        {
            Console.WriteLine("\t"+item);
        }
    }
}

UsingArray();
```
```
Type:System.String[]
Length:3
        First
        Second
        Third

Type:System.String[]
Length:3
        Third
        Second
        First
```
Массиви корисні коли треба маніпулювати визначенним, невеликим надором даних фіксованого розміру. Але багато реальних задачах потребують більш гнучких структур данних. Наприклад контейнер який дінамічно зростає чи зменьшується. Або контейнер який може зберігати лише об'єкти які реалізують необхідний інтерфейс чи мають єдиний базовий клас. 
Простий масив створюється з фіксованим розміром.
```cs
void ArrayHasFixedSize()
{
    string[] strings = { "First", "Second", "Third" };

    strings[4] ="1"; 
    //System.IndexOutOfRangeException:Index was outside the bounds of the array.
    strings[5] ="2";

    Console.WriteLine(strings[5]);
}

ArrayHasFixedSize();
```  
```
Unhandled exception. System.IndexOutOfRangeException: Index was outside the bounds of the array.
```
Аби подолати ці обмеженя бібліотеки .Net мають кілька просторів імен що містять класи колекцій. На відміну від массивів класи колекцій створені для динамічної зміни розмірів на льоту, коли додаються додаються чи прибираються елементи. Крім того ці класи забезпечубть підвищену безпеку типів і оптимізовані для обробки данних з єффективним використанням пам'яті.
Колекції можна розділити на дві категорії:

- Nongeneric (неузагальнені) (System.Collections)
- Generic (узагальнені) (System.Collections.Generic)

Неузагальнені колекції проектувались для роботи з типами System.Object і є вілно типізованим контейнером (за винятком деяких які працюють зрядками).
Узагальнені колекції потребують встановленя типу єлементів під час створення.

## Nongeneric. System.Collections. System.Collections.Specialized 

Функціональність класів коллекцій визначаеться в інтерфейсах.

    ICollaction : Визначає загальні характеристики (розмір, прохід єлементів та безпеку потоків). 

    IClonable: Дозволяє об'єкту реалізації повертати свою копію.

    IDictonary: Дозволяє неузагальненому об'єкту колекції представляти свій вміст за допомогою пари ключ-значення.

    IEnumerable: Повертає об'єкт який реалізовує інтерфейс IEnumerator.

    IEnumerator: Включає можливість пребирати колекцію в стилі foreach.

    IList: Дозволяє додавати, видаляти та індексувати єлементи у послідовному списку об'єктів.

Корисні класи System.Collections для роботи з колекціями реалізовують ці інтерфейси.

ArrayList : ICollection, IEnumerable, IList, ICloneable : Представляє коллекцію об'єктів динамічного розміру перелічені в послідовному порядку.

BitArray : ICollection, IEnumerable, ICloneable : Керує компактним масивом бітових значень які використовуються як логічні одиниці.

Hashtable : IDictionary, ISerializable, IDeserializationCallback, ICloneable : Представляє набір пар ключ-значення, організованих на основі хеш-коду ключа.

Queue : ICollection, ICloneable : Представляє стандартну чергу об'єктів (first-in, first-out).

Stack : ICollection, ICloneable : Представляє стандартний стек (last-in, first-out) шо забезпечує функціональність puch,pop,peek.

SortedList : IDictionary, ICloneable : Представляє колекцію пар ключ-значення відсортовану по ключам та доступними за ключами та індексами.

```cs
void UsingArrayList()
{
    ArrayList arrayList = new() {"First","Second","Third","4","5"};
    arrayList.Add(4);
    arrayList.Add(5);
    arrayList.Remove("4");
    arrayList.RemoveAt(3);
    ArrayListToConsole(arrayList);

    Console.WriteLine();
    arrayList.Clear();

    string[] stringArray = { "First", "Second", "Third", "4", "5" };
    arrayList.AddRange(stringArray);
    arrayList.Add(6);
    arrayList.AddRange(new string[] { "7", "Julia"});
    DateTime dateTime = DateTime.Now;
    arrayList.Add(dateTime.DayOfWeek);

    ArrayListToConsole(arrayList);

    arrayList.RemoveRange(0, 5);

    ArrayListToConsole(arrayList);


    void ArrayListToConsole(ArrayList arrayList)
    {
        Console.WriteLine($"Count:{arrayList.Count}");
        Console.WriteLine($"Capacity:{arrayList.Capacity}");
        foreach (var item in arrayList)
        {
            ObjectToConsole(item);
        }
    }

    void ObjectToConsole(object obj)
    {
        Console.WriteLine($"\t{obj}\t{obj.GetType()}");
    }
}

UsingArrayList();
```
```
Count:5
Capacity:8
        First   System.String
        Second  System.String
        Third   System.String
        4       System.Int32
        5       System.Int32

Count:9
Capacity:16
        First   System.String
        Second  System.String
        Third   System.String
        4       System.String
        5       System.String
        6       System.Int32
        7       System.String
        Julia   System.String
        Wednesday       System.DayOfWeek
Count:4
Capacity:16
        6       System.Int32
        7       System.String
        Julia   System.String
        Wednesday       System.DayOfWeek
```
Клас ArrayList предоставляє вам контейнер в якій можна додавати, видаляти об'єкти і цей контейнер автоматично змінюється від потреб. Аналогічно за допомогою документації можна дослідити Stack, Queue, т.д.
В реальних проетах набагаточастіше використовуються узагальнені аналоги ціх класів з простори імен System.Collections.Generic. 

В класі System.Collections.Specialized також визначені класи колекцій.

ListDictionary : IDictionary : Цей клас корисний коли потрібно керувати невеликою кількісьтю єлементів (біля 10), які можуть змінюватися з часом.

HybridDictionary : ICollection, IEnumerable, IDictionary : Цей клас реалізує IDictionary за допомогою ListDictionary, поки колекція мала, а потім перемикається на Hashtable, коли колекція стає великою.

StringCollection : IList : Цей клас забезпечує оптимальний спосіб керування великими колекціями рядкових даних.

BitVector32 : IEquatable<BitVector32> : Цей клас забезпечує просту структуру, яка зберігає логічні значення та малі цілі числа в 32 бітах пам’яті.

Всі члени класів колекцій можна побачити в Visual Studio > View > Object Browser.

У бібліотеках базових класів .NET Core є два додаткових простору імен, орієнтованих на колекції (System.Collections.ObjectModel і System.Collections.Concurrent)

З розвятком виявилась деякі проблеми з використанням неузагальнених типів. 
Перша з проблем оскількі нетипізовани типи працюють з System.Object класи колекцій можуть показувати погану продуктивність коду з єлементами value type наприклад з числами.
Друга проблема більшість незагальних типів не є типобезпечними оскільки System.Object може містити будьшо. Тому для створення безпечного класу колекції для певного типу треба було робити це вручну. 

### Проблема продуктивності.

В пам'яті дані можуть зберігатись в value та reference типі. 

```cs
void BoxingAndUnboxing()
{
    int valueTypeVariable = 10;
    Console.WriteLine(valueTypeVariable.GetType());

    //boxing
    object referenceToValueTypeVariable = valueTypeVariable;
    Console.WriteLine(referenceToValueTypeVariable.GetType());
    
    //unboxing
    int unboxedInt = (int)referenceToValueTypeVariable;
    Console.WriteLine(unboxedInt.GetType());

    try
    {
        //do not work
        long unboxingLong = (long)referenceToValueTypeVariable; 
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

BoxingAndUnboxing();
```
```
System.Int32
System.Int32
System.Int32
Unable to cast object of type 'System.Int32' to type 'System.Int64'.
```
Boxing, або упакування, означеє шо коли рядок де змінній System.Object присваюється значення типу value, відбуваеться створення в heap об'єкта та копіювання значення з стеку та потім повертаеться посилання. Unboxing, або розпакування, це процес на базі посилання на об'єкт перевтореня об'єкта в heag в відпоідний тип значення в стеку. Цей процес починаеться з перевірки відповідності типу в стеку і типу об'єкту в heap. Для цілкової безпеки процес приведеня з System.Object в інший тип повине бути загорнутиі в блок try/catch і досить трудоміске для кожної операції перетворення. 

Хоча зберігання в типі object не часто потрібне треба визначити шо процес упаковки розпакови досить корисни і про розміщеня в пам'яті виконує середа виконання від вашого імені.  

Члени класу ArrayList які працюють та використовують тип System.Оbject.
```cs
public class ArrayList : IList, ICloneable
{
...
  public virtual int Add(object? value);
  public virtual void Insert(int index, object? value);
  public virtual void Remove(object? obj);
  public virtual object? this[int index] {get; set; }
}
```
Використання техніки box/unbox.
```cs
void UsingSystemObjectInArrayList()
{
    //boxing
    ArrayList myInts = new();
    myInts.Add(10);
    myInts.Add(20);
    myInts.Add(30);

    //unboxing
    int? i = (int?) myInts[0];

    Console.WriteLine(i);
}

UsingSystemObjectInArrayList();
```
Незважаючи на те шо в метод Add передаються числа середовище запаковує і розміщує їх в heap. Зворотній процес потребує приведення і компілятор це покаже. Хоч такий код легко пишеться але процеси упаковки розпаковки зменшують продуктивність:
    1. Треба розмішувати об'єкт в керовану купу.
    2. Значеня даних треба копіювати з стеку в об'єкт.
    3. При розпакуванні значеня з купи потрібно вертати в стек.
    4. Об'єкт в купі якийсь час залишається до збирання сміття.

Все це наргужає середовише виконнаня і відчуваеться якшо треба маніпулювати тисячами об'єктів.

### Проблема безпеки типів.

Треба враховувати в який тип іде розпакування. Він повинен співпадати з тим який був в процесі упаковки. 
```cs
void NoSafetyUsingArrayList()
{
    ArrayList myArray = new();

    myArray.Add(1);
    myArray.Add(true);
    myArray.Add(new OperatingSystem(PlatformID.Unix,new Version()));
    myArray.Add(new DateTime());

    foreach (int item in myArray)
    {
        int i = item;
    }
}

NoSafetyUsingArrayList();
```
В деяких випадках потрібен надгнучкий контейнер, якій може містити будь-шо. Але як бачите це небезпечно.
В більшості випадків потрібен типобеспечний контейнер якій працює з певним типом.  
Спробуємо створити такий контейнер власноруч.
Клас об'єктів для колекції.
Person.cs
```cs
 public class Person
    {
        public int Age { get; set; }
        public string FirstName { get; set; } = "Undefined";
        public string LastName { get; set; } = "Undefined"; 
        public Person() { }
        public Person(int age, string firstName, string lastName)
        {
            Age = age;
            FirstName = firstName;
            LastName = lastName;
        }

        public override string? ToString()
        {
            string fullNameAge = $"{FirstName} {LastName} {Age}";
            return fullNameAge+"\t"+base.ToString();
        }
    }
```
Простий варіант власно-створенного класу для колекцій.

```cs
    public class PersonCollectiom : IEnumerable
    {
        private ArrayList arrayPeople = new();

        public void Add(Person person)
        {
            arrayPeople.Add(person);
        }

        public void Clear() 
        {
            arrayPeople.Clear();
        }

        public int Count => arrayPeople.Count;

        public Person GetPerson(int index) => (Person)arrayPeople[index]!;

        public IEnumerator GetEnumerator() => arrayPeople.GetEnumerator();
    }
```
Клас реалізовує інтерфейс IEnumerable шо дозволяє предбирати єлементи об'єкта за допомогою foreach. Також зверніть увагу шо методи Add, GetPerson протипізовані шоб працювати з типом Person, і не дозволить працювати з будьчим. Це додає впевненості в безпеці типів. 
Використання.
```cs
void UsePersonCollection()
{
    
    PersonCollectiom personages = new PersonCollectiom();
    personages.Add(new("Lara", "Croft", 50));
    personages.Add(new("Slerlock", "Holmes", 40));
    personages.Add(new("Sara", "Connor", 35));
    personages.Add(new("Tony", "Stark", 40));
    personages.Add(new("Захар", "Беркут", 40));

    // personages.Add((new DateTime()); // cannot convert System.DateTime to Collections.Person

    foreach (Person itemPerson in personages)
    {
        Console.WriteLine(itemPerson);
    }
}

UsePersonCollection();
```   
```
Lara Croft 50   Collections.Person
Slerlock Holmes 40      Collections.Person
Sara Connor 35  Collections.Person
Tony Stark 40   Collections.Person
Захар Беркут 40 Collections.Person
```
Хоча спеціальний клас колекції забеспечує безпеку типів, цей підхід потребує створення майже однакову користувацьку колекцію для кожного унікального типу. Наприклад зробимо кастомний колекцій автомобілів.
```cs
    public class Car
    {
        public string Manufacturer { get; set; } = "Undefined";
        public string Model { get; set; } = "Undefined";
        public int Year { get; set; }
    }

    public class CarCollection : IEnumerable
    {
        private ArrayList arrayCars = new();
        public void Add( Car car ) => arrayCars.Add(car);
        public Car? Get(int index) => (Car?)arrayCars[index];
        public int Count => arrayCars.Count;
        public void Clear() => arrayCars.Clear();
        public IEnumerator GetEnumerator() => arrayCars.GetEnumerator();
    }
```
Спеціальний клас колекцій не рішає проблеми продуктивності підчас упаковування/розпаковування. В любому випадку потрібне зберігання в купі.

## Перший погляд на узагальнення (generic).

При використанні загальних класів усувається проеблема продуктивності при упаковувані/розпаковувані, а також проблема. Замість створення спеціальних класів з повторенням тіх самих методів можна використовувати узагальнений тип коллекції якому вказати тип об'єктів зберігання.

```cs
void UseGenericList()
{
    List<Person> personages = new();
    personages.Add(new("Lara", "Croft", 50));
    personages.Add(new("Slerlock", "Holmes", 40));
    personages.Add(new("Sara", "Connor", 35));
    personages.Add(new("Tony", "Stark", 40));
    personages.Add(new("Захар", "Беркут", 40));

    // personages.Add((new DateTime()); // cannot convert System.DateTime to Collections.Person

    Console.WriteLine(personages[1]);

    Console.WriteLine();

    List<int> ints = new();

    ints.Add(10);
    ints.Add(20);
    ints.Add(30);

    Console.WriteLine(ints[1] + ints[2]);
}

UseGenericList();
```
```
Slerlock Holmes 40      Collections.Person

50
```
Як видно з прикладу не створюючи спеціального класу створено колекцію з потрібним типом.
Об'єктр List<Person> personages може містити лише об'єкти Person тому нетреба робити приведеня. При роботі з List<int> ints немає прихованого упакування/розпакування. 
Таким чином, узагальнення покращують продуктивність, забезпечують білшу захищеність типів та не потребують створення окремих класів для конкретних типів.
