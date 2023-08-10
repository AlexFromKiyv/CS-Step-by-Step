# Вказівники (Pointers)

В .Net визначаються дві категорії даних : value та reference типи. Вказівники можна вважати третею категорію яка стоїть осторонь і не часто використвується.
Для роботи з вказівниками ви отримуєте спеціальні оператори та ключові слова які дозволяють обійти схему керування пам'яться .Net Runtime's і замі управляєте пам'ятью.
Ось перелік:

    * : Цей оператор використовується для сторення змінної-вказівника.(тобто змінної, яка представляє пряме розташування в пам'яті).

    & : Цей оператор використовується для отримання адреси змінної в пам'яті.

    -> : Цей оператор використовується для доступу до полів типу.(небезпечна версія оператора крапка)

    [] : Цей оператор дозволяє індексувати слот на який вказує змінна-вказівник.

    ++, -- ,- ,+ ,== ,> ,< ... : У небезпечному контексті оперетори можна застосовувати для типів показчиків.

    Stackalloc : У небезпечному контексті ключове слово можна використовувати для розмішеня масива С# безпосередьно в стек.

    Fixed : У небезпечному контексті ключове слово fixed можна використовувати для тимчасового виправлення змінної, щоб можна було знайти її адресу.

Можливо вам нікони не буде потрібно використовувати вказівники та є дві типові ституації коли вони потрібні:

    - Ви прагните оптімізувати вибрані частини шляхом безпосереднього маніпулювання пам'ятью.

    - Ви викликаєте С-основані метотоди з .dll або COM-сервера, якв вимагають типів вказівників як параметр. Навіть у цьому випадку ви часто можете обійти типи вказівників на користь типу System.IntPtr і членів типу System.Runtime.InteropServices.Marshal.

Коли ви працюете з вказівниками ви повині вказати компілятору про свохї наміри увімкнувши в проекті підтримку "unsafe code". В Visual Studio це можна зробити на сторінці властивостей проекту, на вкладці Build, та встановити властивість "Unsafe code". Ці дії додадуть в файл проекту рядок.

```xml
<AllowUnsafeBlocks>True</AllowUnsafeBlocks>
```
Коли ви маєте намір працювати з вказівниками ви повинні визначити блок коду за допомогою ключового слова unsafe. 
```cs
unsafe
{
    // Work with pointers.
}
```
Крім того за допомогою unsafe можна визначати структури і класи 

```cs
// This entire structure is 'unsafe' and can
// be used only in an unsafe context.
unsafe struct Node
{
  public int Value;
  public Node* Left;
  public Node* Right;
}
// This struct is safe, but the Node2* members
// are not. Technically, you may access 'Value' from
// outside an unsafe context, but not 'Left' and 'Right'.
public struct Node2
{
  public int Value;
  // These can be accessed only in an unsafe context!
  public unsafe Node2* Left;
  public unsafe Node2* Right;
}
```
Метод який працює з вказівниками теж може бути unsafe

```cs
void UseUnsafeMethod()
{
    static unsafe void SquareIntPointer(int* myIntPointer)
    {
        *myIntPointer *= *myIntPointer;
    }

    unsafe
    {
        int myInt = 10;
        SquareIntPointer(&myInt);
        Console.WriteLine(myInt);
    }

    int otherInt = 20; 

    //SquareIntPointer(&otherInt); // ...may only be used in an unsafe context
}

UseUnsafeMethod();
```
```
100
```
Можна визначити метод Main як unsafe тоді весь контекс буде таким і не тербе буде обгорати блоки коду.

## Робота з операторами * та &.

```cs
unsafe void UsePointersOperators()
{

    static unsafe void PrintValueAndAddress()
    {
        int myInt;

        // Define an int pointer, and
        // assign it the address of myInt.
        int* myIntPointer = &myInt;

        Console.WriteLine($"myInt: {myInt}");
        Console.WriteLine($"Adress: {(int)&myIntPointer:X}");

        // Assign value of myInt using pointer indirection.
        *myIntPointer = 123; 

        Console.WriteLine($"myInt: {myInt}");
        Console.WriteLine($"Adress: {(int)&myIntPointer:X}");
    }

    PrintValueAndAddress();
}

UsePointersOperators();
```
```
myInt: 0
Adress: D117E5E0
myInt: 123
Adress: D117E5E0
```

В прикладі створений покажчик на тип int за допомогою * і отримана адреса того на що вказує показчик за допомогою &. Тут весь метод позначено як unsafe тому в ному можна викликати інший unsafe метод.

Іноді можна уникнути використовування unsafe кода.

```cs
unsafe void SafeAndUnsafe()
{
    int i = 5, j = 10;
    PrintTwoInts(i, j);

    SafeSwap(ref i, ref j);
    PrintTwoInts(i, j);

    unsafe
    {
        UnSafeSwap(&i, &j);
    }
    PrintTwoInts(i, j);

    unsafe static void UnSafeSwap(int* x, int* y)
    {
        int temp = *x;
        *x = *y;
        *y = temp;
    }

    static void SafeSwap(ref int x, ref int y )
    {
        int temp = x;
        x = y;
        y = temp;
    }

    void PrintTwoInts(int x, int y)
    {
        Console.WriteLine($"x:{x} y:{y}");
    }

}

SafeAndUnsafe();
```
```
x:5 y:10
x:10 y:5
x:5 y:10
```
В цьому прикладі два методи виконують індентічні діє. Використання ref більш безпечне.

## Доступ до полів через вказівники. ->

Хай у нас є структура

```cs
    struct Point
    {
        public int x;
        public int y;
        public override string? ToString() => $"({x},{y})";

    }
```
```cs
unsafe void AccessToPropertyViaPointer()
{
    Point point = new();
    Console.WriteLine(point);
    Point* pointPointer = &point;
    pointPointer->x = 10;
    pointPointer->y = 20;
    Console.WriteLine(pointPointer->ToString());
 
    Console.WriteLine(*pointPointer);
    
    (*pointPointer).x = 100;
    (*pointPointer).y = 200;
    Console.WriteLine(point);

}

AccessToPropertyViaPointer();
```
```
(0,0)
(10,20)
(10,20)
(100,200)


```
Якшо ми маємо вказівник на складний тип ми можемо звертатися до властивостей і методів за допомогою оператора ->. Але використовуючи оператор непрямого вказівника (*) можна отримати об'єкт и працювати з опреатором крапка.

## Stackalloc

```cs
unsafe void UseStackalloc()
{
    Console.WriteLine(UnsafeStackAlloc());

    static unsafe string UnsafeStackAlloc()
    {
        char* p = stackalloc char[58];
        for (int k = 0; k < 58; k++)
        {
            p[k] = (char)(k + 65);
        }
        return new string(p);
    }
}

UseStackalloc();
```
```
ABCDEFGHIJKLMNOPQRSTUVWXYZ[\]^_`abcdefghijklmnopqrstuvwxyz
```
У небезпечному контексті вам може знадобитися оголосити локальну змінну, яка виділяє пам’ять безпосередньо зі стеку викликів (і, отже, не підлягає збиранню сміття .NET).Для цього C# надає ключове слово stackalloc.

## Закріплення типу за допомогою Fixed.

За природою пам'ять в стеку очишуеться при возвраті методу. Припустимо у нас є тип reference type PointRef.

```cs
    class PointRef
    {
        public int x;
        public int y;
        public override string? ToString() => $"({x},{y})";
    }
```
Змінна такого типу зберігаеться в керованй купі. Якшо некерований код захоче працювати з таким об'ектом може винукнити ситуація коли збирач смітя вже очистив цю пам'ять. 
Щоб заблокувати змінну reference type в пам'яті викристовується fixed.

```cs
unsafe static void UseFixed()
{
    PointRef point = new PointRef { x = 1, y = 2 };


    // Pin point in place so it will not
    // be moved or GC-ed.
    fixed (int* pointerOnX = &point.x)
    {
        Console.WriteLine(point.x);
        point.x = 2;
        *pointerOnX = 3;
    }
    // point is now unpinned, and ready to be GC-ed once
    // the method completes.
    Console.WriteLine(point);
}

UseFixed();
```
```
1
(3,2)
```
Інструкція fixed встановлює вказівник на керований тип і "закріалює" цю змінну під час виконання коду. Якшо не фіксувати об'екти з некерованого коду результат буде непередбачуваним.
Ключове слово fixed дозволяє створити оператор, який блокує посилальну змінну в пам’яті, щоб її адреса залишалася постійною протягом терміну дії оператора (або блоку видимості). Щоразу, коли ви взаємодієте з типом посилання в контексті небезпечного коду, закріплення посилання є обов’язковим.

## sizeof

Це ключове слово використовується для отримання  розмір в байтах внутрішнього типу і отримання розміру спеціального типа в "unsafe" контексті.

```cs
void UseSizeOf()
{
    Console.WriteLine(sizeof(short));
    Console.WriteLine(sizeof(int));
    Console.WriteLine(sizeof(decimal));
}

UseSizeOf();
```
```
2
4
16
```
В цьому прикладі не потрібен небезпечний контекст, оскільки всі типи внутрішні.

```cs
unsafe void UseSizeOfForYourTypes()
{
    unsafe
    {
        Console.WriteLine(sizeof(PointRef));
    }
}

UseSizeOfForYourTypes();
```
```
8
```
Коли ви хочете отримати розмір вашої спеціальної структури вам потрибен блок unsafe.


