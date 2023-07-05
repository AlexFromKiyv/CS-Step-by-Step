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
        new Car("VW", "Beetle", 2020), 
        new Car("VW", "E-Buster", 2025) 
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

Розглянемо інтерфейс IComparable
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





