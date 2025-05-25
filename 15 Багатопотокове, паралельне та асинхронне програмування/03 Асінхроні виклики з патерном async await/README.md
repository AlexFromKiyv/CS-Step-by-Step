# Асінхроні виклики з патерном async await.

Хоча TPL, PLINQ та тип делегата можуть певною мірою спростити справи (особливо порівняно з іншими платформами та мовами), розробникам все ще потрібно знати тонкощі різних передових методів.
Мова програмування C# має два ключові слова, які ще більше спрощують процес створення асинхронного коду. На відміну від усіх прикладів у цьому розділі, коли ви використовуєте ключові слова async та await, компілятор генеруватиме від вашого імені значну кількість коду для потоків, використовуючи численні члени просторів імен System.Threading та System.Threading.Tasks.

## Перший погляд на ключові слова async та await

Ключове слово async у C# використовується для визначення того, що метод, лямбда-вираз або анонімний метод повинні викликатися автоматично асинхронно. Просто позначивши метод модифікатором async, середовище виконання .NET Core створить новий потік виконання для обробки поточного завдання. Крім того, коли ви викликаєте асинхронний метод, ключове слово await автоматично призупиняє поточний потік від будь-якої подальшої активності, доки завдання не буде завершено, залишаючи викликаючий потік вільним для продовження.
Для ілюстрації створіть консольний застосунок з назвою WorkWithAsync та додайте метод з назвою DoWork(), який змушує викликаючий потік чекати :

```cs
static string DoWork()
{
    int threadId = Thread.CurrentThread.ManagedThreadId;
    Console.WriteLine($"\n\tI star to do long work! Thread:{threadId}");
    Thread.Sleep(5000); // Emulation the long work
    return $"\tDone with work! Thread:{threadId}\n";
}


while (true)
{
    Console.WriteLine(DoWork());
    Console.Write($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing:");
    Console.ReadLine();
}
```
Запустіть код на виконання і не чекаючи нажміть двічи Enter.

```

        I star to do long work! Thread:1
        Done with work! Thread:1

Thread 1 says: Enter somthing:

        I star to do long work! Thread:1
        Done with work! Thread:1

Thread 1 says: Enter somthing:

        I star to do long work! Thread:1
        Done with work! Thread:1

Thread 1 says: Enter somthing:
```
Такми чино сінхронне виконання виконується з затримками. Бажано довгі навантаженя відокремити від основного потоку таким чином вивільнивши основний потік для роботи користувача. Якщо ви використаєте будь-який із попередніх методів, показаних у цьому розділі, щоб зробити вашу програму більш адаптивною, вам доведеться багато працювати. Однак, ви можете створити наступну базу коду C#:

```cs
static async Task<string> DoWorkAsync()
{
    return await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"\n\tI star to do long work asynchronous! Thread:{threadId}");
        Thread.Sleep(5000); // Emulation the long work
        return $"\tDone with work! Thread:{threadId}\n";
    });
}

while (true)
{
    string taskResult = await DoWorkAsync();
    Console.WriteLine(taskResult);

    Console.Write($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing:");
    Console.ReadLine();
}
```
```
        I star to do long work asynchronous! Thread:6
        Done with work! Thread:6

Thread 6 says: Enter somthing:

        I star to do long work asynchronous! Thread:7
        Done with work! Thread:7

Thread 7 says: Enter somthing:

        I star to do long work asynchronous! Thread:7
        Done with work! Thread:7

Thread 7 says: Enter somthing:
```
Оператори верхнього рівня, неявно є асинхронними. Зверніть увагу на ключове слово await перед тим, як назвати метод, який буде викликано асинхронним способом. Це важливо: якщо ви декоруєте метод ключовим словом async, але не маєте хоча б одного внутрішнього виклику методу, орієнтованого на await, ви, по суті, створили синхронний виклик методу (фактично, вам буде надано попередження компілятора про це). 
Тепер зверніть увагу, що вам потрібно використовувати клас Task з простору імен System.Threading.Tasks для рефакторингу методів (додається як DoWorkAsync()). По суті, замість того, щоб одразу повертати конкретне значення, що повертається (рядковий об'єкт у цьому прикладі), ви повертаєте об'єкт Task<T>, де параметр універсального типу T є базовим, фактичним значенням, що повертається. Якщо метод не має значення, що повертається, то замість Task<T> ви просто використовуєте Task.
Реалізація DoWorkAsync() тепер безпосередньо повертає об'єкт Task<T>, який є значенням, що повертається Task.Run(). Метод Run() приймає делегат Func<> або Action<>, і, як ви вже знаєте, ви можете спростити собі життя, використовуючи лямбда-вираз. По суті, ваша нова версія фактично говорить наступне:

Коли ви мене викличете, я запущу нове завдання. Це завдання переведе викликаючий потік у режим сну на п'ять секунд, а після його завершення поверне мені рядкове значення. Я помістю цей рядок у новий об'єкт Task\<string\> та поверну його викликаючій стороні.

Переклавши цю нову реалізацію DoWorkAsync() на більш природну мову, ви отримаєте деяке уявлення про справжню роль токена await. Це ключове слово завжди змінюватиме метод, який повертає об'єкт Task. Коли потік логіки досягає токена await, викликаючий потік у цьому методі призупиняється до завершення виклику. Таким чином додадкове завдання працює в вториному потоці і вам не треба за це турбуватись. Якби це був графічний застосунок, користувач міг би продовжувати використовувати інтерфейс користувача, поки виконується метод DoWorkAsync().

Ми можемо запустити декілька завдань не дочекавшись їх завершення, в різних потоках.

```cs

while (true)
{
    //string taskResult = await DoWorkAsync();
    //Console.WriteLine(taskResult);
    _ = DoWorkAsync();

    Console.Write($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing:");
    Console.ReadLine();
}
```

```
Thread 1 says: Enter somthing:
        I star to do long work asynchronous! Thread:6

Thread 1 says: Enter somthing:
        I star to do long work asynchronous! Thread:7

Thread 1 says: Enter somthing:
        I star to do long work asynchronous! Thread:4

Thread 1 says: Enter somthing:
        I star to do long work asynchronous! Thread:10
```



## SynchronizationContext та async/await

Офіційне визначення SynchronizationContext – це базовий клас, який надає контекст із вільними потоками без синхронізації. Хоча це початкове визначення не дуже описове, в офіційній документації далі йдеться:

Мета моделі синхронізації, реалізованої цим класом, полягає в тому, щоб забезпечити правильну роботу внутрішніх асинхронних/синхронних операцій середовища виконання загальномовних програм з різними моделями синхронізації.

Це твердження, разом з тим, що ви знаєте про багатопоточність, проливає світло на це питання. Нагадаємо, що графічні програми (WinForms, WPF) не дозволяють вторинним потокам безпосередньо отримувати доступ до елементів керування, а повинні делегувати цей доступ. Ми вже розглядали об'єкт Dispatcher у прикладі WPF. Для консольних застосунків, це обмеження не діє. Це різні моделі синхронізації, про які йдеться. З огляду на це, давайте глибше розглянемо SynchronizationContext.
SynchonizationContext — це тип, який надає метод віртуальної публікації, який приймає делегат для асинхронного виконання. Це забезпечує шаблон для фреймворків для належної обробки асинхронних запитів (відправлення для WPF/WinForms, безпосереднє виконання для неграфічних застосунків тощо). Це надає спосіб поставити одиницю роботи в чергу для контексту та веде облік невиконаних асинхронних операцій.
Як ми обговорювали раніше, коли делегат ставиться в чергу для асинхронного виконання, його запуск планується в окремому потоці. Ця деталізація обробляється середовищем виконання .NET Core. Зазвичай це робиться за допомогою керованого пулу потоків .NET Core Runtime, але це можна перевизначити за допомогою власної реалізації.
Хоча цю роботу можна керувати вручну за допомогою коду, шаблон async/await виконує більшу частину важкої роботи. Коли очікується асинхронний метод, він використовує реалізації SynchronizationContext та TaskScheduler цільового фреймворку. Наприклад, якщо ви використовуєте async/await у WPF-застосунку, WPF-фреймворк керує відправленням делегата та зворотним викликом кінцевого автомата після завершення завдання awaited для безпечного оновлення елементів керування.

## Роль ConfigureAwait

Тепер, коли ви трохи краще розумієте SynchronizationContext, настав час розглянути роль методу ConfigureAwait(). За замовчуванням очікування завдання призведе до використання контексту синхронізації. Під час розробки графічних застосунків (WinForms, WPF) саме така поведінка вам потрібна. Під час розробки графічних застосунків (WinForms, WPF) саме така поведінка вам потрібна. Щоб побачити це в дії, оновіть свої оператори верхнього рівня наступним чином:

```cs
string message = await DoWorkAsync();
Console.WriteLine($"0 - {message}");
string message1 = await DoWorkAsync().ConfigureAwait(false);
Console.WriteLine($"1 - {message1}");
```


```

        I star to do long work asynchronous! Thread:6
0 -     Done with work! Thread:6


        I star to do long work asynchronous! Thread:6
1 -     Done with work! Thread:6
```

Початковий блок коду використовує SynchronizationContext, наданий фреймворком (у цьому випадку, середовищем виконання .NET Core). Це еквівалентно виклику ConfigureAwait(true). Другий приклад ігнорує поточний контекст і планувальник.
Рекомендації команди .NET Core пропонують під час розробки коду програми (WinForms, WPF тощо) залишати поведінку за замовчуванням.Якщо ви пишете не-програмний код (наприклад, код бібліотеки), використовуйте ConfigureAwait(false). ASP.NET Core не створює власний SynchronizationContext; тому ConfigureAwait(false) не забезпечує переваги під час використання інших фреймворків.

В нашому випадку ми можемо ми можемо виклик асінхроного метода чеканя його виконнання і вивід результату відокремити в окреме завдання:

```cs
static async Task CallDoWorkAsync()
{
    string taskResult = await DoWorkAsync();
    Console.WriteLine(taskResult);
}
```
```cs
while (true)
{
    _ = CallDoWorkAsync();
    Console.Write($"Thread {Thread.CurrentThread.ManagedThreadId} says: Enter somthing:");
    Console.ReadLine();
}
```
```
Thread 1 says: Enter somthing:
        I star to do long work asynchronous! Thread:6

Thread 1 says: Enter somthing:
        I star to do long work asynchronous! Thread:4

Thread 1 says: Enter somthing:
        I star to do long work asynchronous! Thread:9
        Done with work! Thread:9

        Done with work! Thread:4

        Done with work! Thread:6
```
Тут видно що з первинного потоку створюються окремі потоки в яких виконуються завдання.


## Правила іменування асинхронних методів

Звісно, ​​ви помітили зміну назви з DoWork() на DoWorkAsync(), але чому саме ця зміна? Що якщо викликати асінхронний метод так :

```cs
string message = DoWorkAsync();
```

На цьому етапі виникнуть помилки компілятора, оскільки повернене значення DoWork() є об'єктом Task, який ви намагаєтеся призначити безпосередньо рядковій змінній. Пам'ятайте, що токен await витягує внутрішнє повернене значення, що міститься в об'єкті Task. Оскільки ви не використовували цей токен, у вас виникне невідповідність типів.

    «awaitable» метод — це просто метод, який повертає Task або Task<T>.

Враховуючи, що методи, що повертають об'єкти Task, тепер можна викликати неблокуючим чином через токени async та await, рекомендується називати будь-який метод, що повертає Task, суфіксом Async. Таким чином, розробники, які знають правила іменування, отримують візуальне нагадування про те, що ключове слово await є обов'язковим, якщо вони мають намір викликати метод в асинхронному контексті.

    Обробники подій для елементів керування графічним інтерфейсом (наприклад, обробник кліку кнопки), а також методи дій у програмах у стилі MVC, які використовують ключові слова async/await, не дотримуються цієї домовленості про іменування.

## Асинхронні методи, що не повертають дані

Наразі ваш метод DoWorkAsync() повертає Task\<string\>, який містить «реальні дані» для викликаючої сторони, що будуть прозоро отримані через ключове слово await. Однак, що робити, якщо ви хочете створити асинхронний метод, який нічого не повертає? Хоча існує два способи зробити це, насправді існує лише один правильний спосіб. Спочатку давайте розглянемо проблеми з визначенням асинхронного методу void.

### Async Void Methods

Нижче наведено приклад асинхронного методу, який використовує void як тип повернення замість Task:

```cs
static async void MethodReturningVoidAsync()
{

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        /* Do some work here... */
        Thread.Sleep(4_000);
        Console.WriteLine($"Thread: {threadId}");
    });
    Console.WriteLine($"Fire and forget void method completed");
}
```

Якщо викликати цей метод, він працюватиме самостійно, не блокуючи основний потік. Наступний код покаже повідомлення «Завершено» перед повідомленням методу MethodReturningVoidAsync():

```cs
MethodReturningVoidAsync();
Console.WriteLine($"Completed Thread: {Thread.CurrentThread.ManagedThreadId}");
Console.ReadLine();
```
```
Completed Thread: 1
Thread: 6
Fire and forget void method completed
```
Хоча це може здатися життєздатним варіантом для сценаріїв «запустив і забув», є більша проблема. Якщо метод викидає виняток, йому нікуди подітися, окрім контексту синхронізації викликаючого методу. Оновіть метод до наступного:

```cs
static async void MethodReturningVoidAsync()
{

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        /* Do some work here... */
        Thread.Sleep(4_000);
        Console.WriteLine($"Thread: {threadId}");
        throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Fire and forget void method completed");
}
```
```
Completed Thread: 1
Thread: 6
Unhandled exception. System.Exception: Something bad happened
   at Program.<>c.<<Main>$>b__0_4() in D:\...\Program.cs:line 60
   at System.Threading.Tasks.Task`1.InnerInvoke()
   ...
```
Для безпеки оберніть виклик цього методу в блок try-catch і запустіть програму ще раз:

```cs
try
{
    MethodReturningVoidAsync();
    Console.WriteLine($"Completed Thread: {Thread.CurrentThread.ManagedThreadId}");
    Console.ReadLine();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

```
Блок catch не лише не перехоплює виняток, але й сам виняток розміщується в контексті виконання потоків. Тож, хоча це може здатися хорошою ідеєю для сценаріїв «запустив і забув», вам краще сподіватися, що в методі async void не буде створено винятку, інакше вся ваша програма може вийти з ладу.

### Асинхронні методи void з використанням Task

Краще, щоб ваш метод використовував Task замість void. Оновіть метод до наступного:

```cs
static async void MethodReturningVoidTaskAsync()
{

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        /* Do some work here... */
        Thread.Sleep(4_000);
        Console.WriteLine($"Thread: {threadId}");
        //throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Void method completed");
}
```
Якщо викликати метод with без ключового слова await, буде отримано той самий результат, що й у попередньому прикладі:

```cs
static async Task MethodReturningVoidTaskAsync()
{

    await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        /* Do some work here... */
        Thread.Sleep(4_000);
        Console.WriteLine($"Thread: {threadId}");
    });
    Console.WriteLine($"Void method completed");
}

```
Якщо викликати метод with без ключового слова await, буде отримано той самий результат, що й у попередньому прикладі:
```cs
MethodReturningVoidTaskAsync();
Console.WriteLine($"Completed Thread: {Thread.CurrentThread.ManagedThreadId}");
Console.ReadLine();
```
```
Completed Thread: 1
Thread: 6
Void method completed

```
Оновіть MethodReturningVoidTaskAsync() для виклику винятку:

```cs
static async Task MethodReturningVoidTaskAsync()
{
    await Task.Run(() =>
    {
        //...
        throw new Exception("Something bad happened");
    });
    Console.WriteLine($"Void method completed");
}
```
Тепер оберніть виклик цього методу в блок try-catch:

```cs
try
{
    MethodReturningVoidTaskAsync();
    Console.WriteLine($"Completed Thread: {Thread.CurrentThread.ManagedThreadId}");
    Console.ReadLine();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

```
Коли ви запускаєте програму і виникає виняток, відбуваються дві цікаві речі. По-перше, вся програма не аварійно завершує роботу, а по-друге, блок catch не перехоплює виняток. Коли виняток викидається методом Task/Task<T>, він перехоплюється та поміщається до об'єкта Task. Під час використання await виняток (або AggregateException) доступний для обробки. Оновіть код виклику, щоб він очікував на метод, і блок catch тепер працюватиме належним чином:

```cs
try
{
    await MethodReturningVoidTaskAsync();
    //...
}
```
```
Thread: 6
Something bad happened
```
Підсумовуючи, вам слід уникати створення методів async void та використовувати методи async Task. Чи вимагати await, це бізнес-рішення, але в будь-якому випадку, принаймні ви не призведете до збою вашої програми. Тому попередній приклад з використанням CallDoWork не найкраша практика і я  його використав врашовуючи його простоту.

## Асинхронні методи з кількома await

Цілком допустимо, щоб один асинхронний метод мав кілька контекстів очікування у своїй реалізації. Наступний приклад показує це в дії:

```cs
static async Task MultipleAwaits()
{
    await Task.Run(() => { Thread.Sleep(2_000); });
    Console.WriteLine($"Done with first task! {Thread.CurrentThread.ManagedThreadId}");
    await Task.Run(() => { Thread.Sleep(2_000); });
    Console.WriteLine($"Done with second task! {Thread.CurrentThread.ManagedThreadId}");
    await Task.Run(() => { Thread.Sleep(2_000); });
    Console.WriteLine($"Done with third task! {Thread.CurrentThread.ManagedThreadId}");
}

var watch = Stopwatch.StartNew();
await MultipleAwaits();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
```
```
Done with first task! 6
Done with second task! 6
Done with third task! 7
Time: 6066
```
Знову ж таки, тут кожне завдання не робить набагато більше, ніж призупинення поточного потоку на певний час; проте будь-яка одиниця роботи може бути представлена ​​цими завданнями (виклик веб-сервісу, читання бази даних тощо).
Інший варіант — не чекати на кожне завдання, а чекати на всі разом і повертати, коли всі завдання будуть виконані. Це більш імовірний сценарій, коли є три речі (перевірка електронної пошти, оновлення сервера, завантаження файлів), які потрібно виконати пакетно, але можна зробити паралельно. Ось код, оновлений за допомогою методу Task.WhenAll():

```cs
static async Task MultipleAwaitsAsync()
{
    await Task.WhenAll(
            Task.Run(() =>
            {
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with first task! {Thread.CurrentThread.ManagedThreadId}");
            }), 
            Task.Run(() =>
            {
                Thread.Sleep(1_000);
                Console.WriteLine($"Done with second task! {Thread.CurrentThread.ManagedThreadId}");
            }), 
            Task.Run(() =>
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                Thread.Sleep(2_000);
                Console.WriteLine($"Done with third task! {Thread.CurrentThread.ManagedThreadId}");
            })
     );
}

var watch = Stopwatch.StartNew();
await MultipleAwaitsAsync();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
```
Коли ви зараз запустите програму, ви побачите, що три завдання запускаються в порядку найменшого часу сну.
```
Done with second task! Thread:7
Done with third task! Thread:8
Done with first task! Thread:6
Time: 2010
```
Повертаючись до прикладу з початку розділу виконаємо DoWork() подібним чином.

```cs
static async Task WhenAllDoWork()
{
    await Task.WhenAll(
        Task.Run(() => DoWork()),
        Task.Run(() => DoWork()),
        Task.Run(() => DoWork()));
}

var watch = Stopwatch.StartNew();
await WhenAllDoWork();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
```
```

        I star to do long work! Thread:6

        I star to do long work! Thread:8

        I star to do long work! Thread:7
Time: 5032

```
Таким чином виконання виконується параллельно.

Також існує WhenAny(), який сигналізує про завершення одного із завдань. Метод повертає перше завершене завдання. Щоб продемонструвати WhenAny():

```cs
static async Task MultipleAwaitsWhenAnyAsync()
{
    await Task.WhenAny(
        Task.Run(() =>
        {
            Thread.Sleep(2_000);
            Console.WriteLine("Done with first task!");
        }), 
        Task.Run(() =>
        {
            Thread.Sleep(1_000);
            Console.WriteLine("Done with second task!");
        }), 
        Task.Run(() =>
        {
            Thread.Sleep(2_000);
            Console.WriteLine("Done with third task!");
        })
    );
}

var watch = Stopwatch.StartNew();
await MultipleAwaitsWhenAnyAsync();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");

```
```
Done with second task!
Time: 1023
```
Кожен із цих методів також працює з масивом завдань. Щоб продемонструвати це, створіть новий метод з назвою MultipleAwaitsWithListTaskAsync(). У цьому методі створіть List<Task>, додайте до нього три завдання, а потім викличте Task.WhenAll() або Task.WhenAny():

```cs
static async Task MultipleAwaitsWithListTaskAsync()
{
    var tasks = new List<Task>();
    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(2_000);
        Console.WriteLine("Done with first task!");
    }));
    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(1_000);
        Console.WriteLine("Done with second task!");
    }));
    tasks.Add(Task.Run(() =>
    {
        Thread.Sleep(2_000);
        Console.WriteLine("Done with third task!");
    }));
    //await Task.WhenAny(tasks);
    await Task.WhenAll(tasks);
}

var watch = Stopwatch.StartNew();
await MultipleAwaitsWithListTaskAsync();
watch.Stop();
Console.WriteLine($"Time: {watch.ElapsedMilliseconds}");
```
```
Done with second task!
Done with first task!
Done with third task!
Time: 2017
```

## Виклик асинхронних методів із синхронних методів

У кожному з попередніх прикладів використовувалося ключове слово async для повернення потоку до виклику коду під час виконання методу async. У огляді, ключове слово await можна використовувати лише в методі, позначеному як async. Що робити, якщо ви не можете (або не хочете) позначити метод як async?
Існують способи викликати асинхронні методи в синхронному контексті. Більшість із них погані. 
Перший варіант — просто відмовитися від ключового слова await, що дозволить початковому потоку продовжувати виконання, поки асинхронний метод працює в окремому потоці, ніколи не повертаючись до викликаючого кода. Це поводиться подібно до попереднього прикладу виклику асинхронних методів Task. Будь-які значення, які повертає метод, втрачаються, а винятки проковтуються.
Це може відповідати вашим потребам, але якщо ні, у вас є три варіанти. Перший — просто використовувати властивість Result для Task<T> або метод Wait() для методів Task. Якщо метод завершується невдачею, будь-які винятки огортаються винятком AggregateException, що потенційно ускладнює обробку помилок. Ви також можете викликати GetAwaiter().GetResult(). Це поводиться так само, як і виклики Wait() та Result, з невеликою різницею, що винятки не обгортаються в AggregateException. Хоча методи GetAwaiter().GetResult() працюють як з методами з поверненим значенням, так і з методами без поверненого значення, у документації вони позначені як «не для зовнішнього використання», що означає, що вони можуть змінитися або зникнути в майбутньому.
Хоча ці два варіанти здаються нешкідливою заміною використання await в асинхронному методі, існує серйозніша проблема з їх використанням. Виклик Wait(), Result або GetAwaiter().GetResult() блокує потік, що викликає, обробляє метод async в іншому потоці, а потім повертається назад до потоку, що викликає, зв'язуючи два потоки для виконання роботи. Ще гірше те, що кожен з цих процесів може спричинити взаємоблокування, особливо якщо потік, що викликає, знаходиться в інтерфейсі користувача програми.
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
string message = joinableTaskFactory.Run(async () => await DoWorkAsync());
Console.WriteLine(message);
```
```

        I star to do long work asynchronous! Thread:4
        Done with work! Thread:4

```

Як ви знаєте, метод DoWorkAsync() повертає Task<string>, і це значення дійсно повертається методом Run(). Ви також можете викликати методи, які просто повертають Task, наступним чином:

```cs
try
{
    joinableTaskFactory.Run(async () =>
    {
        await MethodReturningVoidTaskAsync();
        //await SomeOtherAsyncMethod();
    });
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

```
```
Thread: 4
Something bad happened
```

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
## Узагальнені типи повернення асинхронних значень

Існують додаткові типи повернення, якщо вони відповідають шаблону асинхронізації. Одним конкретним прикладом є ValueTask. Щоб побачити це в дії, створіть такий код:

```cs
static async ValueTask<int> ReturnAnInt()
{
    await Task.Delay(3_000);
    return 5;
}

//Console.WriteLine( ReturnAnInt() );
Console.WriteLine(await ReturnAnInt());
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


## Приклад застосування async/await в GUI

Для демонстрації ми використаємо той самий проект WPF, що й раніше в цьому розділі. Створемо проект типу WPF Application з назвою PictureHandlerWithAsyncAwait. Встановити пакет System.Drawing.Common. Tools > NuGet Package Manager > Manage NuGet Packages for Solution > В рядку пошуку System.Drawing.Common > Install
Для тестування в католог D:\Temp\Pitures скопіюємо декілька будь-яких зображень з розширенням *.jpg.
Змінемо файл MainWindow.xalm і додайте обробники подій кнопок.

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
Спробуємо застосувати патерн для довгого завдання. Змінемо обробник для кнопки DoWork

```cs
        private void DoWork(int interval)
        {
            Thread.Sleep(interval); // Emulation the long work
        }

        private async void cmdDoWork_Click(object sender, RoutedEventArgs e)
        {
            await Task.Run(() =>
            {
                int threadId = Thread.CurrentThread.ManagedThreadId;
                Dispatcher?.Invoke(() => { Title = $"Start work in thread:{threadId} ...";});
                DoWork(10000);
                Dispatcher?.Invoke(() => { Title = $"Work completed!"; });
            });
        }
```

Якшо ви натисните на DoWork інтерфейс не блокується і можна продовжувати вводити текст покі завдання виконується в окремому потоці.

## Скасування операцій async/await

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
        private async Task ProcessFileAsync(string currentFile,string outputDirectory, CancellationToken token)
        {
            string filename = Path.GetFileName(currentFile);
            using Bitmap bitmap = new(currentFile);
            try
            {
                await Task.Run(() =>
                {
                    Dispatcher?.Invoke(() => { Title = $"Processing in thread:{threadId} {filename}"; });
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

Процес обробки зображень такий самий, як і в попередньому прикладі: отримати каталог зображень, створити вихідний каталог, отримати файли зображень, повернути їх та зберегти в новому каталозі.
Замість використання Parallel.ForEach() у цій новій версії для виконання роботи використовуватимуть асинхронні методи, а сигнатури методів прийматимуть CancellationToken як параметр.

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
Після початкового налаштування код перебирає файли та асинхронно викликає ProcessFileAsync() для кожного файлу. Виклик ProcessFileAsync() обгортається блоком try/catch, а CancellationToken передається в метод ProcessFile(). Якщо Cancel() виконується на CancellationTokenSource (наприклад, коли користувач натискає кнопку «Cancel»), виникає виняток OperationCanceledException. При виникнені винятку цикл переривається. Подія настисканя на виконання обробки не впливає на користувацький інтерфейс бо все відбувається в вторинному робочому потоці. Хоча інтерфейс не блокується обробка зображень відбувається послідовно в окремомоу потоці якій зупиняється і чекає.

## Асінхроний метод Parallel.ForEachAsync.

В класі Paralell є метод ForEachAsync який є асінхроним і дозволяє використати асінхроний метод для тіла циклу.
Додамо в проект обробник натискання.

```cs
        private async void cmdProcessWithForEachAsync_Click(object sender, RoutedEventArgs e)
        {
            await ProcessWithForEachAsync();
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
                    token.ThrowIfCancellationRequested();
                    string filename = Path.GetFileName(currentFile);

                    //For title
                    int threadId = Environment.CurrentManagedThreadId;
                    Dispatcher?.Invoke(() =>
                    {
                        Title = $"Processing in thread:{threadId}   File:{filename}";
                    });

                    using Bitmap bitmap = new Bitmap(currentFile);

                    bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    bitmap.Save(Path.Combine(outputDirectory, filename));
                });
                Dispatcher?.Invoke(() => Title = "Process complite.");
            }
            catch (OperationCanceledException ex)
            {
                Dispatcher?.Invoke(() => { Title = $"Process canceled! {ex.Message}"; });
            }
        }
```
В об'єкті ParallelOptions зберігаеться послання на властивість CancellationTokenSource.Token якій відповідає для скасування виконнання циклу. Тіло цилу використовує асінхроний метод який вказано у вигляді лямбда виразу. Можливо в цьому випадку краще було б зробити окрмий метод замість лямбда-виразу. Як видно завдання повністью відпрацьовується швидше з використання Parallel.ForEachAsync ніж коли обробляється коже окреме зображення.

##  Скасування в патерні async/await за допомогою методу WaitAsync().

Асинхронні виклики можна скасувати за допомогою токена скасування та/або після досягнення ліміту часу за допомогою методу WaitAsync(). Повернемся до проекту WorkWithAsync. 

Ми маємо метод.

```cs
static async Task<string> DoWorkAsync()
{
    return await Task.Run(() =>
    {
        int threadId = Thread.CurrentThread.ManagedThreadId;
        Console.WriteLine($"\n\tI star to do long work asynchronous! Thread:{threadId}");
        Thread.Sleep(5000); // Emulation the long work
        return $"\tDone with work! Thread:{threadId}\n";
    });
}
```
Скасувати завдання можна за допомогою методу WaitAsync().
```cs
async Task UsingWaitAsync()
{
    CancellationTokenSource cancellationTokenSource = new();

    try
    {
        string message = await DoLongWorkAsync().WaitAsync(TimeSpan.FromSeconds(12));
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        string message = await DoLongWorkAsync().WaitAsync(TimeSpan.FromSeconds(2));
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        string message = await DoLongWorkAsync().WaitAsync(cancellationTokenSource.Token);
        await Console.Out.WriteLineAsync(message);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }


    cancellationTokenSource.Cancel();

    try
    {
        _ = await DoLongWorkAsync().WaitAsync(cancellationTokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        _ = await DoLongWorkAsync().WaitAsync(TimeSpan.FromSeconds(2),cancellationTokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
await UsingWaitAsync();
```
```

        I star to do long work asynchronous! Thread:6
        Done with work! Thread:6


        I star to do long work asynchronous! Thread:7
The operation has timed out.

        I star to do long work asynchronous! Thread:11
        Done with work! Thread:11


        I star to do long work asynchronous! Thread:7
A task was canceled.

        I star to do long work asynchronous! Thread:6
A task was canceled.
```

## Скасування операцій async/await у синхронних викликах

Метод Wait() також може приймати токен скасування під час виклику асинхронних методів з неасинхронного методу.
Це можна використовувати з тайм-аутом або без нього. При використанні з тайм-аутом, тайм-аут має бути в мілісекундах:

```cs
void UsingWait()
{
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    MethodReturningVoidTaskAsync().Wait(tokenSource.Token);
    MethodReturningVoidTaskAsync().Wait(10000, tokenSource.Token);

    tokenSource.Cancel();

    try
    {
       MethodReturningVoidTaskAsync().Wait(tokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    try
    {
        MethodReturningVoidTaskAsync().Wait(2000, tokenSource.Token);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
UsingWait();
```
```
Thread: 6
Void method completed
Thread: 6
Void method completed
The operation was canceled.
The operation was canceled.
```
Ви також можете використовувати JoinableTaskFactory та метод WaitAsync() під час виклику із синхронного коду:

```cs
void UsingWaitAsyncInSync()
{
    JoinableTaskFactory joinableTaskFactory = new JoinableTaskFactory(new JoinableTaskContext());
    CancellationTokenSource tokenSource = new CancellationTokenSource();
    try
    {
        joinableTaskFactory.Run(async () =>
        {
            await MethodReturningVoidTaskAsync().WaitAsync(tokenSource.Token);
            await MethodReturningVoidTaskAsync().WaitAsync(TimeSpan.FromSeconds(2), tokenSource.Token);
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
Thread: 4
Void method completed
The operation has timed out.
```
## Асинхронні streams(потоки)

Потоки (більш детально описані в іншій главі) можуть створюватися та використовуватися асинхронно. Метод, який повертає асинхронний потік

1. Оголошується з модифікатором async
2. Повертає IAsyncEnumerable<T>
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
Метод оголошено як async, повертає IAsyncEnumerable\<int\> та використовує yield return для повернення цілих чисел з послідовності.Щоб викликати цей метод, додайте наступний код до коду виклику:

```cs
await foreach (var number in GenerateSequence())
{
    Console.WriteLine(number);
}
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
        //await Console.Out.WriteLineAsync("Download complite");
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

У цьому розділі було багато прикладів; ось ключові моменти цього розділу:

1. Методи (а також лямбда-вирази або анонімні методи) можна позначити ключовим словом async, щоб метод міг виконувати роботу без блокування.

2. Методи (а також лямбда-вирази або анонімні методи), позначені ключовим словом async, будуть виконуватися синхронно, доки не буде знайдено ключове слово await.

3. Один асинхронний метод може мати кілька контекстів await.

4. Коли зустрічається вираз await, викликаючий потік призупиняється до завершення очікуваного завдання. Тим часом керування повертається тому, хто викликає метод.

5. Ключове слово await приховає повернений об'єкт Task з поля зору, виглядаючи так, ніби він безпосередньо повертає базове повернене значення. Методи без поверненого значення просто повертають void.

6. Перевірку параметрів та іншу обробку помилок слід виконувати в основній частині методу, а фактичну асинхронну частину переміщувати до приватної функції.

7. Для змінних стеку об'єкт ValueTask ефективніший за Task, що може призвести до упаковки та розпакування.

8. Як правило, методи, які викликаються асинхронно, слід позначати суфіксом Async.

# Підсумки

Ця глава розпочалася з вивчення ролі простору імен System.Threading. Як ви дізналися, коли програма створює додаткові потоки виконання, це призводить до того, що відповідна програма може виконувати численні завдання одночасно. Ви також розглянули кілька способів захисту блоків коду, залежних від потоків, щоб гарантувати, що спільні ресурси не стануть непридатними для використання одиницями фальшивих даних. 
У цьому розділі було розглянуто деякі моделі роботи з багатопотоковою розробкою, зокрема бібліотеку Task Parallel Library та PLINQ. Я завершив розгляд ролі ключових слів async та await. Як ви бачили, ці ключові слова використовують багато типів фреймворку TPL у фоновому режимі; однак компілятор виконує більшу частину роботи зі створення складного коду потоків та синхронізації за вас.