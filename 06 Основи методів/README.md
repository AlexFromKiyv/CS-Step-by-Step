# Методи

Створимо рішеня Methods з проектом Methods.

## Створення


При створенні методів вказуеться модіфікатор доступу, тип поверненя, та параметри. Методи які повертають значення називають функціями.

```
(Модіфікатор доступу) (Тип поверненя) (Назва) (Параметри)
{
  складові дії методу
}  

static  void MyMethod (string[] args)
{
   // do something

}

```
Назви змінних зазвичай прописують в "Title case". Наприклад: DoIt, ReverseList.

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

## Лямбда вирази.

```cs
SimpleMethodWithLambda();

static void SimpleMethodWithLambda()
{
    Console.WriteLine(MaxGoodWeight(176));

    static double MaxGoodWeight(double height) => (height / 100) * (height / 100) * 24.9;
}

```
Після => вказуеться що повертає функція. Лямбда вирази корисні коли треба виконати невелике завдання.

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
lenght
```cs
BadNoStaticLocalFunction();

static void BadNoStaticLocalFunction()
{
    PrintQuadrate(1);

    static void PrintQuadrate(double length)
    {
        Console.WriteLine(Quadrate());

        double Quadrate()
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
    PrintQuadrate(1);

    static void PrintQuadrate(double length)
    {
        Console.WriteLine(Quadrate(length));

        static double Quadrate(double l) => l * l; 
  
    }
}
```

## Параметри 

Параметри це данні яку передаються методу. Якшо параметр не має модіфікаторів то за замовченням в метод надсилаеться копія данних. В залежності від модіфікаторів парамтери можуть обробляться методами по різному. Значення які предаюця в метод називають аргументами або фактичними параметрами.

## Параметри без модіфікаторів. 

Якшо параметр value type і не має модіфікаторів то в метод передаеться копія данних.

```cs
ValueTypeWithoutModifier();

static void ValueTypeWithoutModifier()
{
    int length = 2;

    Console.WriteLine(Quadrate(length));

    static int Quadrate(int l)
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

    Quadrate(enterlength, out int quadrate);

    static void Quadrate(int length, out int result)
    {
        result = length * length;
    }


    Console.WriteLine($"{enterlength} * {enterlength} = {quadrate}");


    int newQuadrate;

    Quadrate(enterlength, out newQuadrate);

    Console.WriteLine(newQuadrate);
}
```
Змінну в якості параметра можна створювати при визові функції. В тілі функції обовязково треба присоїти їй значення. При передачі існуючої змінної треба використовувати out і її значення після визову змінеться. 
```cs
UsingOutModifier_2();
static void UsingOutModifier_2()
{
    int enterlength = 10;

    static void QuadrateAndVolume(int length,out bool isPositive , out int quadrate, out int volume)
    {
        isPositive = length > 0;
        quadrate = length * length;
        volume = length * length * length;
    }

    QuadrateAndVolume(enterlength, out bool isPositive, out int quadrate, out int volume);

    Console.WriteLine($"{enterlength} isPositive:{isPositive} quadrate:{quadrate}, volume:{volume}");

    QuadrateAndVolume(5, out _, out _, out int newVolume);

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

## Посилання як результат методу.

```cs
void ReferenceAsResult()
{
    int[] ints = { 3, 4, 5, 6 };


    ref int referenceOfItem = ref Find(5, ints);

    referenceOfItem = 100;

    foreach (var item in ints)
    {
        Console.WriteLine(item);
    }

    ref int Find(int number, int[] numbers)
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            if (numbers[i] == number)
            {
                return ref numbers[i];
            }
        }
        throw new ArgumentException("Invalid input parameter.");
    }

}

ReferenceAsResult();
```
```
3
4
100
6
```
В цьому прикладі показано як можна використати зміну яка є посиляннам аби знайти елемент массиву та змінити цей елемент.


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

## Перезавантаженя методів (method overloading)

Є можливість створити декілька методів з однією назвою але з різною кількістю або типом параметрів. Таким чином перезавантажуються методи. Локальни функції не підтримують презавантаженя.

```cs
UsingOverload();
static void UsingOverload()
{
    int myInt = 10;
    double myDouble = 5.23;
    decimal myDecimal = 1000.2356M;
    float myFloat = 100.12F;
    long myLong = 100000000L;

    Console.WriteLine(Quadrate.GetQuadrate(myInt));
    Console.WriteLine(Quadrate.GetQuadrate(myDouble));
    Console.WriteLine(Quadrate.GetQuadrate(myDouble,2));
    Console.WriteLine(Quadrate.GetQuadrate(myDecimal));
    Console.WriteLine(Quadrate.GetQuadrate(myDecimal,2));
    Console.WriteLine(Quadrate.GetQuadrate(myFloat));
    Console.WriteLine(Quadrate.GetQuadrate(myLong)); 

}

static class Quadrate
{
    internal static string GetQuadrate(int lenght)
    {
        Console.WriteLine("I choose method 1");
        return (lenght * lenght).ToString(); 
    }
    internal static string GetQuadrate(double lenght)
    {
        Console.WriteLine("I choose method 2");
        return (lenght * lenght).ToString();
    }
    internal static string GetQuadrate(decimal lenght)
    {
        Console.WriteLine("I choose method 3");
        return (lenght * lenght).ToString();
    }
    internal static string GetQuadrate(double lenght, int accuracy = 2)
    {
        Console.WriteLine("I choose method 4");

        return double.Round(lenght * lenght, accuracy).ToString();
    }
    internal static string GetQuadrate(decimal lenght, int accuracy  = 2)
    {
        Console.WriteLine("I choose method 5");
        return decimal.Round(lenght * lenght, accuracy).ToString();
    }

    internal static string GetQuadrate(long lenght)
    {
        Console.WriteLine("I choose method 6");
        return GetQuadrate((decimal)lenght);
    }

}
```
Використовуючи превантаження ви можете використови одне і теж ім'я для методів які роблять одне й тесаме але для параметрів різних типів. Якшо методи відрізняються лише типом повертання то цього не достньо для превантаженя методу. 

Коли ви вели частину коду Console.WriteLine(Quadrate.GetStringQuadrate( у вас єможливість побачити варіанти первантаженя.

Аби не було непорозуміня не варто створювати методи які відрізняються лише необов'язковим параметрами, оскілки буде 2 місця для покращеня методу.

```cs

string GetQuadrate(ref int lenght) // don't work
string GetQuadrate(out int lenght) //

string GetQuadrate(ref int lenght) // work
string GetQuadrate(int lenght)     // 

```

## Перевірка на null

Коли метод отримує параметр типу reference то він може бути null. 
```cs
CheckParameterForNull();

static void CheckParameterForNull()
{
    //SendMessageBad(null);
    //SendMessageLargeCheck(null);
    SendMessageShortSheck(null);


    static void SendMessageBad(string message)
    {
        Console.WriteLine(message.Length);
    }

    static void SendMessageLargeCheck(string message)
    {
        if (message == null)
        {
            throw new ArgumentNullException(message);
        }
        Console.WriteLine("Send:"+message);
    }

    static void SendMessageShortSheck(string message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Console.WriteLine("Send:" + message);

    }
}
```

## Документування методу.

Додамо в проект Methods class Program.MyMath.cs

```cs
partial class Program
{
    static int RoundedSquare(double x,double y)
    {
        return (int) Math.Round(x*y);
    }
}
```

Для того аби задокументувати що робить фунуція над її назвою введіть /// та заповніть шаблон.

```cs
    /// <summary>
    /// Calculates the area for a rectangle with dimensions x, y
    /// </summary>
    /// <param name="x">length</param>
    /// <param name="y">width</param>
    /// <returns>The area of a rectangle with length and width rounded to a whole value.</returns>
    static int RoundedSquare(double x,double y)
    {
        return (int) Math.Round(x*y);
    }

```
Додайте в файл Program.cs перед визначенням класів
```cs
Documenting();
void Documenting()
{
    Console.WriteLine(RoundedSquare(10.23,12.34));
}
```
Зверніть увагу що при наведені курсора на функції з'являеться підказка.

