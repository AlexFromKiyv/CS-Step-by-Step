# IComparable

Інтерфейс System.IComparable визначає поведінку яка дозволяє сортувати об'єкти на основі певного вказаного ключа.

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
Як видно із опису інтерфейс передбачає можливість об'єкта порівнятись з іншим і визначити своє положеня до нього а конктретно передує , позаду чи натому самому місці.

Класс System.Array визначає статичний метод Sort. Коли массив складається з внутрішніх типів (int,short,string ...) Sort викликаеться без проблем, тому що всі ці типи реалізовують інтерфейс IComparable. Але якшо ви захочите застосувати цей метод до массиву заповненого вашим типом то може виникнути виняток який вкаже на те шо треба реалізувати інтерфейс IComparable.

UseIComparable\Types_v1.cs

```cs
    class Car
    {
        public int Id { get; set; } 
        public string Name { get; set; } = "";
        public int MaxSpeed { get; }

        public Car(int id, string name, int maxSpeed)
        {
            Id = id;
            Name = name;
            MaxSpeed = maxSpeed;
        }
        public Car() {}

        public override string? ToString() => $"{Id}\t{Name}\tMax.Spped:{MaxSpeed}";

    }
```
```cs
ExplorationArraySort_1();
void ExplorationArraySort_1()
{
    Car[] cars = new Car[5];

    cars[0] = new Car(1, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car(3, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car(4, "VOLKSWAGEN Beetle 2011-2016", 180);
    cars[4] = new Car(5, "VOLKSWAGEN Beetle 2016-2019", 180);

    Array.Sort(cars);

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }
}

```
```
Unhandled exception. System.InvalidOperationException: Failed to compare two elements in the array.
 ---> System.ArgumentException: At least one object must implement IComparable.
   at System.Collections.Comparer.Compare(Object a, Object b)
   at System.Collections.Generic.ArraySortHelper`1.InsertionSort(Span`1 keys, Comparison`1 comparer)
   at System.Collections.Generic.ArraySortHelper`1.IntroSort(Span`1 keys, Int32 depthLimit, Comparison`1 comparer)
   at System.Collections.Generic.ArraySortHelper`1.IntrospectiveSort(Span`1 keys, Comparison`1 comparer)
   at System.Collections.Generic.ArraySortHelper`1.Sort(Span`1 keys, IComparer`1 comparer)
   --- End of inner exception stack trace ---
```

Отже аби була можливість використати Array.Sort в класі треба реалізувати IComparable.Коли ви конкретизуєте деталі CompareTo(), ви вирішуєте, якою буде базова лінія операції впорядкування. 

```cs
    class Car_v1 : Car, IComparable
    {
        public Car_v1() {}
        public Car_v1(int id, string name, int maxSpeed) : base(id, name, maxSpeed)
        {
        }

        public int CompareTo(object? obj)
        {
            if (obj is Car_v1 otherCar)
            {
                if (Id > otherCar.Id)
                {
                    return 1;
                }

                if (Id < otherCar.Id)
                {
                    return -1;
                }

                return 0;
            }
            else
            {
                throw new ArgumentException("The parameter is not a compatible type.");
            }
        }
    }

```
```cs
ExplorationArraySort_2();
void ExplorationArraySort_2()
{
    Car_v1[] cars = new Car_v1[5];

    cars[0] = new Car_v1(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v1(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v1(1, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v1(4, "VOLKSWAGEN Beetle 2011-2016", 180);
    cars[4] = new Car_v1(5, "VOLKSWAGEN Beetle 2016-2019", 180);

    Array.Sort(cars);

    foreach (Car_v1 car in cars)
    {
        Console.WriteLine(car);
    }
}
```
```cs
1       VOLKSWAGEN Beetle 1998-2005     Max.Spped:161
2       VOLKSWAGEN Beetle 2001-2002     Max.Spped:225
3       VOLKSWAGEN Beetle 1945-2003     Max.Spped:100
4       VOLKSWAGEN Beetle 2011-2016     Max.Spped:180
5       VOLKSWAGEN Beetle 2016-2019     Max.Spped:180
```

Тут за основу порівнювання вибрано Id. У отриманого об'єкт береться Id порівнюеться з поточним і в залежності від цього повертаеться відповідне число. Значення відповідає вимогам описаним в інтерфейсі.

Реалізацію можна спростити.
```cs
    class Car_v2 : Car, IComparable
    {
        public Car_v2() {}

        public Car_v2(int id, string name, int maxSpeed) : base(id, name, maxSpeed) {}

        public int CompareTo(object? obj)
        {
            if (obj is Car_v2 otherCar)
            {
                return Id.CompareTo(otherCar.Id);
            }

            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

}
```
```cs
ExplorationArraySort_3();
void ExplorationArraySort_3()
{
    Car_v2[] cars = new Car_v2[5];

    cars[0] = new Car_v2(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v2(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v2(1, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v2(4, "VOLKSWAGEN Beetle 2011-2016", 180);
    cars[4] = new Car_v2(5, "VOLKSWAGEN Beetle 2016-2019", 180);

    Array.Sort(cars);

    foreach (Car_v2 car in cars)
    {
        Console.WriteLine(car);
    }
}
```
```
ExplorationArraySort_3();
void ExplorationArraySort_3()
{
    Car_v2[] cars = new Car_v2[5];

    cars[0] = new Car_v2(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v2(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v2(1, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v2(4, "VOLKSWAGEN Beetle 2011-2016", 180);
    cars[4] = new Car_v2(5, "VOLKSWAGEN Beetle 2016-2019", 180);

    Array.Sort(cars);

    foreach (Car_v2 car in cars)
    {
        Console.WriteLine(car);
    }
}
```
Оскільки Id типу int  можна використати вбудовану реалізацію інтерфейсу IComparable.
Аналогічно сортуванню за Id можна зробити сортування за іншими властивостями.

```cs
    class Car_v3 : Car, IComparable
    {
        public Car_v3() { }

        public Car_v3(int id, string name, int maxSpeed) : base(id, name, maxSpeed) { }

        public int CompareTo(object? obj)
        {
            if (obj is Car_v3 otherCar)
            {
                return MaxSpeed.CompareTo(otherCar.MaxSpeed);
            }

            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }
```
```cs
ExplorationArraySort_4();
void ExplorationArraySort_4()
{
    Car_v3[] cars = new Car_v3[5];

    cars[0] = new Car_v3(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v3(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v3(5, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v3(4, "VOLKSWAGEN Beetle 2016-2019", 180);
    cars[4] = new Car_v3(1, "VOLKSWAGEN Beetle 2011-2016", 180);


    Array.Sort(cars);

    foreach (Car_v3 car in cars)
    {
        Console.WriteLine(car);
    }
}
```
```
3       VOLKSWAGEN Beetle 1945-2003     Max.Spped:100
5       VOLKSWAGEN Beetle 1998-2005     Max.Spped:161
4       VOLKSWAGEN Beetle 2016-2019     Max.Spped:180
1       VOLKSWAGEN Beetle 2011-2016     Max.Spped:180
2       VOLKSWAGEN Beetle 2001-2002     Max.Spped:225
```

## Кілька порядків сортування. IComparer.

Якшо треба виконати сортування за кількома властивостями тоді треба використати interface IComparer.

```cs
    //
    // Summary:
    //     Exposes a method that compares two objects.
    public interface IComparer
    {
        //
        // Summary:
        //     Compares two objects and returns a value indicating whether one is less than,
        //     equal to, or greater than the other.
        //
        // Parameters:
        //   x:
        //     The first object to compare.
        //
        //   y:
        //     The second object to compare.
        //
        // Returns:
        //     A signed integer that indicates the relative values of x and y:
        //     - If less than 0, x is less than y.
        //     - If 0, x equals y.
        //     - If greater than 0, x is greater than y.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     Neither x nor y implements the System.IComparable interface. -or- x and y are
        //     of different types and neither one can handle comparisons with the other.
        int Compare(object? x, object? y);
    }
```

В інтерфейсі порівнюються 2 об'єкти. 

```cs
    public class Car_v3NameComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car_v3 car1 && y is Car_v3 car2)
            {
                return string.Compare(car1.Name, car2.Name, StringComparison.OrdinalIgnoreCase);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }
```
```cs
ExplorationArraySort_5();
void ExplorationArraySort_5()
{
    Car_v3[] cars = new Car_v3[5];

    cars[0] = new Car_v3(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v3(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v3(5, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v3(4, "VOLKSWAGEN Beetle 2016-2019", 180);
    cars[4] = new Car_v3(1, "VOLKSWAGEN Beetle 2011-2016", 180);

    Console.WriteLine("Sort by MaxSpeed");

    Array.Sort(cars);

    foreach (Car_v3 car in cars)
    {
        Console.WriteLine(car);
    }

    Console.WriteLine("\nSort by Name");
    Array.Sort(cars,new Car_v3NameComparer());

    foreach (Car_v3 car in cars)
    {
        Console.WriteLine(car);
    }
}
```
```
Sort by MaxSpeed
3       VOLKSWAGEN Beetle 1945-2003     Max.Spped:100
5       VOLKSWAGEN Beetle 1998-2005     Max.Spped:161
4       VOLKSWAGEN Beetle 2016-2019     Max.Spped:180
1       VOLKSWAGEN Beetle 2011-2016     Max.Spped:180
2       VOLKSWAGEN Beetle 2001-2002     Max.Spped:225

Sort by Name
3       VOLKSWAGEN Beetle 1945-2003     Max.Spped:100
5       VOLKSWAGEN Beetle 1998-2005     Max.Spped:161
2       VOLKSWAGEN Beetle 2001-2002     Max.Spped:225
1       VOLKSWAGEN Beetle 2011-2016     Max.Spped:180
4       VOLKSWAGEN Beetle 2016-2019     Max.Spped:180
```
Цей інтерфейс зазвичай не реалізовується в типі який треба відсортувати. Натомість інтерфейс реалізовуеться в допоміжних класах. В цьому прикладі створено додадковий клас в якому порівняння здійснюеться по Name. Як бачимо в цьому випадку метод Sort не використовує поведінку IComparable.

```cs
    class Car
    {
        public int Id { get; set; } 
        public string Name { get; set; } = "";
        public int MaxSpeed { get; }

        public Car(int id, string name, int maxSpeed)
        {
            Id = id;
            Name = name;
            MaxSpeed = maxSpeed;
        }
        public Car() {}

        public override string? ToString() => $"{Id}\t{Name}\tMax.Spped:{MaxSpeed}";

    }
```
```cs
   public class CarNameComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car car1 && y is Car car2)
            {
                return string.Compare(car1.Name, car2.Name, StringComparison.OrdinalIgnoreCase);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

    public class CarIdComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car car1 && y is Car car2)
            {
                return car1.Id.CompareTo(car2.Id);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }

    public class CarMaxSppedComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car car1 && y is Car car2)
            {
                return car1.MaxSpeed.CompareTo(car2.MaxSpeed);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }
```
```cs
ExplorationArraySort_6();
void ExplorationArraySort_6()
{
    Car[] cars = new Car[5];

    cars[0] = new Car(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car(5, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car(4, "VOLKSWAGEN Beetle 2016-2019", 180);
    cars[4] = new Car(1, "VOLKSWAGEN Beetle 2011-2016", 180);

    Console.WriteLine("\nSort by Name");
    Array.Sort(cars, new CarNameComparer());

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }

    Console.WriteLine("\nSort by Id");
    Array.Sort(cars, new CarIdComparer());

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }

    Console.WriteLine("\nSort by MaxSpeed");
    Array.Sort(cars, new CarMaxSppedComparer());

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }
}
```
```
Sort by Name
3       VOLKSWAGEN Beetle 1945-2003     Max.Spped:100
5       VOLKSWAGEN Beetle 1998-2005     Max.Spped:161
2       VOLKSWAGEN Beetle 2001-2002     Max.Spped:225
1       VOLKSWAGEN Beetle 2011-2016     Max.Spped:180
4       VOLKSWAGEN Beetle 2016-2019     Max.Spped:180

Sort by Id
1       VOLKSWAGEN Beetle 2011-2016     Max.Spped:180
2       VOLKSWAGEN Beetle 2001-2002     Max.Spped:225
3       VOLKSWAGEN Beetle 1945-2003     Max.Spped:100
4       VOLKSWAGEN Beetle 2016-2019     Max.Spped:180
5       VOLKSWAGEN Beetle 1998-2005     Max.Spped:161

Sort by MaxSpeed
3       VOLKSWAGEN Beetle 1945-2003     Max.Spped:100
5       VOLKSWAGEN Beetle 1998-2005     Max.Spped:161
1       VOLKSWAGEN Beetle 2011-2016     Max.Spped:180
4       VOLKSWAGEN Beetle 2016-2019     Max.Spped:180
2       VOLKSWAGEN Beetle 2001-2002     Max.Spped:225
```
В цому прикладі клас не реалізовує інтерфейс IComparable а для метода Array.Sort використовуються додадкові класи.

## Тип IComparer в якості властивості типу.
```cs
    class Car_v4 : Car
    {
        public Car_v4() { }

        public Car_v4(int id, string name, int maxSpeed) : base(id, name, maxSpeed) { }

        public static IComparer SortByName => new Car_v4NameComparer();
    }

    public class Car_v4NameComparer : IComparer
    {
        public int Compare(object? x, object? y)
        {
            if (x is Car_v4 car1 && y is Car_v4 car2)
            {
                return string.Compare(car1.Name, car2.Name, StringComparison.OrdinalIgnoreCase);
            }
            throw new ArgumentException("The parameter is not a compatible type.");
        }
    }
```
```cs
ExplorationArraySort_7();
void ExplorationArraySort_7()
{
    Car_v4[] cars = new Car_v4[5];

    cars[0] = new Car_v4(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v4(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v4(5, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v4(4, "VOLKSWAGEN Beetle 2016-2019", 180);
    cars[4] = new Car_v4(1, "VOLKSWAGEN Beetle 2011-2016", 180);

    Array.Sort(cars,Car_v4.SortByName);

    foreach (Car_v4 car in cars)
    {
        Console.WriteLine(car);
    }
}
```
```
3       VOLKSWAGEN Beetle 1945-2003     Max.Spped:100
5       VOLKSWAGEN Beetle 1998-2005     Max.Spped:161
2       VOLKSWAGEN Beetle 2001-2002     Max.Spped:225
1       VOLKSWAGEN Beetle 2011-2016     Max.Spped:180
4       VOLKSWAGEN Beetle 2016-2019     Max.Spped:180
```
Аби не "запам'ятовувати" шо десь є додадковий клас який поверне необхідний IComparer як неотємною частиною класа можна створити статичний метод який дозволить сортувати набори об'єктів.
