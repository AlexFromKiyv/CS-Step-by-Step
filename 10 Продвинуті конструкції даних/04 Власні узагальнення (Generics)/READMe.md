# Власні узагальнення (Generic).

Крім того шо можна використовувати готові узагальнені класи з бібліотеки, можна створювати власні класи. 

## Узагальнення методів.

Створення узагальнених методів це посилена версія перзавантаженя методу. Перезавантаженя звичайних методів це визначеня метода з одной і тою назвою але з різною кількістю або типом параметрів. Перезавантаженя корисна річ але може винукнити ситуація коли треба робити багато методів яки посуті виконують одне й те саме і відрізняються лише типом параметрів.

Generics\CustomGenericMethods\Types.cs
```cs
    static class SwapFunctions
    {
        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        static void Swap(ref Person personA,ref Person personB)
        { 
            Person tempPerson = personA;
            personA = personB;
            personB = tempPerson;
        }
    }
``` 
В цьому прикладі створені методи які міняють місцями дві частини даних за допомогою простого переписуваня. Наче все добре але методів з такою самою логікою може бути нароблено для дуже багатоєкількості типів. Обслуговування великої кількості методі може стати кошмаром.

Створення методу з використанням System.Object не дуже добре, оскілки тягне з собою проблеми упаковки/розпаковки та безпечності типів. 
Багато схожих перезавантажених методів спонукає використати узагальнення.
```cs
        static void Swap<T>(ref T a,  ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
```
В цому методі замість типу вказано параметр(заповнювач) <T> перед списком параметрів. Це означає шо метод оперує типом Т який буде визначений при використані.

```cs
void UseGenericMethod()
{
    int a = 35, b = 50;
    Console.WriteLine($"{a} {b}");
    SwapFunctions.Swap<int>(ref a,ref b);
    Console.WriteLine($"{a} {b}\n");

    string vechile1 = "Bus", vechile2 = "eCar";
    Console.WriteLine($"{vechile1} {vechile2}");
    SwapFunctions.Swap<string>(ref vechile1,ref vechile2);
    Console.WriteLine($"{vechile1} {vechile2}\n");

    Person person1= new Person(1,"Alex"), person2 = new(2,"Ira");
    Console.WriteLine($"{person1} {person2}");
    SwapFunctions.Swap<Person>(ref person1,ref person2);
    Console.WriteLine($"{person1} {person2}");

}

UseGenericMethod();
```
```
35 50

I am swaping two System.Int32

50 35

Bus eCar

I am swaping two System.String

eCar Bus

1 Alex Undefined 0       2 Ira Undefined 0

I am swaping two CustomGenericMethods.Person

2 Ira Undefined 0        1 Alex Undefined 0
```
Основан перевага створення узагальненного методу є шо увас є одна версія методу для підтримки, яка може працювати з єлементами потрібного вам типу з безпечним для типів способом.

Коли ви викликає те загальний метод ви можете опустити вказування типу, оскількі компілятор може його встановити по параметрах. 

```cs
void UseGenericMethodWithoutTypeParameter()
{
    bool run = true, stop = false;

    SwapFunctions.Swap(ref run,ref stop);
}

UseGenericMethodWithoutTypeParameter();
```
Єдине шо це може викликати нерозуміння у коллег коли вони будуть дивитись шо відбуваеться. Краще параметр вказувати аби було зрозуміло шо десь є узагальненний метод.

## Узагальнення структур і класів.

CustomGenericTpe\Point.cs
```cs
    internal struct Point<T>
    {
        private T? _x;
        private T? _y;

        public Point(T x, T y)
        {
            _x = x;
            _y = y;
        }

        public T? X
        {
            get => _x;
            set => _x = value;
        }

        public T? Y
        {
            get => _y;
            set => _y = value;
        }

        public override readonly string? ToString()
        {
            return $"[ {_x} , {_y} ]";
        }

        public void Reset()
        {
            _x = default;
            _y = default;
        }
    }
```
Program.cs
```cs
void UseGenericStruct()
{
    Point<int> point = new(1, 2);
    Console.WriteLine(point);

    Point<double> point1 = new(1.01, 2.02);
    Console.WriteLine(point1);

    Point<string> point2 = new("one", "two");
    Console.WriteLine(point2);

    point1.Reset();
    Console.WriteLine(point1);

    point2.Reset();
    Console.WriteLine(point2);

    Point<long> point3 = default;
    Console.WriteLine(point3);
}

UseGenericStruct();
```
```
[ 1 , 2 ]
[ 1,01 , 2,02 ]
[ one , two ]
[ 0 , 0 ]
[  ,  ]
[ 0 , 0 ]

```
Як видно при визначені узагальнених структур і класів замість конкретних типів полів властивостей і в методах вказуїться параметр Т. Крім того видна шо така структура стає гнучкою і може мати різні застосування.
Додадкову корсить в узагальненнях має ключеве слово default, оскільки дозволяє безпечно встановити значення за замовчуванням.

## Використання узагальнення з шаблоном співставлення.
```cs
void UsePatternMatching()
{
    Point<int> point_1 = new(1, 2);
    DetailOfPoint<int>(point_1);

    Point<string> point_2 = new("one", "two");
    DetailOfPoint<string>(point_2);
 

    void DetailOfPoint<T>(Point<T> point)
    {
        switch (point)
        {
            case Point<string> pointString:
                Console.WriteLine($"Point have string data {pointString}");
                break;
            case Point<double> pointDouble:
                Console.WriteLine($"Point have real data {pointDouble}");
                break;
            case Point<int> pointInt:
                Console.WriteLine($"Point have whole data {pointInt}");
                break;
        }
    }
}

UsePatternMatching();
```
```
Point have whole data [ 1 , 2 ]
Point have string data [ one , two ]
```
В прикладі гілка виконання обираеться в результаті співставленя типу аргументу.

## Обмеженя узагальнень.

При використані узагальнених типів або членів треба вказати тип заповнювача. Це робить використання узагальнення більш безпечними при використані. Але на параметр типу можна задати додадкові обмеження. За допомогою where ви можете вказати як саме повинен виглядяти параметр. Використовуючи це ключеве слово, можна додати перелік обмежень до заданного параметру типу, якій перевірить компілятор.
Обмежити параметр можна:

where T : struct  : Тип заповнювача повинен мати в ланцюгу ієрархії System.ValueType, тобто бути структурою.


where T : class  : Тип заповнювача повинен не мати в ланцюгу ієрархії System.ValueType, тобто бути reference type.

where T : new()  : Тип має мати конструктор за замовчуванням. Зауважте, що це обмеження має бути вказано останнім у багатообмеженому типі.

where T : NameOfBaseClass  : Тип має бути похідним від базового класу.

where T : NameOfInterface  : Тип має реалізовувати інтерфейс. Можна вказувати декілька інтерфейсів через кому.

Використовуваня обмежень потрібно аби створити надзвичайно безпечні типи.

Так може виглядадти ваш узагальнений клас який потребує аби тип заповнювача мав конструктор за замовчуванням.
```cs
	class Car
	{
        public string Model { get; set; }
        public int Yead { get; set; }

        public Car(string model, int yead)
        {
            Model = model;
            Yead = yead;
        }
    }

	class MyGeneticWithDefaultConstructor<T>  where T : new()  
	{
        public T? MyProperty { get; set; }
    }
```
```cs

void UseWhereForGeneric_1()
{
    // Car must be ... parameterless constuctor 
    MyGeneticWithDefaultConstructor<Car> recoverieCary;
}
```
Це може бути корисним коли необхідно створювати екземпляри за замовчуванням наприклад коли використовується new. Крім того таке обмеженя робить перевірку шо тип T є reference type. Також це перевіре що коллега не забув визначити конструктор за замовченням (оскільки будь-який інший конструктор анулює конструктор за замовчуванням що предоставляє платформа).

Після where вказується на який параметр накладується обмеження. Після оператора двокрапка самі обмеженя можна перераховувати через кому.

```cs
    class MyGenericWithManyConstraint<T> where T : class, IEnumerable, new()
    {

    }
```
В цьому випадку T має три вимоги. Це має бути reference type(а не структура),реалізувати інтерфейс IEnumerable та мати конструктор за замовчуванням. Обмеження new() повинно бути останім.    

Параметрів може бути декілька.
```cs
    class AccountDocument
    {
        public int Id { get; set; }
    }

    class MyGenericWithManyConstraintAndParameters<D,T> where D : AccountDocument,
        new() where T : struct, IComparable<T>
    {

    }    
```
Так можна вказати обмеження для кожного з типів параметру.

Обмеженя можна використовувати також для методів.
```cs
    class myClass
    {
        public int MyFunc<T>(T x) where T: struct  
        {
            return 1;
        }
    }
```
Це може пригодитись частіше ніж створеня повних узагальнених типів з декількома параметрами.

## Відсутність обмеженя операторів.

Якшо ви захочете створити метод в якому з змінними типу заповнювача захочете виконати операції накшталт + ,- ,* то шо, тоді ви отримаєте помилку компілятора.
```cs
    class BadMath<T>
    {
        public T Add(T x, T y)
        {
           return x + y; // Operator + cannot be applied to operands of type T
        }
    }
```
Отже цей код нескомпілюється. Хоча це може здатися серйозним обмеженням, треба пам'ятати шо узагальненя не робит перевірку наявності у тима можливості роботи операторів + , - ... . Нажаль не можна вказати обмеженя на тип шо він повинен перезавантажувати оператори. Можна досягти бажаного єфекту використовуючи інтерфейси. Інтерфейси можуть визначити оператори.


