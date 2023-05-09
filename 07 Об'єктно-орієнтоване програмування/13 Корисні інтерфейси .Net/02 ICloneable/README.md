# ICloneable

```cs
    class Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y) {  X = x; Y = y; }
        public Point() { }
        public override string? ToString()
        {
            return $"{X},{Y}";
        }
    }
```
```cs
ExplorationAssignObject();
void ExplorationAssignObject()
{
    Point point1 =new Point(1,1);
    Point point2 = point1;
    
    Console.WriteLine(point1);
    Console.WriteLine(point2);
    
    point2.X = 0; Console.WriteLine("point2.X = 0;");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}
```
```
1,1
1,1
point2.X = 0;
0,1
0,1
```
В цому прикладі операція приначення приводить до двух посилань на той самий об'єкт в пам'яті. Аби дати типу можливість отримувати індентичну копію об'єкта можна застосувати стандартний інтерфес IClonable.
```cs
public interface ICloneable
{
  object Clone();
}
```
Створемо тип реалізуючий цей інтерфейс

```cs
    class Point_v1 : Point, ICloneable
    {
        public Point_v1() {}
        public Point_v1(int x, int y) : base(x, y) {}
        public object Clone() => new Point_v1(X, Y);
    }
```
```cs
ExamineIClonable_1();
void ExamineIClonable_1()
{
    Point_v1 point1 = new(1, 1);
    Point_v1 point2 = (Point_v1)point1.Clone();

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}
```
```
1,1
1,1
point2.X = 0;
1,1
0,1
```
Отже тепер в heap створюється клон і посилання вказують на різні місця.

Якшо тип має багато властивостей і не треба глибоке клонуваня (тобто не треба робити клони внутрішніх зміних яки мають тип reference), тоді код можна спростити.
```cs
    class Point_v2 : Point, ICloneable
    {
        public Point_v2() { }
        public Point_v2(int x, int y) : base(x, y) { }
        public object Clone() => MemberwiseClone();

    }
```
```cs
ExamineIClonable_2();
void ExamineIClonable_2()
{
    Point_v2 point1 = new(1, 1);
    Point_v2 point2 = (Point_v2)point1.Clone();

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}
```
```
1,1
1,1
point2.X = 0;
1,1
0,1
```
Клас System.Object визначае метод MemberwiseClone(). Цей метод необхіден для повехносної копії об'єкта і він protected. Об'єкт може викликати цей метод під час клонування. Вцому прикладі тип не має внутрішніх змінних reference типу тому 
можна використовувати MemberwiseClone. Якшо тип має зміні типу посилання метод MemberwiseClone не буде робити "глибокої" копії а скопіює посилання.

## "Глибоке" клонування.

Припустимо клас має властивість з типом посилання. 

```cs
    class Point_v3 :  ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PointDiscription Description { get; set; } = new PointDiscription();

        public Point_v3() { }
        public Point_v3(int x, int y) { X = x; Y = y; }

        public Point_v3(int x, int y, PointDiscription description) : this(x, y)
        {
            Description = description;
        }
        public override string? ToString() => $"{X},{Y}\t{Description?.Name}\t{Description?.PointId}";
        public object Clone() => MemberwiseClone();
    } 

```
```cs

ExamineIClonable_3();
void ExamineIClonable_3()
{
    Point_v3 point1 = new Point_v3(1, 1, new("Start"));
    Point_v3 point2 = (Point_v3)point1.Clone();

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");
    point2.Description.Name = "End"; 
    Console.WriteLine("point2.Description.Name = \"End\";");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}
```
```
1,1     Start   0de8b783-f78f-4b36-a154-a1d0c15c15b9
1,1     Start   0de8b783-f78f-4b36-a154-a1d0c15c15b9
point2.X = 0;
point2.Description.Name = "End";
1,1     End     0de8b783-f78f-4b36-a154-a1d0c15c15b9
0,1     End     0de8b783-f78f-4b36-a154-a1d0c15c15b9
```
Коли в цьому випадку Clone використовує MemberwiseClone() він робить окремий клон об'єкта разом з посиланням на інший об'єкт. З членами класу які value type все в порядку а reference type вказуе на оди і той самий об'єкт в пам'яті а не на новий клон як зазвичай потрібно.

```cs
    class Point_v4 : ICloneable
    {
        public int X { get; set; }
        public int Y { get; set; }
        public PointDiscription Description { get; set; } = new PointDiscription();

        public Point_v4() { }
        public Point_v4(int x, int y) { X = x; Y = y; }

        public Point_v4(int x, int y, PointDiscription description) : this(x, y)
        {
            Description = description;
        }
        public override string? ToString() => $"{X},{Y}\t{Description?.Name}\t{Description?.PointId}";
        public object Clone()
        {
            Point_v4 newPoint = (Point_v4) MemberwiseClone();

            PointDiscription newPointDiscription = new PointDiscription();

            newPointDiscription.Name = Description.Name;

            newPoint.Description = newPointDiscription;

            return newPoint;
        }
    }
```
```cs
ExamineIClonable_4();
void ExamineIClonable_4()
{
    Point_v4 point1 = new Point_v4(1, 1, new("Start"));
    Point_v4 point2 = (Point_v4)point1.Clone();

    Console.WriteLine(point1);
    Console.WriteLine(point2);

    point2.X = 0; Console.WriteLine("point2.X = 0;");
    point2.Description.Name = "End"; Console.WriteLine("point2.Description.Name = \"End\";");

    Console.WriteLine(point1);
    Console.WriteLine(point2);
}
```
```
1,1     Start   1db60b87-6f81-43ab-8a25-669def767105
1,1     Start   2e8c5907-3489-4490-91c8-1c0d87fc55f8
point2.X = 0;
point2.Description.Name = "End";
1,1     Start   1db60b87-6f81-43ab-8a25-669def767105
0,1     End     2e8c5907-3489-4490-91c8-1c0d87fc55f8
```
Аби метод Clone зробив "глибоку" копію треба крім того шоб зробити копію за добомогою MemberwiseClone створити копію обектів на яки є посилання. В данному випдкі видно шо при клонування створюється окремий об'єкт і копіюється Name. 

### Підсумки

Таким чином якшо тип складаеться тілки з value типів для реалізації метода Clone можна використовувати MemberwiseClone. Якшо тип має зміни типу reference треба робити копії об'єктив на яки є посилання.
 