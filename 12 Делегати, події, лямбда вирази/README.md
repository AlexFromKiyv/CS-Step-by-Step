# Делегати

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









