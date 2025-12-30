# Записи. Records

Розглянемо клас.
```cs
    internal class Car
    {
        public string Manufacturer { get; init; }
        public string Model { get; init; }
        public string Color { get; init; }

        public Car(string manufacturer, string model, string color)
        {
            Manufacturer = manufacturer;
            Model = model;
            Color = color;
        }

        public Car() : this("Not known.", "Not known.", "Not known.")
        {
        }
    }
```
```cs
void UsingCar()
{
    Car car1 = new Car("VW", "Polo", "Red");
    Car car2 = new()
    {
        Manufacturer = "VW",
        Model = "Polo",
        Color = "Red"
    };

    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);

    Console.WriteLine( car1 == car2 );
    Console.WriteLine(ReferenceEquals(car1,car2));
}
```
```
Records.Car
Records.Car
False
False
```
Зверніть увагу на тип власитвості яка ініщіалізується. Тут використовуються логіка реалізована в System.Object. Метод ToString відображає тип посилання. При порівнювані двох об'єктів порівнюються чи на одне і теж місце посилаються змінни. Аби реалізувати інши правила порівняння об'єктів треба додавати окремий код. Оскільки такі задачи досить часті і був добавлений тип records.

## Створення

```cs
    record CarRecord_v1
    {
        public string Manufacturer { get; init; }
        public string Model { get; init; }
        public string Color { get; init; }

        public CarRecord_v1(string manufacturer, string model, string color)
        {
            Manufacturer = manufacturer;
            Model = model;
            Color = color;
        }
        public CarRecord_v1():this("Not known", "Not known", "Not known")
        {
        }
    }
```
```cs
UsingCarRecord_v1();
void UsingCarRecord_v1()
{
    CarRecord_v1 car1 = new("VW","Polo","Red");
    CarRecord_v1 car2 = new()
    {
        Manufacturer = "VW",
        Model = "Polo",
        Color = "Red"
    };

    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);
    Console.WriteLine( car1 == car2 );
    Console.WriteLine(ReferenceEquals(car1, car2));
}
```
```
Records.CarRecord_v1
CarRecord_v1 { Manufacturer = VW, Model = Polo, Color = Red }
True
False
```
Створення record з стандартного типу властивостей схоже на створення класу. Цей класс immutable оскільки використовується init. Теж саме можна створити компактим кодом. Для типів record неявно змінена реалізація Equlas, == та !=. Два записи вважаються однаковими якшо ввони одного типу і співпадають відповідні значення.


## Record з позиційним синтаксисом.

```cs
record CarRecord_v2(string Manufacturer, string Model, string Color);
```
```cs
UsingCarRecord_v2();
void UsingCarRecord_v2()
{
    CarRecord_v2 car1 = new("VW", "Polo", "Red");
    CarRecord_v2 car2 = new CarRecord_v2("VW", "Polo", "Red");

   // car1.Manufacturer = "Volks Wagen"; // don't work  immutable


    Console.WriteLine(car1.GetType());
    Console.WriteLine(car1);
    Console.WriteLine(car1 == car2);
    Console.WriteLine(car1.Equals(car2));
    Console.WriteLine(ReferenceEquals(car1, car2));
}
```
```
Records.CarRecord_v2
CarRecord_v2 { Manufacturer = VW, Model = Polo, Color = Red }
True
True
False
```
Використовуючи синатксис схожий на початок визначення конструктора можна створити тип record якій є immutable, тобто тільки для читання. З записами ініціалізатор не працює. При створені треба враховувати позицію. Тобто record надає головний конструктор з усіма властивостями.

## Deconstruct

```cs
UsingDeconstuct();
void UsingDeconstuct()
{
    CarRecord_v1 car1 = new("VW", "Polo", "Red");
    
    // car1.Deconstruct( It's no here this method

    CarRecord_v2 car2 = new("VW", "Polo", "Red");

    string color;

    car2.Deconstruct(out string manufacturer, out string model, out color);


    Console.WriteLine($"{manufacturer} {model} {color}");

    var (manufacturer_1, model_1, color_1) = car2;

    Console.WriteLine($"{manufacturer_1} {model_1} {color_1}");

}
```
```
VW Polo Red
VW Polo Red
```

При створені record з позиційним синтаксисом надаеться метод Deconstruct. Положеня параметра відповідае порядку створеня типу. 

## Змінювальні(mutable) record.
```cs
    record CarRecord_v3
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }

        public CarRecord_v3(string manufacturer, string model, string color)
        {
            Manufacturer = manufacturer;
            Model = model;
            Color = color;
        }
        public CarRecord_v3() : this("Not known", "Not known", "Not known")
        {
        }
    }
```
Хоча такий синтаксис підтримуеться, тип record передбачає використання в незмінних моделях даних.

## Копіювання record.

Тип record це посуті клас з особовою поведінкою. Зміна такого типу вказує на місце в пам'яті і веде себе як об'єкт класу.

```cs
record CarRecord(string Manufacturer, string Model, string Color);
```
```cs
AssigningRecord();
void AssigningRecord()
{
    CarRecord carRecord_1 = new CarRecord("VW", "Polo", "Red");
    CarRecord carRecord_2 = carRecord_1;

    Console.WriteLine(carRecord_2);
    Console.WriteLine(carRecord_1.Equals(carRecord_2));
    Console.WriteLine(ReferenceEquals(carRecord_1,carRecord_2));
}
```
```
CarRecord { Manufacturer = VW, Model = Polo, Color = Red }
True
True
```
Тоб то призначення просто записує посилання на той самий об'єкт. Але є синтаксис що дозволяє робити копію.

```cs
record CarRecord(string Manufacturer, string Model, string Color);
```
```cs
CopyingRecord();
void CopyingRecord()
{
    CarRecord carRecord = new("VW", "Polo", "Not known");

    CarRecord carRecord1 = carRecord with { Color = "White" };
    CarRecord carRecord2 = carRecord with { Color = "Red" };


    Console.WriteLine(carRecord1);
    Console.WriteLine(carRecord2);

    Console.WriteLine(carRecord.Equals(carRecord1));
    Console.WriteLine(ReferenceEquals(carRecord,carRecord1));

}
```
```
CarRecord { Manufacturer = VW, Model = Polo, Color = White }
CarRecord { Manufacturer = VW, Model = Polo, Color = Red }
False
False
```
Таким чином можна створювати аналогічні записи на основі існуючих встановлюючи потрідні властивості.

## Record struct

Record struct аналогічний типу record але має Value type. Тобто зберігає дани в стеку. Найбільша відмінність record struct від record то шо вони за замовчуваням змінні(muttable). 
```cs
public record struct Point_v1(int X,int Y);
```
```cs
    public record struct Point_v2
    {
        public int X { get; set; } = default;
        public int Y { get; set; } = default;

        public Point_v2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void ToConsole() => Console.WriteLine($"[{X},{Y}]");
    }
```
```cs
UsingRecordStruct();
void UsingRecordStruct()
{
    Point_v1 point1 = new(1, 1);
    Console.WriteLine(point1);
    
    point1.X = 2;
    point1.Y = 2;
    Console.WriteLine(point1);

    Point_v2 point2 = new();
    point2.ToConsole();
    
    Point_v2 point3 = point2 with { Y = 1 };
    point3.ToConsole();
}
```
```
Point_v1 { X = 1, Y = 1 }
Point_v1 { X = 2, Y = 2 }
[0,0]
[0,1]
```
Вирази з with працюють аналогічно record. Для того щоб структура стала незмінною(immutable) треба добавити модіфікатор readonly або змінити в властивості set на init.

```cs
public readonly record struct Point_v3(int X, int Y);
```
```cs
    public record struct Point_v4
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Point_v4(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void ToConsole() => Console.WriteLine($"[{X},{Y}]");
    }
```
```cs
UsingReadonlyRecordStruct();
void UsingReadonlyRecordStruct()
{
    Point_v3 point1 = new(1, 1);
    Console.WriteLine(point1);

    //point1.X = 2; don't work


    Point_v4 point2 = new(2,2);
    //point2.X = 3; don't work
    point2.ToConsole();
}
```
```
Point_v3 { X = 1, Y = 1 }
[2,2]
```
## Deconstruct для record struct.

Визначені за допомогою позиційного синтаксису record struct мають метод Decobstruct.
```cs
public record struct Point_v1(int X,int Y);
```
```cs
UsingDecontrictRecordStruct();
void UsingDecontrictRecordStruct()
{
    Point_v1 point = new(1, 2);
    var (x, y) = point;
    Console.WriteLine(x.GetType());
    Console.WriteLine(x);

    point.Deconstruct(out int x1,out int y1);
    Console.WriteLine(x1);
}
```
```
System.Int32
1
1
```
