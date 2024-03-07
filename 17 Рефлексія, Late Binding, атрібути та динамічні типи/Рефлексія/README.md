# Рефлексія.

## Необхідність метаданих типу

Можливість повного опису типів (класів, інтерфейсів, структур, перерахувань і делегатів) за допомогою метаданих є ключовим елементом платформи .NET. Багато технологій .NET, наприклад серіалізація об’єктів, вимагають можливості виявити формат типів під час виконання. Численні служби компілятора та можливості IDE IntelliSense — усе це залежить від конкретного опису типу.
Використоавуючи утіліту ildasm.exe ми можемо переглядати метадані типу збірки. У згенерованому файлі CarLibrary.il (попередьня глава) перейдіть до розділу METAINFO, щоб переглянути всі метадані CarLibrary. Частина файлу CarLibrary.il:
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

Тут TypDefName використовується для встановлення назви заданого типу, який у цьому випадку є спеціальним переліком CarLibrary.EngineStateEnum. Маркер метаданих Extends використовується для документування базового типу заданого типу .NET (у цьому випадку це тип, на який посилається, System.Enum).Кожне поле переліку позначається за допомогою маркера Field #n.

### Класс в метаданих.

Ось частковий дамп класу Car, який ілюструє наступне:
  - Як поля визначаються з точки зору метаданих .NET
  - Як методи документуються через метадані .NET
  - Як автоматична властивість представлена в метаданих .NET

```console
TypeDef #1
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
//    MethodName: get_Name
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
//     MethodName: set_Name
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
//     Prop.Name : Name
//     Flags     : [none]
//     CallCnvntn: [PROPERTY]
//     hasThis
//     ReturnType: String
//     No arguments.
//     DefltValue:
//     Setter    : set_Name
//     Getter    : get_Name
//     0 Others
```
По-перше, зауважте, що метадані класу Car позначають базовий клас типу (System.Object) і включають різні прапорці, які описують, як був створений цей тип (наприклад, [Public], [Abstract] тощо).
Методи (наприклад, конструктори) описуються своїми параметрами, значенням, що повертається, і назвою. 
Зверніть увагу, як автоматична властивість призводить до створеного компілятором приватного резервного поля (яке було названо <Name>k__BackingField) і двох згенерованих компілятором методів (у випадку властивості читання-запису), названих у цьому прикладі get_Name() і set_Name().
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

У .NET рефлексія — це процес виявлення типу під час виконання. Використовуючи служби відображення, ви можете програмно отримати ту саму інформацію метаданих, згенеровану ildasm.exe, використовуючи дружню об’єктну модель. Наприклад, за допомогою рефлексії можна отримати список усіх типів, що містяться в даній збірці *.dll або *.exe, включаючи методи, поля, властивості та події, визначені даним типом. Ви також можете динамічно виявляти набір інтерфейсів, які підтримують певний тип, параметри методу та інші пов’язані деталі (базові класи, інформацію про простір імен, дані маніфесту тощо).
Як і будь-який простір імен, System.Reflection (який визначено в System.Runtime.dll) містить кілька пов’язаних типів.

Вибірка членів простору імен System.Reflection

    Assembly : Цей абстрактний клас містить члени, які дозволяють завантажувати, досліджувати та керувати збіркою.

    AssemblyName : Цей клас дозволяє виявити численні деталі, що стоять за ідентифікатором збірки (інформацію про версію, інформацію про культуру тощо).

    EventInfo : Цей абстрактний клас містить інформацію про певну подію.

    FieldInfo : Цей абстрактний клас містить інформацію для даного поля.

    MemberInfo : Це абстрактний базовий клас, який визначає загальну поведінку для типів EventInfo, FieldInfo, MethodInfo та PropertyInfo.

    MethodInfo : Цей абстрактний клас містить інформацію для заданого методу.

    Module : Цей абстрактний клас дозволяє отримати доступ до певного модуля в складі з кількома файлами.

    ParameterInfo : Цей клас містить інформацію для заданого параметра.

    PropertyInfo : Цей абстрактний клас містить інформацію про задану властивість.

Щоб зрозуміти, як використовувати простір імен System.Reflection для програмного читання метаданих .NET, вам потрібно спочатку ознайомитися з класом System.Type.

## Клас System.Type.

