# Правила приведення типів

Сворені сімейства класів мають правила за якима відбувається приведеня об'єктів одного типу до спорідненого. Проект ClassCastingRules

```cs
namespace ClassCastingRules
{
    abstract class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Pay { get; set; }
        public Employee(int id, string name, decimal pay)
        {
            Id = id;
            Name = name;
            Pay = pay;
        }
        public Employee() 
        {
            Name = string.Empty;
        }
        public virtual void ToConsole()
        {
            Console.WriteLine($"\n{Id}");
            Console.WriteLine($" Pay:{Pay}");
        }
    }

    class SalesPerson : Employee 
    {
        public SalesPerson()
        {
        }
        public SalesPerson(int id, string name, decimal pay, int salesNumber ) : base(id, name, pay)
        {
            SalesNumber = salesNumber;
        }
        public int SalesNumber { get; set; }

    }

    class Manager : Employee
    {
        public Manager(int id, string name, decimal pay, int stockOptions) : base(id, name, pay)
        {
            StockOptions = stockOptions;
        }
        public int StockOptions { get; set; }

        public override void ToConsole()
        {
            base.ToConsole();
            Console.WriteLine($" StockOptions:{StockOptions}");
        }
    }
}
```
Основним базовим класом є System.Оbject. Тому в ірархії любого класу є цей клас. 
Інакше кажучи любий клас "is an" Object. Враховуючи це єкземпляр любого класу можна зберігати в змінній цього типу. Тип також можна зберігати в типі предків.
```cs
ExploreImplicideCasting();
void ExploreImplicideCasting()
{
    object obj = new Manager(1, "Max", 1000, 200);
    Console.WriteLine(obj);

    Employee employee_1 = new Manager(2, "Bob", 1200, 100);
    employee_1.ToConsole();

    Employee employee_2 = new PartSalesPerson(3, "Jim", 700, 30);
    employee_2.ToConsole();

    SalesPerson salesPerson_1 = new PartSalesPerson(4,"Jill",500,20);
    salesPerson_1.ToConsole();
}
```
```
ClassCastingRules.Manager

2
 Pay:1200
 StockOptions:100

3
 Pay:700

4
 Pay:500

```

Важливо! Коли два класи пов'язані відношенням "is a" безпечно шоб змінна базового типу мала посилання на об'ект похідного класу. Іншими словами зміній базового типу можна призначити об'єкт похідного типу. 

Це називається неявним приведеням. Зміна це посилання на об'єкт в пам'яті. Це працює оскілки базовий клас "підмножина" похідного. Це дозволяє робити потужні програмні конструкції.

```cs
UsingImplicideCasting();

void UsingImplicideCasting()
{
    object obj = new Manager(1, "Max", 1000, 200);
    //GivePromotion(obj); //cannot convert ... to

    Employee employee_1 = new Manager(2, "Bil", 1200, 100);
    GivePromotion(employee_1);

    Employee employee_2 = new PartSalesPerson(3, "Jim", 700, 30);
    GivePromotion(employee_2);

    SalesPerson salesPerson_1 = new PartSalesPerson(3, "Jill", 500, 20);
    GivePromotion(salesPerson_1);

    
    void GivePromotion(Employee employee)
    {
        employee.Pay++;
        Console.WriteLine($"{employee.Name} was promoted! Pay: {employee.Pay}");
        Console.WriteLine(employee.GetType());
        Console.WriteLine();
    }
}
```  
```
Bil was promoted! Pay: 1201
ClassCastingRules.Manager

Jim was promoted! Pay: 701
ClassCastingRules.PartSalesPerson

Jill was promoted! Pay: 501
ClassCastingRules.PartSalesPerson

```
В данному випадку створено метод з одним параметром типу базового класу. В цей метод ви можете предати будья-кого нашадка бо вони повязані звязком "is-a". Метод використовує все шо загальне для всіх похідних типів.

Змінна типу System.Object більш загальний тип. Враховуючи, що об’єкт знаходиться вище в ланцюжку успадкування, ніж Employee, компілятор не допускатиме неявного приведення, намагаючись зберегти ваш код максимально безпечним для типів.

Важливо! Якшо в пам'яті об'єкт відомого типу ви можете явно претвореня до цього типу або його базового за допомогою явного приведення.

```cs
UsingExplicitCasting();
void UsingExplicitCasting()
{
    object obj = new Manager(1, "Max", 1000, 200);
    //GivePromotion(obj); //cannot convert ... to

    GivePromotion((Manager)obj);

    void GivePromotion(Employee employee)
    {
        employee.Pay++;
        Console.WriteLine($"{employee.Name} was promoted! Pay: {employee.Pay}");
        Console.WriteLine(employee.GetType());
        Console.WriteLine();
    }
}
```
```
Max was promoted! Pay: 1001
ClassCastingRules.Manager
```
Тут явним чином відбувається приведеня до типу який можна предети в метод.
 

## as

Треба пам'ятати шо явне приведеня оцінюється під час виконанння а не компіляції.

```cs
namespace ClassCastingRules
{
    abstract class Shape
    {
        public string? Name { get; set; }
        protected Shape(string? name = "No name")
        {
            Name = name;
        }
        public abstract void Draw();
    }

    class Hexagon : Shape
    {
        public Hexagon(string? name = "No name") : base(name)
        {
        }
        public override void Draw()
        {
            Console.WriteLine($"\n Hexagone {Name}"); 
        }
    }
}
```
```cs
ExploreExplicitCasting();
void ExploreExplicitCasting()
{
    object meneger = new Manager(1, "Bill", 1000, 100);

    Hexagon hexagon = (Hexagon)meneger;

    Console.WriteLine(hexagon.Name);
}
```
```
Unhandled exception. System.InvalidCastException: Unable to cast object of type 'ClassCastingRules.Manager' to type 'ClassCastingRules.Hexagon'.
```
Хоча такий код не має сенусу, компілятор не показує помилку а при виконані виникає виняток. У таких випадках краще використовувати конструкцію try і catch.

```cs
ExploreExplicitCastingWithTry();
void ExploreExplicitCastingWithTry()
{
    object meneger = new Manager(1, "Bill", 1000, 100);

    try
    {
        Hexagon hexagon = (Hexagon)meneger;
        Console.WriteLine(hexagon.Name);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
```
```
Unable to cast object of type 'ClassCastingRules.Manager' to type 'ClassCastingRules.Hexagon'.
```
Така ситуація надумана і рідко виникає. Але припустимо ми маємо обробити масив System.Object. 
```cs
UsingKeywordAs();
void UsingKeywordAs()
{
    object[] things = new object[4];
    things[0] = "Hi girl";
    things[1] = new Manager(1, "Bill", 1000, 100);
    things[2] = new Hexagon("Hex");
    things[3] = new PartSalesPerson(3, "Jill", 500, 20);

    foreach( object thing in things)
    {
        Employee? employee = thing as Employee;
        if (employee != null )
        {
            Console.WriteLine(employee.Name);
        }
    }
    Console.WriteLine();

    foreach (object thing in things)
    {
        Shape? shape = thing as Shape;
        if (shape != null)
        {
            Console.WriteLine(shape.Name);
        }
    }
}
```
```
Bill
Jill

Hex
```
Є можливість швидко визначити чи данний елемент сумісний з типом.
Для цього використовується ключове слово as. Якшо не сумісний повертаеться null.

# is





