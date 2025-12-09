# 12 Nullable типи (nullable - що може бути null)

Додамо проект Nullables

Коли ви заповнюєте якусь форму наприклад для регістрації ви можете не заповнювати деякі необовязкові поля наприклад сімейний стан або вік. Це приводить до того що в базі данних є поля які не визначені абож null. В базах даним може бути багато мість де дані не визначенні.

Коли поле або об'єкт не визначенно і ми їx використовуем неналежним чином среда виконання викидує NullReferenceException і програма закінчує роботу. Оскільки виправлення помилок іноді дуже дороге задоволення було вирішено вже коли створюється код нагадувати розробнику що в цьому місті може виникнути преблема. В prodaction коді для всіх мість де винакає вірогідність винятку треба додавати код перевірки. 

```cs
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
CrashWithNull();


class Person
{
    public string Name { get; set; } //Here warning 
    public int Age { get; set; }
    public void Display() => Console.WriteLine($"Name:{Name} Age:{Age} IsNull:{this is null}");
}
```
```
Name: Age:1 IsNull:False
Unhandled exception. System.NullReferenceException: Object reference not set to an instance of an object.
   at Program.<<Main>$>g__AgePlusOne|0_1(Person p) in D:\Temp\MySolution\MyProject\Program.cs:line 18
   at Program.<<Main>$>g__CrashWithNull|0_0() in D:\Temp\MySolution\MyProject\Program.cs:line 10
   at Program.<Main>$(String[] args) in D:\Temp\MySolution\MyProject\Program.cs:line 21
```

Як ми бачимо з прикладу синтаксичний аналізатор показує де в коді можуть винукнути проблеми. Для того і було створено nullable-контекс в усіх файлах проекту за замовченням.


# Nullable і value типи.

Такі типи як int або bool представляють моножину значень. Наприклад int може приймати значеня від –2147483648 від 2147483647, а bool true або false. Всім зміним, типи які відносяться до типів значень(Value type) , не  можна призначити значення null. Значення null використовується для встановлення посилання на порожній об'єкт.

```cs
int weight = null; // don't work //Cannot convert null to int because it is not-nullable value type.
```

Можно зробити так щоб тип крім множини всіх своїх значень примав ще і значення null.
```cs
static void AssignNull()
{
    //int weight = null; // don't work //Cannot convert null to int because it is not-nullable value type.
    //string title = null; // work, but has warning

    string title = "User:";

    int? age;      //nullable
    bool? married; //

    age = null;     // no problem
    married = null; //


    age = GetAgeFromDB();
    married = GetMarriedFromDB();
    
    Console.WriteLine($"{title} {age} {married}");

    static bool? GetMarriedFromDB()
    {
        return null;
    }
    static int? GetAgeFromDB()
    {
        return null;
    }
}
AssignNull();
```
```
User:
```

Коли ви визначили тип nullable bool (bool?) то змінна може приймати значеня true, false, null. І це важливо при отримані данних з бази данних. Іншого зручного способу представлення bool та числових данних без значення немає. Зверніть увагу шо сінтаксіческий аналізатор не видає ніяких попереджень.

При використані ? використовуеться структура загальної структури System.Nullable<T>. 
```cs
static void StructSystemNullable()
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
StructSystemNullable();

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
Змінні nullable типу використовують функціональність структури System.Nullable<T>.  


## Використання nullable value типів

Функціональність структури Nullable<T> можна використовувати при отримані данних від БД. 

```cs
static void UsingNullablesValueType()
{
    
    UserDatabaseSimulator girlJulia = new UserDatabaseSimulator(1, "Julia");
    GetUserInfo(girlJulia);

    UserDatabaseSimulator girlHanna = new UserDatabaseSimulator(2, "Hanna",true,35);
    GetUserInfo(girlHanna);

    UserDatabaseSimulator boyAlex = new UserDatabaseSimulator(3, "Alex",null, 30);
    GetUserInfo(boyAlex);

    UserDatabaseSimulator boyJohn = new UserDatabaseSimulator(4, "Jhon",true);
    GetUserInfo(boyJohn);



    void GetUserInfo(UserDatabaseSimulator user)
    {

        string result = $"Id:{user.id} Name:{user.name} ";

        bool? merried = user.GetMerried();


        if (merried.HasValue)
        {
            result +=$"Merried:{merried.Value} ";
        }
        else
        {
            result += "Merried: undefined ";
        }

        int? age = user.GetAge();

        if (age != null)
        {
           result +=$"Age:{age}";
        }
        else
        {
            result += "Age: undefined";
        }

        Console.WriteLine(result);
    }
}
UsingNullablesValueType();


class UserDatabaseSimulator
{
    public int id;
    public string name; 
    public bool? merried;
    public int? age;

    public UserDatabaseSimulator(int id, string name, bool? merried = null, int? age=null)
    {
        this.id = id;
        this.name = name;
        this.merried = merried;
        this.age = age;
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
```
Id:1 Name:Julia Merried: undefined Age: undefined
Id:2 Name:Hanna Merried:True Age:35
Id:3 Name:Alex Merried: undefined Age:30
Id:4 Name:Jhon Merried:True Age: undefined
```

Оскільки для необовязкових полів БД ми можемо получити як встановлені так і не встановленні значення за допомогю властивості HasValue та != null ми можимо корректно обробити данні. Також існує корисний метод GetValueOrDefault() якій при відсутності значення вертає default. Треба зазначити шо синтаксичний аналізатор не дає попереджень і тобто ми можемо бути більш впевнені внашому коді. Крім того з прикладу видно шо прі різних комбінаціях визначенності та не визначенності данних метод GetUserInfo обробляє данні корректно і не викидає виняткових ситуацій. 


# Nullable reference типи.

Reference тип теж може бути nullable. Як було показано раніше в методі CrashWithNull() де використвоуються змінни NoNullable типу і зволікалися зауваженя аналізатора коду програма закінчувала роботу викинувши виняток. Використовуючи Nullable і обробити випадки коли значення не визначено можно бути більше впевненому в надійності колу.  

```cs
static void UsingNullableReferenceType()
{
    Person? girl;

    girl = GetPersonFromDb(IsItDefinet: true);
    GetPersonData(girl);

    girl = GetPersonFromDb(IsItDefinet: false);
    GetPersonData(girl);


    static void GetPersonData(Person? person)
    {
        if(person != null)
        {
            person.Display();
        }
        else
        {
            Console.WriteLine("Person undefined");
        }
    }

    static Person? GetPersonFromDb(bool IsItDefinet)
    {
        return IsItDefinet ? new Person("SomeOne",30) : null;
    }

}
UsingNullableReferenceType();

class Person
{
    public string? Name { get; set; }
    public int Age { get; set; }

    public Person() {}

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Display() => Console.WriteLine($"Name:{Name} Age:{Age}");
}
```
```
Name:SomeOne Age:30
Person undefined
```
Як ми бачимо синтаксішний аналізатор не дає поппереджень і тому винятків не виникне.
Nullable reference типи можна визначати в nullable-контексті. За замовченям від .Net 6 це весі шаблоши. Це визначаеться в файлі проекту.

```xml
    <Nullable>enable</Nullable>
```
При міграції коду з версій меньще С# 8 рекомендується спочатку включати попередження без вмикання аннотації (?). Потім очшашючи код включати аннотацію використовуючи дерективи компілятору.

Існує можливість налаштувати зауваженя компілятора як помилки.
```
<WarningsAsErrors>CS8604,CS8625</WarningsAsErrors>
```
Крім того існую налаштування якє всі зауваженя трактує як помилки.
```
<TreatWarningsAsErrors>true</TreatWarningsAsErrors>
```

## Оператор ?? та ??=

Для визначення становиша nullable змінних є декілька корисних операторів.

```cs
static void UsingNullCoalescing()
{
    UserDatabaseSimulator girlJulia = new UserDatabaseSimulator(1, "Julia");
    Console.WriteLine($"Is age null: {girlJulia.age == null}");
    int? girlAge;

    //Code without ??
    girlAge = girlJulia.age;
    if (!girlJulia.age.HasValue)
    {
        girlAge = 35;
    }
    Console.WriteLine(girlAge);


    //With operator ??
    girlAge = girlJulia.age ?? 35;
    Console.WriteLine(girlAge);


    //Operator ??=
    girlAge ??= 85;
    Console.WriteLine(girlAge);
} 
UsingNullCoalescing();
```
```
Is age null: True
35
35
35
```

Оператор ?? може мати більше 2 операндів. Він вичисляє їх доки в цьому є резон.

## Оператор object?

Перед тим як використовувати об'єкт класу ви провіряете чи не дорівнює він null.

```cs
static void UsingNullConditional()
{
    //Exapmle1
    ArrayLength(null);
    ArrayLength(new string[] { "good", "better", "best" });

    static void ArrayLength(string[]? args)
    {
        //Without operator ?
        if (args != null)
        {
            Console.WriteLine(args.Length);
        }
        else
        {
            Console.WriteLine(0);
        }

        //With operator ?
        Console.WriteLine(args?.Length ?? 0);
    }


    //Example2
    Person? boy;
    boy = null;
    Action(boy);

    boy = new Person("John",30);
    Action(boy);    

    static void Action(Person? person)
    {
        //Without operator ?
        if (person != null)
        {
            Console.WriteLine(person.Name);
        }

        //With operator ?
        Console.WriteLine(person?.Name);

        person?.Display();
    }
}
UsingNullConditional();

class Person
{
    public string? Name { get; set; }
    public int Age { get; set; }

    public Person() {}

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    public void Display() => Console.WriteLine($"Name:{Name} Age:{Age}");
}
```
```
0
0
3
3

John
John
Name:John Age:30
```

Як бачите цей опреатор спрощує перевірку на null. Також він корисний для подій та делегатів.

## Кращі спопоби перевірки на null
```cs
    if (someting is not null)
    {
        //Action with something
    }

    if (someting is null)
    {
        //Action with something
    }

```

## Перевірка аргументів функцій на null.

Коли функція отримує аргументи їх можна первірити на null.

```cs
void ValidationOfFunctionArguments()
{
    try
    {
        //AddSum1(null);
        AddSum2(null);

    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }


    // Variant 1
    void AddSum1(string account, decimal sum = 0)
    {
        if (account is null)
        {
            throw new ArgumentNullException(nameof(account));
        }
        // the rest code
    }

    // Variant 2
    void AddSum2(string account, decimal sum = 0)
    {
        ArgumentException.ThrowIfNullOrEmpty(account);
        // the rest code

    }
}
ValidationOfFunctionArguments();
```
```
Value cannot be null. (Parameter 'account')
```
Перевірку на null може згенерувати VS. Для цього треба cтати на аргумент(в цьому прикладі account) і натиснути на CTRL + . та вибрати Add null check.
