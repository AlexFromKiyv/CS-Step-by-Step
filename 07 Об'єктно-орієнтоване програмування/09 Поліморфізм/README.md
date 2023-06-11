# Поліморфізм

Розглянемо метод GiveBonus() в проекті Polymorphism

```cs
namespace Polymorphism
{
    class Employee_v1
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Pay { get; set; }
        public Employee_v1(int id, string name, decimal pay)
        {
            Id = id;
            Name = name;
            Pay = pay;
        }

        public Employee_v1()
        {
            Name = "Not known";
        }
        public void GiveBonus(decimal amount) => Pay += amount;
        public void ToConsole() => Console.WriteLine($"{Id} {Name} {Pay}");

    }

    class SalesPerson_v1 : Employee_v1
    {
        public SalesPerson_v1()
        {
        }
        public SalesPerson_v1(int id, string name, decimal pay, int salesNumber) : base(id, name, pay)
        {
            SalesNumber = salesNumber;
        }
        public int SalesNumber { get; set; }
    }


    class Manager_v1: Employee_v1
    {
        public Manager_v1()
        {
        }
        public Manager_v1(int id, string name, decimal pay, int stockOption ) : base(id, name, pay)
        {
            StockOptions = stockOption;
        }
        public int StockOptions { get; set; }
    }
}
```
```cs
ExploreMethodBaseClass();
void ExploreMethodBaseClass()
{
    SalesPerson_v1 salesPerson = new();
    salesPerson.GiveBonus(100);
    salesPerson.ToConsole();

    SalesPerson_v1 salesPerson1 = new(100, "Viktory", 500, 25);
    salesPerson1.GiveBonus(100);
    salesPerson1.ToConsole();

    Manager_v1 manager1 = new(2,"Bob",1000,50);
    manager1.GiveBonus(100);
    manager1.ToConsole();
}
```
```
0 Not known 100
100 Viktory 600
2 Bob 1100
```
В данному випадку в похідних класах метод GiveBonus працює однаково. Але в реальному житі похідні класи мають власні особливості. Наприклад отриманя бонуса продавцем може залежити від кількості продажів і відрізняеться від реалізацію в базовому класі. 

## virtual override

Одже споріднені класи повині по різному відповідани однаковим запитам. Це можно зробити визначивши метод в базовому класі а в похідному перевизначить його.
```cs
namespace Polymorphism
{
    class Employee_v2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Pay { get; set; }
        public Employee_v2(int id, string name, decimal pay)
        {
            Id = id;
            Name = name;
            Pay = pay;
        }
        public Employee_v2()
        {
            Name = "Not known";
        }
        public virtual void GiveBonus(decimal amount) => Pay += amount;
        public virtual void ToConsole()
        {
            Console.WriteLine($"\n{Id} {Name}");
            Console.WriteLine($" Pay:{Pay}");
        }
    }

    class SalesPerson_v2 : Employee_v2
    {
        public SalesPerson_v2()
        {
        }
        public SalesPerson_v2(int id, string name, decimal pay, int salesNumber) : base(id, name, pay)
        {
            SalesNumber = salesNumber;
        }
        public int SalesNumber { get; set; }
        public override void GiveBonus(decimal amount)
        {
            int salesBonus = 0;
            if (SalesNumber > 0) 
            { 
                if (SalesNumber <100) 
                {
                    salesBonus = 1;
                }
                else
                {
                    salesBonus = 2;
                }
            }

            base.GiveBonus(amount*salesBonus);
        }
        public override void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($" SalesNumber:{SalesNumber}");
        }
    }

    class Manager_v2 : Employee_v2
    {
        public Manager_v2()
        {
        }
        public Manager_v2(int id, string name, decimal pay, int stockOption) : base(id, name, pay)
        {
            StockOptions = stockOption;
        }
        public int StockOptions { get; set; }
        public override void GiveBonus(decimal amount)
        {
            base.GiveBonus(amount);
            StockOptions += new Random().Next(500); 
        }
        public override void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($" StockOptions:{StockOptions}");
        }
    }
}
```
```cs
ExploreVirtualOverridMethods();
void ExploreVirtualOverridMethods()
{
    Employee_v2 employee1 = new();
    employee1.GiveBonus(100);
    employee1.ToConsole();

    Employee_v2 employee2 = new(1, "John", 1000);
    employee2.GiveBonus(100);
    employee2.ToConsole();

    SalesPerson_v2 salesPerson1 = new();
    salesPerson1.GiveBonus(100);
    salesPerson1.ToConsole();

    SalesPerson_v2 salesPerson2 = new(2, "Jak", 700, 120);
    salesPerson2.GiveBonus(100);
    salesPerson2.ToConsole();

    Manager_v2 manager1 = new(3, "Bob", 1000, 300);
    manager1.GiveBonus(100);
    manager1.ToConsole();
}
```
```

0 Not known
 Pay:100

1 John
 Pay:1100

0 Not known
 Pay:0
 SalesNumber:0

2 Jak
 Pay:900
 SalesNumber:120

3 Bob
 Pay:1100
 StockOptions:601
```
Поліморфізм дозволяє підкласу визначити власну версію для методів базового класу шляхом превизначення.
Якщо метод базового класу може бути перевизначатись нащадком (але не обов'язково) то цей метод треба позначити virtual.
Коли підклас хоче замінити детаді реалізації віртального методу від використовує ключеве слово override. Перевизначення може використовувати базовий метод використовуючи base. Таким чином не треба реалізоіувати логіку базового класу.
Зверніть увагу ящо підклас не перевизначає метод він використовує метод базового класу.
Інстументи розробки мають інструмент полегщеня превизначення. Для цього,маючи курсор в похідному класі, натисніть Ctl + . Generate overrides ... Крім того коли ви вводите слово override та пробіл з'являеться список всіх методів які можна перевизначити.

Методи можна запаковувати аби не дозволяти його перевизначати.
```cs
        public override sealed void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($" SalesNumber:{SalesNumber}");
        }
```
Нащадок вже не зможе перевизначити цей метод.

## Абстрактний клас. 

Базовий клас розробляють для надання різноманітних членів нащадкам. Також він надає віртуальні методи які нащадки можуть перевизначити.
Іноді треба робити настількі узагальнеший клас аби зібрати тільки загальні властивості. Наприклад можна створити клас чогось рухающогося але потреба створювати об'єкта з'являеться тільки у нащадків цього класу. Іноді треба визначити тільки загальни риси сімейства класів без реалізації.
Якшо клас занато загальни і не докінця визначений набагатокрашим варіантом його проектування є не дозволяти створювати об'єкти такого типу а лише визначити основні риси нащадків. Програмно це можна зробити визначивши abstract class
```cs
namespace Polymorphism
{

    abstract class Employee_v3
    {
        public int Id { get; set; }
        public decimal Pay { get; set; }

        protected Employee_v3(int id, decimal pay)
        {
            Id = id;
            Pay = pay;
        }
        protected Employee_v3()
        {
        }
        public virtual void ToConsole()
        {
            Console.WriteLine($"\n{Id}");
            Console.WriteLine($" Pay:{Pay}");
        }

    }

    class SalesPerson_v3 : Employee_v3
    {
        public SalesPerson_v3()
        {
        }

        public SalesPerson_v3(int id, decimal pay,int salesNumber) : base(id, pay)
        {
            SalesNumber = salesNumber;
        }

        public int SalesNumber { get; set; }
    }

}
```
```cs
ExploreAbstractClass();
void ExploreAbstractClass()
{
    //Employee_v3 employee1 = new(); // Cannot create ... abstact type

    SalesPerson_v3 salesPerson1 = new();
    salesPerson1.ToConsole();

    SalesPerson_v3 salesPerson2 = new(1, 1000, 200);
    salesPerson2.ToConsole();    

}
```
```

0
 Pay:0

1
 Pay:1000
```
Створити об'єкт абстрактного класу не можна і не потрібно оскільки потрібен більш конкретний тип. Абстракний клас це допоміжний клас а не конкретна сутність. В абстракному класі може бути багато конструкторів які будуть визиватися з нашадків.


## Поліморфний інтерфейс.

Розглянемо наступний дізайн класів. Проект PolymorphicInterface

```cs
namespace PolymorphicInterface
{
    abstract class Shape_v1
    {
        public string Name { get; set; }
        protected Shape_v1(string name = "No name")
        {
            Name = name;
        }
        public virtual void Draw()
        {
            Console.WriteLine("Work method Shape.Draw()");
        }
        public virtual void  ToConsole() => Console.WriteLine($"\n {Name}");
    }

    class Circle_v1 : Shape_v1
    {
        public Circle_v1(string name = "No name") : base(name)
        {
        }
    }

    class Hexagon_v1 : Shape_v1
    {
        public Hexagon_v1(string name = "No name") : base(name)
        {
        }
        public override void Draw()
        {
            Console.WriteLine($"Hexogen - {Name}");
        }
    }
}
```
```cs
ExploreAbstractClassMemeber();
void ExploreAbstractClassMemeber()
{
    Circle_v1 circle_1 = new();
    circle_1.ToConsole();
    circle_1.Draw();


    Circle_v1 circle_2 = new("Ball");
    circle_2.ToConsole();
    circle_2.Draw();

    Hexagon_v1 hexagon_1 = new("Max");
    hexagon_1.ToConsole();
    hexagon_1.Draw();
}
```
```

 No name
Work method Shape.Draw()

 Ball
Work method Shape.Draw()

 Max
Hexogen - Max
```
В прикладі базовий клас занадто загальний і тому він абстрактний. Цім ви кажете шо не треба об'єктів цього класу. Конструктор визначені як protect тому його можна викликати лише в похідному класі. Метод Draw визначено як віртуальний тому нащадкі можуть його реалізовувати. Один нашадок не перевизначає метод і використовує той шо в базовому класі, інший заміняє реалізацію. Нашадок з власною реалізацію набагато ясніший чим загальна реалізація. Перевизначення віртуальних методів не обов'язковє.
Тож аби примусити нашадків бути більш чіткишими і виразними треба зробити метод abstract. Коли метод стає abstract це означає шо немає реалізації за замовчуівнням і нашадкі забов'язані мати власну. 
Абстрактні методи можуть бути визначені тільки в абстрактних класах. Методи позначений abstract слугує тіки для вказування. Це вказуваня для нащадків що ви повині реалізувати цей метод щоб відповідати рисам сімейства класів. 
```cs
namespace PolymorphicInterface
{
    abstract class Shape_v2
    {
        public string Name { get; set; }
        protected Shape_v2(string name = "No name")
        {
            Name = name;
        }
        public abstract void Draw();
    }

    class Circle_v2 : Shape_v2
    {
        public Circle_v2(string name = "No name") : base(name)
        {
        }

        public override void Draw() => Console.WriteLine($"Circle({Name})");
    }

    class Hexagon_v2 : Shape_v2
    {
        public Hexagon_v2(string name = "No name") : base(name)
        {
        }

        public override void Draw()
        {
            Console.WriteLine($"This is Hexogen -> {Name}");
        }
    }
}
```
```cs
ExploreAbstractMethods();
void ExploreAbstractMethods()
{

    Shape_v2[] shapes = {
        new Circle_v2(),
        new Circle_v2("Ball"),
        new Hexagon_v2(),
        new Hexagon_v2("Max")
        };

    foreach(Shape_v2 shape in shapes)
    {
        Console.WriteLine(shape.GetType());
        shape.Draw();
    }
}
```
```
PolymorphicInterface.Circle_v2
Circle(No name)

PolymorphicInterface.Circle_v2
Circle(Ball)

PolymorphicInterface.Hexagon_v2
This is Hexogen -> No name

PolymorphicInterface.Hexagon_v2
This is Hexogen -> Max

```
Визов shape.Draw() найкрашим образом демонструю поліморфний інтерфейс. Хоча не можна створити об'єкт базового класу можна зберігати посилання на об'єкти похідного класу. Всі об'єкти шо походять від астрактоного класу підтримують той самий поліморфний інтерфейс, тому в них е реалізований абстрактний метод Draw. Коли переглядається масив тоді викликається необхідний метод. Шо важливо при появі інших нащадків цю частину коду не треба буде змінювати.

Абстрактний клас може визначити будь-яку кількість абстрактних членів. Абстрактний член користний коли вам не достатьно базової реалізації і кожен нащадок повинен створити свою реалізацію. Таким чином нав'язуеться поліморфний інтерфейс для кожного нащадка. Нашадок має реалізувати абстрактні члени враховуючи свої особливості. Простіше кажучи, поліморфний інтерфейс посилається на віртуальні і абстратні методи. Це дозволяє створювати гнучки і розширювані додатки. 

## new (shadowing)

Якщо батьківський клас має реалізацію яка не підходить похідному класу але використовується однакове визначення тоді нащадок може визначити повністью свою нову реалізацію. Це буває корисне коли не достатньо прав доступу до батьківського класу. Похідний клас як би затьмарює батьківський.
```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PolymorphicInterface
{
    abstract class Shape_v3
    {
        protected string _name;
        public string Name 
        { 
            get => _name;
            set 
            {
                if(value.Length < 10)
                {
                    _name = value;
                }
                else
                {
                    _name = "";
                } 
                
            } 
        }
        protected Shape_v3(string name = ""):this() 
        {
            Name = name;
        }
        protected Shape_v3()
        {
            _name = "";
        }

        public abstract void Draw();
    }

    class Circle_v3 : Shape_v3
    {
        public Circle_v3(string name = "") : base(name)
        {
        }

        public override void Draw() => Console.WriteLine($"Circle({Name})");
    }

    class ThreeDCircle_v3 : Circle_v3
    {
        public new string Name { get; set; }
        public ThreeDCircle_v3(string name)
        {
            Name = name;
        }
        public new void Draw() => Console.WriteLine($"Drawing 3D Circle -> {Name}");
    }

}
```
```cs
ExploreShadowing();
void ExploreShadowing()
{
    Circle_v3 circle_1 = new("VeriBiGThreeDCircle");
    circle_1.Draw();

    ThreeDCircle_v3 circle_2 = new("VeriBiGThreeDCircle");
    circle_2.Draw();
    ((Circle_v3)circle_2).Draw();
}
```
```
Circle()
Drawing 3D Circle -> VeriBiGThreeDCircle
Circle()
```
Припустимо базові класи знаходяться в бібліотеці і якісь обмеженя вам не піходять. Або ви отримали від коллег клас якій має туж назву але непотрібну реалізацію. 
Якщо є можливість можна перевизначити метод. Але якшо немає доступу можна визначити медод ключовим словом new. Це означає навмисне ігнорування батьківської версії.(реально це можливо якщо зовнішні реалізації комфліктують з вашими). 
new можна застосовувати до (field, constant, static member, or property).
Але в прикладі показано що можна зробити явне перетвореня до батьківського типу і визвати батьківський метод.

## Різниця між перевизначенням і приховуванням методу батьківського класу.

Розглянемо наступні класи.

```cs
    class Person
    {
        public string Name { get; set; }
        public Person(string name)
        {
            Name = name;
        }
        public virtual void ToConsole() => Console.WriteLine($"Name:{Name}");  
    }

    class Employee_v1 : Person
    {
        public string Company { get; set; }

        public Employee_v1(string name, string company): base(name)
        {
            Company = company;
        }
        public override void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($"Company:{Company}");
        }
    }
```
```cs
DifferenceBetweenOverrideAndNew_Override();
void DifferenceBetweenOverrideAndNew_Override()
{
    Person person = new Employee_v1("Viktory", "Farmak");
    person.ToConsole();
} 
```
```
Name:Viktory
Company:Farmak
```
Хоча похідна person типу Person при виконані використовується метод превизначенного методу який знаходиться в heap. Спробуемо теж саме з прихованим методом.

```cs
    class Employee_v2 : Person
    {
        public string Company { get; set; }

        public Employee_v2(string name, string company) : base(name)
        {
            Company = company;
        }
        public new void ToConsole() // change only here 
        {
            base.ToConsole();
            Console.WriteLine($"Company:{Company}");
        }
    }
```
```cs
DifferenceBetweenOverrideAndNew_New();
void DifferenceBetweenOverrideAndNew_New()
{
    Person person = new Employee_v2("Viktory", "Farmak");
    person.ToConsole();
}
```
```
Name:Viktory
```
Таким чином виконуеться метод класу Person.
