# Інтерфейси

Об'єкти реального світу мають різноманітну поведінку. Наприклад транспортний засім має поведінкову особливість перемішатись. Інтерфейс виражає поведінку яку даний клас або структура може підтримувати. Клас може підтримувати скільки необхідно інтерфейсів шо означає шо клас може підтримувати декілька поведінок, так само як людина може і бігати і плавати і стрибати.
Бібіліотеки .Net побудований таким чином шо визначена значна кількість інтерфейсів та їх реализацій для різних потреб. Наприклад в технології ADO.NET дозволяє комунікувати з різноманітних типами систем управліня БД. Ви можете підклячичтися до БД за допомогою різних об'єктів (SqlConnection, OleDbConnection, OdbcConnection,...) а також до різних БД(MySQL, Oracle,...).Всі вони реалізовують загальний інтерфейс IDbConnection. 

```cs
namespace System.Data
{

    public interface IDbConnection : IDisposable
    {
        string ConnectionString { get; set; }
        int ConnectionTimeout { get; }
        string Database { get; }
        ConnectionState State { get; }
   
        IDbTransaction BeginTransaction();
        IDbTransaction BeginTransaction(IsolationLevel il);
        void ChangeDatabase(string databaseName);
        void Close();
        IDbCommand CreateCommand();
        void Open();
    }
}
```
Цей інтерфайс визначає набір членів які визначають поведінку комунікувати з БД. Це гарантує що кожен об'єкт класу, який реалізовує цей інтерфейс, буде мати ці члени.
Кожний клас реалізовує ці методи в залежності від БД але поведінковий сенс однаковий. Тобто з якою б СУБД ви не працювали ви можете використати властивості і методи цього інтерфейсу (Open() Close() CreateCommand()). 
Іншими словами інтерфейс це наче контракт яку поведінку повинен реалізувати клас.
Інтерфейсів .NET зазвичай  починаються з великої літери I. 

## Інтерфейсні типи в порівняні з абстрактними класами. 

Може здатися шо інтерфейси не відрізняються від абстрактних базових класів. Коли клас позначено як абстрактний, він може визначати будь-яку кількість абстрактних членів, щоб забезпечити поліморфний інтерфейс для всіх похідних типів. В абстрактному класі можна визначити конструктори, поля,  неабстракні члени з реалізацією. Інтерфейси містять в більшості визначення але можуть також мати реалізацію за замовчуванням і статичні члени. 
Є лише дві реальні відмінності: інтерфейси не можуть мати нестатичні конструктори, а клас може реалізувати декілька інтерфейсів. Тобто інтерфейс більше видповідають за поведінка , а абстрактний клас є основою сімейста класів.
Поліморфний інтерфейс, створений абстрактним батьківським класом, має одне велике обмеження, а саме те, що лише похідні типи підтримують члени, визначені абстрактним батьківським класом. Але великі проекти можуть мати великі розгалужені іерархіЇ і не мати одного спільного базового класу. 

Проект Interfaces\Types_v1.cs
```cs
    public abstract class CloneableType
    {
        public abstract object Clone();
    }

    public class Car
    {
    }
    
    public class Bus : Car, CloneableType { } // cannot multiple base classes 
```
Тільки нашадки класу CloneableType будуть підтримувати метод Clone(). Інший набір класів не може отримати цей поліморфний інтерфейс. Також ми не можето зробити базовими декілька класів. 

Інтерфейси можна реалізовувати в будь-якому класі або структурі. Інтерфейси дуже поліморфні. Розглянемо вбудований інтерфейс IClonable.

```cs
namespace System
{
    //
    // Summary:
    //     Supports cloning, which creates a new instance of a class with the same value
    //     as an existing instance.
    public interface ICloneable
    {
        //
        // Summary:
        //     Creates a new object that is a copy of the current instance.
        //
        // Returns:
        //     A new object that is a copy of this instance.
        object Clone();
    }
}
```
Якшо ви подиветесь бібліотеку базових класів .Net багато класів реалізовують його (System.Array, System.Data.SqlClient.SqlConnection, System.OperatingSystem, System.String ...). Хоча типи різні їх об'єкти можна обробляти за допомогою типу інтерфейсу IClonable.

```cs
ExplorationICloneable();
void ExplorationICloneable()
{
    CloneMe("Hi");

    int[] myArray = new int[3] { 1, 2, 3 };
    CloneMe(myArray);

    OperatingSystem operatingSystem = new OperatingSystem(PlatformID.Unix, new Version());
    CloneMe(operatingSystem);


    void CloneMe(ICloneable cloneable)
    {
        object TheClone = cloneable.Clone();
        Console.WriteLine("\n"+cloneable);
        Console.WriteLine($"Clone:{TheClone}");
        Console.WriteLine($"Type:{TheClone.GetType()}");
        Console.WriteLine($"ReferenceEquals:{ReferenceEquals(cloneable, TheClone)}");
    }
}
```
```

Hi
Clone:Hi
Type:System.String
ReferenceEquals:True

System.Int32[]
Clone:System.Int32[]
Type:System.Int32[]
ReferenceEquals:False

Unix 0.0
Clone:Unix 0.0
Type:System.OperatingSystem
ReferenceEquals:False
```
Функція CloneMe має параметр типу ICloneable який приймає любий об'єкт який реалізовує цей інтрефейс. Таким чином незалаежно від ієрархії класів вони мають реалізацію інтерфейса, тим самим гарантуючи наявність метода Clone().

Розглядаючи абстракний клас також треба зазначити шо похідні відного класи забовязані реалізувати абстрактні члени. Наприклад є абстракний клас Shape і ви вирішили додати метод GetNumberOfPoints(), який повертає кількість граничних точок фігури. Після цього всі похідні класи (Circle, Hexagon та ThreeDCircle) мають мати реалізацію цього метода навіть коли в цьому нема сенсу.
Інтерфейсні типи можуть допомогти в цьому випадку. Якщо ви визначаєте інтерфейс, який представляє поведінку «наявності точок», ви можете просто підключити його до типу Hexagon, залишивши Circle і ThreeDCircle недоторканими.

## Визначення спеціалних(власних) інтерфейсів 

Припустимо у нас є наступні класи. 

Interfaces\Types_v2.cs
```cs
    abstract class Shape
    {
        public string Name { get; set; }

        protected Shape(string name = "")
        {
            Name = name;
        }
        public abstract void Draw();
    }

    class Circle : Shape
    {
        public Circle() { }
        public Circle(string name) : base(name)
        {
        }
        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} the Circle");
        }
    }

    class Hexagon : Shape
    {
        public Hexagon() { }
        public Hexagon(string name = "") : base(name)
        {
        }
        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} the Hexagon");
        }
    }

    class ThreeDCircle : Circle
    {

        public new void Draw()
        {
            Console.WriteLine($"Drawing {Name} 3D Circle");
        }
      
    }
```

Додамо в проект інтерфейс. Правий клік на проекті Add > New item > Interface > Name: IPointy.cs > Add

```cs
    /// <summary>
    /// This interface defines the behavior of 'having points.'
    /// </summary>
    internal interface IPointy
    {
        //public int NumberOfPoints; // Error. Cannot contain instance fields

        //public IPointy() { } // Error. Cannot contain instance constructor

        //byte GetNumberOfPoint(); //This method is the same as property Points

        public int Points { get; } // Interface types are able to define any number of property

    }
```
Інтерфейси визначаються за допомогою ключового слова interface. Цей інтерфейс визначає що фігура має точки. Інтерфейси не можуть визначати дані і мати нестатичні конструктори.
Інтефейс може мати властивості. В цому випадку властивість заміняє метод.
Інтефейсні типи не можна створювати за допомогою конструктора як клас або стуктуру. Вони начинають приносити користь тільки коли вони реалізовуються в класах або структурах. Тоб то серед ірархій є класи яки маєть таку поведінку (в цьому прикладі мати точки).

## Реалізація інтейфейса

Аби вказати шо клас реалізує інтерфейс він вказується після базового класу.

```cs
class Bus : Vehicle , IMove
```
```cs
class Tank : IMove
```
```cs
// This struct implicitly derives from System.ValueType and
// implements two interfaces.
public struct PitchFork : ICloneable, IPointy
```

Якшо елементи інтерфейсу не мають реалізації за замовчуванням, вона повина бути в класі. Не можна щось реалізувати а шось ні. Наприклад аби клас реалізовував інтерфейс IDbConnection треба шоб в ному були всі 10 авбстрактних члени. 

Interfaces\Types_v2.cs
```cs
    class Triangle : Shape, IPointy
    {
        public Triangle() { }
        public Triangle(string name = "") : base(name)
        {
        }

        public int Points => 3;

        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} Triangle");
        }
    }

   class Hexagon : Shape, IPointy
    {
        public Hexagon() { }
        public Hexagon(string name = "") : base(name)
        {
        }

        public int Points => 6;

        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} the Hexagon");
        }
    }

```
В ціх класах релізована властивість  public int Points { get; } інтерфейсного типу IPointy. Таким чином в наших класах Tiangle , Hexagon реалізовують інтерфейс IPointy,а
Circle, ThreeDCircle ні оскількі це не має сенсу.

## Використаня членів інтерфейсу.

```cs
InvokeInterfaceMembers();
void InvokeInterfaceMembers()
{
    Hexagon hexagon = new();
    Console.WriteLine(hexagon.Points);
}
```
```
6
```
Все досить просто якшо ви знаете шо тип реалізовує властивість. Шо якшо ви маєта чималий масив єлементів сумісних з Shape і деякі не реалізовують IPointy.

```cs
CheckInterfaceTypeByCast();
void CheckInterfaceTypeByCast()
{
    Shape[] shapes = new Shape[] 
    { 
        new Triangle(), 
        new Circle(), 
        new Hexagon(), 
        new ThreeDCircle() 
    };

    foreach (Shape shape in shapes)
    {
        ShowPoints(shape);
    }

    void ShowPoints(Shape shape)
    {
        try
        {
            IPointy pointy = (IPointy) shape;
            Console.WriteLine(pointy.Points);
        }
        catch (InvalidCastException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
```
```
3
Unable to cast object of type 'Interfaces.Circle' to type 'Interfaces.IPointy'.
6
Unable to cast object of type 'Interfaces.ThreeDCircle' to type 'Interfaces.IPointy'.
```
Один із способів визначити чи підтримує тип інтерфейс, спробувати явно привести до типу інтерфейсу. Якщо тип не підтримує потрібний інтерфейс, ви отримаєте виключення InvalidCastException. Хотілося б не пробувати а точно знати чи реалізує тип інтерфейс.

Можна отримати посилання на інтерфейс за допомогою as.

```cs
CheckInterfaceTypeByAs();
void CheckInterfaceTypeByAs()
{
    Shape[] shapes = new Shape[]
    {
        new Triangle(),
        new Circle(),
        new Hexagon(),
        new ThreeDCircle()
    };

    foreach (Shape shape in shapes)
    {
        ShowPoints(shape);
    }

    void ShowPoints(Shape shape)
    {
        IPointy? pointy = shape as IPointy;
        if (pointy != null)
        {
            Console.WriteLine(pointy.Points);
        }
        else
        {
            Console.WriteLine("The shape has no points.");
        }

    }
}
```
```
3
The shape has no points.
6
The shape has no points.
```
Якшо об'єкт належить ло класу який реалізовує інтерфейс приведеня as поверне посилання на нього. Якшо ні повернеться null. Таким чином можна виявити наявність в типі реалізації. Вцьому випадку не треба використовувати try ... catch і дійсно отримує необхідне посилання відповідного типу.

Можна отримати посилання на інтерфейс за допомогою is.

```cs
CheckInterfaceTypeByIs();
void CheckInterfaceTypeByIs()
{
    Shape[] shapes = new Shape[]
    {
        new Triangle(),
        new Circle(),
        new Hexagon(),
        new ThreeDCircle()
    };

    foreach (Shape shape in shapes)
    {
        ShowPoints(shape);
    }

    void ShowPoints(Shape shape)
    {
        if (shape is IPointy pointy)
        {
            Console.WriteLine(pointy.Points);
        }
        else
        {
            Console.WriteLine("The shape has no points.");
        }
    }
}
```
```
3
The shape has no points.
6
The shape has no points.
```
Схожим чином можна перевірити реалізований інтерфейс за допомогою ключового слова is.
Якшо об'єкт несумістний з типом вертаеться false. Якшо сумісний ви маєте зміну з посиланням.

## Реалізація за замовчуванням. 

Методи і властивості в інтерфейсі можуть мати реалізацію зазамовчуанням.

IPointy.cs
```cs
    internal interface IPointy
    {
        public int Points { get; } 
    }

    interface IRegularPointy : IPointy
    {
        int SideLength { get; set; }
        int NumberOfSide { get; set; }
        int Perimeter => SideLength * NumberOfSide;
    }
```
Тут добавлена статична write-only властивість з реалізацією Perimeter.

Interfaces\Types_v2.cs
```cs
    class Square: Shape, IRegularPointy
    {
        public Square() { }

        public Square(string name) : base(name)
        {
        }

        public override void Draw()
        {
            Console.WriteLine($"Drawing {Name} Square");
        }
        //This comes from the IPointy interface
        public int Points => 4;
        //These come from the IRegularPointy interface
        public int SideLength { get; set ; }
        public int NumberOfSide { get; set; } = 4; 
    }
```
Коли інтерфейс має реалізацію за замовченням його можне реалізовувати в нашадку. 
Але в цьому є особливість використання.

```cs
ExplorationDefaultImplamentationInterface();
void ExplorationDefaultImplamentationInterface()
{
    var square = new Square();
    square.Draw();
    square.SideLength = 10;

    //Console.WriteLine(square.Perimeter); // does not contain definition Perimeter

    Console.WriteLine( ((IRegularPointy)square).Perimeter );

    IRegularPointy regularSquare = new Square() {Name = "Garden", SideLength = 20};

    Console.WriteLine(regularSquare.Perimeter);

    //regularSquare.Draw(); // does not contain definition Perimeter

    ((Square)regularSquare).Draw();

}
```
```
Drawing  Square
40
80
Drawing Garden Square
```
Властивість Perimeter, визначена в інтерфейсі IRegularPointy, не визначена в класі Square, що робить її недоступною з екземпляра Square. Для того авби використатти реалізацію з інтерфейса треба явно привести об'єкт до цього типу.
Можна зразу створювати тип IRegularPointy, але тоді не будуть доступни методі класу Square.
Таким чином перш ніж робити реалізацію за замовчуванням переконайтесь шо вона не приведе до того шо треба буде шукати реалізацію.

## Статичні конструктори і члени.

```cs
    interface IRegularPointy : IPointy
    {
        int SideLength { get; set; }
        int NumberOfSide { get; set; }
        int Perimeter => SideLength * NumberOfSide;


        //Static constructor and member
        static string Inscription { get; set; }
        static IRegularPointy() => Inscription = string.Empty;
    }
```
```cs
ExplorationStaicConstructorAndMemeberOfInterface();

void ExplorationStaicConstructorAndMemeberOfInterface()
{
    Console.WriteLine(IRegularPointy.Inscription);

    IRegularPointy.Inscription = "Shape ...";

    Console.WriteLine(IRegularPointy.Inscription );
}
```
```
No inscription
Shape ...
```
Інтерфейси можуть мати статичні конструктори і члени які функціонують так само як в класах. Статичні конструктори не можуть мати параметрів і мають доступ лише до статичних членів.
Визов статичних члені відбувається на рівні інтерфейсу.








