# Records

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
}
```
```
Records.Car
Records.Car
False
```
Тут використовуються логіка реалізована в System.Object. Метод ToString відображає тип посилання. При порівнювані двох об'єктів порівнюються чи на одне і теж місце посилаються змінни. Аби реалізувати інши правила порівняння об'єктів треба додавати окремий код. Оскільки такі задачи досить часті і був добавлений тип records.

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
}
```
```
Records.CarRecord_v1
CarRecord_v1 { Manufacturer = VW, Model = Polo, Color = Red }
True
```
Створення record з стандартного типу властивостей схоже на створення класу. Цей класс immutable оскільки використовується init. Теж саме можна створити компактим кодом.


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

    CarRecord_v2 car3 = new CarRecord_v2(null, null, null);
    Console.WriteLine(car3.Manufacturer);
    Console.WriteLine(car3.Model);
    Console.WriteLine(car3.Color);

}
```
```
Records.CarRecord_v2
CarRecord_v2 { Manufacturer = VW, Model = Polo, Color = Red }
True




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








