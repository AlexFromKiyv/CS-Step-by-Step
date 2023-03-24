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
Поліморфізм дозволяю підкласу визначити власну версію для методів базового класу шляхом превизначення.
Якщо метод базового класу може бути перевизначатись нащадком (але не обов'язково) то цей метод треба позначити virtual.
Коли підклас хоче замінити детаді реалізації віртального методу від використовує ключеве слово override. Перевизначення може використовувати базовий метод використовуючи base. Таким чином не треба реалізоіувати логіку базового класу.
Зверніть увагу ящо підклас не не перевизначає метод він використовує метод базового класу.
Інстументи розробки мають інструмент полегщеня превизначення. Для цього,маючи курсор в похідному класі, натисніть Ctl + . Generate overrides ... Крім того коли ви вводите слово override та пробіл з'являеться список всіх иетодім які можна перевизначити.

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
Якшо клас занато загальни і не докінця визначений набагатокрашим варіантом його пректування є не дозволяти створювати об'єкти такого типу а лише визначити основні риси нащадків. Програмно це можна зробити визначивши abstract class
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
Створити об'єкт абстрактного класу не можна і не потрібно оскільки потрібен більш конкретний тип.











