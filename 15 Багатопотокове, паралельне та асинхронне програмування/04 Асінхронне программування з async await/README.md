# Асінхронне программування з async await

Цей матеріал взято з документації Microsoft. Рошуковий запит "Asynchronous programming with async and await"

Модель програмування Task asynchronous programming (TAP) забезпечує рівень абстракції порівняно з типовим асинхронним кодуванням. У цій моделі ви пишете код як послідовність операторів, як завжди. Різниця полягає в тому, що ви можете читати свій код, що базується на завданнях, коли компілятор обробляє кожен оператор і перед тим, як він почне обробляти наступний оператор. Щоб реалізувати цю модель, компілятор виконує багато перетворень для виконання кожного завдання. Деякі оператори можуть ініціювати роботу та повертати об'єкт Task, який представляє поточну роботу, і компілятор повинен виконати ці перетворення. Мета асинхронного програмування задач полягає в тому, щоб забезпечити код, який читається як послідовність операторів, але виконується в складнішому порядку. Виконання базується на розподілі зовнішніх ресурсів та моменті завершення завдань.

Модель асинхронного програмування задач аналогічна тому, як люди надають інструкції для процесів, що містять асинхронні завдання. У цій статті на прикладі інструкцій з приготування сніданку показано, як ключові слова async та await спрощують аналіз коду, який містить низку асинхронних інструкцій. Інструкції з приготування сніданку можна надати у вигляді списку:

    1. Налийте чашку кави.
    2. Розігрійте пательню, потім обсмажте два яйця.
    3. Приготуйте три котлети з картопляних оладок.
    4. Підсмажте два шматки хліба.
    5. Намажте тост маслом та варенням.
    6. Налийте склянку апельсинового соку.

Якщо у вас є досвід кулінарії, ви можете виконувати ці інструкції асинхронно. Ви починаєте розігрівати сковороду для яєць, потім починаєте готувати картопляні оладки. Ви кладете хліб у тостер, а потім починаєте готувати яйця. На кожному кроці процесу ви починаєте завдання, а потім переходите до інших завдань, готових до вашої уваги.

Приготування сніданку – гарний приклад асинхронної роботи, яка не є паралельною. Одна людина (або потік) може виконувати всі завдання. Одна людина може готувати сніданок асинхронно, починаючи наступне завдання до завершення попереднього. Кожне завдання приготування виконується незалежно від того, чи хтось активно спостерігає за процесом. Щойно ви почнете розігрівати сковороду для яєць, можете почати готувати картопляні оладки. Після того, як картопляні оладки почнуть готуватися, можна покласти хліб у тостер.

Для паралельного алгоритму потрібно кілька людей, які готують (або кілька потоків).Одна людина готує яйця, інша — картопляні оладки тощо. Кожна людина зосереджена на своєму конкретному завданні. Кожна людина, яка готує (або кожен потік), блокується синхронно в очікуванні завершення поточного завдання: картопляні оладки готові до перевертання, хліб готовий до появи в тостері тощо. Якшо виконувати всі завдання списку послідовне без відволіканя на інші, тобто сінхронно, все приготування може заняти приблизно 30 хвилин.

Розглянемо той самий список синхронних інструкцій, записаних як оператори коду C#:

Program.cs
```cs
void MakeBreakastSync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tСoffee is ready");

    Egg eggs = FryEggs(2);
    Console.WriteLine("\tEggs are ready");

    HashBrown hashBrown = FryHashBrowns(3);
    Console.WriteLine("\tHash browns are ready");

    Toast toast = ToastBread(2);
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");

}
MakeBreakastSync();
```

Types.cs
```cs
public partial class Program
{
    // These classes are intentionally empty for the purpose of this example.
    // They are simply marker classes for the purpose of demonstration,
    // contain no properties, and serve no other purpose.
    internal class HashBrown { }
    internal class Coffee { }
    internal class Egg { }
    internal class Juice { }
    internal class Toast { }

    private static Coffee PourCoffee()
    {
        Console.WriteLine("Pouring coffee");
        return new Coffee();
    }

     private static Egg FryEggs(int howMany)
    {
        Console.WriteLine("Starting fry eggs. Warming the egg pan...");
        Task.Delay(3000).Wait();
        Console.WriteLine($"cracking {howMany} eggs");
        Console.WriteLine("cooking the eggs ...");
        Task.Delay(3000).Wait();
        Console.WriteLine("Put eggs on plate");

        return new Egg();
    }

    private static HashBrown FryHashBrowns(int patties)
    {
        Console.WriteLine($"Starting fry hash browns. putting {patties} hash brown patties in the pan");
        Console.WriteLine("cooking first side of hash browns...");
        Task.Delay(3000).Wait();
        for (int patty = 0; patty < patties; patty++)
        {
            Console.WriteLine("flipping a hash brown patty");
        }
        Console.WriteLine("cooking the second side of hash browns...");
        Task.Delay(3000).Wait();
        Console.WriteLine("Put hash browns on plate");

        return new HashBrown();
    }

    private static Toast ToastBread(int slices)
    {
        Console.WriteLine("Start toasting...");
        for (int slice = 0; slice < slices; slice++)
        {
            Console.WriteLine("Putting a slice of bread in the toaster");
        }
        Task.Delay(3000).Wait();
        Console.WriteLine("Remove toast from toaster");

        return new Toast();
    }

    private static void ApplyJam(Toast toast) =>
    Console.WriteLine("Putting jam on the toast");
    private static void ApplyButter(Toast toast) =>
        Console.WriteLine("Putting butter on the toast");

    private static Juice PourOJ()
    {
        Console.WriteLine("Pouring orange juice");
        return new Juice();
    }
}
```
```
Pouring coffee
        Сoffee is ready
Starting fry eggs. Warming the egg pan...
cracking 2 eggs
cooking the eggs ...
Put eggs on plate
        Eggs are ready
Starting fry hash browns. putting 3 hash brown patties in the pan
cooking first side of hash browns...
flipping a hash brown patty
flipping a hash brown patty
flipping a hash brown patty
cooking the second side of hash browns...
Put hash browns on plate
        Hash browns are ready
Start toasting...
Putting a slice of bread in the toaster
Putting a slice of bread in the toaster
Remove toast from toaster
Putting butter on the toast
Putting jam on the toast
        Toast is ready
Pouring orange juice
        OJ is ready
                Breakfast is ready!
```
Якщо інтерпретувати ці інструкції як комп'ютер, сніданок готується приблизно 30 хвилин. Тривалість – це сума часу виконання окремих завдань. Комп'ютер блокується для кожного оператора, доки не завершить всю роботу, а потім переходить до наступного оператора завдання. Такий підхід може зайняти багато часу. У прикладі зі сніданком комп'ютерний метод створює незадовільний сніданок. Пізніші завдання у синхронному списку, такі як підсмажування хліба, не починаються, доки не будуть завершені попередні. Деякі продукти охолоджуються до того, як сніданок буде готовий до подачі.

Якщо ви хочете, щоб комп'ютер виконував інструкції асинхронно, ви повинні написати асинхронний код. Коли ви пишете клієнтські програми, вам потрібно, щоб інтерфейс користувача реагував на введення даних користувача. Ваша програма не повинна зависати під час завантаження даних з Інтернету. Коли ви пишете серверні програми, ви не хочете блокувати потоки, які можуть обслуговувати інші запити. Використання синхронного коду за наявності асинхронних альтернатив шкодить вашій здатності до менш витратного масштабування. Ви платите за заблоковані потоки.

Успішні сучасні програми вимагають асинхронного коду. Без підтримки мови написання асинхронного коду вимагає зворотних викликів, подій завершення або інших засобів, які приховують початковий намір коду. Перевагою синхронного коду є покрокова дія, що спрощує його сканування та розуміння. Традиційні асинхронні моделі змушують вас зосереджуватися на асинхронній природі коду, а не на фундаментальних діях коду.

## Не блокуйте, а натомість чекайте

Попередній код висвітлює невдалу практику програмування: написання синхронного коду для виконання асинхронних операцій. Код блокує поточний потік від виконання будь-якої іншої роботи. Результат цієї моделі схожий на те, як дивитися на тостер після того, як поклали хліб. Ви ігноруєте будь-які переривання та не починаєте інші завдання, доки не з'явиться хліб. Ви не виймаєте масло та варення з холодильника. Ви можете не помітити, як на плиті розгорається вогонь. Ви хочете одночасно підсмажити хліб і вирішити інші проблеми. Те саме стосується і вашого коду.

Ви можете почати з оновлення коду, щоб потік не блокувався під час виконання завдань. Ключове слово await забезпечує неблокуючий спосіб запуску завдання, а потім продовження його виконання після його завершення. Проста асинхронна версія коду breakfast виглядає так:

```cs
async Task SimpleMakeBreakastAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    Egg eggs = await FryEggsAsync(2);
    Console.WriteLine("\tEggs are ready");

    HashBrown hashBrown = await FryHashBrownsAsync(3);
    Console.WriteLine("\tHash browns are ready");

    Toast toast = await ToastBreadAsync(2);
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
await SimpleMakeBreakastAync();
```
```cs
public partial class Program
{
    //...

    //Async versions
    private static async Task<Egg> FryEggsAsync(int howMany)
    {
        Console.WriteLine("Starting fry eggs. Warming the egg pan...");
        await Task.Delay(3000);
        Console.WriteLine($"cracking {howMany} eggs");
        Console.WriteLine("cooking the eggs ...");
        await Task.Delay(3000);
        Console.WriteLine("Put eggs on plate");

        return new Egg();
    }

    private static async Task<HashBrown> FryHashBrownsAsync(int patties)
    {
        Console.WriteLine($"Starting fry hash browns. putting {patties} hash brown patties in the pan");
        Console.WriteLine("cooking first side of hash browns...");
        await Task.Delay(3000);
        for (int patty = 0; patty < patties; patty++)
        {
            Console.WriteLine("flipping a hash brown patty");
        }
        Console.WriteLine("cooking the second side of hash browns...");
        await Task.Delay(3000);
        Console.WriteLine("Put hash browns on plate");

        return new HashBrown();
    }
    private static async Task<Toast> ToastBreadAsync(int slices)
    {
        Console.WriteLine("Start toasting...");
        for (int slice = 0; slice < slices; slice++)
        {
            Console.WriteLine("Putting a slice of bread in the toaster");
        }
        await Task.Delay(3000);
        Console.WriteLine("Remove toast from toaster");

        return new Toast();
    }
}
```
Pouring coffee
        Coffee is ready
Starting fry eggs. Warming the egg pan...
cracking 2 eggs
cooking the eggs ...
Put eggs on plate
        Eggs are ready
Starting fry hash browns. putting 3 hash brown patties in the pan
cooking first side of hash browns...
flipping a hash brown patty
flipping a hash brown patty
flipping a hash brown patty
cooking the second side of hash browns...
Put hash browns on plate
        Hash browns are ready
Start toasting...
Putting a slice of bread in the toaster
Putting a slice of bread in the toaster
Remove toast from toaster
Putting butter on the toast
Putting jam on the toast
        Toast is ready
Pouring orange juice
        OJ is ready
                Breakfast is ready!

Код оновлює тіла оригінальних методів FryEggs, FryHashBrowns та ToastBread, щоб повертати об'єкти Task\<Egg\>, Task\<HashBrown\> та Task\<Toast\> відповідно. Оновлені назви методів містять суфікс "Async": FryEggsAsync, FryHashBrownsAsync та ToastBreadAsync. Метод SimpleMakeBreakastAync повертає об'єкт Task, хоча він не має виразу повернення, що є особливим задумом (це обговорювалось в "Асинхронні методи, що не повертають дані" попередня глава). 

    Оновлений код ще не використовує ключові можливості асинхронного програмування, що може призвести до скорочення часу виконання. Код обробляє завдання приблизно за той самий час, що й початкова синхронна версія.

Давайте застосуємо приклад сніданку до оновленого коду. Потік не блокується під час приготування яєць або картопляних оладок, але код також не запускає інші завдання, доки поточна робота не завершиться. Ви все ще кладете хліб у тостер і дивитеся на нього, поки хліб не вискочить, але тепер ви можете реагувати на переривання. У ресторані, де приймається кілька замовлень, кухар може почати нове замовлення, поки інший вже готує.

В оновленому коді потік, який працює над сніданком, не блокується під час очікування будь-якого розпочатого, але незавершеного завдання. Для деяких програм цієї зміни достатньо. Ви можете ввімкнути у своїй програмі підтримку взаємодії з користувачем під час завантаження даних з Інтернету. В інших сценаріях може знадобитися розпочати інші завдання, поки чекає на завершення попереднього. 

## Запускайте завдання одночасно

Для більшості операцій потрібно негайно розпочати кілька незалежних завдань. Коли кожне завдання завершено, ви починаєте іншу роботу, готову до початку. Коли ви застосовуєте цю методологію до прикладу зі сніданком, ви можете приготувати сніданок швидше. Ви також все приготуєте майже одночасно, тож зможете насолодитися гарячим сніданком. 

Клас System.Threading.Tasks.Task та пов'язані з ним типи – це класи, які можна використовувати для застосування цього стилю міркувань до завдань, що вже виконуються. Такий підхід дозволяє вам писати код, який більше нагадує те, як ви готуєте сніданок у реальному житті. Ви починаєте готувати яйця, картоплю та тости одночасно. Оскільки кожен продукт харчування вимагає дії, ви звертаєте свою увагу на це завдання, виконуєте дію, а потім чекаєте на щось інше, що потребує вашої уваги.

У вашому коді ви запускаєте завдання та утримуєте об'єкт Task, який представляє роботу. Ви використовуєте метод await для завдання, щоб відкласти виконання роботи до готовності результату.

Застосуйте ці зміни до коду. Перший крок — зберігати завдання для операцій під час їх запуску, а не використовувати вираз await:

```cs
async Task OtherSimpleMakeBreakastAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    Task<Egg> eggsTask = FryEggsAsync(2);
    Egg eggs = await eggsTask;
    Console.WriteLine("\tEggs are ready");

    Task<HashBrown> hashBrownTask = FryHashBrownsAsync(3);
    HashBrown hashBrown = await hashBrownTask;
    Console.WriteLine("\tHash browns are ready");

    Task<Toast> toastTask = ToastBreadAsync(2);
    Toast toast = await toastTask;
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOj is ready");
    Console.WriteLine("\t\tBreakfast is ready!");

}
await OtherSimpleMakeBreakastAync();
```

Ці зміни не допомагають приготувати ваш сніданок швидше. Вираз await застосовується до всіх завдань, щойно вони починаються. Наступний крок – перемістити вирази await для картопляних оладок та яєць у кінець методу, перед подачею сніданку:

```cs
async Task FasterMakeBreakastAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    Task<Egg> eggsTask = FryEggsAsync(2);
    Task<HashBrown> hashBrownTask = FryHashBrownsAsync(3);
    Task<Toast> toastTask = ToastBreadAsync(2);

    Toast toast = await toastTask;
    ApplyButter(toast);
    ApplyJam(toast);
    Console.WriteLine("\tToast is ready");
    
    Juice oj = PourOJ();
    Console.WriteLine("\tOj is ready");

    Egg eggs = await eggsTask;
    Console.WriteLine("\tEggs are ready");
    
    HashBrown hashBrown = await hashBrownTask;
    Console.WriteLine("\tHash browns are ready");

    Console.WriteLine("\t\tBreakfast is ready!");
}
await FasterMakeBreakastAync();
```
```
Pouring coffee
        Coffee is ready
Starting fry eggs. Warming the egg pan...
Starting fry hash browns. putting 3 hash brown patties in the pan
cooking first side of hash browns...
Start toasting...
Putting a slice of bread in the toaster
Putting a slice of bread in the toaster
cracking 2 eggs
cooking the eggs ...
flipping a hash brown patty
flipping a hash brown patty
flipping a hash brown patty
cooking the second side of hash browns...
Remove toast from toaster
Putting butter on the toast
Putting jam on the toast
        Toast is ready
Pouring orange juice
        Oj is ready
Put eggs on plate
        Eggs are ready
Put hash browns on plate
        Hash browns are ready
                Breakfast is ready!
```

Тепер у вас є асинхронно приготований сніданок, на приготування якого потрібно близько 20 хвилин. Загальний час приготування скорочується, оскільки деякі завдання виконуються одночасно.

Оновлення коду покращують процес приготування, зменшуючи час приготування, але вони вносять регресію через спалювання яєць та картопляних оладок. Ви запускаєте всі асинхронні завдання одночасно. Ви чекаєте на кожне завдання лише тоді, коли вам потрібні результати. Код може бути схожим на програму у веб-застосунку, яка здійснює запити до різних мікросервісів, а потім об'єднує результати на одній сторінці. Ви робите всі запити негайно, а потім застосовуєте вираз await до всіх цих завдань і створюєте веб-сторінку.

## Підтримка композиції за допомогою завдань.

Попередні редакції коду допомагають готувати все до сніданку одночасно, крім тосту. Процес приготування тосту являє собою композицію асинхронної операції (підсмажування хліба) із синхронними операціями (намазування тосту маслом та варенням). Цей приклад ілюструє важливу концепцію асинхронного програмування:

    Композиція асинхронної операції, за якою слідує синхронна робота, є асинхронною операцією. Іншими словами, якщо будь-яка частина операції є асинхронною, то вся операція є асинхронною.

У попередніх оновленнях ви дізналися, як використовувати об'єкти Task або Task\<TResult\> для зберігання запущених завдань. Ви очікуєте на кожне завдання, перш ніж використовувати його результат. Наступний крок — створити методи, що представляють поєднання інших робіт. Перш ніж подавати сніданок, потрібно почекати із завданням, яке представляє собою підсмажування хліба, перш ніж намазувати масло та варення.

Ви можете представити цю роботу за допомогою наступного коду:

```cs
static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
{
    var toast = await ToastBreadAsync(number);
    ApplyButter(toast);
    ApplyJam(toast);

    return toast;
}
```
```cs
Toast otherToast = await MakeToastWithButterAndJamAsync(2);
```
```
Start toasting...
Putting a slice of bread in the toaster
Putting a slice of bread in the toaster
Remove toast from toaster
Putting butter on the toast
Putting jam on the toast
```

Метод MakeToastWithButterAndJamAsync має модифікатор async у своїй сигнатурі, який сигналізує компілятору, що метод містить вираз очікування та асинхронні операції. Метод представляє завдання, яке підсмажує хліб, а потім намазує масло та варення. Метод повертає об'єкт Task\<TResult\>, який представляє композицію трьох операцій.

Перероблений основний блок коду тепер виглядає так:

```cs
async Task ImprovedFasterMakeBreakastAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    var eggsTask = FryEggsAsync(2);
    var hashBrownTask = FryHashBrownsAsync(3);
    var toastTask = MakeToastWithButterAndJamAsync(2);

    var eggs = await eggsTask;
    Console.WriteLine("\tEggs are ready");

    var hashBrown = await hashBrownTask;
    Console.WriteLine("\tHash browns are ready");

    var toast = await toastTask;
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
await ImprovedFasterMakeBreakastAync();
```
```
Pouring coffee
        Coffee is ready
Starting fry eggs. Warming the egg pan...
Starting fry hash browns. putting 3 hash brown patties in the pan
cooking first side of hash browns...
Start toasting...
Putting a slice of bread in the toaster
Putting a slice of bread in the toaster
flipping a hash brown patty
flipping a hash brown patty
flipping a hash brown patty
cooking the second side of hash browns...
cracking 2 eggs
cooking the eggs ...
Remove toast from toaster
Putting butter on the toast
Putting jam on the toast
Put hash browns on plate
Put eggs on plate
        Eggs are ready
        Hash browns are ready
        Toast is ready
Pouring orange juice
        OJ is ready
                Breakfast is ready!
```
Ця зміна коду ілюструє важливий прийом роботи з асинхронним кодом. Ви створюєте завдання, розділяючи операції в новий метод, який повертає завдання. Ви можете вибрати, коли чекати на це завдання. Ви можете запускати інші завдання одночасно.

## Обробка асинхронних винятків

До цього моменту ваш код неявно припускає, що всі завдання завершуються успішно. Асинхронні методи викидають винятки, як і їхні синхронні аналоги. Цілі асинхронної підтримки винятків та обробки помилок такі ж, як і для асинхронної підтримки загалом. Найкраща практика — писати код, який читається як послідовність синхронних операторів. Завдання викликають винятки, коли не можуть бути успішно завершені. Клієнтський код може перехоплювати ці винятки, коли вираз await застосовується до запущеного завдання.

У прикладі зі сніданком, припустимо, що тостер загоряється під час підсмажування хліба. Ви можете змоделювати цю проблему, змінивши метод ToastBreadAsync відповідно до наступного коду:

```cs
    private static async Task<Toast> ToastBreadWithFireAsync(int slices, bool onFire)
    {
        Console.WriteLine("Start toasting...");
        for (int slice = 0; slice < slices; slice++)
        {
            Console.WriteLine("Putting a slice of bread in the toaster");
        }
        await Task.Delay(2000);
        if (onFire)
        {
            Console.WriteLine("Fire! Toast is ruined!");
            throw new InvalidOperationException("The toaster is on fire");
        }
        await Task.Delay(1000);
        Console.WriteLine("Remove toast from toaster");
        return new Toast();
    }

    static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
    {
        var toast = await ToastBreadWithFireAsync(number,true);
        ApplyButter(toast);
        ApplyJam(toast);

        return toast;
    }


```
Після внесення змін до коду запустіть програму та перевірте результат:

```
Pouring coffee
        Coffee is ready
Starting fry eggs. Warming the egg pan...
Starting fry hash browns. putting 3 hash brown patties in the pan
cooking first side of hash browns...
Start toasting...
Putting a slice of bread in the toaster
Putting a slice of bread in the toaster
Fire! Toast is ruined!
flipping a hash brown patty
flipping a hash brown patty
flipping a hash brown patty
cooking the second side of hash browns...
cracking 2 eggs
cooking the eggs ...
Put eggs on plate
Put hash browns on plate
        Eggs are ready
        Hash browns are ready
Unhandled exception. System.InvalidOperationException: The toaster is on fire
   at Program.ToastBreadWithFireAsync(Int32 slices, Boolean onFire) in D:\...\Types.cs:line 122
   at Program.MakeToastWithButterAndJamAsync(Int32 number) in D:\...\Types.cs:line 131
   at Program.<<Main>$>g__ImprovedFasterMakeBreakastAync|0_4() in D:\...\Program.cs:line 115
   at Program.<Main>$(String[] args) in D:\...\Program.cs:line 122
   at Program.<Main>(String[] args)
```
Зверніть увагу, що чимало завдань завершуються між моментом, коли тостер загоряється, і моментом, коли система помічає виняток. Коли завдання, що виконується асинхронно, викидає виняток, це завдання вважається помилковим. Об'єкт Task містить виняток, що викидається, у властивості Task.Exception. Завдання з помилками викликають виняток, коли до завдання застосовується вираз await. 

Існує два важливі механізми цього процесу, які слід зрозуміти:

    1. Як виняток зберігається в завданні, що завершилося помилкою
    2. Як виняток розпаковується та повторно генерується, коли код очікує (await) на невдалому завданні

Коли код, що виконується асинхронно, викидає виняток, цей виняток зберігається в об'єкті Task. Властивість Task.Exception є об'єктом System.AggregateException, оскільки під час асинхронної роботи може бути викинуто більше одного винятку. Будь-який викинутий виняток додається до колекції AggregateException.InnerExceptions. Якщо властивість Exception має значення null, створюється новий об'єкт AggregateException, а викинутий виняток є першим елементом у колекції.

Найпоширеніший сценарій для завдання з помилкою полягає в тому, що властивість Exception містить рівно один виняток. Коли ваш код очікує на завдання, що виявилося несправним, він повторно генерує перший виняток AggregateException.InnerExceptions у колекції. Цей результат є причиною того, що вивід прикладу показує об'єкт System.InvalidOperationException, а не об'єкт AggregateException. Вилучення першого внутрішнього винятку робить роботу з асинхронними методами максимально схожою на роботу з їхніми синхронними аналогами. Ви можете перевірити властивість Exception у своєму коді, якщо ваш сценарій може генерувати кілька винятків. 

    Рекомендована практика полягає в тому, щоб будь-які винятки перевірки аргументів виникали синхронно з методів, що повертають завдання. Пошуковий запит "Exceptions in task-returning methods" 

Зловимо виняток.

```cs
async Task BreakastWithFireAync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    try
    {
        var eggsTask = FryEggsAsync(2);
        var hashBrownTask = FryHashBrownsAsync(3);
        var toastTask = MakeToastWithButterAndJamAsync(2);

        var eggs = await eggsTask;
        Console.WriteLine("\tEggs are ready");

        var hashBrown = await hashBrownTask;
        Console.WriteLine("\tHash browns are ready");

        var toast = await toastTask;
        Console.WriteLine("\tToast is ready");
    }catch(Exception ex)
    {
        Console.WriteLine(ex.Message.ToString().ToUpper());
    }

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
await BreakastWithFireAync();
```
```
Pouring coffee
        Coffee is ready
Starting fry eggs. Warming the egg pan...
Starting fry hash browns. putting 3 hash brown patties in the pan
cooking first side of hash browns...
Start toasting...
Putting a slice of bread in the toaster
Putting a slice of bread in the toaster
Fire! Toast is ruined!
flipping a hash brown patty
flipping a hash brown patty
flipping a hash brown patty
cooking the second side of hash browns...
cracking 2 eggs
cooking the eggs ...
Put eggs on plate
Put hash browns on plate
        Eggs are ready
        Hash browns are ready
THE TOASTER IS ON FIRE
Pouring orange juice
        OJ is ready
                Breakfast is ready!
```
Таким чином ми отримали сніданок без тостів і тостера.

Перш ніж перейти до наступного розділу, змініть метод ToastBreadAsync. Ви ж не хочете розпочати ще одну пожежу:

```cs
var toast = await ToastBreadWithFireAsync(number,false);
```

## Ефективне застосування виразів await до завдань

Ви можете покращити серію виразів await в кінці попереднього коду, використовуючи методи класу Task. Одним із API є метод WhenAll, який повертає об'єкт Task, що завершується, коли всі завдання у списку аргументів виконані. Наступний код демонструє цей метод:

```cs
async Task BreakfastWithWhenAllAsync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    var eggsTask = FryEggsAsync(2);
    var hashBrownTask = FryHashBrownsAsync(3);
    var toastTask = MakeToastWithButterAndJamAsync(2);

    await Task.WhenAll(eggsTask, hashBrownTask, toastTask);
    Console.WriteLine("\tEggs are ready");
    Console.WriteLine("\tHash browns are ready");
    Console.WriteLine("\tToast is ready");

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
await BreakfastWithWhenAllAsync();
```

Іншим варіантом є використання методу WhenAny, який повертає об'єкт Task\<Task\>, що завершується після завершення будь-якого з його аргументів. Ви можете чекати на повернене завдання, оскільки знаєте, що завдання виконано. У наступному коді показано, як можна використовувати метод WhenAny для очікування завершення першого завдання, а потім обробки його результату. Після обробки результату виконаного завдання, виконане завдання видаляється зі списку завдань, переданих методу WhenAny.

```cs
async Task BreakfastWithWhenAnyAsync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    var eggsTask = FryEggsAsync(2);
    var hashBrownTask = FryHashBrownsAsync(3);
    var toastTask = MakeToastWithButterAndJamAsync(2);

    var breakfastTasks = new List<Task> { eggsTask, hashBrownTask, toastTask };
    while (breakfastTasks.Count > 0)
    {
        Task finishedTask = await Task.WhenAny(breakfastTasks);
        if (finishedTask == eggsTask)
        {
            Console.WriteLine("Eggs are ready");
        }
        else if (finishedTask == hashBrownTask)
        {
            Console.WriteLine("Hash browns are ready");
        }
        else if (finishedTask == toastTask)
        {
            Console.WriteLine("Toast is ready");
        }
        await finishedTask;
        breakfastTasks.Remove(finishedTask);
    }

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
await BreakfastWithWhenAnyAsync();
```

Ближче до кінця фрагмента коду зверніть увагу на вираз await finishedTask;. Цей рядок важливий, оскільки Task.WhenAny повертає Task\<Task\> – завдання-обгортку, яке містить завершене завдання. Коли ви очікуєте Task.WhenAny, ви очікуєте завершення завдання-обгортки, а результатом є фактичне завдання, яке завершилося першим. Однак, щоб отримати результат цього завдання або переконатися, що будь-які винятки були коректно викликані, потрібно дочекатися завершення самого завдання (зберігається у finishedTask). Навіть якщо ви знаєте, що завдання завершено, повторне очікування дозволяє вам отримати доступ до його результату або обробити будь-які винятки, які могли спричинити помилку.

## Остаточний код

Ось як виглядає остаточна версія коду:

```cs
async Task MakeBreakfastAsync()
{
    Coffee cup = PourCoffee();
    Console.WriteLine("\tCoffee is ready");

    var eggsTask = FryEggsAsync(2);
    var hashBrownTask = FryHashBrownsAsync(3);
    var toastTask = MakeToastWithButterAndJamAsync(2);

    var breakfastTasks = new List<Task> { eggsTask, hashBrownTask, toastTask };
    while (breakfastTasks.Count > 0)
    {
        Task finishedTask = await Task.WhenAny(breakfastTasks);
        if (finishedTask == eggsTask)
        {
            Console.WriteLine("\tEggs are ready");
        }
        else if (finishedTask == hashBrownTask)
        {
            Console.WriteLine("\tHash browns are ready");
        }
        else if (finishedTask == toastTask)
        {
            Console.WriteLine("\tToast is ready");
        }
        await finishedTask;
        breakfastTasks.Remove(finishedTask);
    }

    Juice oj = PourOJ();
    Console.WriteLine("\tOJ is ready");
    Console.WriteLine("\t\tBreakfast is ready!");
}
await MakeBreakfastAsync();
```
```
Pouring coffee
        Coffee is ready
Warming the egg pan...
putting 3 hash brown patties in the pan
cooking first side of hash browns...
Putting a slice of bread in the toaster
Putting a slice of bread in the toaster
Start toasting...
cracking 2 eggs
cooking the eggs ...
flipping a hash brown patty
flipping a hash brown patty
flipping a hash brown patty
cooking the second side of hash browns...
Remove toast from toaster
Putting butter on the toast
Putting jam on the toast
        Toast is ready
Put hash browns on plate
Put eggs on plate
        Hash browns are ready
        Eggs are ready
Pouring orange juice
        OJ is ready
                Breakfast is ready!
```
Код виконує асинхронні завдання сніданку приблизно за 15 хвилин. Загальний час зменшується, оскільки деякі завдання виконуються одночасно. Код одночасно відстежує кілька завдань і виконує дії лише за потреби.

Остаточний код є асинхронним. Він точніше відображає, як людина може готувати сніданок. Порівняйте остаточний код із першим зразком коду у статті. Основні дії все ще зрозумілі після прочитання коду. Ви можете прочитати остаточний код так само, як і список інструкцій для приготування сніданку, як показано на початку статті. Мовні можливості для ключових слів async та await забезпечують переклад, який кожна людина робить, щоб дотримуватися письмових інструкцій: Починайте завдання, як тільки можете, і не блокуйте їх, чекаючи на їх завершення.

Порівняємо час роботи сінхронного і несінхроного методів :

```cs
long TimeOfSyncMethod()
{
    var watch = Stopwatch.StartNew();
    MakeBreakastSync();      
    watch.Stop();
    return watch.ElapsedMilliseconds;
}
async ValueTask<long> TimeOfAsyncMethod()
{
    var watch = Stopwatch.StartNew();
    await MakeBreakfastAsync();
    watch.Stop();
    return watch.ElapsedMilliseconds;
}

async Task CompareTime()
{
    double timeSyncMethod = TimeOfSyncMethod();
    double timeAsyncMethod = await TimeOfAsyncMethod();
    Console.WriteLine("\n\n");
    Console.WriteLine($"Sync  method time:{timeSyncMethod}");
    Console.WriteLine($"Async method time:{timeAsyncMethod}");
    Console.WriteLine($"\tCompare:{timeSyncMethod / timeAsyncMethod}");
}
await CompareTime();
```
```
Sync  method time: 15066
Async method time: 6035
        Compare: 2,496437448218724
```
тобто час роботи коду зменьшився майже в 2,5 рази.


## Async/await чи ContinueWith

Ключові слова async та await забезпечують синтаксичне спрощення порівняно з безпосереднім використанням Task.ContinueWith. Хоча async/await та ContinueWith мають схожу семантику для обробки асинхронних операцій, компілятор не обов'язково перетворює вирази await безпосередньо у виклики методу ContinueWith. Натомість компілятор генерує оптимізований машинний код, який забезпечує таку ж логічну поведінку. Це перетворення забезпечує значні переваги у читабельності та зручності обслуговування, особливо під час об'єднання кількох асинхронних операцій.

Розглянемо сценарій, у якому вам потрібно виконати кілька послідовних асинхронних операцій. Ось як виглядає та сама логіка, реалізована за допомогою ContinueWith у порівнянні з async/await.

### Використання ContinueWith

За допомогою ContinueWith кожен крок у послідовності асинхронних операцій вимагає вкладених продовжень:

```cs
// Using ContinueWith - demonstrates the complexity when chaining operations
static Task MakeBreakfastWithContinueWith()
{
    return StartCookingEggsAsync()
        .ContinueWith(eggsTask =>
        {
            var eggs = eggsTask.Result;
            Console.WriteLine("Eggs ready, starting bacon...");
            return StartCookingBaconAsync();
        })
        .Unwrap()
        .ContinueWith(baconTask =>
        {
            var bacon = baconTask.Result;
            Console.WriteLine("Bacon ready, starting toast...");
            return StartToastingBreadAsync();
        })
        .Unwrap()
        .ContinueWith(toastTask =>
        {
            var toast = toastTask.Result;
            Console.WriteLine("Toast ready, applying butter...");
            return ApplyButterAsync(toast);
        })
        .Unwrap()
        .ContinueWith(butteredToastTask =>
        {
            var butteredToast = butteredToastTask.Result;
            Console.WriteLine("Butter applied, applying jam...");
            return ApplyJamAsync(butteredToast);
        })
        .Unwrap()
        .ContinueWith(finalToastTask =>
        {
            var finalToast = finalToastTask.Result;
            Console.WriteLine("Breakfast completed with ContinueWith!");
        });
}
```

### Використання async/await

Та сама послідовність операцій з використанням async/await читається набагато природніше:

```cs
// Using async/await - much cleaner and easier to read
static async Task MakeBreakfastWithAsyncAwait()
{
    var eggs = await StartCookingEggsAsync();
    Console.WriteLine("Eggs ready, starting bacon...");
    
    var bacon = await StartCookingBaconAsync();
    Console.WriteLine("Bacon ready, starting toast...");
    
    var toast = await StartToastingBreadAsync();
    Console.WriteLine("Toast ready, applying butter...");
    
    var butteredToast = await ApplyButterAsync(toast);
    Console.WriteLine("Butter applied, applying jam...");
    
    var finalToast = await ApplyJamAsync(butteredToast);
    Console.WriteLine("Breakfast completed with async/await!");
}
```

### Чому перевага надається підходу async/await

Підхід async/await пропонує кілька переваг:

    1. Читабельність: Код читається як синхронний код, що полегшує розуміння потоку операцій.
    2. Зручність у підтримці: Додавання або видалення кроків у послідовності вимагає мінімальних змін коду.
    3. Обробка помилок: Обробка винятків за допомогою блоків try/catch працює природним чином, тоді як ContinueWith вимагає ретельної обробки завдань, що призвели до помилок.
    4. Налагодження: Стек викликів та досвід роботи з налагоджувачем набагато кращий за допомогою async/await.
    5. Продуктивність: Оптимізація компілятора для async/await складніша, ніж ручні ланцюжки ContinueWith.

Перевага стає ще більш очевидною зі збільшенням кількості ланцюгових операцій. Хоча з одним продовженням можна впоратися за допомогою ContinueWith, послідовності з 3-4 або більше асинхронних операцій швидко стають важкими для читання та підтримки. Цей шаблон, відомий у функціональному програмуванні як «monadic do-notation», дозволяє вам складати кілька асинхронних операцій послідовно та у зручній для читання формі.