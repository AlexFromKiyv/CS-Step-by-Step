# Індексатори (Indexers)

У массиві доступ до елементів дуже простий.
```cs
void WorkWithArray()
{
    string[] girls = { "Vera", "Olga", "Viktory" };

    girls[1] = "Ira";

    Console.WriteLine(girls[2]); 
    Console.WriteLine();

    for (int i = 0; i < girls.Length; i++)
    {
        Console.WriteLine($"{i}. {girls[i]}");
    }

    //Index was outside the bounds of the array.
    //girls[4] = "Nikita";
}

WorkWithArray();
```
```
Viktory

0. Vera
1. Ira
2. Viktory

```
Треба зауважити шо в С# массиви передбачають певну незмінну кількість елементів. На протилежність від інших мов програмування не можливо додати елемент з індексом якого не було при створені масиву.

## Створення.

За для того аби можно було за допомогою індеска маніпулювати елементами в складних типах треба визначити метод індексатора. Особливо це корисно коли створюеться ваша, спеціальна коллекція.
Визначення індексатора схоже на визначення властивості.

```cs
    class Person
    {
        public string Name { get; set; } 
        public Person(string name = "")
        {
            Name = name;
        }
        public override string? ToString()
        {
            return $"{Name}\t"+base.ToString();
        }
    }


    class PersonCollection_v1
    {
        private Person[] arrayOfPerson = new Person[100];
        public int Count => arrayOfPerson.Length;

        //Indexer
        public Person this[int index] 
        { 
            get => arrayOfPerson[index];
            set => arrayOfPerson[index] = value;
        }
    }
```
```cs
void UseSimpleIndexer()
{
    PersonCollection_v1 people = new();

    people[0] = new Person("John");
    people[0] = new Person("John");
    people[1] = new Person("Sara");

    people[2] = people[1];
    people[1] = new Person("Tony");

    for (int i = 0; i < 5; i++)
    {
        Console.WriteLine(people[i]);
    }

    //people[101] = new Person("");
}

UseSimpleIndexer();
```
```
John    Indexers.Person
Tony    Indexers.Person
Sara    Indexers.Person



```
Коли ваш клас колекції має індексатор цей клас краще інтегрується з бібліотеками базових класів .Net . 
Але такий класс колекції складно назвати добрим оскільки кожного разу створюється массив new Person[100];
Пам'ятайте синтаксис масиву передбачає шо індекси масиву визначаються при його створені і додавання елемент з новим індексом викликає виняток. В деяких мовах програмування масиви можуть розширюватись динамічно.
Використовувати неузагальнений клас аби створювати колекцію з індексаторм не дуже зручно і еффективно. 

## Узагальнені колекції мають індексатор.

Краше використовувати узагальнені коллекції. Фунціональність індексатора з цілими індексами вбудована в них.
```cs
void UseGenericTypeIndexer()
{
    List<Person> people = new()
    {
        new("Tony"),
        new("Sara"),
        new("Sherlock"),
    };


    people[2] = people[0];

    people[0] = new Person("Jhon");

    for (int i = 0; i < people.Count; i++)
    {
        Console.WriteLine(people[i]);
    }

    // It don't insert item.
    //people[3] = new("Someone");
}

UseGenericTypeIndexer();
```
```
Jhon    Indexers.Person
Sara    Indexers.Person
Tony    Indexers.Person

```

## Рядкові індекси.

Індексатор може мати індекси рядкового типу.
```cs
class PersonCollectionWithStringIndex : IEnumerable
    {
        private Dictionary<string, Person> dictionaryOfPerson = new();

        public Person this[string index]
        {
            get => dictionaryOfPerson[index];
            set => dictionaryOfPerson[index] = value;
        }

        public int Count => dictionaryOfPerson.Count;
        public void Clear()
        {
            dictionaryOfPerson.Clear();
        }
        IEnumerator IEnumerable.GetEnumerator() => dictionaryOfPerson.GetEnumerator();
    }
```
```cs
void UseStringIndexer()
{
    PersonCollectionWithStringIndex personage = new();

    personage["John"] = new("John Connor");
    personage["Terminator"] = new("T-800");
    personage["IronMan"] = new("Tony Stark");

    Console.WriteLine(personage["Terminator"]);
    Console.WriteLine();

    personage["Terminator"] = new("T-1000");

    foreach (KeyValuePair<string, Person> item in personage)
    {
        Console.WriteLine($"Index:{item.Key}\tItem:{item.Value}");
    }
}

UseStringIndexer();
```
```
T-800   Indexers.Person

Index:John      Item:John Connor        Indexers.Person
Index:Terminator        Item:T-1000     Indexers.Person
Index:IronMan   Item:Tony Stark Indexers.Person
```
В цьому прикладі викорисовується можливості Dictionary<TKey, TValue> по доступу через індекс. В цьому варіанті є можливість вставки та оновлення елементів в колекцію не зважаючи на розміру та значення індексу. 

## Перезавантаження індексаторів.

В класі або структурі можна мати декілька індексаторів з різними параметрами. Таким чином можна реалізувати доступ ло елементів як через числові так і через рядкові індекси. 

Приклад індексатора з ADO.NET.
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
Тут об'єкт можна отриматни за порядковим номером таблиці або назві таблиці.
В бібліотеках базових класів зазвичай типи підтримують індексатори. Тому навіть якщо ваш проект не потребує індексатора багато типив мають їх. 

## Багатовимірні індексатори.

Так само як масиви можкть бути двовимірні, множину об'єктів можна розмітити в матриці.

```cs
public class SomeContainer
{
  private int[,] myMatrix = new int[10, 10];
  public int this[int row, int column]
  {  /* get or set value */  }
}
```
В ADO.Net багатовимірний індексатор використовується для отриманя даних з таблиці.
```cs
static void MultiIndexerWithDataTable()
{
  // Make a simple DataTable with 3 columns.
  DataTable myTable = new DataTable();
  myTable.Columns.Add(new DataColumn('FirstName'));
  myTable.Columns.Add(new DataColumn('LastName'));
  myTable.Columns.Add(new DataColumn('Age'));
  // Now add a row to the table.
  myTable.Rows.Add('Mel', 'Appleby', 60);
  // Use multidimension indexer to get details of first row.
  Console.WriteLine('First Name: {0}', myTable.Rows[0][0]);
  Console.WriteLine('Last Name: {0}', myTable.Rows[0][1]);
  Console.WriteLine('Age : {0}', myTable.Rows[0][2]);
}
```
Багатовимірні індексатори створюються якщо вних є потреба і як видно їх використання досить просте. Тому індексатор і реалізован.

## Індексатори в інтерфейсах.

