# Інкапсуляція

Зазвичай дані об'єкта про його стан не роблять доступними на пряму. Як правило зміна стану відбуваеться через public члени. 
```cs
    internal class Bus
    {
        public int numberOfSeat;
    }
```
```cs

Bus bus = new Bus();
bus.numberOfSeat = 120345643;
bus.numberOfSeat = -30;
```
Оскілки int дозволяє зберігати різні числа це означая що прямий доступ до даних робить зовсім не схожим їх проекцію з реальними об'єктами. Тобто стан об'єктів стає нікчемним. public поля які відповідають за стан не дозволяють зробити розумні рамки для даних. Тому їх як правило не застосовують в production. З модіфукатором доступу public можуть бути корисними static поля та read-only.

Інкапсуляція забеспечує збереження правільності або цілісності даних стану. Замість робити поля даних загальнодоступними можна зробити набір публічних методів для доступу (get) та зміни (set). Можна створити властивості. Гарно інкапсульований клас захищає цілісність даних стану та приховую деталі роботи. Під каптом може бути різні реалізації а зовні він досить задовольняє потребам. Це добре бо не порушує існуючий код.

Створимо консольний проект EmployeeApp та додамо клас Employee_v1.

```cs
    internal class Employee_v1
    {
        private int _employeeId;
        private string _employeeName;
        private decimal _currentPay;

        public Employee_v1(int employeeId, string employeeName, decimal currentPay)
        {
            _employeeId = employeeId;
            _employeeName = employeeName;
            _currentPay = currentPay;
        }

        public void GiveBonus(decimal amount) => _currentPay += amount;

        public void ToConsole()
        {
            Console.WriteLine($"Id:{_employeeId}");
            Console.WriteLine($"Name:{_employeeName}");
            Console.WriteLine($"Pay:{_currentPay} ");
        }
    }
```

У данному визначені немає доступу до даних аби отримати і установити. Дані не перевіряються і у конструкторі. Традиційний підхід покрашити ситуацію визначеня методів отриманя (get) та установки(set)

```cs
        //get
        public string GetName() => _employeeName;


        //set
        public void SetName(string name)
        {
            if (name.Length > 17)
            {
                Console.WriteLine("The name is not set.It must be less than 17 characters");
            }
            else
            {
                _employeeName = name;
            }
        } 
```
```cs
UsingEmployee_v1();

void UsingEmployee_v1()
{
    Employee_v1 employee = new(1, "Joseph", 10000);
    employee.ToConsole();

    employee.GiveBonus(1000);
    employee.ToConsole();

    employee.SetName("Joe");
    employee.ToConsole();

    employee.SetName("SoIAttemptToInputTooBigName");
}
```

Тут інкапсулюється поле _employeeName. В метод утановки додана додадкова перевірка. Інкапсуляція робить але такі ж самі методи для всатновлення та отримання стану потрібно реалізовувати для всіх полів. Тому в C# є додаткові можливості робити інкапсуляцію.

# Властивості.

Для спрощеня процесу інкапсуляції в C# існує контейнер якій включае в собі методи  отримання(get) і встановлення(set) полів стану які називаються властивостями. 

```cs
    internal class Employee_v2
    {
        private int _employeeId;
        private string? _employeeName;
        private decimal _currentPay;

        public Employee_v2(int employeeId, string? employeeName, decimal currentPay)
        {
            _employeeId = employeeId;
            _employeeName = employeeName;
            _currentPay = currentPay;
        }

        public int Id 
        { 
            get { return _employeeId; } 
            set { _employeeId = value; } 
        }

        public string? Name 
        { 
            get { return _employeeName; } 
            set
            {
                if (value?.Length > 17)
                {
                    Console.WriteLine("The name is not set.It must be less than 17 characters");

                }
                else
                {
                    _employeeName = value;
                }
            }
        }

        public decimal CurrentPay 
        { 
            get => _currentPay;     
            set => _currentPay = value;
        }

        public void GiveBonus(decimal amount) => _currentPay += amount;
        public void ToConsole()
        {
            Console.WriteLine($"Id:{_employeeId}");
            Console.WriteLine($"Name:{_employeeName}");
            Console.WriteLine($"Pay:{_currentPay}\n\n ");
        }
    }
```
```cs
UsingEmployee_v2();

void UsingEmployee_v2()
{
    Employee_v2 employee = new(1, "Joseph", 10000);
    employee.ToConsole();

    employee.GiveBonus(1000);
    employee.ToConsole();

    employee.CurrentPay++;
    employee.ToConsole();

    employee.Name = "Joe";
    employee.ToConsole();

    employee.Name = "SoIAttemptToInputTooBigName";
 
}
```
```
Id:1
Name:Joseph
Pay:10000


Id:1
Name:Joseph
Pay:11000


Id:1
Name:Joseph
Pay:11001


Id:1
Name:Joe
Pay:11001


The name is not set.It must be less than 17 characters
```
В області визначення set використовуеться маркер value для вхідного значення. Коли відбувається звернення до властивостей відпрацьовують відповідні блоки get та set. 
Крім того як видно з прикладу до властивостей можна застосовувати операції наприклад інкремент. В визначенні властивостей можна використовувати лябда вирази. 


## Конструктор з властивостями.

Перевіпка вхідних даних потрібна при створені об'єктів конструктором.

```cs
    internal class Employee_v3
    {
        private int _employeeId;
        private string? _employeeName;
        private decimal _currentPay;

        public Employee_v3()
        {
        }

        public Employee_v3(int id, string? name):this(id,name,default)
        {
        }

        public Employee_v3(int id, string? name, decimal currentPay)
        {
            Id = id;
            Name = name;
            CurrentPay = currentPay;
        }

        public int Id
        {
            get { return _employeeId; }
            set { _employeeId = value; }
        }

        public string? Name
        {
            get { return _employeeName; }
            set
            {
                if (value?.Length > 17)
                {
                    Console.WriteLine("The name is not set.It must be less than 17 characters");

                }
                else
                {
                    _employeeName = value;
                }
            }
        }

        public decimal CurrentPay
        {
            get => _currentPay;
            set => _currentPay = value;
        }

        public void GiveBonus(decimal amount) => Pay += amount;
        public void ToConsole()
        {
            Console.WriteLine($"Id:{Id}");
            Console.WriteLine($"Name:{Name}");
            Console.WriteLine($"Pay:{Pay}\n\n ");
        }

    }

```
```cs
void UsingEmployee_v3()
{
    Employee_v3 employee = new(1, "Joseph", 10000);
    employee.ToConsole();

    employee.GiveBonus(1000);
    employee.ToConsole();

    employee.Name = "Joe";
    employee.ToConsole();

    Employee_v3 badEmployee = new(1, "SoIAttemptToInputTooBigName");
    badEmployee.ToConsole();
}
```
Тут головний конструктор використовує властивості які перевіряють вхідні данні. Зверніть увагу шо в межах класу також іде звертання до властивостей а не приватних даних стану.

## Read-only та write-only властивості.

Властивості можуть буди налаштовані тільки для отримання данних. 

```cs
        public string? PassportNumber { get { return _passportNumber; } }
        public int Id { set { _employeeId = value; } }

        public Employee_v4(int id, string? name, decimal currentPay, string? passportNumber)
        {
            Id = id;
            Name = name;
            CurrentPay = currentPay;
            // PassportNumber = passportNumber; // it read-only
            // check passportNumber
            _passportNumber = passportNumber;
        }
```
Для того аби властивість була read-only опускаеться set. Аби властивість була write-only опускаеться get. Але в конструкторі змінити базове поле не має змоги тоді. Можна зробити також одну із частин приватною для класа.

```cs
        public string? PassportNumber 
        { 
            get { return _passportNumber; } 
            private set { _passportNumber = value; }
        }
```
Таким чином отримати дані можна а змінити лише в межах класу.

## Статичні властивості

Якшо для класу потріблі статичні дани можна створити статичні властивості та конструктори.

```cs
    internal class Employee_v5
    {
        private int _employeeId;
        private string? _employeeName;
        private decimal _currentPay;

        private static decimal _averagePay;

        static Employee_v5()
        {
            _averagePay = 10000; // It's bad style. Here must be reading from configuration.
        }

        public static decimal AveragePay 
        {
            get => _averagePay;
            set => _averagePay = value;
        }
                
        public Employee_v5(int employeeId, string? employeeName, decimal currentPay)
        {
            _employeeId = employeeId;
            _employeeName = employeeName;
            _currentPay = currentPay;
        }

        public bool IsPayMoreThanAverage() => _currentPay > _averagePay; 

    }
```
```cs
UsingEmployee_v5();

void UsingEmployee_v5()
{
    Employee_v5 employee = new(1, "Joseph", 11000);

    Console.WriteLine(employee.IsPayMoreThanAverage());
}
```

## Шаблон зпівставлення з шаблоном властивостей.

```cs
namespace EmployeeApp;
public enum EmployeePayTypeEnum
{
    Hourly,
    Salaried,
    Commission
}
```
```cs
    internal class Employee_v6
    {
        private int _employeeId;
        private string? _employeeName;
        private decimal _currentPay;
        private EmployeePayTypeEnum _payType;

        public int Id { get => _employeeId; set => _employeeId = value; }
        public string? Name { get => _employeeName; set => _employeeName = value; }
        public decimal Pay  { get => _currentPay; set => _currentPay = value;}

        public EmployeePayTypeEnum PayType 
        { 
            get => _payType; set => _payType = value;
        }

        public Employee_v6(int id, string? name, decimal pay)
            :this(id, name, pay, EmployeePayTypeEnum.Salaried)
        {
        }

        public Employee_v6(int id, string? name, decimal pay, EmployeePayTypeEnum payType)
        {
            Id = id;
            Name = name;
            Pay = pay;
            PayType = payType;
        }

        public void GiveBonus(decimal amount)
        {
            Pay = this switch
            {
                { PayType: EmployeePayTypeEnum.Commission } => Pay + amount * 0.1M,
                { PayType:EmployeePayTypeEnum.Hourly} => Pay + amount *40M/2080M,
                { PayType:EmployeePayTypeEnum.Salaried} => Pay + amount,
                _=> Pay
            }; 

        }

    }
```
```cs
UsingEmployee_v6();

void UsingEmployee_v6()
{
    Employee_v6 employee = new(1, "Joseph", 10000,EmployeePayTypeEnum.Commission);

    employee.GiveBonus(1000);

    Console.WriteLine(employee.Pay);

}
```
```
10100,0
```
Таким чином можна використовувати switch. Крім того можна використовувати не одну властивість.

```cs
        public void GiveBonusWithId(decimal amount)
        {
            Pay = this switch
            {
                { Id: > 100, PayType: EmployeePayTypeEnum.Commission } => Pay + amount * 0.1M,
                { Id: > 100, PayType: EmployeePayTypeEnum.Hourly } => Pay + amount * 40M / 2080M,
                { Id: > 100, PayType: EmployeePayTypeEnum.Salaried } => Pay + amount,
                _ => Pay
            };
        }
```
```cs
UsingEmployee_v6();

void UsingEmployee_v6()
{
    Employee_v6 employee = new(1, "Joseph", 10000,EmployeePayTypeEnum.Commission);
    employee.GiveBonus(1000);
    Console.WriteLine(employee.Pay);

    employee.Pay = 10000;
    employee.GiveBonusWithId(1000);
    Console.WriteLine(employee.Pay);

    Employee_v6 max = new(103, "Max", 10000, EmployeePayTypeEnum.Commission);
    max.GiveBonus(1000);
    Console.WriteLine(max.Pay);
}
```
```
10100,0
10000
10100,0
```

Шаблон властивостей можна владати.

```cs
    private DateTime _hireDate;
    public DateTime HireDate { get => _hireDate; set => _hireDate = value; }

    public void GiveBonusWithIdAndHireDate(decimal amount)
    {
        Pay = this switch
        {
        { Id: > 100, PayType: EmployeePayTypeEnum.Commission, HireDate: { Year : > 2020} } => Pay + amount * 0.1M,
        { Id: > 100, PayType: EmployeePayTypeEnum.Hourly, HireDate: { Year: > 2020 } } => Pay + amount * 40M / 2080M,
        { Id: > 100, PayType: EmployeePayTypeEnum.Salaried, HireDate: { Year: > 2020 } } => Pay + amount,
        _ => Pay
        };
    }        

    public void GiveBonusWithIdAndHireDateImproved(decimal amount)
    {
        Pay = this switch
        {
            { Id: > 100, PayType: EmployeePayTypeEnum.Commission, HireDate.Year: > 2020  } => Pay + amount * 0.1M,
            { Id: > 100, PayType: EmployeePayTypeEnum.Hourly, HireDate.Year: > 2020 } => Pay + amount * 40M / 2080M,
            { Id: > 100, PayType: EmployeePayTypeEnum.Salaried, HireDate.Year: > 2020 } => Pay + amount,
            _ => Pay
        };
    }
```

## Автоматичні властивості.

Іноді для областей get та set не потрібно додадкових бізнес правила. Ці означає шо можна марнувати час на створеня приватних полів і схожого коду отриманя та встановлення. Розгянемо проект CarApp. Для швидкого визначення властивостей почніть вводити prop та tab.
```cs
    internal class Car_v1
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public int SerialNumber { get; }

        public Car_v1(string manufacturer, string model, int year)
        {
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
        }

        public Car_v1() : this("Not known", "Not known", default)
        {
        }

        public void ToConsole()
        {
            Console.WriteLine($"Manufacturer: {Manufacturer}");
            Console.WriteLine($"Modle: {Model}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine("\n");
        }
    }
```
```cs
UsingCar_v1();
void UsingCar_v1()
{
    Car_v1 car = new();
    car.ToConsole();

    car.Manufacturer = "VW";
    car.Model = "Golf 7";
    car.Year = 2018;
    car.ToConsole();

    Car_v1 yourCar = new("Mercedes", "Sprinter", 2018);
    yourCar.ToConsole();
}
```
```
Manufacturer: Not known
Modle: Not known
Year: 0


Manufacturer: VW
Modle: Golf 7
Year: 2018


Manufacturer: Mercedes
Modle: Sprinter
Year: 2018
```
Таке визначення властивостей називають автоматичними властивостями і воно прекладає процес інкапсуляції на компілятор. Також можна створювати read-only властивості.

Коли ви використовуєте автоматичні властивості для чисел та логічних значень за замовчуванням втсновлюється 0 та false. Інші типи в автовластивості можуть приймати значення за замовчуванням null і тут потрібно бути обережним.

```cs
    internal class Garage_v1
    {
        public int NumberOfCars { get; set; }
        public Car_v1 MyCar { get; set; } // Non-nullable prop must contain a non-null when exiting constructor. 
    }
```
```cs
UsingGarage_v1();

void UsingGarage_v1()
{
    Garage_v1 garage = new();

    Console.WriteLine(garage.NumberOfCars);
    Console.WriteLine(garage.MyCar.Model);
}

```
Такий код викидає виняток System.NullReferenceException. попередження компілятора застерігає про таку ситуацію бо конструктор за замовчуванням призначає null. Можна замінити конструктор за замовчуванням.

```cs
    internal class Garage_v2
    {
        public int NumberOfCars { get; set; }
        public Car_v1 MyCar { get; set; }

        public Garage_v2()
        {
            NumberOfCars = default;
            MyCar = new Car_v1();
        }
    }
```
```cs

UsingGarage_v2();

void UsingGarage_v2()
{
    Garage_v2 garage = new();

    Console.WriteLine(garage.NumberOfCars);
    Console.WriteLine(garage.MyCar.Model);
}
```

Так само як приватним полям даних можна призначити значення, також можна призначити значення автовластивостям. Це дозволяє не писати додадкових призначень в конструкторах.


```cs
    internal class Garage_v3
    {
        public int NumberOfCars { get; set; } = 1;
        public Car_v1 MyCar { get; set; } = new Car_v1();

        public Garage_v3()
        {
        }

        public Garage_v3(int numberOfCars, Car_v1 myCar)
        {
            NumberOfCars = numberOfCars;
            MyCar = myCar;
        }
    }
```
```cs
UsingGarage_v3();
void UsingGarage_v3()
{
    Garage_v3 garage = new();

    Console.WriteLine(garage.NumberOfCars);
    Console.WriteLine(garage.MyCar.Model);
}
```
```
1
Not known
```
Таким чином автовластивість зручний синтаксис який при необхідності можна пертворити в властивість. 

## field

Для автогенерованих властивостей компілятор робить сховані приватні поля. Аби мати доступ до ціх приватних полів можна декларувати поле як field.

## required

Якшо використати такий модіфікатор до властивості компілятор перевірить, що вивстановили значення властивості пид час створення єкземпляра. Тобто під час виклику конструктора або при ініціалізації.
