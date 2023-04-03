# Складові типу System.Object

Любий класс явно не показує свій базовий клас. Кожен тип зрештою походить від System.Object який представлений ключовим словом object. Цей клас має загальні члени для всіх класів. Коли ми створюємо клаз без вказування базового компілятор неявно добавляе System.Object (object). Тобто любі класи мають доступ до членів object. Формально члени класу можна показати так.
```cs
 // virtual
  public virtual bool Equals(object obj);
  protected virtual void Finalize();
  public virtual int GetHashCode();
  public virtual string ToString();
  // instance-level, nonvirtual
  public Type GetType();
  protected object MemberwiseClone();
  // static
  public static bool Equals(object objA, object objB);
  public static bool ReferenceEquals(object objA, object objB);
```
Зверніть увагу декі клвси віртуальні і тообто їх можна перевизначити, а деякі класи статичні і їх можна викликати на рівні класу.

public virtual bool Equals(object obj) За замовчуванням цей метод вертає true тільки коли елементи посилаються на то самий елемент в пам'яті.Таким чином, Equals() використовується для порівняння посилань на об’єкти, а не стану об’єкта.Як правило, цей метод перевизначається, щоб повернути true, лише якщо об’єкти, які порівнюються, мають однакові значення внутрішнього стану. Майте на увазі, що якщо ви замінюєте Equals(), ви також повинні замінити GetHashCode(), оскільки ці методи використовуються внутрішньо типами Hashtable для отримання підоб’єктів із контейнера. 

protected virtual void Finalize() Цей метод викликається для звільнення будь-яких виділених ресурсів до того, як об’єкт буде знищено.

public virtual int GetHashCode() Iдентифікує конкретний екземпляр об’єкта повертаючи int.

public virtual string ToString() Вертає рядкове представлення в форматі

namespace.type_name 

Цей метод часто перевизначається підкласом, щоб повернути токенізований рядок пар ім’я-значення, які представляють внутрішній стан об’єкта, а не його повне ім’я.   

public Type GetType() Цей метод повертає об’єкт Type, який повністю описує об’єкт, на який ви зараз посилаєтеся. Коротше кажучи, це метод ідентифікації типу виконання, доступний для всіх об’єктів.

protected object MemberwiseClone() Цей метод існує для повернення почленної копії поточного об’єкта, який часто використовується під час клонування об’єкта.

Розглянемо пустий клас який вже має базовий клас object. Проект SystemObject.
```cs
    internal class Person
    {
    }
```
```cs
ExploreSystemObject();
void ExploreSystemObject()
{

    Person person1 = new Person();

    Console.WriteLine($"ToString:\t{person1.ToString()}");
    Console.WriteLine($"GetHashCode:\t{person1.GetHashCode()}");
    Console.WriteLine($"GetType:\t{person1.GetType()}");

    // new reference to person1
    Person person2 = person1;

    // new reference to person1
    object obj = person2;

    Console.WriteLine(obj.Equals(person1));
    Console.WriteLine(obj.Equals(person2));
    Console.WriteLine(person1.Equals(obj));
    Console.WriteLine(person2.Equals(obj));
}
```
```
ToString:       SuperParentClass.Person
GetHashCode:    54267293
GetType:        SuperParentClass.Person
True
True
True
True
```
За замовчуванням Equals перевіряє чи на той замий об'єкт вказуе посилання в змінній. Тут  Person person1 = new Person(); строрює об'єкт в керованій купі. А при Person person2 = person1; не створюється новий об'єкт а записуеться посилання.

## Перевизначення ToString

```cs
    internal class Person_v1
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public Person_v1(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
        public Person_v1()
        {
        }

        public override string? ToString() =>
            $"[First Name: {FirstName}; Last Name: {LastName}; Age: {Age}]";
       
    }
```
```cs
ExploreOverridingToString();
void ExploreOverridingToString()
{
    Person_v1 person_1 = new Person_v1("Mark", "Twain", 49);
    Console.WriteLine(person_1);

    Person_v1 person_2 = new Person_v1();
    Console.WriteLine(person_2);

}
```
```
[First Name: Mark; Last Name: Twain; Age: 49]
[First Name: ; Last Name: ; Age: 0]
```
Аби отримати тип об'єкту можна використовувати GetType, тому поведінка за замовчуванням ToString нічого корисного не дає і її можна первизначити. Це можна зробити аби в текстовому вигляді отримати поточний стан. Це може бути кориснє при налагоджені.Рекомендований підхід полягає в тому, щоб відокремити кожну пару ім’я-значення крапкою з комою та взяти весь рядок у квадратні дужки (багато типів у бібліотеках базових класів .NET Core дотримуються цього підходу).
Однак завжди пам’ятайте, що правильне перевизначення ToString() має також враховувати будь-які дані, визначені в ланцюжку успадкування. Гарною практикою при перевизначенні великіх класів є використанням base який повертає рядок з батьківського класу.

## Перевизначення Equals

Метод Equals в object порівнює посилання на об'єкти. В своїх класах ми можемо перевизначити метод порівнюючи стани.

```cs
    internal class Person_v2
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public Person_v2(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        public Person_v2()
        {
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Person_v2 person))
            {
                return false;
            }

            bool comparation =
                FirstName == person.FirstName
                && LastName == person.LastName
                && Age == person.Age;

            if (comparation)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
```
```cs
ExploreOverridingEquals();
void ExploreOverridingEquals()
{
    Person_v2 person_1 = new();
    Person_v2 person_2 = new();

    Console.WriteLine(person_1.Equals(person_2));

    Person_v2 person_3 = new("Elvis", "Presley", 35);
    Person_v2 person_4 = new("Elvis", "Presley", 35);

    Console.WriteLine(person_3.Equals(person_4));
    Console.WriteLine(person_3.GetHashCode());
    Console.WriteLine(person_4.GetHashCode());


    Person_v2 person_5 = new("Elvis", "Presley", 36);
    Console.WriteLine(person_3.Equals(person_5));
}
```
```
True
True
54267293
18643596
False
```
Такий перевизначений клас може бути кориснішим тому шо порівнує співпадіння властивостей. Метод в якості параметра отримує object. Зверніть увагу шо при першій перевірці object приводиться і зберігається в зміну необхідного типу. Крім того перевіряється на null. 
Якшо клас містить багато полів даних та нетрівіальних типів складність перевизначення зростає. В цьому випадку простий підхід є ревлізувати перевизначиння ToString, а потім порівняти текстове представлення.
```cs
    internal class Person_v3
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public Person_v3(string firstName, string lastName, int age)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
        public Person_v3()
        {
        }

        public override string? ToString() =>
            $"[First Name: {FirstName}; Last Name: {LastName}; Age: {Age}]";

        public override bool Equals(object? obj)
        {
            return obj?.ToString() == ToString();
        }
    }
```
```cs
ExploreOverridingEqualsWithToSting();
void ExploreOverridingEqualsWithToSting()
{
    Person_v3 person_1 = new();
    Person_v3 person_2 = new();

    Console.WriteLine(person_1.Equals(person_2));

    Person_v3 person_3 = new("Elvis", "Presley", 35);
    Person_v3 person_4 = new("Elvis", "Presley", 35);

    Console.WriteLine(person_3.Equals(person_4));
    Console.WriteLine(person_3.GetHashCode());
    Console.WriteLine(person_4.GetHashCode());


    Person_v3 person_5 = new("Elvis", "Presley", 36);
    Console.WriteLine(person_3.Equals(person_5));
}
```
```
True
True
54267293
18643596
False
```
Головне в цій реалізації що стан об'єкта в текстовому представлені це все що потрібно для порівняння об'єктів. Аби тількі цього було достатньо.

##  Перевизначення GetHashCode

При перевизначені ви можете побачити зауваження шо ви змінили реалізацію Equlas але не змінили GetHashCode. Хеш-код - це числове значеня int як певний конкретний стан. 

```cs
void ExploreHashCode()
{
    string string1 = "Hi girl";
    string string2 = "Hi girl";
    string string3 = "нi girl";

    Console.WriteLine(string1.GetHashCode());
    Console.WriteLine(string2.GetHashCode());
    Console.WriteLine(string3.GetHashCode());
}
```
```
963323211
963323211
1370931025
```
За замовчуванням для refetence type System.Object.GetHashCode() використовує розташування об'єкта в пам'яті для отриманя значення hash.
Якшо спеціальний тип буде зберігатися в типі Hashtable(System.Collections namespace) і ви змінили Equlas, тоді обов'язково треба перевизначити GetHashCode. Це повьязано стим шо Hashtable буде визивати оба ці методи для отриманя правільного об'єкта. Тип System.Collections.Hashtable використовує GetHashCode щоб знати де находиться об'єкт а потім Equales надає остаточний результат.

Існує багато алгоритмів як створити hash code. Убільшості випадків можна сгенерувати hash використовуючи реалізацію System.String.
```cs
    internal class Person_v4
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public string SSN { get; } = "";

        public Person_v4(string firstName, string lastName, int age, string sSN)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            SSN = sSN;
        }

        public Person_v4()
        {
        }
        public override string? ToString() =>
            $"[First Name: {FirstName}; Last Name: {LastName}; Age: {Age}]";

        public override bool Equals(object? obj)
        {
            return obj?.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return SSN.GetHashCode();
        }
    }
```
```cs
GenerateHashCodeWithStringProperty();
void GenerateHashCodeWithStringProperty()
{
    Console.WriteLine("1234567".GetHashCode());

    Person_v4 person_1 = new("Mark", "Twain", 40, "1234567");
    Console.WriteLine(person_1.GetHashCode());

    Person_v4 person_2 = new("Mark", "Twain", 40, "1234567");
    Console.WriteLine(person_2.GetHashCode());

    Person_v4 person_3 = new("Elwis", "Presley", 40, "1234567");
    Console.WriteLine(person_3 .GetHashCode());

    Person_v4 person_4 = new("Elwis", "Presley", 40, "1234566");
    Console.WriteLine(person_4.GetHashCode());

}
```
```
193808213
193808213
193808213
193808213
-577016148
```
```
95092599
95092599
95092599
95092599
-1163468339
```
В цьому прикладі не може бути аби різні персони мали однаковий старховий номер. Тому його можна взяти як основу для hash coda. При тому як ви бачите hash code річ яка використовується тільки "тут і заре" один раз. Таким чином System.String має солідну реалізацію але нужно правільно для неї знайти унікальну основу.

Якшо у вас є превизначений ToString і в класі є поле Id можна використовувати цю строку.

Крім того ви можете встати в коді класу натиснути CTRL + . > Generete GetHashCode()

```cs
    internal class Person_v5
    {
        public int Id { get; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public int Age { get; set; }

        public Person_v5(int id, string firstName, string lastName, int age)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }
        public Person_v5()
        {
        }

        public override string? ToString() =>
            $"[Id: {Id}; First Name: {FirstName}; Last Name: {LastName}; Age: {Age}]";

        public override bool Equals(object? obj)
        {
            return obj?.ToString() == ToString();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, Age);
        }
    }
```
```cs
GenerateHashCodeWithHashCode();
void GenerateHashCodeWithHashCode()
{
    Console.WriteLine(HashCode.Combine(1, "Mark", "Twain", 40));

    Person_v5 person_1 = new(1,"Mark", "Twain", 40);
    Console.WriteLine(person_1.GetHashCode());

    Person_v5 person_2 = new(2,"Mark", "Twain", 40);
    Console.WriteLine(person_2.GetHashCode());

    Person_v5 person_3 = new(3,"Elwis", "Presley",40);
    Console.WriteLine(person_3.GetHashCode());

    Person_v5 person_4 = new(4,"Elwis", "Presley", 40);
    Console.WriteLine(person_4.GetHashCode());

}
```
```
1473650869
1473650869
-1527127730
804563149
1473450801
```

## Статистични члени object.

В класі System.Object є статичні класи які можна використовувати на рівні класу.

```cs
    internal class Person_v5
    {
        ...

        public override bool Equals(object? obj)
        {
            Console.Write("Work in method public override bool Equals(object? obj)\t");
            return obj?.ToString() == ToString();
        }

        ...

    }
```
```cs
ExploreStaticMemeberObject();
void ExploreStaticMemeberObject()
{
    Person_v5 person_1 = new();
    Person_v5 person_2 = new();

    Console.WriteLine(object.Equals(person_1,person_2));
    Console.WriteLine(ReferenceEquals(person_1,person_2));

    Person_v5 person_3 = new(1,"Leonardo","da Vinci",35);
    Person_v5 person_4 = new(1, "Leonardo", "da Vinci", 35);

    Console.WriteLine(object.Equals(person_3, person_4));
    Console.WriteLine(ReferenceEquals(person_3, person_4));
}
```
```
Work in method public override bool Equals(object? obj) True
False
Work in method public override bool Equals(object? obj) True
False
```
Як видно Equal рівня класу використовує перевизначену логіку класу.