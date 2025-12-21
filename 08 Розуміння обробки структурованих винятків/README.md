# Розуміння обробки структурованих винятків

# Помилки, багі та винятки (errors,bugs and exceptions)

Жоден програміст не є ідеальним. Написання програмного забезпечення є складною справою, і, враховуючи цю складність, досить часто навіть найкраще програмне забезпечення поставляється з різними проблемами. Іноді причиною проблеми є неправильний код (наприклад, переповнення меж масиву). В інших випадках проблема спричинена неправільним введенням користувача, яке не було враховано в базі коду програми (наприклад, поле введення номера телефону, призначене значенню Chucky). Тепер, незалежно від причини проблеми, кінцевим результатом є те, що програма не працює належним чином. Щоб допомогти сформулювати майбутнє обговорення структурованої обробки винятків, дозвольте мені надати визначення для трьох часто використовуваних термінів, орієнтованих на аномалії.

    Помилки(bugs): це, простіше кажучи, помилки, зроблені програмістом. Наприклад, припустімо, що ви програмуєте за допомогою некерованого C++. Якщо вам не вдалося видалити динамічно виділену пам’ять, що призвело до витоку пам’яті, у вас є помилка.

    Помилки користувача: помилки користувача, з іншого боку, зазвичай спричинені особою, яка запускає вашу програму, а не тими, хто її створив. Наприклад, кінцевий користувач, який вводить неправильний рядок у текстове поле, цілком може створити помилку, якщо ви не впораєтеся з цим помилковим введенням у своїй базі коду.

    Винятки(exceptions): Винятки зазвичай розглядаються як аномалії виконання, які важко, якщо не неможливо, врахувати під час програмування програми. Серед можливих винятків — спроба підключитися до бази даних, яка більше не існує, відкриття пошкодженого XML-файлу або спроба зв’язатися з комп’ютером, який наразі офлайн. У кожному з цих випадків програміст (або кінцевий користувач) мало контролює ці «виняткові» обставини.

Враховуючи ці визначення, має бути зрозуміло, що структурована обробка винятків .NET є технікою роботи з винятками під час виконання. Однак навіть для помилок і помилок користувача, які ви не побачили, середовище виконання часто генерує відповідний виняток, який ідентифікує проблему. Як кілька прикладів, бібліотеки базових класів .NET визначають численні винятки, такі як FormatException, IndexOutOfRangeException, FileNotFoundException, ArgumentOutOfRangeException і так далі. У межах номенклатури .NET винятки враховують помилки, фальшиві дані користувача та помилки під час виконання, навіть якщо програмісти можуть розглядати кожну з них як окрему проблему. Однак перш ніж я зайду занадто далеко вперед, давайте формалізуємо роль структурованої обробки винятків і перевіримо, чим вона відрізняється від традиційних методів обробки помилок.

Щоб приклади коду, були максимально зрозумілими, я не буду відловлювати всі можливі винятки, які можуть бути викинуті даним методом у бібліотеках базових класів. Звичайно, у ваших проектах виробничого рівня ви повинні широко використовувати методи, представлені в цьому розділі.

## Роль обробки винятків .NET

До .NET обробка помилок в операційній системі Windows була плутаною сумішшю методів. Багато програмістів використовували власну логіку обробки помилок у контексті певної програми. Наприклад, команда розробників може визначити набір числових констант, які представляють відомі умови помилки, і використовувати їх як значення, що повертаються методом. Як приклад, розглянемо такий частковий код C:

```c
/* A very C-style error trapping mechanism. */
#define E_FILENOTFOUND 1000
int UseFileSystem()
{
  // Assume something happens in this function
  // that causes the following return value.
  return E_FILENOTFOUND;
}
void main()
{
  int retVal = UseFileSystem();
  if(retVal == E_FILENOTFOUND)
    printf('Cannot find file...');
}
```
Цей підхід далеко не ідеальний, враховуючи, що константа E_FILENOTFOUND є трохи більше, ніж числове значення, і далеко не є корисним агентом щодо вирішення проблеми. В ідеалі ви хотіли б обернути назву помилки, описове повідомлення та іншу корисну інформацію про цю умову помилки в єдиний чітко визначений пакет (саме це відбувається за структурованої обробки винятків).
Очевидною проблемою цих старих методів є величезна відсутність симетрії. Кожен підхід більш-менш пристосований до даної технології, даної мови і, можливо, навіть даного проекту. Щоб покласти край цьому безумству, платформа .NET надає стандартну техніку для надсилання та перехоплення помилок виконання: структуровану обробку винятків. Принадність цього підходу полягає в тому, що розробники тепер мають уніфікований підхід до обробки помилок, який є спільним для всіх мов, орієнтованих на платформу .NET. 
Як додатковий бонус, синтаксис, який використовується для викиду та перехоплення винятків між збірками та межами машини, ідентичний. Наприклад, якщо ви використовуєте C# для створення служби ASP.NET Core RESTful, ви можете викликати помилку JSON для віддаленого абонента, використовуючи ті самі ключові слова, які дозволяють створювати виняток між методами в одній програмі. 
Ще одна перевага винятків .NET полягає в тому, що замість отримання загадкового числового значення винятки є об’єктами, які містять зрозумілий людині опис проблеми, а також детальний знімок стека викликів, який ініціював виняток. Крім того, ви можете надати кінцевому користувачеві довідкову інформацію, яка спрямовує користувача на URL-адресу, яка містить докладні відомості про помилку, а також спеціальні дані, визначені програмістом.

## Будівельні блоки обробки винятків .NET

Програмування зі структурованою обробкою винятків передбачає використання чотирьох взаємопов’язаних сутностей.

1. Тип класу, який представляє деталі винятку
2. Член, який викидає екземпляр класу винятків для викликаючого кода за корректних обставин
3. Блок коду на стороні абонента, який викликає член, схильний до винятків
4. Блок коду на стороні абонента, який обробить (або перехопить) виняткову ситуацію, якщо вона станеться

Мова програмування C# пропонує п’ять ключових слів (try, catch, throw, finally і when), які дозволяють створювати та обробляти винятки. Об’єкт, який представляє проблему, що розглядається, є класом, що розширює System.Exception (або його нащадком). Враховуючи цей факт, давайте перевіримо роль цього базового класу, орієнтованого на винятки.

# Базовий клас System.Exception

Усі винятки зрештою походять від базового класу System.Exception. Ось основне визначення цього класу (зауважте, що деякі з цих членів є віртуальними і тому можуть бути перевизначені похідними класами):

```cs
public class Exception : ISerializable
{
  // Public constructors
  public Exception(string message, Exception innerException);
  public Exception(string message);
  public Exception();

  // Properties
  public virtual IDictionary Data { get; }
  public virtual string HelpLink { get; set; }
  public int HResult {get;set;}
  public Exception InnerException { get; }
  public virtual string Message { get; }
  public virtual string Source { get; set; }
  public virtual string StackTrace { get; }
  public MethodBase TargetSite { get; }
...
  // Methods
  public virtual Exception GetBaseException();
  public virtual void GetObjectData(SerializationInfo info,
    StreamingContext context);
}
```
Як бачите, багато властивостей, визначених System.Exception, за своєю природою доступні лише для читання. Це пояснюється тим, що похідні типи зазвичай надають значення за замовчуванням для кожної властивості. Наприклад, типовим повідомленням типу IndexOutOfRangeException є «Індекс вийшов за межі масиву». Таблиця описує найважливіші елементи System.Exception.

Основні члени типу System.Exception

|Властивість System.Exception|Опис|
|----------------------------|----|
|Data|Ця властивість лише для читання отримує колекцію пар ключ-значення (представлених об’єктом, що реалізує IDictionary), які надають додаткову, визначену програмістом інформацію про виняток.За замовчуванням ця колекція порожня. |
|HelpLink|Ця властивість отримує або встановлює URL-адресу файлу довідки або веб-сайту з повним описом помилки.|
|InnerException|Цю властивість лише для читання можна використовувати для отримання інформації про попередні винятки, які спричинили виникнення поточного винятку.Попередні винятки записуються шляхом їх передачі в конструктор останнього винятку.|
|Message|Ця властивість лише для читання повертає текстовий опис даної помилки.Саме повідомлення про помилку встановлюється як параметр конструктора.|
|Source|Ця властивість отримує або встановлює назву збірки або об’єкта, який викликав поточний виняток.|
|StackTrace|Ця властивість лише для читання містить рядок, який визначає послідовність викликів, які викликали виняткову ситуацію.Як ви могли здогадатися, ця властивість корисна під час налагодження або якщо ви хочете записати помилку у зовнішній журнал помилок.|
|TargetSite|Ця властивість лише для читання повертає об’єкт MethodBase, який описує численні подробиці про метод, який викликав виняток (виклик ToString() ідентифікує метод за назвою).|

## Найпростіший можливий приклад

Щоб проілюструвати корисність структурованої обробки винятків, вам потрібно створити клас, який створить виняток за правильних (або, можна сказати, виняткових) обставин. Припустімо, що ви створили новий проект консольної програми під назвою SimpleException, який визначає два типи класів (Car і Radio), пов’язані за допомогою зв’язку «has-a». Тип Radio визначає один метод увімкнення або вимкнення радіо.

```cs
namespace SimpleException;

class Radio
{
    public void TurnOn(bool on)
    {        
        Console.WriteLine(on ? "Jamming..." : "Quiet time...");
    }
}

```
На додаток до використання класу Radio через стримування/делегування, клас Car (показаний далі) визначається таким чином, що якщо користувач прискорює об’єкт Car понад попередньо визначену максимальну швидкість (зазначену за допомогою постійної змінної-члена з назвою MaxSpeed), його двигун вибухає, роблячи автомобіль непридатним для використання (переймається приватною змінною-членом bool під назвою _carIsDead).

```cs
namespace SimpleException;

class Car
{
    // Constant for maximum speed.
    public const int MaxSpeed = 100;

    // Car properties.
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "NoName";

    // Is the car still operational?
    private bool _carIsDead;

    private readonly Radio _radio = new Radio();

    public Car()
    {
    }
    public Car(int currentSpeed, string petName)
    {
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public void CrankTunes(bool state)
    {
        // Delegate request to inner object.
        _radio.TurnOn(state);
    }
    //Change current speed.
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                Console.WriteLine($"{PetName} has overheated!");
                CurrentSpeed = 0;
                _carIsDead = true;
            }
            else
            {
                Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
            }

        }

    }
}
```
Далі оновіть файл Program.cs, щоб змусити об’єкт Car перевищити попередньо визначену максимальну швидкість (встановлену на 100 у класі Car), як показано тут:

```cs
void TheSimplestPossibleExample()
{
    Car car = new Car("Zippy",20);
    car.CrankTunes(true);
    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(10);
    }
}
TheSimplestPossibleExample();
```
```
Jamming...
        CurrentSpeed = 30
        CurrentSpeed = 40
        CurrentSpeed = 50
        CurrentSpeed = 60
        CurrentSpeed = 70
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Zippy has overheated!
Zippy is out of order...
```

## Створення загального винятку

Тепер, коли у вас є функціональний клас Car, я покажу найпростіший спосіб створення винятку. Поточна реалізація Accelerate() просто відображає повідомлення про помилку, якщо абонент намагається пришвидшити автомобіль понад його верхню межу. Щоб модифікувати цей метод для створення винятку, якщо користувач намагається пришвидшити автомобіль після того, як він досяг значення за вимог виробника, потрібно створити та налаштувати новий екземпляр класу System.Exception, встановивши значення властивості Message тільки для читання через конструктор класу. Якщо ви хочете надіслати об’єкт винятку назад абоненту, використовуйте оператор C# throw(). Ось відповідне оновлення коду для методу Accelerate():

```cs
class Car1
{
    //...
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                // Use the 'throw' keyword to raise an exception.
                throw new Exception($"{PetName} has overheated!");
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
}

```
Перш ніж досліджувати, як абонент уловить цей виняток, давайте розглянемо кілька цікавих моментів. По-перше, коли ви створюєте виняток, ви завжди вирішуєте, що саме є помилкою, про яку йде мова, і коли має бути створено виняток. Тут ви робите припущення, що якщо програма намагається збільшити швидкість об’єкта Car понад максимальну, має бути викинуто об’єкт System.Exception, щоб вказати, що метод Accelerate() не може продовжуватися (що може бути або не бути валідним припущенням; це буде судження з вашого боку на основі програми, яку ви створюєте).
Крім того, ви можете застосувати Accelerate() для автоматичного відновлення без необхідності створювати виняткову ситуацію. Загалом, винятки слід створювати лише тоді, коли виконується термінальна умова (наприклад, не знайти потрібний файл, не вдається підключитися до бази даних тощо), а не використовувати як механізм логічного потоку. Рішення про те, що саме виправдовує створення винятку, є проблемою дизайну, з якою ви завжди повинні боротися. Для поточних цілей припустимо, що запит приреченого автомобіля збільшити швидкість є причиною для створення винятку.
По-друге, зверніть увагу на те, як з методу було видалено фінальний else. Коли створюється виняток (фреймворком або вручну за допомогою оператора throw(), керування повертається до викликаного методу (або блоку catch у спробі catch)). Це усуває потребу в остаточному else. Чи залишати це для зручності читання, вирішувати вам і вашим стандартам кодування.
У будь-якому випадку, виклик нового методу, використовуючи попередню логіку, зрештою буде створено виняток.
Як показано в наведених нижче виводах, результат необроблення цієї помилки є менш ніж ідеальним, враховуючи, що ви отримуєте докладний дамп помилки з наступним завершенням програми (із вашим конкретним шляхом до файлу та номерами рядків):

```cs
void ThrowingAGeneralException()
{
    Car1 car = new Car1("Zippy", 20);
    car.CrankTunes(true);
    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(10);
    }
}
ThrowingAGeneralException();
```
```
Jamming...
        CurrentSpeed = 30
        CurrentSpeed = 40
        CurrentSpeed = 50
        CurrentSpeed = 60
        CurrentSpeed = 70
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Unhandled exception. System.Exception: Zippy has overheated!
   at SimpleException.Car1.Accelerate(Int32 delta) in D:\...\SimpleException\Car1.cs:line 47
```

## Перехоплення винятків

Оскільки метод Accelerate() тепер створює виняток, абонент повинен бути готовий обробити виняток, якщо він трапиться. Коли ви викликаєте метод, який може викликати виняток, ви використовуєте блок try/catch. Після того, як ви перехопили об’єкт винятку, ви можете викликати члени об’єкта винятку, щоб отримати деталі проблеми. Що ви будете робити з цими даними, вирішувати в основному вам. Ви можете записати цю інформацію у файл звіту, записати дані в журнал подій, надіслати електронного листа системному адміністратору або показати проблему кінцевому користувачеві. Тут ви просто виведете вміст у вікно консолі:

```cs
void CatchingExceptions()
{
    Car1 car = new Car1("Zippy", 20);
    car.CrankTunes(true);
    try
    {
        // Speed up past the car's max speed to
        // trigger the exception.
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine($"TargetSite: {ex.TargetSite}");
        Console.WriteLine($"Message: {ex.Message}");
        Console.WriteLine($"Source: {ex.Source}");
    }
    Console.WriteLine("\tOut of try/catch");
}
CatchingExceptions();
```
```
Jamming...
        CurrentSpeed = 30
        CurrentSpeed = 40
        CurrentSpeed = 50
        CurrentSpeed = 60
        CurrentSpeed = 70
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100

         Exceptions
TargetSite: Void Accelerate(Int32)
Message: Zippy has overheated!
Source: SimpleException
        Out of try/catch
```
По суті, блок try — це розділ інструкцій, які можуть викликати виняток під час виконання. Якщо виявляється виняток, потік виконання програми надсилається до відповідного блоку catch. З іншого боку, якщо код у блоці try не викликає виняток, блок catch повністю пропускається, і все в порядку. Як бачите, після обробки виняткової ситуації програма може продовжити роботу з точки після блоку catch. За деяких обставин певний виняток може бути достатньо критичним, щоб вимагати припинення програми. Однак у багатьох випадках логіка в обробнику винятків гарантує, що програма може продовжувати свій шлях (хоча це може бути трохи менш функціональним, наприклад, неможливо підключитися до віддаленого джерела даних). 

throw() також доступний як вираз throw і може бути викликаний будь-де, де дозволено вирази. Після цього робота програми припиняється.

## Налаштування стану винятку

На даний момент об’єкт System.Exception, налаштований у методі Accelerate(), просто встановлює значення, доступне для властивості Message (через параметр конструктора). Однак, як було показано раніше, клас Exception також надає ряд додаткових членів (TargetSite, StackTrace, HelpLink і Data), які можуть бути корисними для подальшого визначення характеру проблеми. Щоб покращити поточний приклад, давайте розглянемо додаткові відомості про цих членів у кожному конкретному випадку.

### Властивість TargetSite

Властивість System.Exception.TargetSite дозволяє визначити різні деталі про метод, який створив певний виняток. Як показано в попередньому прикладі коду, друк значення TargetSite відобразить тип повернення, назву та типи параметрів методу, який викликав виняток. Однак TargetSite повертає не лише рядок із присмаком ванілі, а радше строго типізований об’єкт System.Reflection.MethodBase. Цей тип може бути використаний для збору численних деталей щодо методу порушення, а також класу, який визначає метод порушення. Цей тип може бути використаний для збору численних деталей щодо методу порушення, а також класу, який визначає метод порушення. Для ілюстрації припустимо, що попередню логіку catch було оновлено наступним чином:

```cs
void TheTargetSiteProperty()
{
    Car1 car = new Car1("Zippy", 80);
    try
    {
        // Speed up past the car's max speed to
        // trigger the exception.
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine($"TargetSite: {ex.TargetSite}");
        Console.WriteLine($"TargetSite.DeclaringType: {ex.TargetSite!.DeclaringType}");
        Console.WriteLine($"TargetSite.MemberType: {ex.TargetSite.MemberType}");
    }
}
TheTargetSiteProperty();
```
```
        CurrentSpeed = 90
        CurrentSpeed = 100

         Exceptions
TargetSite: Void Accelerate(Int32)
TargetSite.DeclaringType: SimpleException.Car1
TargetSite.MemberType: Method
```
Цього разу ви використовуєте властивість MethodBase.DeclaringType, щоб визначити повне ім’я класу, який викликав помилку (у цьому випадку SimpleException.Car1), а також властивість об’єкта MethodBase, щоб визначити тип члена (наприклад, властивість чи метод), з якого виникла ця виняткова ситуація.

### Властивість StackTrace

Властивість System.Exception.StackTrace дозволяє визначити серію викликів, які призвели до винятку. Майте на увазі, що ви ніколи не встановлюєте значення StackTrace, оскільки воно встановлюється автоматично під час створення винятку. Для ілюстрації припустімо, що ви ще раз оновили свою логіку catch.

```cs
void TheStackTraceProperty()
{
    Car1 car = new Car1("Zippy", 80);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine($"StackTrace: {ex.StackTrace}");
    }
}
TheStackTraceProperty();
```
```
        CurrentSpeed = 90
        CurrentSpeed = 100

         Exceptions
StackTrace:    at SimpleException.Car1.Accelerate(Int32 delta) in D:\...\UnderstandingExceptions\SimpleException\Car1.cs:line 54
   at Program.<<Main>$>g__TheStackTraceProperty|0_4() in D:\MyWork\CS-Step-by-Step\...\UnderstandingExceptions\SimpleException\Program.cs:line 80
```
Рядок, повернутий із StackTrace, документує послідовність викликів, які призвели до викиду цього винятку. Зверніть увагу, як номер самого нижнього рядка цього рядка визначає перший виклик у послідовності, тоді як номер верхнього рядка визначає точне розташування учасника-порушника. Зрозуміло, що ця інформація може бути дуже корисною під час налагодження чи журналювання певної програми, оскільки ви можете «стежити за потоком» походження помилки.

### Властивість HelpLink

Хоча властивості TargetSite і StackTrace дозволяють програмістам отримати розуміння певного винятку, ця інформація мало корисна для кінцевого користувача. Як ви вже бачили, властивість System.Exception.Message можна використовувати для отримання зрозумілої людині інформації, яку можна відобразити поточному користувачеві. Крім того, можна встановити властивість HelpLink, щоб спрямовувати користувача на певну URL-адресу або стандартний файл довідки, який містить більш детальну інформацію. 
За замовчуванням значенням, керованим властивістю HelpLink, є порожній рядок. Оновіть виняток за допомогою ініціалізації об’єкта, щоб надати більш цікаве значення. Ось відповідні оновлення методу Car.Accelerate():

```cs
public void Accelerate(int delta)
{
    if (_carIsDead)
    {
        Console.WriteLine($"{PetName} is out of order...");
    }
    else
    {
        CurrentSpeed += delta;
        if (CurrentSpeed > MaxSpeed)
        {
            CurrentSpeed = 0;
            _carIsDead = true;

            // Use the 'throw' keyword to raise an exception.
            throw new Exception($"{PetName} has overheated!")
            {
                HelpLink = "https://group.mercedes-benz.com/en/"
            };
            
        }
        Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
    }
}
```
Тепер логіку catch можна оновити, щоб надрукувати цю інформацію довідкового посилання наступним чином:

```cs
void TheHelpLinkProperty()
{
    Car1 car = new Car1("Zippy", 80);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine($"HelpLink: {ex.HelpLink}");
    }
}
TheHelpLinkProperty();
```
```
        CurrentSpeed = 90
        CurrentSpeed = 100

         Exceptions
HelpLink: https://group.mercedes-benz.com/en/
```

### Властивість Data

Властивість Data у System.Exception дозволяє заповнити об’єкт винятку відповідною допоміжною інформацією (наприклад, міткою часу timestamp). Властивість Data повертає об’єкт, що реалізує інтерфейс під назвою IDictionary, визначений у просторі імен System.Collections. Колекції словників дозволяють створювати набір значень, які витягуються за допомогою певного ключа. Зверніть увагу на наступне оновлення методу Car.Accelerate():

```cs
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                // Use the 'throw' keyword to raise an exception.
                throw new Exception($"{PetName} has overheated!")
                {
                    HelpLink = "https://group.mercedes-benz.com/en/",
                    Data =
                    {
                        {"Timestamp",DateTime.Now.ToString()},
                        {"Cause","You have a lead foot." }
                    }
                };
                
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
```
Для успішного перерахування пар ключ-значення переконайтеся, що ви додали директиву using для простору імен System.Collections, оскільки ви використовуватимете тип DictionaryEntry у файлі, що містить клас, що реалізує ваші оператори верхнього рівня.
```
using System.Collections;
```
Далі вам потрібно оновити логіку catch, щоб перевірити, чи значення, яке повертає властивість Data, не є null (значення за замовчуванням). Після цього ви використовуєте властивості Key і Value типу DictionaryEntry, щоб надрукувати спеціальні дані на консолі.

```cs
void TheDataProperty()
{
    Car1 car = new Car1("Zippy", 80);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("\n\t Exceptions");
        Console.WriteLine("\nCustom Data:");
        foreach (DictionaryEntry de in ex.Data)
        {
            Console.WriteLine($"{de.Key} {de.Value}");
        }
    }
}
TheDataProperty();
```
```
        CurrentSpeed = 90
        CurrentSpeed = 100

         Exceptions

Custom Data:
Timestamp 06.02.2025 10:44:07
Cause You have a lead foot.
```
Властивість Data корисна тим, що вона дозволяє вам запакувати спеціальну інформацію щодо наявної помилки, не вимагаючи створення нового типу класу для розширення базового класу Exception. Незважаючи на те, що властивість Data може бути корисною, розробники все ще часто створюють строго типізовані класи винятків, які обробляють спеціальні дані за допомогою строго типізованих властивостей.
Такий підхід дозволяє розробнику вловлювати певний тип, отриманий від винятків, замість того, щоб копатися в колекції даних, щоб отримати додаткові деталі. Щоб зрозуміти, як це зробити, вам потрібно вивчити різницю між винятками рівня системи та рівня програми.

# Винятки системного рівня (System.SystemException)

Бібліотеки базових класів .NET визначають багато класів, які зрештою походять від System.Exception. Наприклад, простір імен System визначає основні виняткові об’єкти, такі як ArgumentOutOfRangeException, IndexOutOfRangeException, StackOverflowException тощо. Інші простори імен визначають винятки, які відображають поведінку цього простору імен. Наприклад, System.Drawing.Printing визначає винятки друку, System.IO визначає винятки на основі вводу/виводу, System.Data визначає винятки, орієнтовані на базу даних, і так далі.
Винятки, створені платформою .NET, (відповідно) називаються системними винятками. Ці винятки, як правило, вважаються невиправними фатальними помилками. Системні винятки походять безпосередньо від базового класу під назвою System.SystemException, який, у свою чергу, походить від System.Exception.

```cs
public class SystemException : Exception
{
  // Various constructors.
}
```
З огляду на те, що тип System.SystemException не додає жодної функціональності, окрім набору спеціальних конструкторів, ви можете задатися питанням, чому взагалі існує SystemException. Простіше кажучи, коли тип винятку походить від System.SystemException, ви можете визначити, що середовище виконання .NET є об’єктом, який викликав виключення, а не базовий код програми, що виконується. Ви можете перевірити це досить просто, використовуючи ключове слово is.

```cs
void SystemLevelExceptions()
{
    // True! NullReferenceException is-a SystemException.
    NullReferenceException nullRefEx = new NullReferenceException();
    Console.WriteLine($"NullReferenceException is-a SystemException? : " +
        $"{nullRefEx is SystemException}");
}
SystemLevelExceptions();
```
```
NullReferenceException is-a SystemException? : True
```

# Винятки на рівні програми (System.ApplicationException)

Враховуючи, що всі винятки .NET є типами класів, ви можете створювати власні винятки для конкретної програми. Однак, оскільки базовий клас System.SystemException представляє винятки, викинуті із середовища виконання, ви, природно, можете припустити, що ви повинні отримувати власні винятки з типу System.Exception. Ви можете зробити це, але натомість ви можете отримати від класу System.ApplicationException.

```cs
public class ApplicationException : Exception
{
  // Various constructors.
}
```
Подібно до SystemException, ApplicationException не визначає жодних додаткових членів, крім набору конструкторів. Функціонально єдина мета System.ApplicationException — визначити джерело помилки. Коли ви обробляєте виняток, що походить від System.ApplicationException, ви можете припустити, що виняток було викликано базою коду програми, що виконується, а не бібліотеками базових класів .NET або механізмом виконання .NET.

# Створення спеціальних винятків, приклад 1

Хоча ви завжди можете створювати екземпляри System.Exception, щоб повідомити про помилку виконання (як показано в першому прикладі), іноді корисно створювати строго типізоване виключення, яке представляє унікальні деталі вашої поточної проблеми. Наприклад, припустімо, що ви хочете створити спеціальний виняток (під назвою CarIsDeadException), щоб представити помилку прискорення приреченого автомобіля. Першим кроком є ​​створення нового класу з System.Exception/System.ApplicationException (за домовленістю всі імена класів винятків закінчуються суфіксом Exception).
Як правило, усі спеціальні класи винятків мають бути визначені як загальнодоступні класи (нагадаємо, що модифікатор доступу за замовчуванням для невкладеного типу є internal). Причина полягає в тому, що винятки часто передаються за межі збірки, і тому вони повинні бути доступні для бази коду виклику.

Створіть новий проект консольної програми під назвою CustomException, скопіюйте файли Car.cs і Radio.cs.

```cs
namespace CustomException;
class Radio
{
    public void TurnOn(bool on)
    {
        Console.WriteLine(on ? "Jamming..." : "Quiet time...");
    }
}
```
```cs
namespace CustomException;

class Car
{
    private bool _carIsDead;

    public const int MaxSpeed = 100;
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "NoName";

    private readonly Radio _radio = new Radio();

    public Car()
    {
    }
    public Car(string petName, int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public void CrankTunes(bool state)
    {
        // Delegate request to inner object.
        _radio.TurnOn(state);
    }
    //Change current speed.
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                throw new Exception($"{PetName} has overheated!");
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
}

```
Далі додайте новий файл під назвою CarIsDeadException1.cs і додайте таке визначення класу:

```cs
namespace CustomException1;
// This custom exception describes the details of the car-is-dead condition.
// (Remember, you can also simply extend Exception.)
public class CarIsDeadException1 : ApplicationException
{

}
```
Як і в будь-якому класі, ви можете включити будь-яку кількість користувальницьких членів, які можна викликати в блоці catch логіки виклику. Ви також можете перевизначити будь-які віртуальни члени, визначених батьківськими класами. Наприклад, ви можете реалізувати CarIsDeadException, перевизначивши віртуальну властивість Message. 

```cs
namespace CustomException;
// This custom exception describes the details of the car-is-dead condition.
// (Remember, you can also simply extend Exception.)
public class CarIsDeadException1 : ApplicationException
{
    private string _messageDetails = string.Empty;
    public DateTime ErrorTimeStamp { get; set; }
    public string CauseOfError { get; set; } = null!;


    public CarIsDeadException1(string message, DateTime time, string cause)
    {
        _messageDetails = message;
        ErrorTimeStamp = time;
        CauseOfError = cause;
    }
    public CarIsDeadException1()
    {
    }

    // Override the Exception.Message property.
    public override string Message => $"Car Error Message:{_messageDetails}";
}
```
Тут клас CarIsDeadException підтримує приватне поле (_messageDetails), яке представляє дані щодо поточного винятку, який можна встановити за допомогою спеціального конструктора. Викидання цього винятку з методу Accelerate() є простим. Просто виділіть, налаштуйте та створіть тип CarIsDeadException, а не System.Exception.

```cs
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                throw new CarIsDeadException1(
                    $"{PetName} has overheated!",
                    DateTime.Now, 
                    "You have a lead foot"
                    );
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
```
Щоб перехопити цей вхідний виняток, вашу область перехоплення тепер можна оновити, щоб перехопити певний тип CarIsDeadException (проте, враховуючи, що CarIsDeadException «є» System.Exception, можна також перехопити System.Exception).

```cs
void CatchingExceptions1()
{
    Car car = new Car("Zippy", 20);
    car.CrankTunes(true);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException1 ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.ErrorTimeStamp);
        Console.WriteLine(ex.CauseOfError);
    }

}
CatchingExceptions1();
```
```
Jamming...
        CurrentSpeed = 30
        CurrentSpeed = 40
        CurrentSpeed = 50
        CurrentSpeed = 60
        CurrentSpeed = 70
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Car Error Message:Zippy has overheated!
06.02.2025 17:00:04
You have a lead foot
```
Отже, тепер, коли ви розумієте основний процес створення спеціального винятку, настав час розвинути ці знання.

## Створення спеціальних винятків, приклад 2

Поточний тип CarIsDeadException замінив віртуальну властивість System.Exception.Message, щоб налаштувати спеціальне повідомлення про помилку, і надав дві спеціальні властивості для обліку додаткових шматочків даних. Насправді, однак, вам не потрібно перевизначати віртуальну властивість Message, оскільки ви можете просто передати вхідне повідомлення конструктору батьківського елемента наступним чином:

```cs
namespace CustomException;

public class CarIsDeadException2 :ApplicationException
{
    public DateTime ErrorTimeStamp { get; set; }
    public string CauseOfError { get; set; } = null!;

    public CarIsDeadException2(string message, DateTime errorTimeStamp, string causeOfError)
        :base(message)
    {
        ErrorTimeStamp = errorTimeStamp;
        CauseOfError = causeOfError;
    }
    public CarIsDeadException2()
    {
    }
}
```
Зверніть увагу, що цього разу ви не визначили рядкову змінну для представлення повідомлення та не перевизначили властивість Message. Швидше, ви просто передаєте параметр своєму конструктору базового класу. Завдяки такому дизайну користувацький клас винятків — це не більше, ніж клас з унікальною назвою, що походить від System.ApplicationException (з додатковими властивостями, якщо це доречно), позбавлений будь-яких перевизначень базового класу.

```cs
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                throw new CarIsDeadException2(
                    $"{PetName} has overheated!",DateTime.Now, "You have a lead foot");
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
```
```cs
void CatchingExceptions2()
{
    Car car = new Car("Zippy", 70);
    car.CrankTunes(true);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException2 ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.ErrorTimeStamp);
        Console.WriteLine(ex.CauseOfError);
    }
}
CatchingExceptions2();
```
```
Jamming...
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Zippy has overheated!
06.02.2025 17:19:39
You have a lead foot
```
Не дивуйтеся, якщо більшість (якщо не всі) ваших користувальницьких класів винятків слідують цьому простому шаблону. Багато разів роль спеціального винятку полягає не обов’язково в наданні додаткової функціональності, окрім того, що успадковується від базових класів, а в тому, щоб надати чітко названий тип, який чітко визначає природу помилки, щоб клієнт міг надати різну логіку обробки для різних типів винятків.

## Створення спеціальних винятків, приклад 3

Якщо ви хочете побудувати справді первинний і правильний спеціальний клас винятків, ви хочете переконатися, що ваш спеціальний виняток виконує наступне:

1. Походить від Exception/ApplicationException
2. Визначає конструктор за замовчуванням
3. Визначає конструктор, який встановлює успадковану властивість Message
4. Визначає конструктор для обробки «inner exceptions»

Щоб завершити ваше дослідження побудови спеціальних винятків, ось остання ітерація CarIsDeadException, яка враховує кожен із цих спеціальних конструкторів (властивості будуть такими, як показано в попередньому прикладі):

```cs
namespace CustomException;

public class CarIsDeadException3 : ApplicationException
{
    public DateTime ErrorTimeStamp { get; set; }
    public string CauseOfError { get; set; } = null!;

    public CarIsDeadException3() { }
    public CarIsDeadException3(DateTime time, string cause) 
        : this(string.Empty, time, cause, null)
    {
    }
    public CarIsDeadException3(string? message, DateTime time,
    string cause) : this(message, time, cause, null)
    {
    }
    public CarIsDeadException3(string? message, DateTime time,
    string cause, Exception? innerException) : base(message, innerException)
    {
        ErrorTimeStamp = time;
        CauseOfError = cause;
    }
}
```
За допомогою цього оновлення вашого спеціального винятку оновіть метод Accelerate() до такого:

```cs
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                throw new CarIsDeadException3(
                    $"{PetName} has overheated!",
                    DateTime.Now, 
                    "You have a lead foot");
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
```
З огляду на те, що створення користувацьких винятків, які відповідають передовим практикам .NET, справді відрізняється лише назвою, ви будете раді дізнатися, що Visual Studio надає шаблон фрагмента коду під назвою Exception, який автоматично створить новий клас винятків, який відповідає передовим практикам .NET.

```cs
void CatchingExceptions3()
{
    Car car = new Car("Zippy", 80);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException3 ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.ErrorTimeStamp);
        Console.WriteLine(ex.CauseOfError);
    }
}
CatchingExceptions3();
```
```
        CurrentSpeed = 90
        CurrentSpeed = 100
Zippy has overheated!
07.02.2025 13:48:13
You have a lead foot
```

# Обробка кількох винятків

У своїй найпростішій формі блок try має один блок catch. Однак насправді ви часто стикаєтеся з ситуаціями, коли оператори в блоці try можуть викликати численні можливі винятки. Створіть новий проект консольної програми C# під назвою ProcessMultipleExceptions. Додайте файли Car.cs, Radio.cs і CarIsDeadException.cs.


```cs

namespace ProcessMultipleExceptions;

class Car
{
    private bool _carIsDead;

    public const int MaxSpeed = 100;
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "NoName";

    private readonly Radio _radio = new Radio();

    public Car()
    {
    }
    public Car(string petName, int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public void CrankTunes(bool state)
    {
        // Delegate request to inner object.
        _radio.TurnOn(state);
    }
    //Change current speed.
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                throw new CarIsDeadException(
                    $"{PetName} has overheated!", DateTime.Now, "You have a lead foot");
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
}
```
```cs
namespace ProcessMultipleExceptions;

public class CarIsDeadException : ApplicationException
{
    public DateTime ErrorTimeStamp { get; set; }
    public string CauseOfError { get; set; } = null!;

    public CarIsDeadException(DateTime time, string cause)
        : this(string.Empty, time, cause, null)
    {
    }
    public CarIsDeadException(string? message, DateTime time,
    string cause) : this(message, time, cause, null)
    {
    }
    public CarIsDeadException(string? message, DateTime time,
    string cause, Exception? innerException) : base(message, innerException)
    {
        ErrorTimeStamp = time;
        CauseOfError = cause;
    }
    public CarIsDeadException()
    {
    }
}
```
Тепер оновіть метод Accelerate() Car, щоб також викидати попередньо визначену бібліотеку базових класів ArgumentOutOfRangeException, якщо ви передаєте недійсний параметр (який, як ви можете вважати, є будь-яким значенням, меншим за нуль). Зауважте, що конструктор цього класу винятків приймає ім’я аргументу-порушника як перший рядок, а потім повідомлення з описом помилки.

```cs
    public void Accelerate(int delta)
    {
        if (delta < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(delta),
                "Speed must be greater than zero");
        }
        //...
    }    
```
Оператор nameof() повертає рядок, що представляє назву об’єкта, у цьому прикладі змінну delta. Це безпечніший спосіб посилатися на об’єкти, методи та змінні C#, коли потрібна рядкова версія.

Логіка catch тепер може конкретно реагувати на кожен тип винятку.

```cs
void CatchingExceptions4()
{
    Car car = new Car("Rusty", 90);
    try
    {
        // Trip Arg out of range exception.
        car.Accelerate(-10);
    }
    catch (CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch(ArgumentOutOfRangeException ex)  
    {
        Console.WriteLine(ex.Message);
    }
}
CatchingExceptions4();
```
```
Speed must be greater than zero (Parameter 'delta')
```
Коли ви створюєте кілька блоків catch, ви повинні знати, що коли виникає виняток, воно буде оброблено першим відповідним catch. Щоб точно проілюструвати, що означає «перший відповідний» catch, припустімо, що ви модернізували попередню логіку за допомогою додаткової області catch, яка намагається обробити всі винятки за межами CarIsDeadException і ArgumentOutOfRangeException, перехоплюючи загальний System.Exception наступним чином:
```cs
void CatchingExceptions5()
{
    Car car = new Car("Rusty", 90);
    try
    {
        // Trip Arg out of range exception.
        car.Accelerate(-10);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }   
    catch (CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message);
    }
}
CatchingExceptions5();
```
Ця логіка обробки винятків створює помилки під час компіляції. Проблема полягає в тому, що перший блок catch може обробляти все, що є похідним від System.Exception (враховуючи зв’язок «is-a»), включаючи типи CarIsDeadException і ArgumentOutOfRangeException. Таким чином, останні два catch блоки недоступні.
Основне правило, про яке слід пам’ятати, полягає в тому, щоб переконатися, що ваші блоки catch структуровані таким чином, щоб перший catch був найбільш конкретним винятком (тобто найбільш похідним типом у ланцюжку успадкування типу винятку), залишаючи остаточний catch для найзагальнішого (тобто базового класу даного ланцюжка успадкування винятків, у цьому випадку System.Exception).

Таким чином, якщо ви хочете визначити блок catch, який оброблятиме будь-які помилки, крім CarIsDeadException і ArgumentOutOfRangeException, ви можете написати наступне:

```cs
void CatchingExceptions6()
{
    Car car = new Car("Rusty", 70);
    try
    {
        car.Accelerate(50);
    }
    catch (CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message);
    }
    // This will catch any other exception
    // beyond CarIsDeadException or
    // ArgumentOutOfRangeException.
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
CatchingExceptions6();
```
```
Speed must be greater than zero (Parameter 'delta')
Rusty has overheated!
```
Там, де це взагалі можливо, завжди віддавайте перевагу відлову конкретних класів винятків, а не загального System.Exception. Хоча в короткостроковій перспективі може здатися, що це спрощує життя (ви можете подумати: «Ах! Це вловлює всі інші речі, які мене не цікавлять»), у довгостроковій перспективі ви можете отримати дивні збої під час виконання, оскільки серйознішу помилку не було безпосередньо вирішено у вашому коді. Пам’ятайте, що останній блок catch, який стосується System.Exception, як правило, є дуже загальним.

## Загальні оператори catch

C# також підтримує «загальну» область захоплення, яка явно не отримує об’єкт винятку, створений певним членом.

```cs
void CatchingExceptions7()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch 
    {
        Console.WriteLine("Something bad happened...");
    }
}
CatchingExceptions7();
```
```
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Something bad happened...
```
Очевидно, що це не найінформативніший спосіб обробки винятків, оскільки у вас немає способу отримати значущі дані про помилку, що сталася (наприклад, назву методу, стек викликів або спеціальне повідомлення). Тим не менш, C# допускає таку конструкцію, яка може бути корисною, коли ви хочете обробляти всі помилки загальним способом.

## Повторий виклик винятку

Коли ви перехоплюєте виняток, логіка в блоці try дозволяє повторно передавати виняток у стек викликів попередньому викликаючому коду. Для цього просто використовуйте throw у блоці catch. Це передає виняткову ситуацію вгору по ланцюжку логіки виклику, що може бути корисним, якщо ваш блок catch здатний лише частково обробити наявну помилку.

```cs
void CatchingExceptions8()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch(CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
        // Do any partial processing of this error and pass the buck.
        throw;
    }
    catch
    {
        Console.WriteLine("Something bad happened...");
    }
}
CatchingExceptions8();

```
```
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Rusty has overheated!
Unhandled exception. ProcessMultipleExceptions.CarIsDeadException: Rusty has overheated!
   at ProcessMultipleExceptions.Car.Accelerate(Int32 delta) in D:\...\Car.cs:line 54
   at Program.<<Main>$>g__CatchingExceptions8|0_4() in D:\...\Program.cs:line 96
   at Program.<Main>$(String[] args) in D:\...\Program.cs:line 110
```
Майте на увазі, що в цьому прикладі коду кінцевим приймачем CarIsDeadException є середовище виконання .NET, оскільки це оператори верхнього рівня, які повторно викликають виняток. Через це ваш кінцевий користувач отримує системне діалогове вікно помилки. Як правило, ви лише повторюєте частково оброблений виняток абоненту, який має можливість обробити вхідний виняток більш витончено. Зверніть також увагу, що ви явно не перекидаєте об’єкт CarIsDeadException, а скоріше використовуєте throw() без аргументів. Ви не створюєте новий об’єкт винятку; ви просто повторно створюєте оригінальний об’єкт винятку (з усією його вихідною інформацією). При цьому зберігається контекст вихідної мети.

## Внутрішні винятки

Як ви можете підозрювати, цілком можливо ініціювати виняток у той час, коли ви обробляєте інший виняток.Наприклад, припустімо, що ви обробляєте виняткову ситуацію CarIsDeadException у певній області catch і під час процесу ви намагаєтесь записати трасування стека у файл на вашому диску C: під назвою carErrors.txt (неявні глобальні оператори using надають вам доступ до простору імен System.IO та його типів, орієнтованих на введення-виведення).

```cs
void CatchingExceptions9()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
        FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);
    }
    catch (Exception ex) 
    {
        Console.WriteLine(ex.Message);     
    }     
 
}
CatchingExceptions9();
```
```
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Rusty has overheated!
Unhandled exception. System.IO.FileNotFoundException: Could not find file 'D:\carError.txt'.
File name: 'D:\carError.txt'
   at Microsoft.Win32.SafeHandles.SafeFileHandle.CreateFile(String fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options)
```
Тепер, якщо вказаний файл не знаходиться на вашому диску, виклик File.Open() призводить до FileNotFoundException. Далі в іншій главі ви дізнаєтеся все про простір імен System.IO, де ви дізнаєтеся, як програмним шляхом визначити, чи існує файл на жорсткому диску, перш ніж спробувати відкрити файл (таким чином повністю уникаючи винятку). Однак, щоб зосередитися на темі винятків, припустимо, що виняток було викинуто.
Коли ви стикаєтеся з винятком під час обробки іншого винятку, найкраща практика стверджує, що ви повинні записати новий об’єкт винятку як «inner exception» у новому об’єкті того самого типу, що й початковий виняток. Причина, по якій вам потрібно виділити новий об’єкт винятку, який обробляється, полягає в тому, що єдиний спосіб задокументувати внутрішній виняток — за допомогою параметра конструктора.

```cs
void CatchingExceptions10()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException ex1)
    {
        Console.WriteLine(ex1.Message);
        try
        {
            FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);
        }
        catch (Exception ex2)
        {
            //This causes a compile error-InnerException is read only
            //ex1.InnerException = ex2;

            // Throw an exception that records the new exception,
            // as well as the message of the first exception.
            throw new CarIsDeadException(ex1.Message, ex1.ErrorTimeStamp, ex1.CauseOfError,
                ex2);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
CatchingExceptions10();
```
```
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Rusty has overheated!
Unhandled exception. ProcessMultipleExceptions.CarIsDeadException: Rusty has overheated!
 ---> System.IO.FileNotFoundException: Could not find file 'D:\carError.txt'.
File name: 'D:\carError.txt'
```
Зауважте, що в цьому випадку я передав об’єкт FileNotFoundException як четвертий параметр конструктору CarIsDeadException. Зауважте, що в цьому випадку я передав об’єкт FileNotFoundException як четвертий параметр конструктору CarIsDeadException. Після того як ви налаштували цей новий об’єкт, ви перекидаєте його в стек викликів до наступного абонента, який у цьому випадку буде оператором верхнього рівня. Враховуючи те, що після операторів верхнього рівня немає «наступного абонента», щоб перехопити виняток, вам знову буде показано діалогове вікно помилки. Подібно до акту повторного виклику винятку, запис внутрішніх винятків зазвичай корисний лише тоді, коли абонент має можливість елегантно перехопити виняток із самого початку. Якщо це так, логіка catch абонента може використовувати властивість InnerException для отримання деталей внутрішнього об’єкта винятку.

```cs
void CatchingExceptions11()
{
    try
    {
        CatchingExceptions10();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.InnerException.Message);
    }
}
CatchingExceptions11();
```
```
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Rusty has overheated!
Could not find file 'D:\carError.txt'.
```

## Блок finally

Область дії try/catch також може визначати додатковий блок finally. Мета finally блоку - забезпечити, щоб набір операторів коду завжди буде виконувати, при винятку (будь -якого типу) чи ні. Щоб проілюструвати, припустимо, що ви хочете завжди вимикати радіо автомобіля, перш ніж вийти з програми, незалежно від будь -якого винятку.

```cs
void BlockFinally()
{
    Car car = new Car("Rusty", 70);
    car.CrankTunes(true);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch(CarIsDeadException ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.CauseOfError);
        FileStream fileStream = File.Open(@"D:\carError.txt", FileMode.Open);
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
        // This will always occur. Exception or not.
        car.CrankTunes(false);
    }
}
BlockFinally();
```
```
Jamming...
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Rusty has overheated!
You have a lead foot
Unhandled exception. System.IO.FileNotFoundException: Could not find file 'D:\carError.txt'.
File name: 'D:\carError.txt'
   at Microsoft.Win32.SafeHandles.SafeFileHandle.CreateFile(String fullPath, FileMode mode, FileAccess access, FileShare share, FileOptions options)
   ...
   \Program.cs:line 210
Quiet time...
```
Якщо ви не включили блок finally, радіо не вимкнеться, якщо виникне виняткова ситуація (що може бути або не бути проблемою).
У більш реальному сценарії, коли вам потрібно позбутися об’єктів, закрити файл або від’єднатися від бази даних (чи що завгодно), блок finally забезпечує розташування для належного очищення.

## Фільтри винятків

Ключове слово when можна розмістити в області catch. Коли ви додаєте це, ви маєте можливість гарантувати, що оператори в блоці catch виконуються, лише якщо виконується якась умова. Цей вираз має мати значення Boolean (true або false) і може бути отриманий за допомогою простого оператора коду в самому визначенні when або шляхом виклику додаткового методу у вашому коді. У двох словах, цей підхід дозволяє додавати «фільтри» до вашої логіки винятків.
Розглянемо наступну змінену логіку винятків. Я додав пропозицію when до обробника CarIsDeadException, щоб гарантувати, що блок catch ніколи не виконується в п’ятницю (надуманий приклад, але хто хоче, щоб його автомобіль зламався прямо перед вихідними?). Зауважте, що один логічний оператор у реченні when повинен бути укладений у круглі дужки.

```cs
void ExceptionFilters()
{
    Car car = new Car("Rusty", 70);
    try
    {
        for (int i = 0; i < 10; i++)
        {
            car.Accelerate(10);
        }
    }
    catch (CarIsDeadException ex) when (ex.ErrorTimeStamp.DayOfWeek != DayOfWeek.Friday)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.CauseOfError);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
ExceptionFilters();
```
В понеділок
```
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Rusty has overheated!
You have a lead foot
```
В п'ятницю
```
        CurrentSpeed = 80
        CurrentSpeed = 90
        CurrentSpeed = 100
Rusty has overheated!
```
Хоча цей приклад дуже надуманий, більш реалістичним використанням фільтра винятків є перехоплення SystemExceptions. Наприклад, припустимо, що ваш код зберігає дані в базі даних, виникає загальний виняток. Вивчаючи дані повідомлення та виключення, ви можете створити певні обробники на основі того, що викликало виняток.

## Налагодження необроблених винятків за допомогою Visual Studio

Visual Studio надає низку інструментів, які допомагають налагодити необроблені винятки. Припустімо, ви збільшили швидкість об’єкта Car понад максимальну, але цього разу не потрудилися завершити свій виклик у блоці try.

```cs
void DebuggingUnhandledExceptions()
{
    Car car = new Car("Rusty", 70);
    car.Accelerate(50);
}
DebuggingUnhandledExceptions();
```
Якщо ви починаєте сеанс налагодження в Visual Studio (за допомогою пункту меню Debug ➤ Start Debugging), Visual Studio автоматично припиняє роботу під час створення неперехопленого винятку. А ще краще, вам буде запропоновано вікно, у якому відображається значення властивості Message. Якщо ви клацнете посилання View Detail , ви побачите подробиці щодо стану об’єкта
Якщо вам не вдається обробити виняток, викликаний методом у бібліотеках базових класів .NET, налагоджувач Visual Studio переривається на операторі, який викликав метод-порушник.

# Висновки

У цьому розділі ви розглянули роль структурованої обробки винятків. Коли методу потрібно надіслати об’єкт помилки викликаючому коду, він виділить, налаштує та викине певний тип, похідний від System.Exception, за допомогою throw(). Викликаючий може обробляти будь-які можливі вхідні винятки за допомогою ключового слова catch C# і додаткової області finally. Є можливість створювати фільтри винятків за допомогою необов’язкового ключового слова when. Розширено місця, з яких можна створювати винятки.
Коли ви створюєте власні спеціальні винятки, ви остаточно створюєте тип класу, що походить від System.ApplicationException, який позначає виняток, створений програмою, що зараз виконується. На відміну від цього, об’єкти помилок, що походять від System.SystemException, представляють критичні (і фатальні) помилки, створені середовищем виконання .NET. І останнє, але не менш важливе: у цій главі проілюстровано різні інструменти в Visual Studio, які можна використовувати для створення спеціальних винятків (відповідно до найкращих практик .NET), а також винятків для налагодження.
