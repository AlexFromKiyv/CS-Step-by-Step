# Late Binding

Простіше кажучи, Late Binding (пізнє зв’язування) — це техніка, за допомогою якої ви можете створити екземпляр заданого типу та викликати його члени під час виконання, не маючи жорстко закодованого під час компіляції знання про його існування.
Коли ви створюєте програму, яка пізно прив’язується до типу у зовнішній збірці, у вас немає причин встановлювати посилання на збірку; отже, маніфест клієнта не містить прямого списку збірки. Пізнє зв’язування відіграє вирішальну роль у будь-якому розширюваному додатку, який ви створюєте.

## Клас Avtivator

```cs
// This program will load an external library,
// and create an object using late binding.
using System.Reflection;

void Run()
{
    Assembly? assembly = null;
    try
    {
        assembly = Assembly.LoadFrom(@"D:\CarLibrary");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    if (assembly != null)
    {
        CreateUsingLateBinding(assembly);
    }

    void CreateUsingLateBinding(Assembly assembly)
    {
        object? obj;
        try
        {
            Type? miniVan = assembly.GetType("CarLibrary.MiniVan");
            if (miniVan != null)
            {
                obj = Activator.CreateInstance(miniVan);
                Console.WriteLine($"Created a {obj} using late binding!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
Run();
```
```
Created a CarLibrary.MiniVan using late binding!
```
Метод Activator.CreateInstance() дозволяє створення екземпляра типу через пізнє зв’язування. Цей метод багато разів перевантажували, щоб забезпечити значну гнучкість.
Найпростіший варіант CreateInstance() приймає дійсний об’єкт Type, який описує сутність, яку ви хочете виділити в пам’ять на льоту.
Однак, оскільки ваша програма не додала посилання на CarLibrary.dll, ви не можете використовувати ключове слово C# using для імпорту простору імен CarLibrary, а отже, ви не можете використовувати тип MiniVan під час операції приведення!
Cуть пізнього зв’язування полягає у створенні екземплярів об’єктів, для яких немає знань під час компіляції. Враховуючи це, як можна викликати базові методи об’єкта MiniVan, що зберігається в посиланні System.Object? Відповідь, звичайно, полягає в використанні рефлексії.

### Виклик методів у створеного об'єкта.

Припустімо, ви хочете викликати метод TurboBoost() MiniVan.

```cs
   void CreateUsingLateBinding(Assembly assembly)
   {
       object? obj;
       try
       {
           Type? miniVan = assembly.GetType("CarLibrary.MiniVan");
           if (miniVan != null)
           {
               obj = Activator.CreateInstance(miniVan);
               Console.WriteLine($"Created a {obj} using late binding!");

               MethodInfo? methodInfo = miniVan.GetMethod("TurboBoost");
               
               //Invoke method ('null' for no parameters)
               methodInfo?.Invoke(obj, null);
           }
       }
       catch (Exception ex)
       {
           Console.WriteLine(ex.Message);
       }
   }
```
Першим кроком є отримання об’єкта MethodInfo для методу TurboBoost() за допомогою Type.GetMethod(). З отриманої MethodInfo ви зможете викликати MiniVan.TurboBoost за допомогою Invoke(). MethodInfo.Invoke() вимагає від вас надіслати всі параметри, які мають бути надані методу, представленому MethodInfo. Ці параметри представлені масивом типів System.Object (оскільки параметри для даного методу можуть бути будь-якою кількістю різних сутностей).

Якщо ви хочете використати пізнє зв’язування для виклику методу, який потребує параметрів, вам слід запакувати аргументи як масив об’єктів із вільною типізацією.