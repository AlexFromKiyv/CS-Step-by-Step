# Ініціалізація об'єктів.

Конструктори дозволяють отримувати початкові дані при створенні об'єктів. Властивості дозволяють беспечно зберігати дані. При використані вибирають самий зручний конструктор а для даних які не входять до конструктору використовують властивості. 

Для спрощеня встановлення початкових даний існує ініціалізатор. Це спеціальний синтаксис. Проект Initializers.

```cs
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; } = 0;
        public Point()
        {
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void ToConsole()
        {
            Console.WriteLine($"[{X},{Y},{Z}]");
            Console.WriteLine("\n");
        }
    }

```
```cs
ObjectsInitialisation();
void ObjectsInitialisation()
{
    //1
    Point point_1= new Point();
    point_1.X = 1;
    point_1.Y = 1;
    point_1.Z = 1;
    point_1.ToConsole();

    //2
    Point point_2= new Point(2,2);
    point_2.Z = 2;
    point_2.ToConsole();

    //3
    Point point_3= new Point { X=3,Y=3,Z=3 };
    point_3.ToConsole();
}
```
```
[1,1,1]


[2,2,2]


[3,3,3]
```
В останньому варіанті ініціалізації перед встановленням властивостей визиваеться конструктор за замовчуванням. Цей синтаксис використовує частину set властивостей і не працює з приватними даними.

## Init-only

Властивість можна визначити з частиною init.

```cs
    internal class Point_v1
    {
        public int X { get; init; }
        public int Y { get; init; }

        public Point_v1()
        {
        }

        public Point_v1(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void ToConsole() => Console.WriteLine($"[{X},{Y}]\n");
        
    }
```
```cs
UsingInitOnly();
void UsingInitOnly()
{
    Point_v1 point_1 = new(1, 1);
    //point_1.X = 2; // Don't work.  Init-only property 
    point_1.ToConsole();

    Point_v1 point_2 = new Point_v1 { X = 2, Y = 2 };
    point_2.ToConsole();
}
```

Після ініціалізації властивість стає read-only. Такі властивості називають immutable(незмінні). Треба зазначити шо превизначеня конструктора за замовчуванням необхідно шоб працював ініціалізатор. Коли визначаеться головний конструктор вбудований конструктор вже не працює.

Ініціалізатор виконуеться після конструктора.
```cs
    internal class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; } = 0;
        public Point()
        {
        }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public void ToConsole()
        {
            Console.WriteLine($"[{X},{Y},{Z}]");
            Console.WriteLine("\n");
        }
    }

```
```cs

CustomConstuctorAndInizialiser();
void CustomConstuctorAndInizialiser()
{
    Point point = new(1, 1) { X = 2, Y = 2, Z = 2 };
    point.ToConsole();
}
```
```
[2,2,2]
```
Як бачите визов конструктора зайвий. Але можна визначити клас коли кастомний конструктор корисний.

```cs
    internal class Point_v2
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PointColorEnum Color { get; set; }
        public Point_v2(int x, int y)
        {
            X = x;
            Y = y;
        }
        public Point_v2(PointColorEnum color)
        {
            Color = color;
        }

        public Point_v2():this(PointColorEnum.Green)
        {
        }
        public void ToConsole() => Console.WriteLine($"[{X},{Y}] - {Color} \n");
    }
```
```cs
UsingPoint_v2();
void UsingPoint_v2()
{
    Point_v2 point_1 = new() { X = 1, Y = 1 };
    point_1.ToConsole();
    
    Point_v2 point_2 = new Point_v2(PointColorEnum.Red) { X = 2, Y = 2 };
    point_2.ToConsole();
   
}
```
```
[1,1] - Green

[2,2] - Red
```

