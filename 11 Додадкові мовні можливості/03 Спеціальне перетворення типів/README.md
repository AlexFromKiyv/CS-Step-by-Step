# Спеціальне перетворення типів

## Неявне та явне претворення типів

Для числових структур.
```cs
void ImplicitAndExplicitConversionsNumeric()
{
    int a = 1;
    long b = a;      // Implicit conversion
    int c = (int)b;  // Explicit conversion
}
```
Перетворення в внутрішніх числових структурах вимагає явного претворення коли ви намагаєтесь зберігти більше значення в меньшому контейнері. Це може привести до втрати данних якшо не включити перевірку. Перетовреня з значення меньшого типу в більший контейнер безпечне тому відбуваеться неявно.  

Для споріднених типів.

```cs
void ImplicitAndExplicitConversionsClasses()
{
    Base baseThing = new Derived() {Id = 1, Name="Drone" }; // Implicit conversion

    Console.WriteLine(baseThing.Id);
    // baseThing have no Name

    Derived derivedThing = (Derived)baseThing; // Explicit conversion
    Console.WriteLine(derivedThing.Id);
    Console.WriteLine(derivedThing.Name);

    Base otherThing = new() { Id = 2 };
    Console.WriteLine(otherThing.Id);

    //Unhandled exception. System.InvalidCastException
    //Derived derivedOtherThing = (Derived)otherThing; 
    //Console.WriteLine(derivedOtherThing.Id);
    //Console.WriteLine(derivedOtherThing.Name);

    Console.WriteLine(otherThing as Derived is null);
    
}

ImplicitAndExplicitConversionsClasses();
```
```
1
1
Drone
2
True

```
Об'єкти можуть перетворюваться в типи вниз і в гору по ієрархії класів. Об'єкт похідного класу завжди можна перетворити на об'єкт базового типу. Однак, якшо об'ект в пам'яті похідного типу а зміна яка на нього вказує базового то можна виконати перетворення явно вказуючи це. Якшо в купі знаходиться об'єкт базового класу то спроба перетворити його на об'єкт похідного класу викличе виняток InvalidCastException.

Іноді виникає потреба перетворення об'єктів які створені від класів які в різних ієрархіях. В цому випадку вбудовані, типові  операції приведення не допомагають.

Припустимо у наc є valuе type структури Rectangle і Square . Структури не можуть викорустовувати класичне успадкування і немає готового способу перетворення. Хоча структури можуть мати схожі риси. Хоча можна створити спеціальний метод перетворення, в C# можна стоврити метод який буде працювати як приведення ().

## Explicit(явне) перетворення.

```cs
    struct Rectangle
    {
        public double Width { get; set; }
        public double Heigth { get; set; }

        public Rectangle(int width, int heigth)
        {
            Width = width;
            Heigth = heigth;
        }

        public override string? ToString()
        {
            return $"[{Width} x {Heigth}]";
        }
    }

    struct Square
    {
        public double Length { get; set; }

        public Square(double length)
        {
            Length = length;
        }
        public override string? ToString()
        {
            return $"[{Length} x {Length}]";
        }

        public static explicit operator Square(Rectangle rectangle) =>
            new Square(Math.Sqrt(rectangle.Heigth * rectangle.Width)  );        
    }

```
```cs
void UseCustomConversion()
{
    Rectangle rectangle = new(10, 20);

    Console.WriteLine(rectangle);

    Console.WriteLine((Square)rectangle);
}

UseCustomConversion();
```
```
[10 x 20]
[14,142135623730951 x 14,142135623730951]
```
В C# є два ключові слова explicit, implicit які можна використовувати шоб контролювати як поводитись при спробі перетворення. В цому прикладі вказано як поводитись при явному перетворенні. Зверніть увагу операція вказана як статична. Вхідний параметр це сутність з якої виконується перетворення.

Явних перетворень може бути декілька.
```cs
struct Square_v1
    {
        public double Length { get; set; }

        public Square_v1(double length)
        {
            Length = length;
        }
        public override string? ToString()
        {
            return $"[{Length} x {Length}]";
        }

        public static explicit operator Square_v1(Rectangle rectangle) =>
           new Square_v1(Math.Sqrt(rectangle.Heigth * rectangle.Width));

        public static explicit operator Square_v1(double volume)
        {
            return new Square_v1(Math.Sqrt(volume));
        }

        public static explicit operator double(Square_v1 square)
        {
            return square.Length * square.Length;
        }
    }
```
```cs
void UseAdditionExplicitConversion()
{
    Rectangle rectangle = new(25,4);
   
    Console.WriteLine(rectangle);

    Square_v1 square = (Square_v1)rectangle;

    Console.WriteLine(square);

    double length = (double)square;

    Console.WriteLine(length);

    Console.WriteLine((Square_v1)length);
}

UseAdditionExplicitConversion();
```
```
[25 x 4]
[10 x 10]
100
[10 x 10]

```
Треба зауважити компілятор ні як не реагує який тип в який ви перетворюєте.

## Explicit(неявне) перетворення.

```cs
    struct Rectangle_v1
    {
        public double Width { get; set; }
        public double Heigth { get; set; }

        public Rectangle_v1(double width, double heigth)
        {
            Width = width;
            Heigth = heigth;
        }

        public override string? ToString()
        {
            return $"[{Width} x {Heigth}]";
        }

        public static implicit operator Rectangle_v1(Square square) => new Rectangle_v1(square.Length, square.Length); 
 
    }
```
```
void UseImlicitConversion()
{
    Square square = new(10);

    Rectangle_v1 rectangle = square;

    Console.WriteLine(rectangle);

    Rectangle_v1 rectangle1 = (Rectangle_v1)square;
    
    Console.WriteLine(rectangle1);

}
```
```
[10 x 10]
[10 x 10]

```
Неявне перетворення можна викликати як явно так і неявно. 