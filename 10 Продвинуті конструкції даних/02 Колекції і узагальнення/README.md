# Колекції і узагальнення. Collections and Generics

Будь-якій програмі, яку ви створюєте на платформі .NET, доведеться вирішувати проблему підтримки та маніпулювання набором точок даних у пам’яті. Ці точки даних можуть надходити з будь-якого місця, включаючи реляційну базу даних, локальний текстовий файл, XML-документ, виклик веб-служби або, можливо, дані користувача.
Коли платформа .NET була вперше випущена, програмісти часто використовували класи простору імен System.Collections для зберігання та взаємодії з частинами даних, які використовуються в програмі. У .NET 2.0 мову програмування C# було вдосконалено для підтримки функції, що називається генерикою; завдяки цій зміні в бібліотеках базових класів було введено новий простір імен: System.Collections.Generic. 
У цій главі надано огляд різноманітних колекцій (загальних і незагальних) просторів імен і типів, що містяться в бібліотеках базових класів .NET Core. Як ви побачите, узагальнені контейнери часто краший вибір ніж неузагальненім аналоги, оскільки вони зазвичай забезпечують більшу безпеку типу та переваги продуктивності. Після того, як ви навчитеся створювати узагальнени елементи, знайдені у фреймворку, і маніпулювати ними, у цій главі буде розглянуто, як створювати власні узагальнені методи та типи. Роблячи це, ви дізнаєтеся про роль обмежень (і відповідного ключового слова where C#), які дозволяють створювати надзвичайно безпечні класи.

# Шо дає .Net для коллекцій даних

Найпримітивнішим контейнером, який можна використовувати для зберігання даних програми, безсумнівно, є масив. Як ви бачили в попередніх главах, масиви C# дозволяють визначати набір ідентично типізованих елементів (включно з масивом System.Objects, який по суті представляє масив будь-якого типу даних) із фіксованою верхньою межею. Усі масиви C# містять багато функціональних можливостей класу System.Array. Для швидкого огляду розглянемо наступний код, який створює масив текстових даних і маніпулює його вмістом різними способами (UseArray):

```cs
void UsingArray1()
{
    string[] strArray = { "First", "Second", "Third" };

    // Show number of items in array using Length property.
    Console.WriteLine($"This array has {strArray.Length} items.");
    Console.WriteLine();
    
    // Display contents using enumerator.
    foreach (string s in strArray)
    {
        Console.WriteLine($"Array Entry: {s}");
    }
    Console.WriteLine();

    // Reverse the array and print again.
    Array.Reverse(strArray);
    foreach (string s in strArray)
    {
        Console.WriteLine($"Array Entry: {s}");
    }

}
UsingArray1();
```
```
This array has 3 items.

Array Entry: First
Array Entry: Second
Array Entry: Third

Array Entry: Third
Array Entry: Second
Array Entry: First

```
Хоча базові масиви можуть бути корисними для керування невеликими обсягами даних фіксованого розміру, є багато інших випадків, коли вам потрібна більш гнучка структура даних, наприклад контейнер, що динамічно зростає та зменшується, або контейнер, який може містити об’єкти, які відповідають лише певному критерію (наприклад, лише об’єкти, що походять від певного базового класу, або лише об’єкти, що реалізують певний інтерфейс). Коли ви використовуєте простий масив, завжди пам’ятайте, що вони створюються з «фіксованим розміром». Якщо створити масив із трьох елементів, ви отримаєте лише три елементи; отже, наступний код призведе до виняткової ситуації під час виконання (точніше, виключення IndexOutOfRangeException):

```cs
void ArrayHasFixedSize()
{
    string[] strArray = { "First", "Second", "Third" };

    strArray[3] = "Hi";
}
ArrayHasFixedSize();
```
```
Unhandled exception. System.IndexOutOfRangeException: Index was outside the bounds of the array.
```

Щоб подолати обмеження простого масиву, бібліотеки базових класів .NET постачаються з кількома просторами імен, що містять класи колекції. На відміну від простого масиву C#, класи колекції створені для динамічної зміни розміру на льоту, коли ви вставляєте або видаляєте елементи. Крім того, багато класів колекції забезпечують підвищену безпеку типів і оптимізовані для обробки даних, що містяться, з ефективним використанням пам’яті. Читаючи цю главу, ви швидко помітите, що клас колекції може належати до однієї з двох широких категорій.

    1. Незагальні колекції (в основному знаходяться в просторі імен System.Collections)
    2. Загальні колекції (переважно знаходяться в просторі імен System.Collections.Generic)

Незагальні колекції, як правило, призначені для роботи з типами System.Object і, отже, є вільно типізованими контейнерами (однак деякі незагальні колекції працюють лише з певним типом даних, наприклад, рядковими об’єктами). Навпаки, загальні колекції є набагато безпечнішими щодо типів, враховуючи, що ви повинні вказати «тип типу», який вони містять під час створення. Як ви побачите, контрольною ознакою будь-якого загального елемента є «параметр типу», позначений кутовими дужками (наприклад, List\<T\>). Деталі генериків (включно з багатьма перевагами, які вони надають) ви розглянете трохи пізніше в цьому розділі. Давайте розглянемо деякі з ключових незагальних типів колекцій у просторах імен System.Collections і System.Collections.Specialized.

# System.Collections namespace

Коли платформа .NET була вперше випущена, програмісти часто використовували неузагальнені класи колекцій, що знаходяться в просторі імен System.Collections, який містить набір класів, що використовуються для керування та впорядкування великих обсягів даних у пам'яті. У таблиці задокументовано деякі з найбільш часто використовуваних класів колекцій цього простору імен та основні інтерфейси, які вони реалізують.

|Клас System.Collections|Сенс у використані|Ключові реалізовані інтерфейси|
|-----------------------|------------------|------------------------------|
|ArrayList|Представляє динамічно змінену колекцію об'єктів, перелічених у послідовному порядку|IList, ICollection, IEnumerable, and ICloneable|
|BitArray|Керує компактним масивом бітових значень, які представлені у вигляді логічних значень, де true вказує, що біт увімкнено (1), а false вказує, що біт вимкнено (0)|ICollection, IEnumerable, and ICloneable|
|Hashtable|Представляє колекцію пар ключ-значення, організованих на основі хеш-коду ключа|IDictionary, ICollection, IEnumerable, and ICloneable|
|Queue|Представляє стандартну колекцію об'єктів за принципом FIFO (перший прийшов, перший вийшов).|ICollection, IEnumerable, and ICloneable|
|SortedList|Представляє колекцію пар ключ-значення, відсортованих за ключами та доступних за ключем та індексом.|IDictionary, ICollection, IEnumerable, and ICloneable|
|Stack|Стек «останнім прийшов, першим вийшов» (LIFO), що забезпечує функціональність push та pop (і peek)|ICollection, IEnumerable, and ICloneable|

Інтерфейси, реалізовані цими класами колекцій, надають широке уявлення про їхню загальну функціональність. У таблиці задокументовано загальний характер цих ключових інтерфейсів, з деякими з яких ви працювали.

|Інтерфейс System.Collections|Сенс у використані|
|----------------------------|------------------|
|ICollection|Визначає загальні характеристики (наприклад, розмір, перерахування та безпеку потоків) для всіх неузагальнених типів колекцій|
|ICloneable|Дозволяє реалізуючому об'єкту повертати копію себе викликаючій функції|
|IDictionary|Дозволяє неузагальненому об'єкту колекції представляти свій вміст за допомогою пар ключ-значення|
|IEnumerable|Повертає об'єкт, що реалізує інтерфейс IEnumerator (див. наступний запис таблиці)|
|IEnumerator|Дозволяє ітерацію елементів колекції у стилі foreach|
|IList|Забезпечує поведінку для додавання, видалення та індексування елементів у послідовному списку об'єктів|

## Ілюстративний приклад: робота з об'єктом ArrayList

Виходячи з вашого досвіду, ви можете мати певний безпосередній досвід використання (або реалізації) деяких із цих класичних структур даних, таких як стеки, черги та списки. Якщо це не так, я надам додаткові деталі щодо їхніх відмінностей, коли ви розглянете їхні загальні аналоги трохи пізніше в цьому розділі. А поки що, ось приклад коду з використанням об'єкта ArrayList(проект UseArrayList):

```cs
using System.Collections;

void UsingArrayList()
{
    ArrayList strArray = new ArrayList();
    strArray.AddRange(new string[] { "First", "Second", "Third" });

    // Show number of items in ArrayList.
    Console.WriteLine($"This collection has {strArray.Count} items.");
    Console.WriteLine();

    // Add a new item and display current count.
    strArray.Add("Fourth!");
    Console.WriteLine($"This collection has {strArray.Count} items.");
    Console.WriteLine();

    // Display contents.
    foreach (string s in strArray)
    {
        Console.WriteLine($"Entry: {s}");
    }
}
UsingArrayList();

```
```
This collection has 3 items.

This collection has 4 items.

Entry: First
Entry: Second
Entry: Third
Entry: Fourth!
```
Зверніть увагу, що ви можете додавати (або видаляти) елементи на льоту, і контейнер автоматично змінює розмір відповідно. 
Як ви могли здогадатися, клас ArrayList має багато корисних членів, окрім властивості Count та методів AddRange() і Add(), тому обов'язково ознайомтеся з документацією .NET для отримання повної інформації. До речі, інші класи System.Collections (Stack, Queue тощо) також повністю задокументовані в довідковій системі .NET.
Однак важливо зазначити, що більшість ваших проектів .NET Core, найімовірніше, не використовуватимуть класи колекцій з простору імен System.Collections! Звичайно, в наші дні набагато частіше використовуються узагальнені класи-аналоги з простору імен System.Collections.Generic. З огляду на це, я не коментуватиму (або наводитиму приклади коду) решту неузагальнених класів, що знаходяться в System.Collections.

## Огляд простору імен System.Collections.Specialized

System.Collections — це не єдиний простір імен .NET Core, який містить неузагальнені класи колекцій. Простір імен System.Collections.Specialized визначає низку спеціалізованих типів колекцій. У таблиці задокументовано деякі з найбільш корисних типів у цьому конкретному просторі імен, орієнтованому на колекції, всі з яких не є узагальненими.

|Тип System.Collections.Specialized|Сенс у використані|
|----------------------------------|------------------|
|HybridDictionary|Цей клас реалізує IDictionary, використовуючи ListDictionary, коли колекція мала, а потім перемикаючись на Hashtable, коли колекція стає великою.|
|ListDictionary|Цей клас корисний, коли вам потрібно керувати невеликою кількістю елементів (приблизно десять), які можуть змінюватися з часом. Цей клас використовує однозв'язний список для зберігання своїх даних.|
|StringCollection|Цей клас забезпечує оптимальний спосіб керування великими колекціями рядкових даних.|
|BitVector32|Цей клас надає просту структуру, яка зберігає логічні значення та малі цілі числа у 32 бітах пам'яті.|

Окрім цих конкретних типів класів, цей простір імен також містить багато додаткових інтерфейсів та абстрактних базових класів, які можна використовувати як відправну точку для створення власних класів колекцій. Хоча ці «спеціалізовані» типи можуть бути саме тим, що потрібно вашим проектам у деяких ситуаціях, я не буду коментувати їх використання тут. Знову ж таки, у багатьох випадках ви, ймовірно, виявите, що простір імен System.Collections.Generic надає класи з подібною функціональністю та додатковими перевагами.

## Проблеми неузагальнених колекцій

Хоча правда, що багато успішних програм .NET були створені протягом багатьох років з використанням цих неузагальнених класів колекцій (та інтерфейсів), історія показала, що використання цих типів може призвести до низки проблем.
Перша проблема полягає в тому, що використання класів System.Collections та System.Collections.Specialized може призвести до низької продуктивності коду, особливо під час маніпулювання числовими даними (наприклад, типами значень). Як ви побачите найближчим часом, CLR повинна виконувати низку операцій передачі пам'яті, коли ви зберігаєте структури в будь-якому неузагальненому класі колекції, прототипом якого є робота з System.Objects, що може негативно вплинути на швидкість виконання.
Друга проблема полягає в тому, що більшість неузагальнених класів колекцій не є типобезпечними, оскільки (знову ж таки) вони були розроблені для роботи з System.Objects, і тому вони можуть містити будь-що. Якщо розробнику потрібно було створити колекцію з високим рівнем типобезпечності (наприклад, контейнер, який може містити об'єкти, що реалізують лише певний інтерфейс), єдиним реальним варіантом було створити новий клас колекції вручну. Робити це було не надто трудомістко, але трохи нудно.
Перш ніж розглядати, як використовувати дженерики у ваших програмах, вам буде корисно детальніше розглянути проблеми неузагальнених класів колекцій; це допоможе вам краще зрозуміти проблеми, які дженерики мають вирішити в першу чергу. Створіть новий проект консольної програми з назвою IssuesWithNonGenericCollections. 
Додайте:

```cs
using System.Collections;
```

## Проблема продуктивності.

Платформа .NET підтримує дві широкі категорії даних: типи значень та типи посилань. Оскільки .NET визначає дві основні категорії типів, іноді може знадобитися представити змінну однієї категорії як змінну іншої категорії. Для цього C# надає простий механізм, який називається boxing, для зберігання даних у типі значення всередині посилальної змінної.Припустимо, що ви створили локальну змінну типу int у методі під назвою SimpleBoxUnboxOperation. Якщо під час роботи вашої програми ви представите цей тип значення як тип посилання, ви об'єднаєте значення в блок наступним чином:

```cs
static void SimpleBoxUnboxOperation()
{
  // Make a ValueType (int) variable.
  int myInt = 25;
  // Box the int into an object reference.
  object boxedInt = myInt;
}
```
Пакування можна формально визначити як процес явного присвоєння типу значення змінній System.Object. Коли ви поміщаєте значення в блок, CLR виділяє новий об'єкт у купі та копіює значення типу значення (у цьому випадку 25) у цей екземпляр. Вам повертається посилання на щойно виділений об'єкт на основі купи.
Протилежна операція також дозволена за допомогою розпакування. Розпакування – це процес перетворення значення, що міститься в посиланні на об'єкт, назад у відповідний тип значення на стеку. Синтаксично кажучи, операція розпакування виглядає як звичайна операція приведення типів. Однак семантика дещо відрізняється. CLR починає з перевірки, чи тип даних, що приймається, еквівалентний типу, що утворюється в блоці, і якщо так, то копіює значення назад у локальну змінну на основі стека. Наприклад, наступні операції розпакування працюють успішно, враховуючи, що базовий тип boxedInt справді є цілим числом:

```cs
static void SimpleBoxUnboxOperation()
{
  // Make a ValueType (int) variable.
  int myInt = 25;
  // Box the int into an object reference.
  object boxedInt = myInt;
  // Unbox the reference back into a corresponding int.
  int unboxedInt = (int)boxedInt;
}
```
Коли компілятор C# зустрічає синтаксис упаковки/розпакування, він генерує CIL-код, який містить коди операцій упаковки/розпакування. Якби ви перевірили скомпільовану збірку за допомогою ildasm.exe, ви б виявили наступне:

```
.method assembly hidebysig static
    void  '<<Main>$>g__SimpleBoxUnboxOperation|0_0'() cil managed
{
  .maxstack  1
  .locals init (int32 V_0, object V_1, int32 V_2)
    IL_0000:  nop
    IL_0001:  ldc.i4.s   25
    IL_0003:  stloc.0
    IL_0004:  ldloc.0
    IL_0005:  box        [System.Runtime]System.Int32
    IL_000a:  stloc.1
    IL_000b:  ldloc.1
    IL_000c:  unbox.any  [System.Runtime]System.Int32
    IL_0011:  stloc.2
    IL_0012:  ret
  } // end of method '<Program>$'::'<<Main>$>g__SimpleBoxUnboxOperation|0_0'
```
Пам'ятайте, що на відміну від типового приведення типів, вам необхідно розпакувати дані у відповідний тип. Якщо ви спробуєте розпакувати фрагмент даних у неправильний тип даних, буде викинуто виняток InvalidCastException. Для повної безпеки слід обгортати кожну операцію розпакування логікою try/catch; однак це було б досить трудомістко для кожної операції розпакування. Розглянемо наступне оновлення коду, яке викличе помилку, оскільки ви намагаєтеся розпакувати упаковане ціле число в довге:

```cs
static void SimpleBoxUnboxOperation()
{
  // Make a ValueType (int) variable.
  int myInt = 25;
  // Box the int into an object reference.
  object boxedInt = myInt;
  // Unbox in the wrong data type to trigger
  // runtime exception.
  try
  {
    long unboxedLong = (long)boxedInt;
  }
  catch (InvalidCastException ex)
  {
    Console.WriteLine(ex.Message);
  }
}
```
На перший погляд, упаковка/розпакування може здатися досить безпрограшною мовною функцією, яка є радше академічною, ніж практичною. Зрештою, вам рідко знадобиться зберігати локальний тип значення в локальній об'єктній змінній, як показано тут. Однак, виявляється, що процес упаковки/розпакування є досить корисним, оскільки дозволяє припустити, що все можна розглядати як System.Object, тоді як CLR піклується про деталі, пов'язані з пам'яттю, за вас.
Давайте розглянемо практичне використання цих методів. Ми розглянемо клас System.Collections.ArrayList та використаємо його для зберігання пакета числових (розподілених у стеку) даних. Відповідні члени класу ArrayList перелічені нижче. Зверніть увагу, що вони прототиповані для роботи з даними System.Object. Тепер розглянемо методи Add(), Insert() та Remove(), а також індексатор класу.

```cs
public class ArrayList : IList, ICloneable
{
...
  public virtual int Add(object? value);
  public virtual void Insert(int index, object? value);
  public virtual void Remove(object? obj);
  public virtual object? this[int index] {get; set; }
}
```
ArrayList був створений для роботи з об'єктами, які представляють дані, виділені в купі, тому може здатися дивним, що наступний код компілюється та виконується без виникнення помилки:

```cs
static void WorkWithArrayList()
{
    // Value types are automatically boxed when
    // passed to a method requesting an object.
    ArrayList myInts = new ArrayList();
    myInts.Add(10);
    myInts.Add(20);
    myInts.Add(35);
}
```
Хоча ви передаєте числові дані безпосередньо в методи, що потребують об'єкта, середовище виконання автоматично упаковує дані зі стеку від вашого імені. Пізніше, якщо ви захочете отримати елемент з ArrayList за допомогою індексатора типів, вам потрібно розпакувати об'єкт, виділений у купі, у ціле число, виділене у стеку, за допомогою операції приведення типів. Пам'ятайте, що індексатор ArrayList повертає System.Objects, а не System.Int32s.

```cs
static void WorkWithArrayList()
{
    // Value types are automatically boxed when
    // passed to a method requesting an object.
    ArrayList myInts = new ArrayList();
    myInts.Add(10);
    myInts.Add(20);
    myInts.Add(35);

    // Unboxing occurs when an object is converted back to
    // stack-based data.
    int i = (int)myInts[0];
    // Now it is reboxed, as WriteLine() requires object types!
    Console.WriteLine($"Value of your int: {i}");
}
WorkWithArrayList();
```
```
Value of your int: 10
```
Знову ж таки, зверніть увагу, що виділений у стеку System.Int32 упаковується перед викликом ArrayList.Add(), тому його можна передати в потрібному System.Object. Також зверніть увагу, що System.Object розпаковується назад у System.Int32 після отримання з ArrayList за допомогою операції приведення типів, а потім знову упаковується під час передачі до методу Console.WriteLine(), оскільки цей метод працює зі змінними System.Object.
З точки зору програміста, упаковка та розпакування зручні, але цей спрощений підхід до передачі пам'яті в стек/купу пов'язаний з проблемами продуктивності (як у швидкості виконання, так і в розмірі коду) та відсутністю безпеки типів. Щоб зрозуміти проблеми з продуктивністю, поміркуйте над цими кроками, які необхідно виконати для упаковки та розпакування простого цілого числа:

    1. Новий об'єкт має бути розміщений в керованій купі.
    2. Значення даних зі стеку має бути передано в цю область пам'яті.
    3. Після розпакування значення, що зберігається в купі, має бути передано назад у стек.
    4. Невикористаний об'єкт у купі (зрештою) буде викинутий у збирання сміття.

Хоча цей конкретний метод WorkWithArrayList() не спричинить суттєвого обмеження продуктивності, ви точно можете відчути вплив, якщо ArrayList містив тисячі цілих чисел, якими ваша програма маніпулює досить регулярно. В ідеальному світі ви могли б маніпулювати даними на основі стеку в контейнері без будь-яких проблем із продуктивністю. В ідеалі було б добре, якби вам не доводилося витягувати дані з цього контейнера за допомогою try/catch областей видимості (саме цього дозволяють досягти generics).

## Проблема безпеки типів

Я торкнувся питання безпеки типів, розглядаючи операції розпакування. Пам'ятайте, що ви повинні розпакувати свої дані в той самий тип даних, який вони були оголошені, що й перед упаковкою. Однак, є ще один аспект безпеки типів, який слід пам'ятати у світі без узагальнень: той факт, що більшість класів System.Collections зазвичай можуть містити будь-що, оскільки їхні члени прототипуються для роботи з System.Objects. Наприклад, цей метод створює ArrayList випадкових бітів непов'язаних даних:

```cs
static void ArrayListOfRandomObjects()
{
  // The ArrayList can hold anything at all.
  ArrayList allMyObjects = new ArrayList();
  allMyObjects.Add(true);
  allMyObjects.Add(new OperatingSystem(PlatformID.MacOSX, new Version(10, 0)));
  allMyObjects.Add(66);
  allMyObjects.Add(3.14);
}
```
У деяких випадках вам знадобиться надзвичайно гнучкий контейнер, який може містити буквально все (як показано тут). Однак, здебільшого вам потрібен контейнер, безпечний за типами, який може працювати лише з певним типом точок даних. Наприклад, вам може знадобитися контейнер, який може містити лише підключення до бази даних, растрові зображення або об'єкти, сумісні з IPointy.
До появи узагальнювачів єдиним способом вирішення цієї проблеми безпеки типів було створення власного (строго типізованого) класу колекції вручну. Припустимо, ви хочете створити власну колекцію, яка може містити лише об'єкти типу Person.

```cs
namespace IssuesWithNonGenericCollections;

public class Person
{
    public int Age { get; set; }
    public string FirstName { get; set; } = "Undefined";
    public string LastName { get; set; } = "Undefined";

    public Person() { }
    public Person(string firstName, string lastName, int age)
    {
        Age = age;
        FirstName = firstName;
        LastName = lastName;
    }

    public override string? ToString()
    {
        return $"Name: {FirstName} {LastName}, Age: {Age}"; 
    }
}
```
Щоб створити колекцію, яка може містити лише об'єкти Person, ви можете визначити змінну-член System.Collections.ArrayList у класі з іменем PersonCollection та налаштувати всі члени для роботи зі строго типізованими об'єктами Person, а не з типами System.Object. Ось простий приклад (користувацька колекція виробничого рівня може підтримувати багато додаткових членів і може розширювати абстрактний базовий клас з простору імен System.Collections або System.Collections.Specialized):

```cs
using System.Collections;

namespace IssuesWithNonGenericCollections;

public class PersonCollection : IEnumerable
{
    private ArrayList arPeople = new ArrayList();
    public IEnumerator GetEnumerator() =>
        arPeople.GetEnumerator();

    // Cast for caller.
    public Person GetPerson(int pos) => (Person)arPeople[pos];
    
    // Insert only Person objects.
    public void AddPerson(Person p)
    {
        arPeople.Add(p);
    }
    // Other methods
    public void ClearPeople()
    {
        arPeople.Clear();
    }
    public int Count => arPeople.Count;
}
```
Зверніть увагу, що клас PersonCollection реалізує інтерфейс IEnumerable, який дозволяє ітерацію, подібну до foreach, для кожного елемента, що міститься в ньому. Також зверніть увагу, що ваші методи GetPerson() та AddPerson() були прототиповані для роботи лише з об'єктами Person, а не з растровими зображеннями, рядками, підключеннями до бази даних чи іншими елементами. Визначивши ці типи, ви можете бути впевнені в безпеці типів, оскільки компілятор C# зможе визначити будь-яку спробу вставки несумісного типу даних.
Використаємо це:

```cs
static void UsePersonCollection()
{
    PersonCollection myPeople = new PersonCollection();
    myPeople.AddPerson(new Person("Homer", "Simpson", 40));
    myPeople.AddPerson(new Person("Marge", "Simpson", 38));
    myPeople.AddPerson(new Person("Lisa", "Simpson", 9));
    myPeople.AddPerson(new Person("Bart", "Simpson", 7));
    myPeople.AddPerson(new Person("Maggie", "Simpson", 2));

    // This would be a compile-time error!
    // myPeople.AddPerson(new Car());

    Console.WriteLine(myPeople.GetPerson(1));
    Console.WriteLine();

    foreach (Person p in myPeople)
    {
        Console.WriteLine(p);
    }
}
UsePersonCollection();
```
```
Name: Marge Simpson, Age: 38

Name: Homer Simpson, Age: 40
Name: Marge Simpson, Age: 38
Name: Lisa Simpson, Age: 9
Name: Bart Simpson, Age: 7
Name: Maggie Simpson, Age: 2
```
Хоча користувацькі колекції забезпечують безпеку типів, такий підхід ставить вас у ситуацію, коли вам доведеться створювати (майже ідентичну) користувацьку колекцію для кожного унікального типу даних, який ви хочете містити. Таким чином, якщо вам потрібна власна колекція, яка може працювати лише з класами, похідними від базового класу Car, вам потрібно створити дуже схожий клас колекції.

```cs
namespace IssuesWithNonGenericCollections;

public class Car
{
    public int Id { get; set; }
    public string Make { get; set; } = "Undefined";
    public string Model { get; set; } = "Undefined";
    public int Year { get; set; }
}
```

```cs
using System.Collections;

namespace IssuesWithNonGenericCollections;

public class CarCollection : IEnumerable
{
    private ArrayList arCars = new ArrayList();

    public IEnumerator GetEnumerator() => arCars.GetEnumerator();

    // Cast for caller.
    public Car GetCar(int pos) => (Car)arCars[pos];
    // Insert only Car objects.
    public void AddCar(Car c)
    {
        arCars.Add(c);
    }
    public void ClearCars()
    {
        arCars.Clear();
    }
    public int Count => arCars.Count;
}

```

Однак, користувацький клас колекції ніяк не вирішує проблему продуктивності за упаковку/розпакування. Навіть якби ви створили власну колекцію з назвою IntCollection, яку ви призначили для роботи лише з елементами System.Int32, вам довелося б виділити певний тип об'єкта для зберігання даних (наприклад, System.Array та ArrayList).

```cs
using System.Collections;
public class IntCollection : IEnumerable
{
  private ArrayList arInts = new ArrayList();
  // Get an int (performs unboxing!).
  public int GetInt(int pos) => (int)arInts[pos];
  // Insert an int (performs boxing)!
  public void AddInt(int i)
  {
    arInts.Add(i);
  }
  public void ClearInts()
  {
    arInts.Clear();
  }
  public int Count => arInts.Count;
  IEnumerator IEnumerable.GetEnumerator() => arInts.GetEnumerator();
}
```
Незалежно від того, який тип ви оберете для зберігання цілих чисел, ви не зможете уникнути дилеми упаковки, використовуючи неузагальнені контейнери.

## Перший погляд на узагальнені колекції

Використовуючи узагальнені класи колекцій, ви виправляєте всі попередні проблеми, включаючи падіня продуктивності за упаковку/розпакування та відсутність безпеки типів. Також, потреба у створенні власного класу колекції стає досить рідкою. Замість того, щоб створювати унікальні класи, які можуть містити людей, автомобілі та цілі числа, ви можете використовувати універсальний клас колекції та вказати тип типу.

```cs
static void UseGenericList()
{
    // This List<> can hold only Person objects.
    List<Person> morePeople = new List<Person>();
    morePeople.Add(new Person("Frank", "Black", 50));
    Console.WriteLine(morePeople[0]);

    // This List<> can hold only integers.
    List<int> moreInts = new List<int>();
    moreInts.Add(10);
    moreInts.Add(2);
    int sum = moreInts[0] + moreInts[1];
    Console.WriteLine(sum);

    // Compile-time error! Can't add Person object
    // to a list of ints!
    //moreInts.Add(new Person());
}
UseGenericList();
```
```
Name: Frank Black, Age: 50
12
```
Перший об'єкт List<T> може містити лише об'єкти Person. Таким чином, вам не потрібно виконувати приведення типів під час вилучення елементів з контейнера, що робить цей підхід більш типобезпечним. Другий List<T> може містити лише цілі числа, всі з яких розміщуються в стеку; іншими словами, немає прихованого пакування чи розпакування, як ви виявили у випадку з неузагальненим ArrayList. Ось короткий перелік переваг, які надають generic контейнери порівняно з їхніми  nongeneric аналогами:

    Узагальнені типи забезпечують кращу продуктивність, оскільки вони не призводять до  упаковку або розпакування під час зберігання типів значень.

    Узагальнені типи є безпечними, оскільки вони можуть містити лише вказаний вами тип.

    Узагальнені типи значно зменшують потребу у створенні власних типів колекцій, оскільки ви вказуєте «тип типу» під час створення контейнера-узагальнення.

# Роль параметрів загального типу

Ви можете знайти загальні класи, інтерфейси, структури та делегати в бібліотеках базових класів .NET, і вони можуть бути частиною будь-якого простору імен .NET. Також майте на увазі, що узагальнені типи мають набагато більше застосувань, ніж просто визначення класу колекції. Звісно, ​​ви побачите багато різних узагальнен, що використовуються в решті цього тексту.

    Тільки класи, структури, інтерфейси та делегати можна писати узагальнено; enum типи – ні.

Коли ви бачите універсальний елемент, перелічений у документації .NET або в браузері об'єктів Visual Studio в гілці System.Collections.Generic, ви помітите пару кутових дужок з літерою або іншим токеном всередині(наприклад List\<T\>). 
Формально кажучи, ці токени називаються параметрами типу; однак, якщо говорити зручніше, їх можна просто називати заповнювачами. Символ <T> можна прочитати як «of T». Таким чином, ви можете прочитати IEnumerable\<T\> як «IEnumerable типу T». 

    Назва параметра типу (заповнювача) не має значення, і це залежить від розробника, який створив універсальний елемент. Однак, зазвичай T використовується для представлення типів, TKey або K – для ключів, а TValue або V – для значень.

Коли ви створюєте узагальнений об'єкт, реалізуєте узагальнений інтерфейс або викликаєте узагальнений член, саме ви повинні надати значення параметру типу. Ви побачите багато прикладів у цьому розділі та в решті тексту. Однак, щоб підготувати основу, давайте розглянемо основи взаємодії з узагальненими типами та членами.

## Визначення параметрів типу для універсальних класів/структур

Під час створення екземпляра універсального класу або структури ви вказуєте параметр типу під час оголошення змінної та під час виклику конструктора. Як ви бачили в попередньому прикладі коду, UseGenericList() визначив два об'єкти List\<T\>.

```cs
// This List<> can hold only Person objects.
List<Person> morePeople = new List<Person>();
// This List<> can hold only integers.
List<int> moreInts = new List<int>();
```

Перший рядок попереднього фрагмента коду можна прочитати як «List<> типу T, де T має тип Person». Або, простіше кажучи, ви можете прочитати це як «список об’єктів Person». Після того, як ви вкажете параметр типу універсального елемента, його не можна змінити (пам’ятайте, що в узагальненнях головне – безпека типів). Коли ви вказуєте параметр типу для універсального класу або структури, усі входження заповнювача (заповнювачів) тепер замінюються наданим вами значенням.
Якби ви переглянули повне оголошення універсального класу List<T> за допомогою браузера об'єктів Visual Studio, ви б побачили, що заповнювач T використовується по всьому визначенню типу List<T>. Ось частковий список:
```cs
// A partial listing of the List<T> class.
namespace System.Collections.Generic;
  public class List<T> : IList<T>, IList, IReadOnlyList<T>
  {
...
  public void Add(T item);
  public void AddRange(IEnumerable<T> collection);
  public ReadOnlyCollection<T> AsReadOnly();
  public int BinarySearch(T item);
  public bool Contains(T item);
  public void CopyTo(T[] array);
  public int FindIndex(System.Predicate<T> match);
  public T FindLast(System.Predicate<T> match);
  public bool Remove(T item);
  public int RemoveAll(System.Predicate<T> match);
  public T[] ToArray();
  public bool TrueForAll(System.Predicate<T> match);
  public T this[int index] { get; set; }
}
```
Коли ви створюєте List\<T\>, що визначає об'єкти Person, це так, ніби тип List\<T\> було визначено наступним чином:

```cs
namespace System.Collections.Generic;
public class List<Person>
  : IList<Person>, IList, IReadOnlyList<Person>
{
...
  public void Add(Person item);
  public void AddRange(IEnumerable<Person> collection);
  public ReadOnlyCollection<Person> AsReadOnly();
  public int BinarySearch(Person item);
  public bool Contains(Person item);
  public void CopyTo(Person[] array);
  public int FindIndex(System.Predicate<Person> match);
  public Person FindLast(System.Predicate<Person> match);
  public bool Remove(Person item);
  public int RemoveAll(System.Predicate<Person> match);
  public Person[] ToArray();
  public bool TrueForAll(System.Predicate<Person> match);
  public Person this[int index] { get; set; }
}
```
Звісно, ​​коли ви створюєте узагальнену змінну List\<T\>, компілятор буквально не створює нову реалізацію класу List\<T\>. Швидше, він звертатиметься лише до членів узагальненого типу, який ви фактично викликаєте.

## Визначення параметрів типу для універсальних членів

Неузагальнений клас або структура можуть підтримувати універсальні властивості. У цих випадках вам також потрібно буде вказати значення заповнювача під час виклику методу. Наприклад, System.Array підтримує кілька універсальних методів. Зокрема, неузагальнений статичний метод Sort() тепер має узагальнений аналог під назвою Sort<T>(). Розглянемо наступний фрагмент коду, де T має тип int(проект UseArray):
```cs
void SpecifyingTypeParametersForGenericMembers()
{
    int[] myInts = { 10, 4, 2, 33, 93 };
    // Specify the placeholder to the generic
    // Sort<>() method.
    Array.Sort<int>(myInts);
    foreach (int i in myInts)
    {
        Console.WriteLine(i);
    }
}
SpecifyingTypeParametersForGenericMembers();

```

## Визначення параметрів типу для універсальних інтерфейсів

Реалізація універсальних інтерфейсів є поширеною практикою під час створення класів або структур, які повинні підтримувати різні режими роботи фреймворку (наприклад, клонування, сортування та перерахування). Ви вже дізналися про низку неузагальнених інтерфейсів, таких як IComparable, IEnumerable, IEnumerator та IComparer. Нагадаємо, що неузагальнений інтерфейс IComparable був визначений так:

```cs
public interface IComparable
{
  int CompareTo(object obj);
}
```
Додамо проект GenericInterfaces. Додайте клас з реалізацією інтерфейса IComparable. 

```cs
namespace GenericInterfaces;

class Car : IComparable
{
    // Constant for maximum speed.
    public const int MaxSpeed = 100;

    // Car properties.
    public int CarId { get; set; }
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "No-name";
    public override string? ToString() => $"{CarId}\t{PetName}\t{CurrentSpeed}";

    public Car()
    {
    }

    public Car(int carId, string petName, int currentSpeed)
    {
        CarId = carId;
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public int CompareTo(object? obj)
    {
        if (obj is Car anotherCar)
        {
            return CarId.CompareTo(anotherCar.CarId);
        }
        else
        {
            throw new ArgumentException("Parameter is not a type Car!");
        }
    }
}
```
Маючи такий клас ми можемо:

```cs
using GenericInterfaces;

void ShowCarArray()
{
    Car[] cars =
    [
        new Car(1, "Rusty", 80),
        new Car(234, "Mary", 40),
        new Car(34, "Viper", 30),
        new Car(4, "Mel", 70),
        new Car(5, "Chucky", 80),
    ];
    Array.Sort(cars);

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }
}
ShowCarArray();
```
```
1       Rusty   80
4       Mel     70
5       Chucky  80
34      Viper   30
234     Mary    40
```
Тепер припустимо, що ви використовуєте загальний аналог цього інтерфейсу.

```cs
public interface IComparable<T>
{
  int CompareTo(T obj);
}
```
У цьому випадку ваш код реалізації буде значно очищено.

```cs
namespace GenericInterfaces;

class Car1 : IComparable<Car1>
{
    //...

    public int CompareTo(Car1? car)
    {
        return CarId.CompareTo(car?.CarId);
    }
}
```
Тут вам не потрібно перевіряти, чи вхідний параметр є типом Car1, оскільки це може бути лише Car1. Якщо хтось передасть несумісний тип даних, ви отримаєте помилку під час компіляції. Тепер, коли ви краще розумієте, як взаємодіяти з узагальненими елементами, а також роль параметрів типу (також відомих як заповнювачі), ви готові дослідити класи та інтерфейси простору імен System.Collections.Generic.

# Простір імен System.Collections.Generic

Коли ви створюєте .NET-застосунок і вам потрібен спосіб керування даними в пам'яті, класи System.Collections.Generic, найімовірніше, підійдуть. На початку цієї глави я коротко згадав деякі основні неузагальнені інтерфейси, реалізовані неузагальненими класами колекцій. Не дивно, що простір імен System.Collections.Generic визначає узагальнені заміни для багатьох із них. Фактично, ви можете знайти низку універсальних інтерфейсів, які розширюють свої неузагальнені аналоги. Фактично, ви можете знайти низку універсальних інтерфейсів, які розширюють свої неузагальнені аналоги. Це може здатися дивним; однак, роблячи це, реалізація класів також підтримуватиме функціональність, що існує в їхніх неузагальнених аналогах. Наприклад, IEnumerable<T> розширює IEnumerable. У таблиці документовано основні узагальнені інтерфейси, з якими ви зіткнетеся під час роботи з узагальненими класами колекцій.

Ключові інтерфейси, що підтримуються класами System.Collections.Generic

|System.Collections.Generic інтерфейс|Сенс у використані|
|------------------------------------|------------------|
|ICollection\<T\>|Визначає загальні характеристики (наприклад, розмір, перерахування та безпеку потоків) для всіх типів універсальних колекцій.|
|IComparer\<T\>|Визначає спосіб порівняння з об'єктами.|
|IDictionary\<TKey, TValue\>|Дозволяє універсальному об'єкту колекції представляти свій вміст за допомогою пар ключ-значення.|
|IEnumerable\<T\>/IAsyncEnumerable\<T\>|Повертає інтерфейс IEnumerator<T> для заданого об'єкта.|
|IEnumerator\<T\>|Дозволяє ітерацію в стилі foreach для загальної колекції.|
|IList\<T\>|Забезпечує поведінку для додавання, видалення та індексування елементів у послідовному списку об'єктів.|
|ISet\<T\>|Надає базовий інтерфейс для абстракції множин.|

Простір імен System.Collections.Generic також визначає кілька класів, які реалізують багато з цих ключових інтерфейсів. У таблиці описано деякі часто використовувані класи цього простору імен, інтерфейси, які вони реалізують, та їхню основну функціональність.

Класи System.Collections.Generic

|Загальний клас|Підтримувані ключові інтерфейси|Сенс у використані|
|--------------|-------------------------------|------------------|
|Dictionary\<TKey, TValue\>|ICollection\<T\>, IDictionary\<TKey, TValue\>, IEnumerable\<T\>|Це являє собою загальну колекцію ключів та значень.|
|LinkedList\<T\>|ICollection\<T\>, IEnumerable\<T\>|Це являє собою подвійно зв'язаний список.|
|List\<T\>|ICollection\<T\>, IEnumerable\<T\>, IList\<T\>|Це послідовний список елементів, розмір якого можна динамічно змінювати.|
|Queue\<T\>|ICollection, IEnumerable\<T\>|Це загальна реалізація списку «хто перший прийшов, той перший пішов».|
|SortedDictionary\<TKey, TValue\>|ICollection\<T\>, IDictionary\<TKey, TValue\>, IEnumerable\<T\>|Це загальна реалізація відсортованого набору пар ключ-значення.|
|SortedSet\<T\>|ICollection\<T\>, IEnumerable\<T\>, ISet\<T\>|Це являє собою колекцію об'єктів, яка зберігається в відсортованому порядку без дублювання.|
|Stack\<T\>|ICollection, IEnumerable\<T\>|Це загальна реалізація списку «останнім прийшов, першим пішов».|

Простір імен System.Collections.Generic також визначає багато допоміжних класів та структур, які працюють разом із певним контейнером. Наприклад, тип LinkedListNode<T> представляє вузол у загальному об'єкті LinkedList<T>, виняток KeyNotFoundException виникає під час спроби отримати елемент з контейнера за допомогою неіснуючого ключа тощо. Обов’язково ознайомтеся з документацією .NET для отримання повної інформації про простір імен System.Collections.Generic.
У будь-якому разі, вашим наступним завданням буде навчитися використовувати деякі з цих універсальних класів колекцій. Однак, перш ніж ви це зробите, дозвольте мені проілюструвати функцію мови C#, яка спрощує спосіб заповнення універсальних (і неузагальнених) контейнерів колекцій даними.

## Розуміння синтаксису ініціалізації колекції

Ви знаете про синтаксис ініціалізації об'єктів, який дозволяє встановлювати властивості нової змінної під час створення. Тісно пов'язаний із цим синтаксис ініціалізації колекції. Ця функція мови C# дозволяє заповнювати багато контейнерів (таких як ArrayList або List<T>) елементами, використовуючи синтаксис, подібний до того, що використовується для заповнення базового масиву. 

    Синтаксис ініціалізації колекції можна застосовувати лише до класів, що підтримують метод Add(), формалізований інтерфейсами ICollection<T>/ICollection.


Створіть нову консольну програму .NET з назвою CollectionInitialization. Очистіть згенерований код у Program.cs та додайте наступне:

```cs
void InitializingASimpleValueType()
{
    // Init a standard array.
    int[] myArrayOfInts = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    // Init a generic List<> of ints.
    List<int> myGenericList = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    // Init an ArrayList with numerical data.
    ArrayList myList = new ArrayList { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    foreach (var item in myGenericList)
    {
        Console.Write($"{item} ");
    }
}
InitializingASimpleValueType();
```
```
0 1 2 3 4 5 6 7 8 9
```
Якщо ваш контейнер керує колекцією класів або структурою, ви можете поєднати синтаксис ініціалізації об'єктів із синтаксисом ініціалізації колекцій, щоб отримати певний функціональний код. В просторі імен System.Drawing є клас Point, який визначєв дві властивості з назвами X та Y. Якщо ви хочете створити узагальнений List\<T\> об'єктів Point, ви можете написати наступне:

```cs
void InitializingAClasesAndStructureType()
{
    List<Point> myListOfPoints = new List<Point>
    {
      new Point { X = 2, Y = 2 },
      new Point { X = 3, Y = 3 },
      new Point { X = 4, Y = 4 }
    };

    foreach (var pt in myListOfPoints)
    {
        Console.WriteLine(pt);
    }
}
InitializingAClasesAndStructureType();
```
```
{X=2,Y=2}
{X=3,Y=3}
{X=4,Y=4}
```
Знову ж таки, перевага цього синтаксису полягає в тому, що ви економите численні натискання клавіш. Хоча вкладені фігурні дужки можуть бути важкими для читання, якщо ви не зважаєте на форматування, уявіть собі обсяг коду, який знадобився б для заповнення наступного списку List<T> прямокутників, якби у вас не було синтаксису ініціалізації колекції.

```cs
void InitializingListOfRectangles()
{
    List<Rectangle> myListOfRects = new List<Rectangle>
{
  new Rectangle {
    Height = 90, Width = 90,
    Location = new Point { X = 10, Y = 10 }},
  new Rectangle {
    Height = 50,Width = 50,
    Location = new Point { X = 2, Y = 2 }},
};
    foreach (var r in myListOfRects)
    {
        Console.WriteLine(r);
    }
}
InitializingListOfRectangles();
```
```
{X=10,Y=10,Width=90,Height=90}
{X=2,Y=2,Width=50,Height=50}
```

## Робота з класом List<T>

Створіть новий проект консольної програми з назвою GenericCollections. Додайте новий файл з назвою Person.cs та додайте до нього наступний код:

```cs
namespace GenericCollections;
public class Person
{
    public int Age { get; set; }
    public string FirstName { get; set; } = "Undefined";
    public string LastName { get; set; } = "Undefined";

    public Person() { }
    public Person(string firstName, string lastName, int age)
    {
        Age = age;
        FirstName = firstName;
        LastName = lastName;
    }

    public override string ToString()
    {
        return $"Name: {FirstName} {LastName}, Age: {Age}";
    }
}
```
Першим узагальненим класом, який ви розглянете, є List\<T\>, з яким ви вже не раз зустрічалися. Клас List\<T\>, безсумнівно, буде вашим найчастіше використовуваним типом у просторі імен System.Collections.Generic, оскільки він дозволяє динамічно змінювати розмір вмісту контейнера. Щоб проілюструвати основи цього типу, розглянемо наступний метод у вашому файлі Program.cs, який використовує List\<T\> для маніпулювання набором об'єктів Person, показаних раніше в цьому розділі; ви можете пам'ятати, що ці об'єкти Person визначали три властивості (Age, FirstName та LastName) та власну реалізацію ToString():

```cs
using GenericCollections;

static void UseGenericList()
{
    // Make a List of Person objects, filled with
    // collection/object init syntax.
    List<Person> people = new List<Person>()
    {
        new Person {FirstName= "Homer", LastName="Simpson", Age=47},
        new Person {FirstName= "Marge", LastName="Simpson", Age=45},
        new Person {FirstName= "Lisa", LastName="Simpson", Age=9},
        new Person {FirstName= "Bart", LastName="Simpson", Age=8}
    };

    // Print out # of items in List.
    Console.WriteLine($"Items in list: {people.Count}");
    
    // Enumerate over list.
    foreach (Person p in people)
    {
        Console.WriteLine(p);
    }
    
    // Insert a new person.
    Console.WriteLine("\tInserting new person.");
    people.Insert(2, new Person { FirstName = "Maggie", LastName = "Simpson", Age = 2 });
    Console.WriteLine($"Items in list: {people.Count}");

    // Copy data into a new array.
    Person[] arrayOfPeople = people.ToArray();
    foreach (Person p in arrayOfPeople)
    {
        Console.WriteLine($"First Names: {p.FirstName}");
    }
}
UseGenericList();
```
```
Items in list: 4
Name: Homer Simpson, Age: 47
Name: Marge Simpson, Age: 45
Name: Lisa Simpson, Age: 9
Name: Bart Simpson, Age: 8
        Inserting new person.
Items in list: 5
First Names: Homer
First Names: Marge
First Names: Maggie
First Names: Lisa
First Names: Bart
```
Тут ви використовуєте синтаксис ініціалізації колекції для заповнення вашого List\<T\> об'єктами, як скорочений запис для багаторазового виклику Add(). Після виведення кількості елементів у колекції (а також перерахування кожного елемента) викликається Insert(). Як бачите, Insert() дозволяє додавати новий елемент до List\<T\> за вказаним індексом. 
Зрештою, зверніть увагу на виклик методу ToArray(), який повертає масив об'єктів Person на основі вмісту вихідного List\<T\>. З цього масиву ви знову перебираєте елементи, використовуючи синтаксис індексатора масиву. Клас List\<T\> визначає багато додаткових членів, що цікавлять, тому обов’язково ознайомтеся з документацією для отримання додаткової інформації.
Далі розглянемо кілька більш універсальних колекцій, зокрема Stack\<T\>, Queue\<T\> та SortedSet\<T\>. Це має допомогти вам зрозуміти основні варіанти зберігання даних вашої користувацької програми.

## Робота з класом Stack\<T\>

Клас Stack\<T\> представляє колекцію, яка зберігає елементи за принципом «останнім прийшов — першим вийшов». Як і слід було очікувати, Stack<T> визначає члени з іменами Push() та Pop() для розміщення елементів у стеку або видалення елементів зі стеку. Наступний метод створює стек об'єктів Person:

```cs
static void UseGenericStack()
{
    Stack<Person> stackOfPeople = new Stack<Person>();
    stackOfPeople.Push(new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 });
    stackOfPeople.Push(new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 });
    stackOfPeople.Push(new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 });

    foreach (var p in stackOfPeople)
    {
        Console.WriteLine(p);
    }
    Console.WriteLine();

    // Now look at the top item, pop it, and look again.
    Console.WriteLine($"\nFirst person is: {stackOfPeople.Peek()}");
    Console.WriteLine($"Popped off {stackOfPeople.Pop()}");
    Console.WriteLine($"\nFirst person is: {stackOfPeople.Peek()}");
    Console.WriteLine($"Popped off {stackOfPeople.Pop()}");
    Console.WriteLine($"\nFirst person is: {stackOfPeople.Peek()}");
    Console.WriteLine($"Popped off {stackOfPeople.Pop()}");

    try
    {
        Console.WriteLine($"\nFirst person is: {stackOfPeople.Peek()}");
        Console.WriteLine($"Popped off {stackOfPeople.Pop()}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine("\nError! {0}", ex.Message);
    }
}
UseGenericStack();
```
```
Name: Lisa Simpson, Age: 9
Name: Marge Simpson, Age: 45
Name: Homer Simpson, Age: 47


First person is: Name: Lisa Simpson, Age: 9
Popped off Name: Lisa Simpson, Age: 9

First person is: Name: Marge Simpson, Age: 45
Popped off Name: Marge Simpson, Age: 45

First person is: Name: Homer Simpson, Age: 47
Popped off Name: Homer Simpson, Age: 47

Error! Stack empty.
```
Тут ви створюєте стек, який містить трьох людей, доданих у порядку їхніх імен: Гомер, Мардж та Ліза. Коли ви переглядаєте стек, ви завжди бачите спочатку об'єкт зверху; тому перший виклик Peek() показує об'єкт третьої особи. Після серії викликів Pop() та Peek() стек зрештою спорожняється, після чого додаткові виклики Peek() та Pop() викликають системний виняток.

## Робота з класом Queue\<T\>

Черги – це контейнери, які забезпечують доступ до елементів у порядку «хто перший прийшов, той перший вийшов». Коли вам потрібно змоделювати сценарій, у якому елементи обробляються в порядку черги, ви виявите, що клас Queue\<T\> відповідає всім вимогам. Окрім функціональності, що надається підтримуваними інтерфейсами, Queue визначає ключові елементи, показані в таблиці.

Члени типу Queue\<T\>

|Члени вибирання в Queue\<T\>|Сенс у використані|
|----------------------------|------------------|
|Dequeue()|Видаляє та повертає об'єкт на початку|
|Enqueue()|Додає об'єкт у кінець|
|Peek()|Повертає об'єкт на початку без його видалення|

Тепер давайте застосуємо ці методи. Ви можете почати з повторного використання класу Person та створення об'єкта Queue\<T\>, який імітує чергу людей, що чекають на замовлення кави.

```cs
static void UseGenericQueue()
{
    // Make a Queue with three people.
    Queue<Person> peopleQ = new();
    peopleQ.Enqueue(new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 });
    peopleQ.Enqueue(new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 });
    peopleQ.Enqueue(new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 });

    foreach (var p in peopleQ)
    {
        Console.WriteLine(p);
    }
    Console.WriteLine();

    // Peek at first person in Q.
    Console.WriteLine($"{peopleQ.Peek().FirstName} is first in line!");

    // Remove each person from Q.
    GetCoffee(peopleQ.Dequeue());
    GetCoffee(peopleQ.Dequeue());
    GetCoffee(peopleQ.Dequeue());
    // Try to de-Q again?
    try
    {
        GetCoffee(peopleQ.Dequeue());
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine("\nError! {0}", e.Message);
    }

    //Local helper function
    static void GetCoffee(Person p)
    {
        Console.WriteLine($"\n{p.FirstName} got coffee!");
    }
}
UseGenericQueue();

```
```
Name: Homer Simpson, Age: 47
Name: Marge Simpson, Age: 45
Name: Lisa Simpson, Age: 9

Homer is first in line!

Homer got coffee!

Marge got coffee!

Lisa got coffee!

Error! Queue empty.
```
Тут ви вставляєте три елементи в клас Queue\<T\> за допомогою його методу Enqueue(). Виклик Peek() дозволяє переглянути (але не видалити) перший елемент, який наразі знаходиться в черзі. Зрештою, виклик Dequeue() видаляє елемент з рядка та надсилає його до допоміжної функції GetCoffee() для обробки. Зверніть увагу, що якщо ви спробуєте видалити елементи з порожньої черги, буде викинуто виняток під час виконання.

## Робота з класом PriorityQueue\<TElement, TPriority\>

PriorityQueue працює так само, як Queue<T>, за винятком того, що кожному елементу в черзі надається пріоритет.
Коли елементи вилучаються з черги, вони видаляються від найнижчого до найвищого пріоритету. Наведений нижче код оновлює попередній приклад черги для використання PriorityQueue:

```cs
static void UsePriorityQueue()
{
    PriorityQueue<Person, int> peopleQ = new();
    peopleQ.Enqueue(new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 }, 3);
    peopleQ.Enqueue(new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 }, 1);
    peopleQ.Enqueue(new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 }, 3);
    peopleQ.Enqueue(new Person { FirstName = "Bart", LastName = "Simpson", Age = 12 }, 2);

    while (peopleQ.Count > 0)
    {
        Console.WriteLine(peopleQ.Dequeue().FirstName); 
        Console.WriteLine(peopleQ.Dequeue().FirstName); 
        Console.WriteLine(peopleQ.Dequeue().FirstName); 
        Console.WriteLine(peopleQ.Dequeue().FirstName); 
    }
}
UsePriorityQueue();
```
```
Homer
Bart
Marge
Lisa
```
Якщо для кількох елементів встановлено найнижчий поточний пріоритет, порядок вилучення з черги не гарантується. Як показано у прикладі коду, третій виклик Dequeue() поверне або Lisa, або Marge, оскільки для обох встановлено пріоритет три. Якщо точний порядок має значення, ви повинні переконатися, що значення для кожного пріоритету унікальні.

## Робота з класом SortedSet<T>

Клас SortedSet\<T\> корисний, оскільки він автоматично забезпечує сортування елементів у наборі під час вставки або видалення елементів. Однак, вам потрібно точно повідомити клас SortedSet\<T\>, як саме ви хочете сортувати об'єкти, передавши як аргумент конструктора об'єкт, який реалізує загальний інтерфейс IComparer\<T\>.
Почніть зі створення нового класу з назвою PeopleByAgeComparer, який реалізує IComparer\<T\>, де T має тип Person.
Нагадаємо, що цей інтерфейс визначає один метод з назвою Compare(), де ви можете створити будь-яку логіку, необхідну для порівняння. Ось проста реалізація цього класу:

```cs
class PeopleByAgeComparer : IComparer<Person>
{
    public int Compare(Person? first, Person? second)
    {
        if (first!= null && second != null)
        {
            return first.Age.CompareTo(second.Age);
        }
        return 0;
    }
}
```
Тепер додайте наступний новий метод, який демонструє використання SortedSet\<Person\>:

```cs
static void UseSortedSet()
{
    // Make some people with different ages.
    SortedSet<Person> sortSetOfPeople = new SortedSet<Person>(new PeopleByAgeComparer())
    {
        new Person {FirstName= "Homer", LastName="Simpson", Age=47},
        new Person {FirstName= "Marge", LastName="Simpson", Age=45},
        new Person {FirstName= "Lisa", LastName="Simpson", Age=9},
        new Person {FirstName= "Bart", LastName="Simpson", Age=8}
    };

    // Note the items are sorted by age!
    foreach (var p in sortSetOfPeople)
    {
        Console.WriteLine(p);
    }
    Console.WriteLine();

    // Add a few new people, with various ages.
    sortSetOfPeople.Add(new Person { FirstName = "Saku", LastName = "Jones", Age = 1 });
    sortSetOfPeople.Add(new Person { FirstName = "Mikko", LastName = "Jones", Age = 32 });

    foreach (var p in sortSetOfPeople)
    {
        Console.WriteLine(p);
    }
    Console.WriteLine();
}
UseSortedSet();
```
```
Name: Bart Simpson, Age: 8
Name: Lisa Simpson, Age: 9
Name: Marge Simpson, Age: 45
Name: Homer Simpson, Age: 47

Name: Saku Jones, Age: 1
Name: Bart Simpson, Age: 8
Name: Lisa Simpson, Age: 9
Name: Mikko Jones, Age: 32
Name: Marge Simpson, Age: 45
Name: Homer Simpson, Age: 47
```
Список об'єктів тепер завжди впорядковується на основі значення властивості Age, незалежно від порядку вставки або видалення об'єктів.

## Робота з класом Dictionary\<TKey, TValue\>

Ще однією зручною узагальненою колекцією є тип Dictionary\<TKey,TValue\>, який дозволяє зберігати будь-яку кількість об'єктів, на які можна посилатися за допомогою унікального ключа. Таким чином, замість отримання елемента зі List<T> за допомогою числового ідентифікатора (наприклад, «Дайте мені другий об'єкт»), ви можете використовувати унікальний текстовий ключ (наприклад, «Дайте мені об'єкт, який я ввів як Homer»).
Як і інші об'єкти колекції, ви можете заповнити Dictionary<TKey,TValue>, викликавши загальний метод Add() вручну. Однак, ви також можете заповнити Dictionary<TKey,TValue>, використовуючи синтаксис ініціалізації колекції. Майте на увазі, що під час заповнення цього об'єкта колекції імена ключів мають бути унікальними. Якщо ви помилково вкажете один і той самий ключ кілька разів, ви отримаєте виняток під час виконання.
Розглянемо наступний метод, який заповнює Dictionary<K,V> різними об'єктами. Зверніть увагу, що під час створення об'єкта Dictionary<TKey,TValue> ви вказуєте тип ключа (TKey) та тип базового об'єкта (TValue) як аргументи конструктора. У цьому прикладі ви використовуєте рядковий тип даних як ключ і тип Person як значення. Також зверніть увагу, що ви можете поєднувати синтаксис ініціалізації об'єктів із синтаксисом ініціалізації колекцій.

```cs
static void UseDictionary()
{
    // Populate using Add() method
    Dictionary<string, Person> peopleA = new Dictionary<string, Person>();
    peopleA.Add("Homer", new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 });
    peopleA.Add("Marge", new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 });
    peopleA.Add("Lisa", new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 });

    // Get Homer.
    Person homer = peopleA["Homer"];
    Console.WriteLine(homer);

    // Populate with initialization syntax.
    Dictionary<string, Person> peopleB = new Dictionary<string, Person>()
    {
        { "Homer", new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 } },
        { "Marge", new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 } },
        { "Lisa", new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 } }
    };
    // Get Lisa.
    Person lisa = peopleB["Lisa"];
    Console.WriteLine(lisa);
}
UseDictionary();
```
```
Name: Homer Simpson, Age: 47
Name: Lisa Simpson, Age: 9
```
Також можливо заповнити Dictionary<TKey,TValue>, використовуючи пов'язаний синтаксис ініціалізації, специфічний для цього типу контейнера (що не дивно називається ініціалізацією словника). Подібно до синтаксису, який використовувався для заповнення об'єкта personB у попередньому прикладі коду, ви все ще визначаєте область ініціалізації для об'єкта колекції; однак ви можете використовувати індексатор, щоб вказати ключ і призначити його новому об'єкту наступним чином:

```cs
    // Populate with dictionary initialization syntax.
    Dictionary<string, Person> peopleC = new Dictionary<string, Person>()
    {
        ["Homer"] = new Person { FirstName = "Homer", LastName = "Simpson", Age = 47 },
        ["Marge"] = new Person { FirstName = "Marge", LastName = "Simpson", Age = 45 },
        ["Lisa"] = new Person { FirstName = "Lisa", LastName = "Simpson", Age = 9 }
    };
```

# Простір імен System.Collections.ObjectModel

Тепер, коли ви розумієте, як працювати з основними узагальненими класами, ми коротко розглянемо додатковий простір імен, орієнтований на колекції, System.Collections.ObjectModel. Це відносно невеликий простір імен, який містить кілька класів. У таблиці задокументовано два класи, про які вам неодмінно слід знати.

Корисні члени System.Collections.ObjectModel

|Тип|Сенс у використанні|
|---|-------------------|
|ObservableCollection<T>|Представляє динамічний збір даних, який надає сповіщення про додавання елементів, видалення елементів або оновлення всього списку.|
|ReadOnlyObservableCollection<T>|Представляє версію ObservableCollection<T> лише для читання|

Клас ObservableCollection<T> корисний тим, що він має можливість інформувати зовнішні об'єкти про зміни його вмісту (як ви можете здогадатися, робота з ReadOnlyObservableCollection<T> подібна, але за своєю природою доступна лише для читання).

## Робота з ObservableCollection<T>
Створіть новий проект консольної програми з назвою ObservableCollections та імпортуйте простір імен System.Collections.ObjectModel у ваш початковий файл коду C#. Багато в чому робота з ObservableCollection\<T\> ідентична роботі з List\<T\>, враховуючи, що обидва ці класи реалізують однакові основні інтерфейси. Унікальність класу ObservableCollection<T> полягає в тому, що він підтримує подію з назвою CollectionChanged.
Ця подія спрацьовуватиме щоразу, коли буде вставлено новий елемент, поточний елемент буде видалено (або переміщено), або ж буде змінено всю колекцію.
Як і будь-яка подія, CollectionChanged визначається через делегат, яким у цьому випадку є NotifyCollectionChangedEventHandler. Цей делегат може викликати будь-який метод, який приймає об'єкт як перший параметр, а NotifyCollectionChangedEventArgs як другий. Розглянемо наступний код, який заповнює спостережувану колекцію, що містить об'єкти Person, та ініціює подію CollectionChanged:

```cs
using ObservableCollections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

void WorkWithObservableCollection()
{
    // Make a collection to observe and add a few Person objects.
    ObservableCollection<Person> people = new ObservableCollection<Person>()
    {
        new Person {FirstName = "Peter", LastName = "Murphy", Age = 52},
        new Person {FirstName = "Kevin", LastName = "Key", Age = 48},
    };

    // Wire up the CollectionChanged event.
    people.CollectionChanged += people_CollectionChanged;

}
WorkWithObservableCollection();
```

Вхідний параметр NotifyCollectionChangedEventArgs визначає дві важливі властивості, OldItems та NewItems, які нададуть вам список елементів, що були в колекції до спрацьовування події, та нових елементів, які були задіяні у зміні. Однак, вам слід перевіряти ці списки лише за певних обставин. Пам’ятайте, що подія CollectionChanged може спрацьовувати, коли елементи додаються, видаляються, переміщуються або скидаються. Щоб дізнатися, яка з цих дій спричинила подію, можна скористатися властивістю Action класу NotifyCollectionChangedEventArgs. Властивість Action можна перевірити для будь-якого з наступних членів переліку NotifyCollectionChangedAction:

```cs
public enum NotifyCollectionChangedAction
{
  Add = 0,
  Remove = 1,
  Replace = 2,
  Move = 3,
  Reset = 4,
}
```
Ось реалізація обробника події CollectionChanged, який перебиратиме старі та нові набори, коли елемент буде вставлено в колекцію або видалено з неї:

```cs
void people_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
{
    // What was the action that caused the event?
    Console.WriteLine($"Action for this event: {e.Action}");

    // They removed something.
    if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
    {
        Console.WriteLine("Here are the OLD items:");
        foreach (Person p in e.OldItems)
        {
            Console.WriteLine(p.ToString());
        }
        Console.WriteLine();
    }

    // They added something.
    if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
    {
        // Now show the NEW items that were inserted.
        Console.WriteLine("Here are the NEW items:");
        foreach (Person p in e.NewItems)
        {
            Console.WriteLine(p.ToString());
        }
    }
}
```
Тепер оновіть свій код виклику, щоб додавати та видаляти елемент.

```cs
void WorkWithObservableCollection()
{
    //...

    ShowCollection();

    // Now add a new item.
    people.Add(new Person("Fred", "Smith", 32));
    Console.WriteLine();

    ShowCollection();

    // Remove an item.
    people.RemoveAt(0);
    Console.WriteLine();

    ShowCollection();

    void ShowCollection()
    {
        // All Collection
        foreach (var p in people)
        {
            Console.WriteLine($"\t{p.ToString()}");
        }
        Console.WriteLine();
    }
}
WorkWithObservableCollection();
```
```
        Name: Peter Murphy, Age: 52
        Name: Kevin Key, Age: 48

Action for this event: Add
Here are the NEW items:
Name: Fred Smith, Age: 32

        Name: Peter Murphy, Age: 52
        Name: Kevin Key, Age: 48
        Name: Fred Smith, Age: 32

Action for this event: Remove
Here are the OLD items:
Name: Peter Murphy, Age: 52


        Name: Kevin Key, Age: 48
        Name: Fred Smith, Age: 32
```
На цьому огляд різних просторів імен, орієнтованих на колекції, завершено. На завершення розділу ви тепер розглянете, як можна створювати власні узагальнені методи та узагальнені типи.

# Створення власних узагальнених методів

Хоча більшість розробників зазвичай використовують існуючі узагальнені типи в бібліотеках базових класів, також можливо створювати власні узагальнені члени та власні узагальнені типи. Давайте розглянемо, як інтегрувати власні узагальнювачі у ваші власні проекти. Перший крок – створити універсальний метод swap. Почніть зі створення нової консольної програми з назвою CustomGenericMethods.
Коли ви створюєте власні узагальнені методи, ви отримуєте надпотужну версію традиційного перевантаження методів. Ви знаєте, що перевантаження – це акт визначення кількох версій одного методу, які відрізняються кількістю або типом параметрів. 
Хоча перевантаження є корисною функцією в об'єктно-орієнтованій мові програмування, одна з проблем полягає в тому, що ви можете легко отримати безліч методів, які по суті роблять одне й те саме. Наприклад, припустимо, що вам потрібно створити методи, які можуть перемикати між двома частинами даних за допомогою простої процедури swap. Ви можете почати зі створення нового статичного класу з методом, який може оперувати цілими числами, ось так:

```cs
    // Swap two integers.
    internal static void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }
```
Поки що все добре. Але тепер припустимо, що вам також потрібно поміняти місцями два об'єкти Person; це вимагатиме створення нової версії Swap().

```cs
    // Swap two Person objects.
    internal static void Swap(ref Person a, ref Person b)
    {
        Person temp = a;
        a = b;
        b = temp;
    }
```
Якби вам також потрібно було поміняти місцями числа з плаваючою комою, растрові зображення, автомобілі, кнопки тощо, вам довелося б створювати ще більше методів, що перетворилося б на кошмар для обслуговування.
Ви могли б створити єдиний (неузагальнений) метод, який би оперував з параметрами об'єкта, але тоді ви зіткнетеся з усіма проблемами, які ви розглядали раніше в цьому розділі, включаючи упаковку, розпакування, відсутність безпеки типів, явне приведення типів тощо.
Щоразу, коли у вас є група перевантажених методів, які відрізняються лише вхідними аргументами, це ваша підказка про те, що узагальнені методи можуть спростити ваше життя. Розглянемо наступний узагальнений метод Swap\<T\>(), який може поміняти місцями будь-які два об'єкти типу T:

```cs
    internal static void Swap<T>(ref T a, ref T b)
    {
        Console.WriteLine($"You sent the Swap() method a {typeof(T)}");
        T temp = a;
        a = b;
        b = temp;
    }
```
Зверніть увагу, як узагальнений метод визначається шляхом визначення параметрів типу після назви методу, але перед списком параметрів. Тут ви стверджуєте, що метод Swap\<T\>() може працювати з будь-якими двома параметрами типу \<T\>. Щоб трохи оживити ситуацію, ви також виводите назву типу наданого заповнювача в консоль за допомогою оператора typeof() у C#. Тепер розглянемо наступний код виклику, який міняє місцями цілі числа та рядки:

```cs
void SwapingIntAndString()
{
    // Swap 2 ints.
    int a = 10, b = 90;
    Console.WriteLine($"Before swap: {a} {b}");
    SwapFunctions.Swap<int>(ref a, ref b);
    Console.WriteLine($"After swap: {a} {b}");
    Console.WriteLine();

    // Swap 2 strings.
    string s1 = "Hello", s2 = "There";
    Console.WriteLine($"Before swap: {s1} {s2}");
    SwapFunctions.Swap<string>(ref s1, ref s2);
    Console.WriteLine($"After swap: {s1} {s2}");
    Console.WriteLine();
}
SwapingIntAndString();
```
```
Before swap: 10 90
You sent the Swap() method a System.Int32
After swap: 90 10

Before swap: Hello There
You sent the Swap() method a System.String
After swap: There Hello
```
Головна перевага цього підходу полягає в тому, що у вас є лише одна версія Swap<T>() для підтримки, проте вона може працювати з будь-якими двома елементами заданого типу безпечним для типів способом. Ще краще, якщо елементи на основі стеку залишаються в стеку, а елементи на основі купи залишаються в купі.

## Визначення компілятором параметрів типу

Під час виклику універсальних методів, таких як Swap<T>, можна за бажанням пропустити параметр типу, якщо (і тільки якщо) універсальний метод вимагає аргументів, оскільки компілятор може визначити параметр типу на основі параметрів членів. Наприклад, ви можете поміняти місцями два значення System.Boolean, додавши наступний код:

```cs
void SwapingBoolean()
{
    // Compiler will infer System.Boolean.
    bool b1 = true, b2 = false;
    Console.WriteLine($"Before swap: {b1}, {b2}");
    SwapFunctions.Swap(ref b1, ref b2);
    Console.WriteLine($"After swap: {b1}, {b2}");
    Console.WriteLine();
}
SwapingBoolean();
```
```
Before swap: True, False
You sent the Swap() method a System.Boolean
After swap: False, True
```
Навіть якщо компілятор може визначити правильний параметр типу на основі типу даних, що використовується для оголошення b1 та b2, вам слід виробити звичку завжди явно вказувати параметр типу.

```cs
SwapFunctions.Swap<bool>(ref b1, ref b2);
```
Це дає зрозуміти вашим колегам-програмістам, що цей метод справді є узагальненим. Більше того, висновок параметрів типу працює лише тоді, коли універсальний метод має хоча б один параметр. Наприклад, припустимо, що у вашому файлі Program.cs є наступний універсальний метод:

```cs
static void DisplayBaseClass<T>()
{
    // BaseType is a method used in reflection,
    // which will be examined in Chapter 17
    Console.WriteLine($"Base class of {typeof(T)} is: {typeof(T).BaseType}.");
}
```

У цьому випадку ви повинні надати параметр типу під час виклику.

```cs
// Must supply type parameter if
// the method does not take params.
DisplayBaseClass<int>();
DisplayBaseClass<string>();
// Compiler error! No params? Must supply placeholder!
//DisplayBaseClass();
```
```
Base class of System.Int32 is: System.ValueType.
Base class of System.String is: System.Object.
```

Звичайно, узагальнені методи не обов'язково повинні бути статичними, як у цих прикладах. Також застосовуються всі правила та опції для неузагальнених методів.

# Створення власних узагальнених структур та класів

Тепер, коли ви розумієте, як визначати та викликати узагальнені методи, час звернути вашу увагу на побудову узагальненої структури (процес побудови узагальненого класу ідентичний) у новому проекті консольної програми з назвою GenericPoint.
Припустимо, ви створили загальну структуру Point, яка підтримує один параметр типу, що представляє базовий тип зберігання для координат (x, y). Викликаюча сторона може потім створювати типи Point<T> наступним чином:

```cs
// Point using ints.
Point<int> p = new Point<int>(10, 10);
// Point using double.
Point<double> p2 = new Point<double>(5.4, 3.3);
// Point using strings.
Point<string> p3 = new Point<string>("","3");
```
Створення точки за допомогою рядків може здатися трохи дивним спочатку, але розглянемо випадок уявних чисел. Тоді може бути сенс використовувати рядки для значень X та Y точки. У будь-якому разі, це демонструє силу узагальнювачів. Ось повне визначення Point<T>:

```cs
namespace GenericPoint;
// A generic Point structure.
public struct Point<T>
{
    // Generic state date.
    private T _xPos;
    private T _yPos;

    // Generic constructor.
    public Point(T xVal, T yVal)
    {
        _xPos = xVal;
        _yPos = yVal;
    }

    // Generic properties.
    public T X
    {
        get => _xPos;
        set => _xPos = value;
    }
    public T Y
    {
        get => _yPos;
        set => _yPos = value;
    }
    public override string ToString() => $"[{_xPos}, {_yPos}]";
}

```
Як бачите, Point<T> використовує свій параметр типу у визначенні даних поля, аргументів конструктора та визначенні властивостей.

## Вирази значень за замовчуванням з узагальненнями

З появою узагальнень ключове слово C# default отримало подвійну ідентичність. Окрім використання в конструкції switch, його можна використовувати для встановлення значення за замовчуванням для параметра типу. Це корисно, оскільки узагальнений тип заздалегідь не знає фактичних заповнювачів, а це означає, що він не може безпечно припустити, яким буде значення за замовчуванням. Значення за замовчуванням для параметра типу такі:

    Числові значення мають значення за замовчуванням 0.

    Типи посилань мають значення за замовчуванням null.

    Поля структури встановлюються на 0 (для типів значень) або null (для типів посилань).

Щоб скинути значення екземпляра Point<T>, можна безпосередньо встановити значення X та Y на 0. Це передбачає, що викликаюча сторона надаватиме лише числові дані. А як щодо рядкової версії? Саме тут стане в нагоді синтаксис default(T). Ключове слово default скидає значення змінної до значення за замовчуванням для типу даних змінної. Додайте метод під назвою ResetPoint() наступним чином:

```cs
    // The "default" keyword is overloaded in C#.
    // When used with generics, it represents the default
    // value of a type parameter.
    public void ResetPoint()
    {
        _xPos = default(T);
        _yPos = default(T);
    }
```
Тепер, коли у вас є метод ResetPoint(), ви можете повноцінно застосувати методи структури Point\<T\>.

```cs
using GenericPoint;

void UsePoint()
{
    // Point using ints.
    Point<int> p = new Point<int>(10, 10);
    Console.WriteLine($"p.ToString()={p}");
    p.ResetPoint();
    Console.WriteLine($"p.ToString()={p}");
    Console.WriteLine();

    // Point using double.
    Point<double> p2 = new Point<double>(5.4, 3.3);
    Console.WriteLine($"p2.ToString()={p2}");
    p2.ResetPoint();
    Console.WriteLine($"p2.ToString()={p2}");
    Console.WriteLine();

    // Point using strings.
    Point<string> p3 = new Point<string>("i", "3i");
    Console.WriteLine($"p3.ToString()={p3}");
    p3.ResetPoint();
    Console.WriteLine($"p3.ToString()={p3}");
    Console.WriteLine();

}
UsePoint();
```
```
p.ToString()=[10, 10]
p.ToString()=[0, 0]

p2.ToString()=[5,4, 3,3]
p2.ToString()=[0, 0]

p3.ToString()=[i, 3i]
p3.ToString()=[, ]
```
## Літеральні вирази за замовчуванням

Окрім встановлення значення властивості за замовчуванням, у C# є літеральні вирази за замовчуванням. Це усуває необхідність вказувати тип змінної в операторі за замовчуванням. Оновіть метод ResetPoint() до наступного:

```cs
public void ResetPoint()
{
  _xPos = default;
  _yPos = default;
}
```
Вираз за замовчуванням не обмежується простими змінними, а також може застосовуватися до складних типів. Наприклад, щоб створити та ініціалізувати структуру Point, можна написати наступне:

```cs
void UseDefault()
{
    Point<string> p4 = default;
    Console.WriteLine($"p4.ToString()={p4}");
    Console.WriteLine();
    Point<int> p5 = default;
    Console.WriteLine($"p5.ToString()={p5}");
}
UseDefault();
```
```
p4.ToString()=[, ]

p5.ToString()=[0, 0]
```
## Зіставлення зі зразком за допомогою узагальнених типів

Існує можливість зіставлення зі зразком для узагальнених типів. Візьмемо наступний метод, який перевіряє екземпляр Point на наявність типу даних, на якому він базується (можливо, неповний, але достатній для демонстрації концепції):

```cs
static void PatternMatching<T>(Point<T> p)
{
    switch (p)
    {
        case Point<string> pString:
            Console.WriteLine("Point is based on strings");
            return;
        case Point<int> pInt:
            Console.WriteLine("Point is based on ints");
            return;
    }
}
```
Спробуємо метод:

```cs
void UsePatternMatching()
{
    Point<string> p1 = default;
    Point<int> p2 = default;
    PatternMatching(p1);
    PatternMatching(p2);
}
UsePatternMatching();
```
```
Point is based on strings
Point is based on ints
```

# Обмеження параметрів типу

Як показано раніще, будь-який універсальний елемент має принаймні один параметр типу, який потрібно вказати під час взаємодії з універсальним типом або членом. Це саме по собі дозволяє вам створювати код, безпечний для типів; однак, ви також можете використовувати ключове слово where, щоб отримати надзвичайно точний опис того, як має виглядати заданий параметр типу. Використовуючи це ключове слово, ви можете додати набір обмежень до заданого параметра типу, які компілятор C# перевірятиме під час компіляції. Зокрема, ви можете обмежити параметр типу, як описано в таблиці.

|Обмеження|Сенс у використанні|
|---------|-------------------|
|where T : struct|Параметр типу \<T\> повинен мати System.ValueType у своєму ланцюжку успадкування (тобто \<T\> має бути структурою).|
|where T : class|Параметр типу \<T\> не повинен мати System.ValueType у своєму ланцюжку успадкування (тобто \<T\> має бути посилальною типом).|
|where T : new()|Параметр типу \<T\> повинен мати конструктор за замовчуванням. Це корисно, якщо ваш універсальний тип має створювати екземпляр параметра типу, оскільки ви не можете припускати, що знаєте формат користувацьких конструкторів. Зверніть увагу, що це обмеження має бути останнім у списку багатообмеженого типу.|
|where T : NameOfBaseClass|Параметр типу \<T\> має бути похідним від класу, визначеного NameOfBaseClass.|
|where T : NameOfInterface|Параметр типу <T> має реалізовувати інтерфейс, визначений у NameOfInterface. Ви можете розділити кілька інтерфейсів списком, розділеним комами.|

Якщо вам не потрібно створювати надзвичайно типобезпечні власні колекції, вам може ніколи не знадобитися використовувати ключове слово where у ваших проектах C#. Незважаючи на це, наступна кілька (часткових) прикладів коду ілюструють, як працювати з ключовим словом where.

## Приклади використання ключового слова where

Почнемо з припущення, що ви створили власний генеричний клас і хочете переконатися, що параметр типу має конструктор за замовчуванням. Почнемо з припущення, що ви створили власний узагальнений клас і хочете переконатися, що параметр типу має конструктор за замовчуванням. Це може бути корисним, коли користувацький узагальнений клас потребує створення екземплярів T, оскільки конструктор за замовчуванням є єдиним конструктором, який потенційно є спільним для всіх типів. Також, обмеження T таким чином дозволяє отримати перевірку під час компіляції; якщо T є посилальною типом, програміст пам'ятав про перевизначення конструктора за замовчуванням у визначенні класу (ви можете пам'ятати, що конструктор за замовчуванням видаляється в класах, коли ви визначаєте свій власний).

```cs
// MyGenericClass derives from object, while
// contained items must have a default ctor.
public class MyGenericClass<T> where T : new()
{
  ...
}
```
Зверніть увагу, що речення where визначає, який параметр типу обмежується, а потім йде оператор двокрапки. Після оператора двокрапки ви перераховуєте кожне можливе обмеження (у цьому випадку конструктор за замовчуванням). Ось ще один приклад:

```cs
// MyGenericClass derives from object, while
// contained items must be a class implementing IDrawable
// and must support a default ctor.
public class MyGenericClass<T> where T : class, IDrawable, new()
{
  ...
}
```
У цьому випадку, T має три вимоги. Це має бути посилальний тип (не структура), як позначено токеном класу. По-друге, T повинен реалізувати інтерфейс IDrawable. По-третє, він також повинен мати конструктор за замовчуванням. Кілька обмежень перераховані у списку, розділеному комами; однак, слід пам'ятати, що обмеження new() завжди має бути вказано останнім! Таким чином, наступний код не скомпілюється:

```cs
// Error! new() constraint must be listed last!
public class MyGenericClass<T> where T : new(), class, IDrawable
{
  ...
}
```
Якщо ви коли-небудь створите власний клас універсальної колекції, який визначає кілька параметрів типу, ви можете вказати унікальний набір обмежень для кожного з них, використовуючи окремі речення where.

```cs
// <K> must extend SomeBaseClass and have a default ctor,
// while <T> must be a structure and implement the
// generic IComparable interface.
public class MyGenericClass<K, T> 
    where K : SomeBaseClass, new()
    where T : struct, IComparable<T>
{
  ...
}
```
Ви рідко зіткнетеся з випадками, коли вам потрібно буде створити повний власний узагальнений клас колекції; однак, ви також можете використовувати ключове слово where для узагальнених методів. Наприклад, якщо ви хочете вказати, що ваш універсальний метод Swap<T>() може працювати лише зі структурами, ви оновите метод ось так:

```cs
// This method will swap any structure, but not classes.
static void Swap<T>(ref T a, ref T b) where T : struct
{
  ...
}
```
Зверніть увагу, що якщо ви обмежите метод Swap() таким чином, ви більше не зможете міняти місцями рядкові об'єкти (як показано у прикладі коду), оскільки рядок є посилальною типом.

## Відсутність обмежень операторів

Наприкінці хочу зробити ще одне зауваження щодо узагальнених методів та обмежень. Вас може здивувати, але під час створення узагальнених методів ви отримаєте помилку компілятора, якщо застосувати будь-які оператори C# (+, -, *, == тощо) до параметрів типу. Наприклад, уявіть собі корисність класу, який може додавати, віднімати, множити та ділити узагальнені типи.

```cs
// Compiler error! Cannot apply
// operators to type parameters!
public class BasicMath<T>
{
  public T Add(T arg1, T arg2)
  { return arg1 + arg2; }
  public T Subtract(T arg1, T arg2)
  { return arg1 - arg2; }
  public T Multiply(T arg1, T arg2)
  { return arg1 * arg2; }
  public T Divide(T arg1, T arg2)
  { return arg1 / arg2; }
}
```
На жаль, попередній клас BasicMath не компілюється. Хоча це може здатися суттєвим обмеженням, потрібно пам'ятати, що узагальнені типи є узагальненими. Звичайно, числові дані можуть працювати з бінарними операторами C#. Однак, для пояснень, якби \<T\> був користувацьким класом або структурним типом, компілятор міг би припустити, що клас підтримує оператори +, -, * та /. В ідеалі C# мав би обмежувати узагальнений тип підтримуваними операторами, як у цьому прикладі:

```cs
// Illustrative code only!
public class BasicMath<T> where T : operator +, operator -,
  operator *, operator /
{
  public T Add(T arg1, T arg2)
  { return arg1 + arg2; }
  public T Subtract(T arg1, T arg2)
  { return arg1 - arg2; }
  public T Multiply(T arg1, T arg2)
  { return arg1 * arg2; }
  public T Divide(T arg1, T arg2)
  { return arg1 / arg2; }
}
```

На жаль, обмеження операторів не підтримуються в поточній версії C#.

# Підсумки

Цей розділ розпочався з розгляду неузагальнених типів колекцій System.Collections та System.Collections.Specialized, включаючи різні проблеми, пов'язані з багатьма неузагальненими контейнерами, такі як відсутність безпеки типів та накладні витрати на операції упаковки та розпакування. Як згадувалося, саме з цих причин сучасні програми .NET зазвичай використовують узагальнені класи колекцій, що знаходяться в System.Collections.Generic та System.Collections.ObjectModel.
Як ви бачили, узагальнений елемент дозволяє вам вказувати заповнювачі (параметри типу) під час створення об'єкта (або виклику, у випадку узагальнених методів). Хоча найчастіше ви просто використовуватимете узагальнені типи, що надаються в бібліотеках базових класів .NET, ви також зможете створювати власні узагальнені типи (і узагальнені методи). Коли ви це зробите, у вас буде можливість вказати будь-яку кількість обмежень (за допомогою ключового слова where), щоб підвищити рівень безпеки типів і гарантувати виконання операцій над типами відомої кількості, які гарантовано демонструють певні базові можливості.
Насамкінець, пам’ятайте, що узагальнені класи можна знайти в багатьох місцях у бібліотеках базових класів .NET. Тут ви зосередилися саме на узагальнених колекціях. Однак, у міру того, як ви будете працювати над рештою цієї книги (і коли ви занурюватиметесь у платформу на власних умовах), ви неодмінно знайдете узагальнені класи, структури та делегати, розташовані в заданому просторі імен. Також звертайте увагу на узагальнені члени неузагальненого класу!