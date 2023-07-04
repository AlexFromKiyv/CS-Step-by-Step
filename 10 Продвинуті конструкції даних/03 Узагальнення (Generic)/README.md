# Узагальнення (Generic)

Узагальненими можуть бути класи, інтерфейси, структури і делегати. Узагальнений тип виглядає з кутавими дужками і символом.(наприклад List<T>). 
Приклади узагальнень можна побачити Visual Studio > View > Object Browser і в пошуку ввести System.Collections.Generic. 
Літера Т це параметр типу або заповнювачи. Напис IEnumerable<T> можна прочитати 
"IEnumerable of type T". T зазвичай використовується для типів, TKey для ключів, TValue для значень.
Коли вистворюєте похідну загального типу, реалізовуете загальний інтерфейс або викликаете загал ьний член треба вказати значеня параметру в вигляді типу. 

### Визначення параметрів узагальнених класів/структур.

```cs
void SpecifyingGenericParameters()
{
    List<Car> cars = new();
    cars.Add(new Car("VW", "Beetle", 2020));
    cars.Add(new Car("VW", "E-Buster", 2025));

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

### Використаня загальних членів класу






