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

    static void PrintRectangle(double lenght)
    {
        Console.WriteLine(Rectangle());

        double Rectangle()
        {
            lenght += 1;
            return lenght * lenght;
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

    static void PrintRectangle(double lenght)
    {
        Console.WriteLine(Rectangle(lenght));

        static double Rectangle(double l) => l * l; 
  
    }
}
```


