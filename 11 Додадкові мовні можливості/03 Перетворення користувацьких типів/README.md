# Перетворення користувацьких типів

Давайте тепер розглянемо тему, тісно пов'язану з перевантаженням операторів: перетворення користувацьких типів. Щоб підготувати основу для обговорення, давайте швидко розглянемо поняття явних та неявних перетворень між числовими даними та пов'язаними типами класів.

Почніть зі створення нового проекту консольної програми з назвою CustomTypeConversions.


## Числові перетворення

Щодо внутрішніх числових типів (sbyte, int, float тощо), явне перетворення потрібне, коли ви намагаєтеся зберегти більше значення в меншому контейнері, оскільки це може призвести до втрати даних. По суті, це ваш спосіб сказати компілятору: «Залиште мене в спокої, я знаю, що намагаюся зробити». І навпаки, неявне перетворення відбувається автоматично, коли ви намагаєтеся помістити менший тип у цільовий тип, що не призведе до втрати даних.

```cs
static void NumericConversions()
{
    int a = 123;
    long b = a;      // Implicit conversion from int to long.
    int c = (int)b;  // Explicit conversion from long to int.
    Console.WriteLine(c);
}
NumericConversions();
```
```
123
```

## Перетворення між пов'язаними типами класів

Типи класів можуть бути пов'язані класичним успадкуванням (відношення «is-a»). У цьому випадку процес перетворення C# дозволяє вам перетворювати типи вгору та вниз по ієрархії класів. Наприклад, похідний клас завжди можна неявно перетворити на базовий тип. Однак, якщо ви хочете зберегти тип базового класу в похідній змінній, ви повинні виконати явне перетворення, ось так:

```cs
// Two related class types.
class Base{}
class Derived : Base{}
// Implicit cast between derived to base.
Base myBaseType;
myBaseType = new Derived();
// Must explicitly cast to store base reference
// in derived type.
Derived myDerivedType = (Derived)myBaseType;
```
Це явне приведення працює, оскільки класи Base та Derived пов'язані класичним успадкуванням, а myBaseType побудовано як екземпляр Derived. Однак, якщо myBaseType є екземпляром Base, приведення типів викличе виняток InvalidCastException. Якщо є сумніви, що приведення типів не вдасться, слід використовувати ключове слово as. Ось перероблений приклад, щоб це продемонструвати:

```cs
// Implicit cast between derived to base.
Base myBaseType2 = new();

//No exception, myDerivedType2 is null
Derived myDerivedType2 = myBaseType2 as Derived;
```
Однак, що робити, якщо у вас є два типи класів у різних ієрархіях без спільного батьківського елемента (окрім System.Object), які потребують перетворення? Оскільки вони не пов'язані класичним успадкуванням, типові операції приведення типів не пропонують жодної допомоги (і ви отримаєте помилку компілятора!).
До речі, розглянемо типи значень (структури). Припустимо, у вас є дві структури з назвами Square та Rectangle. Враховуючи, що структури не можуть використовувати класичне успадкування (оскільки вони завжди запечатані), у вас немає природного способу перетворення типів між цими, здавалося б, пов'язаними типами.
Хоча ви можете створювати допоміжні методи в структурах (наприклад, Rectangle.ToSquare()), C# дозволяє створювати власні процедури перетворення, які дозволяють вашим типам реагувати на оператор приведення типів (). Отже, якщо ви правильно налаштували структури, ви зможете використовувати наступний синтаксис для явного перетворення між ними наступним чином:

```cs
// Convert a Rectangle to a Square!
Rectangle rect = new Rectangle
{
  Width = 3;
  Height = 10;
}
Square sq = (Square)rect;
```

## Створення власних процедур перетворення

C# надає два ключові слова, explicit та implicit, які можна використовувати для керування реакцією типів під час спроби перетворення. Припустимо, що у вас є такі визначення структури:

```cs

namespace CustomTypeConversions;

public struct Rectangle
{
    public int Width { get; set; }
    public int Height { get; set; }
    public Rectangle(int w, int h)
    {
        Width = w;
        Height = h;
    }
    public void Draw()
    {
        for (int i = 0; i < Height; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
        }
    }
    public override string ToString()
      => $"[Width = {Width}; Height = {Height}]";
}

public struct Square
{
    public int Length { get; set; }
    public Square(int l) : this()
    {
        Length = l;
    }
    public void Draw()
    {
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Length; j++)
            {
                Console.Write("*");
            }
            Console.WriteLine();
        }
    }
    public override string ToString() => $"[Length = {Length}]";
    // Rectangles can be explicitly converted into Squares.
    public static explicit operator Square(Rectangle r)
    {
        Square s = new Square { Length = r.Height };
        return s;
    }
}
```

Зверніть увагу, що тип Square визначає явний оператор перетворення. Як і процес перевантаження оператора, процедури перетворення використовують ключове слово operator у поєднанні з ключовим словом explicit або implicit і повинні бути визначені як static. Вхідний параметр – це сутність, з якої ви конвертуєте дані, тоді як тип оператора – це сутність, до якої ви конвертуєте дані. У цьому випадку припущення полягає в тому, що квадрат (геометричний візерунок, у якого всі сторони однакової довжини) можна отримати з висоти прямокутника.Таким чином, ви можете вільно перетворити прямокутник на квадрат наступним чином:

```cs
static void ConversionRectangleTopSquare()
{
    // Make a Rectangle.
    Rectangle r = new Rectangle(15, 4);
    Console.WriteLine(r.ToString());
    r.Draw();

    Console.WriteLine();

    // Convert r into a Square,
    // based on the height of the Rectangle.
    Square s = (Square)r;
    Console.WriteLine(s.ToString());
    s.Draw();
}
ConversionRectangleTopSquare();
```
```
[Width = 15; Height = 4]
***************
***************
***************
***************

[Length = 4]
****
****
****
```
Хоча перетворення прямокутника на квадрат у тій самій області видимості може бути не дуже корисним, припустимо, що у вас є функція, розроблена для отримання параметрів типу Square. 

```cs
    // This method requires a Square type.
    static void DrawSquare(Square sq)
    {
        Console.WriteLine(sq.ToString());
        sq.Draw();
    }
```
Використовуючи явну операцію перетворення типу Square, тепер ви можете передавати типи Rectangle для обробки за допомогою явного приведення типів, ось так:

```cs
static void ConversionRectangleTopSquare_1()
{
    // This method requires a Square type.
    static void DrawSquare(Square sq)
    {
        Console.WriteLine(sq.ToString());
        sq.Draw();
    }

    // Convert Rectangle to Square to invoke method.
    Rectangle rect = new Rectangle(10, 5);
    DrawSquare((Square)rect);

}
ConversionRectangleTopSquare_1();
```

## Додаткові явні перетворення для типу Square

Тепер, коли ви можете явно перетворювати прямокутники на квадрати, давайте розглянемо кілька додаткових явних перетворень. Враховуючи, що квадрат симетричний з усіх сторін, може бути корисним надати явну процедуру перетворення, яка дозволяє викликаючому коду перетворювати типи з цілочисельного типу на тип Square (який, звичайно, матиме довжину сторони, що дорівнює вхідному цілому числу). Аналогічно, що, якби ви оновили Square таким чином, щоб викликаюча функція могла перетворювати типи з типу Square на ціле число? Ось оновлення класу Square:

```cs
    public static explicit operator Square(int sideLength)
    {
        Square newSq = new Square { Length = sideLength };
        return newSq;
    }
    public static explicit operator int(Square s) => s.Length;
```
Ось логіка виклику:

```cs
static void ConversionRectangleTopSquare_2()
{
    // Converting an int to a Square.
    Square square = (Square)90;
    Console.WriteLine($"square = {square}");

    // Converting a Square to an int.
    int side = (int)square;
    Console.WriteLine($"Side length of square = {side}");
}
ConversionRectangleTopSquare_2();
```
```
square = [Length = 90]
Side length of square = 90
```
Чесно кажучи, перетворення з Square у ціле число може бути не найінтуїтивнішою (або корисною) операцією (зрештою, цілком ймовірно, що ви можете просто передати такі значення конструктору). Однак це вказує на важливий факт щодо користувацьких процедур перетворення: компілятору байдуже, що ви конвертуєте або з чого, якщо ви написали синтаксично правильний код.
Таким чином, як і у випадку з операторами перевантаження, те, що ви можете створити явну операцію приведення типів для заданого типу, не означає, що ви повинні це робити. Зазвичай, цей метод буде найбільш корисним під час створення структурних типів, враховуючи, що вони не можуть брати участь у класичному успадкуванні (де приведення типів здійснюється безкоштовно).

## Визначення процедур неявного перетворення

Заборонено визначати явні та неявні функції перетворення для одного типу, якщо вони не відрізняються типом повернення або набором параметрів. Це може здатися обмеженням; однак, другий нюанс полягає в тому, що коли тип визначає неявну процедуру перетворення, викликаюча сторона може використовувати синтаксис явного приведення типів! 
Додамо неявну процедуру перетворення до структури Rectangle, використовуючи ключове слово C# implicit (зверніть увагу, що наступний код припускає, що ширина результуючого прямокутника обчислюється шляхом множення сторони квадрата на 2):

```cs
public struct Rectangle
{
    //...
    public static implicit operator Rectangle(Square s)
    {
        Rectangle r = new Rectangle
        {
            Height = s.Length,
            Width = s.Length * 2 // Assume the length of the new Rectangle with (Length x 2).
        };
        return r;
    }
}
```
З цим оновленням ви тепер можете конвертувати між типами наступним чином:
```cs
static void Conversions()
{
    // Implicit cast OK!
    Square square = new Square { Length = 7 };
    Rectangle rectangle = square;
    Console.WriteLine($"rectangle = {rectangle}");
    
    // Explicit cast syntax still OK!
    square = new Square { Length = 3 };
    rectangle = (Rectangle)square;
    Console.WriteLine($"rectangle = {rectangle}");
}
Conversions();
```
```
rectangle = [Width = 14; Height = 7]
rectangle = [Width = 6; Height = 3]
```
Як і у випадку з перевантаженими операторами, пам'ятайте, що цей фрагмент синтаксису є просто скороченим записом для «звичайних» функцій-членів, і в цьому світлі він завжди необов'язковий. Однак, за правильного використання, власні структури можна використовувати більш природно, оскільки їх можна розглядати як справжні типи класів, пов'язані між собою успадкуванням.
