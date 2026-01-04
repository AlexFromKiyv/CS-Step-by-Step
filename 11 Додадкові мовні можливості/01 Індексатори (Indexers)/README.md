# Індексатори (Indexers)

Створимо проект Console App Indexers.

Ви, безумовно, знайомі з процесом доступу до окремих елементів, що містяться в простому масиві, за допомогою оператора індексування ([]). Ось приклад:

```cs
static void UsingArray()
{
    string[] args = Environment.GetCommandLineArgs();
    // Loop over incoming command-line arguments
    // using index operator.
    for (int i = 0; i < args.Length; i++)
    {
        Console.WriteLine($"Args: {args[i]}");
    }
    Console.WriteLine();

    // Declare an array of local integers.
    int[] myInts = { 10, 9, 100, 432, 9874 };
    // Use the index operator to access each element.
    for (int j = 0; j < myInts.Length; j++)
    {
        Console.WriteLine($"Index {j}  = {myInts[j]}");
    }
}
UsingArray();
```
```
Args: D:\...\Indexers\Indexers\bin\Debug\net9.0\Indexers.dll

Index 0  = 10
Index 1  = 9
Index 2  = 100
Index 3  = 432
Index 4  = 9874
```
Мова C# надає можливість розробляти власні класи та структури, які можна індексувати так само, як стандартний масив, шляхом визначення методу індексатора. Ця функція найбільш корисна під час створення користувацьких класів колекцій (універсальних або неуніверсальних).

Як налаштувати клас PersonCollection (або будь-який інший користувацький клас чи структуру) для підтримки цієї функціональності?

Додамо до проекту клас:
```cs
namespace Indexers;

public class Person
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }

    public Person(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    public Person()
    {
    }

    public override string? ToString()
    {
        return $"{FirstName}\t{LastName}\t{Age}";
    }
}
```
Індексатор представлений як дещо змінене визначення властивості C#. У найпростішому вигляді індексатор створюється за допомогою синтаксису this[]. Ось клас PersonCollection:

```cs
namespace Indexers;

public class PersonCollection 
{
    private ArrayList arrayOfPerson = new();
    public Person this[int index]
    {
        get => (Person)arrayOfPerson[index];
        set => arrayOfPerson.Insert(index,value);
    }

    public int Count => arrayOfPerson.Count;
}
```
Тоді ви можете використовувати індексатор таким чином таким чином :

```cs
static void UsingPersonCollection()
{
    PersonCollection myPeople = new PersonCollection();
    // Add objects with indexer syntax.
    myPeople[0] = new Person ("Homer", "Simpson", 40);
    myPeople[1] = new Person("Marge", "Simpson", 38);
    myPeople[2] = new Person("Lisa", "Simpson", 9);
    myPeople[3] = new Person("Bart", "Simpson", 7);
    myPeople[4] = new Person("Maggie", "Simpson", 2);

    // Now obtain and display each item using indexer.
    for (int i = 0; i < myPeople.Count; i++)
    {
        Console.WriteLine($"{i}\t{myPeople[i]}");
    }
    Console.WriteLine($"\nCount of people :{myPeople.Count}");
}
UsingPersonCollection();
```
```
0       Homer   Simpson 40
1       Marge   Simpson 38
2       Lisa    Simpson 9
3       Bart    Simpson 7
4       Maggie  Simpson 2

Count of people :5
```
Як бачите, індексатори дозволяють маніпулювати внутрішньою колекцією так само, як і стандартним масивом.

Розглянемо індексатор:

```cs
    public Person this[int index]
    {
        get => (Person)arrayOfPerson[index];
        set => arrayOfPerson.Insert(index,value);
    }
```

Окрім використання ключового слова this з дужками, індексатор виглядає так само, як і будь-яке інше оголошення властивості C#. Наприклад, роль області видимості get полягає в поверненні правильного об'єкта викликаючій функції. Тут ви робите це, делегуючи запит індексатору об'єкта ArrayList, оскільки цей клас також підтримує індексатор. Область видимості set контролює додавання нових об'єктів Person; це досягається шляхом виклику методу Insert() об'єкта ArrayList.
Індексатори – це ще одна форма синтаксичного цукру, враховуючи, що цю функціональність також можна реалізувати за допомогою «звичайних» публічних методів, таких як AddPerson() або GetPerson(). Тим не менш, коли ви підтримуєте методи індексування для ваших типів користувацьких колекцій, вони добре інтегруються в структуру бібліотек базових класів .NET. Хоча створення методів індексатора є досить поширеною практикою під час створення власних колекцій, пам’ятайте, що універсальні типи надають вам цю функціональність «з коробки». Розглянемо наступний метод, який використовує узагальнений List\<T\> об'єктів Person. Зверніть увагу, що ви можете просто використовувати індексатор List\<T\> безпосередньо. Ось приклад:

```cs
static void UseGenericListOfPeople()
{
    List<Person> myPeople = new List<Person>();
    myPeople.Add(new Person("Lisa", "Simpson", 9));
    myPeople.Add(new Person("Bart", "Simpson", 7));

    // Change first person with indexer.
    myPeople[0] = new Person("Maggie", "Simpson", 2);

    // Now obtain and display each item using indexer.
    for (int i = 0; i < myPeople.Count; i++)
    {
        Console.WriteLine($"{i}\t{myPeople[i]}");
    }
}
UseGenericListOfPeople();
```
```
0       Maggie  Simpson 2
1       Bart    Simpson 7
```

## Індексування даних за допомогою рядкових значень

Поточний клас PersonCollection визначив індексатор, який дозволяв викликаючому коду ідентифікувати піделементи за допомогою числового значення. Однак, зрозумійте, що це не є вимогою до методу індексатора. Припустимо, ви бажаєте розмістити об'єкти Person за допомогою System.Collections.Generic.Dictionary\<TKey, TValue\>, а не ArrayList. Враховуючи, що типи словника дозволяють доступ до типів, що містяться в них, за допомогою ключа (наприклад, імені особи), ви можете визначити індексатор наступним чином:

```cs
public class PersonCollectionStringIndexer : IEnumerable
{
    private Dictionary<string, Person> people = new Dictionary<string, Person>();

    // This indexer returns a person based on a string index.
    public Person this[string name]
    {
        get => (Person)people[name];
        set => people[name] = value;
    }

    public int Count => people.Count;

    public IEnumerator GetEnumerator() => people.GetEnumerator();
}
```

Тепер викликаючий код зможе взаємодіяти з об'єктами Person, що містяться в ньому, як показано тут.

```cs
static void UsingPersonCollectionStringIndexer()
{
    PersonCollectionStringIndexer people = new();
    people["Homer"] = new Person("Homer", "Simpson", 40);
    people["Marge"] = new Person("Marge", "Simpson", 38);

    Console.WriteLine(people["Homer"]);

    foreach (var person in people)
    {
        Console.WriteLine(person);
    }

}
UsingPersonCollectionStringIndexer();
```
```
Homer   Simpson 40
[Homer, Homer   Simpson 40]
[Marge, Marge   Simpson 38]
```
Знову ж таки, якби ви використовували узагальнений тип Dictionary\<TKey, TValue\> безпосередньо, ви б отримали функціональність методу індексатора з коробки, без створення власного, неузагальненого класу, що підтримує індексатор рядків. Тим не менш, пам'ятайте, що тип даних будь-якого індексатора буде базуватися на тому, як підтримуючий тип колекції дозволяє викликаючій службі отримувати піделементи.

## Перевантаження методів індексатора

Методи індексатора можуть бути перевантажені в одному класі або структурі. Таким чином, якщо є сенс дозволити викликаючому коду отримувати доступ до піделементів за допомогою числового індексу або рядкового значення, ви можете визначити кілька індексаторів для одного типу. Наприклад, в ADO.NET (нативний API доступу до бази даних .NET), клас DataSet підтримує властивість з назвою Tables, яка повертає вам строго типізований тип DataTableCollection. Як виявляється, DataTableCollection визначає три індексатори для отримання та встановлення об'єктів DataTable — один за порядковим номером, а інші за зручним рядковим монікером та необов'язковим простором імен, як показано тут:

```cs
public sealed class DataTableCollection : InternalDataCollectionBase
{
...
  // Overloaded indexers!
  public DataTable this[int index] { get; }
  public DataTable this[string name] { get; }
  public DataTable this[string name, string tableNamespace] { get; }
}
```
Типи в бібліотеках базових класів зазвичай підтримують методи індексування. Тож майте на увазі, що навіть якщо ваш поточний проект не вимагає створення власних індексаторів для ваших класів та структур, багато типів вже підтримують цей синтаксис.

## Індексатори з кількома вимірами

Ви також можете створити метод індексатора, який приймає кілька параметрів. Припустимо, у вас є власна колекція, яка зберігає піделементи у двовимірному масиві. Якщо це так, ви можете визначити метод індексатора наступним чином:

```cs
public class SomeContainer
{
  private int[,] my2DintArray = new int[10, 10];
  public int this[int row, int column]
  {  /* get or set value from 2D array */  }
}
```
Знову ж таки, якщо ви не створюєте високо стилізований власний клас колекцій, вам не буде особливої ​​потреби створювати багатовимірний індексатор. Тим не менш, ADO.NET знову демонструє, наскільки корисною може бути ця конструкція. ADO.NET DataTable — це, по суті, набір рядків і стовпців, подібно до аркуша міліметрового паперу або загальної структури електронної таблиці Microsoft Excel.
Наведений нижче код ілюструє, як вручну створити в пам'яті DataTable, що містить три стовпці (для імені, прізвища та віку кожного запису). Зверніть увагу, як після додавання одного рядка до DataTable ви використовуєте багатовимірний індексатор для деталізації кожного стовпця першого (і єдиного) рядка.

```cs
static void MultiIndexerWithDataTable()
{
    // Make a simple DataTable with 3 columns.
    DataTable myTable = new DataTable();
    myTable.Columns.Add(new DataColumn("FirstName"));
    myTable.Columns.Add(new DataColumn("LastName"));
    myTable.Columns.Add(new DataColumn("Age"));
    // Now add a row to the table.
    myTable.Rows.Add("Mel", "Appleby", 60);
    // Use multidimension indexer to get details of first row.
    Console.WriteLine($"First Name: {myTable.Rows[0][0]}");
    Console.WriteLine($"Last Name: {myTable.Rows[0][1]}");
    Console.WriteLine($"Age : {myTable.Rows[0][2]}");
}
MultiIndexerWithDataTable();
```
```
First Name: Mel
Last Name: Appleby
Age : 60
```
Головна суть цього прикладу полягає в тому, що методи індексатора можуть підтримувати кілька вимірів і, за умови правильного використання, можуть спростити спосіб взаємодії з подоб'єктами, що містяться в користувацьких колекціях.

## Визначення індексаторів для типів інтерфейсів

Визначення індексаторів для типів інтерфейсів

Індексатори можна визначити для заданого типу інтерфейсу .NET, щоб дозволити підтримуючим типам забезпечити власну реалізацію. Ось простий приклад інтерфейсу, який визначає протокол для отримання рядкових об'єктів за допомогою числового індексатора:

```cs
public interface IStringContainer
{
  string this[int index] { get; set; }
}
```
З цим визначенням інтерфейсу будь-який клас або структура, що реалізує цей інтерфейс, тепер повинні підтримувати індексатор читання-запису, який маніпулює піделементами за допомогою числового значення. Ось часткова реалізація такого класу:

```cs
class SomeClass : IStringContainer
{
  private List<string> myStrings = new List<string>();
  public string this[int index]
  {
    get => myStrings[index];
    set => myStrings.Insert(index, value);
  }
}
```
Класи які мають індексатори не обов'язково повини мати в собі коллекцію. Вони можуть реалізовувати необхідну логіку в залежності від стану об'єкта.