# Рефлексія.

## Необхідність метаданих типу

Можливість повного опису типів (класів, інтерфейсів, структур, перерахувань і делегатів) за допомогою метаданих є ключовим елементом платформи .NET. Багато технологій .NET, наприклад серіалізація об’єктів, вимагають можливості виявити формат типів під час виконання. Численні служби компілятора та можливості IDE IntelliSense — усе це залежить від конкретного опису типу.
Використоавуючи утіліту ildasm.exe ми можемо переглядати метадані типів в збірці.

```
ildasm /METADATA /out=CarLibrary.il .\CarLibrary\bin\Debug\netX.0\CarLibrary.dll
```

У згенерованому файлі CarLibrary.il (попередня глава) перейдіть до розділу METAINFO, щоб переглянути всі метадані CarLibrary. Частина файлу CarLibrary.il:
```
// ================================= M E T A I N F O ================================================

// ===========================================================
// ScopeName : CarLibrary.dll
// MVID      : {4E09DF2B-A342-4BC5-9FA1-7128CBDAD3ED}
// 	CustomAttribute #1 (0c000004)
// 	-------------------------------------------------------
// 		CustomAttribute Type: 0a00000b
// 		CustomAttributeName: System.Runtime.CompilerServices.RefSafetyRulesAttribute :: instance void .ctor(int32)
// 		Length: 8
// 		Value : 01 00 0b 00 00 00 00 00                          >                <
// 		ctor args: (11)
// 
// ===========================================================
// Global functions
// -------------------------------------------------------
// 
// Global fields
// -------------------------------------------------------
// 
// Global MemberRefs
// -------------------------------------------------------
// 
// TypeDef #1 (02000002)
// -------------------------------------------------------
// 	TypDefName: CarLibrary.Car  (02000002)
// 	Flags     : [Public] [AutoLayout] [Class] [Abstract] [AnsiClass] [BeforeFieldInit]  (00100081)
// 	Extends   : 0100000F [TypeRef] System.Object
// 	Field #1 (04000001)
// 	-------------------------------------------------------
// 		Field Name: <Name>k__BackingField (04000001)
// 		Flags     : [Private]  (00000001)
// 		CallCnvntn: [FIELD]
// 		Field type:  String
// 		CustomAttribute #1 (0c000002)
// 		-------------------------------------------------------
// 			CustomAttribute Type: 0a00000e
// 			CustomAttributeName: System.Runtime.CompilerServices.CompilerGeneratedAttribute :: instance void .ctor()
// 			Length: 4
// 			Value : 01 00 00 00                                      >                <
// 			ctor args: ()
// 
// 		CustomAttribute #2 (0c000003)
// 		-------------------------------------------------------
// 			CustomAttribute Type: 0a00000f
// 			CustomAttributeName: System.Diagnostics.DebuggerBrowsableAttribute :: instance void .ctor(value class System.Diagnostics.DebuggerBrowsableState)
// 			Length: 8
// 			Value : 01 00 00 00 00 00 00 00                          >                <
// 			ctor args: ( <can not decode> )
// ...
// ...

```
Як бачите, метадані типу .NET є багатослівними (справжній двійковий формат набагато компактніший). Давайте просто зазирнемо до деяких ключових описів метаданих збірки CarLibrary.dll.

Кожен тип, визначений у поточній збірці, документується за допомогою маркера TypeDef #n (де TypeDef є скороченням від type definition). Якщо описуваний тип використовує тип, визначений в окремій збірці .NET, тип, на який посилається, документується за допомогою маркера TypeRef #n (де TypeRef є скороченням від type reference).Маркер TypeRef — це вказівник (якщо хочете) на повне визначення метаданих типу, на який посилається, у зовнішній збірці. Тикам чином, метадані .NET — це набір таблиць, які чітко позначають усі визначення типів (TypeDefs) і типи, на які посилаються (TypeRefs), усі з яких можна перевірити за допомогою ildasm.exe.

### Перерахування в метаданих.

Якшо розглянути CarLibrary.dll, один TypeDef є описом метаданих перерахування CarLibrary.EngineStateEnum

```console
TypeDef #2 (02000003)
// -------------------------------------------------------
// 	TypDefName: CarLibrary.EngineStateEnum  (02000003)
// 	Flags     : [Public] [AutoLayout] [Class] [Sealed] [AnsiClass]  (00000101)
// 	Extends   : 01000013 [TypeRef] System.Enum
// 	Field #1 (04000005)
// 	-------------------------------------------------------
// 		Field Name: value__ (04000005)
// 		Flags     : [Public] [SpecialName] [RTSpecialName]  (00000606)
// 		CallCnvntn: [FIELD]
// 		Field type:  I4
// 
// 	Field #2 (04000006)
// 	-------------------------------------------------------
// 		Field Name: EngineAlive (04000006)
// 		Flags     : [Public] [Static] [Literal] [HasDefault]  (00008056)
// 	DefltValue: (I4) 0
// 		CallCnvntn: [FIELD]
// 		Field type:  ValueClass CarLibrary.EngineStateEnum
// 
// 	Field #3 (04000007)
// 	-------------------------------------------------------
// 		Field Name: EngineDead (04000007)
// 		Flags     : [Public] [Static] [Literal] [HasDefault]  (00008056)
// 	DefltValue: (I4) 1
// 		CallCnvntn: [FIELD]
// 		Field type:  ValueClass CarLibrary.EngineStateEnum
```

Тут TypDefName використовується для встановлення назви заданого типу, який у цьому випадку є спеціальним переліком CarLibrary.EngineStateEnum. Маркер метаданих Extends використовується для документування базового типу заданого типу .NET (у цьому випадку це тип, на який посилається, System.Enum). Кожне поле переліку позначається за допомогою маркера Field #n.

### Класс в метаданих.

Ось частковий дамп класу Car, який ілюструє наступне:

  - Як поля визначаються з точки зору метаданих .NET
  - Як методи документуються через метадані .NET
  - Як автоматична властивість представлена в метаданих .NET

```console
// TypeDef #1
// -------------------------------------------------------
//   TypDefName: CarLibrary.Car
//   Flags     : [Public] [AutoLayout] [Class] [Abstract] [AnsiClass] [BeforeFieldInit]
//   Extends   : [TypeRef] System.Object
//   Field #1
//   -------------------------------------------------------
//     Field Name: <PetName>k__BackingField
//     Flags     : [Private]
//     CallCnvntn: [FIELD]
//     Field type:  String
...
//  Method #1
// -------------------------------------------------------
//    MethodName: get_PetName
//    Flags      : [Public] [HideBySig] [ReuseSlot] [SpecialName]
//    RVA        : 0x00002050
//    ImplFlags  : [IL] [Managed]
//    CallCnvntn: [DEFAULT]
//    hasThis
//    ReturnType: String
//    No arguments.
...
//   Method #2
//   -------------------------------------------------------
//     MethodName: set_PetName
//     Flags     : [Public] [HideBySig] [ReuseSlot] [SpecialName]
//     RVA       : 0x00002058
//     ImplFlags : [IL] [Managed]
//     CallCnvntn: [DEFAULT]
//     hasThis
//     ReturnType: Void
//     1 Arguments
//       Argument #1:  String
//     1 Parameters
//       (1) ParamToken : Name : value flags: [none]
...
//   Property #1
//   -------------------------------------------------------
//     Prop.Name : PetName
//     Flags     : [none]
//     CallCnvntn: [PROPERTY]
//     hasThis
//     ReturnType: String
//     No arguments.
//     DefltValue:
//     Setter    : set_PetName
//     Getter    : get_PetName
//     0 Others
...
```
По-перше, зауважте, що метадані класу Car позначають базовий клас типу (System.Object) і включають різні прапорці, які описують, як був створений цей тип (наприклад, [Public], [Abstract] тощо).
Методи (наприклад, конструктори) описуються своїми параметрами, значенням, що повертається, і назвою. 
Зверніть увагу, як автоматична властивість призводить до створеного компілятором приватного резервного поля (яке було названо <Name>k__BackingField) і двох згенерованих компілятором методів (у випадку властивості читання-запису), названих у цьому прикладі get_PetName() і set_PetName().
Нарешті, фактична властивість відображається на внутрішніх методах get/set за допомогою токенів Getter/Setter метаданих .NET.

### TypeRef

Пам’ятайте, що метадані збірки описуватимуть не лише набір внутрішніх типів (Car, EngineStateEnum тощо), а й будь-які зовнішні типи, на які посилаються внутрішні типи. Наприклад, враховуючи, що CarLibrary.dll визначив два перерахування, ви знайдете блок TypeRef для типу System.Enum таким чином:

```
// TypeRef #19 (01000013)
// -------------------------------------------------------
// Token:             0x01000013
// ResolutionScope:   0x23000001
// TypeRefName:       System.Enum
```

### Визначення збірки

Файл CarLibrary.il також дозволяє переглядати метадані .NET, які описують саму збірку за допомогою маркера Assembly.

```
// Assembly
// -------------------------------------------------------
// 	Token: 0x20000001
// 	Name : CarLibrary
// 	Public Key    :
// 	Hash Algorithm : 0x00008004
// 	Version: 1.0.0.0
// 	Major Version: 0x00000001
// 	Minor Version: 0x00000000
// 	Build Number: 0x00000000
// 	Revision Number: 0x00000000
// 	Locale: <null>
// 	Flags : [none] (00000000)
// 	CustomAttribute #1 (0c000005)
```
### Посилання на інші збірки.

Окрім маркера Assembly та набору блоків TypeDef і TypeRef, метадані .NET також використовують маркери AssemblyRef #n для документування кожної зовнішньої збірки. Враховуючи, що кожна збірка .NET посилається на збірку бібліотеки базового класу System.Runtime, ви знайдете AssemblyRef для збірки System.Runtime.

```
// AssemblyRef #1 (23000001)
// -------------------------------------------------------
// 	Token: 0x23000001
// 	Public Key or Token: b0 3f 5f 7f 11 d5 0a 3a 
// 	Name: System.Runtime
// 	Version: 8.0.0.0
// 	Major Version: 0x00000008
// 	Minor Version: 0x00000000
// 	Build Number: 0x00000000
// 	Revision Number: 0x00000000
// 	Locale: <null>
// 	HashValue Blob:
// 	Flags: [none] (00000000)
```
### Рядкі літералів.

Останнім цікавим моментом щодо метаданих .NET є той факт, що кожен рядковий літерал у вашій кодовій базі задокументовано під маркером User Strings.

```
// User Strings
// -------------------------------------------------------
// 70000001 : (32) L"Eek! Your engine block exploded!"
// 70000043 : (34) L"Ramming speed! Faster is better..."
```

Як показано в попередньому списку метаданих, завжди майте на увазі, що всі рядки чітко задокументовані в метаданих збірки. Це може мати величезні наслідки для безпеки, якщо ви використовуєте рядкові літерали для представлення паролів, номерів кредитних карток або іншої конфіденційної інформації.
Наступним питанням, яке виникне в голові, може бути «Як я можу використати цю інформацію у своїх програмах?»

# Рефлексія.

У .NET рефлексія — це процес виявлення типу під час виконання. Використовуючи рефлексію, ви можете програмно отримати ту саму інформацію метаданих, згенеровану ildasm.exe, використовуючи об’єктну модель. Наприклад, за допомогою рефлексії можна отримати список усіх типів, що містяться в даній збірці *.dll або *.exe, включаючи методи, поля, властивості та події, визначені даним типом. Ви також можете динамічно виявляти набір інтерфейсів, які підтримують певний тип, параметри методу та інші пов’язані деталі (базові класи, інформацію про простір імен, дані маніфесту тощо).
Як і будь-який простір імен, System.Reflection (який визначено в System.Runtime.dll) містить кілька пов’язаних типів.

Вибірка членів простору імен System.Reflection

|Тип|Сенс використання|
|---|-----------------|
|Assembly|Цей абстрактний клас містить члени, які дозволяють завантажувати, досліджувати та керувати збіркою.|
|AssemblyName|Цей клас дозволяє виявити численні деталі, що стоять за ідентифікатором збірки (інформацію про версію, інформацію про культуру тощо).|
|MemberInfo|Це абстрактний базовий клас, який визначає загальну поведінку для типів EventInfo, FieldInfo, MethodInfo та PropertyInfo.|
|FieldInfo|Цей абстрактний клас містить інформацію для даного поля.|
|PropertyInfo|Цей абстрактний клас містить інформацію про задану властивість.|
|MethodInfo|Цей абстрактний клас містить інформацію для заданого методу.|
|EventInfo|Цей абстрактний клас містить інформацію про певну подію.|
|Module|Цей абстрактний клас дозволяє отримати доступ до певного модуля в складі з кількома файлами.
|ParameterInfo|Цей клас містить інформацію для заданого параметра.|


Щоб зрозуміти, як використовувати простір імен System.Reflection для програмного читання метаданих .NET, вам потрібно спочатку ознайомитися з класом System.Type.

# Клас System.Type .

Клас System.Type визначає члени, які можна використовувати для виявлення метаданих типу, велика кількість яких повертає типи з простору імен System.Reflection. Наприклад, Type.GetMethods() повертає масив об’єктів MethodInfo, Type.GetFields() повертає масив об’єктів FieldInfo тощо.

Де які важливі члени, які підтримуються System.Type (додаткову інформацію див. у документації .NET).

Властивості:

    IsAbstract
    IsArray
    IsClass
    IsCOMObject
    IsEnum
    IsGenericTypeDefinition
    IsGenericParameter
    IsInterface
    IsPrimitive
    IsNestedPrivate
    IsNestedPublic
    IsSealed
    IsValueType

Ці властивості дозволяють вам виявити низку основних характеристик типу, на який ви посилаєтеся (наприклад, чи це абстрактна сутність, масив, вкладений клас тощо).

Методи:

    GetConstructors()
    GetEvents()
    GetFields()
    GetInterfaces()
    GetMembers()
    GetMethods()
    GetNestedTypes()
    GetProperties()

Ці методи дозволяють вам отримати масив, що представляє елементи (інтерфейс, метод, властивість тощо), які вас цікавлять. Кожен метод повертає пов’язаний масив (наприклад, GetFields() повертає масив FieldInfo, GetMethods() повертає масив MethodInfo тощо). Майте на увазі, що кожен із цих методів має окрему форму (наприклад, GetMethod(), GetProperty() тощо), яка дозволяє отримати певний елемент за назвою, а не масив усіх пов’язаних елементів.

    FindMembers()

Цей метод повертає масив MemberInfo на основі критеріїв пошуку.

    GetType()

Цей статичний метод повертає екземпляр типу за назвою рядка.

    InvokeMember()

Цей метод дозволяє «пізнє зв’язування» для певного елемента. Пізніше в цьому розділі ви дізнаєтесь про пізнє зв’язування.

## Отримання посилання на тип за допомогою System.Object.GetType()

Розглядяючи клас Type ви можете отримати екземпляр класу Type різними способами. Однак єдине, що ви не можете зробити, це безпосередньо створити об’єкт Type за допомогою ключового слова new, оскільки Type є абстрактним класом. System.Object визначає метод під назвою GetType(), який повертає екземпляр класу Type, який представляє метадані для поточного об’єкта.

Стовримо рішення з проектом ExamineTypeClass та скопіюємо бібліотеку в попередньої глави CarLibrary. Додамо посилання з проекту на бібіліотеку. 

```cs
using CarLibrary;
using ExamineTypeClass;
using System.Reflection;

void ObtainTypeUseObjectGetType()
{
    SportCar sportCar = new();
    Type type = sportCar.GetType();

    Console.WriteLine(type);
    Console.WriteLine(type.GetType());
   
}
ObtainTypeUseObjectGetType();
```
```console
CarLibrary.SportCar
System.RuntimeType
```
Очевидно, що цей підхід працюватиме, лише якщо у вас є знання під час компіляції про тип, який ви хочете обстежити (у цьому випадку SportsCar), і на даний момент є примірник типу в пам’яті.

## Отримання посилання на тип за допомогою typeof()

Наступним способом отримання інформації про тип є використання оператора C# typeof, наприклад

```cs
void ObtainTypeUseTypeOf()
{
    Type type = typeof(SportCar);

    Console.WriteLine(type);
    Console.WriteLine(type.GetType());
}
ObtainTypeUseTypeOf();
```
```
CarLibrary.SportCar
System.RuntimeType
```

На відміну від System.Object.GetType(), оператор typeof є корисним, оскільки вам не потрібно спочатку створювати екземпляр об’єкта, щоб отримати інформацію про тип. Однак ваша база коду все ще повинна мати знання під час компіляції про тип, який ви зацікавлені в дослідженні, оскільки typeof очікує строго типізованого імені типу.

## Отримання посилання на тип за допомогою System.Type.GetType()

Щоб отримати інформацію про тип більш гнучким способом, ви можете викликати статичний член GetType() класу System.Type і вказати повне ім’я рядка типу, який вас цікавить. Використовуючи цей підхід, вам не потрібно знати під час компіляції тип, з якого ви витягуєте метадані, враховуючи, що Type.GetType() приймає екземпляр завжди існуючого System.String.

Нехай в проекті ExamineTypeClass є клас.

```cs
    internal class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        private double _selary;

        public void ChangeSelary(double selary)
        {
            _selary = selary;
        }
    }
```
Використаємо метод Type.GetType() для отриманя внутрішніх і зовнишніх типів.

```cs
void ObtainTypeUseTypeGetType()
{

    Type? type = Type.GetType("ExamineTypeClass.Person", true,true);

    Console.WriteLine(type);
    Console.WriteLine(type?.GetType());

    type = Type.GetType("CarLibrary.SportCar, Carlibrary", true, true);

    Console.WriteLine(type);
    Console.WriteLine(type?.GetType());

}
ObtainTypeUseTypeGetType();
```
```
ExamineTypeClass.Person
System.RuntimeType
CarLibrary.SportCar
System.RuntimeType
```
Метод Type.GetType() було перевантажено, щоб дозволити вам вказати два параметри, один з яких контролює, чи має створюватися виняток, якщо тип неможливо знайти, а інший встановлює чутливість до регістру рядка. Крім того треба вказувати ім'я зовнішбої збірки типу де він находиться.

# Перегляд метаданних програмно.

Існує можливість переглядати і перевіряти метаданні в процесі виконання. Тобто відображати деталі методів, властивостей, полів і підтримуваних інтерфейсів для будь-якого типу в System.Runtime.dll (всі програми .NET мають автоматичний доступ до цієї бібліотеки класів фреймворку) або власно стоврені типи, які є нашадками.

## Метод GetMembers.

Коли ми вже маємо отриманий тип ми можеме переглянути його члени.
```cs
void UseGetMemebers()
{
    Type type = typeof(Person);

    var members = type.GetMembers();

    foreach (var member in members)
    {
        Console.WriteLine($"{member.DeclaringType}   {member.MemberType}   {member.Name}");
    }
}
UseGetMemebers();

```
```
ExamineTypeClass.Person   Method   get_PersonId
ExamineTypeClass.Person   Method   set_PersonId
ExamineTypeClass.Person   Method   get_FirstName
ExamineTypeClass.Person   Method   set_FirstName
ExamineTypeClass.Person   Method   get_LastName
ExamineTypeClass.Person   Method   set_LastName
ExamineTypeClass.Person   Method   ChangeSelary
System.Object   Method   GetType
System.Object   Method   ToString
System.Object   Method   Equals
System.Object   Method   GetHashCode
ExamineTypeClass.Person   Constructor   .ctor
ExamineTypeClass.Person   Property   PersonId
ExamineTypeClass.Person   Property   FirstName
ExamineTypeClass.Person   Property   LastName
```
Тут ми отримуєм тільки всі загальнодоступні члени. Для властивостей виводиться методи get і set. Також виводяться успадковані можливості Object.

Також є перезагружений варіант методу.

```cs
void UseGetMemebersWithBindingFlags()
{
    Type type = typeof(Person);

    var members = type.GetMembers(
        BindingFlags.DeclaredOnly | 
        BindingFlags.Instance | 
        BindingFlags.NonPublic | 
        BindingFlags.Public);

    foreach (var member in members)
    {
        Console.WriteLine($"{member.DeclaringType}   {member.MemberType}   {member.Name}");
    }
}
UseGetMemebersWithBindingFlags();
```
```
ExamineTypeClass.Person   Method   get_LastName
ExamineTypeClass.Person   Method   set_LastName
ExamineTypeClass.Person   Method   ChangeSelary
System.Object   Method   GetType
System.Object   Method   ToString
System.Object   Method   Equals
System.Object   Method   GetHashCode
ExamineTypeClass.Person   Constructor   .ctor
ExamineTypeClass.Person   Property   PersonId
ExamineTypeClass.Person   Property   FirstName
ExamineTypeClass.Person   Property   LastName
ExamineTypeClass.Person   Method   get_PersonId
ExamineTypeClass.Person   Method   set_PersonId
ExamineTypeClass.Person   Method   get_FirstName
ExamineTypeClass.Person   Method   set_FirstName
ExamineTypeClass.Person   Method   get_LastName
ExamineTypeClass.Person   Method   set_LastName
ExamineTypeClass.Person   Method   ChangeSelary
ExamineTypeClass.Person   Constructor   .ctor
ExamineTypeClass.Person   Property   PersonId
ExamineTypeClass.Person   Property   FirstName
ExamineTypeClass.Person   Property   LastName
ExamineTypeClass.Person   Field   <PersonId>k__BackingField
ExamineTypeClass.Person   Field   <FirstName>k__BackingField
ExamineTypeClass.Person   Field   <LastName>k__BackingField
ExamineTypeClass.Person   Field   _selary
```
Також можна отримати члени по назві.

```cs
void GetOneMember()
{
    Type type = typeof(Person);

    var changeSaleries = type.GetMember("ChangeSelary");
    foreach (var member in changeSaleries)
    {
        Console.WriteLine($"{member.DeclaringType}   {member.MemberType}   {member.Name}");
    }
}
GetOneMember();
```
```
ExamineTypeClass.Person   Method   ChangeSelary
```

Оскільки може бути перезавантаження методу повертається масив.

## Дослідження типу.

Створимо можливісість ввеcти тип для обстеження.

SimpleTypeViewer\Program.cs

```cs
static void StartViewer()
{
	string enteredType = string.Empty;
	do
	{	//Get name of type.
        Console.Clear();
        Console.Write("Enter Type or q:");
		enteredType = Console.ReadLine()!;
		if (enteredType.Equals("Q",StringComparison.OrdinalIgnoreCase))
		{
			break;
		}
		Console.Clear();	
		InvestigateTheType(enteredType);
        Console.ReadKey();
    } while (true);
}
StartViewer();

static void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");

}
```
```
Enter Type or q:System.Int32

We want to investigate the type:System.Int32

Enter Type or q:bad

Sorry, can't find type!

```
## Рефлексія даних про тип.

Дані про тип можна отриматити за допоимогою властивостей екземпляра класу Type.

```cs
void AboutType(Type type)
{
    Console.WriteLine();
    Console.WriteLine($"Is type class:{type.IsClass}");
    Console.WriteLine($"Is type abstract:{type.IsAbstract}");
    Console.WriteLine($"Is type generic:{type.IsGenericType}");
    Console.WriteLine($"Is type sealed:{type.IsSealed}");
    Console.WriteLine($"Base type:{type.BaseType}");
}
```
Зміненмо метод InvestigateTheType який викликатиме новий метод та протестуємо.

```cs
void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");
    
    AboutType(type);
}
```
```
We want to investigate the type:System.Array

Is type class:True
Is type abstract:True
Is type generic:False
Is type sealed:False
Base type:System.Object
```
## Рефлексія полів та властивостей.

Поля та властивості можна отримати за допомогою відповідних методів типу Type.

```cs
void ListFilds(Type type)
{
    Console.WriteLine("\nFilds");

    var filds = from f in type.GetFields()
                orderby f.Name
                select f;
    foreach (var f in filds)
    {
        Console.WriteLine("\t"+f.Name);
    }    
}

void ListProperties(Type type)
{
    Console.WriteLine("\nProperties");

    var properties = from p in type.GetProperties()
                     orderby p.Name
                     select p;
    foreach (var p in properties)
    {
        Console.WriteLine("\t"+p.Name);
    }
}

```
Методи Type.GetFields Type.GetProperties повертають массиви об'ектів FieldInfo, PropertyInfo. LINQ to Objects дозволяє створювати строго типізовані запити, які можна застосовувати до колекцій об’єктів у пам’яті. Гарна практика, щоразу, коли ви знаходите блоки циклу або логіку програмування рішень, ви можете використовувати відповідний запит LINQ.

Протестуємо.
```cs
void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");
    
    //AboutType(type);
    ListFilds(type);
    ListProperties(type);
}
```
```
We want to investigate the type:System.String

Filds
        Empty

Properties
        Chars
        Length
```
## Рефлексія методів.

За допомогою методу Type.GetMethods можна отримати массив об'єктів MethodInfo.

```cs

void ListMethods(Type type)
{
    Console.WriteLine("Methods");

    var methods =from t in type.GetMethods()
                 orderby t.Name
                 select t;
    
    foreach (MethodInfo method in methods)
    {
        Console.WriteLine($"\t{method.Name}");
    }
}
```
MethodInfo має багато додаткових членів, які дозволяють визначити, чи є метод статичним, віртуальним, загальним або абстрактним, але тут ми тільки використовуємо Name. 

Протестуємо.
```cs
void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");
    
    //AboutType(type);
    //ListFilds(type);
    //ListProperties(type);
    ListMethods(type);
}
```
```
We want to investigate the type:System.Int32
Methods

We want to investigate the type:System.Int32
Methods
        Abs
        Clamp
        CompareTo
        CompareTo
        CopySign
        CreateChecked
        CreateSaturating
        CreateTruncating
        DivRem
        Equals
        Equals
        GetHashCode
        ...
```

## Рефлексія реалізованих інтерфейсів.

Метод Type.GetInterfaces повертає Type[]

```cs
void ListInterfaces(Type type)
{
    Console.WriteLine("Interfaces");

    var interfaces = from i in type.GetInterfaces()
                     orderby i.Name
                     select i;

    foreach (var item in interfaces)
    {
        Console.WriteLine("\t"+item.Name);
    }
}
```
Протестуємо.
```cs
void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");

    //AboutType(type);
    //ListFilds(type);
    //ListProperties(type);
    //ListMethods(type);
    ListInterfaces(type);
}
```
```
We want to investigate the type:System.String
Interfaces
        ICloneable
        IComparable
        IComparable`1
        IConvertible
        IEnumerable
        IEnumerable`1
        IEquatable`1
        IParsable`1
        ISpanParsable`1

```

Більшість методів «get» System.Type (GetMethods(), GetInterfaces() тощо) були перевантажені, щоб ви могли вказувати значення з переліку BindingFlags. Це забезпечує більший рівень контролю над тим, що саме слід шукати (наприклад, лише статичні учасники, лише публічні учасники, включати приватні учасники тощо).

## Рефлексія всього разом. 

Знімемо коментарі з усіх методів шо працюівли окремо і протестуємо роботу.

```cs
void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");

    AboutType(type);
    ListFilds(type);
    ListProperties(type);
    ListMethods(type);
    ListInterfaces(type);
}
```
Для тестування можна ввести різні типи System.Int32, System.Array, System.Math, System.Object тощо.

## Рефлексія статичних методів.

Якщо при тестувані ввести System.Console, тип не розпізнається. Статичні типи не можна завантажити за допомогою методу Type.GetType(typeName). Замість цього ви повинні використовувати інший механізм, функцію typeof із System.Type.  

```cs
void HowGetStaticType()
{
    Type type = typeof(Console);
    Console.WriteLine($"We want to investigate the type:{type}");
    ReflectionOfType(type);
}
HowGetStaticType();

void ReflectionOfType(Type type)
{
    AboutType(type);
    ListFilds(type);
    ListProperties(type);
    ListMethods(type);
    ListInterfaces(type);
}

```
```
We want to investigate the type:System.Console

Is type class:True
Is type abstract:True
Is type generic:False
Is type sealed:True
Base type:System.Object

Filds

Properties
        BackgroundColor
        BufferHeight
        BufferWidth
        CapsLock
        CursorLeft
        CursorSize
        CursorTop
        CursorVisible
        Error
        ForegroundColor
        In
        InputEncoding
        IsErrorRedirected
        IsInputRedirected
        IsOutputRedirected
        KeyAvailable
        LargestWindowHeight
        LargestWindowWidth
        NumberLock
        Out
        OutputEncoding
        Title
        TreatControlCAsInput
        WindowHeight
        WindowLeft
        WindowTop
        WindowWidth
Methods
Interfaces
```

## Рефлексія узагальнених типів.

Коли ви викликаєте Type.GetType() для отримання опису метаданих загальних типів, ви повинні використовувати спеціальний синтаксис із символом «зворотної галочки» (`), за яким слідує числове значення, яке представляє кількість параметрів типу, які підтримує тип. Наприклад, якщо ви хочете роздрукувати опис метаданих System.Collections.Generic.List\<T\>, вам потрібно передати такий рядок:
 
    System.Collections.Generic.List`1 

Тут ви використовуєте числове значення 1, враховуючи, що List\<T\> має лише один параметр типу. Однак, якщо ви хочете відобразити Dictionary\<TKey, TValue\>, укажіть значення 2, наприклад:

    System.Collections.Generic.Dictionary`2


## Рефлексія значень шо повертає метод та його параметрів.

Крім назви методів класу ми можемо отримати їх додадкові дані. Зробемо невеликі зміни методу ListMethods.

```cs
void ListMethods(Type type)
{
    Console.WriteLine("Methods");

    var methods =from t in type.GetMethods()
                 orderby t.Name
                 select t;
    
    foreach (MethodInfo methodInfo in methods)
    {
        AboutMethod(methodInfo);
    }

}

void AboutMethod(MethodInfo methodInfo)
{
    string? nameOfTheReturnType = methodInfo.ReturnType.FullName;
    string nameOfTheParameters = "(";
    foreach (ParameterInfo paramInfo in methodInfo.GetParameters())
    {
        nameOfTheParameters += $"{paramInfo.ParameterType} {paramInfo.Name}";
    }
    nameOfTheParameters += ")";

    Console.WriteLine($"{nameOfTheReturnType} {methodInfo.Name} {nameOfTheParameters}");
}
```
Протестуємо.
```cs
void InvestigateTheType(string typeName)
{
    Type? type = Type.GetType(typeName);
    if (type == null)
    {
        Console.WriteLine("Sorry, can't find type!");
        return;
    }
    Console.WriteLine($"We want to investigate the type:{type.FullName}");

    //AboutType(type);
    //ListFilds(type);
    //ListProperties(type);
    ListMethods(type);
    //ListInterfaces(type);
}
```
```
We want to investigate the type:System.Math
Methods
System.Int16 Abs (System.Int16 value)
System.Int32 Abs (System.Int32 value)
System.Int64 Abs (System.Int64 value)
System.IntPtr Abs (System.IntPtr value)
System.SByte Abs (System.SByte value)
System.Decimal Abs (System.Decimal value)
System.Double Abs (System.Double value)
System.Single Abs (System.Single value)
System.Double Acos (System.Double d)
System.Double Acosh (System.Double d)
System.Double Asin (System.Double d)
```

Тип MethodInfo надає властивість ReturnType і метод GetParameters() для того аби отримати тип повернення та типи вхідних параметрів. 
Поточна реалізація ListMethods() корисна тим, що ви можете безпосередньо досліджувати кожен параметр і тип повернення методу за допомогою об’єктної моделі System.Reflection.

Всі типи XXXInfo (MethodInfo, PropertyInfo, EventInfo тощо) перевизначили ToString() для відображення сігнатури запитуваного елемента. Тож нам може бути достатьньо не створювати метод AboutMethod а зробити таку реалізацію.

```cs

void ListMethods(Type type)
{
    Console.WriteLine("Methods");

    var methods =from t in type.GetMethods()
                 orderby t.Name
                 select t;
    
    foreach (MethodInfo methodInfo in methods)
    {
        Console.WriteLine("\t"+methodInfo);
    }
}
```
```
We want to investigate the type:System.Math
Methods
        Int16 Abs(Int16)
        Int32 Abs(Int32)
        Int64 Abs(Int64)
        IntPtr Abs(IntPtr)
        SByte Abs(SByte)
        System.Decimal Abs(System.Decimal)
        Double Abs(Double)
        Single Abs(Single)
        Double Acos(Double)
        Double Acosh(Double)
        Double Asin(Double)
        Double Asinh(Double)
        Double Atan(Double)
        Double Atan2(Double, Double)
        Double Atanh(Double)
        Int64 BigMul(Int32, Int32)
        UInt64 BigMul(UInt64, UInt64, UInt64 ByRef)
        Int64 BigMul(Int64, Int64, Int64 ByRef)
```
Зрозуміло, що простір імен System.Reflection і клас System.Type дозволяють відображати багато інших аспектів типу, окрім того, що зараз відображає SimpleTypeViewer. Ви можете отримати події типу, отримати список будь-яких загальних параметрів для певного члена та зібрати десятки інших деталей. Тим не менш, на цьому етапі ви створили браузер об’єктів.

Основне обмеження цього конкретного прикладу полягає в тому, що у вас немає можливості відобразити поза поточною збіркою або збірками в бібліотеках базових класів, на які завжди є посилання. У зв’язку з цим виникає запитання: «Як я можу створювати програми, які можуть завантажувати (і відображати) збірки, на які немає посилань під час компіляції?»
