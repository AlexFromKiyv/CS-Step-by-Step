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

## ref параметри.

```cs

UsingRefModifier();

static void UsingRefModifier()
{
    int x = 5;
    int y = 8;

    Console.WriteLine($"Before:  x:{x} y:{y}");

    SwapInt(ref x,ref y);

    Console.WriteLine($"After:   x:{x} y:{y}");

    SwapInt(ref x, ref y);

    Console.WriteLine($"After:   x:{x} y:{y}");

    static void SwapInt(ref int a, ref int b)
    {
        if (a < b)
        {   int t = b;
            b = a;
            a = t;
        }
    }

    string str1 = "Bye";
    string str2 = "Hi";

    Console.WriteLine("Before: " + str1 + " " + str2);

    SwapStr(ref str1, ref str2);
    
    Console.WriteLine("After: " + str1 + " " + str2);

    static void SwapStr(ref string a, ref string b)
    {
        string stringTemp = b;
        b = a;
        a = stringTemp;
    }
}
```

При використовувані ref модіфікатора параметри повині бути ініціалізовані до визову функції. Функція впливає на зміні шо за її межами і параметри передаються як посилання на існуючу в пам'яті змінну.

## in параметри.

Модіфікатор in для параметрів передае значення за посиланям і не дозволяє методу його змінювати.
Цей модіфікатор корисний коли в якості параметра передається наприклад велика структура яку не треба змінювати і коли копіювання без модіфікатора затримує процесс. Крім того при передачі reference типів ви можете змінити данні в методі і модіфікатор in рішає цю проблему.

```cs
UsingInModifier();

static void UsingInModifier()
{
    string greeting = "Welcome to paradise!";

    Console.WriteLine(greeting is ValueType); //False

    Console.WriteLine($"Before:{greeting}");
    ChangeWay(greeting);
    Console.WriteLine($"After:{greeting}");

    static void ChangeWay(string greetingString)
    {
        greetingString = "Welcom to hell!";
    }

    Console.WriteLine($"Before:{greeting}");
    ChangeWayWithIn(greeting);
    Console.WriteLine($"After:{greeting}");


    static void ChangeWayWithIn(in string greetingStreeng)
    {
        //greetingStreeng = "Welcom to paradise!"; //it don't work
        // using greetingString 
        Console.WriteLine(greetingStreeng.Length);
    }
}
```
Хоча тип string не є ValueType в методи без модіфікаторів предається значення. Вказуючи модіфікатор in ви даєте зрозуміти шо цей параметр не буде змінюватися.

# params модіфікатор.

Цей модіфікатор дозволяє передати в метод змінну кількість параметрів одного типу як одне ціле.

```cs
UsingParamsModifier();

static void UsingParamsModifier()
{
    Console.WriteLine(GetSum());

    Console.WriteLine(GetSum(1));

    Console.WriteLine(GetSum(1,2,3,4));

    double d = 7.34; 

    Console.WriteLine(GetSum(1.2,3.3,4.5,d));

    double[] myDoubleArray = new double[] {4,5,6.7};

    Console.WriteLine(GetSum(myDoubleArray));


    static double GetSum(params double[] values)
    {
        double sum = 0;

        if (values.Length > 0)
        {
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }
        }
        return sum;
    }
}
```
При попадані всі параиетри предані в метод потрапляють в массив. Щоб уникнути неоднозначність праметр з модіфікатором params повиниен бути тільки один і у кінці всіх інших.

## Необов'язкові параметри.

```cs
UsingOptionalPatameters();
static void UsingOptionalPatameters()
{
    Console.WriteLine(GetStringTemperature(20));
    Console.WriteLine(GetStringTemperature(68,"F"));

    static string GetStringTemperature(double temperature,string scale = "C")
    {
        return temperature.ToString() + "°" + scale;
    }

    //static string GetStringTemperatureWithDateTime(double temperature, 
    //    string scale = "C", 
    //    DateTime dateTime = DateTime.Now) // it don't work
    //{
    //    return temperature.ToString() + "°" + scale + dateTime.ToString() ;
    //}

}
```
Параметри за замовчуванням повині бути визначені під час компіляції. 

## Іменовані параметри.

```cs
UsingNamedParameters();
static void UsingNamedParameters()
{

    Volume(length: 1, height: 3, width: 2);


    static void Volume(int length, int width , int height)
    {
        Console.WriteLine($"Lenght:{length} Width:{width} Height:{height}" );
        Console.WriteLine(length*width*height);    
    }

  
    Console.WriteLine(GetStringTemperature(temperature:20));
  
    static string GetStringTemperature(string scale = "C",double temperature = 0)
    {
        return temperature.ToString() + "°" + scale;
    }
}
```
Таким чином не об'язково дотримуватися порядку параметрів у методі при визові. Оператор : присваює значення необхідному параметру. Змішаний варіант виклику потребує аби позиційні параматри були перед іменованими або знаходилися в правільному місті.


