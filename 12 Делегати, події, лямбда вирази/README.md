 # Делегати, події, лямбда вирази

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

Отже, коли ви створюєте тип за допомогою ключового слова делегату C#, ви опосередковано оголошуєте тип класу, який походить від System.MulticastDelegate. Цей клас надає нащадкам доступ до списку, який містить адреси методів, підтримуваних об’єктом делегату, а також кілька додаткових методів (і кілька перевантажених операторів) для взаємодії зі списком викликів. Ось деякі вибрані члени System.MulticastDelegate:

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
System.MulticastDelegate отримує додаткову функціональність від свого батьківського класу System.Delegate. Ось частковий знімок визначення класу:

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