# Анонімні типи

Ви знаєте переваги визначення класів для представлення стану та функціональності певного елемента, який ви намагаєтеся змоделювати. Звісно, щоразу, коли вам потрібно визначити клас, який призначено для повторного використання в проектах і який забезпечує численні частини функціональності через набір методів, подій, властивостей і спеціальних конструкторів, створення нового класу C# є звичайною практикою. 
Однак, бувають і інші випадки, коли вам потрібно визначити клас просто для моделювання набору інкапсульованих (і певним чином пов'язаних) точок даних без будь-яких пов'язаних методів, подій чи іншої спеціалізованої функціональності. Крім того, що робити, якщо цей тип використовуватиметься лише кількома методами у вашій програмі? Було б досить незручно визначати повне визначення класу, коли ви чудово знаєте, що цей клас буде використовуватися лише в кількох місцях. Щоб підкреслити цей момент, ось приблизний план того, що вам може знадобитися зробити, коли вам потрібно створити «простий» тип даних, який дотримується типової семантики на основі значень:

```cs
class SomeClass
{
  // Define a set of private member variables...
  // Make a property for each member variable...
  // Override ToString() to account for key member variables...
  // Override GetHashCode() and Equals() to work with value-based equality...
}
```
Як бачите, це не обов'язково так просто. Вам не лише потрібно написати чимало коду, але й потрібно підтримувати ще один клас у вашій системі. Для таких тимчасових даних було б корисно швидко створювати власний тип даних. Наприклад, припустимо, вам потрібно створити власний метод, який отримує набір вхідних параметрів. Ви хочете взяти ці параметри та використати їх для створення нового типу даних для використання в області видимості цього методу. Крім того, ви хочете швидко вивести ці дані за допомогою типового методу ToString() та, можливо, використати інші члени System.Object. Ви можете зробити це саме за допомогою синтаксису анонімних типів.

## Визначення анонімного типу

Коли ви визначаєте анонімний тип, ви робите це за допомогою ключового слова var разом із синтаксисом ініціалізації об'єкта. Ви повинні використовувати ключове слово var, оскільки компілятор автоматично генеруватиме нове визначення класу під час компіляції (і ви ніколи не побачите назви цього класу у своєму коді C#). Синтаксис ініціалізації використовується для того, щоб вказати компілятору створити приватні поля та властивості (лише для читання) для щойно створеного типу. 
Для ілюстрації створіть новий проект консольної програми з назвою AnonymousTypes. Тепер додайте наступний метод до вашого файлу Program.cs, який миттєво створює новий тип, використовуючи вхідні дані параметрів:

```cs

static void BuildAnonymousType(string make, string color, int speed)
{
    // Build anonymous type using incoming args.
    var car = new { Make = make, Color = color, Speed = speed };

    // Note you can now use this type to get the property data!
    Console.WriteLine($"You have a {car.Color} {car.Make} going {car.Speed} MPH");
    Console.WriteLine();
    // Anonymous types have custom implementations of each virtual
    // method of System.Object. For example:
    Console.WriteLine($"ToString() == {car.ToString()}");
}
```
```cs

static void DefiningAnonymousType()
{
    BuildAnonymousType("Ford", "Red", 20);
}
DefiningAnonymousType();
```
```
You have a Red Ford going 20 MPH

ToString() == { Make = Ford, Color = Red, Speed = 20 }
```

Зверніть увагу, що анонімний тип також можна створити в одній строчці, окрім обгортання коду у функцію, як показано тут:

```cs
static void DefiningAnonymousType()
{
    //...
    Console.WriteLine();
    
    // Make an anonymous type representing a car.
    var myCar = new { Color = "Bright Pink", Make = "Saab", CurrentSpeed = 55 };
    // Now show the color and make.
    Console.WriteLine($"My car is a {myCar.Color} {myCar.Make}.");
}
DefiningAnonymousType();
```
```
My car is a Bright Pink Saab.
```
Отже, на цьому етапі просто зрозумійте, що анонімні типи дозволяють швидко моделювати «форму» даних з мінімальними накладними витратами. Ця техніка — це трохи більше, ніж спосіб створення нового типу даних на льоту, який підтримує основну інкапсуляцію за допомогою властивостей і діє відповідно до семантики на основі значень. Щоб зрозуміти останній пункт, давайте розглянемо, як компілятор C# створює анонімні типи під час компіляції та, зокрема, як він перевизначає члени System.Object.

## Внутрішнє представлення анонімних типів

Усі анонімні типи автоматично походять від System.Object і, отже, підтримують кожен із членів, наданих цим базовим класом. З огляду на це, ви можете викликати ToString(), GetHashCode(), Equals() або GetType() для неявно типізованого об'єкта myCar. Припустимо, що ваш файл Program.cs визначає таку статичну допоміжну функцію:

```cs
static void ReflectOverAnonymousType(object obj)
{
    Console.WriteLine($"obj is an instance of: {obj.GetType().Name}");
    Console.WriteLine($"Base class of {obj.GetType().Name} is {obj.GetType().BaseType}");
    Console.WriteLine($"obj.ToString() == {obj.ToString()}");
    Console.WriteLine($"obj.GetHashCode() == {obj.GetHashCode()}");
    Console.WriteLine();
}
```
Тепер припустимо, що ви викликаєте цей метод, передаючи об'єкт myCar як параметр, ось так:

```cs
static void ReflectCar()
{
    // Make an anonymous type representing a car.
    var myCar = new
    {
        Color = "Bright Pink",
        Make = "Saab",
        CurrentSpeed = 55
    };

    ReflectOverAnonymousType(myCar);
}
ReflectCar();
```
```
obj is an instance of: <>f__AnonymousType1`3
Base class of <>f__AnonymousType1`3 is System.Object
obj.ToString() == { Color = Bright Pink, Make = Saab, CurrentSpeed = 55 }
obj.GetHashCode() == -548328084
```
Спочатку зверніть увагу, що в цьому прикладі об'єкт myCar має тип <>f__AnonymousType1`3. Пам'ятайте, що призначене ім'я типу повністю визначається компілятором і не є безпосередньо доступним у вашій базі коду C#.
Мабуть, найважливіше зауважте, що кожна пара ім'я-значення, визначена за допомогою синтаксису ініціалізації об'єкта, відображається на властивість з ідентичною назвою, доступну лише для читання, та відповідне приватне поле ініціалізації, доступне лише для ініціалізації. Наведений нижче код C# наближено описує клас, згенерований компілятором, який використовується для представлення об'єкта myCar (що знову можна перевірити за допомогою ildasm.exe):

```
class private sealed '<>f__AnonymousType1'3'<'<Color>j__TPar',
  '<Make>j__TPar', <CurrentSpeed>j__TPar>'
  extends [System.Runtime][System.Object]
{
  // init-only fields.
  private initonly <Color>j__TPar <Color>i__Field;
  private initonly <CurrentSpeed>j__TPar <CurrentSpeed>i__Field;
  private initonly <Make>j__TPar <Make>i__Field;
  // Default constructor.
  public <>f__AnonymousType0(<Color>j__TPar Color,
    <Make>j__TPar Make, <CurrentSpeed>j__TPar CurrentSpeed);
  // Overridden methods.
  public override bool Equals(object value);
  public override int GetHashCode();
  public override string ToString();
  // Read-only properties.
  <Color>j__TPar Color { get; }
  <CurrentSpeed>j__TPar CurrentSpeed { get; }
  <Make>j__TPar Make { get; }
}
```

### Реалізація ToString() та GetHashCode()

Усі анонімні типи автоматично походять від System.Object та мають перевизначену версію Equals(), GetHashCode() та ToString(). Реалізація ToString() просто створює рядок з кожної пари ім'я-значення. Ось приклад:

```cs
public override string ToString()
{
  StringBuilder builder = new StringBuilder();
  builder.Append("{ Color = ");
  builder.Append(this.<Color>i__Field);
  builder.Append(", Make = ");
  builder.Append(this.<Make>i__Field);
  builder.Append(", CurrentSpeed = ");
  builder.Append(this.<CurrentSpeed>i__Field);
  builder.Append(" }");
  return builder.ToString();
}
```

Реалізація GetHashCode() обчислює хеш-значення, використовуючи змінні-члени кожного анонімного типу як вхідні дані для типу System.Collections.Generic.EqualityComparer\<T\>. Використовуючи цю реалізацію GetHashCode(), два анонімні типи повернуть однакове хеш-значення, якщо вони мають однаковий набір властивостей, яким було призначено однакові значення. З огляду на цю реалізацію, анонімні типи добре підходять для розміщення в контейнері Hashtable.

## Семантика рівності для анонімних типів

Хоча реалізація перевизначених методів ToString() та GetHashCode() є простою, вам може бути цікаво, як реалізовано метод Equals(). Наприклад, якщо ви визначите дві змінні «анонімні автомобілі», які вказують однакові пари ім'я-значення, чи вважатимуться ці дві змінні рівними? Щоб побачити результати на власні очі, оновіть свій клас Program.cs наступним новим методом:

```cs
static void EqualityTest()
{
    // Make 2 anonymous classes with identical name/value pairs.
    var firstCar = new { Color = "Bright Pink", Make = "Saab", CurrentSpeed = 55 };
    var secondCar = new { Color = "Bright Pink", Make = "Saab", CurrentSpeed = 55 };

    // Are they considered equal when using Equals()?
    if (firstCar.Equals(secondCar))
    {
        Console.WriteLine("Same anonymous object!");
    }
    else
    {
        Console.WriteLine("Not the same anonymous object!");
    }

    // Are they considered equal when using ==?
    if (firstCar == secondCar)
    {
        Console.WriteLine("Same anonymous object!");
    }
    else
    {
        Console.WriteLine("Not the same anonymous object!");
    }

    // Are these objects the same underlying type?
    if (firstCar.GetType().Name == secondCar.GetType().Name)
    {
        Console.WriteLine("We are both the same type!");
    }
    else
    {
        Console.WriteLine("We are different types!");
    }

    // Show all the details.
    Console.WriteLine();
    ReflectOverAnonymousType(firstCar);
    ReflectOverAnonymousType(secondCar);
}
EqualityTest();
```
```
Same anonymous object!
Not the same anonymous object!
We are both the same type!

obj is an instance of: <>f__AnonymousType1`3
Base class of <>f__AnonymousType1`3 is System.Object
obj.ToString() == { Color = Bright Pink, Make = Saab, CurrentSpeed = 55 }
obj.GetHashCode() == 1373988699

obj is an instance of: <>f__AnonymousType1`3
Base class of <>f__AnonymousType1`3 is System.Object
obj.ToString() == { Color = Bright Pink, Make = Saab, CurrentSpeed = 55 }
obj.GetHashCode() == 1373988699
```
Коли ви запустите цей тестовий код, ви побачите, що перша умовна перевірка, де ви викликаєте Equals(), повертає значення true, і тому на екран виводиться повідомлення «Same anonymous object!». Це пояснюється тим, що згенерований компілятором метод Equals() використовує семантику на основі значень під час перевірки рівності (наприклад, перевіряє значення кожного поля об'єктів, що порівнюються). 
Однак, друга умовна перевірка, яка використовує оператор рівності C# (==), виводить «Not the same anonymous object!» На перший погляд це може здатися дещо нелогічним. Такий результат пояснюється тим, що анонімні типи не отримують перевантажених версій операторів рівності C# (== та !=). З огляду на це, коли ви перевіряєте рівність анонімних типів за допомогою операторів рівності C# (а не методу Equals()), на рівність перевіряються посилання, а не значення, що зберігаються об'єктами.
Зрештою, в останньому умовному тесті (де ви перевіряєте назву базового типу), ви виявляєте, що анонімні типи є екземплярами одного й того ж типу класу, згенерованого компілятором (у цьому прикладі <>f AnonymousType1`3), оскільки firstCar та secondCar мають однакові властивості (Color, Make та CurrentSpeed).
Це ілюструє важливий, але тонкий момент: компілятор генеруватиме нове визначення класу лише тоді, коли анонімний тип містить унікальні імена анонімного типу. Таким чином, якщо оголошувати ідентичні анонімні типи (знову ж таки, тобто з однаковими іменами) в одній збірці, компілятор генерує лише одне анонімне визначення типу.

## Анонімні типи, що містять анонімні типи

Можна створити анонімний тип, який складається з інших анонімних типів. Наприклад, припустимо, що ви хочете змоделювати замовлення на купівлю, яке складається з позначки часу, ціни та придбаного автомобіля. Ось новий (трохи складніший) анонімний тип, що представляє таку сутність:

```cs
static void AnonymousTypesContainingAnonymousTypes()
{
    // Make an anonymous type that is composed of another.
    var purchaseItem = new
    {
        TimeBought = DateTime.Now,
        ItemBought = new { Color = "Red", Make = "Saab", CurrentSpeed = 55 },
        Price = 34.000
    };

    ReflectOverAnonymousType(purchaseItem);
    ReflectOverAnonymousType(purchaseItem.ItemBought);
}
AnonymousTypesContainingAnonymousTypes();
```
```
obj is an instance of: <>f__AnonymousType2`3
Base class of <>f__AnonymousType2`3 is System.Object
obj.ToString() == { TimeBought = 09.05.2025 12:55:16, ItemBought = { Color = Red, Make = Saab, CurrentSpeed = 55 }, Price = 34 }
obj.GetHashCode() == 707749463

obj is an instance of: <>f__AnonymousType1`3
Base class of <>f__AnonymousType1`3 is System.Object
obj.ToString() == { Color = Red, Make = Saab, CurrentSpeed = 55 }
obj.GetHashCode() == -1929040930
```
На цьому етапі ви повинні розуміти синтаксис, який використовується для визначення анонімних типів, але ви, можливо, все ще вагаєтесь, де саме (і коли) використовувати цю нову мовну функцію. Якщо бути відвертим, оголошення анонімних типів слід використовувати економно, зазвичай лише під час використання набору технологій LINQ. Ви ніколи не захочете відмовитися від використання строго типізованих класів/структур лише заради цього, враховуючи численні обмеження анонімних типів, які включають наступне:

1. Ви не контролюєте ім'я анонімного типу.
2. Анонімні типи завжди розширюють System.Object.
3. Поля та властивості анонімного типу завжди доступні лише для читання.
4. Анонімні типи не можуть підтримувати події, користувацькі методи, користувацькі оператори або користувацькі перевизначення.
5. Анонімні типи завжди неявно запечатані.
6. Анонімні типи завжди створюються за допомогою конструктора за замовчуванням.

Однак, програмуючи з використанням набору технологій LINQ, ви виявите, що в багатьох випадках цей синтаксис може бути корисним, коли потрібно швидко змоделювати загальну форму сутності, а не її функціональність.