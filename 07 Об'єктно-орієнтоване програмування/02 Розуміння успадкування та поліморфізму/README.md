# Розуміння успадкування та поліморфізму

У попередньому розділі розглядався перший стовп ООП: інкапсуляція. Ви навчилися створювати єдиний чітко визначений тип класу з конструкторами та різними членами (полями, властивостями, методами, константами та полями лише для читання). У цій главі буде зосереджено на двох стовпах ООП, що залишилися: спадкуванні та поліморфізмі.
По-перше, ви дізнаєтесь, як створювати родини споріднених класів за допомогою успадкування.Як ви побачите, ця форма повторного використання коду дозволяє вам визначати загальну функціональність у батьківському класі, яка може бути використана та, можливо, змінена дочірніми класами. Попутно ви дізнаєтеся, як створити поліморфний інтерфейс в ієрархії класів за допомогою віртуальних і абстрактних членів, а також дізнаєтесь про роль явного приведення. 
Розділ закінчиться вивченням ролі кінцевого батьківського класу в бібліотеках базових класів .NET: System.Object.

# Розуміння основної механіки успадкування

Успадкування — це аспект ООП, який полегшує повторне використання коду. Зокрема, повторне використання коду має два варіанти: успадкування (відношення «is-a») і модель обмеження/делегування (відношення «has-a»). Давайте почнемо цю главу з вивчення класичної моделі успадкування зв’язку «is-a».
Коли ви встановлюєте зв’язки «is-a» між класами, ви створюєте залежність між двома або більше типами класів. Основна ідея класичного успадкування полягає в тому, що нові класи можна створювати, використовуючи існуючі класи як відправну точку.

Для початку з простого прикладу створіть новий проект консольної програми під назвою BasicInheritance. Тепер припустімо, що ви розробили клас під назвою Car, який моделює деякі основні деталі автомобіля.

```cs
namespace BasicInheritance;

class Car1
{
    public readonly int MaxSpeed;
    private int _speed;

    public int Speed
    {
        get => _speed;
        set
        {
            _speed = value;
            if (_speed > MaxSpeed)
            {
                _speed = MaxSpeed;
            }
        }
    }

    public Car1(int maxSpeed)
    {
        MaxSpeed = maxSpeed;
    }
    public Car1()
    {
        MaxSpeed = 55;
    }
}
```

Зверніть увагу, що клас Car використовує служби інкапсуляції для керування доступом до приватного поля _speed ​​за допомогою загальнодоступної властивості Speed. На цьому етапі ви можете використовувати свій тип автомобіля таким чином:

```cs
using BasicInheritance;

void UsingClassCar1()
{
    // Make a Car object, set max speed and current speed.
    Car1 car = new Car1(80) { Speed = 50};
    // Print current speed.
    Console.WriteLine($"My car is going {car.Speed} MPH");
}
UsingClassCar1();
```
```
My car is going 50 MPH
```

## Визначення батьківського класу існуючого класу

Припустімо, ви хочете створити новий клас під назвою MiniVan. Як і базовий Car, ви хочете визначити клас MiniVan для підтримки даних про максимальну швидкість, поточну швидкість і властивість під назвою Speed, щоб дозволити користувачеві об’єкта змінювати стан об’єкта. Очевидно, що класи Car і MiniVan пов’язані між собою; фактично, можна сказати, що MiniVan "is-a" тип Car. Відношення «is-a» (офіційно називається класичним успадкуванням) дозволяє створювати нові визначення класу, які розширюють функціональність існуючого класу.

Існуючий клас, який слугуватиме основою для нового класу, називається базовим класом, суперкласом або батьківським класом. Роль базового класу полягає у визначенні всіх загальних даних і членів для класів, які його розширюють. Класи розширення формально називаються похідними або дочірніми класами. У C# ви використовуєте оператор двокрапки у визначенні класу, щоб встановити зв’язок «is-a» між класами. 

Припустімо, ви створили такий новий клас MiniVan:

```cs
namespace BasicInheritance;

class MiniVan1 : Car1
{
}
```
Наразі цей новий клас не визначив жодного члена. Отже, що ви отримали, розширивши свій MiniVan з базового класу Car? Простіше кажучи, об’єкти MiniVan тепер мають доступ до кожного відкритого члена, визначеного в батьківському класі.

Хоча конструктори зазвичай визначаються як публічні, похідний клас ніколи не успадковує конструктори батьківського класу. Конструктори використовуються для створення лише класу, у якому вони визначені, хоча вони можуть бути викликані похідним класом через ланцюжок конструкторів. 

Враховуючи зв’язок між цими двома типами класів, тепер ви можете використовувати клас MiniVan так:

```cs
void UsingMiniVan1()
{
    // Don't work
    //MiniVan1 miniVan = new(100) { Speed = 50 };

    // Now make a MiniVan object.
    MiniVan1 miniVan = new() { Speed = 50 };
    Console.WriteLine($"My van is going {miniVan.Speed}");
}
UsingMiniVan1();
```
```
My van is going 50
```
Ще раз зауважте, що хоча ви не додали жодного члена до класу MiniVan, у вас є прямий доступ до загальнодоступної властивості Speed ​​вашого батьківського класу, тому ви повторно використовували код. Ще раз зауважте, що хоча ви не додали жодного члена до класу MiniVan, у вас є прямий доступ до загальнодоступної властивості Speed ​​вашого батьківського класу, тому ви повторно використовували код. Це набагато кращий підхід, ніж створення класу MiniVan, який має ті самі члени, що й Car, наприклад властивість Speed. Якби ви створили дублікат коду між цими двома класами, вам потрібно було б тепер підтримувати два тіла коду, що, безумовно, є нецільовим використанням вашого часу. Завжди пам’ятайте, що спадкування зберігає інкапсуляцію; отже, наступний код призводить до помилки компілятора, оскільки приватні члени ніколи не можуть бути доступні з посилання на об’єкт:

```cs
    MiniVan1 miniVan1 = new();
    miniVan1.Speed = 30;
    Console.WriteLine($"My van is going {miniVan.Speed}");

    // Error! Can't access private members! 
    //miniVan1._speed = 70;
```
У зв’язку з цим, якби MiniVan визначив власний набір членів, він усе одно не мав би доступу до жодного приватного члена базового класу Car. Пам’ятайте, доступ до приватних членів може отримати лише клас, який їх визначає. Наприклад, такий метод у MiniVan призведе до помилки компілятора:

```cs
class MiniVan1 : Car1
{

    public void LowSpeed()
    {
        // Error! Cannot access private
        // members of parent within a derived type.
        //_speed = 10;

        // OK! Can access public members
        // of a parent within a derived type.
        Speed = 10;
    }
}
```

### Щодо кількох базових класів

Говорячи про базові класи, важливо мати на увазі, що C# вимагає, щоб даний клас мав рівно один прямий базовий клас. Неможливо створити тип класу, який безпосередньо походить від двох або більше базових класів (цей прийом, який підтримується в некерованому C++, відомий як множинне успадкування або просто MI). Якщо ви спробуєте створити клас, який визначає два прямі батьківські класи, як показано в наведеному нижче коді, ви отримаєте помилки компілятора:

```cs
// Illegal! C# does not allow
// multiple inheritance for classes!
class MyClass
  : BaseClassOne, BaseClassTwo
{}
```
Як ви побачите в розділі "Робота з інтерфейсами", платформа .NET Core дійсно дозволяє певному класу або структурі реалізувати будь-яку кількість дискретних інтерфейсів. Таким чином, тип C# може демонструвати ряд поведінки, уникаючи складнощів, пов’язаних з MI. Використовуючи цю техніку, ви можете побудувати складні ієрархії інтерфейсів, які моделюють складну поведінку.

## Використання ключового слова sealed (запечатаний, закритий).

C# надає інше ключове слово, запечатане, яке запобігає успадкуванню. Коли ви позначаєте клас як закритий, компілятор не дозволить вам успадкуватися від цього типу. Наприклад, припустимо, що ви вирішили, що немає сенсу далі розширювати клас MiniVan.

```cs
sealed class MiniVan1 : Car1
{
    //...
}
```
Якби ви (або ваш товариш по команді) спробували отримати від цього класу, ви отримали б помилку під час компіляції.

```cs
// Error! Cannot extend
// a class marked with the sealed keyword!
class DeluxeMiniVan
  : MiniVan1
{
}
```
Найчастіше запечатування класу має найкращий сенс, коли ви проектуєте службовий(utility) клас. Наприклад, простір імен System визначає численні запечатані класи, наприклад клас String. Таким чином, як і MiniVan, якщо ви спробуєте створити новий клас, який розширює System.String, ви отримаєте помилку під час компіляції.

```cs
// Another error! Cannot extend
// a class marked as sealed!
class MyString
  : String
{
}
```

Структури C# завжди неявно запечатані. Таким чином, ви ніколи не можете вивести одну структуру з іншої структури, клас із структури або структуру з класу. Структури можна використовувати для моделювання лише окремих, атомарних, визначених користувачем типів даних. Якщо ви хочете використовувати зв’язок «is-a», ви повинні використовувати класи.

Як ви здогадуєтеся, є багато інших деталей про успадкування, які ви дізнаєтеся протягом решти цього розділу. Наразі просто майте на увазі, що оператор двокрапки дозволяє вам встановлювати зв’язки базовий/похідний клас, тоді як ключове слово sealed запобігає подальшому успадкуванню.

## Visual Studio Class Diagrams

Visual Studio дозволяє візуально встановлювати зв’язки базового/похідного класу під час розробки. Щоб скористатися цим аспектом IDE, ваш перший крок — включити новий файл діаграми класів у ваш поточний проект. Для цього перейдіть до пункту меню Project ➤ Add New Item і клацніть піктограму Class Diagram.
Після цього ви побачите порожню поверхню дизайнера. Щоб додати типи до конструктора класів, просто перетягніть кожен файл із вікна Solution Explorer на поверхню. Також пам’ятайте, що якщо ви видалите елемент із візуального дизайнера (просто вибравши його та натиснувши клавішу Delete), це не знищить пов’язаний вихідний код, а просто видалить елемент із поверхні дизайнера.
На малюнку показано поточну ієрархію класів.

![ClassDesigner](ClassDiagram.jpg)

Окрім простого відображення зв’язків типів у вашій поточній програмі, ви також можете створювати нові типи та заповнювати їхні члени за допомогою панелі інструментів Class Designer та вікна Class Details.

Якщо ви хочете використовувати ці візуальні інструменти протягом решти книги, не соромтеся. Однак завжди переконайтеся, що ви аналізуєте згенерований код, щоб мати чітке розуміння того, що ці інструменти зробили від вашого імені.

# Розуміння другого стовпа ООП: успадкування детально

Тепер, коли ви ознайомилися з основним синтаксисом успадкування, давайте створимо більш складний приклад і познайомимося з численними деталями побудови ієрархії класів. Для цього ви повторно використаєте клас Employee, розроблений у попередьному розділі. Для початку створіть новий проект C# Console Application під назвою EmployeeApp. Скопіюємо або створимо  файли класів.

EmployeePayTypeEnum.cs
```cs
namespace EmployeeApp;

public enum EmployeePayTypeEnum
{
    Hourly,
    Salaried,
    Commission
} 
```
Employee6.Core.cs
```cs
namespace EmployeeApp;

partial class Employee6
{
    // Field data
    private string _name = null!;
    private string _SSN = null!;

    // Properties
    public int Id { get; set; }
    public string Name
    {
        get => _name;
        set
        {
            if (value.Length > 15)
            {
                Console.WriteLine("Error! Name length exceeds 15 characters!");
            }
            else
            {
                _name = value;
            }
        }
    }
    public float Pay { get; set; }
    public int Age { get; set; }
    public string SSN => _SSN;
    public EmployeePayTypeEnum PayType {  get; set; }

    //Constructors
    public Employee6(int id, string name, float pay, int age, string ssn,
        EmployeePayTypeEnum payType)
    {
        Id = id;
        Name = name;
        Pay = pay;
        Age = age;
        _SSN = ssn;
        PayType = payType;
    }
    public Employee6(int id, string name, float pay) :
        this(id, name, pay, 0, "", EmployeePayTypeEnum.Salaried)
    { }
    public Employee6() { }
}
```
Employee6.cs
```cs
namespace EmployeeApp;

partial class Employee6
{
    // Mathods
    public void GiveBonus(float amount)
    {
        Pay = this switch
        {
            { Age: >= 18, PayType: EmployeePayTypeEnum.Commission }
            => Pay += 0.10F * amount,
            { Age: >= 18, PayType: EmployeePayTypeEnum.Hourly }
            => Pay += 40F * amount / 2080F,
            { Age: >= 18, PayType: EmployeePayTypeEnum.Salaried }
            => Pay += amount,
            _ => Pay += 0
        };
    }

    public void DisplayStatus()
    {
        Console.WriteLine($"{Id}\t{Name}\t{Age}\t{Pay}");
    }
}
```

За допомогою .NET Core усі файли в поточній структурі каталогів автоматично включаються у ваш проект. Простого копіювання двох файлів з іншого проекту в поточний каталог проекту достатньо, щоб включити їх до вашого проекту.

Оскільки назви проектів співпадають простори імен в файлах теж співпадають. 

```cs
namespace EmployeeApp;

partial class Employee6
{
    //...
}
```
Для перевірки використаємо метод

```cs
void UsingEmployee6Class()
{
    Employee6 employee = new(23, "Marvin", 1000, 35,
        "111 - 11 - 1111", EmployeePayTypeEnum.Salaried);
    employee.DisplayStatus();
    employee.GiveBonus(100);
    employee.DisplayStatus();
}
UsingEmployee6Class();
```
```
23      Marvin  35      1000
23      Marvin  35      1100
```
Наша мета — створити сімейство класів, які моделюють різні типи працівників компанії. Припустімо, ви хочете використати функціональність класу Employee для створення двох нових класів (SalesPerson і Manager). Новий клас SalesPerson «is-an» Employee (як і Manager). Пам’ятайте, що за класичною моделлю успадкування базові класи (такі як Employee) використовуються для визначення загальних характеристик, спільних для всіх нащадків. Підкласи (такі як SalesPerson і Manager) розширюють цю загальну функціональність, додаючи більш специфічну функціональність. У нашому прикладі ви припустите, що клас Manager розширює Employee, записуючи кількість опціонів на акції, тоді як клас SalesPerson зберігає кількість здійснених продажів.

Вставте новий файл класу, який визначає клас Manager із такою автоматичною властивістю:

```cs
namespace EmployeeApp;
// Managers need to know their number of stock options.
class Manager6 :Employee6
{
    public int StockOptions { get; set; }
}

```
Далі додайте ще один новий файл класу (SalesPerson.cs), який визначає клас SalesPerson із відповідною автоматичною властивістю.

```cs
namespace EmployeeApp;
// Salespeople need to know their number of sales.
class SalesPerson6
{
    public int SalesNumber { get; set; }
}

```
Тепер, коли ви встановили зв’язок «is-a», SalesPerson і Manager автоматично успадкували всіх відкритих членів базового класу Employee. Щоб проілюструвати, оновіть Program.cs:

```cs
void UsingSalesPerson6()
{
    SalesPerson6 salesPerson = new()
    {
        Name = "Fred",
        Age = 31,
        SalesNumber = 50
    };

    salesPerson.DisplayStatus();
}
UsingSalesPerson6();
```
```
0       Fred    31      0
```

## Виклик конструкторів базового класу за допомогою ключового слова base

Наразі SalesPerson і Manager можна створити лише за допомогою конструктора за замовчуванням «халявного» без параметрів. Маючи це на увазі, припустимо, що ви рішили додали новий конструктор із семи аргументів до типу Manager. Для цього ви можете реалізувати цей спеціальний конструктор у класі Manager наступним чином:

```cs
    public Manager6(int id, string name, float pay, int age, string ssn, 
        EmployeePayTypeEnum payType, int stockOptions )
    {
        // This property is defined by the Manager class.
        StockOptions = stockOptions;

        // Assign incoming parameters using the
        // inherited properties of the parent class.
        Id = id;
        Name = name;
        Pay = pay;
        Age = age;
        // OOPS! This would be a compiler error,
        // if the SSN property were read-only! 
        _SSN = ssn;
        PayType = payType;
    }
```
Якщо ви подивитеся на список параметрів, то чітко побачите, що більшість цих аргументів мають зберігатися в змінних-членах, визначених базовим класом Employee.
Перша проблема з цим підходом полягає в тому, що якщо ви визначили будь-яку властивість як доступну лише для читання (наприклад, властивість SocialSecurityNumber), ви не можете призначити параметр вхідного рядка цьому полю, як показано в кінцевому операторі коду цього спеціального конструктора.
Друга проблема полягає в тому, що ви опосередковано створили досить неефективний конструктор, враховуючи, що в C#, якщо ви не вкажете інше, конструктор за замовчуванням базового класу викликається автоматично перед виконанням логіки похідного конструктора. Після цього моменту поточна реалізація отримує доступ до багатьох загальнодоступних властивостей базового класу Employee, щоб встановити його стан. Таким чином, ви справді зробили вісім звернень (шість успадкованих властивостей і два виклики конструктора) під час створення об’єкта Manager! 
Щоб оптимізувати створення похідного класу, вам буде краще застосувати ваші конструктори підкласів, щоб явно викликати відповідний настроюваний конструктор базового класу, а не типовий. Таким чином ви можете зменшити кількість викликів успадкованих членів ініціалізації (що економить час обробки).
По-перше, переконайтеся, що ваш батьківський клас Employee має такий конструктор із шістьма аргументами:

```cs
public Employee6(int id, string name, float pay, int age, string ssn,
    EmployeePayTypeEnum payType)
{
    Id = id;
    Name = name;
    Pay = pay;
    Age = age;
    _SSN = ssn;
    PayType = payType;
}
```
Тепер давайте модернізуємо спеціальний конструктор типу Manager, щоб викликати цей конструктор за допомогою ключового слова base.

```cs
    public Manager6(int id, string name, float pay, int age, string ssn, 
         int stockOptions) 
        : base(id, name, pay, age, ssn, EmployeePayTypeEnum.Salaried)
    {
        StockOptions = stockOptions;
    }
```
Тут ключове слово base залежить від сігнатури конструктора (подібно до синтаксису, який використовується для об’єднання конструкторів в один клас за допомогою ключового слова this), що завжди вказує на те, що похідний конструктор передає дані безпосередньо у батьковій конструктор. У цій ситуації ви явно викликаєте конструктор із шістьма параметрами, визначений Employee, і заощаджуєте собі непотрібні виклики під час створення дочірнього класу. Крім того, ви додали особливу поведінку до класу менеджера, оскільки тип оплати завжди встановлюється на Salaried.
Настроюваний конструктор SalesPerson виглядає майже ідентично, за винятком того, що для типу оплати встановлено Commission.

```cs
    public SalesPerson6(int id, string name, float pay, int age, string ssn, 
        int salesNumber) 
        : base(id, name, pay, age, ssn, EmployeePayTypeEnum.Commission)
    {
        SalesNumber = salesNumber;
    }
```
Ви можете використовувати ключове слово base щоразу, коли підклас хоче отримати доступ до публічного або захищеного члена, визначеного батьківським класом. Використання цього ключового слова не обмежується логікою конструктора. Ви побачите приклади використання бази таким чином під час дослідження поліморфізму, далі в цій главі.
Зрештою, пам’ятайте, що коли ви додаєте власний конструктор до визначення класу, конструктор за замовчуванням мовчки видаляється. Тому обов’язково перевизначте конструктор за замовчуванням для типів SalesPerson і Manager. Ось приклад:

```cs
void CallingBaseClassConstructors()
{
    Manager6 manager = new Manager6(5, "Alexandr", 1000, 35, "1234-234-32",
        EmployeePayTypeEnum.Commission, 123);
    manager.DisplayStatus();
}
CallingBaseClassConstructors();
```
```
5       Alexandr        35      1000
```

## Зберігання сімейних таємниць: ключове слово protected

Як ви вже знаєте, public елементи доступні з будь-якого місця, тоді як private елементи можуть бути доступні лише для класу, який їх визначив. C# бере на себе лідерство серед багатьох інших сучасних об’єктних мов і надає додаткове ключове слово для визначення доступності учасників: protected.
Коли базовий клас визначає protected дані або protected члени, він встановлює набір елементів, до яких може отримати прямий доступ будь-який нащадок. Якщо ви хочете дозволити дочірнім класам SalesPerson і Manager прямий доступ до сектора даних, визначеного Employee, ви можете оновити оригінальне визначення класу Employee (у файлі EmployeeCore.cs) таким чином:

```cs
partial class Employee7
{
    // Field data
    protected string name = null!;
    protected string ssn = null!;
    //...

}    
```
Згідно з умовами, захищені члени мають назву PascalCased (EmpName), а не underscore-camelCase (_empName). Це не вимога мови, а загальний стиль коду.

Перевага визначення захищених членів у базовому класі полягає в тому, що похідним типам більше не потрібно опосередковано отримувати доступ до даних за допомогою публічних методів або властивостей. Звичайно, можлива помилка полягає в тому, що коли похідний тип має прямий доступ до внутрішніх даних свого батька, можна випадково обійти існуючі бізнес-правила, знайдені в загальнодоступних властивостях. Коли ви визначаєте захищені члени, ви створюєте рівень довіри між батьківським класом і дочірнім класом, оскільки компілятор не виявить жодних порушень бізнес-правил вашого типу. 
Нарешті, зрозумійте, що щодо користувача об’єкта захищені дані вважаються приватними (оскільки користувач знаходиться «поза» родиною).

```cs
void TheProtectedKeyword()
{
    Employee7 employee = new();
    // Error! Can't access protected data from client code.
    employee.name = "John";
}
```
Хоча захищені дані поля можуть порушити інкапсуляцію, досить безпечно (і корисно) визначати захищені методи. Під час побудови ієрархії класів зазвичай визначають набір методів, які призначені лише для використання похідними типами і не призначені для використання зовнішнім світом.

## Додавання sealed класу

Запечатаний(sealed) клас не може бути розширений іншими класами. Як згадувалося, цей прийом найчастіше використовується, коли ви проектуєте utility(корисний) клас. Однак під час побудови ієрархії класів ви можете виявити, що певну гілку в ланцюжку успадкування слід «закрити», оскільки немає сенсу далі розширювати родовід. Наприклад, припустімо, що ви додали ще один клас до своєї програми (PtSalesPerson), який розширює існуючий тип SalesPerson.

![SealedClass](SealedClass.jpg)

PtSalesPerson — це клас, який, звісно, ​​представляє продавця, який працює неповний робочий день. Для аргументації, скажімо, ви хочете переконатися, що жоден інший розробник не зможе створити підклас від PTSalesPerson. Щоб заборонити іншим розширювати клас, використовуйте ключове слово sealed.

```cs
namespace EmployeeApp;

sealed class PtSalesPerson6 : SalesPerson6
{
    public PtSalesPerson6(int id, string name, float pay, int age, string ssn, int salesNumber) 
        : base(id, name, pay, age, ssn, salesNumber)
    {
    }
    
}
```

# Розуміння успадкування з record

Типи record також підтримують успадкування. Щоб дослідити це, додайте новий проект із типом consoleapp під назвою RecordInheritance.

## Спадкування для типів record зі стандартними властивостями

Додайте два нових файли з іменами Car1.cs і MiniVan1.cs і додайте такий код визначення запису до відповідних файлів:

```cs
public record Car1
{
    public string Make { get; init; } = null!;
    public string Model { get; init; } = null!;
    public string Color { get; init; } = null!;

    public Car1(string make, string model, string color)
    {
        Make = make;
        Model = model;
        Color = color;
    }
}
```
```cs
namespace RecordInheritance;

public sealed record MiniVan1 : Car1
{
    public int Seating {  get; set; }

    public MiniVan1(string make, string model, string color, int seating) 
        : base(make, model, color)
    {
        Seating = seating;
    }
}
```
Зверніть увагу, що між цими прикладами використання типів записів і попередніми прикладами використання класів немає великої різниці. Модифікатор sealed для типу запису запобігає похідним від інших типів записів із запечатаних типів записів. Незважаючи на те, що він не використовується в перелічених прикладах, модифікатор protected до властивостей і методів поводиться так само, як із успадкуванням класу. Це пояснюється тим, що типи записів є лише особливим типом класу.
Типи записів також включають неявні приведення до свого базового класу, як показано в наступному коді:

```cs
using RecordInheritance;

void ImplicitCastsRecords()
{
    Car1 c = new Car1("Honda", "Pilot", "Blue");
    MiniVan1 m = new MiniVan1("Honda", "Pilot", "Blue", 10);
    Console.WriteLine($"Checking MiniVan is-a Car:{m is Car1}");
}
ImplicitCastsRecords();
```
```
Checking MiniVan is-a Car:True
```
Важливо зазначити, що навіть якщо типи записів є спеціалізованими класами, ви не можете перехресно успадкувати між класами та записами. Щоб було зрозуміло, класи не можуть успадковувати типи записів, а типи записів не можуть успадковуватись від класів. Розглянемо наступний код і зауважимо, що останні два приклади не компілюються:

```cs
namespace RecordInheritance;
public class TestClass { }
public record TestRecord { }
//Classes cannot inherit records
//public class Test2 : TestRecord { }
//Records types cannot inherit from classes
//public record Test2 : TestClass {  }
```

## Успадкування для типів record із позиційними параметрами

Спадкування також працює з типами позиційних записів. Похідний record оголошує позиційні параметри для всіх параметрів у базовому записі. Похідний record не приховує їх, а використовує з базового запису. Похідний record лише створює та ініціалізує властивості, яких немає в базовому. Щоб побачити це в дії, створіть новий файл. Додайте наступний код у свій файл:

```cs
namespace RecordInheritance;

public record Car2(string Make, string Model, string Color);
public record MiniVan2(string Make, string Model, string Color,int Seating)
    : Car2(Make, Model,Color);


public record MotorCycle(string Make, string Model);
public record Scooter(string Make, string Model):MotorCycle(Make, Model);
public record FancyScooter(string Make, string Model, string FancyColor)
    : Scooter(Make, Model);
```

Спробуємо використати.

```cs
void UsingPosotionalRecord()
{
    MiniVan2 vito = new MiniVan2("Mercedes", "Vito 110", "DarkGrey", 18);
    Console.WriteLine(vito);

    FancyScooter scooter = new FancyScooter("Honda", "Reveals","Red-Blue");
    Console.WriteLine(scooter);
}
UsingPosotionalRecord();
```
```
MiniVan2 { Make = Mercedes, Model = Vito 110, Color = DarkGrey, Seating = 18 }
FancyScooter { Make = Honda, Model = Reveals, FancyColor = Red-Blue }
```

## Мутація без деконструкції з успадкованими типами record

Під час створення нових екземплярів типу record за допомогою виразу with результуючий тип record є тим самим типом часу виконання операнда. Візьмемо такий приклад:

```cs
void NondestructiveMutationWithInheritedRecord()
{
    MotorCycle motorCycle1 = new FancyScooter("Harley", "Lowrider", "Gold");
    Console.WriteLine(motorCycle1);
    Console.WriteLine($"motorCycle1 is FancyScooter :{motorCycle1 is FancyScooter}");

    MotorCycle motorCycle2 = motorCycle1 with { Model = "Low Rider S" };
    Console.WriteLine(motorCycle2);
    Console.WriteLine($"motorCycle2 is FancyScooter :{motorCycle2 is FancyScooter}");
}
NondestructiveMutationWithInheritedRecord();
```
```
FancyScooter { Make = Harley, Model = Lowrider, FancyColor = Gold }
motorCycle1 is FancyScooter :True
FancyScooter { Make = Harley, Model = Low Rider S, FancyColor = Gold }
motorCycle2 is FancyScooter :True
```

## Рівність із успадкованими типами record

Типи записів використовують семантику значення для визначення рівності. Ще одна деталь щодо типів записів полягає в тому, що тип запису є частиною розгляду рівності. Візьміть до уваги попередні типи MotorCycle і Scooter:

```cs
public record MotorCycle(string Make, string Model);
public record Scooter(string Make, string Model):MotorCycle(Make, Model);
```
Ігноруючи той факт, що зазвичай успадковані класи розширюють базові класи, ці прості приклади визначають два різні типи записів, які мають однакові властивості. Під час створення екземплярів з однаковими значеннями властивостей вони не проходять перевірку на рівність через те, що є різними типами. Візьмемо, наприклад, такий код і результати:

```cs
void EqualityWithInheritedRecord()
{
    MotorCycle motorCycle = new MotorCycle("Harley", "Low Rider");
    MotorCycle scooter = new Scooter("Harley", "Low Rider");
    Console.WriteLine($"MotorCycle and Scooter are equal: {Equals(motorCycle,scooter)}");
    Console.WriteLine(motorCycle);
    Console.WriteLine(scooter);
}
EqualityWithInheritedRecord();
```
```
MotorCycle and Scooter are equal: False
MotorCycle { Make = Harley, Model = Low Rider }
Scooter { Make = Harley, Model = Low Rider }

```
Зверніть увагу, що змінні оголошено як типи записів MotorCycle. Незважаючи на це, типи не однакові, оскільки типи середовища виконання різні.

## Поведінка деконструктора з успадкованими типами record

Метод Deconstruct() похідного record повертає значення всіх позиційних властивостей оголошеного типу під час компіляції. Однак, якщо змінна приведена до похідного типу, тоді всі позиційні властивості похідного типу деконструюються, як показано тут:

```cs
void DeconstructorBehaviorWithInheritedRecord()
{
    MotorCycle motorCycle = new FancyScooter("Harley", "Low rider", "Gold");
    var (make1, model1) = motorCycle;
    Console.WriteLine(make1+"\t"+model1);

    // You need to cast the variable to the derived type
    var (make2, model2,color2) = (FancyScooter)motorCycle;
    Console.WriteLine(make2 + "\t" + model2+"\t"+color2);
}
DeconstructorBehaviorWithInheritedRecord();
```
```
Harley  Low rider
Harley  Low rider       Gold
```

## Програмування для утримання/делегування

Повторне використання коду буває двох варіантів. Ви щойно дослідили класичне співвідношення «is-a». Перш ніж вивчати третій стовп ООП (поліморфізм), давайте розглянемо зв’язок «має» (також відомий як модель утримання/делегування або агрегації).
Повертаючись до проекту EmployeeApp, створіть новий файл під назвою BenefitPackage.cs і додайте код для моделювання пакета пільг для співробітників, як показано нижче:

```cs
namespace EmployeeApp;

// This new type will function as a contained class
class BenefitPackage
{
    // Assume we have other members that represent
    // dental/health benefits, and so on.
    public double ComputePayDeduction()
    {
        return 125.0;
    }
}

```

Очевидно, було б досить дивно встановити зв’язок «is-a» між класом BenefitPackage і типами співробітників. Однак має бути зрозуміло, що між ними можуть бути встановлені певні стосунки. Коротше кажучи, ви хотіли б висловити ідею, що кожен працівник «has-a» BenefitPackage. Для цього ви можете оновити визначення класу Employee таким чином:

Employee7.Core.cs
```cs
partial class Employee7
{
    // Field data
    //...

    protected BenefitPackage employeeBenefits = new();
    // ...
}
```
На цьому етапі ви успішно розмістили інший об’єкт. Однак надання зовнішньому світу функціональності об’єкта, що міститься, вимагає делегування. Делегування — це просто акт додавання публічних членів до класу-вмісту, які використовують функціональні можливості вміщеного об’єкта. Наприклад, ви можете оновити клас Employee, щоб відкрити об’єкт empBenefits, що міститься, за допомогою спеціальної властивості, а також використовувати його функціональні можливості всередині за допомогою нового методу під назвою GetBenefitCost().

Employee7.Core.cs
```cs
partial class Employee7
{
    // Field data
    // ...
    protected BenefitPackage employeeBenefits = new();

    // Properties
    //...
    public BenefitPackage Benefits 
    { 
        get => employeeBenefits; 
        set => employeeBenefits = value; 
    }    
    // ...
}

Employee7.cs
```cs
partial class Employee7
{
    // Mathods
    //...

    // Expose certain benefit behaviors of object.
    public double GetBenefitCost() => employeeBenefits.ComputePayDeduction();
}
```
У наступному оновленому коді зверніть увагу на те, як можна взаємодіяти з внутрішнім типом BenefitsPackage, визначеним типом Employee:

```cs
void ClassUseOtherClass()
{
    Manager7 manager = new Manager7(15, "John", 100000, 50, "333-23-2322", 9000);
    manager.DisplayStatus();
    Console.WriteLine($"Benefit Cost:{manager.GetBenefitCost()}");
}
ClassUseOtherClass();
```
```
15      John    50      100000
Benefit Cost:125
```

## Розуміння визначень вкладених типів

Концепція вкладених типів — це розгортання зв’язку «has-a», який ви щойно розглянули. У C# (як і в інших мовах .NET) можна визначити тип (enum, клас, інтерфейс, структуру або делегат) безпосередньо в межах класу або структури. Коли ви це зробите, вкладений (або «inner») тип вважається членом в який вкладається (або «outer») класу, і в очах середовища виконання ним можна маніпулювати, як і будь-яким іншим членом (поля, властивості, методи, та події). Синтаксис, який використовується для вкладення типу, досить простий.

```cs
public class OuterClass
{
  // A public nested type can be used by anybody.
  public class PublicInnerClass {}
  // A private nested type can only be used by members
  // of the containing class.
  private class PrivateInnerClass {}
}
```
Хоча синтаксис досить зрозумілий, зрозуміти, чому ви хочете це зробити, може бути неочевидним.

    Вкладені типи дозволяють отримати повний контроль над рівнем доступу внутрішнього типу, оскільки вони можуть бути оголошені приватно (нагадаємо, що невкладені класи не можуть бути оголошені за допомогою ключового слова private).

    Оскільки вкладений тип є членом класу, що містить, він може отримати доступ до приватних членів класу, що містить.

    Часто вкладений тип корисний лише як помічник для зовнішнього класу і не призначений для використання зовнішнім світом.

Коли тип вкладає інший тип класу, він може створювати змінні-члени цього типу, як це було б для будь-якої точки даних. Однак, якщо ви хочете використовувати вкладений тип поза типом, що містить, ви повинні кваліфікувати його за сферою дії типу вкладення. Розглянемо наступний код:

```cs
// Create and use the public inner class. OK!
OuterClass.PublicInnerClass inner;
inner = new OuterClass.PublicInnerClass();
// Compiler Error! Cannot access the private class.
OuterClass.PrivateInnerClass inner2;
inner2 = new OuterClass.PrivateInnerClass();
```

Щоб використати цю концепцію в прикладі працівника, припустімо, що тепер ви вклали BenefitPackage безпосередньо в тип класу Employee.

Employee7.Core.cs
```cs
partial class Employee7
{
    //...
    // Class member
    public class BenefitPackage
    {
        public double ComputePayDeduction()
        {
            return 125.0;
        }
    }
    //...
}
```
Процес вкладення може бути настільки «глибоким», як вам потрібно. Наприклад, припустімо, що ви хочете створити перелік під назвою BenefitPackageLevel, який документує різні рівні виплат, які може вибрати працівник. Щоб програмно забезпечити тісний зв’язок між Employee, BenefitPackage і BenefitPackageLevel, ви можете вкласти перерахування таким чином:

```cs
partial class Employee7
{
    //...
    // Class member
    public class BenefitPackage
    {
        public enum BenefitPackageLevel
        {
        Standard, Gold, Platinum
        }

        public double ComputePayDeduction()
        {
            return 125.0;
        }
    }
    //...
}
```
Через взаємозв’язки вкладеності зверніть увагу на те, як ви повинні використовувати цей перелік

```cs
void ClassUseOtherClass()
{

    //...

    Employee7.BenefitPackage.BenefitPackageLevel packageLevel =
        Employee7.BenefitPackage.BenefitPackageLevel.Platinum;
    Console.WriteLine(packageLevel);
}
ClassUseOtherClass();

```
```
...
Platinum
```
На цьому етапі ви познайомилися з низкою ключових слів (і концепцій), які дозволяють вам будувати ієрархії пов’язаних типів за допомогою класичного успадкування, утримування та вкладених типів. Якщо деталі зараз нечіткі, не переймайтеся. До кінця ви побудуєте ряд додаткових ієрархій. Далі розглянемо останній стовп ООП: поліморфізм.

# Розуміння третьої основи ООП: підтримка поліморфізма в C#

Нехай в базовий класі Employee визначив метод під назвою GiveBonus(), який реалізований як спочату наступним чином :

```cs
    public void GiveBonus(float amount)
    {
        Pay += amount;
    }
```
Оскільки цей метод було визначено за допомогою ключового слова public, тепер ви можете надавати бонуси продавцям і менеджерам (а також продавцям, які працюють неповний робочий день).

```cs
void UsingGiveBonus()
{
    Manager7 manager = new(3, "Jack", 100000, 50, "3256-56-2536", 9000);
    manager.GiveBonus(300);
    manager.DisplayStatus();

    SalesPerson7 salesPerson = new(8, "Olga", 3000, 35, "2342-34-3432", 31);
    salesPerson.GiveBonus(200);
    salesPerson.DisplayStatus();
}
UsingGiveBonus();
```
```
3       Jack    50      100300
8       Olga    35      3200
```
Проблема з поточним дизайном полягає в тому, що публічно успадкований метод GiveBonus() працює однаково для всіх підкласів. В ідеалі бонус продавця повинен враховувати кількість продажів. Можливо, менеджерам варто отримати додаткові опціони на акції в поєднанні з підвищенням зарплати. Враховуючи це, ви раптом стикаєтеся з цікавим питанням: «Як споріднені типи можуть по-різному відповідати на той самий запит?»

## Використання virtual і override ключових слів

Поліморфізм надає можливість для підкласу визначати власну версію методу, визначеного його базовим класом, використовуючи процес, який називається перевизначенням методу. Щоб модернізувати свій поточний дизайн, вам потрібно зрозуміти значення ключових слів virtual і override. Якщо базовий клас хоче визначити метод, який може бути (але не обов’язково) перевизначений підкласом, він повинен позначити метод ключовим словом virtual.

```cs
 // This method can now be 'overridden' by a derived class.
 public virtual void GiveBonus(float amount)
 {
     Pay += amount;
 }
```
Методи, позначені ключовим словом virtual, називаються віртуальними методами. Коли підклас хоче змінити деталі реалізації віртуального методу, він робить це за допомогою ключового слова override. Наприклад, SalesPerson і Manager можуть замінити GiveBonus() наступним чином (припустимо, що PTSalesPerson не замінить GiveBonus() і, отже, просто успадковує версію, визначену SalesPerson):

```cs
class SalesPerson7 : Employee7
{

    //...

    public override void GiveBonus(float amount)
    {
        int salesBonus = 0;

        if (SalesNumber >= 0 && SalesNumber <= 100)
        {
            salesBonus = 10;
        }
        else
        {
            if (SalesNumber >= 101 && SalesNumber <= 200)
            {
                salesBonus = 15;
            }
            else
            {
                salesBonus = 20;
            }
        }
        base.GiveBonus(amount*salesBonus);
    }
}
```
```cs
class Manager7 :Employee7
{
 
    //...

    public override void GiveBonus(float amount)
    {
        base.GiveBonus(amount);
        Random r = new Random();
        StockOptions += r.Next(500);
    }
}
```
Зверніть увагу, що кожен перевизначений метод може вільно використовувати поведінку за замовчуванням за допомогою ключового слова base. Таким чином, вам не потрібно повністю реалізовувати логіку GiveBonus(), але ви можете повторно використовувати (і, можливо, розширити) типову поведінку батьківського класу. Також припустимо, що поточний метод DisplayStats() класу Employee оголошено віртуально.

```cs
partial class Employee7
{
    // Mathods

    //...

    public virtual void DisplayStatus()
    {
        Console.WriteLine($"Id:\t{Id}");
        Console.WriteLine($"Name:\t{Name}");
        Console.WriteLine($"Age:\t{Age}");
        Console.WriteLine($"Pay:\t{Pay}");
        Console.WriteLine($"SSN:\t{SSN}");
    }

    //...
}
```
Зробивши так, кожен підклас може перевизначити цей метод для відображення кількості продажів (для продавців) і поточних опціонів на акції (для менеджерів). Наприклад, розглянемо версію методу DisplayStats() від менеджера (клас SalesPerson реалізує DisplayStats() подібним чином, щоб показати кількість продажів).

Manager7.cs
```cs
    public override void DisplayStatus()
    {
        base.DisplayStatus();
        Console.WriteLine($"StockOptions:\t{StockOptions}");
    }
```
SalesPerson7.cs
```cs
    public override void DisplayStatus()
    {
        base.DisplayStatus();
        Console.WriteLine($"SalesNumber:\t{SalesNumber}");
    }
```
Тепер, коли кожен підклас може інтерпретувати значення цих віртуальних методів для себе, кожен екземпляр об’єкта поводиться як більш незалежна сутність.

```cs
void UsingGiveBonus()
{
    Manager7 manager = new(3, "Jack", 100000, 50, "3256-56-2536", 9000);
    manager.GiveBonus(300);
    manager.DisplayStatus();

    SalesPerson7 salesPerson = new(8, "Olga", 3000, 35, "2342-34-3432", 31);
    salesPerson.GiveBonus(200);
    salesPerson.DisplayStatus();
}
UsingGiveBonus();
```
```

Id:     3
Name:   Jack
Age:    50
Pay:    100300
SSN:    3256-56-2536
StockOptions:   9324

Id:     8
Name:   Olga
Age:    35
Pay:    5000
SSN:    2342-34-3432
SalesNumber:    31
```

## Перевизначення віртуальних членів із кодом Visual Studio/Visual Studio

Як ви вже могли помітити, коли ви змінюєте член, ви повинні згадати тип кожного параметра, не кажучи вже про назву методу та правила передачі параметрів (ref, out і params). І Visual Studio, і Visual Studio Code мають корисну функцію, якою можна скористатися під час заміни віртуального члена. Якщо ви введете слово override в межах типу класу (потім натисніть пробіл), IntelliSense автоматично відобразить список усіх елементів, які можна замінити, визначених у ваших батьківських класах, за винятком уже перевизначених методів. Коли ви вибираєте учасника та натискаєте клавішу Enter, IDE відповідає, автоматично заповнюючи заглушку методу від вашого імені. Зауважте, що ви також отримуєте оператор коду, який викликає вашу батьківську версію віртуального члена (ви можете видалити цей рядок, якщо він не потрібен). Наприклад, якщо ви використали цю техніку під час заміни методу DisplayStats(), ви можете знайти такий автоматично згенерований код:

```cs
    public override void DisplayStatus()
    {
        base.DisplayStatus();
    }
```

## Запечатування віртуальних учасників (sealed)

Згадайте, що ключове слово sealed можна застосувати до типу класу, щоб запобігти іншим типам розширювати його поведінку через успадкування. Як ви, мабуть, пам’ятаєте, ви запечатали PtSalesPerson, оскільки вважали, що іншим розробникам немає сенсу далі розширювати цю лінію успадкування. 
У зв’язку з цим, інколи ви не хочете закривати весь клас, а просто хочете запобігти перевизначення похідними типами конкретних віртуальних методів. Наприклад, припустімо, що ви не хочете, щоб продавці, які працюють неповний робочий день, отримували індивідуальні бонуси. Щоб запобігти заміні класом PTSalesPerson віртуального методу GiveBonus(), ви можете ефективно закріпити цей метод у класі SalesPerson таким чином:

```cs
class SalesPerson7 : Employee7
{

    // ...

    public override sealed void GiveBonus(float amount)
    {
        // ...
    }

    // ...

}
```
Тут SalesPerson справді перевизначив віртуальний метод GiveBonus(), визначений у класі Employee; однак він явно позначив його як запечатаний. Таким чином, якщо ви спробуєте перевизначити цей метод у класі PtSalesPerson, ви отримаєте помилки під час компіляції, як показано в наступному коді:

```cs
sealed class PtSalesPerson7 : SalesPerson7
{
    // Compiler error! Can't override this method
    // in the PTSalesPerson class, as it was sealed.
    public override void GiveBonus(float amount)
    {
        base.GiveBonus(0);
    }
}

```