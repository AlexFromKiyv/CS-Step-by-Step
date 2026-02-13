# Dynamic Assemblies

За визначенням, статичні збірки — це двійкові файли .NET, які завантажуються безпосередньо з дискового сховища, тобто вони розташовані десь у фізичному файлі (або, можливо, у наборі файлів у випадку багатофайлової збірки) у момент, коли CLR запитує їх. Як ви могли здогадатися, кожного разу, коли ви компілюєте вихідний код C#, ви отримуєте статичну збірку.
Динамічна збірка створюється в пам’яті на льоту, використовуючи типи, надані простором імен System.Reflection.Emit. Простір імен System.Reflection.Emit дає змогу створювати збірку та її модулі, визначення типів і логіку реалізації CIL під час виконання. Після того як ви це зробите, ви можете зберегти двійковий файл у пам’яті на диск. Це, звичайно, призводить до нової статичної збірки. Правда, процес створення динамічної збірки з використанням простору імен System.Reflection.Emit вимагає певного рівня розуміння природи кодів операцій CIL.
Хоча створення динамічних збірок є складним (і незвичайним) завданням програмування, вони можуть бути корисними за різних обставин. Ось приклад:

    Ви створюєте інструмент програмування .NET, якому потрібно генерувати збірки на вимогу на основі введених користувачем даних.

    Ви створюєте програму, яка має генерувати проксі для віддалених типів на льоту на основі отриманих метаданих.

    Ви хочете завантажити статичну збірку та динамічно вставляти нові типи в бінарний файл.


## Дослідження System.Reflection.Emit.

Створення динамічної збірки вимагає від вас певного знайомства з кодами операцій CIL, але типи простору імен System.Reflection.Emit максимально приховують складність CIL. Наприклад, замість того, щоб вказувати необхідні директиви CIL і атрибути для визначення типу класу, ви можете просто використовувати клас TypeBuilder. Подібним чином, якщо ви хочете визначити новий конструктор рівня екземпляра, вам не потрібно випускати маркер specialname, rtspecialname або .ctor; швидше, ви можете використовувати ConstructorBuilder.

Ключові члени простору імен System.Reflection.Emit.

|Член|Використання|
|----|------------|
|AssemblyBuilder|Використовується для створення збірки (*.dll або *.exe) під час виконання. *.exe має викликати метод ModuleBuilder.SetEntryPoint(), щоб установити метод, який є точкою входу до модуля. Якщо точка входу не вказана, буде згенеровано *.dll.|
|ModuleBuilder|Використовується для визначення набору модулів у поточній збірці.|
|EnumBuilder|Використовується для створення типу enum .NET.|
|TypeBuilder|Може використовуватися для створення класів, інтерфейсів, структур і делегатів у модулі під час виконання.|
|MethodBuilder LocalBuilder PropertyBuilder FieldBuilder ConstructorBuilder CustomAttributeBuilder ParameterBuilder EventBuilder|Використовується для створення членів типу (таких як методи, локальні змінні, властивості, конструктори та атрибути) під час виконання.|
|ILGenerator|Видає коди операцій CIL у певний елемент типу.|
|OpCodes|Надає численні поля, які відображаються на коди операцій CIL. Цей тип використовується в поєднанні з різними членами System.Reflection.Emit.ILGenerator.|
  
Загалом, типи простору імен System.Reflection.Emit дозволяють програмно представляти необроблені маркери CIL під час створення динамічної збірки.

## Вибудова дінамічної збірки.

Щоб проілюструвати процес визначення збірки .NET Core під час виконання, давайте розглянемо процес створення однофайлової динамічної збірки. Ця збірка буде мати можливісті яку дозволяють створювати об'єкт наступного класу.

```cs
// This class will be created at runtime
// using System.Reflection.Emit.
public class HelloWorld
{
  private string theMessage;
  HelloWorld() {}
  HelloWorld(string s) {theMessage = s;}
  public string GetMsg() {return theMessage;}
  public void SayHello()
  {
    System.Console.WriteLine("Hello from the HelloWorld class!");
  }
}
```
Клас HelloWorld підтримує конструктор за замовчуванням і спеціальний конструктор, який використовується для призначення значення приватної змінної-члена (theMessage) типу string. Крім того, HelloWorld підтримує загальнодоступний метод екземпляра під назвою SayHello(), який друкує привітання до стандартного потоку вводу-виводу, та інший метод екземпляра під назвою GetMsg(), який повертає внутрішній приватний рядок.

Створемо проект ExploringSystemReflectionEmit і додамо до нього NuGet пакет System.Reflection.Emit. Далі імпортуємо необхідні простори імен.
```cs
using System.Reflection;
using System.Reflection.Emit;
```
Створимо статичний метод який буде виконувати:

    Визначення характеристик динамічної збірки (назви, версії тощо)
    Реалізація типу HelloClass
    Повертати AssemblyBuilder до методу виклику


### Визначення метаданих збірки.

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{
    AssemblyName assemblyName = new AssemblyName
    {
        Name = "HelloWorldAssembly",
        Version = new Version("1.0.0.0")
    };
    // Create builder to make assembly.
    var builder = AssemblyBuilder.DefineDynamicAssembly(assemblyName,
        AssemblyBuilderAccess.Run);
    return builder;
}
```
Спочатку виставляються мінімальні характеристики збірки за допомогою типу AssemblyName. Далі отримуємо тип AssemblyBuilder за допомогою статичного методу AssemblyBuilder.DefineDynamicAssembly().
Викликаючи DefineDynamicAssembly(), ви повинні вказати режим доступу до збірки, яку ви хочете визначити, найбільш поширені значення:

    RunAndCollect : Збірку буде негайно вивантажено, а її пам’ять буде відновлено, коли вона стане недоступною.

    Run : Це означає, що динамічна збірка може бути виконана в пам'яті, але не збережена на диску.

### Визначення модуля за допомогою ModuleBuilder.

Наступним завданням є визначення набору модулів для вашої нової збірки. 

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{
        AssemblyName assemblyName = new AssemblyName
        {
            Name = "HelloWorldAssembly",
            Version = new Version("1.0.0.0")
        };
        // Create new assembly.
        var builder = AssemblyBuilder.DefineDynamicAssembly(assemblyName,
            AssemblyBuilderAccess.Run);

    // Define the name of the module.
    ModuleBuilder moduleBuilder = builder.DefineDynamicModule("HelloWorldAssembly");

    return builder;
}
```

ModuleBuilder є ключовим типом, який використовується під час розробки динамічних збірок. ModuleBuilder підтримує кілька членів, які дозволяють визначати набір типів, що містяться в даному модулі (класи, інтерфейси, структури тощо), а також набір вбудованих ресурсів (таблиці рядків, зображення тощо). Ось його два методи:

|Член|Використання|
|----|------------|
|DefineEnum()|Використовується для створення визначення Enum .NET.|
|DefineType()|Створює TypeBuilder, який дозволяє визначати типи значень, інтерфейси та типи класів (включаючи делегати).|

Ключовим членом класу ModuleBuilder, про який слід знати, є DefineType(). Окрім визначення імені типу, ви також будете використовувати перелік System.Reflection.TypeAttributes для опису формату самого типу. Деякі (але не всі) ключові члени переліку TypeAttributes :

|Член|Використання|
|----|------------|
|Abstract|Вказує, що тип є абстрактним.|
|Class|Вказує, що тип є класом.|
|Interface|Вказує, що тип є інтерфейсом.|
|NestedAssembly|Вказує, що клас є вкладеним із видимістю збірки і, отже, доступний лише за допомогою методів у своїй збірці|
|NestedFamANDAssem|Вказує, що клас є вкладеним у видимість збірки та сімейства, і тому доступний лише методам, що знаходяться на перетині його сімейства та збірки.|
|NestedFamily|Вказує, що клас є вкладеним із видимістю сімейства і, таким чином, доступний лише методами в межах свого власного типу та будь-яких підтипів.|
|NestedFamORAssem|Вказує, що клас є вкладеним із видимістю сімейства або збірки і, таким чином, доступний лише методам, що лежать в об’єднанні його сімейства та збірки.|
|NestedPrivate|Вказує, що клас є вкладеним із приватною видимістю.|
|NestedPublic|Вказує, що клас є вкладеним із публічною видимістю.|
|NotPublic|Вказує, що клас не є публічним.|
|Public|Вказує, що клас є публічним.|
|Sealed|Вказує, що клас запечатаний і не може бути розширений.|
|Serializable|Вказує, що клас можна серіалізувати.|


### Визначення класу та приватного строкового поля.

Маючи визначний модуль можна визначити в ньому клас та поле.

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{

    ...

    // Define the name of the module.
    ModuleBuilder moduleBuilder = builder.DefineDynamicModule("HelloWorldAssembly");

    // Define a public class
    TypeBuilder typeHelloWordBuilder =
        moduleBuilder.DefineType("HelloWorldAssembly.HelloWorld", TypeAttributes.Public);

    // Define a private String
    FieldBuilder fieldMessageBuilder =
        typeHelloWordBuilder.DefineField(
            "theMessage",
            typeof(string),
            FieldAttributes.Private); 

    return builder;
}

```
Метод об'єкта типу ModuleBuilder DefineType повертає TypeBuilder. Цей тип має методи для визаченя членів типу і зокрема дозволяє отримати FieldBuilder. Аналогічно за допомогою типу TypeBuilder можна визначати конструктори, властивості, методи і інші члени.

### Визначення конструкторів та роль System.Reflection.Emit.ILGenerator 

Метод TypeBuilder.DefineConstructor() можна використовувати для визначення конструктора для поточного типу.

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{

    ...

    // Create the custom ctor taking single string arg.
    Type[] constructorArgs = [typeof(string)];
    ConstructorBuilder constructorBuilder = typeHelloWordBuilder.DefineConstructor(
        MethodAttributes.Public,
        CallingConventions.Standard,
        constructorArgs);

    return builder;
}
```

Однак, коли мова йде про реалізацію конструктора HelloWorld, вам потрібно вставити необроблений CIL-код у тіло конструктора, який відповідає за призначення вхідного параметра внутрішньому приватному рядку. Це може зробити ILGenerator. 

Роль типу ILGenerator полягає у впровадженні кодів операцій CIL у заданий член типу. Однак ви не можете безпосередньо створювати об’єкти ILGenerator, оскільки цей тип не має відкритих конструкторів; скоріше, ви отримуєте тип ILGenerator шляхом виклику конкретних методів типів, орієнтованих на конструктор (таких як типи MethodBuilder і ConstructorBuilder).

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{

    ...

    // Create the custom ctor taking single string arg.
    Type[] constructorArgs = [typeof(string)];
    ConstructorBuilder constructorBuilder = typeHelloWordBuilder.DefineConstructor(
        MethodAttributes.Public,
        CallingConventions.Standard,
        constructorArgs);

    ILGenerator constructorIl = constructorBuilder.GetILGenerator();

    return builder;
}
```
Маючи ILGenerator, ви зможете видавати необроблені коди операцій CIL за допомогою будь-якої кількості методів. Ось деякі з них:

|Метод|Використання|
|BeginCatchBlock()|Почати блок catch|
|BeginExceptionBlock()|Почати межу коду для exception|
|BeginFinallyBlock()|Почати блок fynally|
|BeginScope()|Починається лексичний обсяг коду|
|DeclareLocal()|Оголошує локальну змінну|
|DefineLabel()| Оголошує нову мітку|
|Emit()|Багато разів перевантажується, щоб дозволити видавати коди операцій CIL|
|EmitCall()|Надсилає виклик або код операції callvirt у потік CIL|
|EmitWriteLine()|Викликає Console.WriteLine() із різними типами значень|
|EndExceptionBlock()|Кінець блоку exception|
|EndScope()|Кінец лексичний обсяг коду|
|ThrowException()|Видає інструкцію для створення винятку|
|UsingNamespace()|Визначає простір імен, який буде використовуватися під час оцінювання локальних значень, і стежить за поточною активною лексичною областю|

Ключовим методом ILGenerator є Emit(), який працює в поєднанні з типом класу System.Reflection.Emit.OpCodes. Тип генратора надає велику кількість полів лише для читання, які відображаються на необроблені коди операцій CIL.

Додамо реалізацію конструктора

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{
    ...

    // Create the custom ctor taking single string arg.
    Type[] constructorArgs = [typeof(string)];
    ConstructorBuilder constructorBuilder = typeHelloWordBuilder.DefineConstructor(
        MethodAttributes.Public,
        CallingConventions.Standard,
        constructorArgs);

    ILGenerator constructorIl = constructorBuilder.GetILGenerator();

    //Emit the necessary CIL into the ctor
    constructorIl.Emit(OpCodes.Ldarg_0);
    Type objectClass = typeof(object);
    ConstructorInfo superConstructor =
        objectClass.GetConstructor(new Type[0])!;
    constructorIl.Emit(OpCodes.Call, superConstructor);
    //Load this pointer onto the stack
    constructorIl.Emit(OpCodes.Ldarg_0);
    constructorIl.Emit(OpCodes.Ldarg_1);
    //Load argument on virtual stack and store in msdField
    constructorIl.Emit(OpCodes.Stfld, fieldMessageBuilder);
    constructorIl.Emit(OpCodes.Ret);


    return builder;
}
```
Метод Emit() класу ILGenerator є сутністю, яка відповідає за розміщення CIL у реалізації члена. Сам Emit() часто використовує тип класу OpCodes, який розкриває набір кодів операції CIL за допомогою полів лише для читання. Наприклад, OpCodes.Ret сигналізує про повернення виклику методу, OpCodes.Stfld виконує призначення змінної-члена, а OpCodes.Call використовується для виклику заданого методу (у цьому випадку конструктора базового класу). 
Тепер, як ви знаєте, щойно ви визначаєте спеціальний конструктор для типу, типовий конструктор мовчки видаляється. Додамо конструктор без аргументів, просто викличте метод DefineDefaultConstructor().

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{
    ...
    // Create the default constructor.
    typeHelloWordBuilder.DefineDefaultConstructor(
      MethodAttributes.Public);

    return builder;
}
```

### Визначення методу.

Створення методу нагадує створеня конструктора. Спочатку у об'єкта TypeBuilder викликаємо метод DefineMethod і отримуємо MethodBuilder. Потім з нього отримуємо ILGenerator за допомогою якого вставняємо інструкції CIL в тіло методу.

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{
   ...

    // Create the SayHello method.
    MethodBuilder methodSayHiBuilder = typeHelloWordBuilder.
        DefineMethod("SayHello", MethodAttributes.Public,null,null);

    ILGenerator methodIl = methodSayHiBuilder.GetILGenerator();
    methodIl.EmitWriteLine("Hello from the HelloWorld class!");
    methodIl.Emit(OpCodes.Ret);
    
    return builder;

}
```
Тут ви створили загальнодоступний метод (MethodAttributes.Public), який не приймає параметрів і нічого не повертає. Також зверніть увагу на виклик EmitWriteLine(). Цей допоміжний член класу ILGenerator автоматично записує рядок у стандартний вихід із мінімальною турботою.

Аналогічно можна додадти властивість 

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{
   ...

    // Now create the GetMsg() method.
    MethodBuilder getMsgMethod = typeHelloWordBuilder.DefineMethod(
      "GetMsg",
      MethodAttributes.Public,
      typeof(string),
      null);
    ILGenerator methodGetMsgIl = getMsgMethod.GetILGenerator();
    methodGetMsgIl.Emit(OpCodes.Ldarg_0);
    methodGetMsgIl.Emit(OpCodes.Ldfld, fieldMessageBuilder);
    methodGetMsgIl.Emit(OpCodes.Ret);
    
    return builder;

}
```

### Створення типу

Останім кроком в побудові методу створимо тип і повернемо збірку.

```cs
static AssemblyBuilder CreateHelloWorldAssemblyBuilder()
{

...
    // Create
    typeHelloWordBuilder.CreateType();

    return builder;
}

```

### Використаня методу шо створює збірку.

Маючи метод шо генерує збірку використаємо її для створеня об'єкту.

```cs
void UsingCreateHelloWorldAssemblyBuilder()
{
    AssemblyBuilder assemblyBuilder = CreateHelloWorldAssemblyBuilder();
    Console.WriteLine($"\nI have {assemblyBuilder.GetType()} {assemblyBuilder.GetName()}\n");

    Type typeHelloWorld = assemblyBuilder.GetType("HelloWorldAssembly.HelloWorld")!;
    Console.WriteLine($"\nI have {typeHelloWorld.GetType()} {typeHelloWorld}\n");

    string msg = "Hi girl!";
    object[] constrArgs = [msg];

    object helloWorldObject = Activator.CreateInstance(typeHelloWorld, constrArgs )!;
    Console.WriteLine($"\nI have {helloWorldObject.GetType()} \n");

    MethodInfo methodInfoSayHello = typeHelloWorld.GetMethod("SayHello")!;
    methodInfoSayHello.Invoke(helloWorldObject, null);

    MethodInfo methodInfoPropertyGetMsg = typeHelloWorld.GetMethod("GetMsg")!;
    Console.WriteLine(methodInfoPropertyGetMsg.Invoke(helloWorldObject,null));
}
UsingCreateHelloWorldAssemblyBuilder();

```
```
I have System.Reflection.Emit.RuntimeAssemblyBuilder HelloWorldAssembly, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null


I have System.RuntimeType HelloWorldAssembly.HelloWorld


I have HelloWorldAssembly.HelloWorld

Hello from the HelloWorld class!
Hi girl!
```
Для початку виколикається метод який повертає посиланя на AssemblyBuilder.
Далі використається пізнє зв’язування, щоб створити екземпляр класу HelloWorld і взаємодіяти з його членами.

Для створення більш просунутих збірок потрібні знання CIL.
