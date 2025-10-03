# RESTful сервіси з ASP.NET Core

У попередній главі було представлено ASP.NET Core. Після ознайомлення з новими функціями та впровадження деяких комплексних питань, у цьому розділі ми завершимо роботу над RESTful сервісом AutoLot.Api.

## Знайомство з ASP.NET Core RESTful Services

З самого початку, ASP.NET Web API був розроблений як сервісний фреймворк для створення RESTful (REpresentational State Transfer) сервісів. Він базується на фреймворку MVC без V (view), з оптимізаціями для створення сервісів без заголовків. Ці служби можуть бути викликані будь-якою технологією, а не лише тими, що належать Microsoft. Виклики до служби веб-API базуються на основних HTTP методах (Get, Put, Post, Delete) через універсальний ідентифікатор ресурсу (URI), наприклад, такий:

```http
http://www.myshop.com:5001/api/cars
```

Якщо це схоже на універсальний локатор ресурсів (uniform resource locator URL), то це тому, що це так! URL — це просто URI, який вказує на фізичний ресурс у мережі. 
Виклики до веб-API використовують схему протоколу передачі гіпертексту (HTTP) на певному хості (у цьому прикладі www.myshop.com) на певному порту (5001 у попередньому прикладі), після чого йде шлях (api/cars) та необов'язковий запит і фрагмент (не показано в цьому прикладі). Виклики веб-API також можуть містити текст у тілі повідомлення, як ви побачите в цій главі. Як обговорювалося в попередньому розділі, ASP.NET Core уніфікував Web API та MVC в один фреймворк.

# Дії контролера з RESTful-сервісами

Нагадаємо, що дії повертають IActionResult (або Task<IActionResult> для асинхронних операцій). Окрім допоміжних методів у ControllerBase, які повертають певні коди стану HTTP, методи дій можуть повертати вміст у вигляді форматованих відповідей JavaScript Object Notation (JSON). Строго кажучи, методи дій можуть повертати широкий спектр форматів. JSON розглядається, оскільки він є найпоширенішим.

## Результати відповіді у форматі JSON

Більшість RESTful API отримують дані від клієнтів та надсилають їх назад за допомогою JSON (вимовляється «джей-сон»). Простий приклад JSON, що складається з двох значень:

```json
[
  "value1",
  "value2"
]
```
    В главі "Робота з файлами та серіалізація об'єктів" детально розглядається серіалізація JSON за допомогою System.Text.Json.

API також використовують коди стану HTTP для повідомлення про успіх або невдачу. Деякі з допоміжних методів стану HTTP, доступних у класі ControllerBase, були перелічені в початковій главі секції. Успішні запити повертають коди стану в діапазоні 200, де 200 (OK) є найпоширенішим кодом успіху. Насправді, це настільки поширене явище, що вам не потрібно явно повертати OK. Якщо виняток не викинуто, і код не вказує код стану, клієнту буде повернуто значення 200 разом з будь-якими даними.
Щоб налаштувати наступні приклади (та решту ціеї глави), додайте наступний код до файлу GlobalUsing.cs:

```cs
global using Microsoft.AspNetCore.Mvc;
```
Далі додайте новий контролер з назвою ValuesController.cs до каталогу Controllers проекту AutoLot.Api:

```cs
namespace AutoLot.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
 
}
```

Якщо ви використовуєте Visual Studio, для контролерів існує будувальник за шаблоном. Щоб отримати доступ до цього, клацніть правою кнопкою миші папку Controllers у проекті AutoLot.Api та виберіть Add ➤ Controller. Виберіть Common ➤ API у лівій панелі, а потім API Controller – Empty.
Цей код встановлює маршрут для контролера за допомогою літерала (api) та токена ([controller]). Цей шаблон маршруту відповідатиме URL-адресам, таким як www.myshop.com/api/values. Наступний атрибут (ApiController) підключає кілька функцій, специфічних для API, в ASP.NET Core (розглянуті в наступному розділі). Зрештою, контролер успадковується від ControllerBase. Як обговорювалося раніше, ASP.NET Core об'єднав усі різні типи контролерів, доступні в класичному ASP.NET, в один, під назвою Controller, з базовим класом ControllerBase. Клас Controller забезпечує функціональність, специфічну для перегляду (V у MVC), тоді як ControllerBase надає всю решту основної функціональності для застосунків у стилі MVC.
Існує кілька способів повернути контент у форматі JSON з методу дії. Наведені нижче приклади повертають однаковий JSON разом із кодом стану 200. Відмінності здебільшого стилістичні. Відмінності здебільшого стилістичні. Додайте наступний код до класу ValuesController:

```cs
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new string[] { "value1", "value2" });
    }

    [HttpGet("one")]
    public IEnumerable<string> Get1()
    {
        return new string[] { "value1", "value2" };
    }

    [HttpGet("two")]
    public ActionResult<IEnumerable<string>> Get2()
    {
        return new string[] { "value1", "value2" };
    }

    [HttpGet("three")]
    public string[] Get3()
    {
        return new string[] { "value1", "value2" };
    }

    [HttpGet("four")]
    public IActionResult Get4()
    {
        return new JsonResult(new string[] { "value1", "value2" });
    }
```
Щоб перевірити це, запустіть застосунок AutoLot.Api, і ви побачите всі методи з ValuesController, перелічені в інтерфейсі Swagger. Нагадаємо, що під час визначення маршрутів суфікс Controller пропускається з назви, тому кінцеві точки на ValuesController відображаються як Values, а не ValuesController. Щоб виконати один із методів, натисніть кнопку «Get», кнопку «Try it out», а потім кнопку «Execute». Ви побачите, що виконання кожного методу призводить до однакових результатів JSON.

## Налаштування обробки JSON

Функціональність надається рядком.

```cs
builder.Services.AddControllers();
```
Метод AddControllers() можна розширити для налаштування обробки JSON. За замовчуванням для ASP.NET Core використовується регістр літер у форматі верблюжого шрифту (перша літера мала, кожне наступне слово починається з великої, як «carRepo»). Це відповідає більшості фреймворків, що не належать Microsoft та використовуються для веб-розробки.
Однак, попередні версії ASP.NET використовували регістр Паскаля (перша літера велика, кожен наступний символ слова — велика, як «CarRepo»). Перехід на верблюжий регістр став критично важливим для багатьох програм, які очікували використання регістру Паскаля.
Існує дві властивості серіалізації, які можна встановити, щоб допомогти вирішити цю проблему. Перший спосіб — змусити JSON-серіалізатор використовувати регістр літер Pascal, встановивши для його PropertyNamingPolicy значення null замість JsonNamingPolicy.CamelCase. Друга зміна полягає у використанні назв властивостей без урахування регістру. Коли ця опція ввімкнена, JSON, що надходить у програму, може бути у форматі Pascal або camel case. Щоб внести ці зміни, викличте AddJsonOptions() у методі AddControllers():

```cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
```
Наступна зміна полягає в тому, щоб зробити JSON більш читабельним, записавши його з відступами:

```cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
    });
```
Перш ніж вносити остаточні зміни, додайте наступний код до файлу GlobalUsings.cs:

```cs
global using System.Text.Json.Serialization;
```
Остання зміна полягає в тому, щоб серіалізатор JSON ігнорував цикли посилань:

```cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.WriteIndented = true;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });
```

## Атрибут ApiController

Атрибут ApiController надає специфічні для REST правила, домовленості та поведінку в поєднанні з класом ControllerBase. Ці домовленості та поведінка описані в наступних розділах. Важливо зазначити, що це також працює у сценаріях базового класу. Наприклад, якщо у вас є власний базовий клас, оздоблений атрибутом ApiController, будь-які похідні контролери поводитимуться так, ніби цей атрибут застосовується безпосередньо до них.

```cs
[ApiController]
public abstract class BaseCrudController : ControllerBase
{
}
//ApiController attribute is implicitly implied on this controller as well
public class CarsController : BaseCrudController
{
}
```
Зрештою, атрибут можна застосувати на рівні збірки, що потім застосовує атрибут до кожного контролера в проекті. Щоб застосувати атрибут до кожного контролера в проекті, додайте наступний атрибут на початок файлу Program.cs:

```cs
[assembly: ApiController]
```

## Вимога маршрутизації за допомогою атрибутів

Під час використання атрибута ApiController контролер повинен використовувати маршрутизацію атрибутів. Це просто забезпечення дотримання того, що багато хто вважає найкращою практикою.

## Автоматичні відповіді 400

Якщо виникла проблема з прив’язкою моделі, дія автоматично поверне код відповіді HTTP 400 (неправильний запит). Така поведінка еквівалентна наступному коду:

```cs
if (!ModelState.IsValid)
{
  return BadRequest(ModelState);
}
```
ASP.NET Core використовує фільтр дії ModelStateInvalidFilter для виконання попередньої перевірки. Коли виникає помилка зв'язування або перевірки, відповідь HTTP 400 у тілі містить детальну інформацію про помилки, яка є серіалізованим екземпляром класу ValidationProblemDetails, що відповідає специфікації RFC 7807 (https://datatracker.ietf.org/doc/html/rfc7807). Цей клас походить від HttpProblemDetails, який, у свою чергу, походить від ProblemDetails. Повний ланцюжок ієрархії класів показано тут:

```cs
public class ProblemDetails
{
  public string? Type { get; set; }
  public string? Title { get; set; }
  public int? Status { get; set; }
  public string? Detail { get; set; }
  public string? Instance { get; set; }
  public IDictionary<string, object?> Extensions { get; } = new Dictionary<string, object?>(StringComparer.Ordinal);
}
public class HttpValidationProblemDetails : ProblemDetails
{
  public IDictionary<string, string[]> Errors { get; } = new Dictionary<string, string[]>(StringComparer.Ordinal);
}
public class ValidationProblemDetails : HttpValidationProblemDetails
{
  public new IDictionary<string, string[]> Errors => base.Errors;
}
```

Щоб побачити автоматичну обробку помилки 400 у дії, оновіть файл ValuesController.cs наступною дією HTTP Post:

```cs
[HttpPost]
public IActionResult BadBindingExample(WeatherForecast forecast)
{
  return Ok(forecast);
}
```
Запустіть програму та на сторінці Swagger клацніть POST-версію кінцевої точки /api/Values, натисніть «Try it out», але перед натисканням «Execute» очистіть автоматично згенероване тіло запиту. Це призведе до помилки зв’язування та повернення таких відомостей про помилку:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "": [
      "A non-empty request body is required."
    ],
    "forecast": [
      "The forecast field is required."
    ]
  },
  "traceId": "00-e5d51ba505c91daf2c3781f708a6de4e-38f4030cf102a179-00"
}
```
Цю поведінку можна вимкнути за допомогою налаштування.

```cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //...
    }).ConfigureApiBehaviorOptions(options =>
    {
        //suppress automatic model state binding errors
        options.SuppressModelStateInvalidFilter = true;
    });
```
Навіть якщо вимкнено, ви все ще можете надсилати інформацію про помилку як екземпляр ValidationProblemDetails. Оновіть метод дії BadBindingExample() до наступного:

```cs
    [HttpPost]
    public IActionResult BadBindingExample(WeatherForecast forecast)
    {
        return ModelState.IsValid ? Ok(forecast) : ValidationProblem(ModelState);
    }
```
Коли ви запустите програму та виконаєте код BadBindingExample(), ви побачите, що відповідь містить той самий JSON, що й автоматично оброблена помилка.

## Висновок параметра джерела зв'язування

Механізм прив'язки моделі визначить, де отримуються значення, на основі домовленостей, перелічених у таблиці.

Умовні позначення джерела

|Джерело|Зв'язані параметри|
|-------|------------------|
|FromBody|Виводиться для параметрів складних типів, за винятком вбудованих типів зі спеціальним значенням, таких як IFormCollection або CancellationToken. Може існувати лише один параметр FromBody, інакше буде викинуто виняток. Якщо зв'язування потрібне для простого типу (наприклад, string або int), то атрибут FromBody все одно є обов'язковим.|
|FromForm|Визначається для параметрів дій типів IFormFile та IFormFileCollection. Коли параметр позначено як FromForm, визначається тип вмісту multipart/form-data.|
|FromRoute|Виводиться для будь-якого імені параметра, яке відповідає імені токена маршруту.|
|FromQuery|Виведено для будь-яких інших параметрів дії.|

Цю поведінку можна вимкнути за допомогою налаштування.

```cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //...
    }).ConfigureApiBehaviorOptions(options =>
    {
        //...
        //suppress all binding inference
        options.SuppressInferBindingSourcesForParameters = true;
        options.SuppressConsumesConstraintForFormFileParameters = true;
    });
```

## Деталі проблеми для кодів стану помилки

ASP.NET Core перетворює результат помилки (статус 400 або вище) на результат того ж ProblemDetails, що й раніше. Щоб перевірити цю поведінку, додайте ще один метод до ValuesController наступним чином:

```cs
    [HttpGet("error")]
    public IActionResult Error()
    {
        return NotFound();
    }
```
Запустіть програму та скористайтеся інтерфейсом Swagger, щоб виконати нову кінцеву точку помилки.

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404,
  "traceId": "00-e23ce76008b75c4f9705068a72d13f8d-cc4c899068a8d210-00"
}
```
Цю поведінку можна вимкнути через налаштування.

```cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //...        
    }).ConfigureApiBehaviorOptions(options =>
    {
        //...
        options.SuppressMapClientErrors = true;
    });
```
Коли цю поведінку вимкнено, виклик кінцевої точки помилки повертає код 404 без будь-якої додаткової інформації.
Коли виникає помилка клієнта (і зіставлення помилок клієнта не пригнічується), для Link та тексту Title можна встановити власні значення, які є зручнішими для користувача. Наприклад, помилки 404 можуть змінити посилання ProblemDetails на https://httpstatuses.com/404 та заголовок на «Invalid Location» за допомогою наступного коду:

```cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //...        
    }).ConfigureApiBehaviorOptions(options =>
    {
        //...
        options.SuppressMapClientErrors = false;
        options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
        options.ClientErrorMapping[StatusCodes.Status404NotFound].Title = "Invalid location";

    });
```

Це оновлює повернуті значення до наступного:

```json
{
  "type": "https://httpstatuses.com/404",
  "title": "Invalid location",
  "status": 404,
  "traceId": "00-fe6dfe8a2cfc0a6b21e05f20f6b1133a-360045699711cefb-00"
}
```
## Скидання налаштувань

Після завершення тестування різних опцій оновіть код до налаштувань, що використовуються в решті цієї глави:

```cs
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //...
    }).ConfigureApiBehaviorOptions(options =>
    {
        //suppress automatic model state binding errors
        options.SuppressModelStateInvalidFilter = true;
        
        options.ClientErrorMapping[StatusCodes.Status404NotFound].Link = "https://httpstatuses.com/404";
        options.ClientErrorMapping[StatusCodes.Status404NotFound].Title = "Invalid location";
    });
```

# Версіонування API

Під час створення API важливо пам’ятати, що люди не взаємодіють з вашими кінцевими точками, а взаємодіють програми. Якщо змінюється інтерфейс користувача програми, люди зазвичай можуть зрозуміти зміни та продовжувати користуватися вашою програмою.Якщо змінюється інтерфейс користувача програми, люди зазвичай можуть зрозуміти зміни та продовжувати користуватися вашою програмою. Якщо змінюється API, клієнтська програма просто перестає працювати. Якщо ваш API є публічним, має більше одного клієнта або ви плануєте внести зміни, вам слід додати підтримку версій.

## Керівні принципи Microsoft щодо REST API

Корпорація Майкрософт опублікувала рекомендації щодо REST API (пошуковий запит Microsoft Graph REST API Guidelines), і ці рекомендації містять розділ про керування версіями. Щоб відповідати цим рекомендаціям, API повинні підтримувати явне керування версіями. 
Версії API складаються з основної та другорядної версій і повідомляються/запитуються як Major.Minor, наприклад, 1.0, 1.5 тощо. Якщо проміжна версія дорівнює нулю, її можна пропустити. Іншими словами, 1 те саме, що й 1.0. Окрім основної та проміжної версії, в кінець версії можна додати статус, щоб вказати, що версія ще не має виробничої якості, наприклад, 1.0-Beta або 1.0.Beta. Зверніть увагу, що статуси можна розділяти крапкою або тире. Коли версія має нульову проміжну версію та статус, тоді версію можна вказати як Major.Minor-Status (1.0-Beta) або Major-Status (1-Beta), використовуючи тире або крапку як роздільник статусу.
Окрім версій Major.Minor-Status, існує також груповий формат, який є додатковою функцією та підтримується через версіонізацію сегментів, що не є URL-адресою. Формат групової версії визначається як РРРР-ММ-ДД. Груповий формат не повинен замінювати версійний формат Major.Minor-Status.
Існує два варіанти вказати версію у виклику API: вбудовано в URL-адресу (в кінці кореневого каталогу сервісу) або як параметр рядка запиту в кінці URL-адреси. У наступних прикладах показано виклики API версії 1.0 для AutoLot.Api (за умови, що хостом є myshop.com):

```url
www.myshop.com/api/v1.0/cars
www.myshop.com/api/cars?api-version=1.0
```
Хоча API можуть використовувати як вбудовування URL-адрес, так і параметри рядка запиту, рекомендації вимагають, щоб API були узгодженими. Іншими словами, не допускайте використання одних кінцевих точок рядками запитів та інших вбудовуванням URL-адрес. Допустимо, щоб усі кінцеві точки підтримували обидва методи.

    Ці рекомендації безумовно варто прочитати під час створення RESTful-сервісів .NET Core.

## Додавання пакетів NuGet для керування версіями

Щоб додати повну підтримку версій ASP.NET Core, до ваших API-проектів потрібно додати два пакети NuGet. Перший – це пакет Microsoft.AspNetCore.Mvc.Versioning, який надає атрибути версії, метод AddApiVersioning() та клас ApiVersion. Другий – це пакет Microsoft.AspNetCore.Mvc.Versioning.Explorer NuGet, який робить метод AddVersionedApiExplorer() доступним. Обидва ці пакети були додані до AutoLot.Api під час створення проектів та рішення.
Додайте наступні оператори до GlobalUsings.cs

```cs

```

## Клас ApiVersion.

Клас ApiVersion є серцем підтримки версій API. Він надає контейнер для зберігання основної та другорядної версій, а також додаткової інформації про групу та стан. Він також надає методи для розбору рядків на екземпляри ApiVersion та виведення правильно відформатованого рядка версії та перевантажень для операцій рівності та порівняння. Клас наведено тут для довідки.

```cs
public class ApiVersion
{
  //Constructors allowing for all combinations of group and versions
  public ApiVersion( DateTime groupVersion )
  public ApiVersion( DateTime groupVersion, string status )
  public ApiVersion( int majorVersion, int minorVersion )
  public ApiVersion( int majorVersion, int minorVersion, String? status )
  public ApiVersion( DateTime groupVersion, int majorVersion, int minorVersion )
  public ApiVersion( DateTime groupVersion, int majorVersion, int minorVersion, String? status)
//static method to return the same as ApiVersion(1,0)
  public static ApiVersion Default { get; }
//static method to return ApiVersion(null,null,null,null)
  public static ApiVersion Neutral { get; }
//Properties for the version information
  public DateTime? GroupVersion { get; }
  public int? MajorVersion { get; }
  public int? MinorVersion { get; } //(defaults to zero if null)
  public string? Status { get; }
//checks the status for valid format (all alpha-numeric, no special characters or spaces)
  public static bool IsValidStatus( String? status )
//Parsing strings into ApiVersion instances
  public static ApiVersion Parse( string text )
  public static bool TryParse( string text, [NotNullWhen( true )] out ApiVersion? version )
//Output properly formatted version string
  public virtual string ToString( string format ) => ToString( format, InvariantCulture );
  public override string ToString() => ToString( null, InvariantCulture );
//Equality overrides to quickly compare two versions
  public override bool Equals( Object? obj ) => Equals( obj as ApiVersion );
  public static bool operator ==( ApiVersion? version1, ApiVersion? version2 ) =>
  public static bool operator !=( ApiVersion? version1, ApiVersion? version2 ) =>
  public static bool operator <( ApiVersion? version1, ApiVersion? version2 ) =>
  public static bool operator <=( ApiVersion? version1, ApiVersion? version2 ) =>
  public static bool operator >( ApiVersion? version1, ApiVersion? version2 ) =>
  public static bool operator >=( ApiVersion? version1, ApiVersion? version2 ) =>
  public virtual bool Equals( ApiVersion? other ) => other != null && GetHashCode
  public virtual int CompareTo( ApiVersion? other )
}
```

## Додавання підтримки версій API

Корінь підтримки версій додається за допомогою методу AddApiVersioning(). Це мегаметод, який додає безліч сервісів до контейнера DI. Щоб додати стандартну підтримку версій API до проєкту AutoLot.Api, викличте цей метод у IServiceCollection, ось так (наразі немає потреби додавати це до файлу Program.cs, оскільки це буде зроблено за допомогою методу розширення найближчим часом):

```cs
builder.Services.AddApiVersioning();
```

Для більш надійної підтримки версій, підтримку версій можна налаштувати за допомогою класу ApiBehaviorOptions. Перш ніж використовувати це, давайте додамо метод розширення для зберігання всього коду конфігурації версій. Почніть зі створення нової папки з назвою ApiVersionSupport у проекті. У цій папці додайте новий публічний статичний клас з назвою ApiVersionConfiguration.

```cs
namespace AutoLot.Api.ApiVersionSupport;

public static class ApiVersionConfiguration
{
}
```
Додайте новий простір імен до файлу GlobalUsings.cs:

```cs
global using AutoLot.Api.ApiVersionSupport;
```

Додайте метод розширення для IServiceCollection, який також приймає об'єкт ApiVersion, що використовуватиметься для встановлення версії за замовчуванням. Якщо версія за замовчуванням не вказана, створіть новий екземпляр об'єкта ApiVersion з версією, встановленою на 1.0:

```cs

    public static IServiceCollection AddAutoLotApiVersionConfiguration(
        this IServiceCollection services, ApiVersion defaultVersion = null)
    {
        defaultVersion ??= ApiVersion.Default;
        //remaining implementation goes here
        return services;
    }
```
Далі, після оператора для версії за замовчуванням, додайте виклик AddApiVersioning():

```cs
        defaultVersion ??= ApiVersion.Default;
        services.AddApiVersioning();
```
Цей метод приймає необов'язковий Action\<ApiVersionOptions\>, який можна використовувати для налаштування всіх параметрів. Перш ніж додавати опції, давайте розглянемо, що доступно. У таблиці перелічені доступні параметри керування версіями.

Властивості APIVersioningOptions

|Варіант|Опис|
|-------|----|
|RouteConstraintName|Маркер маршруту під час використання керування версіями URL-адрес. За замовчуванням використовується apiVersion.|
|ReportApiVersions|Вказує, чи надсилається інформація про версію системи у відповідях HTTP.Якщо значення true, HTTP-заголовки api-supported-versions та api-deprecated-versions додаються для всіх дійсних маршрутів служби. За замовчуванням значення false. |
|AssumeDefaultVersionWhenUnspecified|Якщо встановлено значення true, використовується версія API за замовчуванням, коли інформація про версію не вказана в запиті.Версія базується на результаті виклику IApiVersionSelector.SelectVersion().За замовчуванням використовується значення false.|
|DefaultApiVersion|Встановлює версію API за замовчуванням, яка використовуватиметься, коли інформація про версію не вказана в запиті, а для AssumeDefaultVersionWhenUnspecified встановлено значення true.Значення за замовчуванням — ApiVersion.Default (1.0).|
|ApiVersionReader|Отримує або встановлює ApiVersionReader для використання. Можна використовувати QueryStringApiVersionReader, HeaderApiVersionReader, MediaTypeApiVersionReader, UrlSegmentApiVersionReader. Значення за замовчуванням — QueryStringApiVersionReader.|
|ApiVersionSelector|Отримує або встановлює ApiVersionSelector. За замовчуванням використовується DefaultApiVersionSelector.|
|UseApiBehavior|Якщо значення true, політики версій API застосовуються лише до контролерів, які мають атрибут ApiController.За замовчуванням значення true.|

Тепер, коли ви розумієте доступні опції, ви можете оновити виклик AddApiVersioning(). Наведений нижче код спочатку встановлює версію за замовчуванням з параметра (або використовує версію 1.0 за замовчуванням, якщо параметр має значення null). Потім він встановлює прапорець для використання версії за замовчуванням, якщо клієнт не вказав версію. Далі він фільтрує контролери без атрибута ApiController, повідомляючи про підтримувані версії API у заголовках відповідей. Останній блок дозволяє всім методам, доступним для клієнтів, вказувати версію в запитах:

```cs
    public static IServiceCollection AddAutoLotApiVersionConfiguration(
        this IServiceCollection services, ApiVersion? defaultVersion = null)
    {
        defaultVersion ??= ApiVersion.Default;

        services.AddApiVersioning(options =>
        {
            //Set version
            options.DefaultApiVersion = defaultVersion;
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.UseApiBehavior = true;

            // reporting api versions will return the headers "api-supported-versions"
            // and "api-deprecated-versions"
            options.ReportApiVersions = true;

            //This combines all of the avalialbe option as well as 
            // allows for using "v" or "api-version" as options for
            // query string, header, or media type versioning
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new QueryStringApiVersionReader(), //defaults to "api-version"
                new QueryStringApiVersionReader("v"),
                new HeaderApiVersionReader("api-version"),
                new HeaderApiVersionReader("v"),
                new MediaTypeApiVersionReader(), //defaults to "v"
                new MediaTypeApiVersionReader("api-version")
            );
        });
        return services;
    }
```
У попередньому прикладі коду вмикаються всі доступні опції ApiVersionReader. Як бачите, кожен із параметрів сегмента, що не є URL-адресою, вказується двічі. Перший виклик для кожної пари вмикає зчитувач, який використовує api-version як ключ. Другий виклик приймає параметр у конструктор, який вказує зчитувальному засобу шукати користувацький ключ. У цих прикладах приймається простий ключ v. Під час створення реальних застосунків вам, ймовірно, не захочеться вмикати всі ці опції, вони показані тут як приклад різних конфігурацій зчитувача.

## Оновлення операторів верхнього рівня

Наступний крок – додати метод розширення до операторів верхнього рівня у файлі Program.cs. Додайте виклик методу розширення у файл одразу після AddEndpointApiExplorer() (який додає ApiDescriptionProvider для кінцевих точок):

```cs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoLotApiVersionConfiguration(new ApiVersion(1, 0));
```

## Оновлення версій API та правил іменування

Коли ввімкнено керування версіями API, контролери можуть бути названі разом із їхньою версією, і вона буде видалена так само, як і суфікс Controller. Це означає, що у вашому проєкті можуть бути ValuesController та Values2Controller, а маршрут для обох — mysite.com/api/Values. Важливо розуміти, що число в назві не має жодного відношення до фактичної версії, яку обслуговує контролер, це просто оновлення до домовленості, щоб ви могли розділяти класи контролерів за версіями та при цьому мати той самий маршрут.

## Атрибути версії API

Тепер, коли підтримка версій увімкнена, ви можете почати додавати атрибути версії до своїх контролерів та методів дій. У таблиці перелічені доступні атрибути.

Атрибути керування версіями API

|Атрибут|Опис|
|-------|----|
|ApiVersion|Атрибут рівня контролера або дії, який встановлює версію API, яку прийматиме метод контролера/дії.Може використовуватися кілька разів, щоб вказати, що приймається більше однієї версії.Версії в атрибутах ApiVersion доступні для виявлення.|
|ApiVersionNeutral|Атрибут рівня контролера, який відключає керування версіями. Запити на версії рядка запиту ігноруються. Будь-яка правильно сформована URL-адреса під час використання керування версіями URL-адрес буде прийнята, навіть якщо заповнювач представляє недійсну версію.|
|MapToApiVersion|Атрибут рівня дії, який зіставляє дію з певною версією, коли на рівні контролера вказано кілька версій. Версії в атрибутах MapToApiVersion неможливо виявити.|
|AdvertiseApiVersions|Оповіщає про додаткові версії, окрім тих, що містяться в екземплярі програми.Використовується, коли версії API розділені між розгортаннями, а інформацію про API неможливо агрегувати через Провідник версій API.|

Важливо розуміти, що після ввімкнення керування версіями воно вмикається для всіх контролерів API. Якщо для контролера не вказано версію, він використовуватиме версію за замовчуванням (1.0 у поточній конфігурації). Щоб перевірити це, запустіть програму та виконайте наступні команди CURL, усі з яких дають однаковий результат (нагадаємо, що пропуск мінорної версії те саме, що й вказівка ​​нуля):

```console
curl -G  https://localhost:5011/WeatherForecast
curl -G  https://localhost:5011/WeatherForecast?api-version=1
curl -G  https://localhost:5011/WeatherForecast?api-version=1.0
```
Тепер оновіть виклик до цього:
```console
curl -G  https://localhost:5011/WeatherForecast?api-version=2.0
```
При запиті версії 2.0, повернене значення показує, що це непідтримувана версія:
```
D:\MyWork\...\AutoLot>curl -G  https://localhost:5011/WeatherForecast?api-version=2.0
{
  "Error": {
    "Code": "UnsupportedApiVersion",
    "Message": "The HTTP resource that matches the request URI 'https://localhost:5011/WeatherForecast' does not support the API version '2.0'.",
    "InnerError": null
  }
}
```
Щоб кінцева точка прогнозу погоди була доступною незалежно від запитуваної версії, оновіть контролер атрибутом [ApiVersionNeutral]:

```cs
    [ApiVersionNeutral]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        //...
    }
```
Тепер, незалежно від запитуваної версії (чи відсутності запитуваної версії), прогноз повертається.

## Чергування версій

Контролер та/або метод дії можуть підтримувати більше однієї версії, що називається чергуванням версій. Почнемо з оновлення ValuesController для підтримки версії 1.0:

```cs
[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
  //...
}
```
```console
curl -i https://localhost:5011/api/values?api-version=1.0
curl -i https://localhost:5011/api/values?api-version=2.0
```
Якщо ви хочете, щоб ValuesController також підтримував версію 2.0, ви можете просто додати ще один атрибут ApiVersion. Наступна зміна оновлює контролер та всі його методи для підтримки версій 1.0 та 2.0:

```cs
[ApiVersion("1.0")]
[ApiVersion("2.0")]
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
  //...
}
```
Тепер, припустимо, ви хочете додати метод до цього контролера, якого не було у версії 1.0. Є два способи зробити це. Перший — додати метод до контролера та використати атрибут ApiVersion:

```cs
    [HttpGet("{id}")]
    [ApiVersion("2.0")]
    public IActionResult Get(int id)
    {
        return Ok(new[] { $"value1{id}", "value2" });
    }
```

```
curl -i https://localhost:5011/api/values/5?api-version=2.0
```
Інший варіант — використовувати атрибут MapToApiVersion, ось так:

```cs
    [HttpGet("{id}")]
    [MapToApiVersion("2.0")]
    public IActionResult Get(int id)
    {
        return Ok(new[] { $"value1{id}", "value2" });
    }
```

## Розділення контролерів

Хоча чергування версій повністю підтримується інструментарієм, це може призвести до дуже незручних контролерів, які з часом стає важко підтримувати. Більш поширений підхід полягає у використанні розділення контролерів. Залиште всі методи дій версії 1.0 у ValuesController, але створіть Values2Controller для зберігання всіх методів версії 2.0, ось так:

```cs
namespace AutoLot.Api.Controllers;

[ApiVersion("2.0")]
[Route("api/[controller]")]
[ApiController]
public class Values2Controller : ControllerBase
{
    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        return Ok(new[] { $"value{id}", "value22" });
    }
}
```


```cs
[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new string[] { "value1", "value2" });
    }

    //...
}
```
```console
curl -i https://localhost:5011/api/values
curl -i https://localhost:5011/api/values/3?api-version=2
```


Як обговорювалося в оновлених правилах іменування, базові маршрути обох контролерів визначені як /api/Values. Таке розділення не впливає на відкритий API, а лише забезпечує механізм очищення кодової бази.

## Запити Query String Version та маршрутизація

Версії API (за винятком маршрутизації сегментів URL) вводяться в дію, коли маршрут неоднозначний. Якщо маршрут обслуговується з двох кінцевих точок, процес вибору шукатиме явну версію API, яка відповідає запитуваній версії. Якщо явний збіг не знайдено, шукатиметься неявний збіг. Якщо збіг не знайдено, маршрут завершиться невдачею.

Наприклад, наступні два методи Get() обслуговуються одним і тим самим маршрутом /api/Values:

```cs

[ApiVersion("2.0")]
[Route("api/[controller]")]
[ApiController]
public class Values2Controller : ControllerBase
{
    //...

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] { "Version2:value1", "Version2:value2" });
    }

}
```
```cs
[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new string[] { "value1", "value2" });
    }
}
```
Хоча при компіляції буде помилка, і не буде працювати Swagger UI, запити будуть викликати відповідні методи. 

```console
curl -i https://localhost:5011/api/values
curl -i https://localhost:5011/api/values?api-version=1
curl -i https://localhost:5011/api/values?api-version=2
```
Однак, якщо ви оновите атрибути Values2Controller, додавши атрибут [ApiVersion("1.0")] перші два запити завершиться невдачею.

## Пріоритет атрибутів

Як зазначалося раніше, атрибут ApiVersion можна використовувати на рівні контролера або дії. Попередній приклад маршрутизації з дублікатами атрибутів [ApiVersion('1.0')] на рівні контролера завершився невдачею через неоднозначний збіг. Оновіть Values2Controller, щоб перемістити атрибут [ApiVersion('1.0')] до методу Get() ось так:

```cs
[ApiVersion("2.0")]
[Route("api/[controller]")]
[ApiController]
public class Values2Controller : ControllerBase
{

    //...

    [ApiVersion("1.0")]
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new[] { "Version2:value1", "Version2:value2" });
    }
}
```
З цією зміною виконання CURL, який запитує метод Get() версії 1.0, завершується успішно та повертає значення з Values2Controller. Це пов'язано з порядком пріоритету атрибута ApiVersion. Під час застосування на рівні контролера зазначена версія неявно застосовується до всіх методів дій у контролері. Коли атрибут застосовується на рівні дії, версія явно застосовується до методу. Як обговорювалося раніше, явні версії мають перевагу над неявними. На завершення видаліть [ApiVersion("1.0")] з методу дії Get класу Values2Controller.

## Отримання версії API у запитах

Під час використання чергування версій може бути важливим знати запитувану версію. На щастя, це просте завдання. Існує два методи отримання запитуваної версії. Перший – це виклик методу GetRequestedApiVersion() у HttpContext, а другий (введений в ASP.NET Core 3.0) – використання прив’язки моделі. Обидва показані тут в оновленому методі Get() у Values2Controller:

```cs
    [HttpGet("{id}")]
    public IActionResult Get(int id, ApiVersion versionFromModelBinding)
    {
        var versionFromContext = HttpContext.GetRequestedApiVersion();

        return Ok(new[] { versionFromContext.ToString(), versionFromModelBinding.ToString() });
    }
```
```console
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8; v=2.0
Date: Wed, 24 Sep 2025 15:31:54 GMT
Server: Kestrel
Transfer-Encoding: chunked
api-supported-versions: 1.0, 2.0

[
  "2.0",
  "2.0"
]
```
## Оновлення маршрутів для версіонування сегментів URL-адрес

У кожному з попередніх прикладів використовувалося керування версіями рядка запиту. Щоб використовувати керування версіями сегментів URL-адрес, атрибут Route потрібно оновити, щоб механізм керування версіями знав, який параметр маршруту відповідає версії. Це досягається шляхом додавання маршруту, який використовує токен маршруту {version:apiVersion}, наступним чином:

```cs
[ApiVersion("2.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class Values2Controller : ControllerBase
{
  //...
}
```
```console
curl -i https://localhost:5011/api/v2/values/
```
Зверніть увагу, що в попередньому прикладі визначено два маршрути. Це необхідно для підтримки керування версіями сегментів URL-адрес та маршрутизації сегментів, що не пов'язані з URL-адресами. Якщо застосунок підтримуватиме лише керування версіями сегментів URL-адрес, атрибут [Route("api/[controller]")] можна видалити.
Також аби методи з ValuesController були доступні з такими запитами потрібно оновити цей контролер.

```cs
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
```



## Версіонування сегментів URL-адрес та значення статусу версії

Нагадаємо, що версію можна визначити за допомогою текстового статусу, наприклад, Beta. Цей формат також підтримується керуванням версіями сегментів URL-адрес шляхом простого включення статусу до URL-адреси. Наприклад, оновіть Values2Controller, щоб додати версію 2.0-Beta:

```cs
[ApiVersion("2.0")]
[ApiVersion("2.0-Beta")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class Values2Controller : ControllerBase
```
Щоб запросити бета-версію API 2.0, звернувшись до API, можна використовувати крапку або тире як роздільник. 
```console
curl -i https://localhost:5011/api/values/1?api-version=2.0-Beta
curl -i https://localhost:5011/api/values/1?api-version=2.0.Beta
curl -i https://localhost:5011/api/v2.0-Beta/values/5
curl -i https://localhost:5011/api/v2.0.Beta/values/5
```

## Застарілі версії

Під час додавання версій гарною практикою є видалення старіших, невикористаних версій. Однак, ви ж не хочете нікого здивувати раптовим видаленням версій. Найкращий спосіб впоратися зі старішими версіями – це визнати їх застарілими. Це повідомляє клієнтів, що ця версія зникне в майбутньому, і найкраще перейти на іншу (ймовірно, новішу) версію. Щоб позначити версію як застарілу, просто додайте Deprecated = true до атрибута ApiVersion. Додавання наступного коду до ValuesController позначає версію 0.5 як застарілу:

```cs
[ApiVersion("0.5",Deprecated = true)]
[ApiVersion("1.0")]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ValuesController : ControllerBase
{
  // ...
}
```
```console
curl -i https://localhost:5011/api/values?api-version=0.5
```
Застарілі версії відображаються в заголовках, повідомляючи клієнтам, що їхня версія буде вилучена в майбутньому:

```console
HTTP/1.1 200 OK
Content-Type: application/json; charset=utf-8; v=0.5
Date: Wed, 24 Sep 2025 16:36:00 GMT
Server: Kestrel
Transfer-Encoding: chunked
api-supported-versions: 1.0, 2.0-Beta, 2.0
api-deprecated-versions: 0.5
```

## Запити щодо непідтримуваних версій

Нагадуємо, що якщо клієнт звертається до API з дійсним, однозначним маршрутом, але непідтримуваною версією, програма відповість HTTP 400 (неправильний запит) з повідомленням, яке відображається після команди CURL:

```console
curl -G -i https://localhost:5011/api/v2.0.RC/Values/1
```

## Додавання провідника версій API

Якщо ви не плануєте додавати підтримку версій до своєї документації Swagger, на цьому етапі можна вважати налаштування версійності завершеним. Однак, щоб задокументувати різні доступні версії, екземпляр IApiVersionDescriptionProvider необхідно додати до контейнера DI за допомогою методу розширення AddVersionedApiExplorer(). Цей метод приймає Action\<ApiExlorerOptions\>, який використовується для встановлення параметрів для провідника.

Деякі властивості APIExplorerOptions

|Опція|Опис|
|-----|----|
|GroupNameFormat|Отримує або встановлює формат, який використовується для створення назв груп з версій API. Значення за замовчуванням – null.|
|SubstitutionFormat|Отримує або встановлює формат, який використовується для форматування версії API, що підставляється в шаблони маршрутів. Значення за замовчуванням — «VVV», яке форматує основну версію та необов’язкову проміжну версію.|
|SubstitueApiVersionInUrl|Отримує або встановлює значення, яке вказує, чи слід підставляти параметр версії API у шаблони маршрутів. За замовчуванням має значення false.|
|DefaultApiVersionParameterDescription|Отримує або встановлює опис за замовчуванням, який використовується для параметрів версії API.За замовчуванням використовується значення «The requested API version».|
|AddApiVersionParametersWhenVersionNeutral|тримує або встановлює значення, яке вказує, чи додаються параметри версії API, коли API є нейтральним до версії. За замовчуванням має значення false.|
|DefaultApiVersion|Gets or sets the default version when request does not specify version information. Defaults to ApiVersion.Default (1.0).|
|AssumeDefaultVersionWhenUnspecified|Отримує або встановлює значення, яке вказує, чи передбачається версія за замовчуванням, коли клієнт не надає версію API служби. Значення за замовчуванням походить від властивості з такою ж назвою в ApiVersioningOptions.|
|ApiVersionParameterSource|Отримує або встановлює джерело для визначення параметрів версії API.|

Додайте виклик методу AddVersionedApiExplorer() до методу AddAutoLotApiVersionConfiguration() у класі ApiVersionConfiguration. Наведений нижче код встановлює версію за замовчуванням, використовує версію за замовчуванням, якщо клієнт її не надає, встановлює формат звітної версії на «v'Major.Minor-Status» та вмикає підстановку версій в URL-адресах:

```cs
    public static IServiceCollection AddAutoLotApiVersionConfiguration(
        this IServiceCollection services, ApiVersion? defaultVersion = null)
    {
        defaultVersion ??= ApiVersion.Default;

        //...

        services.AddVersionedApiExplorer( options =>
        {
            options.DefaultApiVersion = defaultVersion;
            options.AssumeDefaultVersionWhenUnspecified = true;
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            options.GroupNameFormat = "'v'VVV";
            // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            // can also be used to control the format of the API version in route templates
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
```

# Оновлення налаштувань Swagger/OpenAPI

Swagger (також відомий як OpenAPI) – це відкритий стандарт для документування RESTful API. Дві основні бібліотеки з відкритим кодом для додавання Swagger до API ASP.NET Core – це Swashbuckle та NSwag. Swashbuckle генерує документ swagger.json для вашої програми, який містить інформацію про сайт, кожну кінцеву точку та будь-які об'єкти, що беруть участь у кінцевих точках.
Swashbuckle також надає інтерактивний інтерфейс користувача під назвою Swagger UI, який відображає вміст файлу swagger.json, як ви вже використовували в попередніх прикладах. Цей досвід можна покращити, додавши додаткову документацію до згенерованого файлу swagger.json.
Щоб розпочати, додайте такі оператори до файлу GlobalUsings.cs:

```cs
global using Microsoft.Extensions.Options;
global using Microsoft.OpenApi.Any;
global using Microsoft.OpenApi.Models;
global using Swashbuckle.AspNetCore.Annotations;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using System.Reflection;
global using System.Text.Json;
```

## Додайте файл XML-документації.

.NET може згенерувати файл XML-документації з вашого проекту, перевіривши сигнатури методів, а також документацію розробників для методів, що містяться в коментарях з потрійною скісну рискою (///). Ви повинні погодитися на створення цього файлу.
Щоб увімкнути створення файлу XML-документації за допомогою Visual Studio, клацніть правою кнопкою миші проект AutoLot.Api та відкрийте вікно «Properties». Виберіть «Build/Output» на лівій панелі, встановіть прапорець «Файл XML-документації» та введіть AutoLot.Api.xml як ім’я файлу. Також додайте 1591 у текстове поле «Suppress warnings». Цей параметр вимикає попередження компілятора для методів, які не мають XML-коментарів із потрійною косою рискою. Зробивши це подивіться зміни файлу проекта.

```xml
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
    <GenerateDocumentationFile>True</GenerateDocumentationFile>
    <DocumentationFile>AutoLot.Api.xml</DocumentationFile>
  </PropertyGroup>

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
    <NoWarn>1701;1702;1591</NoWarn>
  </PropertyGroup>

  <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
    <NoWarn>1701;1702;1591</NoWarn>
  </PropertyGroup>
```

Після збірки проєкту згенерований файл буде створено в кореневому каталозі проєкту. Нарешті, налаштуйте згенерований XML-файл так, щоб він завжди копіювався до вихідного каталогу.

```xml
	<ItemGroup>
		<None Update="AutoLot.Api.xml">
			<CopyToOutputDirectory>Always</CopyToOutputDirectory>
		</None>
	</ItemGroup>
```
Щоб додати власні коментарі, які будуть додані до файлу документації, додайте коментарі з потрійною скісну рискою (///) до методу Get() ValuesController до цього:

```cs
    /// <summary>
    /// This is an example Get method returning JSON
    /// </summary>
    /// <remarks>This is one of several examples for returning JSON:
    /// <pre>
    /// [
    ///   "value1",
    ///   "value2"
    /// ]
    /// </pre>
    /// </remarks>
    /// <returns>List of strings</returns>
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new string[] { "value1", "value2" });
    }

```
Під час збірки проєкту в кореневому каталозі проєкту створюється новий файл з назвою AutoLot.Api.xml. Відкрийте файл, щоб переглянути щойно додані коментарі.

```xml
<?xml version="1.0"?>
<doc>
    <assembly>
        <name>AutoLot.Api</name>
    </assembly>
    <members>
        <member name="M:AutoLot.Api.Controllers.ValuesController.Get">
            <summary>
            This is an example Get method returning JSON
            </summary>
            <remarks>This is one of several examples for returning JSON:
            <pre>
            [
              "value1",
              "value2"
            ]
            </pre>
            </remarks>
            <returns>List of strings</returns>
        </member>
    </members>
</doc>
```
Якщо під час використання Visual Studio ввести три зворотні скісну риску перед визначенням класу або методу, Visual Studio автоматично створить щаблон заповнення для XML-коментаря.
XML-коментарі будуть незабаром об'єднані зі згенерованим файлом swagger.json.

## Налаштування Swagger у застосунку

Для сторінки Swagger є кілька налаштувань, таких як заголовок, опис і контактна інформація. Замість жорсткого кодування цих параметрів у додатку, їх можна буде налаштувати за допомогою патерн «Options». Почніть зі створення нової папки з назвою Swagger у кореневому каталозі проекту AutoLot.Api.
Почніть зі створення нової папки з назвою Swagger у кореневому каталозі проекту AutoLot.Api. У цій папці створіть ще одну папку з назвою Models. У папці Models створіть новий клас з назвою SwaggerVersionDescription.cs та оновіть його до наступного:

```cs
namespace AutoLot.Api.Swagger.Models;

public class SwaggerVersionDescription
{
    public int MajorVersion { get; set; }
    public int MinorVersion { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }
}
```
Далі створіть ще один клас під назвою SwaggerApplicationSettings.cs та оновіть його до наступного:

```cs
namespace AutoLot.Api.Swagger.Models;

public class SwaggerApplicationSettings
{
    public string Title { get; set; }
    public List<SwaggerVersionDescription> Descriptions { get; set; } = new List<SwaggerVersionDescription>();
    public string ContactName { get; set; }
    public string ContactEmail { get; set; }
}
```
Налаштування додаються до файлу appsettings.json, оскільки вони не змінюватимуться між середовищами:

```json
{
  "AllowedHosts": "*",
  "SwaggerApplicationSettings": {
    "Title": "AutoLot APIs",
    "Descriptions": [
      {
        "MajorVersion": 0,
        "MinorVersion": 0,
        "Status": "",
        "Description": "Unable to obtain version description."
      },
      {
        "MajorVersion": 0,
        "MinorVersion": 5,
        "Status": "",
        "Description": "Deprecated Version 0.5"
      },
      {
        "MajorVersion": 1,
        "MinorVersion": 0,
        "Status": "",
        "Description": "Version 1.0"
      },
      {
        "MajorVersion": 2,
        "MinorVersion": 0,
        "Status": "",
        "Description": "Version 2.0"
      },
      {
        "MajorVersion": 2,
        "MinorVersion": 0,
        "Status": "Beta",
        "Description": "Version 2.0-Beta"
      }
    ],
    "ContactName": "Alex Good",
    "ContactEmail": "alex@myshop.com"
  }

}
```
Оновіть файл GlobalUsings.cs, щоб включити нові простори імен:

```cs
global using AutoLot.Api.Swagger;
global using AutoLot.Api.Swagger.Models;
```
Дотримуючись шаблону використання методів розширення для реєстрації сервісів, створіть новий публічний статичний клас з назвою SwaggerConfiguration у папці Swagger.


```cs
namespace AutoLot.Api.Swagger;

public static class SwaggerConfiguration
{
    public static void AddAndConfigureSwagger(
    this IServiceCollection services,
    IConfiguration config,
    string xmlPathAndFile,
    bool addBasicSecurity)
    {
        //implementation goes here
    }
}
```
Тепер, коли метод розширення налаштовано, зареєструйте параметри за допомогою шаблону «Options»:

```cs
    public static void AddAndConfigureSwagger(
    this IServiceCollection services,
    IConfiguration config,
    string xmlPathAndFile,
    bool addBasicSecurity)
    {
        services.Configure<SwaggerApplicationSettings>(config.GetSection(nameof(SwaggerApplicationSettings)));
    }
```
Нарешті, викличте метод розширення у файлі Program.cs безпосередньо перед викликом AddSwaggerGen():

```cs
builder.Services.AddAndConfigureSwagger(
    builder.Configuration,
    Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"),
    true);
```

## Фільтр операцій SwaggerDefaultValues

Коли API мають версії (як у цьому випадку), код Swagger, який постачається зі стандартним шаблоном сервісу ASP.NET Core RESTful, не налаштований для обробки версій. У папці Swagger створіть новий клас з назвою SwaggerDefaultValues.cs та оновіть його до наступного:

```cs
namespace AutoLot.Api.Swagger;

/// <summary>
/// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
/// </summary>
/// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
/// Once they are fixed and published, this class can be removed.</remarks>
public class SwaggerDefaultValues : IOperationFilter
{
    /// <summary>
    /// Applies the filter to the specified operation using the given context.
    /// </summary>
    /// <param name="operation">The operation to apply the filter to.</param>
    /// <param name="context">The current operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        //operation.Deprecated = (operation.Deprecated | apiDescription.IsDeprecated())
        operation.Deprecated |= apiDescription.IsDeprecated();

        // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1752#issue-663991077
        foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
        {
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/b7cf75e7905050305b115dd96640ddd6e74c7ac9/src/Swashbuckle.AspNetCore.SwaggerGen/SwaggerGenerator/SwaggerGenerator.cs#L383-L387
            var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
            var response = operation.Responses[responseKey];

            foreach (var contentType in response.Content.Keys)
            {
                if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                {
                    response.Content.Remove(contentType);
                }
            }
        }

        if (operation.Parameters == null)
        {
            return;
        }

        // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
        // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413 -> closed
        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata?.Description;

            if (parameter.Schema.Default == null && description.DefaultValue != null)
            {
                // REF: https://github.com/Microsoft/aspnet-api-versioning/issues/429#issuecomment-605402330
                var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata.ModelType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}
```

## Клас ConfigureSwaggerOptions

Наступний клас, який потрібно додати, також надається зі зразків ASP.NET Core, але тут його змінено для використання SwaggerApplicationSettings. Створіть публічний клас з назвою ConfigureSwaggerOptions у папці Swagger та реалізуйте в ньому IConfigureOptions\<SwaggerGenOptions\>, ось так:

```cs
namespace AutoLot.Api.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        throw new NotImplementedException();
    }
}

```
У конструкторі візьміть екземпляр IApiVersionDescriptionProvider та OptionsMonitor для SwaggerApplicationSettings та призначте кожному змінну рівня класу :

```cs
    readonly IApiVersionDescriptionProvider _provider;
    private readonly SwaggerApplicationSettings _settings;

    public ConfigureSwaggerOptions(
        IApiVersionDescriptionProvider provider,
        IOptionsMonitor<SwaggerApplicationSettings> settingsMonitor)
    {
        _provider = provider;
        _settings = settingsMonitor.CurrentValue;
    }
```
Метод Configure() перебирає версії API, генеруючи документ Swagger для кожної версії. Додайте наступний метод:

```cs
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, _settings));
        }
    }
```
Метод CreateInfoForApiVersion() створює екземпляр об'єкта OpenApiInfo для кожної версії. OpenApiInfo містить описову інформацію для програми, таку як назва, інформація про версію, опис тощо.

```cs
    internal static OpenApiInfo CreateInfoForApiVersion(
        ApiVersionDescription description, 
        SwaggerApplicationSettings settings)
    {
        var versionDesc =
            settings.Descriptions.FirstOrDefault(x =>
                x.MajorVersion == (description.ApiVersion.MajorVersion ?? 0)
                && x.MinorVersion == (description.ApiVersion.MinorVersion ?? 0)
                && (string.IsNullOrEmpty(description.ApiVersion.Status) || x.Status==description.ApiVersion.Status));
        var info = new OpenApiInfo()
        {
            Title = settings.Title,
            Version = description.ApiVersion.ToString(),
            Description = $"{versionDesc?.Description}",
            Contact = new OpenApiContact() { Name = settings.ContactName, Email = settings.ContactEmail },
            TermsOfService = new System.Uri("https://www.linktotermsofservice.com"),
            License = new OpenApiLicense() { Name = "MIT", Url = new System.Uri("https://opensource.org/licenses/MIT") }
        };
        if (description.IsDeprecated)
        {
            info.Description += "<p><font color='red'>This API version has been deprecated.</font></p>";
        }

        return info;
    }
```

## Оновіть виклик SwaggerGen()

Шаблон за замовчуванням називається просто AddSwaggerGen() у файлі Program.cs з операторами верхнього рівня, що додає дуже базову підтримку. Видаліть цей рядок з операторів верхнього рівня та додайте його до методу AddAndConfigureSwagger(). У наступному коді ввімкнено анотації, встановлено OperationFilter та додано XML-коментарі.Якщо безпека не ввімкнена, метод на цьому завершується. Якщо запитується безпека, решта методу додає підтримку базової автентифікації в інтерфейс Swagger.

```cs
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.OperationFilter<SwaggerDefaultValues>();
            c.IncludeXmlComments(xmlPathAndFile);
            if (!addBasicSecurity)
            {
                return;
            }
            c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                In = ParameterLocation.Header,
                Description = "Basic Authorization header using the Bearer scheme."
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "basic"
                        }
                    },
                    new List<string> {}
                }
            });
        });
```

## Оновлення виклику UseSwaggerUI()

Останній крок – замінити виклик UseSwaggerUI() у файлі Program.cs версією, яка використовує весь фреймворк, який ми щойно створили. У виклику екземпляр IApiVersionDescriptionProvider отримується з контейнера DI та використовується для циклічного перебору версій API, що підтримуються застосунком, створюючи нову кінцеву точку Swagger UI для кожної версії.

```cs
// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
// specifying the Swagger JSON endpoint.
app.UseSwaggerUI(
    options =>
    {
        using var scope = app.Services.CreateScope();
        var versionProvider = scope.ServiceProvider.GetRequiredService<IApiVersionDescriptionProvider>();
        // build a swagger endpoint for each discovered API version
        foreach (var description in versionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
```
Тепер, коли все налаштовано, запустіть програму. Перейдіть по посиланням.

```console
https://localhost:5011/swagger/v1/swagger.json
https://localhost:5011/swagger/v0.5/swagger.json
https://localhost:5011/swagger/v2/swagger.json
```

## Додаткові параметри документації для кінцевих точок API

Існують додаткові атрибути, що доповнюють документацію Swagger. Атрибут Produces вказує тип вмісту для кінцевої точки. Атрибут ProducesResponseType використовує перерахування StatusCodes для позначення можливого коду повернення кінцевої точки. Оновіть метод Get() класу ValuesController, щоб вказати application/json як тип повернення, а результат дії повертав або 200 OK, 400 Bad Request, або 401 Unauthorized.

```cs
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult Get()
    {
        return Ok(new string[] { "value1", "value2" });
    }
```

Хоча атрибут ProducesResponseType додає коди відповідей до документації, цю інформацію не можна налаштувати. На щастя, Swashbuckle додає атрибут SwaggerResponse саме для цієї мети. Оновіть метод Get() до наступного:

```cs
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, "The execution was successful")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted")]
    public IActionResult Get()
    {
        return Ok(new string[] { "value1", "value2" });
    }
```
Перш ніж анотації Swagger будуть отримані та додані до згенерованої документації, їх необхідно ввімкнути. Вони вже були ввімкнені в методі AddAndConfigureSwagger(). Тепер, коли ви переглядатимете розділ відповідей інтерфейсу Swagger, ви побачите налаштовані повідомлення.

# Стовреня функцій AutoLot.Api
## Створення BaseCrudController

Більшість функцій програми AutoLot.Api можна класифікувати як один із таких методів:

    GetOne()
    GetAll()
    UpdateOne()
    AddOne()
    DeleteOne()

Основні методи API будуть реалізовані в узагальненому базовому контролері API. Оновіть файл GlobalUsings.cs, додавши наступне:

```cs
global using AutoLot.Dal.Exceptions;
global using AutoLot.Dal.Repos;
global using AutoLot.Dal.Repos.Base;
global using AutoLot.Dal.Repos.Interfaces;
global using AutoLot.Models.Entities;
global using AutoLot.Models.Entities.Base;
```
Далі створіть нову папку з назвою Base в каталозі Controllers. У цій папці додайте новий клас з назвою BaseCrudController.cs та оновіть визначення класу до наступного:

```cs
namespace AutoLot.Api.Controllers.Base;

[ApiController]
[Route("api/[controller]")]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class BaseCrudController<TEntity, TController> : ControllerBase
    where TEntity : BaseEntity, new()
    where TController : class
{
    //implementation goes here
}
```
Додайте наступний оператор до файлу GlobalUsings.cs:

```cs
global using AutoLot.Api.Controllers.Base;
```
Клас є публічним та абстрактним і успадковує ControllerBase. Клас приймає два узагальнені параметри. Перший тип обмежений тим, щоб бути похідним від BaseEntity та мати конструктор за замовчуванням, а другий має бути класом (для фреймворку логування). Як обговорювалося раніше, коли атрибут ApiController додається до базового класу, похідні контролери отримують функціональність, що надається цим атрибутом.

### Конструктор

Наступним кроком є ​​додавання двох protected змінних рівня класу: однієї для зберігання екземпляра IBaseRepo\<TEntity\>, а іншої для зберігання екземпляра IAppLogging\<TController\>.Обидві з них слід встановити за допомогою конструктора.

```cs
    protected readonly IBaseRepo<TEntity> MainRepo;
    protected readonly IAppLogging<TController> Logger;

    protected BaseCrudController(IAppLogging<TController> logger, IBaseRepo<TEntity> repo)
    {
        MainRepo = repo;
        Logger = logger;
    }
```
Тип сутності для репозиторію відповідає типу сутності для похідного контролера. Наприклад, CarsController використовуватиме CarRepo. Це дозволяє виконувати роботу, специфічну для типу, в похідних контролерах, але інкапсулює прості CRUD-операції в базовому контролері.

### Методи Get

Існує чотири HTTP-методи Get: GetOnBad(), GetOneFuture(), GetOne() та GetAll(). У цій програмі припустимо, що метод GetOneBad() застарів як частина версії 0.5. GetOneFuture() є частиною наступного бета-релізу (2.0-Beta), тоді як методи GetOne() та GetAll() є частиною робочого API версії 1.0.
Додайте метод GetAllBad() ось так:

```cs
    /// <summary>
    /// Gets all records. Deprecated
    /// </summary>
    /// <returns>All records</returns>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, "The execution was successful")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted")]
    [ApiVersion("0.5", Deprecated = true)]
    [HttpGet]
    public ActionResult<IEnumerable<TEntity>> GetAllBad()
    {
        throw new Exception("I said not to use this one");
    }
```
Коли версія вважається застарілою, необхідно додати прапорець «Deprecated» до всіх атрибутів ApiVersion у вашій програмі, щоб гарантувати правильне повідомлення про версії.
Далі додайте майбутню реалізацію GetAllFuture() ось так:

```cs
    /// <summary>
    /// Gets all records really fast (when it’s written)
    /// </summary>
    /// <returns>All records</returns>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, "The execution was successful")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted access attempted")]
    [ApiVersion("2.0-Beta")]
    [HttpGet]
    public ActionResult<IEnumerable<TEntity>> GetAllFuture()
    {
        throw new NotImplementedException("I'm working on it");
    }
```
Тепер час створити справжні методи отримання. Спочатку додайте метод GetAll(). Цей метод слугує кінцевою точкою для маршруту get all похідного контролера (наприклад, /Cars).

```cs
    /// <summary>
    /// Gets all records
    /// </summary>
    /// <returns>All records</returns>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, "The execution was successful")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted")]
    [ApiVersion("1.0")]
    [HttpGet]
    public ActionResult<IEnumerable<TEntity>> GetAll()
    {
        return Ok(MainRepo.GetAllIgnoreQueryFilters());
    }
```

Наступний метод отримує один запис на основі ідентифікатора, який передається як обов'язковий параметр маршруту (наприклад, /Cars/5).

```cs
    /// <summary>
    /// Gets a single record
    /// </summary>
    /// <param name="id">Primary key of the record</param>
    /// <returns>Single record</returns>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, "The execution was successful")]
    [SwaggerResponse(204, "No content")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted")]
    [ApiVersion("1.0")]
    [HttpGet("{id}")]
    public ActionResult<TEntity> GetOne(int id)
    {
        var entity = MainRepo.Find(id);

        if (entity == null)
        {
            return NoContent();
        }

        return Ok(entity);
    }
```
Значення маршруту автоматично присвоюється параметру id (неявно [FromRoute]).

### Метод UpdateOne

Метод HTTP Put представляє оновлення запису.Метод наведено тут з наступним поясненням:

```cs
    /// <summary>
    /// Updates a single record
    /// </summary>
    /// <remarks>
    /// Sample body:
    /// <pre>
    /// {
    ///   "Id": 1,
    ///   "TimeStamp": "AAAAAAAAB+E="
    ///   "MakeId": 1,
    ///   "Color": "Black",
    ///   "PetName": "Zippy",
    ///   "MakeColor": "VW (Black)",
    /// }
    /// </pre>
    /// </remarks>
    /// <param name="id">Primary key of the record to update</param>
    /// <param name="entity">Entity to update</param>
    /// <returns>Single record</returns>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, "The execution was successful")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted")]
    [HttpPut("{id}")]
    [ApiVersion("1.0")]
    public IActionResult UpdateOne(int id, TEntity entity)
    {
        if (id != entity.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            MainRepo.Update(entity);
        }
        catch (CustomException ex)
        {
            //This shows an example with the custom exception
            //Should handle more gracefully
            return BadRequest(ex);
        }
        catch (Exception ex)
        {
            //Should handle more gracefully
            return BadRequest(ex);
        }

        return Ok(entity);
    }

```

Метод починається з встановлення маршруту як HttpPut запиту на основі маршруту похідного контролера з обов'язковим параметром маршруту id. Значення маршруту призначається параметру id (неявний [FromRoute]), а сутність призначається з тіла запиту (неявний [FromBody]). 
Метод перевіряє, чи збігається значення маршруту (id) з ідентифікатором у тілі. Якщо ні, повертається BadRequest. Якщо це так, використовується явна перевірка на достовірність ModelState. Якщо ModelState недійсний, клієнту буде повернуто код 400 (BadRequest). Пам’ятайте, що явна перевірка на достовірність ModelState не потрібна, якщо неявна перевірка ввімкнена за допомогою атрибута ApiController.
Якщо до цього моменту все пройшло успішно, репозиторій використовується для оновлення запису. Якщо оновлення завершується невдачею з винятком, клієнту повертається код 400. Якщо все пройшло успішно, клієнту повертається код 200 (OK) з оновленим записом, що передається як тіло відповіді.
Обробка винятків у цьому прикладі (як і в решті прикладів) вкрай неадекватна. У виробничих застосунках слід використовувати все, що ви вивчили до цього моменту, а також фільтри винятків (представлені пізніше в цьому розділі) для коректної обробки проблем відповідно до вимог.

### Метод AddOne

Метод HTTP Post представляє вставку запису. Метод наведено тут з наступним поясненням: 


```cs
    /// <summary>
    /// Adds a single record
    /// </summary>
    /// <remarks>
    /// Sample body:
    /// <pre>
    /// {
    ///   "Id": 1,
    ///   "TimeStamp": "AAAAAAAAB+E="
    ///   "MakeId": 1,
    ///   "Color": "Black",
    ///   "PetName": "Zippy",
    ///   "MakeColor": "VW (Black)",
    /// }
    /// </pre>
    /// </remarks>
    /// <returns>Added record</returns>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(201, "The execution was successful")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted")]
    [HttpPost]
    [ApiVersion("1.0")]
    public ActionResult<TEntity> AddOne(TEntity entity)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            MainRepo.Add(entity);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }

        return CreatedAtAction(nameof(GetOne), new { id = entity.Id }, entity);
    }
```

Цей метод починається з визначення маршруту як HTTP Post. Немає параметра маршруту, оскільки це новий запис. Якщо ModelState є дійсним і репозиторій успішно додає запис, відповіддю буде CreatedAtAction(). Це повертає клієнту HTTP 201 з URL-адресою щойно створеної сутності як значенням заголовка Location. Тіло відповіді – це щойно додана сутність у форматі JSON.

### Метод DeleteOne

Метод HTTP Delete представляє видалення запису. Після створення екземпляра з вмісту тіла, використовуйте репозиторій для обробки видалення. Повний метод наведено тут:

```cs
    /// <summary>
    /// Deletes a single record
    /// </summary>
    /// <remarks>
    /// Sample body:
    /// <pre>
    /// {
    ///   "Id": 1,
    ///   "TimeStamp": "AAAAAAAAB+E="
    /// }
    /// </pre>
    /// </remarks>
    /// <returns>Nothing</returns>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, "The execution was successful")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted")]
    [HttpDelete("{id}")]
    [ApiVersion("1.0")]
    public ActionResult<TEntity> DeleteOne(int id, TEntity entity)
    {
        if (id != entity.Id)
        {
            return BadRequest();
        }

        try
        {
            MainRepo.Delete(entity);
        }
        catch (Exception ex)
        {
            //Should handle more gracefully
            return new BadRequestObjectResult(ex.GetBaseException()?.Message);
        }

        return Ok();
    }
```
Цей метод починається з визначення маршруту як HTTP Delete з ідентифікатором як обов'язковим параметром маршруту. Ідентифікатор у маршруті порівнюється з ідентифікатором, надісланим разом з рештою сутності в тілі, і якщо вони не збігаються, повертається BadRequest. Якщо репозиторій успішно видаляє запис, відповідь – OK; якщо є помилка, відповідь – BadRequest.
Якщо ви пам'ятаєте розділи EF Core, сутність можна видалити, маючи лише її первинний ключ та значення позначки часу. Це дозволяє видаляти об'єкт без надсилання всієї сутності в запиті. Якщо клієнти використовують цю скорочену версію, надсилаючи лише Id та TimeStamp, цей метод завершиться невдачею, якщо ввімкнено неявну перевірку ModelState та виконуються перевірки валідації для решти властивостей.

## CarsController

Додатку AutoLot.Api потрібен додатковий метод HTTP Get для отримання записів Car на основі значення Make. Це перейде до нового класу під назвою CarsController. Створіть новий API-контролер з назвою CarsController у папці Controllers. CarsController походить від BaseCrudController, а конструктор приймає специфічне для сутності репозиторій та екземпляр логера. Ось початкова структура контролера:

```cs
namespace AutoLot.Api.Controllers;

public class CarsController : BaseCrudController<Car, CarsController>
{
    public CarsController(IAppLogging<CarsController> logger, ICarRepo repo) : base(logger, repo)
    {
    }

}
```
Клас CarsController розширює базовий клас ще одним методом дії, який отримує всі автомобілі певної марки.
Додайте наступний код:

```cs
    /// <summary>
    /// Gets all cars by make
    /// </summary>
    /// <returns>All cars for a make</returns>
    /// <param name="id">Primary key of the make</param>
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [SwaggerResponse(200, "The execution was successful")]
    [SwaggerResponse(400, "The request was invalid")]
    [SwaggerResponse(401, "Unauthorized access attempted")]
    [HttpGet("bymake/{id?}")]
    [ApiVersion("1.0")]
    public ActionResult<IEnumerable<Car>> GetCarsByMake(int? id)
    {
        if (id.HasValue && id.Value > 0)
        {
            return Ok(((ICarRepo)MainRepo).GetAllBy(id.Value));
        }
        return Ok(MainRepo.GetAllIgnoreQueryFilters());
    }

```
Атрибут HTTP Get розширює маршрут константою bymake, а потім необов'язковим ідентифікатором марки, за якою потрібно фільтрувати, наприклад:

```console
https://localhost:5011/api/cars/bymake/5
```
Далі перевіряється, чи було передано значення для ідентифікатора. Якщо ні, отримується інформація про всі транспортні засоби. Якщо значення було передано, використовується метод GetAllBy() класу CarRepo для отримання автомобілів за маркою. Оскільки захищена властивість MainRepo базового класу визначена як IBaseRepo\<TEntity\>, її необхідно привести назад до інтерфейсу ICarRepo.

## Решта контролерів

Решта контролерів, специфічних для сутностей, походять від BaseCrudController, але не додають жодної додаткової функціональності. Додайте ще сім API-контролерів з назвами CarDriversController, CreditRisksController, CustomersController, DriversController, MakesController, OrdersController та RadiosController до папки Controllers.

```cs
namespace AutoLot.Api.Controllers;

public class CarDriversController : BaseCrudController<CarDriver, CarDriversController>
{
    public CarDriversController(IAppLogging<CarDriversController> logger, ICarDriverRepo repo )
        : base(logger,repo)
    {
    }
}

public class CreditRisksController : BaseCrudController<CreditRisk, CreditRisksController>
{
    public CreditRisksController(IAppLogging<CreditRisksController> logger, ICreditRiskRepo repo )
        : base(logger,repo)
    {
    }
}

public class CustomersController : BaseCrudController<Customer, CustomersController>
{
    public CustomersController(IAppLogging<CustomersController> logger, ICustomerRepo repo)
        : base(logger, repo)
    {
    }
}

public class DriversController : BaseCrudController<Driver, DriversController>
{
    public DriversController(IAppLogging<DriversController> logger, IDriverRepo repo)
        : base(logger, repo)
    {
    }
}

public class MakesController : BaseCrudController<Make, MakesController>
{
    public MakesController(IAppLogging<MakesController> logger, IMakeRepo repo)
        : base(logger, repo)
    {
    }
}

public class OrdersController : BaseCrudController<Order, OrdersController>
{
    public OrdersController(IAppLogging<OrdersController> logger,IOrderRepo repo)
        : base(logger, repo)
    {
    }
}

public class RadiosController : BaseCrudController<Radio, RadiosController>
{
    public RadiosController(IAppLogging<RadiosController> logger,IRadioRepo repo)
        : base(logger, repo)
    {
    }
}

```

На цьому налаштування всіх контролерів завершено, і ви можете використовувати інтерфейс користувача Swagger для тестування всієї функціональності. Якщо ви збираєтеся додавати/оновлювати/видаляти записи, оновіть значення RebuildDataBase на true у файлі appsettings.development.json.
При тестувані методів може виникнути помилка валідації. Це можна вирішити вказавши властивості сутності як не обов'язкві. Наприклад

```cs
public partial class Car : BaseEntity
{


    //public virtual Radio RadioNavigation { get; set; } = null!;


    [InverseProperty(nameof(Radio.CarNavigation))]
    public virtual Radio? RadioNavigation { get; set; } = null!;

}
```


# Фільтри винятків 

Коли у веб-API-додатку виникає виняток, сторінка помилки не відображається, оскільки клієнтом зазвичай є інша програма, а не людина. Будь-яка інформація має бути надіслана у форматі JSON разом із кодом стану HTTP. Як обговорювалося раніше, можна створювати фільтри, які запускаються у разі необробленого винятку. Фільтри можна застосовувати глобально, на рівні контролера або на рівні дії. Для цієї програми ви збираєтеся створити фільтр винятків для надсилання відформатованого JSON назад (разом із HTTP 500) та включення трасування стека, якщо сайт працює в режимі налагодження.

    Фільтри – це надзвичайно потужна функція .NET Core. У цьому розділі ми розглядаємо лише фільтри винятків, але існує багато інших, які можна створити, що може значно заощадити час під час створення застосунків ASP.NET Core. Повну інформацію про фільтри дивіться в документації.

## Створення CustomExceptionFilter

Перед створенням фільтра додайте наступний оператор до файлу GlobalUsings.cs:

```cs
global using Microsoft.AspNetCore.Mvc.Filters;
```
Створіть новий каталог з назвою Filters, і в цьому каталозі додайте новий клас з назвою CustomExceptionFilterAttribute.cs. Змініть клас на public та успадкуйте від ExceptionFilterAttribute. Перевизначте метод OnException(), як показано тут:

```cs

namespace AutoLot.Api.Filters;

public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        
    }
}
```
На відміну від більшості фільтрів в ASP.NET Core, які мають обробник подій «до» та «після», фільтри винятків мають лише один обробник: OnException() (або OnExceptionAsync()). Цей обробник має один параметр, ExceptionContext. Цей параметр надає доступ до ActionContext, а також до винятку, який був викликаний.
Фільтри також беруть участь у впровадженні залежностей, дозволяючи доступ до будь-якого елемента контейнера в коді. У цьому прикладі нам потрібен екземпляр IWebHostEnvironment, вставлений у фільтр. Це буде використано для визначення середовища виконання. Якщо середовищем є «Розробка», відповідь також повинна містити трасування стека. Додайте змінну рівня класу для зберігання екземпляра IWebHostEnvironment та додайте конструктор, як показано тут:

```cs
    private readonly IWebHostEnvironment _hostEnvironment;

    public CustomExceptionFilterAttribute(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

```
Код в обробнику події OnException() перевіряє тип винятку та створює відповідну відповідь. Якщо середовищем є Development, трасування стека включається до повідомлення відповіді. Динамічний об'єкт, що містить значення, що надсилаються на виклик запиту, створюється та повертається в IActionResult.

```cs
    public override void OnException(ExceptionContext context)
    {
        var ex = context.Exception;
        string stackTrace = _hostEnvironment.IsDevelopment() ? context.Exception.StackTrace : string.Empty;
        string message = ex.Message;
        string error;
        IActionResult actionResult;
        switch (ex)
        {
            case DbUpdateConcurrencyException ce:
                //Returns a 400
                error = "Concurrency Issue.";
                actionResult = new BadRequestObjectResult(
                    new { Error = error, Message = message, StackTrace = stackTrace });
                break;
            default:
                error = "General Error.";
                actionResult = new ObjectResult(new { Error = error, Message = message, StackTrace = stackTrace })
                {
                    StatusCode = 500
                };
                break;
        }

        //context.ExceptionHandled = true; //If this is uncommented, the exception is swallowed
        context.Result = actionResult;
    }
```
Якщо ви хочете, щоб фільтр винятків проковтнув винятки та встановив відповідь на 200 (наприклад, щоб зареєструвати помилку, але не повернути її клієнту), додайте такий рядок перед встановленням результату (закоментовано в попередньому прикладі):

```cs
context.ExceptionHandled = true;
```
Нарешті, додайте наступний оператор до файлу GlobalUsings.cs:

```cs
global using AutoLot.Api.Filters;
```
## Застосування фільтра

Нагадуємо, що фільтри можна застосовувати до методів дій, контролерів або глобально до програми. Код фільтрів before виконується ззовні всередину (глобальний, контролер, метод дії), тоді як код after виконується зсередини назовні (метод дії, контролер, глобальний). Для фільтра винятків OnException() спрацьовує після виконання методу дії.
Додавання фільтрів на рівні програми виконується за допомогою методу AddControllers() у верхньому рядку файлу Program.cs. Відкрийте файл та оновіть метод AddControllers() до наступного:

```cs
builder.Services.AddControllers(config =>
    {
        config.Filters.Add(new CustomExceptionFilterAttribute(builder.Environment));
    })
    .AddJsonOptions(options =>
    {
        //...
    }).ConfigureApiBehaviorOptions(options =>
    {
        //...
    });
```

## Тестування фільтра винятків

Щоб перевірити фільтр винятків, запустіть програму та скористайтеся одним із застарілих методів Get() (наприклад, /api/Car) за допомогою Swagger. Тіло відповіді в інтерфейсі Swagger має відповідати наступному:

```console
{
  "Error": "General Error.",
  "Message": "I said not to use this one",
  "StackTrace": "   at AutoLot.Api.Controllers.Base.BaseCrudController`2.GetAllBad() in D:\\...\\AutoLot\\AutoLot.Api\\Controllers\\Base\\BaseCrudController.cs:line 34 ... "
}
```
Також можна зробити запит

```console
curl -i https://localhost:5011/api/v2.0.Beta/cars
```


# Додайте підтримку запитів із перехресних джерел. (Cross-Origin Requests Support) 

API повинні мати політики, які дозволяють або забороняють клієнтам, що походять з іншого сервера, взаємодіяти з API. Такі типи запитів називаються крос-запитами (CORS). Хоча це не потрібно, коли ви працюєте локально на своєму комп'ютері у світі, повністю заснованому на ASP.NET Core, це потрібно фреймворкам JavaScript, які хочуть взаємодіяти з вашим API, навіть коли всі вони працюють локально.
 
    Щоб отримати додаткові відомості про підтримку CORS, зверніться до документації за запитом: "Enable Cross-Origin Requests (CORS) in ASP.NET Core"

# Створення політики CORS

ASP.NET Core має розширену підтримку для налаштування ядер, включаючи методи для дозволу/заборони заголовків, методів, походження, облікових даних тощо. 

    Деталі по запиту "Enable Cross-Origin Requests (CORS) in ASP.NET Core"

У цьому прикладі ми залишимо все якомога відкритіше. Зверніть увагу, що це точно не те, що ви хочете робити зі своїми реальними програмами. Налаштування починається зі створення політики CORS та додавання цієї політики до колекції служб. Політика створюється з назвою, а потім правилами.
У наступному прикладі створюється політика з назвою AllowAll і виконується саме це. Додайте наступний код до операторів верхнього рівня у файлі Program.cs перед викликом var app = builder.Build().

```cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyOrigin();
    });
});
```

## Додавання політики CORS до обробки HTTP-конвеєра

Останній крок – додати політику CORS до обробки HTTP-конвеєра. Додайте наступний рядок до операторів верхнього рівня файлу Program.cs.cs, переконавшись, що він знаходиться після виклику методу app.UseHttpsRedirection():

```cs
app.UseHttpsRedirection();

//Add CORS Policy
app.UseCors("AllowAll");

```