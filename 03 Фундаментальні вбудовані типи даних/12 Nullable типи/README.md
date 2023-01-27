# 12 Nullable типи (nullable - що може бути null)

Додамо проект Nullables

Коли ви заповнюєте якусь форму наприклад для регістрації ви можете не заповнювати деякі необовязкові поля наприклад сімейний стан або вік. Це приводить до того що в базі данних є поля які не визначені абож null. В базах даним може бути багато мість де дані не визначенні тобто null.

Коли поле або об'єкт не визначенно і ми їx використовуем неналежним чином среда виконання викидує NullReferenceException і програма закінчує роботу. Оскільки виправлення помилок іноді дуже дороге задоволення було вирішено вже коли створюється код нагадувати розробнику що в цьому місті може виникнути преблема.

```cs
CrashWithNull();
static void CrashWithNull()
{

    Person girl = new(); 

    AgePlusOne(girl);//Ok 
    girl.Display();  //Ok  Name: Age:1 IsNull:False

    Person boy = null; //War: Converting null to no-nullable type. // Start problem
    AgePlusOne(boy); //War: may be null here  // Сontinuation of the problem

    Person maneger = new();
    maneger.Name = null; //War: Converting null to no-nullable type. // Start problem
    maneger.Name.ToUpper();//War: may be null here  // Exeption thrown Name == null

    static void AgePlusOne(Person p)
    {
        p.Age++; // Exeption thrown if p == null
    }
}


class Person
{
    public string Name { get; set; } //Here warning 
    public int Age { get; set; }
    public void Display() => Console.WriteLine($"Name:{Name} Age:{Age} IsNull:{this is null}");
}
```
Як ми бачимо з прикладу синтаксичний аналізатор показує де в коді можуть винукнути проблеми.


# Nullable і value типи.

Такі типи як int або bool представляють моножину значень. Наприклад int може приймати значеня від –2147483648 від 2147483647, а bool true або false. Всім зміним, типи які відносяться до типів значень(Value type) , не  можна призначити значення null. Значення null використовується для встановлення посилання на порожній об'єкт.


Можно зробити так щоб тип крім множини всіх своїх значень примав ще і значення null.
```cs
AssignNull();

static void AssignNull()
{
    //int age = null; //not-nullable //Cannot convert null to int because it is not-nullable value type.

    int? age; //nullable
    bool? married; //nullable

    age = GetAgeFromDB();
    married = GetMarriedFromDB();
    
    Console.WriteLine($"{age} {married}");


    static bool? GetMarriedFromDB()
    {
        return null;
    }

    static int? GetAgeFromDB()
    {
        return null;
    }
}
```

Коли ви визначили тип nullable bool (bool?) то змінна може приймати значеня true, false, null. І це важливо при отримані данних з бази данних. Іншого зручного способу представлення bool та числових данних без значення немає.

При використані ? використовуеться структура загальної структури System.Nullable<T>. 
```cs
UsingSystemNullable();
static void UsingSystemNullable()
{

    Nullable<bool> merried = null; 
    Nullable<int> age = null;
    Console.WriteLine("merried = null, age = null");
    Console.WriteLine($"merried.HasValue : {merried.HasValue}");
    Console.WriteLine($"age.GetValueOrDefault() : {age.GetValueOrDefault()}");
    Console.WriteLine($"merried is ValueType : {merried is ValueType}");
    Console.WriteLine($"age is ValueType : {age is ValueType}");

    merried = true;
    age = 25;

    Console.WriteLine("\nAfter: merried = true, age=25");
    Console.WriteLine($"merried.HasValue : {merried.HasValue}");
    Console.WriteLine($"age.GetValueOrDefault() : {age.GetValueOrDefault()}");
    Console.WriteLine($"merried is ValueType : {merried is ValueType}");
    Console.WriteLine($"age is ValueType : {age is ValueType}");
    Console.WriteLine($"merried.Value : {merried.Value}");
    Console.WriteLine($"age.Value : {age.Value}");


    //Nullable<Person> person = null; // Person must be non-nullable value type.
}

class Person
{
    public string Name { get; set; }
}
```
Результат:
```
merried = null, age = null
merried.HasValue : False
age.GetValueOrDefault() : 0
merried is ValueType : False
age is ValueType : False

After: merried = true, age=25
merried.HasValue : True
age.GetValueOrDefault() : 25
merried is ValueType : True
age is ValueType : True
merried.Value : True
age.Value : 25
```

## Використання nullable value типів

Змінні nullable типу використовують функціональність структури System.Nullable<T>.  

```cs
UsingNullables();
static void UsingNullables()
{
    UserDatabaseSimulator user = new UserDatabaseSimulator(1, "Julia");

    int? age = user.GetAge();

    if (age.HasValue)
    {
        Console.WriteLine(GetUserInfo(user)+age);   
    }
    else
    {
        Console.WriteLine(GetUserInfo(user)+"age is undefined");     
    }

    bool? merried = user.GetMerried();

    if (merried != null)
    {
        Console.WriteLine(GetUserInfo(user) + $"Merried:{merried}");
    }
    else
    {
        Console.WriteLine(GetUserInfo(user) + "merried is undefined");
    }
    static string GetUserInfo(UserDatabaseSimulator user) => $"{user.id} {user.name} ";
}

class UserDatabaseSimulator
{
    public int id;
    public string name; 
    public bool? merried = true;
    public int? age = null;

    public UserDatabaseSimulator(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public bool? GetMerried()
    {
        return merried;
    }

    public int? GetAge()
    {
        return age;
    }
}
```

Оскільки для необовязкових полів БД ми можемо получити як встановлені так і не встановленні значення за допомогю властивості HasValue та != null ми можимо корректно обробити данні.



## Nullable reference типи.

Починаючи з С# 10 включають nullable reference типи у всіх шаблонах за замовченням. Коли в коді виникає ситуація коли об'єкт може прийняти значенyя null і іде доступ до члену об'екту то тоді компілятор попереджає шо тут може виникнути виняток і програма закінчить працювати. 

