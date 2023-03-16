# Успадкування

Можливості одного класу можуть бути використовани іншим. І це дозволює повторно використовувати код. Один клас може бути відправною точкою іншого. Проект BasicInheritance.

```cs
    internal class Car
    {
        public readonly int MaxSpeed;
        private int _currentSpeed;
        public Car(int maxSpeed)
        {
            MaxSpeed = maxSpeed;
        }
        public Car():this(60) 
        {
        }
        public int Speed 
        { 
            get { return _currentSpeed; }
            set
            {
                if (value > MaxSpeed)
                {
                    _currentSpeed = MaxSpeed;
                }
                else
                {
                    _currentSpeed = value;
                }
            }
        }
    }
```
```cs
ExamineParentClass();
void ExamineParentClass()
{
    Car car = new Car();

    car.Speed = 50;
    Console.WriteLine($"Car with a speed {car.Speed}");

    car.Speed = 80;
    Console.WriteLine($"Car with a speed {car.Speed}");

    Car redCar = new Car(120);

    redCar.Speed = 70;
    Console.WriteLine($"Car with a speed {redCar.Speed}");

    redCar.Speed = 150;
    Console.WriteLine($"Car with a speed {redCar.Speed}");

}
```
```
Car with a speed 50
Car with a speed 60
Car with a speed 70
Car with a speed 120
```
Як видно тут за допомогою загальнодоступної властивості інкапсулюється приватне поле. Тобко клас реалізовує свою логіку поведінки. Цю логіку може успадкувати клас нащадок.

```cs
    internal class MiniVan : Car
    {
        public void SpeedUp()
        {
            Speed++;
            //_currentSpeed++; //he hasn't access
        }
    }
```
```cs
void ExamineInheritancedClass()
{
    MiniVan van = new();
    Console.WriteLine(van.Speed);

    MiniVan van1 = new();
    van1.Speed = 100;
    Console.WriteLine(van1.Speed);

    MiniVan van2 = new() { Speed = 80 };
    Console.WriteLine(van2.Speed);

    //MiniVan van3 = new(100) //   doesn`t contain constructor

    Console.WriteLine(van2 is MiniVan);
    Console.WriteLine(van2 is Car);
}
```
```
0
60
60
True
True
```
Таке відношення класі називають "is a". Ми можемо cказати MiniVan is a type of Car, або Person is a LiveOrganism. Так відношеня дозволяє створювати класи які розширюють функціональність існуючого.  Клас якій служе основою називають базовим, суперкласом або батьківським. Роль цього класу визначити всі загальні властивості і члени для всіх класів шо його розширюють. Клас розширення називають похідним або дочірнім. Таким чином не треба дублювати з базового в похідний клас і підтримувати код в обох класах.

Похідний клас не має доступу до приватних полів базового. 

Як видно з прикладу похідний клас не успадковує жодного конструктора. Конструктор можна визвати тільки з класу у якому він визначений. Можна викликати конструктор базового класу в ланцюгу конструкторів.

Похідний клас може мати один базовий клас але базовий клас теж може походити від іншого. Клас може реалізовумати багато різних Interface. При ралізіції інтерфейсів клас може демонструвати різні поведінки уникаючи складнощів успадкування від декількох класів.

## Sealed (запечатаний)

```cs
    sealed class Truck :Car
    {
        public int CarryingCapacity { get; set; }
    }

    //class SuperTruck : Truck { } // cannot derive from sealed type

    //class MyString : String // cannot derive from sealed type
```
Клас позначений як sealed не може бути базою іншого класу. Sealed клас корисний при розробці класів утіліт. В просторі імен System багато запечатаних класів. Наприклад не можна за базу вибрети клас String. 

Тип struct є sealed і неможна його брати за основу іншого типу. Структури можна використовувати для моделювання лише окремих, атомарних, визначених користувачем типів даних.






