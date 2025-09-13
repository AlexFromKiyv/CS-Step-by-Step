# Занурення в ASP.NET Core

У цьому розділі детально розглядаються функції ASP.NET Core. У міру вивчення цих функцій ви додаватимете їх до проектів, створених у попередній главі.

## Що нового в ASP.NET Core

Окрім підтримки базової функціональності ASP.NET MVC та ASP.NET Web API, Microsoft додала безліч нових функцій та покращень порівняно з попередніми фреймворками. Окрім об’єднання фреймворків та контролерів, тепер підтримується новий стиль вебзастосунків за допомогою сторінок Razor. Ось деякі додаткові покращення та нововведення:

    Веб-додатки на основі Razor Page
    Обізнаність щодо середовища
    Мінімальні шаблони з операторами верхнього рівня
    Система конфігурації, готова до хмарних технологій, на основі середовища
    Вбудоване впровадження залежностей
    Шаблон «Опції»
    Фабрика HTTP-клієнтів
    Гнучкі моделі розробки та розгортання
    Легкий, високопродуктивний та модульний конвеєр HTTP-запитів
    Помічники тегів
    Компоненти View
    Значні покращення продуктивності
    Інтегрований журнал(logging)


# Сторінки Razor (Razor Pages)

Іншим варіантом створення вебзастосунків за допомогою ASP.NET Core є використання сторінок Razor. Замість використання шаблону MVC, сторінкові додатки Razor (як випливає з назви) орієнтовані на сторінки. Кожна сторінка Razor складається з двох файлів: файлу сторінки (наприклад, Index.cshtml) – це представлення, а класу C# PageModel (наприклад, Index.cshtml.cs) – це файл коду для файлу сторінки в якому обробляються події.
Веб-застосунки на основі сторінок Razor також підтримують часткові та макетні представлення, які будуть детально розглянуті в наступних главах.

## Файл Razor Page

Як і у веб-застосунках на основі MVC, файл сторінки Razor відповідає за відображення вмісту сторінки та отримання вхідних даних від користувача. Файли сторінок Razor будуть детально розглянуті в окремій главі притсвяченій Razor Pages.

## Клас PageModel

Як і клас Controller для застосунків у стилі MVC, клас PageModel надає допоміжні методи для вебзастосунків на основі сторінок Razor. Сторінки Razor походять від класу PageModel і зазвичай іменуються із суфіксом Model, наприклад CreateModel. Суфікс моделі пропускається під час маршрутизації на сторінку. У таблиці наведено найпоширеніші методи.

Деякі допоміжні методи, що надаються класом PageModel.

|Метод|Сенс у використанні|
|-----|-------------------|
|HttpContext|Повертає HttpContext для поточної виконуваної дії.|
|Request|Повертає HttpRequest для дії, що виконується на даний момент.|
|Response|Повертає HttpResponse для дії, що виконується на даний момент.|
|RouteData|Повертає RouteData для поточної виконуваної дії (маршрутизація розглядається далі в цьому розділі).|
|ModelState|Повертає стан моделі щодо зв'язування моделі та перевірки (обидва розглянуто пізніше в цьому розділі).|
|Url|Повертає екземпляр IUrlHelper, що надає доступ до створення URL-адрес для застосунків та сервісів ASP.NET Core MVC.|
|ViewData, TempData|Надає дані до представлення через ViewDataDictionary та TempDataDictionary|
|Page|Повертає PageResult (похідний від ActionResult) як HTTP-відповідь.|
|PartialView|Повертає PartialViewResult до конвеєра відповідей.|
|ViewComponent|Повертає ViewComponentResult до конвеєра відповідей.|
|OnPageHandlerSelected|Виконується, коли вибрано метод обробника сторінки, але перед зв'язуванням моделі.|
|OnPageHandlerSelectionAsync|Асинхронна версія OnPageHandlerSelected.|
|OnPageHandlerExecuting|Виконується перед виконанням методу обробника сторінки.|
|OnPageHandlerExecutionAsync|Асинхронна версія OnPageHandlerExecuting.|
|OnPageHandlerExecuted|Виконується після виконання методу обробника сторінки.|
|User|Повертає користувача ClaimsPrincipal.|
|Content|Повертає ContentResult у відповідь. Перевантаження дозволяють додавати тип контенту та визначення кодування.|
|File|Повертає FileContentResult у відповідь.|
|Redirect|Серія методів, які перенаправляють користувача на іншу URL-адресу, повертаючи RedirectResult.|
|LocalRedirect|Серія методів, які перенаправляють користувача на іншу URL-адресу, лише якщо URL-адреса є локальною. Безпечніший за загальні методи перенаправлення.|
|RedirectToAction, RedirectToPage, RedirectToRoute|Серія методів, які перенаправляють на інший метод дії, Razor Page або іменований маршрут. Маршрутизація розглядається далі в цьому розділі.|
|TryUpdateModelAsync|Використовується для явного зв'язування моделі.|
|TryValidateModel|Використовується для явної перевірки моделі.|

В таблиці – допоміжні засоби коду стану HTTP.

Деякі допоміжні методи коду стану HTTP, що надаються класом PageModelClass

|Метод|Код стану HTTP, дія, результат|Код статуса|
|-----|------------------------------|-----------|
|NotFound|NotFound|404|
|Forbid|ForbidResult|403|
|BadRequest|BadRequestResult|400|
|StatusCode(int)StatusCode(int, object)|StatusCodeResultObjectResult|Визначається параметром int.|

Ви можете бути здивовані, побачивши деякі знайомі методи з класу Controller. Сторінкові програми Razor мають багато спільних функцій із програмами у стилі MVC, як ви вже бачили та бачитимете далі в цих главах.

## Методи обробника сторінок

Як обговорювалося в розділі маршрутизації, сторінки Razor визначають методи обробника для обробки HTTP-запитів Get та Post. Клас PageModel підтримує як синхронні, так і асинхронні методи обробника. Дієслово, що в назві обробника, залежить від назви методу, причому OnPost()/OnPostAsync() обробляє HTTP-запити post, а OnGet()/OnGetAsync() — HTTP-запити get. Версії асинхронних функцій перелічені тут:

```cs
public class DeleteModel : PageModel
{
  public async Task<IActionResult> OnGetAsync(int? id)
  {
    //handle the get request here
  }
  public async Task<IActionResult> OnPostAsync(int? id)
  {
    //handle the post request here
  }
}
```

Назви методів обробників можна змінювати, і для кожного HTTP-метода може існувати кілька методів обробників, проте перевантажені версії з однаковою назвою не допускаються. Це буде розглянуто в окремій главі.

# Обізнаність про середовище

Обізнаність програм ASP.NET Core про середовище виконання включає змінні середовища хоста та розташування файлів через екземпляр IWebHostEnvironment, який реалізує інтерфейс IHostEnvironment. У таблиці показано властивості, доступні через ці інтерфейси.

Властивості IWebHostEnvironment

|Властивість|Надається функціональність|
|-----------|--------------------------|
|ApplicationName|Видає або встановлює назву програми. За замовчуванням використовується ім'я запису.|
|ContentRootPath|Видає або встановлює абсолютний шлях до каталогу, що містить файли вмісту програми.|
|ContentRootFileProvider|Видає або встановлює IFileProvider, що вказує на ContentRootPath.|
|EnvironmentName|Видає або встановлює назву середовища. Встановлює значення змінної середовища ASPNETCORE_ENVIRONMENT.|
|WebRootPath|Видає або встановлює абсолютний шлях до каталогу, що містить файли вмісту веб-застосунку.|
|WebRootFileProvider|Отримує або встановлює IFileProvider, що вказує на WebRootPath.|

Окрім доступу до відповідних шляхів до файлів, IWebHostEnvironment використовується для визначення середовища виконання.

## Визначення середовища виконання

ASP.NET Core автоматично зчитує значення змінної середовища з назвою ASPNETCORE_ENVIRONMENT, щоб встановити середовище виконання. Якщо змінна ASPNETCORE_ENVIRONMENT не встановлена, ASP.NET Core встановлює значення Production. Набір значень доступний через властивість EnvironmentName в IWebHostEnvironment. 
Під час розробки застосунків ASP.NET Core ця змінна зазвичай встановлюється за допомогою файлу launchSettings.json або командного рядка. Нижчі середовища (staging, production, тощо) зазвичай використовують стандартні змінні середовища операційної системи.
Ви можете використовувати будь-яке ім'я для середовища або три, що надаються статичним класом Environments.

```cs
public static class Environments
{
  public static readonly string Development = "Development";
  public static readonly string Staging = "Staging";
  public static readonly string Production = "Production";
}
```
Клас HostEnvironmentEnvExtensions надає методи розширення для IHostEnvironment для роботи з властивістю EnvironmentName.

Методи HostEnvironmentEnvExtensions

|Метод|Надається функціональність|
|-----|--------------------------|
|IsProduction|Повертає значення true, якщо змінна середовища має значення Production (без урахування регістру)|
|IsStaging|Повертає значення true, якщо змінна середовища має значення Staging (без урахування регістру).|
|IsDevelopment|Повертає значення true, якщо змінна середовища має значення Development (без урахування регістру)|
|IsEnvironment|Повертає значення true, якщо змінна середовища відповідає рядку, переданому в метод (без урахування регістру)|

Ось деякі приклади використання налаштувань середовища:

1. Визначення файлів конфігурації для завантаження
2. Налаштування параметрів налагодження, помилок та ведення журналу
3. Завантаження файлів JavaScript та CSS, специфічних для середовища

Перегляньте файл Program.cs у проекті AutoLot.Mvc. Ближче до кінця файлу перевіряється середовище, щоб визначити, чи слід використовувати стандартний обробник винятків та HSTS (HTTP Strict Transport Security Protocol):

```cs
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

```

Оновіть цей блок, щоб перевернути його:

```cs
if (app.Environment.IsDevelopment())
{
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
```
Далі, під час розробки програми додайте сторінку винятків розробника до конвеєра. Це надає деталі налагодження, такі як трасування стека, детальна інформація про винятки тощо. Стандартний обробник винятків відображає просту сторінку помилки.

```cs
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
```
Оновіть блок проєкту AutoLot.Web до наступного (зверніть увагу на інший маршрут для дескриптора помилки в застосунку на основі сторінок Razor):

```cs
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
```
Проект AutoLot.Api дещо відрізняється. Він перевіряє середовище розробки, і якщо воно працює в процесі розробки, Swagger та SwaggerUI додаються до конвеєра. Для цієї програми ми перемістимо код Swagger з блоку if, щоб він завжди був доступний (залиште блок if там, оскільки він буде використаний пізніше в цьому розділі):

```cs
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();
```
Оскільки немає інтерфейсу користувача, пов'язаного з RESTful-сервісами, немає потреби в сторінці винятків розробника. 

Чи будуть сторінки Swagger доступні поза середовищем розробки, залежить від бізнесу. Ми переносимо код Swagger для роботи в усіх середовищах, щоб сторінка Swagger завжди була доступна під час роботи. Swagger буде детально розглянуто в наступній главі.


# WebAppBuilder та WebApp

На відміну від класичних застосунків ASP.NET MVC або ASP.NET Web API, застосунки ASP.NET Core – це консольні застосунки .NET, які створюють та налаштовують WebApplication, що є екземпляром IHost. Створення конфігурації IHost налаштовує програму на прослуховування та відповідь на HTTP-запити.
Шаблони за замовчуванням для ASP.NET Core MVC, Razor та сервісних застосунків є мінімальними. Ці файли будуть додаватися в міру того, як ви будете проходити розділи ASP.NET Core. До появи версій .NET 6 та C# 10 веб-хостинг створювався в методі Main() файлу Program.cs та налаштовувався для вашої програми у файлі Startup.cs. З виходом .NET 6 та C# 10 шаблон ASP.NET Core використовує оператори верхнього рівня у файлі Program.cs для створення та налаштування та не має файлу Startup.cs.

## Файл Program.cs із RESTful-сервісами

Відкрийте клас Program.cs у застосунку AutoLot.Api та перегляньте його вміст, наведений тут для довідки:
```cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
```

Якщо ви використовуєте попередню версію ASP.NET Core, попередній код було розділено між файлами Program.cs та Startup.cs. У ASP.NET Core 6 ці файли об'єднані в оператори верхнього рівня у файлі Program.cs.Код перед викликом builder.Build() містився у файлі Program.cs, а метод ConfigureServices() – у файлі Startup.cs, і відповідає за створення WebApplicationBuilder та реєстрацію служб у контейнері впровадження залежностей. Код, що включає виклик builder.Build() та решту коду у файлі, містився у методі Configure() і відповідає за налаштування HTTP-конвеєра.
Метод CreateBuilder() стискає найтиповіші налаштування програми в один виклик методу. Він налаштовує програму (використовуючи змінні середовища та JSON-файли appsettings), налаштовує постачальника журналювання за замовчуванням та встановлює контейнер для впровадження залежностей. Повернений WebApplicationBuilder використовується для реєстрації сервісів, додавання додаткової інформації про конфігурацію, підтримки ведення журналу тощо. Наступний набір методів додає необхідні базові сервіси до контейнера впровадження залежностей для побудови RESTful сервісів. Метод AddControllers() додає підтримку використання контролерів та методів дій, метод AddEndpointsApiExplorer() надає інформацію про API (і використовується Swagger), а AddSwaggerGen() створює базову підтримку OpenAPI.

    Додаючи сервіси до контейнера Dependency Injection, обов’язково додайте їх до операторів верхнього рівня за допомогою методів розширення в місті коментаря //Add services to the container . Їх необхідно додати перед викликом методу builder.Build().

Метод builder.Build() генерує WebApplication та налаштовує наступну групу викликів методів для налаштування HTTP-конвеєра. Розділ «Environment» обговорювався раніше. Наступний набір викликів гарантує, що всі запити використовують HTTPS, активує проміжне програмне забезпечення авторизації та зіставляє контролери з їхніми кінцевими точками. Нарешті, метод Run() запускає застосунок і готує все для отримання веб-запитів і відповіді на них.

## Файл Program.cs із застосунками у стилі MVC

Відкрийте клас Program.cs у застосунку AutoLot.Mvc та перегляньте вміст, показаний тут для довідки:

```cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

Перша відмінність полягає у виклику AddControllersWithViews(). Сервіси ASP.NET Core RESTful використовують той самий шаблон методу контролера/дії, що й програми в стилі MVC, тільки без представлень. Це додає підтримку представлень у програму. Виклики Swagger та API Explorer пропущені, оскільки вони використовуються сервісом API.
Наступна відмінність полягає у виклику UserExceptionHandler(), коли програма не працює в середовищі розробки. Це зручний обробник винятків, який відображає просту інформацію (і не містить технічних даних для налагодження). Далі йде метод UseHsts(), який вмикає HTTP Strict Transport Security, запобігаючи користувачам перемикатися назад на HTTP після підключення. Це також запобігає їм натискати на попередження щодо недійсних сертифікатів.
Виклик UseStaticFiles() дозволяє відображати статичний контент (зображення, файли JavaScript, файли CSS тощо) через програму. Цей виклик не застосовується в застосунках у стилі API, оскільки вони зазвичай не потребують рендерингу статичного контенту. Останні зміни додають до програми підтримку маршрутизації кінцевих точок та маршрут за замовчуванням.

## Файл Program.cs в застосунках на основі Razor Page

Відкрийте клас Program.cs у застосунку AutoLot.Web та перегляньте його вміст, наведений тут для довідки:

```cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

```

Між цим файлом та початковим файлом для програми AutoLot.Mvc є лише три відмінності. Замість додавання підтримки контролерів з представленнями, існує виклик AddRazorPages() для додавання підтримки сторінок Razor, додавання маршрутизації для сторінок Razor за допомогою виклику методу MapRazorPages(), і немає налаштованого маршруту за замовчуванням.

# Конфігурація програми

ASP.NET Core використовує систему конфігурації .NET на основі JSON, представлену в розділі "Збірки .Net та конфігуруваня проекту". Нагадуємо, що вона базується на простих JSON-файлах, які містять параметри конфігурації. Файлом конфігурації за замовчуванням є файл appsettings.json. Початкова версія файлу appsettings.json (створеного веб-застосунком ASP.NET Core та шаблонами служби API) містить лише конфігураційну інформацію для ведення журналу, а також дозволяє всім хостам (наприклад, https://localhost:xxxx) прив’язуватися до застосунку:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

Шаблон також створює файл appsettings.Development.json. Система конфігурації працює разом із середовищем виконання, щоб завантажувати додаткові файли конфігурації на основі середовища виконання. Це досягається шляхом надання системі конфігурації інструкції завантажити файл з назвою appsettings.{environmentname}.json після файлу appsettings.json. Під час роботи в режимі «Development» файл appsettings.Development.json завантажується після файлу початкових налаштувань. Якщо середовище є Staging, завантажується файл appsettings.Staging.json тощо. Важливо зазначити, що коли завантажено більше одного файлу, будь-які налаштування, що з'являються в обох файлах, перезаписуються останнім завантаженим файлом; вони не додаються.
Для кожного з проектів веб-застосунків додайте наступну інформацію про рядок підключення, оновивши фактичний рядок підключення відповідно до вашого середовища з глави "Створення Data Access Layer з EF Core", у файли appsettings.Development.json:

```cs
  "ConnectionStrings": {
    "AutoLot": "Server=(localdb)\\mssqllocaldb;Database=AutoLot;Trusted_Connection=True;ConnectRetryCount=0"
  }
```
Кожен елемент у JSON має бути розділений комами. Додаючи елемент ConnectionStrings, обов’язково додайте кому після фігурної дужки, яка починається з елемента, що додається. Далі скопіюйте кожен із файлів appsettings.Development.json до нового файлу з назвою appsettings.Production.json у кожному з проектів веб-застосунків. Оновіть записи рядка підключення до наступного:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "AutoLot": "It’s a secret"
  }
}
```
Це частково демонструє потужність нової системи конфігурації. Розробники не мають доступу до секретної інформації про виробництво (наприклад, рядків підключення), лише до несекретної інформації, проте все це все ще можна перевірити в системі контролю версій.

У виробничих сценаріях, що використовують цей шаблон, секрети зазвичай токенізуються. Процес збірки та випуску замінює токени виробничою інформацією.

Усі значення конфігурації доступні через екземпляр IConfiguration, який доступний через систему впровадження залежностей ASP.NET Core. У операторах верхнього рівня перед створенням веб-застосунку конфігурація доступна з WebApplicationBuilder ось так:

```cs
var config = builder.Configuration;
```
Після створення веб-застосунку екземпляр IConfiguration доступний з екземпляра WebApplication:

```cs
var config = app.Configuration;
```
До налаштувань можна отримати доступ за допомогою традиційних методів, розглянутих у розділі "Збірки .Net та конфігуруваня проекту". Також існує скорочений спосіб отримання рядків підключення програми.

```cs
config.GetConnectionString("AutoLot")
```
```cs
var config = builder.Configuration;
Console.WriteLine(config.GetConnectionString("AutoLot"));
```
Додаткові функції конфігурації, включаючи шаблон «Опції», будуть обговорені пізніше в цій главі.

# Вбудоване впровадження залежностей. Dependency injection (DI)

Впровадження залежностей (DI) – це механізм підтримки слабкого зв'язку між об'єктами. Замість безпосереднього створення залежних об'єктів або передачі конкретних реалізацій у класи та/або методи, параметри визначаються як інтерфейси. Таким чином, будь-яку реалізацію інтерфейсу можна передати в класи або методи та класи, що значно підвищує гнучкість застосунку.
Підтримка DI є одним з головних принципів переписаного ASP.NET Core. Оператори верхнього рівня у файлі Program.cs (розглянуто далі в цьому розділі) не лише приймають усі конфігураційні та проміжні служби через впровадження залежностей, але й ваші власні класи також можна (і потрібно) додавати до контейнера служб для впровадження в інші частини програми. Коли елемент налаштовується в контейнері ASP.NET Core DI, існують три варіанти терміну служби, як показано в таблиці.

|Пожиттева опція|Надані функції|
|---------------|--------------|
|Transient(Перехідний)|Створюються щоразу, коли вони потрібні.|
|Scoped(Охоплений)|Створюється один раз для кожного запиту. Рекомендовано для об'єктів Entity Framework DbContext.|
|Singleton(Одинарний)|Створюється один раз за першим запитом, а потім використовується повторно протягом усього терміну служби об'єкта. Це рекомендований підхід, а не реалізація класу як Singleton.|

    Якщо ви хочете використовувати інший контейнер для впровадження залежностей, ASP.NET Core було розроблено з урахуванням цієї гнучкості. Зверніться до документації, щоб дізнатися, як підключити інший контейнер.

Сервіси додаються до контейнера DI шляхом їх додавання до IServiceCollection для програми. Під час використання шаблону операторів верхнього рівня у веб-застосунках .NET 6, екземпляр IServiceCollection створюється WebApplicationBuilder, і цей екземпляр використовується для додавання служб до контейнера.
Під час додавання служб до контейнера DI обов'язково додайте їх перед рядком, який створює об'єкт WebApplication:

```cs
var app = builder.Build();
```

## Додавання необхідних складових для підтримки веб застосунку до контейнера впровадження залежностей.

Під час створення вебзастосунків на основі MVC, метод AddControllersWithView() додає необхідні сервіси для підтримки шаблону MVC в ASP.NET Core. Наведений нижче код (у файлі Program.cs проекту AutoLot.Mvc) отримує доступ до IServiceCollection WebApplicationBuilder та додає необхідну підтримку DI для контролерів та представлень:

```cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
```

Веб-застосунки RESTful-сервісів не використовують представлення, але потребують підтримки контролерів, тому вони використовують метод AddControllers(). Шаблон API для ASP.NET Core також додає підтримку Swagger (реалізації OpenAPI в .NET) та провідника кінцевих точок ASP.NET Core:

```cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
```
Зрештою, програми на основі сторінок Razor повинні ввімкнути підтримку сторінок Razor за допомогою методу AddRazorPages():

```cs
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
```

## Додавання похідних класів DbContext до контейнера DI

Реєстрація похідного класу DbContext у застосунку ASP.NET Core дозволяє контейнеру DI обробляти ініціалізацію та час життя контексту. Методи AddDbContext<>()/AddDbContextPool() додають належним чином налаштований клас контексту до контейнера DI. Версія AddDbContextPool() створює пул екземплярів, які очищуються між запитами, гарантуючи відсутність забруднення даних між запитами. Це може покращити продуктивність вашої програми, оскільки усуває початкові витрати на створення контексту після його додавання до пулу.

Почніть з додавання наступних операторів global using до GlobalUsings.cs для проектів AutoLot.Api, AutoLot.Mvc, AutoLot.Web:

```cs
global using AutoLot.Dal.EfStructures;
global using AutoLot.Dal.Initialization;
global using Microsoft.EntityFrameworkCore;
```
Наведений нижче код (який необхідно додати до кожного з файлів Program.cs у веб-проектах) отримує рядок підключення з файлу конфігурації JSON та додає ApplicationDbContext як об'єднаний ресурс до колекції сервісів:

```cs
var connectionString = builder.Configuration.GetConnectionString("AutoLot");
builder.Services.AddDbContextPool<ApplicationDbContext>(
    options => options.UseSqlServer(connectionString,
        sqlOptions => sqlOptions.EnableRetryOnFailure().CommandTimeout(60)));
```
Тепер, щоразу, коли потрібен екземпляр ApplicationDbContext, система впровадження залежностей (DI) подбає про створення (або отримання його з пулу) та переробку екземпляра (або повернення його до пулу).
EF Core запровадив набір мінімальних API для додавання похідних класів DbContext до колекції сервісів. Замість попереднього коду можна використовувати наступне скорочення:

```cs
builder.Services.AddSqlServer<ApplicationDbContext>(connectionString, options =>
{
  options.EnableRetryOnFailure().CommandTimeout(60);
});
```
Зверніть увагу, що мінімальний API не має такого ж рівня можливостей. Деякі функції, такі як пул DbContext, не підтримуються. Щоб отримати додаткові відомості про мінімальні API, зверніться до документації.

## Додавання користувацьких сервісів до контейнера впровадження залежностей

Сервіси застосунків (такі як репозиторії в проекті AutoLot.Dal) можна додати до контейнера DI, використовуючи один із параметрів терміну служби з таблиці. Наприклад, щоб додати CarRepo як сервіс до контейнера DI, ви б використали наступне:

```cs
services.AddScoped<ICarRepo, CarRepo>();
```
У попередньому прикладі додано сервіс з обмеженою областю видимості (AddScoped<>()) до контейнера DI, вказавши тип сервісу (ICarRepo) та конкретну реалізацію (CarRepo) для впровадження. Ви можете додати всі репозиторії безпосередньо до всіх вебзастосунків у файлі Program.cs або створити метод розширення для інкапсуляції викликів. Цей процес підтримує чистоту файлу Program.cs.
Перед створенням методу розширення оновіть файл GlobalUsings.cs у проекті AutoLot.Services до наступного:

```cs

global using AutoLot.Dal.Repos;
global using AutoLot.Dal.Repos.Base;
global using AutoLot.Dal.Repos.Interfaces;

global using AutoLot.Models.Entities;
global using AutoLot.Models.Entities.Base;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Hosting;
global using Microsoft.Extensions.Logging;

global using Serilog;
global using Serilog.Context;
global using Serilog.Core.Enrichers;
global using Serilog.Events;
global using Serilog.Sinks.MSSqlServer;

global using System.Data;
global using System.Runtime.CompilerServices;
```
Далі створіть нову папку з назвою DataServices у проекті AutoLot.Services. У цій папці створіть новий публічний статичний клас з назвою DataServiceConfiguration. Оновіть цей клас до наступного:

```cs

namespace AutoLot.Services.DataServices;

public static class DataServiceConfiguration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICarDriverRepo, CarDriverRepo>();
        services.AddScoped<ICarRepo, CarRepo>();
        services.AddScoped<ICreditRiskRepo, CreditRiskRepo>();
        services.AddScoped<ICustomerOrderViewModelRepo, CustomerOrderViewModelRepo>();
        services.AddScoped<ICustomerRepo, CustomerRepo>();
        services.AddScoped<IDriverRepo, DriverRepo>();
        services.AddScoped<IMakeRepo, MakeRepo>();
        services.AddScoped<IOrderRepo, OrderRepo>();
        services.AddScoped<IRadioRepo, RadioRepo>();

        return services;
    }
}
```

Далі додайте наступний код до файлу GlobalUsings.cs у кожному вебзастосунку (AutoLot.Api, AutoLot.Mvc, AutoLot.Web):

```cs
global using AutoLot.Services.DataServices;
```
Нарешті, додайте наступний код до операторів верхнього рівня в кожній із веб-застосунків, переконавшись, що ви додали їх над рядком, який створює WebApplication:

```cs
builder.Services.AddRepositories();
```


## Ієрархії залежностей

Коли існує ланцюжок залежностей, усі залежності мають бути додані до контейнера DI, інакше виникне помилка під час виконання, коли контейнер DI спробує створити екземпляр конкретного класу. Якщо ви пам'ятаєте з наших репозиторіїв, кожен з них мав публічний конструктор, який приймав екземпляр ApplicationDbContext, який додавався до контейнера DI перед додаванням до репозиторіїв. Якщо ApplicationDbContext не було в контейнері DI, то репозиторії, що залежать від нього, не могли бути побудовані.

## Впровадження залежностей

Сервіси які додані в контейнер DI можна впроваджувати в конструктори та методи класів, у представлення Razor, а також у сторінки Razor та класи PageModel. Під час впровадження в конструктор контролера або класу PageModel додайте тип, який потрібно ввести в конструктор, ось так:

```cs
//Controller
public class CarsController : Controller
{
  private readonly ICarRep _repo;
  public CarsController(ICarRepo repo)
  {
    _repo = repo;
  }
  //omitted for brevity
}
//PageModel
public class CreateModel : PageModel
{
  private readonly ICarRepo _repo;
  public CreateModel(ICarRepo repo)
  {
    _repo = repo;
  }
  //omitted for brevity
}
```
Впровадження підтримується для методів дій та методів обробників сторінок. Щоб розрізнити ціль зв'язування та сервіс з контейнера DI, необхідно використовувати атрибут FromServices:

```cs
//Controller
public class CarsController : Controller
{
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> CreateAsync([FromServices]ICarRepo repo)
  {
    //do something
  }
  //omitted for brevity
}
//PageModel
public class CreateModel : PageModel
{
  public async Task<IActionResult> OnPostAsync([FromServices]ICarRepo repo)
  {
    //do somethng
  }
  //omitted for brevity
}
```
Ви можете задаватися питанням, коли слід використовувати ін'єкцію конструктора, а коли — ін'єкцію методу, і відповідь, звичайно ж, «це залежить від ситуації». Я надаю перевагу використанню ін'єкції конструктора для сервісів, що використовуються в класі, та ін'єкції методу для більш цілеспрямованих сценаріїв.

Щоб вставити код у MVC-представлення або представлення сторінки Razor, скористайтеся командою @inject:

```cs
@inject ICarRepo Repo
```

## Отримання залежностей у Program.cs

Вам може бути цікаво, як отримати залежності з контейнера DI, коли ви знаходитесь в операторах верхнього рівня у файлі Program.cs. Наприклад, якщо ви хочете ініціалізувати базу даних, вам потрібен екземпляр ApplicationDbContext. Немає конструктора, методу дії, методу обробника сторінки або представлення, в які можна вставити екземпляр. 
Окрім традиційних методів DI, сервіси також можна отримувати безпосередньо з ServiceProvider застосунку. Веб-застосунок надає доступ до налаштованого ServiceProvider через властивість Services. Щоб отримати сервіс, спочатку створіть екземпляр IServiceScope. Це забезпечує контейнер для зберігання сервісу протягом усього терміну служби. Потім отримайте ServiceProvider з IServiceScope, який надаватиме послуги в межах поточної області видимості.
Припустимо, ви хочете вибірково очищати та перезаповнювати базу даних під час роботи в середовищі розробки. Щоб налаштувати це, спочатку додайте наступний рядок до файлів appsettings.Development.json у кожному з веб-проектів:

```json
"RebuildDataBase": true
```
Далі додайте наступний рядок до кожного з файлів appsettings.Production.json у кожному з веб-проектів:

```json
"RebuildDataBase": false
```
У блоці розробки оператора if у Program.cs, якщо налаштоване значення для RebuildDatabase є true, тоді створіть новий IServiceScope для екземпляра ApplicationDbContext та використовуйте його для виклику методу ClearAndReseedDatabase() (приклад показано з проекту AutoLot.Mvc):

```cs
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    //Initialize the database
    if (app.Configuration.GetValue<bool>("RebuildDataBase"))
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        SampleDataInitializer.ClearAndSeedData(dbContext);
    }
}
```
Внесіть точно такі ж зміни до файлу Program.cs у проекті AutoLot.Web. Оновлення файлу Program.cs у проекті AutoLot.Api показано тут:

```cs
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //Initialize the database
    if (app.Configuration.GetValue<bool>("RebuildDataBase"))
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        SampleDataInitializer.ClearAndSeedData(dbContext);
    }
}
```

## Створення спільних сервісів даних

На завершення обговорення впровадження залежностей, ми збираємося створити набір сервісів даних, які використовуватимуться як проектами AutoLot.Mvc, так і AutoLot.Web. Мета сервісів — представити єдиний набір інтерфейсів для доступу до всіх даних. Буде дві конкретні реалізації інтерфейсів: одна, яка звертатиметься до проекту AutoLot.Api, а інша — безпосередньо до коду AutoLot.Dal. Конкретні реалізації, що додаються до контейнера DI, визначатимуться налаштуванням конфігурації проекту.

## Інтерфейси

Спочатку створіть новий каталог з назвою Interfaces у каталозі DataServices проекту AutoLot.Services. Далі додайте наступний інтерфейс IDataServiceBase<T> до каталогу Interfaces:

```cs
namespace AutoLot.Services.DataServices.Interfaces;

public interface IDataServiceBase<TEntity> where TEntity : BaseEntity, new()
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity> FindAsync(int id);
    Task<TEntity> AddAsync(TEntity entity, bool persist = true);
    Task<TEntity> UpdateAsync(TEntity entity, bool persist = true);
    Task DeleteAsync(TEntity entity, bool persist = true);

    //implemented ghost method since it won’t be used by the API data service
    void ResetChangeTracker() { }
}

```
Інтерфейс IMakeDataService просто реалізує інтерфейс IDataServiceBase<Make>:

```cs
namespace AutoLot.Services.DataServices.Interfaces;

public interface IMakeDataService : IDataServiceBase<Make>
{
}
```
Інтерфейс ICarDataService реалізує інтерфейс IDataServiceBase<Car> та додає метод для отримання записів про автомобіль за ідентифікатором марки:

```cs
namespace AutoLot.Services.DataServices.Interfaces;

public interface ICarDataService : IDataServiceBase<Car>
{
    Task<IEnumerable<Car>> GetAllByMakeIdAsync(int? makeId);
}
```

## Пряма реалізація AutoLot.Dal

Почніть зі створення нового каталогу з назвою Dal у каталозі DataServices та додайте новий каталог з назвою Base до каталогу Dal. Додайте новий клас з назвою DalDataServiceBase, зробіть його публічним абстрактним та реалізуйте інтерфейс IDataServiceBase<T>:

```cs
namespace AutoLot.Services.DataServices.Dal.Base;

public abstract class DalDataServiceBase<TEntity> : IDataServiceBase<TEntity>
  where TEntity : BaseEntity, new()
{
    //implementation goes here
}
```
Додайте конструктор, який приймає екземпляр IBaseRepo<T> та призначає його змінній рівня класу:

```cs
    protected readonly IBaseRepo<TEntity> MainRepo;
    protected DalDataServiceBase(IBaseRepo<TEntity> mainRepo)
    {
        MainRepo = mainRepo;
    }
```
Згадайте, що всі методи створення, читання, оновлення та видалення (CRUD) у базовому інтерфейсі були визначені як Task або Task\<T\>. Вони визначені таким чином, оскільки виклики до RESTful сервісу є асинхронними викликами. Також пам’ятайте, що методи репозиторію, які були створені за допомогою AutoLot.Dal, не є асинхронними. Причиною цього було здебільшого навчання, а не створення додаткових труднощів у вивченні EF Core. Після завершення реалізації решти служб даних ви можете або залишити методи репозиторію синхронними (як вони будуть показані тут), або рефакторинг репозиторіїв, щоб додати асинхронні версії методів.
Базові методи викликають пов'язані методи в MainRepo:

```cs
    public async Task<IEnumerable<TEntity>> GetAllAsync() =>
        MainRepo.GetAllIgnoreQueryFilters();
    public async Task<TEntity?> FindAsync(int id) =>
        MainRepo.Find(id);
    public async Task<TEntity> AddAsync(TEntity entity, bool persist = true)
    {
        MainRepo.Add(entity, persist);
        return entity;
    }
    public async Task<TEntity> UpdateAsync(TEntity entity, bool persist = true)
    {
        MainRepo.Update(entity, persist);
        return entity;        
    }
    public async Task DeleteAsync(TEntity entity, bool persist = true) =>
        MainRepo.Delete(entity, persist);
```
Останній метод скидає ChangeTracker у контексті, очищаючи його для повторного використання:

```cs
    public void ResetChangeTracker()
    {
        MainRepo.Context.ChangeTracker.Clear();
    } 
```
Додайте такі оператори global using до файлу GlobalUsings.cs:

```cs
global using AutoLot.Services.DataServices.Dal;
global using AutoLot.Services.DataServices.Dal.Base;
```
Додайте новий клас з назвою MakeDalDataService.cs до каталогу Dal та оновіть його відповідно до наступного:

```cs
namespace AutoLot.Services.DataServices.Dal;

public class MakeDalDataService : DalDataServiceBase<Make>, IMakeDataService
{
    public MakeDalDataService(IMakeRepo mainRepo) : base(mainRepo)
    {
    }
}
```
Додайте клас з назвою CarDalDataService.cs та оновіть його відповідно до наступного, що реалізує один додатковий метод з інтерфейсу:

```cs

namespace AutoLot.Services.DataServices.Dal;

public class CarDalDataService : DalDataServiceBase<Car>,ICarDataService
{
    private readonly ICarRepo _repo;

    public CarDalDataService(ICarRepo repo) : base(repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Car>> GetAllByMakeIdAsync(int? makeId) =>
        makeId.HasValue ? _repo.GetAllBy(makeId.Value) : MainRepo.GetAllIgnoreQueryFilters();
}
```

### Як можна оновити базовий репозіторій асінхроними методами.

При реалізації методів наприклад FindAsync в класі DalDataServiceBase ми викликаєм сінхрону версію в репозіторії: 

```cs
    public async Task<TEntity> FindAsync(int id) =>
        MainRepo.Find(id);
```
Додамо асінхронк версію в репозіторій. В інтерфейсі IBaseRepo додамо:

```cs
Task<T?> FindAsync(int id);
```
В класі BaseRepo додамо реалізацію: 

```cs
    public virtual async Task<T?> FindAsync(int id) =>
        await Table.FindAsync(id);
```
Маючи такий метод в репозіторії ми можемо використатийого в класі DalDataServiceBase 

```cs
    //public async Task<TEntity> FindAsync(int id) =>
    //    MainRepo.Find(id);
    public async Task<TEntity?> FindAsync(int id) =>
       await MainRepo.FindAsync(id);
```
Таким чином ми використати асінхрону версію DBSet<T>.
Краще також в базовому інтерфейсі визначати:

```cs
    //Task<TEntity> FindAsync(int id);
    Task<TEntity?> FindAsync(int id);
```
Тому що релаізовані методи DBSet<T>, наприклад DBSet<T>.FindAsync(...) повертають ValueTask<T?>. Повертається або сутність або null.

Перевіримо зміни в AutoLot.Mvc HomeController

```cs
        public async Task<IActionResult> Index([FromServices]ICarDataService carDataService)
        {
            var car = await carDataService.FindAsync(1);
             _logger.LogAppWarning($"{car?.Id} {car?.PetName} {car?.Color}");
            return View();
        }
```

## Початкова реалізація API

Більша частина реалізації версії API служб даних буде завершена після створення фабрики HTTP-клієнтів у наступному розділі. У цьому розділі буде створено класи для ілюстрації реалізації перемикання інтерфейсів. Почніть зі створення нового каталогу з назвою Api в каталозі DataServices та додайте новий каталог з назвою Base в каталозі Api. Додайте новий клас з назвою ApiDataServiceBase, зробіть його публічним абстрактним та реалізуйте інтерфейс IDataServiceBase<T>:

```cs
namespace AutoLot.Services.DataServices.Api.Base;

public abstract class ApiDataServiceBase<TEntity> : IDataServiceBase<TEntity>
    where TEntity : BaseEntity, new()
{
    protected ApiDataServiceBase()
    {
    }
    public Task<IEnumerable<TEntity>> GetAllAsync()
    {
        throw new NotImplementedException();
    }
    public Task<TEntity?> FindAsync(int id)
    {
        throw new NotImplementedException();
    }
    public Task<TEntity> AddAsync(TEntity entity, bool persist = true)
    {
        throw new NotImplementedException();
    }
    public Task<TEntity> UpdateAsync(TEntity entity, bool persist = true)
    {
        throw new NotImplementedException();
    }
    public Task DeleteAsync(TEntity entity, bool persist = true)
    {
        throw new NotImplementedException();
    }
}

```
Додайте такі оператори global using до файлу GlobalUsings.cs:

```cs
global using AutoLot.Services.DataServices.Api;
global using AutoLot.Services.DataServices.Api.Base;
```
Додайте новий клас з назвою MakeApiDataService.cs до каталогу Api та оновіть його відповідно до наступного:

```cs
namespace AutoLot.Services.DataServices.Api;

public class MakeApiDataService : ApiDataServiceBase<Make>, IMakeDataService
{
    public MakeApiDataService():base()
    {
    }
}
```
Додайте клас з назвою CarApiDataService.cs та оновіть його, щоб він відповідав наступному, що реалізує один додатковий метод з інтерфейсу:

```cs
namespace AutoLot.Services.DataServices.Api;

public class CarApiDataService : ApiDataServiceBase<Car>, ICarDataService
{
    public CarApiDataService() : base()
    {
    }

    public Task<IEnumerable<Car>> GetAllByMakeIdAsync(int? makeId)
    {
        throw new NotImplementedException();
    }
}
```

## Додавання служб даних до контейнера DI

Останній крок – додати ICarDataService та IMakeDataService до колекції сервісів. Почніть з додавання наступного рядка до файлів appsettings.Development.json та appsettings.Production.json у проєкті AutoLot.Mvc та AutoLot.Web:

```json
"UseApi": false,
```
Додайте такі оператори global using до файлу GlobalUsings.cs у проектах AutoLot.Mvc та AutoLot.Web:

```cs
global using AutoLot.Services.DataServices.Api;
global using AutoLot.Services.DataServices.Dal;
```
Додайте новий публічний статичний метод з назвою AddDataServices() до класу DataServiceConfiguration. У цьому методі перевіряється значення прапорця конфігурації UseApi, і якщо для нього встановлено значення true, додається версія API класів служб даних до колекції служб. В іншому випадку використовуються версії рівня доступу до даних:

```cs
    public static IServiceCollection AddDataServices(this IServiceCollection services,
    ConfigurationManager config)
    {
        if (config.GetValue<bool>("UseApi"))
        {
            services.AddScoped<ICarDataService, CarApiDataService>();
            services.AddScoped<IMakeDataService, MakeApiDataService>();
        }
        else
        {
            services.AddScoped<ICarDataService, CarDalDataService>();
            services.AddScoped<IMakeDataService, MakeDalDataService>();
        }
        return services;
    }
```
Викличте новий метод розширення в операторах верхнього рівня в Program.cs у проектах AutoLot.Mvc та AutoLot.Web:

```cs
builder.Services.AddDataServices(builder.Configuration);
```

# Патерн Options в ASP.NET Core.

Патерн Options надає механізм для створення екземплярів класів з налаштованих параметрів та впровадження налаштованих класів в інші класи за допомогою впровадження залежностей. Класи впроваджуються в інший клас за допомогою однієї з версій IOptions<T>. Існує кілька версій цього інтерфейсу, як показано в таблиці.

Деякі інтерфейси IOptions

|Інтерфейс|Опис|
|---------|----|
|IOptionsMonitor<T>|Отримує параметри та підтримує наступне: сповіщення про зміни (за допомогою OnChange), перезавантаження конфігурації, іменовані параметри (за допомогою Get та CurrentValue) та анулювання вибіркових параметрів.|
|IOptionsMonitorCache<T>|Кешує екземпляри T з підтримкою повної/часткової ануляції/перезавантаження.|
|IOptionsSnaphot<T>|Перераховує параметри для кожного запиту.|
|IOptionsFactory<T>|Створює нові екземпляри T.|
|IOptions<T>|Кореневий інтерфейс. Не підтримує IOptionsMonitor<T>. Залишено для зворотної сумісності.|

## Використання патерну.

Простий приклад — отримати інформацію про автодилера з конфігурації, налаштувати клас з цими даними та вставити їх у метод дії контролера для відображення. Розміщуючи інформацію у файлі налаштувань, дані можна налаштовувати без необхідності повторного розгортання сайту.
Почніть з додавання інформації про дилера до файлів appsettings.json для проектів AutoLot.Mvc та AutoLot.Web:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "DealerInfo": {
    "DealerName": "Skimedic's Used Cars",
    "City": "West Chester",
    "State": "Ohio"
  }
}
```
Далі нам потрібно створити модель перегляду для зберігання інформації про дилера. Створіть нову папку з назвою ViewModels у проекті AutoLot.Services. У цій папці додайте новий клас з назвою DealerInfo.cs:

```cs
namespace AutoLot.Services.ViewModels;

public class DealerInfo
{
    public string DealerName { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
}
```
    Клас, який потрібно налаштувати, повинен мати публічний конструктор без параметрів і бути неабстрактним. Властивості є зв'язаними, а поля — ні. Значення за замовчуванням можна встановити для властивостей класу.

Далі додайте такі оператори до файлів AutoLot.Mvc та AutoLot.Web GlobalUsings.cs:

```cs
global using AutoLot.Services.ViewModels;
global using Microsoft.Extensions.Options;
```

Метод Configure<>() IServiceCollection зіставляє розділ конфігураційних файлів з певним типом. Цей тип потім можна впровадити в класи та представлення за допомогою шаблону options. У файлах Program.cs для файлів AutoLot.Mvc та AutoLot.Web Program.cs додайте наступний код після рядка, який використовується для налаштування сервісів даних:

```cs
builder.Services.Configure<DealerInfo>(builder.Configuration.GetSection(nameof(DealerInfo)));
```

Тепер, коли DealerInfo налаштовано, екземпляри отримуються шляхом ін'єкції одного з інтерфейсів IOptions у конструктор класу, метод дії контролера або метод обробника сторінок Razor. Зверніть увагу, що вводиться не екземпляр класу DealerInfo, а екземпляр інтерфейсу IOptions<T>. Щоб отримати налаштований екземпляр, необхідно використовувати CurrentValue (IOptionsMonitor<T>) або Value (IOptionsSnapshot<T>). У наступному прикладі використовується ін'єкція методу для передачі екземпляра if IOptionsMonitor<DealerInfo> у метод Index класу HomeController у проекті AutoLot.Mvc, потім отримує CurrentValue та передає налаштований екземпляр класу DealerInfo до представлення (хоча представлення ще нічого з ним не робить).

```cs
        public IActionResult Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
        {
            DealerInfo? vm = dealerMonitor.CurrentValue;
            _logger.LogAppWarning($"{vm.DealerName} - {vm.City} - {vm.State}");
            return View(vm);
        }
```

У наступному прикладі повторюється процес для сторінки Index Razor у папці Pages проєкту AutoLot.Web. Замість передачі екземпляра до представлення, його присвоюють властивості на сторінці. Оновіть Index.cshtml.cs, додавши таку саму ін'єкцію в метод OnGet():

```cs
        public DealerInfo DealerInfoInstance { get; set; }  

        //...

        public void OnGet([FromServices] IOptionsMonitor<DealerInfo> dealerOptions)
        {
            DealerInfoInstance = dealerOptions.CurrentValue;
            _logger.LogWarning(DealerInfoInstance.DealerName);
        }
```

    Щоб отримати додаткові відомості про шаблон в ASP.NET Core, зверніться до документації. "Options pattern in ASP.NET Core"

# Фабрика HTTP-клієнтів (The HTTP Client Factory)

ASP.NET Core має реалізацію IHTTPClientFactory, яку можна використовувати для створення та налаштування екземплярів HttpClient. Фабрика керує пулінгом та часом життя базового екземпляра HttpClientMessageHandler, абстрагуючи це від розробника. Вона забезпечує чотири механізми використання:

    1. Базове використання
    2. Іменовані клієнти
    3. Типовані клієнти
    4. Згенеровані клієнти

Після вивчення базового використання іменованих клієнтів та типізованих клієнтів, ми створимо обгортки API-сервісів, які будуть використовуватися сервісами даних, створеними раніше в цьому розділі.

    Щоб отримати інформацію про згенерованих клієнтів, зверніться до документації: "Make HTTP requests using IHttpClientFactory in ASP.NET Core"


## Базове використання

Базовий спосіб використання реєструє IHttpClientFactory у колекції сервісів, а потім використовує введений екземпляр фабрики для створення екземплярів HttPClient. Цей метод є зручним способом рефакторингу існуючої програми, яка створює екземпляри HttPClient. Щоб реалізувати це базове використання, додайте наступний рядок до операторів верхнього рівня в Program.cs (насправді цього робити не потрібно, оскільки проекти використовуватимуть типізовані клієнти):

```cs
builder.Services.AddHttpClient();
```
Потім у класі, якому потрібен HttpClient, вставте IHttpClientFactory у конструктор, а потім викличте CreateClient():

```cs
namespace AutoLot.Services.DataServices.Api.Examples;

public class BasicUsageWithIHttpClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    public BasicUsageWithIHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task DoSomethingAsync()
    {
        var client = _httpClientFactory.CreateClient();
        //do something interesting with the client
    }
}
```

## Іменовані клієнти

Іменовані клієнти корисні, коли ваша програма використовує окремі екземпляри HttpClient, особливо коли вони налаштовані по-різному. Під час реєстрації IHttpClientFactory надається ім'я разом із будь-якою конкретною конфігурацією для цього використання HttpClient. Щоб створити клієнта з назвою AutoLotApi, додайте наступний код до операторів верхнього рівня в Program.cs (не потрібно цього робити, оскільки проекти використовуватимуть типізовані клієнти):

```cs
using AutoLot.Services.DataServices.Api.Examples;

builder.Services.AddHttpClient(NamedUsageWithIHttpClientFactory.API_NAME, client =>
{
    //add any configuration here
});
```
Потім у класі, якому потрібен HttpClient, вставте IHttpClientFactory у конструктор, а потім викличте CreateClient(), передаючи ім'я клієнта, якого потрібно створити. Конфігурація з виклику AddHttpClient() використовується для створення нового екземпляра:

```cs
namespace AutoLot.Services.DataServices.Api.Examples;

public class NamedUsageWithIHttpClientFactory
{
    public const string API_NAME = 'AutoLotApi';
    private readonly IHttpClientFactory _clientFactory;
    public NamedUsageWithIHttpClientFactory(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
    }
    public async Task DoSomethingAsync()
    {
        var client = _clientFactory.CreateClient(API_NAME);
        //do something interesting with the client
    }
}
```

## Типізовані клієнти

Типізовані клієнти – це класи, які приймають екземпляр HttpClient шляхом ін'єкції в його конструктор. Оскільки типізований клієнт є класом, його можна додати до колекції сервісів та впровадити в інші класи, повністю інкапсулюючи виклики за допомогою HttpClient. Як приклад, припустимо, що у вас є клас з назвою ApiServiceWrapper, який приймає HttpClient та реалізує IApiServiceWrapper наступним чином:

```cs
namespace AutoLot.Services.DataServices.Api.Examples;

internal interface IApiServiceWrapperExample
{
    //interesting methods places here
}
```
```cs
namespace AutoLot.Services.DataServices.Api.Examples;

public class ApiServiceWrapperExample : IApiServiceWrapperExample
{
    protected readonly HttpClient Client;
    public ApiServiceWrapperExample(HttpClient client)
    {
        Client = client;
        //common client configuration goes here
    }
    //interesting methods implemented here
}

```
Після налаштування інтерфейсу та класу їх можна додати до колекції сервісів наступним чином:

```cs
builder.Services.AddHttpClient<IApiServiceWrapperExample,ApiServiceWrapperExample>();
```

Впровадьте інтерфейс IApiServiceWrapper у клас, який має здійснювати виклики до API, та використовуйте методи екземпляра класу для виклику API. Цей шаблон повністю абстрагує HttpClient від коду виклику:

```cs
namespace AutoLot.Services.DataServices.Api.Examples;

public class TypedUsageWithIHttpClientFactory
{
    private readonly IApiServiceWrapperExample _serviceWrapper;
    public TypedUsageWithIHttpClientFactory(IApiServiceWrapperExample serviceWrapper)
    {
        _serviceWrapper = serviceWrapper;
    }
    public async Task DoSomethingAsync()
    {
        //do something interesting with the service wrapper
    }
}
```
Окрім параметрів конфігурації в конструкторі класу, виклик AddHttpClient() також може налаштувати клієнта:

```cs
builder.Services.AddHttpClient<IApiServiceWrapperExample,ApiServiceWrapperExample>(client=>
{
  //configuration goes here
});
```

# Обгортка сервісу AutoLot API.

Обгортка сервісу AutoLot API використовує типізований базовий клієнт та типізовані клієнти, специфічні для сутності, для інкапсуляції всіх викликів до проекту AutoLot.Api. Як проєкт AutoLot.Mvc, так і AutoLot.Web використовуватимуть обгортку сервісу через класи сервісів даних, які були розглянуті раніше в цьому розділі та завершені в кінці розділу. Проєкт AutoLot.Api буде завершено в наступній главі.

## Оновлення конфігурації програми

Кінцеві точки застосунку AutoLot.Api відрізнятимуться залежно від середовища. Наприклад, під час розробки на вашій робочій станції базовим URI є https://localhost:5011. У вашому середовищі інтеграції URI може бути https://mytestserver.com. Обізнаність середовиша, разом з оновленою системою конфігурації, буде використана для додавання цих різних значень.
Файл appsettings.Development.json додасть інформацію про службу для локальної машини. Коли код переміщується через різні середовища, налаштування оновлюватимуться у файлі кожного середовища відповідно до базового URI та кінцевих точок для цього середовища. У цьому прикладі ви оновлюєте лише налаштування для середовища розробки. У файлах appsettings.Development.json у проектах AutoLot.Mvc та AutoLot.Web додайте наступний код:

```json
  "ApiServiceSettings": {
    "Uri": "https://localhost:5011/",
    "UserName": "AutoLotUser",
    "Password": "SecretPassword",
    "CarBaseUri": "api/v1/Cars",
    "MakeBaseUri": "api/v1/Makes",
    "MajorVersion": 1,
    "MinorVersion": 0,
    "Status": ""
  },
```
    Переконайтеся, що номер порту відповідає вашій конфігурації для AutoLot.Api.

### Створення класу ApiServiceSettings

Налаштування сервісу будуть заповнені з налаштувань так само, як і інформація про дилера. У проекті AutoLot.Service створіть нову папку з назвою ApiWrapper, а в цій папці створіть нову папку з назвою Models. У цій папці додайте клас з назвою ApiServiceSettings.cs. Назви властивостей класу повинні збігатися з назвами властивостей у розділі JSON ApiServiceSettings:

```cs
namespace AutoLot.Services.ApiWrapper.Models;

public class ApiServiceSettings
{
    public ApiServiceSettings() { }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Uri { get; set; }
    public string CarBaseUri { get; set; }
    public string MakeBaseUri { get; set; }
    public int MajorVersion { get; set; }
    public int MinorVersion { get; set; }
    public string Status { get; set; }
    public string ApiVersion => $"{MajorVersion}.{MinorVersion}" + (!string.IsNullOrEmpty(Status) ? $"-{Status}" : string.Empty);
}
```

    Версіонування API буде детально розглянуто в іншій главі.

Додайте наступний оператор using до файлу GlobalUsings.cs у проекті AutoLot.Service:

```cs
global using AutoLot.Services.ApiWrapper.Models;
```

### Реєстрація класу ApiServiceSettings

Ми знову використаємо метод розширення для реєстрації всього необхідного для обгортки сервісу API, включаючи налаштування ApiServiceSettings.cs за допомогою шаблону Options. Створіть нову папку з назвою Configuration у папці ApiWrapper, а в цій папці створіть новий публічний статичний клас з назвою ServiceConfiguration:

```cs
namespace AutoLot.Services.ApiWrapper.Configuration;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureApiServiceWrapper(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ApiServiceSettings>(config.GetSection(nameof(ApiServiceSettings)));
        return services;
    }
}
```
Додайте наступний оператор using до файлу GlobalUsings.cs для обох проектів AutoLot.Mvc та AutoLot.Web:

```cs
global using AutoLot.Services.ApiWrapper.Configuration;
```
Додайте наступний код до операторів верхнього рівня в Program.cs (як у AutoLot.Mvc, так і в AutoLot.Web) перед викликом builder.Build():

```cs
builder.Services.ConfigureApiServiceWrapper(builder.Configuration);
```
Перевіримо в HomeController.

```cs
        public async Task<IActionResult> Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor, [FromServices] IOptionsMonitor<ApiServiceSettings> apiServiceSettings)
        {
            Console.WriteLine(apiServiceSettings.CurrentValue.ToJson(Newtonsoft.Json.Formatting.Indented));
            //..
        }
```
```console
{
  "UserName": "AutoLotUser",
  "Password": "SecretPassword",
  "Uri": "https://localhost:5011/",
  "CarBaseUri": "api/v1/Cars",
  "MakeBaseUri": "api/v1/Makes",
  "MajorVersion": 1,
  "MinorVersion": 0,
  "Status": "",
  "ApiVersion": "1.0"
}
```


## Базовий клас та інтерфейс API-обгортки сервісу

Клас ApiServiceWrapperBase — це узагальнений базовий клас, який виконує операції створення, читання, оновлення та видалення (CRUD) для RESTful-сервісу AutoLot.Api. Це централізує зв'язок із сервісом, налаштування HTTP-клієнта, обробку помилок тощо. Класи, специфічні для сутностей, успадкуються від цього базового класу та будуть додані до колекції сервісів.

## Інтерфейс IApiServiceWrapperBase

Інтерфейс для сервісу-обгортки AutoLot містить поширені методи для виклику сервісу AutoLot.Api. Створіть каталог з назвою Interfaces у каталозі ApiWrapper. Додайте новий інтерфейс з назвою IApiServiceWrapperBase.cs та оновіть його до коду, показаного тут:

```cs
namespace AutoLot.Services.ApiWrapper.Interfaces;

public interface IApiServiceWrapperBase<TEntity> where TEntity : BaseEntity, new()
{
    Task<IList<TEntity>> GetAllEntitiesAsync();
    Task<TEntity?> GetEntityAsync(int id);
    Task<TEntity> AddEntityAsync(TEntity entity);
    Task<TEntity> UpdateEntityAsync(TEntity entity);
    Task DeleteEntityAsync(TEntity entity);
}
```
Додайте наступний оператор using до файлу GlobalUsings.cs для проекту AutoLot.Services:

```cs
global using AutoLot.Services.ApiWrapper.Interfaces;
```
### Інтерфейси, налаштовані для сутностей

У каталозі Interfaces створіть два нових інтерфейси з назвами ICarApiServiceWrapper.cs та IMakeApiServiceWrapper.cs.

```cs
namespace AutoLot.Services.ApiWrapper.Interfaces;

public interface ICarApiServiceWrapper : IApiServiceWrapperBase<Car>
{
    Task<IList<Car>> GetCarsByMakeAsync(int id);
}
```
```cs
namespace AutoLot.Services.ApiWrapper.Interfaces;

public interface IMakeApiServiceWrapper : IApiServiceWrapperBase<Make>
{
}
```

## Клас ApiServiceWrapperBase

Перш ніж створювати базовий клас, додайте такі оператори global using до файлу GlobalUsings.cs:

```cs
global using AutoLot.Services.ApiWrapper.Base;
global using Microsoft.Extensions.Options;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;
global using System.Text;
global using System.Text.Json;
```
Створіть нову папку з назвою Base в каталозі ApiWrapper проекту AutoLot.Services та додайте клас з назвою ApiServiceWrapperBase. Зробіть клас публічним та абстрактним і реалізуйте інтерфейс IApiServiceWrapperBase. Додайте protected конструктор, який приймає екземпляр HttpClient та IOptionsMonitor\<ApiServiceSettings\> і рядок для кінцевої точки, специфічної для сутності.

```cs
namespace AutoLot.Services.ApiWrapper.Base;

public abstract class ApiServiceWrapperBase<TEntity> : IApiServiceWrapperBase<TEntity> where TEntity : BaseEntity, new()
{
    protected readonly HttpClient Client;
    protected readonly ApiServiceSettings ApiSettings;
    private readonly string _endPoint;
    protected readonly string ApiVersion;

    protected ApiServiceWrapperBase(HttpClient client, IOptionsMonitor<ApiServiceSettings> apiSettingsMonitor, string endPoint)
    {
        Client = client;
        ApiSettings = apiSettingsMonitor.CurrentValue;
        _endPoint = endPoint;

        ApiVersion = ApiSettings.ApiVersion;
    }
}
```

### Налаштування HttpClient у конструкторі.

Конструктор додає стандартну конфігурацію до HttpClient, яка використовуватиметься всіма методами, включаючи AuthorizationHeader, який використовує базову автентифікацію. Базова автентифікація буде детально розглянута в наступній главі «Restful сервіси з ASP.NET Core», тому зараз просто зрозумійте, що базова автентифікація приймає ім'я користувача та пароль, об'єднує їх (розділені двокрапкою) та виконує Base64 кодування. До того коду встановлюєтся BaseAddress для HttpClient та вказуєтся, що клієнт очікує JSON.

```cs
    protected ApiServiceWrapperBase(HttpClient client, IOptionsMonitor<ApiServiceSettings> apiSettingsMonitor, string endPoint)
    {
        Client = client;
        ApiSettings = apiSettingsMonitor.CurrentValue;
        _endPoint = endPoint;
    
        Client.BaseAddress = new Uri(ApiSettings.Uri);
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        var authToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(
            $"{apiSettingsMonitor.CurrentValue.UserName}" +
            $":{apiSettingsMonitor.CurrentValue.Password}"));
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("basic", authToken);

        ApiVersion = ApiSettings.ApiVersion;
    }
```

### Внутрішні методи підтримки

Клас містить три допоміжні методи, які використовуються публічними методами.

### Допоміжні методи Post та Put

Ці методи обгортають пов'язані методи HttpClient.

```cs
    internal async Task<HttpResponseMessage> PostAsJsonAsync(string uri, string json)
    {
        return await Client.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
    }
    internal async Task<HttpResponseMessage> PutAsJsonAsync(string uri, string json)
    {
        return await Client.PutAsync(uri, new StringContent(json, Encoding.UTF8, "application/json"));
    }
```

### Допоміжний метод HTTP Delete

Специфікація HTTP 1.1 (і пізніші версії) дозволяє передавати тіло в операторі видалення, але для цього ще немає методу розширення HttpClient. HttpRequestMessage необхідно створювати з нуля. Першим кроком є ​​створення повідомлення запиту за допомогою ініціалізації об'єктів для встановлення Content, Method та RequestUri. Після завершення цього повідомлення надсилається, а відповідь повертається до викликаючого коду:

```cs
    internal async Task<HttpResponseMessage> DeleteAsJsonAsync(string uri, string json)
    {
        HttpRequestMessage request = new HttpRequestMessage
        {
            Content = new StringContent(json, Encoding.UTF8, "application/json"),
            Method = HttpMethod.Delete,
            RequestUri = new Uri(uri)
        };
        return await Client.SendAsync(request);
    }
```

### HTTP-виклики Get

Існує два виклики Get: один для отримання всіх записів і один для отримання одного запису. Обидва вони виконують однаковий шаблон.

```cs
    public async Task<IList<TEntity>> GetAllEntitiesAsync()
    {
        var response = await Client.GetAsync($"{ApiSettings.Uri}{_endPoint}?v={ApiVersion}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IList<TEntity>>();
        return result;
    }

    public async Task<TEntity> GetEntityAsync(int id)
    {
        var response = await Client.GetAsync($"{ApiSettings.Uri}{_endPoint}/{id}?v={ApiVersion}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TEntity>();
        return result;
    }
```
Метод GetAsync() викликається для повернення HttpResponseMessage. Успіх або невдача виклику перевіряється за допомогою методу EnsureSuccessStatusCode(), який викидає виняток, якщо виклик не повернув код успішного стану. Потім тіло відповіді серіалізується назад у тип властивості (або як сутність, або як список сутностей) та повертається до викликаючого коду. Кінцева точка створюється з налаштувань, а ApiVersion додається як значення рядка запиту.

### Виклик HTTP Post

Метод додавання запису використовує HTTP-запит Post. Він використовує допоміжний метод для відправки на додавання сутності у форматі JSON, а потім повертає запис з тіла відповіді.

```cs
    public async Task<TEntity> AddEntityAsync(TEntity entity)
    {
        var response = await PostAsJsonAsync($"{ApiSettings.Uri}{_endPoint}?v={ApiVersion}",
            JsonSerializer.Serialize(entity));
        if (response == null)
        {
            throw new Exception("Unable to communicate with the service");
        }
        var location = response.Headers?.Location?.OriginalString;

        return await response.Content.ReadFromJsonAsync<TEntity>() ?? await GetEntityAsync(entity.Id);
    }
```
Є два рядки, які варто зазначити. 
Перший — це рядок, що отримує місцезнаходження location. Зазвичай, коли виклик HTTP Post успішний, служба повертає код стану HTTP 201 (Created at). Сервіс також додасть URI щойно створеного ресурсу до заголовка Location. У попередньому коді демонструється отримання заголовка Location, але розташування не використовується в процесі кодування.
Другий рядок зауваження стосується зчитування вмісту відповіді та створення екземпляра оновленого запису. Метод-обгортка сервісу має отримати оновлений екземпляр із сервісу, щоб гарантувати, що значення, згенеровані сервером (такі як Id та TimeStamp), оновлюються в клієнтській програмі. Повернення оновленої сутності у відповіді на HTTP-виклики post не є гарантованою функцією сервісу. Якщо сервіс не повертає сутність, тоді метод використовує метод GetEntityAsync(). Також можна використовувати URI з location, якщо необхідно, але оскільки GetEntityAsync() надає все необхідне, отримання значення location призначене лише для демонстраційних цілей.

### Виклик HTTP Put

Метод оновлення запису використовує HTTP-запит Put за допомогою допоміжного методу PutAsJsonAsync(). Цей метод також припускає, що оновлена ​​сутність знаходиться в тілі відповіді, і якщо ні, викликає GetEntityAsync() для оновлення значень, згенерованих сервером.

```cs
    public async Task<TEntity> UpdateEntityAsync(TEntity entity)
    {
        var response = await PutAsJsonAsync($"{ApiSettings.Uri}{_endPoint}/{entity.Id}?v={ApiVersion}",
            JsonSerializer.Serialize(entity));
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TEntity>() ?? await GetEntityAsync(entity.Id);
    }
```

### Виклик HTTP Delete

Останній метод, який потрібно додати, призначений для виконання HTTP Delete. Шаблон відповідає шаблону решти методів: використовуйте допоміжний метод і перевірте відповідь на успішність. Немає нічого, що можна було б повернути до викликаючого коду, оскільки сутність було видалено.

```cs
    public async Task DeleteEntityAsync(TEntity entity)
    {
        var response = await DeleteAsJsonAsync($"{ApiSettings.Uri}{_endPoint}/{entity.Id}?v={ApiVersion}",
                JsonSerializer.Serialize(entity));
        response.EnsureSuccessStatusCode();
    }
```


## Класи, налаштовані для сутності

У каталозі ApiWrapper створіть два нових класи з назвами CarApiServiceWrapper.cs та MakeApiServiceWrapper.cs. Конструктор для кожного класу приймає екземпляр HttpClient та IOptions<ApiServiceSettings> і передає їх базовому класу разом із кінцевою точкою, що відповідає сутності:

```cs
namespace AutoLot.Services.ApiWrapper;

public class CarApiServiceWrapper : ApiServiceWrapperBase<Car>, ICarApiServiceWrapper
{
    public CarApiServiceWrapper(HttpClient client, IOptionsMonitor<ApiServiceSettings> apiSettingsMonitor)
    : base(client, apiSettingsMonitor, apiSettingsMonitor.CurrentValue.CarBaseUri)
    {
    }

    public Task<IList<Car>> GetCarsByMakeAsync(int id)
    {
        throw new NotImplementedException();
    }
}
```
```cs
namespace AutoLot.Services.ApiWrapper;

public class MakeApiServiceWrapper : ApiServiceWrapperBase<Make>, IMakeApiServiceWrapper
{
    public MakeApiServiceWrapper(HttpClient client, IOptionsMonitor<ApiServiceSettings> apiSettingsMonitor)
        : base(client, apiSettingsMonitor, apiSettingsMonitor.CurrentValue.MakeBaseUri)
    {
    }
}
```

Для роботи MakeApiServiceWrapper потрібні лише методи, що надаються в ApiServiceWrapperBase. Клас CarApiServiceWrapper має один додатковий метод для отримання списку записів Car за виробником. Метод дотримується того ж шаблону, що й методи HTTP Get базового класу, але використовує унікальну кінцеву точку:

```cs
    public async Task<IList<Car>> GetCarsByMakeAsync(int id)
    {
        var response = await Client.GetAsync($"{ApiSettings.Uri}{ApiSettings.CarBaseUri}/bymake/{id}?v={ApiVersion}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<IList<Car>>();
        return result;
    }
```
Як останній крок, зареєструйте два типізовані клієнти, оновивши метод ConfigureApiServiceWrapper у класі ServiceConfiguration до наступного:

```cs
public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureApiServiceWrapper(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<ApiServiceSettings>(config.GetSection(nameof(ApiServiceSettings)));
        services.AddHttpClient<ICarApiServiceWrapper, CarApiServiceWrapper>();
        services.AddHttpClient<IMakeApiServiceWrapper, MakeApiServiceWrapper>();
        return services;
    }
}
```
Таким чином маємо класи огортки в яких працює налаштований HttpClient.

# Завершення роботи над службами даних API

Тепер, коли обгортки API-сервісів завершено, настав час повернутися до API-сервісів даних та завершити реалізацію класів.

## Завершення класу ApiDataServiceBase

Першим кроком для завершення базового класу є оновлення конструктора для отримання екземпляра інтерфейсу IApiServiceWrapperBase<TEntity> та присвоєння його полю:

```cs
    protected readonly IApiServiceWrapperBase<TEntity> ServiceWrapper;
    protected ApiDataServiceBase(IApiServiceWrapperBase<TEntity> serviceWrapperBase)
    {
        ServiceWrapper = serviceWrapperBase;
    }
```

Реалізація кожного з методів CRUD викликає відповідний метод на ServiceWrapper:

```cs
    public async Task<IEnumerable<TEntity>> GetAllAsync()
        => await ServiceWrapper.GetAllEntitiesAsync();
    public async Task<TEntity?> FindAsync(int id)
        => await ServiceWrapper.GetEntityAsync(id);
    public async Task<TEntity> AddAsync(TEntity entity, bool persist = true)
        => await ServiceWrapper.AddEntityAsync(entity);
    public async Task<TEntity> UpdateAsync(TEntity entity, bool persist = true)
        => await ServiceWrapper.UpdateEntityAsync(entity);
    public async Task DeleteAsync(TEntity entity, bool persist = true)
        => await ServiceWrapper.DeleteEntityAsync(entity);
```

## Завершіть класи, специфічні для сутностей

Класи CarApiDataService та MakeApiDataService потребують оновлення конструкторів, щоб отримати специфічний для сутності похідний екземпляр інтерфейсу IApiServiceWrapperBase<TEntity> та передати його базовому класу:

```cs
    public CarApiDataService(ICarApiServiceWrapper serviceWrapper) : base(serviceWrapper)
    {
    }
```
```cs
    public MakeApiDataService(IMakeApiServiceWrapper serviceWrapper):base(serviceWrapper)
    {
    }
```
Метод GetAllByMakeIdAsync() визначає, чи було передано значення для параметра makeId. Якщо значення є, викликається відповідний метод на ICarApiServiceWrapper. В іншому випадку викликається базовий GetAllAsync():

```cs
    public async Task<IEnumerable<Car>> GetAllByMakeIdAsync(int? makeId)
    {
        return makeId.HasValue
            ? await ((ICarApiServiceWrapper)ServiceWrapper).GetCarsByMakeAsync(makeId.Value)
            : await GetAllAsync();
    }
```
Перевіримо впроваджені сервіси.

В файлі конфігурації AutoLot.Mvc змінемо

```cs
"UseApi": true,
```
Оскільки для реалізації інтерфейсу використовується 

```cs
    services.AddScoped<ICarDataService, CarApiDataService>();
```
Можна виконати в HomeControler

```cs

        public async Task<IActionResult> Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor, [FromServices] ICarDataService carDataService)
        {
            try
            {
                var car = await carDataService.FindAsync(1);
                Console.WriteLine(car?.PetName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            DealerInfo? vm = dealerMonitor.CurrentValue;
            return View(vm);
        }
```
Ми отримаєм:

```console
No connection could be made because the target machine actively refused it. (localhost:5011)
```
Тому шо Restful API ще не зроблений і не працює але запит вже відпраляється.

Якшо запустити два проекти можна помітити
```console
Response status code does not indicate success: 404 (Not Found).
```
Якшо змінити конфігурацію:

```cs
"UseApi": false,
```
Отримаємо значення з БД

```
Car entry Zippy was added from Database
Zippy
```

# Розгортання застосунків ASP.NET Core

Попередні версії ASP.NET-застосунків можна було розгортати лише на серверах Windows, використовуючи IIS як веб-сервер. ASP.NET Core можна розгорнути на кількох операційних системах різними способами, використовуючи різноманітні веб-сервери. Застосунки ASP.NET Core також можна розгортати поза веб-сервером. Варіанти високого рівня такі:

    На сервері Windows (включно з Azure) за допомогою IIS
    На сервері Windows (включно з службами програм Azure) поза IIS
    На сервері Linux за допомогою Apache або NGINX
    У Windows або Linux у контейнері

Така гнучкість дозволяє організаціям вибирати платформу розгортання, яка має для них найбільший сенс, включаючи популярні моделі розгортання на основі контейнерів (наприклад, використання Docker), на відміну від прив'язки до серверів Windows.

# Легкий та модульний конвеєр HTTP-запитів

Дотримуючись принципів .NET, ви повинні погодитися на все в ASP.NET Core. За замовчуванням у програму нічого не завантажується. Це дозволяє зробити програми максимально легкими, покращити продуктивність, мінімізувати обсях ресурсів та потенційний ризик.

# Ведення журналу (Logging)

Ведення журналу в ASP.NET Core базується на ILoggerFactory. Це дозволяє різним постачальникам логування підключатися до системи логування для надсилання повідомлень журналу в різні місця, такі як консоль. ILoggerFactory використовується для створення екземпляра ILogger<T>, який надає такі методи для ведення журналу за допомогою класу LoggerExtensions:

```cs
public static class LoggerExtensions
{
    public static void LogDebug(this ILogger logger, EventId eventId,
    Exception exception, string message, params object[] args)
    //...
    public static void LogTrace(this ILogger logger, EventId eventId,
    Exception exception, string message, params object[] args)
    //...
    public static void LogInformation(this ILogger logger, EventId eventId,
    Exception exception, string message, params object[] args)
    //..
    public static void LogWarning(this ILogger logger, EventId eventId,
    Exception exception, string message, params object[] args)
    //..
     public static void LogError(this ILogger logger, EventId eventId,
    Exception exception, string message, params object[] args)
    //..
    public static void LogCritical(this ILogger logger, EventId eventId,
    Exception exception, string message, params object[] args)
    //..
    public static void Log(this ILogger logger, LogLevel logLevel, string message, params
    object[] args)
}
```

## Додавання ведення журналу за допомогою Serilog

Будь-який постачальник, що надає розширення ILoggerFactory, може бути використаний для ведення журналу в ASP.NET Core, і Serilog є одним із таких фреймворків для ведення журналу. У наступних розділах розглядається створення інфраструктури ведення журналу на основі Serilog та налаштування програм ASP.NET Core для використання нового коду ведення журналу.

## Налаштування ведення журналу

Щоб налаштувати Serilog, ми використовуватимемо файли конфігурації програми разом із класом C#. Почніть з додавання нової папки з назвою Logging до проекту AutoLot.Service. Створіть нову папку з назвою Settings у папці Logging, а в цій новій папці додайте клас з назвою AppLoggingSettings.

```cs
namespace AutoLot.Services.Logging.Settings;

public class AppLoggingSettings
{
    public GeneralSettings General { get; set; }
    public FileSettings File { get; set; }
    public SqlServerSettings MSSqlServer { get; set; }

    public class GeneralSettings
    {
        public string RestrictedToMinimumLevel { get; set; }
    }

    public class FileSettings
    {
        public string Drive { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string FullLogPathAndFileName =>
            $"{Drive}{Path.VolumeSeparatorChar}{Path.DirectorySeparatorChar}{FilePath}{Path.DirectorySeparatorChar}{FileName}";
    }

    public class SqlServerSettings
    {
        public string TableName { get; set; }
        public string Schema { get; set; }
        public string ConnectionStringName { get; set; }
    }
}

```
Додайте наступний оператор using до файлу GlobalUsings.cs у проекті AutoLot.Service.

```cs
global using AutoLot.Services.Logging.Settings;
```
Далі, використовуйте наступний JSON, щоб замінити сформовані за замовчуванням дані журналу у файлах appsettings.Development.json для проектів AutoLot.Api, AutoLot.Mvc та AutoLot.Web:

```json
  "AppLoggingSettings": {
    "General": {
      "RestrictedToMinimumLevel": "Information"
    },
    "File": {
      "Drive": "d",
      "FilePath": "temp",
      "FileName": "log_AutoLot.txt"
    },
    "MSSqlServer": {
      "TableName": "SeriLogs",
      "Schema": "log",
      "ConnectionStringName": "AutoLot"
    }
  }
```
Далі додайте наступний вузол AppName до кожного з файлів, налаштований для кожної програми:

```json
 "AppName": "AutoLot.Api - Dev"
```
```json
 "AppName": "AutoLot.Mvc - Dev"
```
```json
 "AppName": "AutoLot.Web - Dev"
```
Додадйте додадковий рядок з початку в проект AutoLot.Web:

```json
 "DetailedErrors": true,
```
Останній крок – очистити розділ Logging кожного з файлів appsettings.json, залишивши лише запис AllowedHosts у проекті AutoLot.Api, а також AllowedHosts та DealerInfo у проектах AutoLot.Mvc та AutoLot.Web:

```json
{
  "AllowedHosts": "*",
  "DealerInfo": {
    "DealerName": "Skimedic's Used Cars",
    "City": "West Chester",
    "State": "Ohio"
  }
}
```

## Конфігурація ведення журналу

Наступний крок – налаштування Serilog. Почніть з додавання нової папки під назвою Configuration у папку Logging проекту AutoLot.Service. У цій папці додайте новий клас з назвою LoggingConfiguration. Зробіть клас публічним та статичним, як показано тут:

```cs
namespace AutoLot.Services.Logging.Configuration;

public static class LoggingConfiguration
{
}

```
Serilog використовує приймачі (sinks) для запису в різні цільові об'єкти журналювання. Завдяки цьому механізму один виклик SeriLog записуватиме дані в багато місць. Цільовими об'єктами, які ми використовуватимемо для ведення журналу в додатках ASP.NET Core, є текстовий файл, база даних і консоль. Для текстових файлів та приймачів бази даних потрібно налаштувати шаблон виводу для приймача текстових файлів та список полів для приймача бази даних. 
Щоб налаштувати шаблон файлу, створіть наступний статичний рядок лише для читання:

```cs
    internal static readonly string OutputTemplate =
        @"[{Timestamp:yy-MM-dd HH:mm:ss} {Level}]{ApplicationName}:{SourceContext}{NewLine}Message:{Message}{NewLine}in method {MemberName} at {FilePath}:{LineNumber}{NewLine}{Exception}{NewLine}"
```

Приймачу SQL Server потрібен список стовпців, ідентифікованих за допомогою типу SqlColumn. Додайте наступний код для налаштування стовпців бази даних:

```cs
    internal static readonly ColumnOptions ColumnOptions = new ColumnOptions
    {
        AdditionalColumns = new List<SqlColumn>
        {
            new SqlColumn {DataType = SqlDbType.VarChar, ColumnName = "ApplicationName"},
            new SqlColumn {DataType = SqlDbType.VarChar, ColumnName = "MachineName"},
            new SqlColumn {DataType = SqlDbType.VarChar, ColumnName = "MemberName"},
            new SqlColumn {DataType = SqlDbType.VarChar, ColumnName = "FilePath"},
            new SqlColumn {DataType = SqlDbType.Int, ColumnName = "LineNumber"},
            new SqlColumn {DataType = SqlDbType.VarChar, ColumnName = "SourceContext"},
            new SqlColumn {DataType = SqlDbType.VarChar, ColumnName = "RequestPath"},
            new SqlColumn {DataType = SqlDbType.VarChar, ColumnName = "ActionName"},
        }
    };
```

Заміна логера за замовчуванням на Serilog — це триетапний процес. Перший — очистити існуючого провайдера, другий — додати Serilog до WebApplicationBuilder, а третій — завершити налаштування Serilog. Додайте новий метод під назвою ConfigureSerilog(), який є методом розширення для WebApplicationBuilder. Перший рядок очищає логери за замовчуванням, а останній рядок додає повністю налаштований фреймворк Serilog до фреймворку логування WebApplicationBuilder.

```cs
    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        ConfigurationManager? config = builder.Configuration;
        AppLoggingSettings? settings = config.GetSection(nameof(AppLoggingSettings)).Get<AppLoggingSettings>();
        
        string? connectionStringName = settings.MSSqlServer.ConnectionStringName;
        string? connectionString = config.GetConnectionString(connectionStringName);
        string tableName = settings.MSSqlServer.TableName;
        string schema = settings.MSSqlServer.Schema;
        
        string restrictedToMinimumLevel = settings.General.RestrictedToMinimumLevel;

        if (!Enum.TryParse<LogEventLevel>(restrictedToMinimumLevel, out var logLevel))
        {
            logLevel = LogEventLevel.Debug;
        }
        
        var sqlOptions = new MSSqlServerSinkOptions
        {
            AutoCreateSqlTable = true,
            SchemaName = schema,
            TableName = tableName,
        };

        if (builder.Environment.IsDevelopment())
        {
            sqlOptions.BatchPeriod = new TimeSpan(0, 0, 0, 1);
            sqlOptions.BatchPostingLimit = 1;
        }

        var log = new LoggerConfiguration()
            .MinimumLevel.Is(logLevel)
            .Enrich.FromLogContext()
            .Enrich.With(new PropertyEnricher("ApplicationName", config.GetValue<string>("ApplicationName")))
            .Enrich.WithMachineName()
            .WriteTo.File(
                path: builder.Environment.IsDevelopment()
                    ? settings.File.FullLogPathAndFileName
                    : settings.File.FileName,
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: logLevel,
                outputTemplate: OutputTemplate)
            .WriteTo.MSSqlServer(connectionString,
                sqlOptions,
                restrictedToMinimumLevel: logLevel,
                columnOptions: ColumnOptions)
            .WriteTo.Console(restrictedToMinimumLevel: logLevel);

        builder.Logging.AddSerilog(log.CreateLogger(), false);
    }
```
Коли все готово, настав час створити фреймворк для логування, який використовуватиме Serilog.

# Система реєстрації(Logging Framework) AutoLot

Фреймворк логування AutoLot використовує вбудовані можливості логування ASP.NET Core для спрощення використання Serilog. Починається все з інтерфейсу IAppLogging.

## Інтерфейс IAppLogging

Інтерфейс IAppLogging\<T\> містить методи ведення журналу для спеціально системи ведення журналу. Додайте новий каталог з назвою Interfaces до каталогу Logging у проекті AutoLot.Service. У цьому каталозі додайте новий інтерфейс з назвою IAppLogging<T>:

```cs
namespace AutoLot.Services.Logging.Interfaces;

public interface IAppLogging<T>
{
    void LogAppError(Exception exception,
    string message,
    [CallerMemberName] string memberName = "",
    [CallerFilePath] string sourceFilePath = "",
    [CallerLineNumber] int sourceLineNumber = 0);

    void LogAppError(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void LogAppCritical(Exception exception,
        string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void LogAppCritical(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void LogAppDebug(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void LogAppTrace(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void LogAppInformation(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);

    void LogAppWarning(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0);
}
```
Атрибути CallerMemberName, CallerFilePath і CallerLineNumber перевіряють стек викликів, щоб отримати значення, для яких вони названі з коду виклику. Наприклад, якщо рядок, який викликає LogAppError(), знаходиться у функції DoWork() у файлі з іменем MyClassFile.cs та знаходиться на рядку номер 36, тоді виклик:

```cs
_appLogger.LogAppError(ex, "ERROR!");
```
перетворюється на еквівалент цього:

```cs
_appLogger.LogAppError(ex,"ERROR","DoWork","c:/myfilepath/MyClassFile.cs",36);
```
Якщо у виклик методу передаються значення для будь-якого з атрибутованих параметрів, замість значень з атрибутів використовуються передані значення.

Додайте наступний оператор using до файлу GlobalUsings.cs у проекті AutoLot.Services:

```cs
global using AutoLot.Services.Logging.Interfaces;
```

## Клас AppLogging

Клас AppLogging реалізує інтерфейс IAppLogging. Додайте новий клас з назвою AppLogging до каталогу Logging. Зробіть клас публічним, реалізуйте IAppLogging<T> та додайте конструктор, який приймає екземпляр ILogger<T> та зберігає його у змінній рівня класу.

```cs
namespace AutoLot.Services.Logging;

public class AppLogging<T> : IAppLogging<T>
{
    private readonly ILogger<T> _logger;

    public AppLogging(ILogger<T> logger)
    {
        _logger = logger;
    }
}
```
Serilog дозволяє додавати додаткові властивості до стандартного процесу логування, передаючи їх у LogContext. Додайте внутрішній метод для реєстрації події з винятком та передачі властивостей MemberName, FilePath та LineNumber. Метод PushProperty() повертає IDisposable, тому метод видаляє все перед виходом з методу.

```cs
    internal static void LogWithException(string memberName,
        string sourceFilePath, int sourceLineNumber, Exception ex, string message,
        Action<Exception, string, object[]> logAction)
    {
        var list = new List<IDisposable>
        {
            LogContext.PushProperty("MemberName", memberName),
            LogContext.PushProperty("FilePath", sourceFilePath),
            LogContext.PushProperty("LineNumber", sourceLineNumber),
        };
        logAction(ex, message, null);
        foreach (var item in list)
        {
            item.Dispose();
        }
    }
```
Повторіть процес для подій журналу, які не містять винятків:

```cs
    internal static void LogWithoutException(string memberName,
        string sourceFilePath, int sourceLineNumber, string message,
        Action<string, object[]> logAction)
    {
        var list = new List<IDisposable>
        {
            LogContext.PushProperty("MemberName", memberName),
            LogContext.PushProperty("FilePath", sourceFilePath),
            LogContext.PushProperty("LineNumber", sourceLineNumber),
        };
        logAction(message, null);
        foreach (var item in list)
        {
            item.Dispose();
        }
    }

```

Для кожного типу події логування викличте відповідний допоміжний метод для запису в журнали:

```cs

    public void LogAppError(Exception exception, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogWithException(memberName, sourceFilePath, sourceLineNumber, exception, message, _logger.LogError);
    }

    public void LogAppError(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogWithoutException(memberName, sourceFilePath, sourceLineNumber, message, _logger.LogError);
    }

    public void LogAppCritical(Exception exception, string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogWithException(memberName, sourceFilePath, sourceLineNumber, exception, message, _logger.LogCritical);
    }

    public void LogAppCritical(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogWithoutException(memberName, sourceFilePath, sourceLineNumber, message, _logger.LogCritical);
    }

    public void LogAppDebug(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogWithoutException(memberName, sourceFilePath, sourceLineNumber, message, _logger.LogDebug);
    }

    public void LogAppTrace(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogWithoutException(memberName, sourceFilePath, sourceLineNumber, message, _logger.LogTrace);
    }

    public void LogAppInformation(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogWithoutException(memberName, sourceFilePath, sourceLineNumber, message, _logger.LogInformation);
    }

    public void LogAppWarning(string message,
        [CallerMemberName] string memberName = "",
        [CallerFilePath] string sourceFilePath = "",
        [CallerLineNumber] int sourceLineNumber = 0)
    {
        LogWithoutException(memberName, sourceFilePath, sourceLineNumber, message, _logger.LogWarning);
    }

```

## Остаточна конфігурація

Остаточна конфігурація полягає в додаванні інтерфейсу IAppLogging<> до контейнера DI та виклику методу розширення для додавання SeriLog до WebApplicationBuilder. Почніть зі створення нового методу розширення в класі LoggingConfiguration:

```cs

    public static IServiceCollection RegisterLoggingInterfaces(this IServiceCollection services)
    {
        services.AddScoped(typeof(IAppLogging<>), typeof(AppLogging<>));
        return services;
    }
```
Далі додайте наступний глобальний оператор using до файлу GlobalUsings.cs у кожному веб-проєкті:

```cs
global using AutoLot.Services.Logging.Configuration;
global using AutoLot.Services.Logging.Interfaces;
```
Далі додайте обидва методи розширення до операторів верхнього рівня у файлі Program.cs. Зверніть увагу, що метод ConfigureSerilog() розширює WebAppBuilder (змінну builder), а метод RegisterLoggingInterfaces() розширює IServiceCollection:

```cs
//Configure logging
builder.ConfigureSerilog();
builder.Services.RegisterLoggingInterfaces();
```

## Додавання ведення журналу до служб даних

Після встановлення Serilog та системи ведення журналу AutoLot настав час оновити служби даних, щоб додати можливості ведення журналу.

## Оновлення базових класів

Починаючи з класів ApiDataServiceBase та DalDataServiceBase, оновіть загальне визначення, щоб воно також приймало клас, що реалізує IDataServiceBase\<TEntity\>. Це дозволяє строго типізувати інтерфейс IAppLogging\<TDataService\> для кожного з похідних класів:

```cs
public abstract class ApiDataServiceBase<TEntity,TDataSerbice> : IDataServiceBase<TEntity>
    where TEntity : BaseEntity, new()
    where TDataSerbice : IDataServiceBase<TEntity>
{
    //...
}
```
```cs
public abstract class DalDataServiceBase<TEntity, TDataService> : IDataServiceBase<TEntity>
    where TEntity : BaseEntity, new()
    where TDataService : IDataServiceBase<TEntity>
{
    //...
}
```
Далі оновіть кожен з конструкторів, щоб він приймав екземпляр IAppLogging<TDataService> та призначив його захищеному полю класу:

```cs
public abstract class ApiDataServiceBase<TEntity, TDataService> : IDataServiceBase<TEntity>
    where TEntity : BaseEntity, new()
    where TDataService : IDataServiceBase<TEntity>
{
    protected readonly IApiServiceWrapperBase<TEntity> ServiceWrapper;
    protected readonly IAppLogging<TDataService> AppLoggingInstance;
    protected ApiDataServiceBase(IAppLogging<TDataService> appLogging, IApiServiceWrapperBase<TEntity> serviceWrapperBase)
    {
        ServiceWrapper = serviceWrapperBase;
        AppLoggingInstance = appLogging;
    }
    //...
}
```
```cs
public abstract class DalDataServiceBase<TEntity, TDataService> : IDataServiceBase<TEntity>
    where TEntity : BaseEntity, new()
    where TDataService : IDataServiceBase<TEntity>
{
    protected readonly IBaseRepo<TEntity> MainRepo;
    protected readonly IAppLogging<TDataService> AppLoggingInstance;
    protected DalDataServiceBase(IAppLogging<TDataService> appLogging, IBaseRepo<TEntity> mainRepo)
    {
        MainRepo = mainRepo;
        AppLoggingInstance = appLogging;
    }
    //...
}
```
## Оновлення класів служб даних, специфічних для сутностей

Кожен із класів, специфічних для сутності, повинен змінити свою сигнатуру успадкування, щоб використовувати новий універсальний параметр, а також прийняти екземпляр IAppLogging у конструкторі та передати його базовому класу:

```cs
public class CarApiDataService : ApiDataServiceBase<Car, CarApiDataService>, ICarDataService
{
    public CarApiDataService(IAppLogging<CarApiDataService> appLogging, ICarApiServiceWrapper serviceWrapper) : base(appLogging, serviceWrapper)
    {
    }

    //...
}
```
```cs
public class MakeApiDataService : ApiDataServiceBase<Make, MakeApiDataService>, IMakeDataService
{
    public MakeApiDataService(IAppLogging<MakeApiDataService> appLogging, IMakeApiServiceWrapper serviceWrapper):base(appLogging, serviceWrapper)
    {
    }
}
```
```cs
public class CarDalDataService : DalDataServiceBase<Car, CarDalDataService>,ICarDataService
{
    private readonly ICarRepo _repo;

    public CarDalDataService(IAppLogging<CarDalDataService> appLogging, ICarRepo repo) : base(appLogging, repo)
    {
        _repo = repo;
    }

    //...
}
```
```cs
public class MakeDalDataService : DalDataServiceBase<Make, MakeDalDataService>, IMakeDataService
{
    public MakeDalDataService(IAppLogging<MakeDalDataService> appLogging, IMakeRepo mainRepo) : base(appLogging, mainRepo)
    {
    }
}
```

## Тестування фреймворку ведення журналу

На завершення розділу давайте протестуємо фреймворк логування. Першим кроком є ​​оновлення HomeController у проекті AutoLot.Mvc для використання нової системи ведення журналу. Замініть параметр ILogger<HomeController> на IAppLogging<HomeController> та оновіть тип поля ось так:

```cs
    public class HomeController : Controller
    {
        private readonly IAppLogging<HomeController> _logger;

        public HomeController(IAppLogging<HomeController> logger)
        {
            _logger = logger;
        }
        //...
    }
```
З урахуванням цього, запишіть помилку тесту в метод Index():

```cs
        public IActionResult Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
        {
            _logger.LogAppError("My test error!!!");
            DealerInfo? vm = dealerMonitor.CurrentValue;
            return View(vm);
        }
```
Запустіть проєкт AutoLot.Mvc. Після запуску програми запис буде збережено в таблиці SeriLogs, а також записано у файл з назвою log_AutoLotYYYYMMDD.txt.
Коли ви відкриєте файл журналу, ви можете здивуватися, побачивши багато додаткових записів, які не надійшли від одного виклику логера. Це тому, що EF Core та ASP.NET Core створюють дуже детальне журналювання, коли рівень журналювання встановлено на Information. Щоб усунути шум, оновіть файли appsettings.Development.json у проектах AutoLot.Api, AutoLot,Mvc та AutoLot.Web, щоб рівень журналу був Warning, ось так:
```json
      "RestrictedToMinimumLevel": "Warning"
```

Залишимо відпавку повідомлень на консоль змінивши метод ConfigureSerilog классу LoggingConfiguration.

```cs
    public static void ConfigureSerilog(this WebApplicationBuilder builder)
    {

        //...

        var log = new LoggerConfiguration()
            .MinimumLevel.Is(logLevel)
            .Enrich.FromLogContext()
            .Enrich.With(new PropertyEnricher("ApplicationName", config.GetValue<string>("ApplicationName")))
            .Enrich.WithMachineName()
            //.WriteTo.File(
            //    path: builder.Environment.IsDevelopment()
            //        ? settings.File.FullLogPathAndFileName
            //        : settings.File.FileName,
            //    rollingInterval: RollingInterval.Day,
            //    restrictedToMinimumLevel: logLevel,
            //    outputTemplate: OutputTemplate)
            //.WriteTo.MSSqlServer(connectionString,
            //    sqlOptions,
            //    restrictedToMinimumLevel: logLevel,
            //    columnOptions: ColumnOptions)
            .WriteTo.Console(restrictedToMinimumLevel: logLevel);

        builder.Logging.AddSerilog(log.CreateLogger(), false);
    }
```
Після ціх змін не буде змін в текстовому файлі і БД.


## Утиліти для роботи з рядками

Нагадаємо, що одна з домовленостей в ASP.NET Core видаляє суфікс Controller з контролерів та суфікс Async з методів дій під час маршрутизації контролерів та дій. Під час ручного створення маршрутів часто доводиться видаляти суфікс через код. Хоча код легко писати, він може бути повторюваним. Щоб зменшити повторення коду маніпулювання рядками, наступним кроком буде створення двох методів розширення рядків.
Додайте новий каталог з назвою Utilities у проекті AutoLot.Services, і в цьому каталозі створіть новий публічний статичний клас з назвою StringExtensions. У цьому класі додайте такі два методи розширення:

```cs
namespace AutoLot.Services.Utilities;

public static class StringExtensions
{
    public static string RemoveControllerSuffix(this string original)
    => original.Replace("Controller", "", StringComparison.OrdinalIgnoreCase);
    public static string RemoveAsyncSuffix(this string original)
        => original.Replace("Async", "", StringComparison.OrdinalIgnoreCase);
    public static string RemoveModelSuffix(this string original)
        => original.Replace("Model", "", StringComparison.OrdinalIgnoreCase);
}

```
Далі додайте наступний оператор using до файлу GlobalUsings.cs у проекті AutoLot.Services та всіх трьох вебзастосунках:

```cs
global using AutoLot.Services.Utilities;
```

# Підсумки

У цьому розділі заглибилися в нові функції, представлені в ASP.NET Core, і розпочався процес оновлення трьох програм ASP.NET Core. У наступному розділі ви завершите роботу над програмою AutoLot.Api.