# Асінхроні виклики з патерном async/await.

Хоча TPL, PLINQ та тип делегата можуть певною мірою спростити справи (особливо порівняно з іншими платформами та мовами), розробникам все ще потрібно знати тонкощі різних передових методів.
Мова програмування C# має два ключові слова, які ще більше спрощують процес створення асинхронного коду. На відміну від усіх прикладів у цьому розділі, коли ви використовуєте ключові слова async та await, компілятор генеруватиме від вашого імені значну кількість коду для потоків, використовуючи численні члени просторів імен System.Threading та System.Threading.Tasks.

## Перший погляд на ключові слова async та await у C#

Ключове слово async у C# використовується для визначення того, що метод, лямбда-вираз або анонімний метод повинні викликатися автоматично асинхронно. Просто позначивши метод модифікатором async, середовище виконання .NET Core створить новий потік виконання для обробки поточного завдання. Крім того, коли ви викликаєте асинхронний метод, ключове слово await автоматично призупиняє поточний потік від будь-якої подальшої активності, доки завдання не буде завершено, залишаючи викликаючий потік вільним для продовження.
Для ілюстрації створіть консольний застосунок з назвою FirstLookAtAsyncAwait та додайте метод з назвою DoWork(), який змушує викликаючий потік чекати п'ять секунд. Ось код на даний момент:

```cs
static string GetThreadInfo()
{
    Thread thread = Thread.CurrentThread;
    return $"\tThreadId:{thread.ManagedThreadId}" +
        $"\tIsBackground:{thread.IsBackground}" +
        $"\tThreadState:{thread.ThreadState}";
}

static string DoWork()
{
    Console.WriteLine($"\tI star do work!\t{GetThreadInfo()}");
    Thread.Sleep(5000); // Emulation the long work
    return $"\tDone with work!";
}

string result = DoWork();
Console.WriteLine(result);
Console.WriteLine($"Complited\t{GetThreadInfo()}");
```
```
        I star do work!         ThreadId:1      IsBackground:False      ThreadState:Running
        Done with work!
Complited               ThreadId:1      IsBackground:False      ThreadState:Running
```
Тепер, враховуючи вашу роботу в цій главі, ви знаєте, що якби ви запустили програму, вам довелося б зачекати п'ять секунд, перш ніж щось ще станеться. Оскільки все відбувається в одному потоці, якби це був графічний додаток, весь екран був би заблокований до завершення роботи. 
Якщо ви використаєте будь-який із попередніх методів, показаних у цьому розділі, щоб зробити вашу програму більш адаптивною, вам доведеться виконати чимало роботи. Однак ви можете створити наступну базу коду C#:

```cs
static async Task<string> DoWorkAsync()
{
    return await Task.Run(() => {
        Console.WriteLine($"\tI star do work!\t{GetThreadInfo()}");
        Thread.Sleep(5000); // Emulation the long work
        return $"\tDone with work!";
    });
}

string message = await DoWorkAsync();
Console.WriteLine(message);
Console.WriteLine($"Complited\t{GetThreadInfo()}");
```
```
        I star do work!         ThreadId:6      IsBackground:True       ThreadState:Background
        Done with work!
Complited               ThreadId:6      IsBackground:True       ThreadState:Background
```
Зверніть увагу на ключове слово await перед тим, як назвати метод, який буде викликано асинхронно.

```cs
string message = await DoWorkAsync();
```
Це важливо: якщо ви декоруєте метод ключовим словом async, але не маєте хоча б одного внутрішнього виклику методу, орієнтованого на await, ви, по суті, створили синхронний виклик методу (фактично, вам буде надано попередження компілятора про це). Додайте простий виклик.
```cs
DoWorkAsync();
```
Після такого виклику ви нічого не побачите, бо призавершені основного потоку зупиняється вториний потік. Середовище попереджає про це.

Тепер зверніть увагу, що для рефакторингу методів верхнього рівня та DoWork() вам потрібно використовувати клас Task з простору імен System.Threading.Tasks. По суті, замість того, щоб одразу повертати конкретне значення, що повертається (рядковий об'єкт у цьому прикладі), ви повертаєте об'єкт Task\<T\>, де параметр універсального типу T є базовим, фактичним значенням, що повертається. Якщо метод не має значення, що повертається, то замість Task\<T\> ви просто використовуєте Task.

```cs
static async Task<string> DoWorkAsync()
{
    return await Task.Run(() => {
        //...
        return $"\tDone with work!";
    });
}
```
Реалізація DoWorkAsync() тепер безпосередньо повертає об'єкт Task\<string\>, який є значенням, що повертається функцією Task.Run(). Метод Run() приймає делегат Func<> або Action<>, і, як ви знаєте, ви можете спростити собі життя, використовуючи лямбда-вираз. Тобто тут діють правила на створення Task\<T\> По суті, ваша нова версія виклику DoWorkAsync() фактично говорить наступне:

    Коли ви мене викличете, я запущу нове завдання. Це завдання переведе потік у режим сну на п'ять секунд, а після його завершення поверне мені рядкове значення. Я помістю цей рядок у новий об'єкт Task<string> та поверну його викликаючій стороні.

Переклавши цю нову реалізацію DoWorkAsync() на більш природну мову, ви отримаєте деяке уявлення про справжню роль токена await. Це ключове слово завжди змінюватиме метод, який повертає об'єкт Task або Task\<T\>. Коли потік логіки досягає токена await, викликаючий потік у цьому методі призупиняється до завершення виклику. В  фреймворці з графічним застосунком, користувач міг би продовжувати використовувати інтерфейс користувача, поки виконується метод DoWorkAsync().

## SynchronizationContext та async/await

Офіційне визначення SynchronizationContext – це базовий клас, який надає контекст із вільними потоками без синхронізації. Хоча це початкове визначення не дуже описове, в офіційній документації далі йдеться:

    Мета моделі синхронізації, реалізованої цим класом, полягає в тому, щоб забезпечити правильну роботу внутрішніх асинхронних/синхронних операцій середовища виконання загальномовних програм з різними моделями синхронізації.

Це твердження, разом з тим, що ви знаєте про багатопоточність, проливає світло на це питання. Нагадаємо, що графічні програми (WinForms, WPF) не дозволяють вторинним потокам безпосередньо отримувати доступ до елементів керування, а повинні делегувати цей доступ. Ми вже розглядали об'єкт Dispatcher у прикладі WPF. Для консольних застосунків, які не використовували WPF, це обмеження не діє. Це різні моделі синхронізації, про які йдеться. 
SynchonizationContext – це тип, що надає віртуальний метод публікації, який приймає делегат для асинхронного виконання. Це забезпечує шаблон для фреймворків для належної обробки асинхронних запитів (диспетчеризація для WPF/WinForms, безпосереднє виконання для не-графічних застосунків тощо). Він надає спосіб ставити одиницю роботи в чергу для контексту та веде облік невиконаних асинхронних операцій.
Як ми обговорювали раніше, коли делегат ставиться в чергу для асинхронного виконання, його запуск планується в окремому потоці. Ця деталізація обробляється середовищем виконання .NET Core. Зазвичай це робиться за допомогою керованого пулу потоків середовища виконання .NET Core, але це можна змінити за допомогою спеціальної реалізації.
Хоча цю спеціальну роботу можна керувати вручну за допомогою коду, шаблон async/await виконує більшу частину важкої роботи. Коли очікується асинхронний метод, він використовує реалізації SynchronizationContext та TaskScheduler цільового фреймворку. Наприклад, якщо ви використовуєте async/await у WPF-застосунку, WPF-фреймворк керує диспетчеризація делегата та зворотним викликом кінцевого отримувача після завершення завдання awaited для безпечного оновлення елементів керування.

## Роль ConfigureAwait (це може буде затаріле)

Тепер, коли ви трохи краще розумієте SynchronizationContext, настав час розглянути роль методу ConfigureAwait(). За замовчуванням очікування завдання призведе до використання контексту синхронізації. Під час розробки графічних застосунків (WinForms, WPF) саме така поведінка вам потрібна. Однак, якщо ви пишете код програми без графічного інтерфейсу, накладні витрати на розміщення оригінального контексту в черзі, коли він не потрібен, можуть потенційно спричинити проблеми з продуктивністю вашої програми.

Щоб побачити це в дії, оновіть свої оператори верхнього рівня наступним чином:

```cs
string message = await DoWorkAsync();
Console.WriteLine($"0 - {message}");
string message1 = await DoWorkAsync().ConfigureAwait(false);
Console.WriteLine($"1 - {message1}");
```
Початковий блок коду використовує SynchronizationContext, наданий фреймворком (у цьому випадку, середовищем виконання .NET Core). Це еквівалентно виклику ConfigureAwait(true). Другий приклад ігнорує поточний контекст і планувальник.

Рекомендації команди .NET Core пропонують під час розробки коду програми (WinForms, WPF тощо) залишати поведінку за замовчуванням. Якщо ви пишете не-програмний код (наприклад, код бібліотеки), використовуйте ConfigureAwait(false). Єдиним винятком є ​​ASP.NET Core (розглянуто, далі). ASP.NET Core не створює власний SynchronizationContext; тому ConfigureAwait(false) не забезпечує переваги при використанні інших фреймворків.

## Правила іменування асинхронних методів

Припустимо ви створили новий асінхроний метод MyWork:

```cs
static async Task<string> MyWork()
{
    return await Task.Run(() => {

        string result = "";
        //...Do long work to get result 
        return result;
    });
}
```
Потім хтось захотів його використати:

```cs
string otherString = MyWork();
```
Зверніть увагу, що ви справді позначили метод ключовим словом async, але не використали ключове слово await перед викликом методу MyWork(). На цьому етапі виникнуть помилки компілятора, оскільки повернене значення MyWork() є об'єктом Task\<string\>, який ви намагаєтеся призначити безпосередньо рядковій змінній. Пам'ятайте, що токен await витягує внутрішнє повернене значення, що міститься в об'єкті Task. Оскільки ви не використовували цей токен, у вас виникне невідповідність типів.

```cs
string otherString = await MyWork();
Console.WriteLine(await MyWork());
```
```
The result:
```

    «Очікуваний» метод — це просто метод, який повертає Task або Task<T>.

Враховуючи, що методи, що повертають об'єкти Task, тепер можна викликати неблокуючим чином через токени async та await, рекомендується називати будь-який метод, що повертає Task, суфіксом Async. Таким чином, розробники, які знають правила іменування, отримують візуальне нагадування про те, що ключове слово await є обов'язковим, якщо вони мають намір викликати метод в асинхронному контексті.

    Обробники подій для елементів керування графічним інтерфейсом (наприклад, обробник кліку кнопки), а також методи дій у програмах у стилі MVC, які використовують ключові слова async/await, не дотримуються цієї конвенції іменування.

## Асинхронні методи, що не повертають дані

Наразі ваш метод DoWorkAsync() повертає Task<string>, який містить «реальні дані» для викликаючої сторони, що будуть прозоро отримані через ключове слово await. Однак, що робити, якщо ви хочете створити асинхронний метод, який нічого не повертає? Хоча існує два способи зробити це, насправді існує лише один правильний спосіб. Спочатку розглянемо проблеми з визначенням методу async void.

### Mетоди async void

Нижче наведено приклад асинхронного методу, який використовує void як тип повернення замість Task:

```cs
static async void MethodReturningVoidAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine(GetThreadInfo());
        /* Do some work here... */
        Thread.Sleep(3000);

    });
    Console.WriteLine($"Fire and forget void method completed");
}

MethodReturningVoidAsync();
Console.WriteLine($"Completed.{GetThreadInfo()}");
Console.ReadLine();
```
```
Completed.      ThreadId:1      IsBackground:False      ThreadState:Running
        ThreadId:6      IsBackground:True       ThreadState:Background
Fire and forget void method completed
```
Метод працює самостійно, не блокуючи основний потік. Повідомлення «Completed...» перед повідомленням методу MethodReturningVoidAsync() "Fire and forget..."
Хоча це може здатися життєздатним варіантом для сценаріїв «fire and forget», є більша проблема. Якщо метод викидає виняток, йому нікуди не перейти, окрім контексту синхронізації викликаючого методу. Оновіть метод до наступного:

```cs
static async void MethodReturningVoidWithExeptionAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine(GetThreadInfo());
        /* Do some work here... */
        Thread.Sleep(3000);
        throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Fire and forget void method completed");
}
```
Для безпеки оберніть виклик цього методу в блок try-catch і запустіть програму ще раз:
```cs
try
{
    MethodReturningVoidWithExeptionAsync();    
}
catch (Exception e)
{
    Console.WriteLine(e);
}
```
Блок catch не тільки не перехоплює виняток, але виняток поміщається в контекст виконання потоку. Тож, хоча це може здатися хорошою ідеєю для сценаріїв «fire and forget», вам краще сподіватися, що в методі async void не буде створено винятку, інакше вся ваша програма може вийти з ладу.

### Методи async Task шо нічоно не повертають.

Краще, щоб ваш метод використовував Task замість void. Оновіть метод до наступного:

```cs
static async Task MethodReturningTaskAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine(GetThreadInfo());
        /* Do some work here... */
        Thread.Sleep(3000);
    });
    Console.WriteLine($"Void method completed");
}
```
Якщо викликати метод без ключового слова await, буде отримано той самий результат, що й у попередньому прикладі.

```cs
MethodReturningTaskAsync();
Console.WriteLine($"Completed.{GetThreadInfo()}");
Console.ReadLine();
```
```
Completed.      ThreadId:1      IsBackground:False      ThreadState:Running
        ThreadId:6      IsBackground:True       ThreadState:Background
Void method completed
```
Оновіть MethodReturningTaskAsync() для виклику винятку:

```cs
static async Task MethodReturningTaskWithExeptionAsync()
{
    await Task.Run(() =>
    {
        Console.WriteLine(GetThreadInfo());
        /* Do some work here... */
        Thread.Sleep(3000);
        throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Void method completed");
}
```
Тепер оберніть виклик цього методу в блок try-catch:

```cs
try
{
    MethodReturningTaskWithExeptionAsync();
    Console.WriteLine($"Completed.{GetThreadInfo()}");
    Console.ReadLine();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
```
```
Completed.      ThreadId:1      IsBackground:False      ThreadState:Running
        ThreadId:6      IsBackground:True       ThreadState:Background

```
Коли ви запускаєте програму і виникає виняток, відбуваються дві цікаві речі. Перша полягає в тому, що вся програма не аварійно завершує роботу, а друга — що блок catch не перехоплює виняток. Виняток виникає в вториному фономому потоці. Коли метод Task/Task<T> викидає виняток, він перехоплюється та поміщається до об'єкта Task. Під час використання await виняток (або AggregateException) доступний для обробки. Оновіть код виклику, щоб він очікував на метод, і блок catch тепер працюватиме належним чином:

```cs
try
{
    await MethodReturningTaskWithExeptionAsync();
    Console.WriteLine($"Completed.{GetThreadInfo()}");
    Console.ReadLine();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
```
```
        ThreadId:6      IsBackground:True       ThreadState:Background
Something bad happened
```
Підсумовуючи, вам слід уникати створення асинхронних методів void та використовувати асинхронні методи Task. Чи чекати на них, чи ні, стає бізнес-рішенням, але в будь-якому разі, принаймні ви не призведете до збою вашої програми!

## Асинхронні методи з кількома await

Цілком допустимо, щоб один асинхронний метод мав кілька контекстів очікування у своїй реалізації. Наступний приклад показує це в дії:

```cs
static async Task MultipleAwaitsAsync()
{
    await Task.Run(() =>
    {
        Thread.Sleep(2000);
        Console.WriteLine($"Done with first task! {GetThreadInfo()}");
    });
    await Task.Run(() => 
    {
        Thread.Sleep(1000);
        Console.WriteLine($"Done with second task! {GetThreadInfo()}");
    });
    await Task.Run(() => 
    {
        Thread.Sleep(2000);
        Console.WriteLine($"Done with third task! {GetThreadInfo()}");
    });
}

var watch = Stopwatch.StartNew();
await MultipleAwaitsAsync();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
```
```
Done with first task!   ThreadId:6      IsBackground:True       ThreadState:Background
Done with second task!  ThreadId:6      IsBackground:True       ThreadState:Background
Done with third task!   ThreadId:7      IsBackground:True       ThreadState:Background
Time: 5068
```
Знову ж таки, тут кожне завдання не робить набагато більше, ніж призупинення поточного потоку на певний час; проте будь-яка одиниця роботи може бути представлена ​​цими завданнями (виклик веб-сервісу, читання бази даних тощо).

Інший варіант — не чекати на кожне завдання, а чекати на всі разом і повертатися, коли всі завдання будуть виконані. Це більш імовірний сценарій, де є три речі (перевірка електронної пошти, оновлення сервера, завантаження файлів), які необхідно виконати пакетно, але можна зробити паралельно. Ось код, оновлений за допомогою методу Task.WhenAll():

```cs
static async Task MultipleAwaitsWhenAllAsync()
{
    await Task.WhenAll(
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with first task! {GetThreadInfo()}");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(1_000);
                Console.WriteLine($"Done with second task! {GetThreadInfo()}");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with third task! {GetThreadInfo()}");
            })
     );
}

var watch = Stopwatch.StartNew();
await MultipleAwaitsWhenAllAsync();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
```

Коли ви зараз запустите програму, ви побачите, що три завдання запускаються в порядку найменшого часу сну.

```
Done with second task!  ThreadId:7      IsBackground:True       ThreadState:Background
Done with third task!   ThreadId:8      IsBackground:True       ThreadState:Background
Done with first task!   ThreadId:6      IsBackground:True       ThreadState:Background
Time: 2017
```
Також існує WhenAny(), який сигналізує про завершення одного із завдань. Метод повертає перше завершене завдання. 

```cs
static async Task MultipleAwaitsWhenAnyAsync()
{
    await Task.WhenAny(
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with first task! {GetThreadInfo()}");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(1_000);
                Console.WriteLine($"Done with second task! {GetThreadInfo()}");
            }),
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with third task! {GetThreadInfo()}");
            })
     );
}
```
```
Done with second task!  ThreadId:7      IsBackground:True       ThreadState:Background
Time: 1050
```

Кожен із цих методів також працює з масивом завдань. Щоб продемонструвати це, створіть новий метод з назвою MultipleAwaitsWithListTaskAsync(). У цьому методі створіть List, додайте до нього три завдання, а потім викличте Task.WhenAll() або Task.WhenAny():

```cs
static async Task MultipleAwaitsWithListTaskAsync()
{
    var tasks = new List<Task>();
    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(2_000);
        Console.WriteLine($"Done with first task! {GetThreadInfo()}");
    }));
    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(1_000);
        Console.WriteLine($"Done with second task! {GetThreadInfo()}");
    }));
    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(2_000);
        Console.WriteLine($"Done with third task! {GetThreadInfo()}");
    }));
    await Task.WhenAll(tasks);
    //await Task.WhenAny(tasks);

}

var watch = Stopwatch.StartNew();
await MultipleAwaitsWithListTaskAsync();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");

```
```
Done with second task!  ThreadId:7      IsBackground:True       ThreadState:Background
Done with first task!   ThreadId:6      IsBackground:True       ThreadState:Background
Done with third task!   ThreadId:8      IsBackground:True       ThreadState:Background
Time: 2010
```
## Виклик асинхронних методів із синхронних методів

У кожному з попередніх прикладів використовувалося ключове слово async для повернення потоку щоб викликати код під час виконання асінхронного методу. Для отримання результату, ключове слово await можна використовувати лише в методі, позначеному як async. Що робити, якщо ви не можете (або не хочете) позначити метод як async? Існують способи викликати асинхронні методи в синхронному контексті. На жаль, більшість із них погані. 
Перший варіант полягає в тому, щоб просто відмовитися від ключового слова await, дозволяючи початковому потоку продовжувати виконання, тоді як асинхронний метод виконується в окремому потоці, ніколи не повертаючись до викликаючого.
Це поводиться подібно до попереднього прикладу виклику асинхронних методів Task. Будь-які значення, які повертає метод, втрачаються, а винятки проковтуються.
Це може відповідати вашим потребам, але якщо ні, у вас є три варіанти. Перший — просто використовувати властивість Result для Task\<T\> або метод Wait() для методів Task. Якщо метод завершується невдачею, будь-які винятки обгортаються в AggregateException, що потенційно ускладнює обробку помилок. Ви також можете викликати GetAwaiter().GetResult(). Це поводиться так само, як і виклики Wait() та Result, з невеликою різницею, що винятки не обгортаються в AggregateException. Хоча методи GetAwaiter().GetResult() працюють як з методами зі значенням, що повертається, так і з методами без значення, що повертається, вони позначені в документації як «не для зовнішнього використання», що означає, що вони можуть змінитися або зникнути в майбутньому. 
Хоча ці два варіанти здаються нешкідливою заміною використання await в асинхронному методі, існує серйозніша проблема з їх використанням. Виклик Wait(), Result або GetAwaiter().GetResult() блокує викликаючий потік, обробляє асинхронний метод в іншому потоці, а потім повертається назад до викликаючого потоку, зв'язуючи два потоки для виконання роботи. Ще гірше те, що кожен з цих варіантів може спричинити взаємоблокування, особливо якщо викликаючий потік знаходиться в інтерфейсі користувача програми.
Щоб допомогти виявити та виправити неправильний код async/await (та правила іменування), додайте до проєкту пакет Microsoft.VisualStudio.Threading.Analyzers. Цей пакет додає аналізатори, які надаватимуть попередження компілятора, коли виявлено неправильний код потоків, включаючи неправильні правила іменування. Щоб побачити це в дії, додайте наступний код до операторів верхнього рівня:

```cs
 _ = DoWorkAsync().Result;
 _ = DoWorkAsync().GetAwaiter().GetResult();
```

Це призводить до такого попередження компілятора:

```
VSTHRD002   Synchronously waiting on tasks or awaiters may cause deadlocks. Use await or
JoinableTaskFactory.Run instead.
```
Не лише відображатиметься попередження компілятора, але й надано рекомендоване рішення! Щоб використовувати клас JoinableTaskFactory, потрібно додати пакет Microsoft.VisualStudio.Threading NuGet та наступний оператор using на початку файлу Program.cs:

```cs
using Microsoft.VisualStudio.Threading;
```
Об'єкту JoinableTaskFactory потрібен JoinableTaskContext у конструкторі:

```cs
JoinableTaskFactory joinableTaskFactory = new JoinableTaskFactory(
    new JoinableTaskContext());
```
З урахуванням цього ви можете використовувати метод Run() для безпечного виконання асинхронного методу, такого як метод DoWork(), із синхронного контексту:
```cs
string message = joinableTaskFactory.Run(async () => await DoWorkAsync());
Console.WriteLine(message);
```
```
        I star do work!         ThreadId:4      IsBackground:True       ThreadState:Background
        Done with work!
```

    Хоча в назвах пакетів є VisualStudio, вони не залежать від Visual Studio. Це пакети .NET, які можна використовувати як з встановленим Visual Studio, так і без нього.

## Await в блоках catch та finally

Існує можливість розміщувати виклики await у блоках catch та finally. Сам метод має бути асинхронним для цього. Наступний приклад коду демонструє цю можливість:

```cs
static async Task<string> MethodWithTryCatch()
{
  try
  {
    //Do some work
    return 'Hello';
  }
  catch (Exception ex)
  {
    await LogTheErrors();
    throw;
  }
  finally
  {
    await DoMagicCleanUp();
  }
}
```

## Узагальнені асинхронні типи повернення

Існують додаткові типи повернення, якщо вони відповідають шаблону асинхронізації. Одним конкретним прикладом є ValueTask. Щоб побачити це в дії, створіть такий код:

```cs
static async ValueTask<int> ReturnAnIntAsync()
{
    await Task.Delay(3_000);
    return 5;
}

//Console.WriteLine(ReturnAnIntAsync()); // You won't see anything.
Console.WriteLine(await ReturnAnIntAsync());
```
```
5
```
Ті самі правила застосовуються для ValueTask, що й для Task, оскільки ValueTask є просто Task для типів значень, а не примусовим виділенням об'єкта в купі.

## Локальні функції з async/await

Локальні функції також можуть бути корисними для асинхронних методів. Щоб продемонструвати перевагу, спочатку потрібно побачити проблему. Додайте новий метод з назвою MethodWithProblems() та додайте наступний код:

```cs
static async Task MethodWithProblems(int firstParam, int secondParam)
{
    await Task.Run(() =>
    {
        //Call long running method
        Thread.Sleep(4_000);
        Console.WriteLine("First Complete");
        //Call another long running method that fails because
        //the second parameter is out of range
        Console.WriteLine("Something bad happened");
    });
}
await MethodWithProblems(1, -2);
```
```
First Complete
Something bad happened

```
Сценарій такий, що друге тривале завдання завершується невдачею через недійсні вхідні дані. Ви можете (і повинні) додавати перевірки на початок методу, але оскільки весь метод є асинхронним, немає гарантії, коли перевірки будуть виконані. Було б краще, якби перевірки відбувалися одразу, перш ніж викликальний код перейде далі. У наступному оновленні перевірки виконуються синхронно, а потім приватна функція виконується асинхронно:

```cs
static async Task MethodWithProblemsFixed(int firstParam, int secondParam)
{
    if (secondParam < 0)
    {
        Console.WriteLine("Bad data");
        return;
    }
    await actualImplementation();
    async Task actualImplementation()
    {
        await Task.Run(() =>
        {
            //Call long running method
            Thread.Sleep(4_000);
            Console.WriteLine("First Complete");
            //Call another long running method that fails because
            //the second parameter is out of range
            Console.WriteLine("Something bad happened");
        });
    }
}
await MethodWithProblemsFixed(1, -2);
```
```
Bad data
```

## Приклад застосування async/await в GUI

Для демонстрації ми використаємо той самий проект WPF, що й раніше в цьому розділі. Створемо проект типу WPF Application з назвою PictureHandlerWithAsyncAwait. Встановити пакет System.Drawing.Common. Tools > NuGet Package Manager > Manage NuGet Packages for Solution > В рядку пошуку System.Drawing.Common > Install
Для тестування в католог D:\Temp\Pitures скопіюємо декілька будь-яких зображень з розширенням *.jpg. Змінемо файл MainWindow.xalm і додайте обробники подій кнопок.

```xml
<<Window x:Class="PictureHandlerWithAsyncAwait.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:PictureHandlerWithAsyncAwait"
        mc:Ignorable="d"
        Title="Picture Handler with Async/Await." Height="450" Width="800">
    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>
        <Label Grid.Row="0" Grid.Column="0">
            Feel free to type here while the images are processed...
        </Label>
        <TextBox Grid.Row="1" Grid.Column="0"  Margin="10,10,10,10"/>
        <Grid Grid.Row="2" Grid.Column="0">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="Auto"/>
                <ColumnDefinition Width="*"/>
                <ColumnDefinition Width="Auto"/>
            </Grid.ColumnDefinitions>
            <Button Name="cmdCancel" Grid.Column="0" Margin="65,0,665,10" Click="cmdCancel_Click" Height="20" VerticalAlignment="Bottom" Grid.ColumnSpan="2">
                Cancel
            </Button>
            <Button Name="cmdDoWork" Grid.Row="0" Grid.Column="1" Margin="353,10,353,10" 
                    Click="cmdDoWork_Click">
                Do work
            </Button>
            <Button Name="cmdProcess" Grid.Row="0" Grid.Column="1" Margin="468,10,210,10"
                    Click="cmdProcess_Click">
                Process
            </Button>
            <Button Name="cmdProcessWithForEachAsync" Grid.Row="0" Grid.Column="1" Margin="595,10,40,10" 
                    Click="cmdProcessWithForEachAsync_Click">
                ProcessWithForEachAsync
            </Button>
        </Grid>
    </Grid>
</Window>
```
Змінимо файл MainWindow.xalm.сs (можливо треба розгорнути стрілку файлу MainWindow.xalm). В верхній частині оператори using.

```cs
//using System.Windows.Shapes;
using System.IO;
using System.Drawing;
```
Спробуємо застосувати патерн async/await для довгого завдання. Змінемо обробник для кнопки DoWork

```cs
        private static string GetThreadInfo(Thread thread)
        {
            return  $" ThreadId: {thread.ManagedThreadId} " +
                    $" IsBackground: {thread.IsBackground} " +
                    $" ThreadState: {thread.ThreadState} ";
        }
        private void DoWork(int interval)
        {
            Thread.Sleep(interval); // Emulation the long work
        }

        private async void cmdDoWork_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                Thread thread = Thread.CurrentThread;
                int? taskId = Task.CurrentId;
                Dispatcher?.Invoke(() => { Title = $"Start work {taskId}. {GetThreadInfo(thread)} ...";});
                
                DoWork(5000);
                
                Dispatcher?.Invoke(() => { Title = $"End work {taskId}. {GetThreadInfo(thread)}"; });
            });
        }
```
Якшо ви натисните на DoWork інтерфейс не блокується і можна продовжувати вводити текст покі завдання виконується в окремому потоці. Також натиснути можна декілька разів не чекаючи.

## Обробка зображень і скасування операцій з async/await

Скасування також можливе за допомогою шаблону async/await, і це набагато простіше, ніж за допомогою Parallel.ForEach. Додамо зміну рівня класа типу CancellationTokenSource та обробник події натискання Cancel.

```cs
        private CancellationTokenSource? _cancellationTokenSource = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource?.Cancel();
        }
```
Додамо метод який обробляє одне зобрадення.

```cs
        private async Task ProcessFileAsync(string currentFile, string outputDirectory, CancellationToken token)
        {
            string filename = Path.GetFileName(currentFile);
            try
            {
                await Task.Run(() =>
                {
                    Thread thread = Thread.CurrentThread;
                    int? taskId = Task.CurrentId;
                    Dispatcher?.Invoke(() => { Title = $"Processing {taskId} {GetThreadInfo(thread)} {filename}";});

                    using Bitmap bitmap = new(currentFile);
                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(Path.Combine(outputDirectory, filename));
                }, token);
            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
```
Цей метод використовує інше перевантаження команди Task.Run, приймаючи CancellationToken як параметр. Команда Task.Run обгорнута блоком try/catch (як і код виклику) на випадок, якщо користувач натисне кнопку Cancel.

Процес обробки зображень такий самий, як і в попередньому прикладі: отримати каталог зображень, створити вихідний каталог, отримати файли зображень, повернути їх та зберегти в новому каталозі. У цій версії для виконання роботи використовуватимуть асинхронні методи в циклі foreach, а сигнатури методів прийматимуть CancellationToken як параметр.

```cs
        private async void cmdProcess_Click(object sender, RoutedEventArgs e)
        {
            _cancellationTokenSource = new();
            string pictureDirectory = @"D:\Temp\Pictures";
            string outputDirectory = @"D:\Temp\ModifiedPictures";

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            try
            {
                foreach (var file in files)
                {
                    await ProcessFileAsync(file, outputDirectory, _cancellationTokenSource.Token);
                }
                Title = "Process complite";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            _cancellationTokenSource = null;
        }
```
Після початкового налаштування код перебирає файли та асинхронно викликає ProcessFileAsync() для кожного файлу. Виклик ProcessFileAsync() обгортається блоком try/catch, а CancellationToken передається в метод ProcessFile(). Якщо Cancel() виконується на CancellationTokenSource (наприклад, коли користувач натискає кнопку «Cancel»), виникає виняток OperationCanceledException. При виникнені винятку цикл переривається. Подія настисканя на виконання обробки не впливає на користувацький інтерфейс бо все відбувається в вторинному робочому потоці. Хоча інтерфейс не блокується обробка зображень відбувається послідовно в томуж самому вторинному потоці якій зупиняється і чекає.

## Асінхроний метод Parallel.ForEachAsync.

В класі Paralell є метод ForEachAsync який є асінхроним і дозволяє використати асінхроний метод для тіла циклу. Додамо в проект обробник натискання.

```cs
        private async void cmdProcessWithForEachAsync_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await ProcessWithForEachAsync();
            }
            catch (Exception ex)
            {
                Title = ex.Message;
            }
      
        }

        private async Task ProcessWithForEachAsync()
        {
            _cancellationTokenSource = new();

            string pictureDirectory = @"D:\Temp\Pictures";
            string outputDirectory = @"D:\Temp\ModifiedPictures";

            // Use ParallelOptions instance to store the CancellationToken.
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.CancellationToken = _cancellationTokenSource.Token;
            parallelOptions.MaxDegreeOfParallelism = Environment.ProcessorCount;

            //Recreate directory 
            if (Directory.Exists(outputDirectory))
            {
                Directory.Delete(outputDirectory, true);
            }
            Directory.CreateDirectory(outputDirectory);

            //Process
            string[] files = Directory.GetFiles(pictureDirectory, "*.jpg", SearchOption.AllDirectories);

            try
            {
                await Parallel.ForEachAsync(files, parallelOptions, async (currentFile, token) =>
                {
                    await Task.Run(() =>
                    {
                        token.ThrowIfCancellationRequested();
                        string filename = Path.GetFileName(currentFile);

                        Thread thread = Thread.CurrentThread;
                        int? taskId = Task.CurrentId;
                        Dispatcher?.Invoke(() => { Title = $"Processing {taskId} {GetThreadInfo(thread)} {filename}"; });

                        using Bitmap bitmap = new Bitmap(currentFile);
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bitmap.Save(Path.Combine(outputDirectory, filename));
                    });
                });
                Dispatcher?.Invoke(() => Title = "Process complite.");
            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
            }
        }
```
В об'єкті ParallelOptions зберігаеться послання на властивість CancellationTokenSource.Token якій відповідає для скасування виконнання циклу. Тіло цилу використовує асінхроний метод який вказано у вигляді лямбда виразу. Можливо в цьому випадку краще було б зробити окремий метод замість лямбда-виразу. Як видно завдання параалельно відпрацьовуються швидше з використання Parallel.ForEachAsync ніж коли звичайний цикл завдань.

## Скасування в патерні async/await за допомогою методу WaitAsync().

Асинхронні виклики можна скасувати за допомогою токена скасування та/або після досягнення ліміту часу за допомогою методу WaitAsync(). Повернемся до проекту FirstLookAtAsyncAwait.

Ми маємо метод.
```cs
static async Task<string> DoWorkAsync()
{
    return await Task.Run(() => {
        Console.WriteLine($"\tI star do work!\t{GetThreadInfo()}");
        Thread.Sleep(5000); // Emulation the long work
        return $"\tDone with work!\t{GetThreadInfo()}\n";
    });
}
```

Скасувати завдання можна за допомогою методу WaitAsync().

```cs
async Task UsingWaitAsync()
{
    CancellationTokenSource cancellationTokenSource = new();

    //try
    //{
    //    string message = await DoWorkAsync().WaitAsync(TimeSpan.FromSeconds(12));
    //    await Console.Out.WriteLineAsync(message);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

    //try
    //{
    //    string message = await DoWorkAsync().WaitAsync(TimeSpan.FromSeconds(2));
    //    await Console.Out.WriteLineAsync(message);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

    //try
    //{
    //    string message = await DoWorkAsync().WaitAsync(cancellationTokenSource.Token);
    //    await Console.Out.WriteLineAsync(message);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}


    cancellationTokenSource.Cancel();

    //try
    //{
    //    _ = await DoWorkAsync().WaitAsync(cancellationTokenSource.Token);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

    try
    {
        _ = await DoWorkAsync().WaitAsync(TimeSpan.FromSeconds(2), cancellationTokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
await UsingWaitAsync();
```
```
        I star do work!         ThreadId:6      IsBackground:True       ThreadState:Background
A task was canceled.
```
## Скасування операцій async/await у синхронних викликах

Метод Wait() також може приймати токен скасування під час виклику асинхронних методів з неасинхронного методу. Це можна використовувати з тайм-аутом або без нього. При використанні з тайм-аутом, тайм-аут має бути в мілісекундах:

```cs
void UsingWait()
{
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    //MethodReturningTaskAsync().Wait(tokenSource.Token);
    //MethodReturningTaskAsync().Wait(10000, tokenSource.Token);

    tokenSource.Cancel();

    //try
    //{
    //    MethodReturningTaskAsync().Wait(tokenSource.Token);
    //}
    //catch (Exception ex)
    //{
    //    Console.WriteLine(ex.Message);
    //}

    try
    {
        MethodReturningTaskAsync().Wait(2000, tokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UsingWait();
```
```
        ThreadId:6      IsBackground:True       ThreadState:Background
The operation was canceled.
```
Ви також можете використовувати JoinableTaskFactory та метод WaitAsync() під час виклику із синхронного коду:

```cs
void UsingWaitAsyncInSync()
{
    JoinableTaskFactory joinableTaskFactory = new JoinableTaskFactory(new JoinableTaskContext());
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    tokenSource.Cancel();
    try
    {
        joinableTaskFactory.Run(async () =>
        {
            //await MethodReturningTaskAsync().WaitAsync(tokenSource.Token);
            await MethodReturningTaskAsync().WaitAsync(TimeSpan.FromSeconds(2), tokenSource.Token);
        });
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UsingWaitAsyncInSync();
```
```
        ThreadId:4      IsBackground:True       ThreadState:Background
A task was canceled.
```

## Асинхронні streams(потоки)

Потоки (більш детально описані в іншій главі) можуть створюватися та використовуватися асинхронно. Метод, який повертає асинхронний потік

1. Оголошується з модифікатором async
2. Повертає IAsyncEnumerable
3. Містить оператори yield return (розглянуті раніше) для повернення послідовних елементів в асинхронному потоці

Візьмемо наступний приклад:
```cs
static async IAsyncEnumerable<int> GenerateSequence()
{
    for (int i = 0; i < 20; i++)
    {
        await Task.Delay(100);
        yield return i;
    }
}
```
Метод оголошено як async, повертає IAsyncEnumerable<int> та використовує yield return для повернення цілих чисел з послідовності.Щоб викликати цей метод, додайте наступний код до коду виклику:
```cs
await foreach (var number in GenerateSequence())
{
    Console.WriteLine(number);
}
```
```
0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19
```

## Використання async/await для завантажень.

Згадаемо проект MyEBookReader в якому ми завантажували і обробляли книгу. Додамо новий проект MyEBookReaderAsyncAwait

```cs
using System.Text;

async Task DownloadBookWihtAsyncAwaitAndGetStatisticAsync()
{
    string _theEbook = string.Empty;

    await GetBookAsync();
    Console.ReadLine();

    // Methods
    async Task GetBookAsync()
    {
        HttpClient httpClient = new();
        _theEbook = await httpClient.GetStringAsync("http://www.gutenberg.org/files/98/98-0.txt");
        GetStats();
    }

    void GetStats()
    {
        string[] words = _theEbook.Split(new char[] { ' ', '\u000A', ',', '.', ';', ':', '-', '?', '/' }, StringSplitOptions.RemoveEmptyEntries);

        string[] tenMostCommon = [];
        string longestWord = string.Empty;

        Parallel.Invoke(
            () => { tenMostCommon = FindTenMostCommon(words); },
            () => { longestWord = FindLongestWord(words); }
            );

        StringBuilder stringBuilder = new StringBuilder("Ten most common words are:\n");

        foreach (var word in tenMostCommon)
        {
            stringBuilder.AppendLine(word);
        }

        stringBuilder.AppendLine($"Longest word is: {longestWord}");

        Console.WriteLine(stringBuilder.ToString(), "Book info");
    }

    string[] FindTenMostCommon(string[] words)
    {
        var frequencyOrder = from word in words
                             where word.Length > 6
                             group word by word into g
                             orderby g.Count() descending
                             select g.Key;
        string[] result = frequencyOrder.Take(20).ToArray();
        return result;
    }

    string FindLongestWord(string[] words)
    {
        var query = from word in words
                    orderby word.Length descending
                    select word;
        return query.FirstOrDefault()!;
    }
}
await DownloadBookWihtAsyncAwaitAndGetStatisticAsync();
```
```
Ten most common words are:
Defarge
himself
Manette
through
nothing
business
another
looking
prisoner
Cruncher
Stryver
CHAPTER
without
Monsieur
Monseigneur
Tellson's
Charles
returned
husband
Gutenberg
Longest word is: undistinguishable
```
В цьому прикладі оновлено метод GetBookAsync

```cs
    async Task GetBookAsync()
    {
        HttpClient httpClient = new();
        _theEbook = await httpClient.GetStringAsync("http://www.gutenberg.org/files/98/98-0.txt");
        await Console.Out.WriteLineAsync("Download complite");
        GetStats();        
    }
```
Після створення нового екземпляра класу HttpClient, код викликає метод GetStringAsync() для здійснення HTTP-виклику Get для отримання тексту з веб-сайту. Як бачите, HttpClient надає набагато стислий спосіб здійснення HTTP-викликів. Клас HttpClient буде розглянуто набагато детальніше в розділах, присвячених ASP.NET Core.

## Підсумки стосовно патерна async/await

Ключові моменти при роботі з async/await:

1. Методи (а також лямбда-вирази або анонімні методи) можна позначити ключовим словом async, щоб метод міг виконувати роботу в робочому потоці без блокування первинного.

2. Методи (а також лямбда-вирази або анонімні методи), позначені ключовим словом async, будуть виконуватися синхронно, доки не буде знайдено ключове слово await.

3. Один асинхронний метод може мати кілька контекстів await.

4. Коли зустрічається вираз await, викликаючий потік призупиняється до завершення очікуваного завдання. Тим часом керування повертається тому, хто викликає метод.

5. Ключове слово await приховає повернений об'єкт Task з поля зору, виглядаючи так, ніби він безпосередньо повертає базове повернене значення. Методи без поверненого значення просто повертають void.

6. Перевірку параметрів та іншу обробку помилок слід виконувати в основній частині методу, а фактичну частину переміщувати до приватної асінхронної функції.

7. Для змінних стеку об'єкт ValueTask ефективніший за Task, що може призвести до упаковки та розпакування.

8. Як правило, методи, які викликаються асинхронно, слід позначати суфіксом Async.

# Підсумки

Ця глава розпочалася з вивчення ролі простору імен System.Threading. Як ви дізналися, коли програма створює додаткові потоки виконання, це призводить до того, що відповідна програма може виконувати численні завдання одночасно. Ви також розглянули кілька способів захисту блоків коду, залежних від потоків, щоб гарантувати, що спільні ресурси не стануть непридатними для використання одиницями фальшивих даних.
У цьому розділі було розглянуто деякі моделі роботи з багатопотоковою розробкою, зокрема бібліотеку Task Parallel Library та PLINQ. 
Я завершив розгляд ролі ключових слів async та await. Як ви бачили, ці ключові слова використовують багато типів фреймворку TPL у фоновому режимі; однак компілятор виконує більшу частину роботи зі створення складного коду потоків та синхронізації за вас.