## Перезавантаження операторів.

C# має набір операторів які просто визначаються маркерами +, -, * (та інші), для виконання основних функцій з внутрішніми типами. Наприклад можна сладати числа або строки. Кожен тип передбачає свою поведінку. Числа складаються, рядки зчеплюються.
Коли мова йде про ваші типи ви можете перезавантажити оператори, але не всі.

+, -, !, ~, ++, --, true, false : ці унарні оператори можна перзавантажити. Якшо треба презавантажити false або true язик вимагає перезавантаженя обох.

+, -, *, /, %, &, |, ^, <<, >> : ці бінарні оператори можна перзавантажити.

==,!=, <, >, <=, >= : ці бінарні оператори можна перзавантажити, але парами.

[] : не перезавантажуеться бо це може зрогбити індексатор.

() : не перезавантажуеться бо це може зрогбити спеціальні метоли перетворення.

+=, -=, *=, /=, %=, &=, |=, ^=, <<=, >>= : скорочені оператори не презавантажуються однак вони спрацьовують коли ви перезавантажуєье відповідний бінарний метод.

## Перезавантаження бінарних операторів.

Оператор + для двох точок може означати нову точку з суммою координат. 
```cs
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override string? ToString() => $"[{X},{Y}]";
    }
```
```cs
    class Point_v1 : Point
    {
        public Point_v1(int x, int y) : base(x, y)
        {
        }

        public static Point_v1 operator + (Point_v1 point1, Point_v1 point2) =>
            new Point_v1(point1.X + point2.X, point1.Y + point2.Y);
        public static Point_v1 operator - (Point_v1 point1, Point_v1 point2) =>
            new Point_v1(point1.X - point2.X, point1.Y - point2.Y);

    }
```

Аби компілятор розумів як реагувати на оператори в бінарниx операціях де операнди об'єкти класу треба визначити ці оператори за допомогою operator. Таке визначення повино бути static.

При визначені operator логіка може бути різною.
```cs
    class Point_v2 : Point
    {
        public Point_v2(int x, int y) : base(x, y)
        {
        }

        public static Point_v2 operator + (Point_v2 point1, int change) =>
            new Point_v2(point1.X + change, point1.Y + change);
        public static Point_v2 operator - (Point_v2 point1, int change) =>
            new Point_v2(point1.X - change, point1.Y - change);
    }
```
Треба зауважити шо коли ви хочете аби оператор працював з різними порядками операндів треба реалізовувати всі можливі варіанти. Компілятор сам не визначить протилежні розміщеня операндів для оператора. 
```cs
    class Point_v3 : Point
    {
        
        public Point_v3(int x, int y) : base(x, y)
        {
        }
        public static Point_v3 operator +(Point_v3 point1, int change) =>
            new Point_v3(point1.X + change, point1.Y + change);
        public static Point_v3 operator +( int change, Point_v3 point1) =>
            new Point_v3(change + point1.X , change + point1.Y );
    }
```

Тепер можна їх використати так само як і для чисел.
```cs
void UseOverlodingOperatorAdditionSubtraction()
{
    Point_v1 point = new(1, 1);
    Point_v1 point1 = new(2, 2);
    
    Console.WriteLine(point+point1);

    Console.WriteLine(point1-point);

    Console.WriteLine(point-point1+point1+point+new Point_v1(10,10));

    Point_v2 point2 = new(20, 20);

    Console.WriteLine(point2 + 100 - 12 + 27);

    //Operator '+' cannot...
    //Console.WriteLine(100 + point2);

    Point_v3 point3 = new(30, 30);

    Console.WriteLine( point3 + 3);
    Console.WriteLine( 3 + point3 );
}

UseOverlodingOperatorAdditionSubtraction();
```
```
[3,3]
[1,1]
[12,12]
[135,135]
[33,33]
[33,33]
```

## Оператори типу += , -=.

Якщо в класі реалізовани бінарні оперетори ці оперетори також працюють.

```cs
void UseShorthandOperator()
{
    Point_v1 point1 = new(1, 1);
    Point_v1 point2 = new(2, 2);

    point1 += point2;

    Console.WriteLine(point1);

}

UseShorthandOperator();
```
```
[3,3]
```

## Унарні оператори.

Можна перезавантажити опертори типу ++. Таке перезавантаження також повинно бути static. В цому випадку ви передаете один параметр того ж типу.
```cs
    class Point_v4 : Point
    {
        public Point_v4(int x, int y) : base(x, y)
        {
        }

        public static Point_v4 operator ++(Point_v4 point) =>
            new Point_v4(point.X+1, point.Y+1);
        public static Point_v4 operator --(Point_v4 point) =>
            new Point_v4(point.X - 1, point.Y - 1);
    }
```
```cs
void UseIncrement()
{
    Point_v4 point = new(1, 1);

    point++;

    Console.WriteLine(point);
}

UseIncrement();
```
```
[2,2]
```
Не забувайте що вирази ++x та x++ змінюють значення в пам'яті по різному.

## Перезавантаження Equals

Метод Equal успадковується з System.Оbject порівнює посилання на об'єкт. Якшо ви вирішили перевизначити цей метод на основі значень стану то логічно перезавантажити відповідні оператори ==, !=.

```cs
   class Point_v5 : Point
    {
        public Point_v5(int x, int y) : base(x, y)
        {
        }

        public override bool Equals(object? obj)
        {
            return (obj?.ToString() == this.ToString());
        }

        public override int GetHashCode() => GetHashCode();

        public static bool operator ==(Point_v5 point1, Point_v5 point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Point_v5 point1, Point_v5 point2)
        {
            return !point1.Equals(point2);
        }
    }
```
```cs
void UseEqualityOperators()
{
    Point_v5 point1 = new(1, 1);
    Point_v5 point2 = new(1, 1);
    Point_v5 point3 = new(2, 3);

    Console.WriteLine(point1 == point2);
    Console.WriteLine(point1 == point3);
}

UseEqualityOperators();
```
```
True
False
```
Простіше поріанювати об'єкти опертором == ніж визивати Equals.

