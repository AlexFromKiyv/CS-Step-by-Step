# Клас. Конструктори

Клас це визначений користувачем тип який складаеться з полів данних і членами які працюють з ними. Набір данних полів є "станом" екземпляра класу якій називають об'єктом. Це поєднання полів і методів дозволяє створити модель об'єктів реального світу. Під членами класу розуміють властивості і методи.

## Створення

1. Створимо рішеня OOP з консольним проектом SimpleClass.
2. На проекті правий-клік Add > Class.
3. Name: Bike.cs
4. Замініть контент цього класу
```cs
namespace SimpleClass
{
    class Bike
    {
        public string ownerName;
        public int currentSpeed;

        public void StateToConsol() => Console.WriteLine($"Bicycler: {ownerName} is driving at speed: {currentSpeed}");
        public void SpeedUp(int speedChange) => currentSpeed += speedChange;
    }
}
```
Тут члени-змінні визначають стан а методи поведінку. Члени оголошено з модіфікаторм public. Це означає коли буде створено об'єкт то до такіх членів буде безпосередній доступ. Гарною практикою є прості поля класу робити або приватними або захищеними. 
Iснує домовленність приватні поля починати з _. Наприклад _ownerPhoneNumber. 

5. Замініть контент Program.cs
```cs
using SimpleClass;

UsingClassBike();
void UsingClassBike()
{
    Bike bike;
   // bike.StateToConsol(); don't work
    bike = new();
    bike.StateToConsol();

    bike.ownerName = "Mark";
    bike.currentSpeed = 5;
    bike.StateToConsol();


    for (int i = 0; i < 5; i++)
    {
        bike.SpeedUp(1);
        bike.StateToConsol();
    }
}
```
```
Bicycler:  is driving at speed: 0
Bicycler: Mark is driving at speed: 5
Bicycler: Mark is driving at speed: 6
Bicycler: Mark is driving at speed: 7
Bicycler: Mark is driving at speed: 8
Bicycler: Mark is driving at speed: 9
Bicycler: Mark is driving at speed: 10
```
В строчці Bike bike; створюється об'єкт якій ше не визначено. Наступна bike = new(); присваює посилання на об'єкт який створено з використанням конструктора за замовчуванням.

## Конструктори.

Оскільки об'єкти мають стан є бажання мати механізм його втановленя при створенні.
Конструктор — це спеціальний метод класу, який викликається опосередковано під час створення об’єкта за допомогою ключового слова new. Вони мають таку саму назву як клас і нічно не повертають.

Як видно з попереднього прикладу при створені об'єкта поля отримали значення default. Це робота конструктора за замовчуванням. Цю поведінку можна перевизначити.

```cs
        //Overload default constructor
        public Bike()
        {
            ownerName = "Noname";
            currentSpeed = 2;
        }

```
```
Bicycler: Noname is driving at speed: 2
Bicycler: Mark is driving at speed: 5
Bicycler: Mark is driving at speed: 6
Bicycler: Mark is driving at speed: 7
Bicycler: Mark is driving at speed: 8
Bicycler: Mark is driving at speed: 9
Bicycler: Mark is driving at speed: 10
```
Конструктор за замовчуванням ніколи не приймає аргументів.

Як правило визначаюсться додадкові конструктори які дозволяють сворювати об'єкти як потрібно.

```cs
        // currentSpeed = default // construtor can be one-line method
        public Bike(string ownerName) => this.ownerName = ownerName;


        // caller will set all
        public Bike(string ownerName, int currentSpeed)
        {
            this.ownerName = ownerName;
            this.currentSpeed = currentSpeed;
        }

```

Конструктори відрізняються кількістю або типом параметрів. Тут діють правила перезавантаження методів. Тепер є кілька способів створити об'єкти.

```cs
UsingVariousConstructors();
void UsingVariousConstructors()
{
    Bike bike = new();
    bike.StateToConsol();

    Bike veraOnBike = new("Vera");
    veraOnBike.StateToConsol();

    Bike maxBike = new("Max", 15);
    maxBike.StateToConsol();
}
```
```
Bicycler: Noname is driving at speed: 2
Bicycler: Vera is driving at speed: 0
Bicycler: Max is driving at speed: 15
```
Параметр конструктора може мати модіфікатор out.

Як тільки ви створили власний конструктор компілятор розуміє шо ви взяли процес створення в свої руки і конструктора за замовчуванням не предоставляє. Тобто ви повині самі превизначити його. Наприклад
```cs
        // set all data members to default values
        public Bike()
        {
        }
```
Інакше компілятор видасть помилку.

Для створення конструкторів у пригоді буде комбінація клавіш CTRL + . 

## this

Ключеве слово this надає доступ до поточного екземпляра класу.
```cs
        public Bike(string ownerName) => this.ownerName = ownerName;
```
Якшо в цій строчці зтерти this компілятор покаже попередження оскільки назва поля зпівпадає з назвою параметра. Додамо в клас метод

```cs
public void SetOwnerName(string ownerName) => ownerName = ownerName;
```
Спробуємо виконати.
```cs
IfNoUseThis();
void IfNoUseThis()
{
    Bike bike = new();
    bike.SetOwnerName("David");
    bike.StateToConsol();
}
```
```
Bicycler: Noname is driving at speed: 2
```
Хоча метод компілюеться він не робить того шо нам треба. Компьлятор попереджує шо ми призначаємо змінну саму собі.

```cs
        public void SetOwnerNameWithThis(string ownerName) => this.ownerName = ownerName;
```
```
Bicycler: David is driving at speed: 2
```
this вказую шо змінну поля треба шукати в полях поточного об'єкту, а не брати змінну методу. Таким чином this рішає ноеоднозначність.

this користно використовувати в ланцюгу конструкторів. Цей паттерн корисний аби не повторювати веріфікацію данних в кожному конструкторі.
```cs
   class BadBus25
    {
        public int numberOfSeats;
        public string? driverName;

        public BadBus25()
        {
        }

        public BadBus25(int numberOfSeats)
        {
            if (numberOfSeats > 24)
            {
                numberOfSeats = 24;
            }
            this.numberOfSeats = numberOfSeats;
        }

        public BadBus25(int numberOfSeats, string driverName) 
        {
            if (numberOfSeats > 24)
            {
                numberOfSeats = 24;
            }
            this.numberOfSeats = numberOfSeats;

            this.driverName = driverName;
        }
    }
```
В класі реалізонана однакова логіка перевірки в різних конструкторах. Якшо треба буде змінити логіку її требу буде відствжувати в двух місцях. Можна створити окремий метод перевірки але існує інший варіант.
```cs
    class Bus25
    {
        public int numberOfSeats;
        public string? driverName;

        public Bus25(int numberOfSeats, string? driverName)
        {
            Console.WriteLine("in main constructor");
            if (numberOfSeats > 24)
            {
                numberOfSeats = 24;
            }
            this.numberOfSeats = numberOfSeats;
            this.driverName = driverName;
        }

        public Bus25()
        {
            Console.WriteLine("in default constructor");
        }

        public Bus25(int numberOfSeats) : this(numberOfSeats, null)
        {
            Console.WriteLine("in constructor for numberOfSeats");
        }

        public Bus25(string? driverName) : this(default, driverName)
        {
            Console.WriteLine("in constructor for driverName");
        }

        public void StateToConsol() => Console.WriteLine($"driverName: {driverName}  numberOfSeats: {numberOfSeats}");
    }
``` 
Тут спочатку реалізовуеться конструктор з максимальною кількістю праметрів і необхідною логікою. Інші конструктори визивають його через this. Таким чином треба турбуватись за один конструктор в класі. 

Викличемо один із конструкторів
```cs
UsingChainOfConstructors();
void UsingChainOfConstructors()
{
    Bus25 bus25 = new(30);
    bus25.StateToConsol();
}
```
```
in main constructor
in constructor for numberOfSeats
driverName:   numberOfSeats: 24
```
Як видно спочатку виконуеться конструктор з всіма параметрами а потім сам конструктор. 

Можна створити конструктор використовуючи необов'язкові параметри.
```cs
    class Bus
    {
        public int numberOfSeats;
        public string? driverName;

        public Bus(int numberOfSeats = 20 , string? driverName = "Someone")
        {
            if (numberOfSeats > 30)
            {
                numberOfSeats = 30;
            }

            this.numberOfSeats = numberOfSeats;
            this.driverName = driverName;
        }
        public void ToConsol() => Console.WriteLine($"driverName: {driverName}  numberOfSeats: {numberOfSeats}");
    }
```
Таке визначення тепер надає декілька способів створення об’єктів за допомогою єдиного конструктора.
```cs
UsingConstructorWithOpionalParameter();

void UsingConstructorWithOpionalParameter()
{
    Bus bus_1 = new();
    bus_1.ToConsol();
    
    Bus bus_2 = new(15);
    bus_2.ToConsol();

    Bus bus_3 = new(driverName:"Mark");
    bus_3.ToConsol();

    Bus bus_4 = new(35, "Jack");
    bus_4.ToConsol();

    Bus bus_5 = new(default, default);
    bus_5.ToConsol();
}
```
```
driverName: Someone  numberOfSeats: 20
driverName: Someone  numberOfSeats: 15
driverName: Mark  numberOfSeats: 20
driverName: Jack  numberOfSeats: 30
driverName:   numberOfSeats: 0
```

## Деконструктори.

Ця можливість дозволяє розкласти об'єкт на складові.

```cs
    internal class Car
    {
        public string? Manufacturer { get; set; }
        public string? Model { get; set; }

        public Car(string? manufacturer = "", string? model="")
        {
            Manufacturer = manufacturer;
            Model = model;
        }

        public void Deconstruct(out string?  manufacturer, out string? model)
        {
            manufacturer = Manufacturer;
            model = Model;            
        }
    }
```
```cs
UsingDeconstruct();
void UsingDeconstruct()
{
    Car car = new() { Manufacturer = "VW", Model = "E-Golf" };

    (string? manufacturer, string? model) = car;

    Console.WriteLine( $"{manufacturer} - {model}");

    (_, string? model1) = car;

    Console.WriteLine(model1);
}
```
```
VW - E-Golf
E-Golf
```






