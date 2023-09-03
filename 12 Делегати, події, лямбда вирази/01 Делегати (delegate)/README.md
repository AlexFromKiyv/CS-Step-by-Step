# 01 Делегати (delegate)

Більшість програм не мають простий прямолінійший шаблон, що стоврюється класи  з методами і іде взаємодія коду програми з ними. Багато програм побудовано таким чином що об'єкт міг комукувати назад до сутності яка його створила використовуючи механізм зворотнього виклику. Цей механізм можна використовувати будь-де але він особливо важливий в графічних інтерфейсах. При натискані на кнопку, наприклад, викликаються зовнішні методи.
Делегати є крашим засобом зворонього віклику. По суті делегат це типобезпечний об'ект який вказує на метод або список методів які будуть визвани пізніше.
У платформі .Net зворотні виклики здійсьнюються типобезпечним і об'єктно-оріентованим способом за дотпомогою делегатів. Делегат - це типобезпечний об'єкт якій вказує на інший метод або список методів, які можна викликати пізніше. Зокрема делегат зберігає три важливі відомості:

    - Aдресс методу за яким він здійсьнює виклик.
    - Параметри цього методу.
    - Тип повернення.

Делегати можуть вказувати на статични або методи екземплярів класу.
Пілсля створення об'єкта делегату та надання необхідних данних, він може бути дінамічно викликати методи на які вказує під час виконання.

## Визначення типу делегата.

```cs
    // This delegate can point to any method,
    // taking two integers and returning an integer.
    public delegate int BinaryIntOp(int x, int y);
```
Для визначення використовується ключеве слово delegate. Назва делегата може бути будь-якою але краше шоб відповідала суті. Сігнатура делегата визначає конкретні значеня вхідних і вихвдних параметрів які відповідають методу на який він буде вказувати. В прикладі делегат може вказувати на будь-який метод який повертає int та приймає два int як параметри.
При компіляції типу делегата автоматично генерується saled клас шо походить від System.MulticastDelegate. Цей клас (разом з своїм базовим класом System.Delegate) забезпечує необхідну інфраструктуру щоб делегата міг зберігати список методі які будуть викликатись пізніше. В ньому створено метод Invoke на основі визначення делегата. Посуті генерується схожий класс.

```cs
sealed class BinaryIntOp : System.MulticastDelegate
{
  public int Invoke(int x, int y);
...
}
```
Сігнатура методу Invoke точно співпадає з визначенням делегата. Викоистання делегата не потребує визову цього методу безпосередьно він викликається залаштунками.
Припустимо ми визначемо делегат інакше.
```cs
public delegate string MyDelegate (bool a, bool b, bool c);
```
В цьому випадку компілятор сгенерує клас схожиц на такий.
```cs
sealed class MyDelegate : System.MulticastDelegate
{
  public string Invoke(bool a, bool b, bool c);
...
}

```
Також делегат може вказувати на метод з out, ref та params параметрами.
Таким чином визначення типу делегата приводить до створення запечатаного класу з згенерованим компілятором методом параметри і тип повернення якого базується на декларації делегата.

## System.MulticastDelegate та System.Delegate базові класи.

Коли вказуеться тип за допомогою ключового слова delegate по суті створюєтся клас який походить від класу System.MulticastDelegate. Цей клас надає нашадкам доступ до списку, якій містить адреси методів, підтимуваних об'єктом делегату. Також в ньому є додадкові методи , перезавантажених операторів для взаємодії зі списком. Батьківським класом цього класу є клас System.Delegate. Ось деякі його члени.

```cs
public abstract class Delegate : ICloneable, ISerializable
{
  // Methods to interact with the list of functions.
  public static Delegate Combine(params Delegate[] delegates);
  public static Delegate Combine(Delegate a, Delegate b);
  public static Delegate Remove(
    Delegate source, Delegate value);
  public static Delegate RemoveAll(
    Delegate source, Delegate value);
  // Overloaded operators.
  public static bool operator ==(Delegate d1, Delegate d2);
  public static bool operator !=(Delegate d1, Delegate d2);
  // Properties that expose the delegate target.
  public MethodInfo Method { get; }
  public object Target { get; }
}
```
Від цого класу походить клас System.MulticastDelegate.Ось деякі його члени.

```cs
public abstract class MulticastDelegate : Delegate
{
  // Returns the list of methods 'pointed to.'
  public sealed override Delegate[] GetInvocationList();
  // Overloaded operators.
  public static bool operator ==
    (MulticastDelegate d1, MulticastDelegate d2);
  public static bool operator !=
    (MulticastDelegate d1, MulticastDelegate d2);
  // Used internally to manage the list of methods maintained by the delegate.
  private IntPtr _invocationCount;
  private object _invocationList;
}

```

Ці класи можна назвати службовими і ви не будете пряцювати з ними напряму але вони дають розуміння побудови всіх делегатів. Використовуючи слова delegat ви опосередковано стоврюєте клас який є MulticastDelegate. 
Ось опис основніх членів всіх делегатів.

  Method : Ця властивість повертає об'єкт System.Reflection.MethodInfo який пердставляє деталі статичного методу, який підтримує делегат.

  Target : Якшо метод, який потрібно викликати, визначено на рівні об'єкта(а не статичний), Target повертає об'єкт який репрезентує метод, якій підтримуїє делегат. Якшо значення, що повертає Target, дорівнює null, метод що потрібно викликати є статичним членом. 

  Combine() : Цей статичний метод додає метод до списку шо підтримує делегат. Цей метод запускається за допомогою перезавантаженого оператора += як скорочений варіант.

  GetInvocationList(): Цей метод повертає масив об'єктів System.Delegate, кожен з якиx реперезентує метод який можна викликати.

  Remove()/RemoveAll() : Ці статични методи видаляють метод (або всі) з списку виклику делегата. Метод Remove() скороченою нотацією -=.


## Просте використання делегата.

Types.cs
```cs
    public class SimpleMath
    {
        public static int Add(int x, int y) =>  x + y; 
        public static int Subtract(int  x, int y) => x - y;

    }
```
Program.cs
```cs
using Delegates;
void UseSimpleDelegate()
{
    // Create a BinaryIntOp delegate object that
    // 'points to' SimpleMath.Add().
    BinaryIntOp biIntOp = new(SimpleMath.Add);

    // Invoke Add() method indirectly using delegate object.
    // Invoke() is really called here!
    int result = biIntOp(5, 5);

    Console.WriteLine(result);
    
    Console.WriteLine(biIntOp.Invoke(10,10));
}

UseSimpleDelegate();



//...

// Additional type definitions must be placed at the end of the
// top-level statements

// This delegate can point to any method,
// taking two integers and returning an integer.
public delegate int BinaryIntOp(int x, int y);
```
```
10
20
```
Звергіть увагу на формат оголошеня типу делегату. Це декларування вказує що об'екти-делегати можуть вказувати на будь-який метод шо приймає два параметри int та повертає int (фактична назва методу не має значення). 
В класі створюється два статичних методи які відповідають шаблону визначеному делегатом. 
Якшо треба призначити цільовий метод певному об'єкту делегат, ім'я методу передається в конструктор делегата при створені.
Середовище виконання в цьому випадку створює клас похідний від MulticastDelegate з необхідною інфраструктурою. Потім вивкликається метод Invoce цього класу і в нього передаються параметри. Не вимагається явного виклику метода Invoce.

Делегати безпечні для типів.
```cs
    public class SimpleMath
    {
        //...
        public static int Square(int x) => x * x; 
    }
```
```cs
void DelegateIsTypeSafe()
{
    BinaryIntOp intOp1 = new(SimpleMath.Subtract);

    Console.WriteLine(intOp1(10,20));
    
    //BinaryIntOp intOp2 = new(SimpleMath.Square); // Square is method that have diferent signateure.
}
```
Компілятор покаже помилку якшо ви спробуєте створити об'єкт делегату який не буде відповідати шаблону.

## Об'єкт делегату.

Створимо метод який видає сладові делегата.

Types.cs
```cs
    public class MyMath
    {
        public int AddTwoInt(int x, int y) => x + y;
    }
```
Program.cs
```cs
static void DisplayDelegateInfo(Delegate delegateObject)
{
    foreach (Delegate @delegate in delegateObject.GetInvocationList())
    {
        Console.WriteLine($"Method name:{@delegate.Method}");
        Console.WriteLine($"Type name:{@delegate.Target}");
    }
}
void InvestigatingDelegateObject()
{
    BinaryIntOp intOp = new(SimpleMath.Add);
    DisplayDelegateInfo(intOp);

    MyMath myMath = new();
    BinaryIntOp intOp1 = new(myMath.AddTwoInt);
    DisplayDelegateInfo(intOp1);
}

InvestigatingDelegateObject();
```
```
Method name:Int32 Add(Int32, Int32)
Type name:
Method name:Int32 AddTwoInt(Int32, Int32)
Type name:Delegates.MyMath
```
Допоміжний метод отримує об'єкт далаегата та виводить назви методу що підтримує делегат а також назву класу що визначає метод. Список методів видає метод GetInvocationList(). 
Зауважте шо якшо метод статичний він не є частиною об'єкта і тому не вивиодиться. Якшо метод приналежить об'екту тип об'єкту зберігаеться в Target.

## Використання делегата при зміні стану об'єкта.

Зрозуміло шо просте використання делегата не має великої користі, тому шо метод можна використати на пряму. Користь делегатів виявляється коли потрібно створювати гнучкий механізм виклику. 
Щоб продемонструвати більш реальний варіант використання делегата, давайте використаемо делегат для визначеня класу Car, якій може інформувати зовнішнім сутностям про стан двигуна. Для цього в класі треба:
1. Визначити тип делегата який буде використовуватися для сповішеня зовнішнього викликаючого.
2. Оголосити зміну як член класу цього типу делегата.
3. Створити допоміжну функцію класу яка дозволить зовнішньому викликаючому  встановити метод для зворотнього виклику.
4. Використати метод Accelerate для запуску методів списку делегата при певних обставинах.

Припустимо в нас є базова частина класу Car.
```cs
    internal class Car
    {
        // State data
        public string Name { get; set; } = string.Empty;
        public int MaxSpeed { get; set; } = 100;
        public int CurrentSpeed { get; set; }

        private bool _isDead;

        //Constructors
        public Car(string name, int maxSpeed, int currentSpeed )
        {
            Name = name;
            MaxSpeed = maxSpeed;
            CurrentSpeed = currentSpeed;
        }
        public Car()
        {
        }
    }    
```
Додамо можливості роботи делегата.
```cs
        // For delegate work

        //Define delegate type
        public delegate void CarEngineHandler(string messageForCaller);

        //Variable for delegate
        private CarEngineHandler _listOfHandlers;

        //For external caller allows register method for call 
        public void RegisterCarEngineHandler(CarEngineHandler methodToCall)
        {
            _listOfHandlers = methodToCall;
        } 
```
Зауважте делегат визначений в межах класу шо означає шо він працює з цім класом. Це не об'язково. Тип делегату може вказувати на будьякий метод який приймає в якості параметра string і нічого не повертає. Далі визнчаетья зміна для об'екту делегата і допоміжна функція яка дозволяє вказати методи на які буде вказувати делегат. Зміну можна було б зробити public і таким чином не треба булоб робит додадковий метод регістрації. Але в данній реалізацію служба інкапсуляції робить рішеня бульш безпечним.
Тепер треба зробити метод який буде використовувати методи.

```cs
        public void Accelerate(int delta)
        {
            if (_isDead)
            {
                _listOfHandlers?.Invoke("Sorry, this car is dead!");
            }
            else
            {
                CurrentSpeed += delta;
                

                if (CurrentSpeed > MaxSpeed)
                {
                    _isDead = true;
                    _listOfHandlers?.Invoke("Car dead!");
                }
                else
                {
                    Console.WriteLine($"Current speed {Name}: {CurrentSpeed}");
                }

                if ((MaxSpeed - CurrentSpeed) < 10 && !_isDead )
                {
                    _listOfHandlers?.Invoke($"Careful buddy! Gonna blow! Current speed:{CurrentSpeed}");
                }
            }
        }
```
Зверніть увагу що відбуваеться перевірка змінної на null перед викликом методу Invoke. Завдяки цьому, якшо визиваючий код не назначить метод, на який буде вказувати делегат, під ча виконання не виникне виняток NullReferenceException.
Таким чином інфраструктура делегата зроблена можна її використати.
```cs
using NotificationsWithDelegate;

void UseDelegateInfrastructure()
{
    Car carGrey = new("VW E-Golf Grey",150,130);

	for (int i = 0; i < 10; i++)
	{
		carGrey.Accelerate(3);
	}

    Console.WriteLine("\n\n");

    Car carRed = new("VW E-Golf Red", 150, 130);
	carRed.RegisterCarEngineHandler(new Car.CarEngineHandler(OnCarEngineEvent));

    for (int i = 0; i < 10; i++)
    {
		carRed.Accelerate(3);
	}

	void OnCarEngineEvent(string message)
	{
		Console.WriteLine($"  Message from car engine: {message}");
	}
}
UseDelegateInfrastructure();
``` 
```
Current speed VW E-Golf Grey: 133
Current speed VW E-Golf Grey: 136
Current speed VW E-Golf Grey: 139
Current speed VW E-Golf Grey: 142
Current speed VW E-Golf Grey: 145
Current speed VW E-Golf Grey: 148



Current speed VW E-Golf Red: 133
Current speed VW E-Golf Red: 136
Current speed VW E-Golf Red: 139
Current speed VW E-Golf Red: 142
  Message from car engine: Careful buddy! Gonna blow! Current speed:142
Current speed VW E-Golf Red: 145
  Message from car engine: Careful buddy! Gonna blow! Current speed:145
Current speed VW E-Golf Red: 148
  Message from car engine: Careful buddy! Gonna blow! Current speed:148
  Message from car engine: Car dead!
  Message from car engine: Sorry, this car is dead!
  Message from car engine: Sorry, this car is dead!
  Message from car engine: Sorry, this car is dead!
```
Перший об'єкт не призначає обробника для сповішення стану двигуна, тому сповіщень про стан немає. Другий об'єкт региструє обробника і відповідно спрацьлвує метод Invoke який визиває метод. 
Важливий момент цього прикладу що метод сповіщеня знаходиться в коді роботи з об'єктом а не в ному. Вся логіка сповіщення знаходиться в об'єкті а процес відображення зовні. Знову ж таки метод відповідає сігнатурі делегата. 

## Додавання та вилучання декількох обробників.

Об'єкт типу делегата може отримувати декілька методів і зберігати список на виконання.

```cs

        public void RegisterCarEngineHandlers(CarEngineHandler methodToCall)
        {
            _listOfHandlers += methodToCall;
        }

        public void UnRegisterCarEngineHandlers(CarEngineHandler methodToCall)
        {
            _listOfHandlers -= methodToCall;
        }
```
Тут замість оператора = використовуються перезавантажені версії += та -=. При роботі ці операторо використовують методи класу Combine та Remove.

```cs
void UseMulticasting()
{
    Car car = new("VW E-Golf", 150, 133);
    car.RegisterCarEngineHandlers(new Car.CarEngineHandler(OnCarEngineEvent1));
    car.RegisterCarEngineHandlers(new Car.CarEngineHandler(OnCarEngineEvent2));

    for (int i = 0; i < 7; i++)
    {
        car.Accelerate(3);
    }

    Console.WriteLine("\n\n");

    Car car1 = new("Mercedes W123", 150, 133);


    Car.CarEngineHandler handler = new Car.CarEngineHandler(OnCarEngineEvent1);
    car1.RegisterCarEngineHandlers(handler);
    car1.RegisterCarEngineHandlers(new Car.CarEngineHandler(OnCarEngineEvent2));
    car1.UnRegisterCarEngineHandlers(handler);

    for (int i = 0; i < 7; i++)
    {
        car1.Accelerate(3);
    }


    void OnCarEngineEvent1(string message)
    {
        Console.WriteLine($"\t{message}");
    }
    void OnCarEngineEvent2(string message)
    {
        Console.WriteLine($"\t{message.ToUpper()}");
    }

}

UseMulticasting();
```
```
Current speed VW E-Golf: 136
Current speed VW E-Golf: 139
Current speed VW E-Golf: 142
        Careful buddy! Gonna blow! Current speed:142
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:142
Current speed VW E-Golf: 145
        Careful buddy! Gonna blow! Current speed:145
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:145
Current speed VW E-Golf: 148
        Careful buddy! Gonna blow! Current speed:148
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:148
        Car dead!
        CAR DEAD!
        Sorry, this car is dead!
        SORRY, THIS CAR IS DEAD!



Current speed Mercedes W123: 136
Current speed Mercedes W123: 139
Current speed Mercedes W123: 142
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:142
Current speed Mercedes W123: 145
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:145
Current speed Mercedes W123: 148
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:148
        CAR DEAD!
        SORRY, THIS CAR IS DEAD!
```
Таким чином можна працювати з списком виклику.

## Спрощений синтаксис з перетворенням.

В попередніх прикладах для регістрації обробника ми створювали екземпляр об'єкта делегата в конструктор якого передавали назву метода. Коли треба використати можливості класу MulticastDelegate та Delegate це має сенс. В більшрості випадків такий об'єкт потрібен тільки для того шоб отримати назву метода на виконання або списку таких. Тому зробили спрощення яке називаеться method group conversion (перетворення групи методів). Це дозволяє вказати замість об'єкта делегати назву метода, підчас виклику метода який примає тип делегата як аргумент.
```cs
void UseMethodGroupConversion()
{
    Car car = new("BMW i3", 180, 160);

    car.RegisterCarEngineHandler(OnCarEngineEvent);
    car.UnRegisterCarEngineHandlers(OnCarEngineEvent);
    
    car.RegisterCarEngineHandlers(OnCarEngineEvent);

    for (int i = 0; i < 7; i++)
    {
        car.Accelerate(3);
    }

    void OnCarEngineEvent(string message)
    {
        Console.WriteLine($"\t{message.ToUpper()}");
    }
}

UseMethodGroupConversion();
```
```
Current speed BMW i3: 163
Current speed BMW i3: 166
Current speed BMW i3: 169
Current speed BMW i3: 172
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:172
Current speed BMW i3: 175
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:175
Current speed BMW i3: 178
        CAREFUL BUDDY! GONNA BLOW! CURRENT SPEED:178
        CAR DEAD!
```
Якшо метод не підійде по сігнатурі делегата то компілятор видасть помилку.

## Узагальнені делегати.

Можна створити делегата з узагальненями. Наприклад делегата якій отримує один параметре якогось типу а повертає void.
 
GenericDelegate\Types.cs
```cs
// This generic delegate can represent any method
// returning void and taking a single parameter of type T.
public delegate void MyDenericDelegate<T>(T arg);

``` 
Зверніть увагу шо визначення MyDenericDelegate<T> вказує єдиний параметр типу, який реперезентуе який буде тип аргументу цьлового методу.
Отже це узагальнене визначеня який має бути метод.

```cs
void UseMyGenericDelegate()
{
    MyDenericDelegate<string> workWithString = new(StringTarget);
    workWithString += StringTarget; //work
    workWithString("Hi");

    MyDenericDelegate<int> workWithInt = new(IntTarget);
    workWithInt += IntTarget;
 // workWithInt += StringTarget;  // don't work 
    workWithInt(3);

    void StringTarget(string text)
    {
        Console.WriteLine(text.ToUpper());
    }

    void IntTarget(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(i);
        }
    }
}

UseMyGenericDelegate();
```
```
HI
HI
0
1
2
0
1
2
```
Як бачимо об'ект делегаат не зможе додати до списку виконання метод який не співпадає по сігнатурі.

## Делегати Action<> та Func<>

В попередніх прикладах при використані делегатів ми виконували наступні кроки:

    1. Визначаємо спеціальний тип делегата, який відповідає методам на яки буде вказувати.
    2. Створює екземпляр типу спеціального типу делегата та передаем назву метода як аргумент конструктора.
    3. Викликаємо метод опосередковано через виклмк Invoke об'єкта делегата.

Коли ви використовуєте цей підхід ви створюєте декілька спеціальних делегатів які використовує тільки в поточному завданні. При цому значення має сігнатура делегата яка може повоторюватись. Хоча може бути шо назва має значення для конкретного проекту. У багатьох випадках вам потрібен "якийсь" делегат який ортимує параметри та можливо повертає значення відмінне від void. В таких випадках платформа має вбудованиі типи делегатів Action<> та Func<>.
Загальний делегат Action визначено в просторі імен System і ви можете використати цей загальний делегат, щоб вказати на метод, який приймає до 16 аргументів і повертає void. Оскільки він загальний потрібно вказати тип кожного параметра.

```cs
void UseActionDelegate()
{
    Action<string, ConsoleColor, int> printToConsole = new(MessageToConsole);

    printToConsole("Hi!", ConsoleColor.Yellow, 3);


    void MessageToConsole(string message,ConsoleColor txtColor, int printCount)
    {
        ConsoleColor previousColor = Console.ForegroundColor;
        Console.ForegroundColor = txtColor;

        for (int i = 0; i < printCount; i++)
        {
            Console.WriteLine(message);
        }

        Console.ForegroundColor = previousColor;
    }
}

UseActionDelegate();
```
```
Hi!
Hi!
Hi!
```
Замість того аби створювати власний делегат необхідної сігнатури ми використовуємо вбудований загальний делегат Action 

```cs
 public delegate void Action<in T1, in T2, in T3>(T1 arg1, T2 arg2, T3 arg3);
```
Вказавши необхідні типи та метод виклику ми створи об'єкт який вказує на метод який треба викликати. Таким чином ми не створювали власний делегат і не придумували йому назву, тобто виключили пункт 1 з списку того шо ми робим коли створюємо зворотній виклик. 
Делегат Action може вказувати на методи які приймають аргументи та повертає void. Якшо вам потрібен делегат який буде вказувати на методи яки вертають значення можна використати вбудований делегат Func<>. Загальний делегат Func може вказувати на методи з 16-ма параметрами та повертає вказаний тип.

```cs
void UseFuncDelegate()
{
    Func<int, int, int> biIntOp = Add;
    int result = biIntOp(5,5);
    Console.WriteLine(result);

    biIntOp += Subtract;
    foreach (var item in biIntOp.GetInvocationList())
    {
        Console.WriteLine(biIntOp.Method);
    }
    result = biIntOp.Invoke(5,5);
    Console.WriteLine(result);

    Func<string?> getNumberAsString = InputString;
    Console.WriteLine(getNumberAsString());

    // Local function for delegates
    static int Add(int x,int y) =>  x + y;

    static int Subtract(int x, int y) => x - y;

    static string? InputString()
    {
        Console.Write("Input whole number:");
        string? input = Console.ReadLine();

        return int.TryParse(input,out int _) ? input : null;
    } 
}
UseFuncDelegate();
```
```
10
Int32 <<Main>$>g__Subtract|0_4(Int32, Int32)
Int32 <<Main>$>g__Subtract|0_4(Int32, Int32)
0
Input whole number:5
5
```
Тут ми бачимо перша чатина це той самий приклад який був в пункті "Визанчення типу делегату" на почтаку статті в якій ми вказували на методи Add та Subtract. Різниця в тому шо ми не створюємо власний тип делегату delegate int BinaryIntOp(int x, int y) а користуемся вбудованим. Майте на увазі шо останій параметр вказує на тип значення шо повертає метод.
Оскілки такий підхід трохи простіший встає питання:чи постійно використовувати делегати Action і Func? Відповід як в багатох випадках программовання "залежить від завдання". У багатьох випадках ці делегати підходять. 
Коли вам потрібно передати більше значення делегата, для домену завданя можна встановити власну назву делегата. Це додає лише один рябок коду. 
