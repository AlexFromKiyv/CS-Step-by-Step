# Методи

## Створення

Створимо рішеня Methods з проектом Methods.

При створенні методів вказуеться модіфікатор доступу, тип поверненя, та параметри. Методи які повертають значення називають функціями.

```
(Модіфікатор доступу) (Тип поверненя) (Назва) (Параметри)  

static  void MyMethod (string[] args)

```

```cs
SimpleMethod();
static void SimpleMethod()
{
    double height = 176;

    double result = MaxGoodWeight(height);

    Console.WriteLine(result);


    static double MaxGoodWeight(double height)
    {
        return (height / 100) * (height / 100) * 24.9;
    }
}
```

Для простих функцій можна використовувати лямбда вираз.

```cs
SimpleMethodWithLambda();

static void SimpleMethodWithLambda()
{
    Console.WriteLine(MaxGoodWeight(176));

    static double MaxGoodWeight(double height) => (height / 100) * (height / 100) * 24.9;
}

```
## Локальні функції

Функцію яка задекларована в іншій функції називають локальною. Вана має бути або private або static.
Для локальних функцій не підтримується перезавантаження.

```cs
SimpleMethodWithValidation();
static void SimpleMethodWithValidation()
{
    for (int height = 164; height < 192; height+=2)
    {
        Console.WriteLine(MaxGoodWeightWithValidation(height));
    }
   
    //Console.WriteLine(MaxGoodWeight(320)); it do not work

    static string MaxGoodWeightWithValidation(double height)
    {

        if (height > 130 && height < 280 )
        {
            return $"Max good weight for {height} cm is {MaxGoodWeight(height)} "; 
        }
        else
        {
            return $"Bad input height {height} ";
        }

        // Local function
        static double MaxGoodWeight(double height) => (height/100)*(height/100)*24.9;
        
    }
}
```
Локальні функції досяжні в межах іншої функції де вони створені. Локальним функціям можна додавати атрібути наприклад #nullable enable

```cs
BadNoStaticLocalFunction();

static void BadNoStaticLocalFunction()
{
    PrintRectangle(1);

    static void PrintRectangle(double length)
    {
        Console.WriteLine(Rectangle());

        double Rectangle()
        {
            length += 1;
            return length * length;
        }
    }
}

```

Якшо потрібно щоб локальна функція не змінювала параметрів головної функції напряму її треба робити статичною.

```cs
StaticLocalFunction();
static void StaticLocalFunction()
{
    PrintRectangle(1);

    static void PrintRectangle(double length)
    {
        Console.WriteLine(Rectangle(length));

        static double Rectangle(double l) => l * l; 
  
    }
}
```

## Value та Reference типи.





## Параметри 

Параметри це данні яку передаються методу. Якшо параметр не має модіфікаторів то за замовченням в метод надсилаеться копія данних. В залежності від модіфікаторів парамтери можуть обробляться методами по різному.

## Параметри без модіфікаторів. 

Якшо параметр value type і не має модіфікаторів то в метод передаеться копія данних.

```cs
ValueTypeWithoutModifier();

static void ValueTypeWithoutModifier()
{
    int length = 2;

    Console.WriteLine(Rectangle(length));

    static int Rectangle(int l)
    {
        Console.WriteLine(l is ValueType);
       
        int result = l * l;
        
        return result;
    }

}

```
Оскільки int параметр l є ValueType при виконанні для нього в стеку функції створюєця місце куди записуеться значеня з яким визиваеться функція. Тобто переписується значеня яке храниться в length. Коли функія відпрацювала місце в стеку звільняєця. Функція ні як не впливає на зовнішню змінну length.

## out параметри.

```cs
UsingOutModifier_1();
static void UsingOutModifier_1()
{
    int enterlength = 10;

    Rectangle(enterlength, out int rectangle);

    static void Rectangle(int length, out int result)
    {
        result = length * length;
    }


    Console.WriteLine($"{enterlength} * {enterlength} = {rectangle}");


    int newRectangle;

    Rectangle(enterlength, out newRectangle);

    Console.WriteLine(newRectangle);
}
```
Змінну в якості параметра можна створювати при визові функції. В тілі функції обовязково треба присоїти їй значення. При передачі існуючої змінної треба використовувати out і її значення після визову змінеться. 
```cs
UsingOutModifier_2();
static void UsingOutModifier_2()
{
    int enterlength = 10;

    static void RectangleAndVolume(int length,out bool isPositive , out int rectangle, out int volume)
    {
        isPositive = length > 0;
        rectangle = length * length;
        volume = length * length * length;
    }

    RectangleAndVolume(enterlength, out bool isPositive, out int rectangle, out int volume);

    Console.WriteLine($"{enterlength} isPositive:{isPositive} rectangle:{rectangle}, volume:{volume}");

    RectangleAndVolume(5, out _, out _, out int newVolume);

    Console.WriteLine(newVolume);

}
```
Дійсна користь від оператора полягає шо він дозволяє одній функції вертати декілька параметрів. Якшо вам не потрібні деякі значення ви можете відкинути ЇЇ за допомогою out _. _ - це фіктивна змінна яка навмисно не використовується.






