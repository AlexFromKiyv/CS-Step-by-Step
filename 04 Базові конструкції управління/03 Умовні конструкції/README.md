Додамо проект під назвою If

## Логічні опертори та вирази.

```CS
LogicalExpression();

static void LogicalExpression()
{
    int weight = 70;

    Console.WriteLine($"weight = {weight}");
    Console.WriteLine($"weight == 70 : {weight == 70}");
    Console.WriteLine($"weight != 70 : {weight != 70}");
    Console.WriteLine($"weight > 70  : {weight > 70}");
    Console.WriteLine($"weight < 70  : {weight < 70}");
    Console.WriteLine($"weight >= 70  : {weight >= 70}");
    Console.WriteLine($"weight <= 70  : {weight <= 70}");
    
    Console.WriteLine("------------------------------");

    Console.WriteLine($" true && true   : {true && true }");
    Console.WriteLine($" true && false  : {true && false}");
    Console.WriteLine($" false && true  : {false && true}");
    Console.WriteLine($" false && false : {false && false}");

    Console.WriteLine("------------------------------");

    Console.WriteLine($" true || true   : {true || true}");
    Console.WriteLine($" true || false  : {true || false}");
    Console.WriteLine($" false || true  : {false || true}");
    Console.WriteLine($" false || false : {false || false}");

    Console.WriteLine("------------------------------");

    Console.WriteLine($" !true  : {!true}");
    Console.WriteLine($" !false : {!false}");

    Console.WriteLine("------------------------------");

    Console.WriteLine($"false || true && true || true && false || true : {false || true && true || true && false || true }");
}
```
Оператори && || не перевіряють вираз до кінця якшо результат визначен. Аби превірити весь вираз існують  & |. 


## IF/ELSE

```cs
SimpleIf();

static void SimpleIf()
{
    bool logicalExpression = true;
    if (logicalExpression)
    {
        Console.WriteLine("Logical expression is true");
    }

}


SimpleIfElse();

static void SimpleIfElse()
{
    bool logicalExpression = false;
    if (logicalExpression)
    {
        Console.WriteLine("Logical expression is true");
    }
    else
    {
        Console.WriteLine("Logical expression is false");
    }
}
```
## IS

```cs
UsingIs();
static void UsingIs()
{
    string myVariable1 = "Hi";

    if (myVariable1 is string newString)
    {
        Console.WriteLine(newString);
    }
    
    int myVariable2 = 70;
    if (myVariable2 is int newInt)
    {
        Console.WriteLine(newInt);
    }
}
```

В цьому прикладі перевіряеться тип об'єкту і якщо умова виконується присваюється новій змінній для того шоб потім використовувати.



## Matching patterns.

```cs
PatternMatchingWithIf();
void PatternMatchingWithIf()
{
    Console.WriteLine(GetSquare(10));
    Console.WriteLine(GetSquare("Hi"));

    int GetSquare(object o)
    { 
        if (o is int value)
        {
            return value * value;
        }
        else
        {
            return -1;
        }
    }
}
```


```cs
UsingTypePattern();
static void UsingTypePattern()
{
    Type myType = typeof(short);

    if(myType is Type)
    {
        Console.WriteLine($"{myType} is type.");
    }

}
```
Перевірка чи є зміна тип.

```cs
UsingParenthesizedPattern();
static void UsingParenthesizedPattern()
{
    char myChar = 'y';

    if (myChar is (>= 'a' and <= 'z') or (>= 'A' and <= 'Z') or '.' or ',' or ';')
    {
        Console.WriteLine($"{myChar} is a character or separator");
    }
}
```

```cs
UsingRelationalConjuctiveDisjunctivePattern();
static void UsingRelationalConjuctiveDisjunctivePattern()
{
    char myChar = 'y';

    if (myChar is >= 'a' and <= 'z' or >= 'A' and <= 'Z')
    {
        Console.WriteLine($"{myChar} is a character");
    };
}
```
```cs
UsingNegativePattern();
static void UsingNegativePattern()
{
    object myObject = 0;
    if (myObject is not string) 
    {
        Console.WriteLine($"{myObject} not string");
    }

    if (myObject is not null)
    {
        Console.WriteLine($"{myObject} not null");
    }
}
```

## Оператор ?

```cs
UsingConditionalOperator();

static void UsingConditionalOperator()
{
    int weight = 95;

    string result = (weight < 72) ? "It's good" : "It's no good";

    Console.WriteLine(result);

    // do not work
    //(weight < 72) ? Console.WriteLine("It's good") : Console.WriteLine("It's no good");
}

```
Для виразності цього оператору я логічний вираз поміщаю в дужки. Якшо умова виконується виконується код за ? , інакше виконується код за : .

```cs
UsingConditionalOperatorWithRef();

static void UsingConditionalOperatorWithRef()
{
    int[] smallArray = new int[] { 1, 2, 3, 4, 5 };
    int[] largeArray = new int[] { 10, 20, 30, 40, 50, 60, 70 };

    int index = 7;
    ref int refValue = ref ((index < 5) ? ref smallArray[index] : ref largeArray[index - 5]);
    refValue = 0;

    Console.WriteLine(string.Join(" ",largeArray));

    index = 2;
    ((index < 5) ? ref smallArray[index] : ref largeArray[index - 5]) = 100;
    Console.WriteLine(string.Join(" ", smallArray));

}

```
В цьому прикладі резульатом роботи оператору є посилання на єлемент масиву. І це посилання можно використовувати не зберігаючи в окремій змінній. 

# Switch у простому варіанті.

Додамо проект під назвою Switch

```cs

SimpleSwitch();
static void SimpleSwitch()
{
    Console.WriteLine("Choose: ");
    Console.WriteLine("1. tea");
    Console.WriteLine("2. coffee");
    Console.WriteLine("3. water");
    Console.Write("Enter your varient:");
    
    int.TryParse(Console.ReadLine(), out int enteredVariant);

    string result;
    switch (enteredVariant)
    {
        case 1: result = "I'm making tea";
            break;
        case 2:
            result = "I'm making coffee";
            break;
        case 3:
            result = "I'm pouring water";
            break;
        default: result = "Invalid input";
            break;
    }
    Console.WriteLine(result);
}

```

Кожен case має закінчуватися breake, return або go to. Інакше він буде преходити до наступного case.

```cs
SimpleSwitchWithString();
static void SimpleSwitchWithString()
{
    Console.WriteLine("Choose: ");
    Console.WriteLine("tea");
    Console.WriteLine("coffee");
    Console.WriteLine("water");
    Console.Write("Enter your varient:");

    string enteredVariant = (Console.ReadLine()).ToLower(); Console.Clear();

    string result;
    switch (enteredVariant)
    {
        case "tea":
            result = "I'm making tea";
            break;
        case "coffee":
            result = "I'm making coffee";
            break;
        case "water":
            result = "I'm pouring water";
            break;
        default:
            result = "Invalid input";
            break;
    }
    Console.WriteLine(result);
}
```
Можна використовувати return

```cs
SwitchWithReturn();

void SwitchWithReturn()
{
    Console.WriteLine(DoAction(1,1,1));

    int DoAction(int actionCode, int a, int b)
    {
        switch (actionCode)
        {
            case 1: return a + b;
            case 2: return a - b;
            case 3: return a * b;
            default: return 0;
        }
    }
}
```

Оператор switch може порівнювати типи char, string, bool, int, long, та enum. 

```cs
SwitchWithEnum();
static void SwitchWithEnum()
{
    Console.Write("Enter day of the week:");

    DayOfWeek dayOfWeek;

    try
    {
        dayOfWeek = (DayOfWeek) Enum.Parse(typeof(DayOfWeek),Console.ReadLine());
    }
    catch (Exception)
    {
        Console.WriteLine("Bad input!");
        return;
    }
    Console.Clear();

    string result;

    switch (dayOfWeek)
    {
        case DayOfWeek.Sunday: result = "Long walk.";
            break;
        case DayOfWeek.Monday: 
            result = "Morning exercise only";
            break;
        case DayOfWeek.Tuesday: 
            result = "Run";
            break;
        case DayOfWeek.Wednesday: 
            result = "Morning exercise only";
            break;
        case DayOfWeek.Thursday: 
            result = "Run";
            break;
        case DayOfWeek.Friday:
            result = "Long walk.";
            break;
        case DayOfWeek.Saturday:
            result = "Morning exercise only";
            break;
        default:
            result = "Bad input!";
            break;
    }
    Console.WriteLine(result);
}
```
Так можна використовувати Enum

```cs
        case DayOfWeek.Saturday:
        case DayOfWeek.Sunday:
            result = "On weekend long walk.";
            break;
```
Якшо результат декількох варіантів має однаковий результат їx можна об'єднати як тут Saturday,Sunday


## Pattern Matching (шаблон зіставлення) в Switch

У простому варіанті switch співставляє значення з константами і називають constant pattern (шаблон констант). Але оператор може оцінювати тип (type pattern) і case не обмежується постійними значеннями.

```cs
PatternMatchingInSwitch();
static void PatternMatchingInSwitch()
{
    object inputHeight;

    //inputHeight = "176";
    //inputHeight = 176;
    //inputHeight = 176M;
    inputHeight = 176.5;


    switch (inputHeight)
    {
        case string stringHeight:
            Console.WriteLine("We receive string "+stringHeight); 
            break;
        case double doubeHeight:
            Console.WriteLine(MaxGoodWeight(doubeHeight));
            break;
        //case double doubeHeight when doubeHeight > 0: //Compile error due to previos line 
        //    Console.WriteLine("It's no good.");
        //    break;
        default:
            Console.WriteLine($"We recive {inputHeight.GetType()} {inputHeight}");
            break;
    }
    
    static double MaxGoodWeight(double height)
    {
        return (height / 100) * (height / 100) * 24.9;
    }

}
```

У данному випадку switch зіставляє тип отриманого значення з типом case. Крім того змінній відповідного типу присваюється значення і додадково превіряеться (when). Але ця змінна недосяжна за оператором case.
В випадку type pattern порядок case має значення. Визначеня  case double doubeHeight: прекриває інши варіанти з when.

## Switch expression

```cs
UsingSwitchExpression();
static void UsingSwitchExpression()
{
    string stringColor = "Green";

    string color = stringColor switch
    {
        "Red"  => "#FF0000",
        "Green"=> "#00FF00", 
        "Blue" => "#0000FF",
        _      => "#000000"
    };

    Console.WriteLine(color);
} 
```
Switch вираз досить зрозуміло і компактно.

```cs
UsingSwitchExpressionWithTuple();

static void UsingSwitchExpressionWithTuple()
{
    string result = (1,1) switch
    {
        (1, 0) => "ON 0",
        (0, 1) => "ON 1",
        (1, 1) => "ON",
        _      => "OFF"
    };

    Console.WriteLine(result);
}
```
Так можна використовувати в switch кортежі.

Додадкові варіанти використання swith:

Об'єктно-орієнтоване програмування > Правила приведення типів > Співставленя типів в switch