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

Для досить простих функцій можна використовувати лямбда вираз.

```cs
SimpleMethodWithLambda();

static void SimpleMethodWithLambda()
{
    Console.WriteLine(MaxGoodWeight(176));

    static double MaxGoodWeight(double height) => (height / 100) * (height / 100) * 24.9;
}

```
