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

# Властивості

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


## Конструктор з властивостями

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

## Read-only властивості

Властивості можуть буди налаштовані тільки для отримання данних. 

```cs
        public string? PassportNumber { get { return _passportNumber; } }

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
Для того аби властивість була read-only опускаеться set. Але в конструкторі змінити базове поле не має змоги тоді. 
