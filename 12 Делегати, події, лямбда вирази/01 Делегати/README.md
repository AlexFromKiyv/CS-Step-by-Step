 # Делегати

 До цього моменту в тексті більшість програм, які ви розробили, додавали різні фрагменти коду до Program.cs як оператори верхнього рівня, які тим чи іншим чином надсилали запити до певного об’єкта. Однак багато програм вимагають, щоб об’єкт мав можливість зв’язуватися з об’єктом, який його створив, за допомогою механізму зворотного виклику. Хоча механізми зворотного виклику можна використовувати в будь-якій програмі, вони особливо критичні для графічних інтерфейсів користувача, оскільки елементи керування (такі як кнопка) повинні викликати зовнішні методи за правильних обставин (коли натискається кнопка, коли миша входить у поверхню кнопки тощо). 
 На платформі .NET тип делегату є кращим засобом визначення та відповіді на зворотні виклики в програмах. По суті, тип делегату .NET — це типобезпечний об’єкт, який «вказує» на метод або список методів, які можна викликати пізніше. Проте, на відміну від традиційного покажчика функції C++, делегати — це класи, які мають вбудовану підтримку багатоадресної передачі.

 У цій главі ви дізнаєтесь, як створювати типи делегатів і керувати ними, а потім дослідите ключове слово C# event, яке оптимізує процес роботи з типами делегатів. Попутно ви також ознайомитеся з декількома функціями C#, орієнтованими на делегати та події, включаючи анонімні методи та перетворення груп методів.
 Ми завершимо цю главу вивченням лямбда-виразів. Використовуючи лямбда-оператор C# (=>), ви можете вказати блок інструкцій коду (і параметри для передачі цим інструкціям коду), де потрібен делегат із суворим типом. Використовуючи лямбда-оператор C# (=>), ви можете вказати блок інструкцій коду (і параметри для передачі цим інструкціям коду), де потрібен делегат із суворим типом. Як ви побачите, лямбда-вираз — це трохи більше, ніж замаскований анонімний метод і забезпечує спрощений підхід до роботи з делегатами. Крім того, цю саму операцію можна використовувати для реалізації методу або властивості з одним оператором за допомогою стислого синтаксису.

 # Розуміння типу делегата

 Перш ніж формально визначати делегатів, давайте трохи поглянемо. Історично Windows API часто використовував покажчики функцій у стилі C для створення сутностей, які називаються функціями зворотного виклику або просто зворотними викликами. Використовуючи зворотні виклики, програмісти змогли налаштувати одну функцію для звітування (зворотного виклику) іншій функції в додатку. Завдяки цьому підходу розробники Windows змогли обробляти натискання кнопок, переміщення миші, вибір меню та загальний двонаправлений зв’язок між двома об’єктами в пам’яті.
 У .NET зворотні виклики виконуються безпечним для типів і об’єктно-орієнтованим способом за допомогою делегатів. Делегат — це типобезпечний об’єкт, який вказує на інший метод (або, можливо, список методів) у програмі, який можна викликати пізніше. Зокрема, делегат зберігає три важливі відомості.

    1. Адреса методу, за яким він здійснює виклики
    2. Параметри (якщо є) цього методу
    3. Тип повернення (якщо є) цього методу

Делегати .NET можуть вказувати на статичні або екземплярні методи. 

Після створення об’єкта делегату та надання необхідної інформації він може динамічно викликати метод(и), на які він вказує під час виконання.

## Визначення типу делегату в C#

Якщо ви хочете створити тип делегату в C#, ви використовуєте ключове слово delegate. Назва вашого типу делегату може бути будь-якою. Однак ви повинні визначити делегат, щоб відповідати сигнатурі методів, на які він вказуватиме. Наприклад, наступний тип делегату (з назвою BinaryOp) може вказувати на будь-який метод, який повертає ціле число та приймає два цілих числа як вхідні параметри:

```cs
// This delegate can point to any method,
// taking two integers and returning an integer.
public delegate int BinaryOp(int x, int y);
```
Коли компілятор C# обробляє типи делегатів, він автоматично генерує запечатаний клас, що походить від System.MulticastDelegate. Цей клас (у поєднанні зі своїм базовим класом System.Delegate) забезпечує необхідну інфраструктуру для того, щоб делегат міг зберігати список методів, які будуть викликані пізніше. Наприклад, якщо ви перевірите делегат BinaryOp за допомогою ildasm.exe, ви знайдете деякі деталі, як показано тут:
```
// -------------------------------------------------------
//     TypDefName:BinaryOp
//     Extends   : System.MulticastDelegate
//     Method #1
//     -------------------------------------------------------
//             MethodName: .ctor
//             ReturnType: Void
//             2 Arguments
//                     Argument #1:  Object
//                     Argument #2:  I
//             2 Parameters
//                     (1) ParamToken :Name : object flags: [none]
//                     (2) ParamToken : Name : method flags: [none]
//     Method #2
//     -------------------------------------------------------
//             MethodName: Invoke
//             ReturnType: I4
//             2 Arguments
//                     Argument #1:  I4
//                     Argument #2:  I4
//             2 Parameters
//                     (1) ParamToken : Name : x flags: [none]
//                     (2) ParamToken : Name : y flags: [none] //
//     Method #3
//     -------------------------------------------------------
//             MethodName: BeginInvoke
//             ReturnType: Class System.IAsyncResult
//             4 Arguments
//                     Argument #1:  I4
//                     Argument #2:  I4
//                     Argument #3:  Class System.AsyncCallback
//                     Argument #4:  Object
//             4 Parameters
//                     (1) ParamToken : Name : x flags: [none]
//                     (2) ParamToken : Name : y flags: [none]
//                     (3) ParamToken : Name : callback flags: [none]
//                     (4) ParamToken : Name : object flags: [none]
//
//     Method #4
//     -------------------------------------------------------
//             MethodName: EndInvoke
//             ReturnType: I4 (int32)
//             1 Arguments
//                     Argument #1:  Class System.IAsyncResult
//             1 Parameters
//                     (1) ParamToken : Name : result flags: [none]
```
Як бачите, згенерований компілятором клас BinaryOp визначає три публічні методи. Invoke() є ключовим методом у .NET, оскільки він використовується для синхронного виклику кожного методу, підтримуваного об’єктом делегату, тобто абонент повинен дочекатися завершення виклику, перш ніж продовжити свій шлях. Як не дивно, синхронний метод Invoke() може не потребувати явного виклику з вашого коду C#. Як ви побачите трохи пізніше, Invoke() викликається за лаштунками, коли ви використовуєте відповідний синтаксис C#.
Тепер, як саме компілятор знає, як визначити метод Invoke()? Щоб зрозуміти процес, ось суть типу класу BinaryOp, створеного компілятором:

```cs
sealed class BinaryOp : System.MulticastDelegate
{
  public int Invoke(int x, int y);
...
}
```
По-перше, зауважте, що параметри та тип повернення, визначені для методу Invoke(), точно відповідають визначенню делегату BinaryOp.
Давайте подивимося інший приклад. Припустімо, ви визначили тип делегату, який може вказувати на будь-який метод, що повертає рядок і отримує три вхідні параметри System.Boolean.

```cs
public delegate string MyDelegate (bool a, bool b, bool c);
```
Цього разу згенерований компілятором клас розбивається наступним чином:

```cs
sealed class MyDelegate : System.MulticastDelegate
{
  public string Invoke(bool a, bool b, bool c);
...
}
```
Делегати також можуть «вказувати» на методи, які містять будь-яку кількість параметрів out або ref (а також параметрів масиву, позначених ключовим словом params). Наприклад, припустимо такий тип делегата:

```cs
public delegate string MyOtherDelegate(
  out bool a, ref bool b, int c);
```
Сигнатура методу Invoke() виглядає так, як ви очікували. Підводячи підсумок, визначення типу делегату C# призводить до створення запечатаного класу зі згенерованим компілятором методом, параметри якого та типи повернення базуються на декларації делегата. Наступний псевдокод наближає базовий шаблон:

```cs
// This is only pseudo-code!
public sealed class DelegateName : System.MulticastDelegate
{
  public delegateReturnValue Invoke(allDelegateInputRefAndOutParams);
}
```

## Базові класи System.MulticastDelegate та System.Delegate

Отже, коли ви створюєте тип за допомогою ключового слова delegate, ви опосередковано оголошуєте тип класу, який походить від System.MulticastDelegate. Цей клас надає нащадкам доступ до списку, який містить адреси методів, підтримуваних об’єктом делегату, а також кілька додаткових методів (і кілька перевантажених операторів) для взаємодії зі списком викликів. Ось деякі вибрані члени System.MulticastDelegate:

```cs
public abstract class MulticastDelegate : Delegate
{
  // Returns the list of methods 'pointed to.'
  public sealed override Delegate[] GetInvocationList();
  // Overloaded operators.
  public static bool operator == (MulticastDelegate d1, MulticastDelegate d2);
  public static bool operator != (MulticastDelegate d1, MulticastDelegate d2);
  // Used internally to manage the list of methods maintained by the delegate.
  private IntPtr _invocationCount;
  private object _invocationList;
}
```
System.MulticastDelegate отримує додаткову функціональність від свого батьківського класу System.Delegate. Ось частковий знімок визначення класу:

```cs
public abstract class Delegate : ICloneable, ISerializable
{
  // Methods to interact with the list of functions.
  public static Delegate Combine(params Delegate[] delegates);
  public static Delegate Combine(Delegate a, Delegate b);
  public static Delegate Remove(Delegate source, Delegate value);
  public static Delegate RemoveAll(Delegate source, Delegate value);
  // Overloaded operators.
  public static bool operator == (Delegate d1, Delegate d2);
  public static bool operator != (Delegate d1, Delegate d2);
  // Properties that expose the delegate target.
  public MethodInfo Method { get; }
  public object Target { get; }
}
```
Тепер зрозумійте, що ви ніколи не можете напряму отримати від цих базових класів у своєму коді (це помилка компілятора). Тим не менш, коли ви використовуєте ключове слово delegate, ви опосередковано створюєте клас, який «is-a» MulticastDelegate.

Члени System.MulticastDelegate/System.Delegate

|Член|Значення в роботі|
|----|-----------------|
|Method|Ця властивість повертає об’єкт System.Reflection.MethodInfo, який представляє деталі статичного методу, який підтримує делегат.|
|Target|Якщо метод, який потрібно викликати, визначено на рівні об’єкта (а не статичний метод), Target повертає об’єкт, який представляє метод, підтримуваний делегатом. Якщо значення, що повертається з Target, дорівнює null, метод, який потрібно викликати, є статичним членом.|
|Combine()|Цей статичний метод додає метод до списку, який підтримує делегат. У C# цей метод запускається за допомогою перевантаженого оператора += як скороченого позначення.|
|GetInvocationList()|Цей метод повертає масив об’єктів System.Delegate, кожен з яких представляє метод, який можна викликати.|
|Remove()/RemoveAll()|Ці статичні методи видаляють метод (або всі методи) зі списку викликів делегату. У C# метод Remove() можна викликати опосередковано за допомогою перевантаженого оператора -=|

# Найпростіший можливий приклад делегату

Звісно, ​​делегати можуть викликати деяку плутанину, коли зустрічаються вперше. Таким чином, щоб зрушити з місця, давайте розглянемо просту консольну програму, під назвою SimpleDelegate, яка використовує тип делегату BinaryOp, який ви бачили раніше. Ось повний код із подальшим аналізом:

```cs
namespace SimpleDelegate;

class SimpleMath
{
    public static int Add(int a, int b) => a + b;
    public static int Subtract(int a, int b) => a - b;
}
```
Program.cs
```cs


using SimpleDelegate;

void UsingDelegate()
{
    // Create a BinaryOp delegate object that
    // 'points to' SimpleMath.Add().
    BinaryOp myAdd = new BinaryOp(SimpleMath.Add);

    // Invoke Add() method indirectly using delegate object.
    Console.WriteLine(myAdd(10,10));
    Console.WriteLine(myAdd.Invoke(10,20));
}
UsingDelegate();

// Additional type definitions must be placed at the end of the
// top-level statements

// This delegate can point to any method,
// taking two integers and returning an integer.
public delegate int BinaryOp(int x, int y);
```
```
20
30
```

Пам’ятайте, що додаткові оголошення типів (у цьому прикладі делегат BinaryOp) мають бути після всіх операторів верхнього рівня.

Знову ж таки, зверніть увагу на формат оголошення типу делегату BinaryOp; він визначає, що об’єкти-делегати BinaryOp можуть вказувати на будь-який метод, який приймає два цілих числа та повертає ціле число (фактична назва методу, на який вказується, не має значення). Якщо ви хочете призначити цільовий метод певному об’єкту делегату, просто передайте ім’я методу конструктору делегата.

```cs
    // Create a BinaryOp delegate object that
    // 'points to' SimpleMath.Add().
    BinaryOp myAdd = new BinaryOp(SimpleMath.Add);
```

На цьому етапі ви можете викликати член, на який вказано, за допомогою синтаксису, який виглядає як прямий виклик функції.

```cs
    // Invoke Add() method indirectly using delegate object.
    Console.WriteLine(myAdd(10,10));
```
Під капотом середовище виконання викликає згенерований компілятором метод Invoke() у вашому класі, похідному від MulticastDelegate. Ви можете переконатися в цьому самостійно, якщо відкриєте збірку в ildasm.exe і перевірите код CIL у методі Main().

```
.method private hidebysig static void '<Main>$'(string[] args) cil managed
{
...
  callvirt   instance int32 BinaryOp::Invoke(int32, int32)
}
```
C# не вимагає від вас явного виклику Invoke() у вашому коді. Оскільки BinaryOp може вказувати на методи, які приймають два аргументи, такий оператор коду також допустимий:

```cs
    Console.WriteLine(myAdd.Invoke(10,20));
```

Пам’ятайте, що делегати .NET безпечні для типів. Таким чином, якщо ви спробуєте створити об’єкт делегату, який вказує на метод, який не відповідає шаблону, ви отримаєте помилку під час компіляції. Для ілюстрації припустімо, що клас SimpleMath тепер визначає додатковий метод під назвою SquareNumber(), який приймає одне ціле число як вхідні дані.

```cs
class SimpleMath
{
    //...
    public static int SquareNumber(int a) => a * a;
}
```
Враховуючи, що делегат BinaryOp може вказувати лише на методи, які приймають два цілі числа та повертають ціле число, наступний код є незаконним і не компілюється:

```cs
    // Compile Error
    BinaryOp sQ = new BinaryOp(SimpleMath.SquareNumber);
```

## Дослідження об’єкта делегату

Давайте доповнимо поточний приклад, створивши статичний метод (під назвою DisplayDelegateInfo()) у файлі Program.cs. 

```cs
static void DisplayDelegateInfo(Delegate delObj)
{
    // Print the names of each member in the
    // delegate's invocation list.
    foreach (Delegate @delegate in delObj.GetInvocationList())
    {
        Console.WriteLine($"Method Name: {@delegate.Method}");
        Console.WriteLine($"Type Name: {@delegate.Target}");
    }
}

static void InvestigatingADelegateObject()
{
    BinaryOp bOp = new(SimpleMath.Add);
    DisplayDelegateInfo(bOp);
}
InvestigatingADelegateObject();
```
```
Method Name: Int32 Add(Int32, Int32)
Type Name:
```
Метод виводить назви методів, що підтримуються об’єктом делегату, а також назву класу, що визначає метод. Для цього ви перебираєте масив System.Delegate[], який повертає GetInvocationList(), викликаючи властивості Target і Method кожного об’єкта.
Зверніть увагу, що назва цільового класу (SimpleMath) наразі не відображається під час виклику властивості Target. Причина пов’язана з тим, що ваш делегат BinaryOp вказує на статичний метод і, отже, немає об’єкта для посилання. Однак якщо ви створите методи otherAdd() і otherSubtract(), щоб вони були нестатичними, ви можете створити екземпляр класу SimpleMath і вказати методи для виклику за допомогою посилання на об’єкт.
```cs
class SimpleMath
{
    //...
    public int otherAdd(int a, int b) => a + b;
}
```
```cs
static void InvestigatingADelegateObject()
{
    //...
    SimpleMath simpleMath = new();
    BinaryOp bop1 = new(simpleMath.otherAdd);
    DisplayDelegateInfo(bop1);
}
InvestigatingADelegateObject();
```
```
Method Name: Int32 Add(Int32, Int32)
Type Name:
Method Name: Int32 otherAdd(Int32, Int32)
Type Name: SimpleDelegate.SimpleMath
```

# Надсилання сповіщень про стан об’єкта за допомогою делегатів

Зрозуміло, що попередній приклад SimpleDelegate мав суто ілюстративний характер, враховуючи, що не було б вагомих причин визначати делегат просто для додавання двох чисел. Щоб забезпечити більш реалістичне використання типів делегатів, давайте використаємо делегати для визначення класу Car, який може інформувати зовнішні об’єкти про поточний стан двигуна. Для цього вам потрібно буде зробити наступне:

1. Визначте новий тип делегату, який використовуватиметься для надсилання сповіщень викликаючому коду.
2. Оголосіть змінну-член цього делегату в класі Car.
3. Створіть допоміжну функцію в Car, яка дозволить викликаючому коду вказати метод зворотного виклику.
4. Застосуйте метод Accelerate() в коді, щоб викликати список викликів делегату за правильних обставин. 

Для початку створіть новий проект консольної програми під назвою CarDelegate. Тепер визначте новий клас Car, який спочатку виглядає так: 

```cs
namespace CarDelegate;

class Car
{
    // Internal state data.
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; } = 100;
    public string PetName { get; set; } = string.Empty;

    // Is the car alive or dead?
    private bool _carIsDead;

    // Class constructors.
    public Car() { }

    public Car(string petName, int maxSpeed, int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        MaxSpeed = maxSpeed;
        PetName = petName;
    }
}
```
Тепер розглянемо наступні оновлення, які стосуються перших трьох пунктів:

```cs
class Car
{

    //...

    // 1) Define a delegate type.
    public  delegate void CarEngineHandler(string messageForCaller);

    // 2) Define a member variable of this delegate.
    private CarEngineHandler? _listOfHandlers;

    // 3) Add registration function for the caller.
    public void RegisterWithCarEngine(CarEngineHandler methodToCall)
    {
        _listOfHandlers = methodToCall;
    }
}
```

Зверніть увагу на те, що в цьому прикладі ви визначаєте типи делегатів безпосередньо в межах класу Car, що, звичайно, не є необхідним, але допомагає переконатися в тому, що делегат природно працює з цим класом. Тип делегату, CarEngineHandler, може вказувати на будь-який метод, який приймає один рядок як вхідні дані та void як значення, що повертається. Далі зауважте, що ви оголошуєте приватну змінну-член вашого типу делегату (під назвою _listOfHandlers) і допоміжну функцію (під назвою RegisterWithCarEngine()), яка дозволяє викликаючому коду призначити метод до списку викликів делегата.
На цьому етапі вам потрібно створити метод Accelerate(). Нагадаємо, тут йдеться про те, щоб дозволити об’єкту Car надсилати повідомлення, пов’язані з двигуном, будь-якому підписаному слухачу. Ось оновлення:

```cs
    // 4) Implement the Accelerate() method to invoke the delegate's
    //    invocation list under the correct circumstances.
    public void Accelerate(int delta)
    {
        // If this car is 'dead,' send dead message.
        if (_carIsDead)
        {
            _listOfHandlers?.Invoke("Sorry, this car is dead...");
        }
        else
        {
            CurrentSpeed += delta;
            // Is this car 'almost dead'?
            if ((MaxSpeed - CurrentSpeed) <= 10)
            {
                _listOfHandlers?.Invoke("Careful buddy! Gonna blow!");
            }

            if (CurrentSpeed >= MaxSpeed)
            {
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine($"CurrentSpeed = {CurrentSpeed}");
            }
        }
    }
```
Зверніть увагу, що ви використовуєте null синтаксис розповсюдження під час спроби викликати методи, які підтримуються змінною-членом _listOfHandlers. Причина полягає в тому, що завданням викликаючого коду буде розміщення цих об’єктів шляхом виклику допоміжного методу RegisterWithCarEngine(). Якщо зовнішній код не викликає цей метод, а ви намагаєтесь викликати список викликів делегата, під час виконання ви ініціюєте NullReferenceException. Тепер, коли у вас є делегована інфраструктура, спостерігайте за оновленнями файлу Program.cs, показаними тут:

```cs

using CarDelegate;

void DelegatesAsEventEnablers()
{
    // First, make a Car object.
    Car car = new Car("SlugBug",100,10);

    // Now, tell the car which method to call
    // when it wants to send us messages.
    car.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEvent));

    // Speed up (this will trigger the events).
    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}

static void OnCarEngineEvent(string message)
{
    Console.WriteLine("\n*** Message From Car Object ***");
    Console.WriteLine($"=> {message}");
    Console.WriteLine("********************\n");
}
```
Код починається з простого створення нового об’єкта Car. Оскільки вам цікаво дізнатися про події двигуна, наступним кроком є ​​виклик вашої спеціальної функції реєстрації RegisterWithCarEngine(). Цей метод очікує передачі екземпляра вкладеного делегату CarEngineHandler, і, як і з будь-яким делегатом, ви вказуєте «метод, на який потрібно вказувати» як параметр конструктора. Хитрість у цьому прикладі полягає в тому, що відповідний метод знаходиться у файлі Program.cs. Знову ж таки, зауважте, що метод OnCarEngineEvent() повністю відповідає пов’язаному делегату, оскільки він приймає рядок як вхідні дані та повертає void. Розглянемо результат поточного прикладу:

```
***** Speeding up *****
CurrentSpeed = 30
CurrentSpeed = 50
CurrentSpeed = 70

*** Message From Car Object ***
=> Careful buddy! Gonna blow!
********************

CurrentSpeed = 90

*** Message From Car Object ***
=> Careful buddy! Gonna blow!
********************


*** Message From Car Object ***
=> Sorry, this car is dead...
********************
```

## Увімкнення багатоадресної розсилки(Multicasting)

Нагадаємо, делегати .NET мають вбудовану можливість багатоадресної передачі. Іншими словами, об’єкт делегату може підтримувати список методів для виклику, а не лише один метод. Якщо ви хочете додати кілька методів до об’єкта делегату, ви просто використовуєте перевантажений оператор +=, а не пряме призначення. Щоб увімкнути групову розсилку для класу Car, ви можете оновити метод RegisterWithCarEngine() таким чином:

```cs
    // Now with multicasting support!
    // Note we are now using the += operator, not
    // the assignment operator (=).
    public void RegisterWithCarEngine(CarEngineHandler methodToCall)
    {
        _listOfHandlers += methodToCall;
    }
```
Коли ви використовуєте оператор += для об’єкта делегату, компілятор перетворює це на виклик статичного методу Delegate.Combine(). Фактично, ви можете викликати Delegate.Combine() безпосередньо; однак оператор += пропонує простішу альтернативу. Немає необхідності змінювати поточний метод RegisterWithCarEngine(), але ось приклад використання Delegate.Combine() замість оператора +=:

```cs
public void RegisterWithCarEngine( CarEngineHandler methodToCall )
{
  if (_listOfHandlers == null)
  {
    _listOfHandlers = methodToCall;
  }
  else
  {
    _listOfHandlers =
      Delegate.Combine(_listOfHandlers, methodToCall)
        as CarEngineHandler;
  }
}
```
У будь-якому випадку викликаючий код тепер може зареєструвати кілька цілей для одного сповіщення зворотного виклику. Тут другий обробник друкує вхідне повідомлення у верхньому регістрі лише для відображення:

```cs
static void OnCarEngineEventToUpper(string message)
{
    Console.WriteLine($"=> {message.ToUpper()}");
}

```
```cs
    // Now, tell the car which method to call
    // when it wants to send us messages.
    car.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEvent));
    car.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEventToUpper));
```

## Видалення цільових методів зі списку викликів делегата

Клас Delegate також визначає статичний метод Remove(), який дозволяє викликаючому коду динамічно видаляти метод зі списку викликів об’єкта делегату. Це дозволяє просто дозволити абоненту «скасувати підписку» на дане сповіщення під час виконання. Хоча ви можете викликати Delegate.Remove() безпосередньо в коді, розробники C# можуть використовувати оператор -= як зручний скорочений запис. Давайте додамо новий метод до класу Car, який дозволяє абоненту видалити метод зі списку викликів.

```cs
class Car
{
 //...
    public void UnRegisterWithCarEngine(CarEngineHandler methodName)
    {
        _listOfHandlers -= methodName;
    }
 //...
}    
```
З поточними оновленнями класу автомобіля ви можете припинити отримання сповіщень двигуна на другому обробнику, оновивши код виклику таким чином:

```cs
void DelegatesAsEventEnablers()
{

    //...
    Car.CarEngineHandler handler2 = OnCarEngineEventToUpper;
    car.RegisterWithCarEngine(handler2);
    car.UnRegisterWithCarEngine(handler2);
    //...
}
DelegatesAsEventEnablers();

//...

static void OnCarEngineEventToUpper(string message)
{
    Console.WriteLine($"=> {message.ToUpper()}\n");
}

```
```
***** Speeding up *****
CurrentSpeed = 30
CurrentSpeed = 50
CurrentSpeed = 70

*** Message From Car Object ***
=> Careful buddy! Gonna blow!
********************

CurrentSpeed = 90

*** Message From Car Object ***
=> Careful buddy! Gonna blow!
********************


*** Message From Car Object ***
=> Sorry, this car is dead...
********************
```
Одна відмінність у цьому коді полягає в тому, що цього разу ви створюєте об’єкт Car.CarEngineHandler і зберігаєте його в локальній змінній, щоб ви могли використовувати цей об’єкт для скасування реєстрації зі сповіщенням пізніше. Таким чином об'ект делегати зберігає сиписок з якото таким чином можна виділити посилання на метод і в результаті він не буде запускатись при випадку події.

## Синтаксис перетворення групи методів

У попередньому прикладі CarDelegate ви явно створили екземпляри об’єкта делегату Car.CarEngineHandler для реєстрації та скасування реєстрації в сповіщеннях системи.

```cs
    Car car = new Car("SlugBug",100,10);

    car.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEvent));
    car.RegisterWithCarEngine(new Car.CarEngineHandler(OnCarEngineEventToUpper));

    Car.CarEngineHandler handler2 = OnCarEngineEventToUpper;
    car.RegisterWithCarEngine(handler2);
    car.UnRegisterWithCarEngine(handler2);
```

Безсумнівно, якщо вам потрібно викликати будь-який із успадкованих членів MulticastDelegate або Delegate, створення змінної делегату вручну є найпростішим способом зробити це. Однак у більшості випадків вам не потрібно затримуватися на об’єкті делегату. Натомість вам зазвичай потрібно використовувати об’єкт делегату лише для передачі імені методу як параметра конструктора. Для спрощення C# надає скорочений спосіб, який називається перетворенням групи методів. Ця функція дозволяє вказати ім’я прямого методу, а не об’єкт делегату, під час виклику методів, які приймають делегати як аргументи.
Для ілюстрації розглянемо наступні оновлення файлу Program.cs, який використовує перетворення групи методів для реєстрації та скасування реєстрації в сповіщеннях системи:

```cs
static void DelegatesAsEventEnablers1()
{

    Car car = new Car("SlugBug", 100, 10);

    car.RegisterWithCarEngine(OnCarEngineEvent);

    car.RegisterWithCarEngine(OnCarEngineEventToUpper);
    car.UnRegisterWithCarEngine(OnCarEngineEventToUpper);

    Console.WriteLine("***** Speeding up *****");
    for (int i = 0; i < 6; i++)
    {
        car.Accelerate(20);
    }
}
DelegatesAsEventEnablers1();
```
Зверніть увагу, що ви не розподіляєте напряму пов’язаний об’єкт делегату, а просто вказуєте метод, який відповідає очікуваній сигнатурі делегату (у цьому випадку метод повертає void і приймає один рядок). Зрозумійте, що компілятор C# все ще забезпечує безпеку типів. Таким чином, якщо метод не приймає рядок і не повертає void, вам буде видано помилку компілятора.

# Розуміння узагальнених делегатів

C# дозволяє визначати узагальнені типи делегатів. Наприклад, припустімо, що ви хочете визначити тип делегату, який може викликати будь-який метод, що повертає void і отримує один параметр. Якщо розглянутий аргумент може відрізнятися, ви можете змоделювати це за допомогою параметра типу. Для ілюстрації розглянемо наступний код у новому проекті консольної програми під назвою GenericDelegate:

```cs
// Methods
static void StringTarget(string arg)
{
    Console.WriteLine($"arg in uppercase is: {arg.ToUpper()}");
}
static void IntTarget(int arg)
{
    Console.WriteLine($"++arg is: {++arg}");
}

// Using the generic delegate.

// Register targets.

MyGenericDelegate<string> strTarget = new(StringTarget);
strTarget("hi everyone");

MyGenericDelegate<int> intTarget = new(IntTarget);
intTarget(50);


// This generic delegate can represent any method
// returning void and taking a single parameter of type T.
public delegate void MyGenericDelegate<T>(T arg);
```
```
arg in uppercase is: HI EVERYONE
++arg is: 51
```
Зверніть увагу, що MyGenericDelegate<T> визначає єдиний параметр типу, який представляє аргумент для передачі цільовому делегату. При створенні екземпляра цього типу необхідно вказати значення параметра типу, а також ім’я методу, який буде викликати делегат. Таким чином, якщо ви вказали тип рядка, ви надсилаєте значення рядка цільовому методу. Враховуючи формат об’єкта strTarget, метод StringTarget() тепер має приймати один рядок як параметр.

## Узагальнени делегати Action<> і Func<>

У цій главі ви бачили, що якщо ви хочете використовувати делегати для ввімкнення зворотних викликів у ваших програмах, ви зазвичай виконуєте наведені тут дії:

1. Визначте спеціальний делегат, який відповідає формату методу, на який вказується.
2. Створіть екземпляр свого спеціального делегату, передавши назву методу як аргумент конструктора.
3. Викличте метод опосередковано через виклик Invoke() для об’єкта делегату.

Коли ви використовуєте цей підхід, ви зазвичай отримуєте кілька користувальницьких делегатів, які ніколи не використовуватимуться крім поточного завдання (наприклад, MyGenericDelegate<T>, CarEngineHandler тощо). Хоча вам справді може знадобитися спеціальний тип делегату з унікальною назвою для вашого проекту, в інших випадках точна назва типу делегату не має значення. У багатьох випадках вам просто потрібен «якийсь делегат», який приймає набір аргументів і, можливо, має повертане значення, відмінне від void. У цих випадках ви можете використовувати вбудовані в фреймворк типи делегатів Action<> і Func<>. Щоб проілюструвати їх корисність, створіть новий проект консольної програми під назвою ActionAndFuncDelegates.
Загальний делегат Action<> визначено в просторі імен System, і ви можете використовувати цей загальний делегат, щоб «вказати» на метод, який приймає до 16 аргументів (цього має бути достатньо!) і повертає void. Тепер згадайте, оскільки Action<> є загальним делегатом, вам також потрібно буде вказати основні типи кожного параметра. 
Оновіть файл Program.cs, щоб визначити новий статичний метод, який приймає три унікальних параметра. Ось приклад:

```cs
//Methods
static void DisplayMessage(string message, ConsoleColor color, int printCount)
{
    ConsoleColor previousColor = Console.ForegroundColor;
    
    Console.ForegroundColor = color;
    for (int i = 0; i < printCount; i++)
    {
        Console.WriteLine(message);
    }

    Console.ForegroundColor = previousColor;
}
```

Тепер замість того, щоб вручну створювати спеціальний делегат для передачі потоку програми метод DisplayMessage(), ви можете використати готовий делегат Action<>, наприклад:

```cs
static void UsingActionDelegate()
{
    // Use the Action<> delegate to point to DisplayMessage.
    Action<string, ConsoleColor, int> actionTarget = DisplayMessage;
    actionTarget("Hi", ConsoleColor.Yellow, 3);
}
UsingActionDelegate();
```
```
Hi
Hi
Hi
```

Як бачите, використання делегату Action<> позбавить вас від клопоту визначення власного типу делегату. Однак пам’ятайте, що тип делегату Action<> може вказувати лише на методи, які приймають значення, що повертається void. Якщо ви хочете вказати на метод, який має значення, що повертається (і не хочете самостійно писати власний делегат), ви можете використовувати Func<>. Загальний делегат Func<> може вказувати на методи, які приймають до 16 параметрів і настроюване значення, що повертається. Для ілюстрації додайте такий новий метод до файлу Program.cs:

```cs
static int Add(int x, int y)
{
    return x + y;
}
```
Раніше в цьому розділі я попросив вас створити власний делегат BinaryOp, щоб «вказувати» на методи додавання та віднімання. Однак ви можете спростити свої зусилля, використовуючи версію Func<>, яка приймає загалом три параметри типу. Майте на увазі, що кінцевий параметр типу Function<> завжди є значенням, яке повертає метод. Щоб підтвердити це, припустимо, що файл Program.cs також визначає такий метод:

```cs
static string SumToString(int x, int y)
{
    return (x + y).ToString();
}
```
Тепер код виклику може викликати кожен із цих методів, наприклад:

```cs
static void UsingFuncDelegate()
{
    Func<int, int, int> funcSumInt = Add;
    Console.WriteLine(funcSumInt(1, 1));

    Func<int, int, string> funcSumString = SumToString;
    Console.WriteLine(funcSumString(1, 2) is String);
}
UsingFuncDelegate();
```
```
2
True
```
У будь-якому випадку, враховуючи те, що Action<> і Func<> можуть заощадити вам крок ручного визначення власного делегату, ви можете задатися питанням, чи варто вам використовувати їх постійно. Відповідь, як і на багато інших аспектів програмування, така: «it depends». У багатьох випадках Action<> і Func<> будуть кращими варіантами дій. Однак, якщо вам потрібен делегат із спеціальним ім’ям, яке, на вашу думку, допомагає краще охопити ваш проблемний домен, створити спеціальний делегат так само просто, як один оператор коду.
Багато важливих API .NET значною мірою використовують делегати Action<> і Func<>, включаючи структуру паралельного програмування та LINQ (серед іншого).


# Чому не варто визначати зміну-член делегата public.

Делегати є цікавими конструкціями, оскільки вони дозволяють об’єктам у пам’яті брати участь у двосторонньому спілкуванні. Однак робота з делегатами в необробленому вигляді може призвести до створення шаблонного коду (визначення делегату, оголошення необхідних змінних-членів, створення спеціальних методів реєстрації та скасування реєстрації для збереження інкапсуляції тощо).
Більше того, коли ви використовуєте делегати безпосередьньо як механізм зворотного виклику вашої програми, якщо ви не визначите змінні члена делегату класу як приватні, код виклику матиме прямий доступ до об’єктів делегату. У цьому випадку викликаючий код міг би перепризначити змінну новому об’єкту делегату (фактично видаливши поточний список функцій для виклику), і, що ще гірше, він міг би безпосередньо викликати список викликів делегата. Щоб продемонструвати цю проблему, створіть нову консольну програму під назвою PublicDelegateProblem і додайте таку переробку (і спрощення) класу Car з попереднього прикладу CarDelegate:

```cs
namespace PublicDelegateProblem;

public class Car
{
    public delegate void CarEngineHandler(string messageForCaller);

    // Now a public member!
    public CarEngineHandler? ListOfHandler;

    // Just fire out the Exploded notification.
    public void Accelerate(int delta)
    {
        if (ListOfHandler != null)
        {
            ListOfHandler("Sorry, this car is dead...");
        } 
    }
}
```
Зверніть увагу, що у вас більше немає приватних змінних делегатів-членів, інкапсульованих за допомогою спеціальних методів реєстрації. Оскільки ці члени справді є загальнодоступними, викликаючий код може отримати прямий доступ до змінної-члена ListOfHandlers і перепризначити цей тип новим об’єктам CarEngineHandler і викликати делегат, коли забажає.

```cs
// Methods 
using PublicDelegateProblem;

static void CallWhenExploded(string message)
{
    Console.WriteLine(message);
}
static void CallHereToo(String message)
{
    Console.WriteLine(message);
}

// Using
static void MakeProblem()
{
    Car car = new();
    // We have direct access to the delegate!
    car.ListOfHandler = CallWhenExploded;
    car.Accelerate(10);

    // We can now assign to a whole new object...
    // confusing at best.
    car.ListOfHandler = CallHereToo;
    car.Accelerate(10);

    // The caller can also directly invoke the delegate!
    car.ListOfHandler.Invoke("hee, hee, hee...");
}
MakeProblem();
```
```
Sorry, this car is dead...
Sorry, this car is dead...
hee, hee, hee...
```
Відкриття публічних делегатів порушує інкапсуляцію, що може не лише призвести до коду, який важко підтримувати (і налагоджувати), але також може створити для вашої програми можливі ризики безпеці!
Очевидно, що ви не хочете надавати іншим програмам повноваження змінювати те, на що вказує делегат, або викликати учасників без вашого дозволу. Враховуючи це, загальноприйнятою практикою є оголошення приватних змінних делегатів-членів.

На цьому наш перший погляд на тип делегату завершується. Далі перейдемо до пов’язаної теми ключового слова C# event.