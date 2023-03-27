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
Інакше кажучи любий клас "is an" Object. Враховуючи це єкземпляр любого класу можна зберігати в змінній цього типу. Тип також можна зберігати в типі попередників.
```cs
ExploreHierarchy();
void ExploreHierarchy()
{
    object obj = new Manager(1, "Max", 1000, 200);
    Console.WriteLine(obj);

    Employee employee_1 = new Manager(2, "Bob", 1200, 100);
    employee_1.ToConsole();

    SalesPerson salesPerson_1 = new PartSalesPerson(3,"Jill",500,20);
    salesPerson_1.ToConsole();
}
```
```
ClassCastingRules.Manager

2
 Pay:1200
 StockOptions:100

3
 Pay:500
```

Важливе правило: коли два класи пов'язані відношенням "is a" завжди безпечно зберігати посилання на похідний об'ект в базовому типі. 

Це називається неявним приведеням оскілки похідний клас "не меньше" базового. Це дозволяє робити потужні програмні конструкції.
```cs
```  


