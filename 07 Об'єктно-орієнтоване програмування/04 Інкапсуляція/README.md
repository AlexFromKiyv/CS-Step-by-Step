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


