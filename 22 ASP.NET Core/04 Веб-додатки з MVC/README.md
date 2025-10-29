# 04 Веб-додатки з MVC

ASP.NET Core має багато функцій, спільних для всіх типів програм. У цьому розділі розглядаються такі області, як можливості інтерфейсу користувача ASP.NET Core, компоненти представлення, керування бібліотеками на стороні клієнта та завершення веб-застосунку AutoLot.Mvc.
Розділ починається з повернення V до MVC.

# «V» в ASP.NET Core

Під час створення сервісів ASP.NET Core використовуються лише M (моделі) та C (контролери) шаблону MVC. Користувацький інтерфейс створюється за допомогою V, або представлень(views) шаблону MVC. Представлення створюються за допомогою коду HTML, JavaScript, CSS та Razor. Вони опціонально мають базовий вигляд макета та відображаються з методу дії контролера або компонента вигляду.

## ViewResults та методи дій.

Результати ViewResults та PartialView – це ActionResults, які повертаються з методу дії за допомогою допоміжних методів Controller. PartialViewResult призначений для відображення всередині іншого представлення та не використовує представлення макета, тоді як ViewResult зазвичай відображається разом із представленням макета.
У ASP.NET Core прийнято, що View або PartialView відтворюють файл *.cshtml з тим самим ім'ям, що й метод. Очікується, що представлення буде розташоване або в папці з назвою контролера (без суфікса контролера), або в папці Shared (обидві розташовані в батьківській папці Views). Наприклад, наступний код відобразить представлення SampleAction.cshtml, розташоване в папці Views\Sample або Views\Shared:

```cs
//Controller name is the main folder under the Views folder ..\Views\Sample\SampleAction.cshtml
public class SampleController: Controller
{
  //Action name is the default name for the view under the controller’s folder
  public ActionResult SampleAction()
  {
    return View();
  }
}
```
Спочатку шукається папка представлення, названа відповідно до контролера. Якщо представлення не знайдено, шукається в папці Shared. Якщо його все ще не вдається знайти, виникає виняток.
Щоб відобразити представлення з назвою, яка відрізняється від назви методу дії, передайте назву файлу (без розширення cshtml).Наведений нижче код відобразить представлення CustomViewName.cshtml:

```cs
public ActionResult SampleAction()
{
  return View("CustomViewName");
}
```
Існує два перевантаження для передачі об'єкта даних, який стає моделлю для представлення. У першому прикладі використовується ім’я перегляду за замовчуванням, а в другому прикладі вказується інше ім’я перегляду.

```cs
public ActionResult SampleAction()
{
  var sampleModel = new SampleViewModel();
  return View(sampleModel);
}
public ActionResult SampleAction()
{
  var sampleModel = new SampleViewModel();
  return View("CustomViewName",sampleModel);
}
```
Коли для методу дії не знайдено представлення, поведінка залежить від конфігурації обробки помилок у файлі Program.cs. Під час використання сторінки винятків розробника до браузера передається багато інформації. Щоб побачити це в дії, відкрийте HomeController і додайте новий метод дії наступним чином:

```cs
[HttpGet]
Public async Task<IActionResult> RazorSyntaxAsync()
{
  return View();
}
```
Метод дії доповнюється атрибутом [HTTPGet], щоб встановити цей метод кінцевою точкою застосунку для маршруту /Home/RazorSyntax (пам’ятайте, що суфікс Async видаляється механізмом маршрутизації), якщо запит є HTTP Get. Метод повертає ViewResult за допомогою методу View. Оскільки представлення не було названо, ASP.NET Core шукатиме представлення з іменем RazorSyntax.cshtml у каталозі Views\Home або Views\Shared. Якщо представлення не знайдено в жодному з місць розташування, клієнту (браузеру) буде повернуто виняток.
Переконайтеся, що прапорець UseApi у файлі appsettings.Development.json встановлено на значення false. Потім запустіть програму та перейдіть за URL-адресою https://localhost:5001/Home/RazorSyntax, щоб переглянути сторінку винятків розробника.

```
An unhandled exception occurred while processing the request.
InvalidOperationException: The view 'RazorSyntax' was not found. The following locations were searched:
/Views/Home/RazorSyntax.cshtml
/Views/Shared/RazorSyntax.cshtml
...
```
Сторінка винятків розробника надає достатньо інформації для налагодження вашої програми, включаючи необроблені деталі винятків разом із трасуванням стека. Тепер закоментуйте цей рядок у файлі Program.cs, замінивши його «стандартним» обробником помилок, ось так:

```cs
    //app.UseDeveloperExceptionPage();
    app.UseExceptionHandler("/Home/Error");
```
Запустіть програму ще раз і перейдіть за адресою https://localhost:5001/Home/RazorSyntax, і ви побачите стандартну сторінку помилки.

Стандартний обробник помилок перенаправляє помилки до методу дії Error класу HomeController:

```cs
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
```
Якщо ви переглянете каталог \Views\Home, то побачите, що там немає представлення з назвою Error.cshtml. Це представлення розташоване в каталозі \Views\Shared. Якщо ви хочете налаштувати представлення, оновіть його вміст.
Не забудьте повернути метод Configure() для використання сторінки винятків розробника після завершення експериментів з процесом обробки помилок:

```cs
    app.UseDeveloperExceptionPage();
```
Щоб отримати додаткові відомості про налаштування обробки помилок та додаткові доступні опції, зверніться до документації: "Handle errors in ASP.NET Core"

# Механізм перегляду Razor та синтаксис Razor.

Механізм перегляду Razor використовує Razor як основну мову. Razor — це серверний код, вбудований у представлення, заснований на C#. Інтеграція Razor в HTML та CSS призводить до створення коду, який набагато чистіший та легший для читання. 
Щоб розпочати, додайте нове представлення, клацнувши правою кнопкою миші папку Views\Home у проекті AutoLot.Mvc та вибравши Add ➤ View. Виберіть «Razor View – Empty» у діалоговому вікні «Add New Scaffolded Item» та назвіть вигляд RazorSyntax.cshtml.

Представлення Razor зазвичай мають сувору типізацію за допомогою директиви @model (зверніть увагу на малу літеру m). Оновіть тип нового вигляду до сутності Car, додавши наступне у верхній частині вигляду:

```html
@model AutoLot.Models.Entities.Car
```
HTML, CSS, JavaScript і Razor працюють разом у перегляді Razor. Почніть з додавання тегу \<h1\> у верхню частину сторінки, щоб додати заголовок:

```html
<h1>Razor Syntax</h1>
```
Блоки операторів Razor відкриваються символом @ та є або самостійними операторами (як foreach), або укладені у фігурні дужки, як у наведених нижче прикладах:

```html
<h3>
@for (int i = 0; i < 13; i++)
{
    @i@:&nbsp; 
}
</h3>
<br />

@{
    //Code Block
    var foo = "Foo";
    var bar = "Bar";
    var htmlList = "<ul><li>one</li><li>two</li></ul>";
}
```
Щоб вивести значення змінної у представлення, просто використовуйте знак @ з іменем змінної. Це еквівалентно Response.Write(). Зверніть увагу, що після оператора немає крапки з комою, яка закриває його, під час виведення безпосередньо в браузер.

```html
@foo<br />
@htmlList<br />
@foo.@bar<br />
```
У попередньому прикладі (@foo.@bar) дві змінні були розділені крапкою (.). Це не звичайна нотація C# з "крапкою" для навігації ланцюжком властивостей. Це просто вивід двох змінних у потік відповідей з фізичною крапкою між ними. Якщо вам потрібно «розставити крапку» вниз по змінній, використовуйте символ @ на змінній, а потім пишіть свій код як завжди.

```html
@foo.ToUpper()<br/>
```
Якщо ви хочете вивести необроблений HTML, ви використовуєте так званий помічник HTML. Це вбудовані в Razor View Engine допоміжні засоби. Ось рядок для виведення необробленого HTML:

```html
@Html.Raw(htmlList)
<br/>
<hr />
```

Аби використати модель змінимо код дії:

```cs
        public async Task<IActionResult> RazorSyntaxAsync()
        {
            Car car = new() { Id = 1, MakeId = 1, Color = "Blue", PetName = "Snoopy" };
            return View(car);
        }
```


В перегляді, блоки коду можуть змішувати розмітку та код. Рядки, що починаються з розмітки, інтерпретуються як HTML, тоді як усі інші рядки інтерпретуються як код. Якщо рядок починається з тексту, який не є кодом, необхідно використовувати індикатор вмісту (@:) або індикатор блоку вмісту \<text\>\</text\>. Зверніть увагу, що рядки можуть переходити між собою. Ось приклад:

```cshtml
@{
    @:Straight Text
    <div>Value:@Model.PetName</div>
    <text>
        Lines without HTML tag
    </text>
    <br />
}
<hr />
```
Якщо ви хочете екранувати символ @, використовуйте подвійний @. Razor також достатньо розумний для обробки адрес електронної пошти, тому їх не потрібно екранувати. Якщо вам потрібно, щоб Razor обробляв знак @, як токен Razor, додайте дужки.

```cshtml
Email Address Handling:
<br />
foo@foo.com
<br />
@@foo
<br />
test@foo
<br />
test@(foo)
<br />
```
Попередній код виводить foo@foo.com, @foo, test@foo та testFoo відповідно.

Коментарі Razor відкриваються символом @* та закриваються символом *@.

```cshtml
@*
    Multiline Comments
    Hi.
*@
```
Razor також підтримує вбудовані функції. Наведений нижче приклад функції сортує список рядків:

```cshtml
@functions {
    public static IList<string> SortList(IList<string> strings)
    {
        var list = from s in strings orderby s select s;
        return list.ToList();
    }
}
```
Наведений нижче код створює список рядків, сортує їх за допомогою функції SortList(), а потім виводить відсортований список у браузер:

```cshtml
@{
    var myList = new List<string> {"C", "A", "Z", "F"};
    var sortedList = SortList(myList); 
}
@foreach (string s in sortedList)
{
    @s@:&nbsp;
}
<hr/>
```
Ось ще один приклад, який створює делегат, що може бути використаний для встановлення жирного шрифту в рядку:

```cshtml
@{
    Func<dynamic, object> b = @<strong>@item</strong>;
}
This will be bold: @b("Foo")
```
Razor також містить HTML-помічники, які є методами, що надаються ASP.NET Core. Два приклади — DisplayForModel() та EditorForModel(). Перший використовує відображення моделі представлення для відображення на веб-сторінці. Останній також використовує рефлексію для створення HTML-коду для форми редагування (зауважте, що він не надає теги Form, а лише розмітку для моделі). Помічники HTML будуть детально розглянуті пізніше в цьому розділі.
ASP.NET Core має допоміжні засоби тегів. Допоміжні засоби тегів поєднують розмітку та код і розглядаються пізніше в цьому розділі.

# Представлення (View)

Представлення (Views) – це спеціальні файли коду з розширенням cshtml, написані з використанням комбінації розмітки HTML, CSS, JavaScript та синтаксису Razor.

## Каталог Views

Папка Views – це місце, де зберігаються представлення в проектах ASP.NET Core, використовуючи шаблон MVC. У кореневому каталозі папки «Views» є два файли: _ViewStart.cshtml та _ViewImports.cshtml.
Файл _ViewStart.cshtml виконує свій код перед відображенням будь-якого іншого представлення (за винятком часткових представлень та макетів). Файл зазвичай використовується для встановлення макета за замовчуванням для переглядів, які не вказують його.Макети детальніше обговорюються в розділі «Макети». Файл _ViewStart.cshtml показано тут:

```cshtml
@{
    Layout = "_Layout";
}
```
Файл _ViewImports.cshtml використовується для імпорту спільних директив, таких як оператори @using. Його вміст застосовується до всіх представлень в одному каталозі або підкаталозі файлу _ViewImports. Цей файл є еквівалентом представлення файлу GlobalUsings.cs для коду C#. Додайте оператор @using для AutoLot.Models.Entities.

```cshtml
@using AutoLot.Mvc
@using AutoLot.Mvc.Models
@using AutoLot.Models.Entities
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```
Рядок @addTagHelper буде описано разом із помічниками тегів.

Як згадувалося, кожен контролер отримує власний каталог у папці Views, де зберігаються його специфічні представлення. Назви відповідають назві контролера (без слова Controller). Наприклад, каталог Views\Cars містить усі представлення (views) для CarsController. Перегляди зазвичай називаються за методами дії, які їх рендерять, хоча імена можна змінювати, як показано раніше.

## Каталог Shared

У розділі Views є спеціальний каталог під назвою Shared. У цьому каталозі містяться представлення, доступні всім контролерам та діям. Як згадувалося, якщо запитуваний файл перегляду не може бути знайдений у каталозі, що відповідає контролеру, пошук виконується у спільній папці.

## Папка DisplayTemplates

Папка DisplayTemplates містить користувацькі шаблони, які керують способом відображення типів і сприяють повторному використанню коду та забезпеченню узгодженості відображення. Коли викликаються методи DisplayFor()/DisplayForModel(), механізм перегляду Razor шукає шаблон з такою ж назвою, як і тип, що відображатиметься, наприклад, Car.cshtml для класу Car. Якщо власний шаблон неможливо знайти, розмітка відображається за допомогою рефлексії. Шлях пошуку починається в папці, наприклад, Views\CarController\DisplayTemplates, і якщо його не знайдено, то пошук здійснюється в папці Views\Shared\DisplayTemplates. Шаблони відображення не обов'язково повинні збігатися з назвою класу. Якщо назви шаблону та класу відрізняються, назви шаблонів необхідно передати в методи DisplayFor()/DisplayForModel().

  Методи DisplayFor()/DisplayForModel() будуть детально розглянуті пізніше в цьому розділі. Наразі знайте, що метод DisplayFor() приймає вираз для відображення властивості моделі, з якою пов'язане типізоване представлення. Метод DisplayForModel() відображає всю модель.

## Шаблон відображення дати та часу

Створіть нову папку з назвою DisplayTemplates у папці Views\Shared. Додайте нове представлення з назвою DateTime.cshtml до цієї папки. Очистіть весь згенерований код та коментарі та замініть їх наступним:

```cshtml
@model DateTime?
@if (Model == null)
{
    @:Unknown
}
else
{
    if (ViewData.ModelMetadata.IsNullableValueType)
    {
        @:@(Model.Value.ToString("d"))
    }
    else
    {
        @:@(((DateTime)Model).ToString("d"))
    }
}
```
Зверніть увагу, що директива @model, яка суворо типізує представлення, використовує малу літеру m. Під час посилання на призначене значення моделі в Razor використовується велика літера M. У цьому прикладі визначення моделі може мати значення null. Якщо значення моделі, передане у представлення, дорівнює null, шаблон відображає слово Unknown. В іншому випадку дата відображається у форматі Short Date, використовуючи властивість Value типу, що допускає значення null, або саму модель.
Для перегляду роботи цього шаблону додамо в RazorSyntax.cshtml.

```cshtml
@Html.DisplayForModel();
```
Також змінемо HomeControler

```cs
        public async Task<IActionResult> RazorSyntaxAsync()
        {
            Car car = new()
            {
                Id = 1,
                MakeId = 1,
                Color = "Blue",
                PetName = "Snoopy",
                DateBuilt = DateTime.Now
            };
            return View(car);
        }
```
Як бачите спацьовує шаблон визначений для типу DateTime.

## Шаблон відображення Car

Створіть новий каталог з назвою Cars  у каталозі Views та додайте каталог з назвою DisplayTemplates у каталозі Cars. Додайте нове представлення з назвою Car.cshtml до цієї папки. Очистіть весь згенерований код і коментарі та замініть їх наступним кодом, який відображає сутність Car:

```html
@model AutoLot.Models.Entities.Car
<dl class="row">
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.Id)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.Id)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.MakeId)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.MakeNavigation.Name)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.Color)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.Color)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.PetName)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.PetName)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.Price)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.Price)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.DateBuilt)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.DateBuilt)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.IsDrivable)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.IsDrivable)
    </dd>
</dl>
```
HTML-допоміжний метод DisplayNameFor() відображає назву властивості, якщо тільки властивість не оформлена атрибутом Display(Name='') або DisplayName(''), і в цьому випадку використовується значення відображення.
Для того шоб перевірити вигляд треба додати контролер з дією і представлення для дії. Але з початку додадмо в GlobalUsings.cs

```cs
global using Microsoft.AspNetCore.Mvc;
global using AutoLot.Models.Entities;
```
Додамо контролер з дією.

```cs
namespace AutoLot.Mvc.Controllers;

public class CarsController : Controller
{
    public IActionResult Templates()
    {
        Car car = new()
        {
            Id = 1,
            MakeId = 2,
            Price = "100",
            Color = "Red",
            PetName = "Chery",
            DateBuilt = DateTime.Now,
            MakeNavigation = new Make { Id = 2, Name = "VW" }
        };        
        return View(car);
    }
}
```
Додамо перегляд в папці Views\Cars  

```html
@model AutoLot.Models.Entities.Car

@Html.DisplayForModel()
```
Запустимо застосунок і перейдемо https://localhost:5001/Cars/Templates

## Шаблон відображення Car з кольором

Скопіюйте представлення Car.cshtml до іншого представлення з назвою CarWithColors.cshtml у каталозі Cars\DisplayTemplates. Різниця полягає в тому, що цей шаблон змінює колір тексту Color на основі значення властивості Color моделі. Оновіть тег \<dd\> нового шаблону для кольору наступним чином:

```html
        <dd class="col-sm-10" style="color:@Model.Color">
            @Html.DisplayFor(model => model.Color)
        </dd>
```
Щоб використовувати цей шаблон замість шаблону Car.cshtml, викличте функцію DisplayForModel() з іменем шаблону (зверніть увагу, що правила розташування все ще застосовуються).

```html
@Html.DisplayForModel("CarWithColors")
```

## Каталог EditorTemplates.

Папка EditorTemplates працює так само, як і папка DisplayTemplates, за винятком того, що шаблони використовуються для редагування.

## Шаблон редагування Car
Створіть новий каталог з назвою EditorTemplates у каталозі Views\Cars. Додайте нове представлення з назвою Car.cshtml до цієї папки. Очистіть весь згенерований код і коментарі та замініть їх наступним кодом, який представляє розмітку для редагування сутності Car:

```html
@model Car
<div asp-validation-summary="All" class="text-danger"></div>
<div>
    <label asp-for="MakeId" class="col-form-label"></label>
    <select asp-for="MakeId" class="form-control" asp-items="@ViewBag.LookupValues"></select>
    <span asp-validation-for="MakeId" class="text-danger"></span>
</div>
<div>
    <label asp-for="Color" class="col-form-label"></label>
    <input asp-for="Color" class="form-control"/>
    <span asp-validation-for="Color" class="text-danger"></span>
</div>
<div>
    <label asp-for="PetName" class="col-form-label"></label>
    <input asp-for="PetName" class="form-control" />
    <span asp-validation-for="PetName" class="text-danger"></span>
</div>
<div>
    <label asp-for="Price" class="col-form-label"></label>
    <input asp-for="Price" class="form-control"/>
    <span asp-validation-for="Price" class="text-danger"></span>
</div>
<div>
    <label asp-for="DateBuilt" class="col-form-label"></label>
    <input asp-for="DateBuilt" class="form-control"/>
    <span asp-validation-for="DateBuilt" class="text-danger"></span>
</div>
<div>
    <label asp-for="IsDrivable" class="col-form-label"></label>
    <input asp-for="IsDrivable" />
    <span asp-validation-for="IsDrivable" class="text-danger"></span>
</div>
```
Шаблон редактора використовує кілька допоміжних функцій тегів (asp-for, asp-items, asp-validation-for та asp-validation-summary). Вони будуть розглянуті пізніше в цій главі.

Шаблони редакторів викликаються за допомогою HTML-хелперів EditorFor()/EditorForModel(). Як і шаблони відображення, ці методи шукатимуть представлення з назвою типу, що відображається (наприклад, Car.cshtml), або за назвою представлення, переданою в метод.

```html
@Html.EditorForModel()
```

## Ізоляція CSS для перегляду

Наприклад, шаблон за замовчуванням створює файл _Layout.cshtml.css разом із файлом _Layout.cshtml. CSS, що міститься у файлі _Layout.cshtml.css та будь-який CSS у файлі, що стосується певного представлення (наприклад, Index.cshtml.css), об'єднуються у файл з назвою {assembly_name}.styles.css. Це завантажується у відображений контент, який надсилається до браузера. Якщо ви переглянете файл site.css у каталозі wwwroot\css, то помітите, що він дуже малий. Більшу частину стандартного CSS було перенесено до файлу _Layout.cshtml.css у шаблонах ASP.NET Core 6.
Далі клацніть правою кнопкою миші на каталозі \Views\Home та виберіть Add ➤ New Item, перейдіть до ASP.NET Core/Web/Content у лівій панелі, виберіть Style Sheet  та назвіть її Index.cshtml.css. Оновіть вміст до наступного:

```css
h1 {
    background-color: blue;
}
```
Коли ви запустите програму, ви побачите, що фон тексту привітання на головній сторінці має синій фон. Тепер перейдіть на сторінку Privacy, натиснувши на посилання в меню, і ви побачите, що заголовок не має синього фону.
Коли ви переглянете вихідні коди в інструментах розробника, ви побачите файл site.css (у папці css) та AutoLot.Mvc.Styles.css, у кореневому каталозі, який поєднує CSS з представлень _Layout.cshtml.css та Index.cshtml.css.
Відкрийте об'єднаний файл, і ви побачите, що всі селектори CSS оздоблені атрибутами. Кожен файл додає згенерований атрибут селектору до CSS, який він визначає. Наприклад, CSS для файлу Index.cshtml.css наведено тут:

```css
/* _content/AutoLot.Mvc/Views/Home/Index.cshtml.rz.scp.css */
h1[b-7wgwza0zra] {
    background-color: blue;
}
```
Ці атрибути використовуються для позначення елементів, що містяться в області дії цього CSS-файлу. Якщо ви переглянете елементи та перейдете до тегу \<h1\> з перегляду Index, ви побачите атрибут у згенерованому HTML:

```html
<h1 b-7wgwza0zra="" class="display-4">Welcome</h1>
```
Ця функція дозволяє дуже детально контролювати CSS на вашому сайті, водночас підтримуючи загальний CSS у файлі site.css.

Одне важливе застереження полягає в тому, що файл {assembly_name}.styles.css генерується лише під час запуску в режимі розробки або під час публікації сайту. Якщо ви хочете змінити середовище (наприклад, через файл launchSettings.json) під час налагодження, вам потрібно ввімкнути використання статичних веб-ресурсів, як-от цей (у файлі Program.cs):

```cs
//Enable CSS isolation in a non-deployed session
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseWebRoot("wwwroot");
    builder.WebHost.UseStaticWebAssets();
}
```

## Макети

MVC підтримує макети, які поділяються між переглядами, щоб надати сторінкам сайту узгоджений вигляд і відчуття. Перейдіть до папки Views\Shared і відкрийте файл _Layout.cshtml. Це повноцінний HTML-файл із тегами \<head\> та \<body\>. Цей файл є основою, на якій відображаються інші представлення. Макети містять навігацію та будь-яку розмітку заголовка та/або нижнього колонтитула, що дозволяє зберігати подання невеликими, простими та сфокусованими.
Прокрутіть файл униз, доки не побачите наступний рядок коду Razor:

```cshtml
@RenderBody()
```
Цей рядок вказує сторінці макета, де відображати вигляд. Тепер прокрутіть униз до рядка безпосередньо перед закриваючим тегом \</body\>. Наступний рядок створює новий розділ для макета та робить його необов'язковим:

```cshtml
@await RenderSectionAsync('scripts', required: false)
```

Секції також можна позначити як обов'язкові, передавши значення true як другий параметр. Їх також можна відображати синхронно, ось так:

```cs
@RenderSection('Header',true)
```
Будь-який код та/або розмітка в блоці @section файлу представлення не буде відображатися за допомогою виклику @RenderBody(), а буде відображатися в місці визначення розділу макета. Наприклад, припустимо, що у вас є представлення з такою реалізацією section:

```html
@section Scripts {
    <script src="~/lib/jquery-validation/dist/jquery.validate.js"></script>
}
```
Код з представлення відображатиметься в макеті замість визначення розділу. Якщо макет має це визначення:

```html
<script src='~/lib/jquery/dist/jquery.min.js'></script>
<script src='~/lib/bootstrap/dist/js/bootstrap.bundle.min.js'></script>
<script src='~/js/site.js' asp-append-version='true'></script>
@await RenderSectionAsync('Scripts', required: false)
```
потім додається розділ представлення, в результаті чого до браузера надсилається така розмітка:

```html
<script src='~/lib/jquery/dist/jquery.min.js'></script>
<script src='~/lib/bootstrap/dist/js/bootstrap.bundle.min.js'></script>
<script src='~/js/site.js' asp-append-version='true'></script>
<script src='~/lib/jquery-validation/dist/jquery.validate.js'></script>
```
В ASP.NET Core є @IgnoreBody() та @IgnoreSection(). Ці методи, розміщені в макеті, відповідно не відображатимуть тіло представлення або певний розділ. Вони дозволяють вмикати або вимикати функції представлення з макета на основі умовної логіки, такої як рівні безпеки. Атрибут asp-append-version – це допоміжний елемент тегів, який буде розглянуто пізніше в цій главі.

## Визначення макета для представлення

Щоб указати макет для перегляду, додайте такий блок Razor у верхню частину перегляду:

```cshtml
@{
    Layout = '_MyCustomLayout';
}
```
Якщо представлення не має спеціально визначеного макета, воно шукатиме файл _ViewStart.cshtml у тому ж каталозі, що й представлення. Якщо такого макета немає в тому самому каталозі, виконується пошук у кожному каталозі вище поточного розташування, доки не буде знайдено такий макет, використовуючи перший знайдений файл _ViewStart.cshtml (незалежно від того, чи визначено макет). Якщо такого не знайдено, макет не використовується, по суті, відображається лише часткове подання.

## Часткові представлення

Часткові представлення корисні для інкапсуляції інтерфейсу користувача, що допомагає зменшити повторюваний код та/або розмітку. Часткове представлення не використовує макет і вставляється в інше представлення або відтворюється за допомогою компонента представлення (розглянуто далі в цій главі).
Щоб відобразити вигляд як частковий вигляд з методу дії контролера, використовуйте метод PartialView() замість методу View(). Наприклад, оновіть метод дії HomeController Index() наступним чином:

```cs
        public async Task<IActionResult> Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
        {
            DealerInfo? vm = dealerMonitor.CurrentValue;
            return PartialView(vm);
        }
```
Коли ви запустите програму, ви побачите вміст представлення, але не макет (меню, заголовок, нижній колонтитул тощо). Обов’язково поверніть метод Index() назад до використання методу View(). 
Часткові представлення також можна відобразити з представлення за допомогою допоміжного тегу <partial>, що буде продемонстровано в наступному розділі.

## Розділення макета на часткові

Іноді файли макетів можуть стати великими та громіздкими. Одним з методів керування цим є розділення макета на набір сфокусованих частин. Вам також потрібно буде оновити CSS та JavaScript, на які посилаються, до поточних версій, оскільки клієнтські бібліотеки, що постачаються з шаблонами, зазвичай мають одну-дві менші версії. У наступному розділі буде створено частини макету, бібліотеки CSS та JavaScript будуть оновлені пізніше в цьому розділі.

### Створення частин
Створіть нову папку під назвою Partials у папці Shared. Створіть три порожні представлення з іменами _Head.cshtml, _JavaScriptFiles.cshtml та _Menu.cshtml.

### Частина Head
Виріжте вміст макета, що знаходиться між тегами \<head\>\</head\>, та вставте його у файл _Head.cshtml.

```html
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - AutoLot.Mvc</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/AutoLot.Mvc.styles.css" asp-append-version="true" />
```
У _Layout.cshtml замініть видалену розмітку викликом для відображення нового часткового коду:

```html
<head>
    <partial name="Partials/_Head" />
</head>
```
Тег \<partial\> – це ще один приклад допоміжного елемента тегів. Атрибут name – це назва часткового елемента, шлях до якого починається з поточного каталогу представлення (без розширення .cshtml), у цьому випадку це Views\Shared.

### Частина Menu

Для частини Menu виріжте всю розмітку між тегами <header></header> та вставте її у файл _Menu.cshtml. Оновіть _Layout для часткового відображення меню.

```html
    <header>
        <partial name="Partials/_Menu" />
    </header>
```

### Частина JavaScript-файлів

Останній крок на цьому етапі — вирізати теги \<script\> для файлівJavaScript та вставити їх у частку JavaScriptFiles. Обов’язково залиште тег RenderSection на місці. Ось фрагмент JavaScriptFiles:

```html
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>
```
Ось поточна розмітка для файлу _Layout.cshtml:

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <partial name="Partials/_Head" />
</head>
<body>
    <header>
        <partial name="Partials/_Menu" />
    </header>
    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody()
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; 2025 - AutoLot.Mvc - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </div>
    </footer>
    <partial name="Partials/_JavaScriptFiles" />
    @await RenderSectionAsync("Scripts", required: false)

</body>
</html>

```
Нижній колонтитул використовує допоміжні атрибути тегу прив'язки (asp-area/asp-controller/asp-action), які будуть розглянуті пізніше в цій главі.

# Надсилання даних до представлень

Існує кілька способів надсилання даних до представлення. Коли представлення мають сувору типізацію, дані можна надсилати під час відображення представлень (або з методу дії, або через допоміжний елемент тегу \<partial\>).

Перш ніж продовжити, додайте такі оператори до файлу GlobalUsings.cs:

```cs
global using AutoLot.Mvc.Models;
global using AutoLot.Models.Entities;
global using AutoLot.Models.Entities.Base;
global using AutoLot.Mvc.Controllers;
global using AutoLot.Services.DataServices.Interfaces;
global using Microsoft.AspNetCore.Http.Features;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.AspNetCore.Mvc.Rendering;
global using Microsoft.AspNetCore.Mvc.Routing;
global using Microsoft.AspNetCore.Mvc.ViewComponents;
global using Microsoft.AspNetCore.Razor.TagHelpers;
global using Microsoft.Extensions.DependencyInjection.Extensions;
global using System.Diagnostics;
```

## Строго типізовані подання та моделі представлень

Коли модель або ViewModel передається в метод view, значення присвоюється властивості @model строго типізованого view, як показано тут (зверніть увагу на малу літеру m):

```cshtml
@model IEnumerable<Order>
```
@model встановлює тип для представлення, а потім до нього можна отримати доступ за допомогою команди @Model Razor, ось так (зверніть увагу на велику літеру M):

```cshtml
@foreach (var item in Model)
{
  //Do something interesting here
}
```
Якщо ви пам'ятаєте, представлення RazorSyntax.cshtml мало сувору типізацію типу Car. Оновіть метод дії до наступного, щоб використовувати ICarDataService для отримання запису Car (переконайтеся, що для параметра UseApi у файлі appsettings.Development.json встановлено значення false):

```cs
        public async Task<IActionResult> RazorSyntaxAsync([FromServices] ICarDataService dataService )
        {
            var car = await dataService.FindAsync(6);
            return View(car);
        }
```
Після передачі екземпляра до представлення, додайте наступний код у кінець представлення:

```cshtml
<hr/>
The Car named @Model.PetName is a <span style=" color : @Model.Color ">@Model.Color</span> @Model.MakeNavigation.Name

```

Під час визначення типу для представлення директива @model використовує малу літеру m. Під час посилання на екземпляр використовуйте велику літеру M. Зверніть увагу, що код Razor використовується в рядку з необробленим текстом, як значення стилю та в тезі <span>, демонструючи деяку універсальність використання Razor в HTML.

Підчас тестування цього прикладу може виникнути помилка. Тоді треба перевірити Метод в класі DalDataServiceBase

```cs
    public async Task<TEntity?> FindAsync(int id) =>
        MainRepo.Find(id);
```
Це потрібно аби отримати MakeNavigation а не null.

Значення моделі також можна передати в часткове представлення за допомогою допоміжного тегу \<partial\>, як показано тут. Цей приклад буде протестовано пізніше:

```cshtml
@model IEnumerable<Car>
@{
    ViewData["Title"] = "Index";
}
<h1>Vehicle Inventory</h1>
<partial name="Partials/_CarListPartial" model="@Model"/>
```
_CarListPartial отримає модель і рендерить її.

Як інший приклад, нагадаємо, що метод дії Index() класу HomeController передає налаштований DealerInfo до представлення:

```cs
        public async Task<IActionResult> Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
        {
            DealerInfo? vm = dealerMonitor.CurrentValue;
            return View(vm);
        }
```

Щоб скористатися цією інформацією, оновіть файл Index.cshtml до наступного:

```cshtml
@model AutoLot.Services.ViewModels.DealerInfo
@{
    ViewData["Title"] = "Home";
}

<div class="text-center">
    <h1 class="display-4">Welcome to @Model.DealerName</h1>
    <p class="lead">Located in @Model.City, @Model.State</p>
</div>
```

## ViewBag, ViewData та TempData

Об'єкти ViewBag, ViewData та TempData – це механізми для надсилання невеликих обсягів даних у представлення. У таблиці наведено три механізми передачі даних від контролера до представлення, крім передачі моделі в представлення за допомогою методів View()/PartialView(). Їх також можна використовувати для передачі даних між представленнями та методами дій контролера.

Додаткові способи надсилання даних до представлення

|Об'єкт транспортування даних|Опис використання|
|----------------------------|-----------------|
|TempData|Це короткочасний об'єкт, який працює лише під час поточного та наступного запитів. Зазвичай використовується під час перенаправлення на інший метод дії.|
|ViewData|Словник, який дозволяє зберігати значення в парах «ім'я-значення» (наприклад, ViewData["Title"] = "My Page").|
|ViewBag|Динамічна обгортка для словника ViewData (наприклад, ViewBag.Title = "My Page").|

Як ViewBag, так і ViewData вказують на один і той самий об'єкт; вони просто надають різні способи доступу до даних. Давайте ще раз розглянемо файл _HeadPartial.cshtml, який ви створили раніше з оригінального файлу _Layout.cshtml:

```cshtml
<title>@ViewData["Title"] - AutoLot.Mvc</title>
```
Ви помітите, що атрибут \<title\> використовує ViewData для встановлення значення. Оскільки ViewData є конструкцією Razor, вона починається з символу @. Щоб побачити це в дії, оновіть представлення RazorSyntax.cshtml наступним кодом:

```cshtml
@model AutoLot.Models.Entities.Car
@{
    ViewData["Title"] = "RazorSyntax";
}
<h1>Razor Syntax</h1>
```
Тепер, коли ви запустите програму та перейдете за адресою https://localhost:5001/Home/RazorSyntax, ви побачите заголовок «Razor Syntax – AutoLot.Mvc» у вкладці браузера.

## Вставка даних

Будь-що з колекції IServiceCollection можна вставити у представлення. Додайте наступний код у початок файлу _Layout.cshtml, який вставляє IWebHostEnvironment:

```cshtml
@inject IWebHostEnvironment _env
```
Далі оновіть нижній колонтитул, щоб відобразити середовище, в якому зараз працює програма:

```html
            &copy; 2025 - AutoLot.Mvc - @_env.EnvironmentName <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
```

# Керування бібліотеками на стороні клієнта

Важливою частиною створення користувацького інтерфейсу для веб-застосунку є керування клієнтськими бібліотеками CSS та JavaScript. У шаблоні за замовчуванням встановлено jQuery, jQuery Validation та jQuery Validation Unobtrusive. Однак, ці бібліотеки, ймовірно, вже застарілі, коли ви створюєте новий проєкт, через швидкість оновлень бібліотек CSS та JavaScript. Оновлення їх вручну може бути виснажливим та трудомістким завданням.
Проект LibraryManager тепер є частиною Visual Studio, а також доступний як глобальний інструмент .NET. LibraryManager використовує простий JSON-файл для завантаження інструментів CSS та JavaScript з CDNJS.com, UNPKG.com, JSDeliver або файлової системи. Бібліотеки, на які посилаються, можна налаштувати для завантаження на вимогу або з кожною збіркою проєкту.

## Встановлення менеджера бібліотек як глобального інструменту .NET

Менеджер бібліотек тепер вбудовано у Visual Studio. Щоб встановити його як глобальний інструмент .NET, введіть таку команду:

```console
dotnet tool install --global Microsoft.Web.LibraryManager.Cli
```

## Додавання клієнтських бібліотек до AutoLot.Mvc

Шаблон за замовчуванням завантажив бібліотеки CSS та JavaScript у каталог wwwroot\lib. Щоб перейти до керування бібліотеками за допомогою LibraryManager, почніть з видалення всього каталогу lib та всіх каталогів і файлів, які він містить.

## Додайте файл libman.json

Файл libman.json контролює, що встановлюється, з яких джерел та куди встановлюються файли.

### В Visual Studio
Якщо ви використовуєте Visual Studio, клацніть правою кнопкою миші на проекті AutoLot.Mvc і виберіть «Manage Client-Side Libraries». Це додає файл libman.json до кореневого каталогу проекту. У Visual Studio також є опція для інтеграції Library Manager з процесом MSBuild. Якщо ви не встановили пакет NuGet Microsoft.Web.LibraryManager.Build перед додаванням файлу libman.json (що ми зробили під час створення проектів), клацніть правою кнопкою миші на файлі libman.json і виберіть «Enable restore on build». Це запропонує вам дозволити додавання пакета NuGet до проєкту. Дозвольте встановлення пакета, якщо буде запропоновано. Якщо спочатку встановити пакет, проєкт автоматично налаштовується на відновлення під час збірки.
Початковий файл визначає постачальника за замовчуванням як cdnjs, який посилається на api.cdnjs.com:

```json
{
  "version": "3.0",
  "defaultProvider": "cdnjs",
  "libraries": []
}
```
### В Командний рядок

Створіть новий файл libman.json за допомогою такої команди (це встановить постачальника за замовчуванням на cdnjs.com):

```console
libman init --default-provider cdnjs
```
Якщо ви не встановлюватимете постачальника за замовчуванням за допомогою командного рядка, вам буде запропоновано вибрати його, за замовчуванням буде встановлено cdnjs.

## Оновіть файл libman.json

Під час пошуку бібліотек для встановлення, CDNJS.com має зручний, зрозумілий API. Усі доступні бібліотеки за такою URL-адресою:

https://api.cdnjs.com/libraries?output=human

Коли ви знайдете бібліотеку, яку хочете встановити, оновіть URL-адресу, додавши її назву зі списку, щоб побачити всі версії та файли для кожної версії. Наприклад, щоб побачити все, що доступно для jQuery, введіть наступне:

https://api.cdnjs.com/libraries/jquery?output=human

Після того, як ви визначитеся з версією та файлами для встановлення, додайте назву бібліотеки (і версію), місце призначення (зазвичай wwwroot/lib/<назва бібліотеки>) та файли для завантаження. Наприклад, щоб завантажити jQuery, введіть наступне в масив JSON libraries:

```json
{
  'library': 'jquery@3.6.0',
  'destination': 'wwwroot/lib/jquery',
  'files': [ 'jquery.js']
},
```
Після додавання всіх файлів, необхідних для цієї програми, повний файл libman.json показано тут:

```json
{
  "version": "1.0",
  "defaultProvider": "cdnjs",
  "defaultDestination": "wwwroot/lib",
  "libraries": [
      {
        "library": "jquery@3.7.1",
        "destination": "wwwroot/lib/jquery",
        "files": [
          "jquery.js",
          "jquery.min.js",
          "jquery.min.map"
        ]
      },
      {
        "library": "jquery-validate@1.21.0",
        "destination": "wwwroot/lib/jquery-validation",
        "files": [ "jquery.validate.js", "jquery.validate.min.js", "additional-methods.js", "additional-methods.min.js" ]
      },
      {
        "library": "jquery-validation-unobtrusive@4.0.0",
        "destination": "wwwroot/lib/jquery-validation-unobtrusive",
        "files": [ "jquery.validate.unobtrusive.js", "jquery.validate.unobtrusive.min.js" ]
      },
      {
        "library": "bootstrap@5.3.8",
        "destination": "wwwroot/lib/bootstrap",
        "files": [
          "css/bootstrap.css",
          "css/bootstrap.css.map",
          "css/bootstrap.min.css",
          "css/bootstrap.min.css.map",
          "js/bootstrap.bundle.js",
          "js/bootstrap.bundle.js.map",
          "js/bootstrap.bundle.min.js",
          "js/bootstrap.bundle.min.js.map",
          "js/bootstrap.js",
          "js/bootstrap.js.map",
          "js/bootstrap.min.js",
          "js/bootstrap.min.js.map"
        ]
      },
    {
      "library": "font-awesome@5.15.4",
      "destination": "wwwroot/lib/font-awesome/",
      "files": [
        "js/all.js",
        "js/all.min.js",
        "css/all.css",
        "css/all.min.css",
        "sprites/brands.svg",
        "sprites/regular.svg",
        "sprites/solid.svg",
        "webfonts/fa-brands-400.eot",
        "webfonts/fa-brands-400.svg",
        "webfonts/fa-brands-400.ttf",
        "webfonts/fa-brands-400.woff",
        "webfonts/fa-brands-400.woff2",
        "webfonts/fa-regular-400.eot",
        "webfonts/fa-regular-400.svg",
        "webfonts/fa-regular-400.ttf",
        "webfonts/fa-regular-400.woff",
        "webfonts/fa-regular-400.woff2",
        "webfonts/fa-solid-900.eot",
        "webfonts/fa-solid-900.svg",
        "webfonts/fa-solid-900.ttf",
        "webfonts/fa-solid-900.woff",
        "webfonts/fa-solid-900.woff2"
      ]
    }
  ]
}
```
Після збереження файлу (у Visual Studio) файли будуть завантажені в папку wwwroot\lib проекту.
Якщо ви запускаєте програму з командного рядка, введіть таку команду, щоб перезавантажити всі файли:

```console
libman restore
```
Доступні додаткові параметри командного рядка. Введіть libman -h, щоб переглянути всі параметри.

## Оновлення посилань на JavaScript та CSS

Розташування багатьох файлів JavaScript та CSS змінилося з переходом до Library Manager. Bootstrap та jQuery завантажувалися з папки \dist в оригінальному коді scaffold. Оновлення до використання libman.json також додало Font Awesome до програми.
Розташування файлів Bootstrap потрібно оновити на ~/lib/bootstrap/css замість ~/lib/boostrap/dist/css, а також додати Font Awesome. Оновіть файл _Head.cshtml до наступного.

```html
<link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css" />
<link rel="stylesheet" href="~/lib/font-awesome/css/all.css" />
```
Далі оновіть _JavaScriptFiles.cshtml, щоб видалити \dist з розташування jQuery та Bootstrap.

```html
<script src="~/lib/jquery/jquery.min.js"></script>
<script src="~/lib/bootstrap/js/bootstrap.bundle.min.js"></script>
```
Остання зміна полягає в оновленні розташування jquery.validation у частковому перегляді _ValidationScriptsPartial.cshtml.

```html
<script src="~/lib/jquery-validation/jquery.validate.min.js"></script>
```

# Об'єднання в пакети та мініфікація.

Менеджер бібліотек легко та ефективно встановлює клієнтські бібліотеки у ваш проект. Два додаткові міркування щодо створення вебзастосунків з клієнтськими бібліотеками - це пакетування та мінімізація для покращення продуктивності.

## Пакетування (Bundling)

Веббраузери мають встановлене обмеження на кількість файлів, які вони дозволяють одночасно завантажувати з однієї кінцевої точки. Це може бути проблематично, якщо ви використовуєте методи розробки SOLID для файлів JavaScript та CSS, розділяючи пов'язаний код та стилі на менші, зручніші у підтримці файли. Це забезпечує кращий досвід розробки, але може знизити продуктивність програми, поки файли чекають своєї черги на завантаження. Bundling — це просто об’єднання файлів разом, щоб запобігти їх блокуванню під час очікування ліміту завантажень браузера.

## Mініфікація (Minification)

Також, для покращення продуктивності, процес мініфікації змінює файли CSS та JavaScript, щоб зробити їх меншими. Непотрібні пробіли та закінчення рядків видаляються, а назви, що не є ключовими словами, скорочуються. Хоча це робить файл майже нечитабельним для людини, функціональність не змінюється, а розмір можна значно зменшити. Це, у свою чергу, пришвидшує процес завантаження, тим самим покращуючи продуктивність програми.

## WebOptimizer

Існує багато інструментів розробки, які можуть об'єднувати та мініфікувати файли як частину процесу збірки. 
WebOptimizer — це пакет з відкритим кодом, який використовує проміжне програмне забезпечення для пакетування та мініфікації файлів як частини конвеєра ASP.NET Core. Це гарантує, що об’єднані та мініфіковані файли точно відображають необроблені файли. Вони не лише точні, але й кешуються як частина процесу, що значно зменшує кількість зчитувань диска для запитів сторінок. WebOptimizer використовує техніку кешування, щоб забезпечити постійну актуальність файлів. До кожного налаштованого посилання на файл додається хеш файлу, ось так:

https://localhost:5001/AutoLot.Mvc.styles.css?v=9Ozsx8PuquAgpHzcVo7FiIxEbbs

Коли файл змінюється, хеш змінюється, і браузер перезавантажує файл, оскільки URL-адреса змінилася. Так само функціонує допоміжний засіб тегу asp-append-version. Допоміжні засоби тегів розглядаються далі в цьому розділі. 
Ви вже додали пакет NuGet LiberShark.WebOptimizer.Core під час створення проектів. Тепер настав час його використовувати.

## Додавання та налаштування WebOptimizer

Першим кроком є ​​додавання WebOptimizer до конвеєра. Відкрийте файл Program.cs у проекті AutoLot.Mvc та додайте наступний рядок (безпосередньо перед викликом app.UseStaticFiles():

```cs
app.UseWebOptimizer();
```
Наступний крок – налаштувати, що мінімізувати та об’єднувати в пакет. Зазвичай, під час розробки застосунку, ви хочете використовувати незв'язані/не мініфіковані версії файлів, а зв'язані/мінфіковані версії використовувати для проміжної розробки та продакшен. У файлі Program.cs додайте наступний блок коду перед var app = builder.Build():

```cs
if (builder.Environment.IsDevelopment())
{
  builder.Services.AddWebOptimizer(false,false);
}
else
{
  builder.Services.AddWebOptimizer(options =>
  {
    //Configuration goes here
  });
}
```
У попередньому блоці коду, якщо середовищем є Development, усі пакетування та мініфікації вимкнено:

```cs
  builder.Services.AddWebOptimizer(false,false);
```
WebOptimizer можна налаштувати на мінімізацію всіх CSS-файлів та JavaScript-файлів, знайдених у каталозі wwwroot:

```cs
options.MinifyCssFiles(); //Minifies all CSS files
options.MinifyJsFiles(); //Minifies all JS files
```
WebOptimizer також можна налаштувати для мінімізації певного файлу або всіх файлів у певному каталозі:

```cs
options.MinifyCssFiles('css/site.css'); //Minifies the site.css file
options.MinifyCssFiles('lib/**/*.css'); //Minifies all CSS files under the wwwroot/lib directory
options.MinifyJsFiles(“js/site.js”); //Minifies the site.js file
options.MinifyJsFiles(“lib/**/*.js”); //Minifies all JavaScript files under the wwwroot/lib directory
```
Бібліотеки з відкритим кодом вже мають мініфіковані версії, завантажені через Library Manager, тому мініфікувати потрібно лише файли, специфічні для проекту, включаючи згенерований CSS-файл, якщо ви використовуєте ізоляцію CSS. Оновіть блок if до наступного:

```cs

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddWebOptimizer(false, false);

    //builder.Services.AddWebOptimizer(options =>
    //{
    //    options.MinifyCssFiles("AutoLot.Mvc.styles.css");
    //    options.MinifyCssFiles("/css/site.css");
    //    options.MinifyJsFiles("/js/site.js");
    //});

}
else
{
    builder.Services.AddWebOptimizer(options =>
    {
        options.MinifyCssFiles("AutoLot.Mvc.styles.css");
        options.MinifyCssFiles("cs/site.cs");
        options.MinifyJsFiles("js/site.js");
    });
}
```
У варіанті розробки код налаштовано для коментування/розкоментування різних опцій, щоб ви могли відтворити продакшен без переходу на нього.

WebOptimizer також підтримує пакетування для CSS та JavaScript файлів. Перший приклад створює JavaScript-пакет за допомогою глобалізації файлів, а другий створює JavaScript-пакет зі списком конкретних імен.

```cs
        options.AddJavaScriptBundle("js/validations/validationCode.js", "js/validations/**/*.js");
        options.AddJavaScriptBundle("js/validations/validationCode.js", "js/validations/validators.js", "js/validations/errorFormatting.js");
```
Щоб пакетувати CSS-файли, використовуйте метод AddCssBundle().

Важливо зазначити, що мініфіковані та об’єднані файли насправді не знаходяться на диску, а розміщуються в кеші. Також важливо зазначити, що мінімізовані файли мають ту саму назву (site.js) і не мають звичайної літери min в назві (site.min.js).

    Під час оновлення ваших представлень для додавання посилань на пакетні файли, Visual Studio повідомить про те, що пакетний файл не існує. Не хвилюйтеся, він все одно відтворюватиметься з кешу.

## Оновлення _ViewImports.cshtml

Останній крок – додати до системи помічники тегів WebOptimizer. Додайте наступний рядок у кінець файлу _ViewImports.cshtml:

```cshtml
@addTagHelper *, WebOptimizer.Core
```

# Контролери

Перш ніж продовжити огляд аспектів інтерфейсу користувача застосунків у стилі MVC, час оновити HomeController для використання маршрутизації атрибутів та додати додаткові контролери для основного застосунку. AutoLot.Mvc має базовий контролер та два похідні контролери.
Додаток AutoLot.Mvc використовуватиме лише маршрутизацію за атрибутами, тому маршрут за замовчуванням, доданий шаблоном MVC, можна видалити. Щоб внести цю зміну, закоментуйте метод MapControllerRoute() у Program.cs та додайте метод MapControllers():

```cs
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
```

# HomeController

Коли маршрут за замовчуванням було закоментовано, механізм маршрутизації більше не може знайти HomeController або його методи дій. Це виправляється додаванням атрибутів Route на контролері та/або методах дій. Коли атрибут Route додається на рівні контролера, усі методи дій успадковують той самий шаблон маршруту. Методи дій можуть додавати до шаблону або скидати його. Оновіть оголошення контролера, щоб додати маршрут контролера за замовчуванням з таким атрибутом Route:

```cs
[Route("[controller]/[action]")]
public class HomeController : Controller
{
    //...
}
```
Хоча стандартна таблиця маршрутизації дозволяє вказувати значення за замовчуванням для токенів маршруту, маршрутизація за атрибутами цього не дозволяє. Це може спричинити проблеми з маршрутизацією, якщо в запиті не вказано повний маршрут. Щоб створити еквівалент маршруту за замовчуванням, додайте такі три атрибути Route до методу Index() ось так:

```cs
    [Route("/")]
    [Route("/[controller]")]
    [Route("/[controller]/[action]")]
    public IActionResult Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
    {
        ...
    }
```
Нагадаємо, що маршрут для методу дії можна скинути, розпочавши шаблон зі склесної риски (/). Перший атрибут ([Route('/')]) відповідає запитам, які не вказують жодних параметрів маршруту (наприклад, www.myShop.com). Другий атрибут ([Route('/[controller]')]) відповідає запитам, які вказують лише частину маршруту, що стосується контролера (www.myShop.com/home), а останній ([Route('/[controller]/[action]')]) відповідає маршрутам з контролером та методом дії (www.myShop.com/home/index). Ці три шаблони маршрутів, що використовуються разом, відтворюють маршрут за замовчуванням, який був у вихідному шаблоні програми. 

    Навіть якщо маршрут контролера вказує шаблон контролера/дії, його необхідно повторити в методі Index(), оскільки інші атрибути Route в методі дії скидають маршрут контролера.

Нарешті, позначте методи Index() та Privacy() атрибутом HttpGet. Метод Error() не позначено атрибутом HttpGet, оскільки він має виконуватися щоразу, коли виникає помилка, незалежно від HTTP-методу запиту:

```cs
    [HttpGet]
    [Route("/")]
    [Route("/[controller]")]
    [Route("/[controller]/[action]")]
    public async Task<IActionResult> Index([FromServices] IOptionsMonitor<DealerInfo> dealerMonitor)
    {
        DealerInfo? vm = dealerMonitor.CurrentValue;
        return View(vm);
    }
     
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }
```

# BaseCrudController

BaseCrudController містить шаблонні методи створення, читання, оновлення та видалення (CRUD), які використовуватимуть похідні контролери. Почніть збирати цей базовий контролер, додавши новий каталог під назвою Base в каталог Controllers, клацніть правою кнопкою миші на каталозі Base, виберіть Add ➤ Controller, потім виберіть MVC Controller – Empty та назвіть контролер BaseCrudController. Зробіть його публічним, абстрактним та узагальненим для класу, типізованим для BaseEntity (для методів CRUD) та будь-якого класу (для логування). Призначте класу маршрут [controller]/[action] :

```cs
namespace AutoLot.Mvc.Controllers.Base;

[Route("[controller]/[action]")]
public abstract class BaseCrudController<TEntity,TController> : Controller 
    where TEntity : BaseEntity, new()
    where TController : class
{
    public IActionResult Index()
    {
        return View();
    }
}
```
Додайте наступний оператор до файлу GlobalUsings.cs:

```cs
global using AutoLot.Mvc.Controllers.Base;
```
Додайте конструктор, який приймає екземпляр IAppLogging\<TController\> та IDataService\<TEntity\> та призначає їх полям рівня класу:

```cs
    protected readonly IAppLogging<TController> AppLoggingInstance;
    protected readonly IDataServiceBase<TEntity> MainDataService;

    protected BaseCrudController(IAppLogging<TController> appLogging, IDataServiceBase<TEntity> mainDataService)
    {
        AppLoggingInstance = appLogging;
        MainDataService = mainDataService;
    }
```
Далі додайте protected метод для отримання однієї сутності за допомогою введеного сервісу даних:

```cs
    protected async Task<TEntity?> GetOneEntityAsync(int id) =>
        await MainDataService.FindAsync(id);
```

Додайте захищений абстрактний метод, який повертатиме SelectList, що використовується для заповнення випадаючих списків для значень пошуку, таких як Make для класу Car:

```cs
protected abstract Task<SelectList> GetLookupValuesAsync();
```

## Метод IndexAsync

Метод IndexAsync() служить методом за замовчуванням для похідних контролерів, і тому для нього потрібно визначити маршрути [controller] та [controller]/[action], як і для методу Index() на HomeController. Зверніть увагу, що застосунок може мати лише один базовий маршрут ([Route(“/”)]), тому обов’язково додайте лише два наступні атрибути Route разом із атрибутом [HttpGet]:

```cs
    [HttpGet]
    [Route("/[controller]")]
    [Route("/[controller]/[action]")]
    public IActionResult Index()
    {
        return View();
    }
```
Усі методи дій на базовому контролері позначені як віртуальні, щоб похідні контролери могли їх перевизначати та використовувати асинхронні методи для підтримки викликів служби даних:

```cs
    public virtual Task<IActionResult> IndexAsync()
    {
        //...
    }
```
Нагадаємо, що під час маршрутизації до методу дії async із суфіксом Async цей суфікс відкидається. Маршрут для методу IndexAsync() — /[controller]/Index, а не /[controller]/IndexAsync.
Метод IndexAsync() (і майбутнє представлення) використовується для відображення списку всіх сутностей. Цей список отримується за допомогою методу GetAllAsync() у класі служби даних, а потім передається до представлення (зверніть увагу на зміну використання тіла виразу):

```cs
public virtual async Task<IActionResult> IndexAsync()
    => View(await MainDataService.GetAllAsync());
```

## TestBaseCrudController

Роблячі функціонал базового контролера будемо його тестувати. Створимо похідний клас TestBaseCrudController в папці Controllers:

```cs
namespace AutoLot.Mvc.Controllers;

public class TestBaseCrudController : BaseCrudController<Car, TestBaseCrudController>
{
    public TestBaseCrudController(IAppLogging<TestBaseCrudController> appLogging, ICarDataService mainDataService) : base(appLogging, mainDataService)
    {
    }

    protected override Task<SelectList> GetLookupValuesAsync()
    {
        throw new NotImplementedException();
    }
}
```
Додамо папку TestBaseCrud в папці Views і додамо перегляд Index.cshtml :

```cshtml
@model IEnumerable<Car>

@{
    ViewData["Title"] = "Test of BaseCrudControler";
}

<h6>Test of method Index</h6>

@foreach (var item in Model)
{
    <h6>
        @item.Id&emsp;
        @item.MakeId&emsp;
        @item.PetName&emsp;
        @item.Color&emsp;
        @Convert.ToBase64String(item.TimeStamp)
    </h6>
}
```
Змінемо файл _Menu.cshtml папки головного шаблону добавни новє посилання

```html
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-controller="TestBaseCrud" asp-action="Index">TestBaseCrud</a>
                </li>
```
Запустимо додадток і перевіримо посилання. Як очикувалось відпрацьовує метод IndexAsync базового контролера.

## Метод DetailsAsync

Продовжимо реалізовувати базовий контролер. Метод DetailsAsync() використовується для відображення окремого запису під час виклику з HTTP-GET методом. Маршрут контролера розширюється необов'язковим значенням ідентифікатора. Необов'язкові токени маршруту позначені знаком питання в токені та представляють параметри, що допускають значення null, у методі дії. Додайте наступний метод дії:

```cs
    [HttpGet("{id?}")]
    public virtual async Task<IActionResult> DetailsAsync(int? id)
    {
        //...
    }
```
Тіло методу повертає BadRequest(), якщо параметр має значення null, інакше викликає метод GetOneEntityAsync(). Якщо сутність не повертається з бази даних/API, метод повертає результат NotFound(). Якщо сутність знайдено, вона повертається до представлення:

```cs
    [HttpGet("{id?}")]
    public virtual async Task<IActionResult> DetailsAsync(int? id)
    {
        if (!id.HasValue)
        {
            return BadRequest();
        }

        var entity = await GetOneEntityAsync(id.Value);
        if(entity == null)
        {
            return NotFound();
        }
        return View(entity);
    }
```

Методи BadRequest() та NotFound() – це два допоміжні методи ControllerBase, розглянуті в почаьковій главі про ASP.NET Core. Це зручні методи для повернення кодів стану HTTP 400 та 404 клієнту.

    Просте повернення кодів помилок 400 та 404 до браузера може не отримати найкращого сприйняття від користувачів. Для робочих програм вам знадобиться надсилати більш дружнє повідомлення. Це буде продемонстровано пізніше.

Протестуємо метод. Додамо наступний шаблон \Views\TestBaseCrud\DisplayTemplates\Car.cshtml

```cshtml
@model Car
<dl class="row">
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.Id)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.Id)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.MakeId)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.MakeNavigation.Name)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.Color)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.Color)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.PetName)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.PetName)
    </dd>
    <dt class="col-sm-2">
        @Html.DisplayNameFor(model => model.TimeStamp)
    </dt>
    <dd class="col-sm-10">
        @Html.DisplayFor(model => model.TimeStamp)
    </dd>
</dl>
```

Додамо перегляд \Views\TestBaseCrud\Details.cshtml

```cshtml
@model Car

@{
    ViewData["Title"] = "Test of method Details";
}

<h6>@ViewData["Title"]</h6>

@Html.DisplayForModel()
```
Додамов в \Views\TestBaseCrud\Index.cshtml

```html
<hr />
<h6>Test Details</h6>
<a href="https://localhost:5001/TestBaseCrud/Details/5">https://localhost:5001/TestBaseCrud/Details/5</a>
<br />
<a href="https://localhost:5001/TestBaseCrud/Details/">https://localhost:5001/TestBaseCrud/Details/</a>
<br />
<a href="https://localhost:5001/TestBaseCrud/Details/215">https://localhost:5001/TestBaseCrud/Details/215</a>

```
Таким чином перевірили всі варіанти роботи методу.


## Методи дії CreateAsync
Процес створення використовує два методи дії: один є кінцевою точкою для HTTP-запитів Get, а інший — отримує HTTP-запити Post. 

### Метод Get

Метод дії get CreateAsync() додає пов’язаний запис SelectList до словника ViewData та повертає представлення.

```cs
    [HttpGet]
    public virtual async Task<IActionResult> CreateAsync()
    {
        ViewData["LookupValues"] = await GetLookupValuesAsync();
        return View();
    }
```

Для тестування цього методу створимо декілька переглядів.  Додамо папку Views\TestBaseCrud\EditorTemplates і в ній Car.cshtml

```cshtml
@model Car
@{
    var timeStamp = (Model != null) ? Convert.ToBase64String(Model.TimeStamp) : " ";
}

<div asp-validation-summary="All" class="text-danger"></div>
<div>
    <label asp-for="MakeId" class="col-form-label"></label>
    <select asp-for="MakeId" class="form-control" asp-items="@ViewBag.LookupValues"></select>
    <span asp-validation-for="MakeId" class="text-danger"></span>
</div>
<div>
    <label asp-for="Color" class="col-form-label"></label>
    <input asp-for="Color" class="form-control" />
    <span asp-validation-for="Color" class="text-danger"></span>
</div>
<div>
    <label asp-for="PetName" class="col-form-label"></label>
    <input asp-for="PetName" class="form-control" />
    <span asp-validation-for="PetName" class="text-danger"></span>
</div>
<div>
    <label asp-for="IsDrivable" class="col-form-label"></label>
    <input asp-for="IsDrivable" />
    <span asp-validation-for="IsDrivable" class="text-danger"></span>
</div>
<div>
    <label asp-for="TimeStamp" class="col-form-label"></label>
    <input asp-for="TimeStamp" value=@timeStamp />
    <span asp-validation-for="TimeStamp" class="text-danger"></span>
</div>
```
Долі в папку Додамо папку Views\TestBaseCrud\Create.cshtml

```cshtml
@model Car

@{
    ViewData["Title"] = "Test Create Get Method";
}

<h6>@ViewData["Title"]</h6>
<hr/>
<form asp-controller="TestBaseCrud" asp-action="Create">
    <div class="row">
        <div class="col-md-4">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            @Html.EditorForModel()
        </div>
    </div>
    <div class="d-flex flex-row mt-3">
        <button type="submit" class="btn btn-success">Create</button>
    </div>
</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial"/>
}
```
Тепер в тестовому контролері потрібно реалізувати метод GetLookupValuesAsync(). Для цього оновимо конструктор і додамо зміну рівня класу

```cs
    private readonly IMakeDataService _lookupDataService;
    public TestBaseCrudController(IAppLogging<TestBaseCrudController> appLogging, ICarDataService mainDataService, IMakeDataService makeDataService) : base(appLogging, mainDataService)
    {
        _lookupDataService = makeDataService;
    }
```

Реалізуємо метод.

```cs
    protected override async Task<SelectList> GetLookupValuesAsync()
        => new SelectList( await _lookupDataService.GetAllAsync(),nameof(Make.Id),nameof(Make.Name));

```

Додамо в  Views\TestBaseCrud\Index.cshtml

```html
<hr />
<h6>Test Create</h6>
<a href="https://localhost:5001/TestBaseCrud/Create">https://localhost:5001/TestBaseCrud/Create</a>
```

### Метод Post

Метод дії HTTP post CreateAsync() використовує неявне зв'язування моделі для створення екземпляра сутності типу TEntity зі значень форми. Код наведено тут з наступним поясненням:

```cs
    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> CreateAsync(TEntity entity)
    {
        if (ModelState.IsValid)
        {
            await MainDataService.AddAsync(entity);
            return RedirectToAction(nameof(DetailsAsync).RemoveAsyncSuffix(),new {id = entity.Id})
        }
        ViewData["LookupValues"] = await GetLookupValuesAsync();
        return View();
    }
```

Атрибут HttpPost позначає це як кінцеву точку застосунку для маршруту Controller/Create, коли запит є Post. Атрибут ValidateAntiForgeryToken вимагає та перевіряє приховане вхідне значення для __RequestVerificationToken із запиту. Додавання токена до представлень буде розглянуто пізніше.
Параметр entity неявно пов'язаний з вхідними даними запиту. Якщо ModelState є валідна, сутність додається до бази даних, а потім користувача перенаправляють до методу дії IndexAsync(). Це шаблон Post-Redirect-Get. Користувач надіслав метод Post (CreateAsync()), а потім перенаправляється до методу Get (DetailsAsync()). Це запобігає повторному надсиланню Post браузером, якщо користувач оновлює сторінку. Метод nameof() використовується в команді перенаправлення замість простих рядків, щоб отримати перевірку компілятором. Однак, пам’ятайте, що маршрутизація видаляє суфікс Async з методів дій, тому для використання методу nameof() цей суфікс необхідно видалити. Метод розширення RemoveAsyncSuffix() написаний саме для цього.
Як останнє зауваження щодо перенаправлення, дія DetailsAsync() вимагає ідентифікатора параметра маршруту. Якщо ModelState не валідна, SelectList додається до ViewData, а надіслана сутність надсилається назад до представлення, щоб користувач міг виправити проблеми. ModelState також неявно надсилається до представлення, щоб можна було відобразити будь-які помилки.

## Методи дій EditAsync

У процесі редагування також використовуються два методи дій: метод HTTP get для повернення об'єкта для редагування та метод HTTP post для надсилання значень оновленого запису.

### Метод Get

Метод дії HTTP-дієї get EditAsync() отримує окрему сутність за ідентифікатором, використовуючи обгортку сервісу, та надсилає її до представлення.

```cs
 [HttpGet("{id?}")]
 public virtual async Task<IActionResult> EditAsync(int? id)
 {
     if (!id.HasValue)
     {
         ViewData["Error"] = "Bad Request";
         return View();
     }
     var entity = await GetOneEntityAsync(id.Value);
     if (entity == null)
     {
         ViewData["Error"] = "Not Found";
         return View();
     }
     ViewData["LookupValues"] = await GetLookupValuesAsync();
     return View(entity);
 }
```
Маршрут має параметр необов'язковий параметр ідентифікатора, який потім передається в метод. Метод використовує допоміжний метод GetOneEntityAsync() для отримання запису. Якщо ідентифікатор має значення null або запис неможливо знайти, метод додає повідомлення про помилку до ViewData та повертає представлення без будь-яких даних. В іншому випадку він створює SelectList значень пошуку, додає його до словника ViewData та відтворює представлення.

Для тестування додамо Views\TestBaseCrud\Edit.cshtml

```cshtml
@model Car

@{
    ViewData["Title"] = "Test of method Edit ";
}
@if (ViewData["Error"] != null)
{
    <div class="alert alert-danger" role="alert">
        @ViewData["Error"]
    </div>
}
else
{
    <h6>@ViewData["Title"]</h6>
    <hr />
    <form asp-controller="TestBaseCrud" asp-action="Edit">
        <div class="row">
            <div class="col-md-4">
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                @Html.EditorForModel()
            </div>
        </div>
        <div class="d-flex flex-row mt-3">
            <button type="submit" class="btn btn-success">Edit</button>
        </div>
    </form>

    @section Scripts {
        <partial name="_ValidationScriptsPartial" />
    }
}

```

Додамо в  Views\TestBaseCrud\Index.cshtml

```html
<hr />
<h6>Test Edit</h6>
<a href="https://localhost:5001/TestBaseCrud/Edit/1">https://localhost:5001/TestBaseCrud/Edit/1</a>
<br />
<a href="https://localhost:5001/TestBaseCrud/Edit/">https://localhost:5001/TestBaseCrud/Edit/</a>
<br />
<a href="https://localhost:5001/TestBaseCrud/Edit/303">https://localhost:5001/TestBaseCrud/Edit/303</a>
```
### Метод Post

Метод дії HTTP post EditAsync() подібний до методу HTTP post CreateAsync(), за винятком того, що він викликає UpdateAsync() для класу служб даних:

```cs
    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> EditAsync(int? id, TEntity entity)
    {
        if (id != entity.Id)
        {
            ViewData["Error"] = "Bad Request";
            return View();
        }
        if (ModelState.IsValid)
        {
            await MainDataService.UpdateAsync(entity);
            return RedirectToAction(nameof(DetailsAsync).RemoveAsyncSuffix(), new { id });
        }
        ViewData["LookupValues"] = await GetLookupValuesAsync();
        return View(entity);
    }
```
Метод HTTP post EditAsync() приймає один обов'язковий параметр маршруту для id. Якщо це не збігається з ідентифікатором відновленої сутності, клієнту надсилається повідомлення про помилку та порожній результат перегляду. Якщо ModelState є валідною, сутність оновлюється, а потім користувача перенаправляють до методу дії DetailsAsync(), передаючи параметр id як значення маршруту. Оскільки ім'я змінної збігається з ім'ям параметра, використовується скорочений метод створення анонімного об'єкта (new {id}). Також використовується шаблон Post-Redirect-Get.
Якщо ModelState неваліднє, SelectList додається до ViewData, а надіслана сутність надсилається назад до представлення. ModelState також неявно надсилається до представлення, щоб можна було відобразити будь-які помилки.

## Методи дії DeleteAsync

Процес видалення також використовує два методи дії.

### Метод Delete Get

Метод дії HTTP DeleteAsync() функціонує так само, як і метод дії EditAsync():

```cs
    [HttpGet("{id?}")]
    public virtual async Task<IActionResult> DeleteAsync(int? id)
    {
        if (!id.HasValue)
        {
            ViewData["Error"] = "Bad Request";
            return View();
        }

        var entity = await GetOneEntityAsync(id.Value);
        if (entity == null)
        {
            ViewData["Error"] = "Not Found";
            return View();
        }

        return View(entity);
    }
```

Для тестування додамо Views\TestBaseCrud\Views\Delete.cshtml

```cshtml
@model Car

@{
    ViewData["Title"] = "Test of method Delete";
}
@if (ViewData["Error"] != null)
{
    <div class="alert alert-danger" role="alert">
        @ViewData["Error"]
    </div>
}
else
{
    <h3>Delete @Model.PetName</h3>

    <h3>Are you sure you want to delete this car?</h3>
    <div>
        <div asp-validation-summary="All" class="text-danger"></div>
        @Html.DisplayForModel()
        <form asp-action="Delete">
            <input type="hidden" asp-for="Id"/>
            <input type="hidden" asp-for="TimeStamp"/>
            <button type="submit" class="btn btn-danger">
                Delete 
            </button>
        </form>
    </div>
}
```
Додамо в  Views\TestBaseCrud\Index.cshtml

```html
<hr />
<h6>Test Delete</h6>
<a href="https://localhost:5001/TestBaseCrud/Delete/1">https://localhost:5001/TestBaseCrud/Delete/1</a>
```

### Метод Delete Post

Для обробки методу дії Delete() відправляється запит HTTP post Delete використовуючи лише Id та TimeStamp, а потім надсилає цей екземпляр до служби даних:

```cs
    [HttpPost("{id}")]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> DeleteAsync(int id, TEntity entity)
    {
        if (id != entity.Id)
        {
            ViewData["Error"] = "Bad Request";
            return View();
        }

        try
        {
            await MainDataService.DeleteAsync(entity);
            return RedirectToAction(nameof(IndexAsync).RemoveAsyncSuffix());
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            ModelState.AddModelError(string.Empty, ex.Message);
            MainDataService.ResetChangeTracker();
            entity = await GetOneEntityAsync(id);
            return View(entity);
        }
    }
```
Метод HTTP Post Delete() оптимізовано таким чином, щоб надсилати лише значення, необхідні EF Core для видалення запису. Якщо видалення пройде успішно, це не буде проблемою, оскільки користувача буде перенаправлено на сторінку індексу. Однак, якщо видалення з якоїсь причини не вдається (наприклад, через каскадну проблему), решта властивостей (і будь-які необхідні властивості навігації) будуть null. Це спричиняє дві проблеми. По-перше, ModelState, ймовірно, буде недійсним. По-друге, користувач не зможе підтвердити, що він видаляє правильний запис, оскільки у поданні нічого не відображатиметься.
Щоб вирішити цю проблему, ModelState очищається від будь-яких помилок, потім додається помилка рівня моделі, використовуючи повідомлення про помилку винятку як вміст. Наступний рядок викликає службу даних для скидання ChangeTracker. Це необхідно лише під час безпосереднього використання коду AutoLot.Dal. Метод DbSet\<TEntity\>.Find() (метод сервера, який викликається під час виконання методу GetOneEntityAsync()) спочатку перевірить, чи вже відстежується сутність з таким самим первинним ключем. Якщо так, цей екземпляр повертається. Проблема в цьому робочому процесі полягає в тому, що екземпляр, який наразі відстежується, має заповнені лише властивості Id та TimeStamp. Виклик ResetChangeTracker() змушує похідний DbContext припинити відстеження всіх сутностей (у цьому випадку це лише сутність, яку потрібно видалити). Після очищення ChangeTracker метод Find() повернеться до бази даних, щоб отримати повну сутність.
Під час використання AutoLot.Api як серверної частини це не є проблемою, оскільки API не зберігає стан між викликами.

# Використання BindProperty

Замість отримання параметра сутності в кожному з методів HTTP post, контролери MVC також підтримують використання BindProperty. Це публічна властивість на контролері, позначена атрибутом BindProperty, ось так:

```cs
    [BindProperty]
    public TEntity Entity { get; set; }

```


З урахуванням цього, сигнатури методів post видаляють параметр entity і та використовують властивість рівня контролера. Прив'язка моделі все ще неявно, і ModelState працює так само. Якщо ви хочете спробувати це, додайте властивість Entity до контролера та оновіть підписи записів до наступного (показуючи лише верхній рядок):

```cs
public abstract class BaseCrudWithBindingPropertyController<TEntity, TController> : Controller
    where TEntity : BaseEntity, new()
    where TController : class
{
    protected readonly IAppLogging<TController> AppLoggingInstance;
    protected readonly IDataServiceBase<TEntity> MainDataService;

    protected BaseCrudWithBindingPropertyController(IAppLogging<TController> appLogging, IDataServiceBase<TEntity> mainDataService)
    {
        AppLoggingInstance = appLogging;
        MainDataService = mainDataService;
    }

    [BindProperty]
    public TEntity Entity { get; set; }

    [ActionName("Create")]
    public virtual async Task<IActionResult> CreatePostAsync()
    {
        //...
    }

    [ActionName("Edit")]
    public virtual async Task<IActionResult> EditPostAsync(int id)
    {
        //...
    }

    [ActionName("Delete")]
    public virtual async Task<IActionResult> DeletePostAsync(int id)

    //...
}
```
Окрім видалення параметра сутності, зверніть увагу на зміни в назвах методів дії. Це пов'язано з тим, що коли метод CreateAsync() post втратив параметр, він має точно таку ж назву та сігнатуру, як і метод CreateAsync() get. Хоча механізм маршрутизації з цим влаштований (оскільки один є post, а інший — get), C# — ні. Рішення полягає в тому, щоб змінити назву методу, а потім додати атрибут ActionName. Атрибут ActionName має пріоритет над назвою методу, тому механізм маршрутизації зіставляє метод CreatePostAsync() назад з кінцевою точкою Create.
Останнє оновлення, яке потрібно внести до контролера, — це перейменування entity на Entity у кожному з методів, оскільки властивість пишеться з великої літери, тоді як параметри були нижчими.

Для тестування цого класу достатньо змінити базовий клас для контротера TestBaseCrudController.

```cs
public class TestBaseCrudController : BaseCrudWithBindingPropertyController<Car, TestBaseCrudController>
{
    //...
}
```
Все робить як і раніше.


# Контролер CarsController

CarsController успадковується від BaseCrudController, отримуючи базовий маршрут та стандартні операції CRUD. Він також має строгий типізаційний зв'язок із сутністю Car та ICarDataService. Змінимо CarsController. Зробіть його публічним та успадковуйте від BaseCrudController, реалізуйте абстрактний метод та додайте необхідний конструктор (обов'язково додайте async до перевизначення):

```cs
namespace AutoLot.Mvc.Controllers;

public class CarsController : BaseCrudController<Car, CarsController>
{
    public CarsController(IAppLogging<CarsController> appLogging, ICarDataService mainDataService) : base(appLogging, mainDataService)
    {
    }

    //...
   
    protected override async Task<SelectList> GetLookupValuesAsync()
    {
        throw new NotImplementedException();
    }
}
```
Сутності Car мають пов'язану інформацію про марку, і під час редагування або додавання записів Car, представлення матимуть випадаючі елементи керування, щоб користувач міг вибрати марку за назвою. Щоб побудувати Make SelectList, контролеру потрібен екземпляр IMakeDataService. Оновіть конструктор, щоб він врахував це та присвоїв його полю рівня класу:

```cs
    private readonly IMakeDataService _lookupDataService;
    public CarsController(IAppLogging<CarsController> appLogging, 
        ICarDataService mainDataService,
        IMakeDataService makeDataService) : base(appLogging, mainDataService)
    {
        _lookupDataService = makeDataService;
    }
```
Після налаштування служби даних оновіть метод GetLookupValuesAsync() для створення списку вибору записів Make:

```cs
    protected override async Task<SelectList> GetLookupValuesAsync()
        => new SelectList(await _lookupDataService.GetAllAsync(), nameof(Make.Id), nameof(Make.Name));
    
```
Потрібен ще один метод дії, який використовується для отримання списку автомобілів певної марки. Маршрут розширено атрибутом [HttpGet] для прийняття makeId та makeName. MakeId використовується для вибору записів, makeName додається до ViewBag для відображення у представленні:

```cs
    [HttpGet("{makeId}/{makeName}")]
    public async Task<IActionResult> ByMakeAsync(int makeId, string makeName)
    {
        ViewBag.MakeName = makeName;
        return View(await ((ICarDataService)MainDataService).GetAllByMakeIdAsync(makeId));
    }
```
На цьому контролери для основної частини програми завершено.

# Areas (Області)
Areas – це розділені розділи веб-застосунку ASP.NET Core (на основі сторінок MVC або Razor), які використовуються для організації пов'язаної функціональності. Вони мають власний простір імен для маршрутизації, а також окрему структуру каталогів для переглядів та сторінок Razor. Коли область додається до проєкту, файли каркасу створюються в каталозі Areas\\[AreaName]. У каталозі [AreaName] створено чотири початкові каталоги:

    Controllers
    Models
    Data
    Views (MVC app) or Pages (Razor pages app)

Контролери для області повинні мати атрибут [Area("[AreaName]")] (наприклад, [Area("Admin")]), щоб ідентифікувати їх як контролер області. Хоча контролери для області зазвичай знаходяться в каталозі Areas\\[AreaName]\Controllers, технічно вони можуть знаходитися будь-де, якщо їм належним чином присвоєно атрибути. Єдине обмеження стосується представлень (views) та сторінок Razor. За замовчуванням, представлення мають знаходитися в одному з наступних каталогів (сторінки та області Razor детально розглянуті в наступній главі):

    /Areas/<Area-Name>/Views/<Controller-Name>/<Action-Name>.cshtml
    /Areas/<Area-Name>/Views/Shared/<Action-Name>.cshtml
    /Views/Shared/<Action-Name>.cshtml

Зверніть увагу, що структура каталогів така ж, як і в головному каталозі Views. Представлення контролера розміщуються в папці з назвою контролера (без суфікса Controller) або в каталозі Shared у дереві каталогів області. Якщо перегляд не знайдено в одному з цих двох місць, пошук виконується в папці Shared у головному каталозі Views. У головному каталозі Views\ControllerName ніколи не виконується пошук представлень контролера області, навіть якщо головна програма має контролер з такою ж назвою.
Для застосунку AutoLot.Mvc ми додамо areas для керування записами Make. Щоб додати область, клацніть правою кнопкою миші на проекті AutoLot.Mvc, виберіть Add ➤ New Scaffolded Item, потім виберіть MVC Area та назвіть її Admin. Видаліть каталоги Models та Data, оскільки вони не потрібні.

## Маршрутизація area

Коли область (Area) створюється, до проекту додається файл ScaffoldingReadMe.txt з інструкціями щодо додавання маршруту контролера для області (Area). Якщо ви використовуєте традиційну маршрутизацію, запропонований маршрут близький до того, що вам потрібно додати. Кращий спосіб додати маршрут Area – це використовувати метод MapAreaControllerRoute(), ось так (обов’язково розмістіть маршрути Area перед будь-якими маршрутами, що не належать до Area):

```cs
app.MapAreaControllerRoute(
    name: "AdminArea",
    areaName:"Admin",
    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
);
```
Оскільки AutoLot.Mvc використовує маршрутизацію за атрибутами, метод MapAreaControllerRoute() не потрібен.

## Контролер area MakesController

Як згадувалося раніше, контролери області повинні мати застосований атрибут [Area]. Для демонстрації додайте новий контролер з назвою MakesController до каталогу Areas\Admin\Controllers. Початковий код точно такий самий, як початкове налаштування CarsController, за винятком того, що він використовує версію служб даних Make та має застосований атрибут Area:

```cs

namespace AutoLot.Mvc.Areas.Admin.Controllers;

[Area("Admin")]
[Route("Admin/[controler]/[action]")]
public class MakesController : BaseCrudController<Make, MakesController>
{
    public MakesController(IAppLogging<MakesController> appLogging, IMakeDataService mainDataService) : base(appLogging, mainDataService)
    {
    }
}

```
GetLookupValuesAsync() не є обов'язковою, тому налаштуйте її на повернення , що є кращим способом повернення пустого списку з асинхронного методу.

```cs
    protected override async Task<SelectList> GetLookupValuesAsync()
        => await Task.FromResult<SelectList>(new(default, "", ""));
```

Як останню зміну, перевизначте метод дії IndexAsync(), щоб можна було застосувати атрибути маршруту за замовчуванням, все ще використовуючи код базового класу:

```cs
    [Route("/Admin")]
    [Route("/Admin/[controller]")]
    [Route("/Admin/[controller]/[action]")]
    public override async Task<IActionResult> IndexAsync()
    {
        return await base.IndexAsync();
    }
```
Щоб перевірити, чи працює маршрутизація, додайте просте представлення для методу дії Index(). Спочатку додайте новий каталог з назвою Makes у папці Areas\Admin\Views, а в цій папці додайте нове порожнє представлення Razor з назвою Index.cshtml. Змініть його

```cs
@model IEnumerable<Make>

@Html.DisplayForModel()
```
Далі запустіть програму та перейдіть за адресою https://localhost:5001/Admin.

Щоб продемонструвати, що контролери Area можуть знаходитися поза структурою каталогів Areas, перемістіть MakesController до головної папки Controllers та ще раз перевірте маршрути. Ви побачите, що вони все ще працюють. Однак, якщо перемістити файл Index.cshtml куди завгодно, окрім Areas\Admin\Views\Makes, Areas\Admin\Views\Shared або Views\Shared, ви виявите, що виникне виняток InvalidOperationException через відсутність представлення. Перш ніж продовжити читання, переконайтеся, що контролер і вікно перегляду повернуто на свої місця.

## _ViewImports і _ViewStart

Файли _ViewImports.cshtml та _ViewStart.cshtml застосовуються до всіх представлень на одному рівні каталогу та нижче. Розташування цих файлів за замовчуванням знаходиться в головному каталозі \Views. За допомогою Areas ви можете скопіювати існуючі файли (або створити нові) в папку Areas\\[Назва_контролера]\Views або перемістити існуючі файли до кореневого каталогу проекту, щоб вони також охоплювали всі подання Area. Для цього проекту перемістіть файли _ViewImports.cshtml та _ViewStart.cshtml до кореневого каталогу проекту, щоб вони були застосовані до всього проекту.

# Tag Helpers (Помічники тегів)
Помічники тегів – це нова функція, представлена ​​в ASP.NET Core. Допоміжний елемент тегу — це розмітка (користувацький тег або атрибут стандартного тегу), яка представляє серверний код. Потім серверний код допомагає формувати відправляємий HTML-код. Вони значно покращують процес розробки та читабельність MVC-представлень. Якщо ви розробляєте за допомогою Visual Studio, є додаткова перевага IntelliSense для вбудованих помічників тегів. Помічники тегів значною мірою замінюють помічники Html для окремих тегів.

    Якщо ви не знайомі з Html-хелперами, це функції Razor, які генерують HTML. Html-хелпери, які не реплікуються теговими хелперами, будуть розглянуті пізніше в цьому розділі.

Наприклад, наступний HTML-хелпер створює мітку для властивості FullName клієнта: 

```cshtml
@Html.Label("PetName", "Pet Name:", new { @class = "customer" })
```
Це генерує наступний HTML-код:

```html
<label class="customer" for="PetName">Pet Name:</label>
```

Для розробника C# синтаксис допоміжного HTML-коду, ймовірно, добре зрозумілий. Але він не інтуїтивно зрозумілий, особливо для тих, хто працює з HTML/CSS/JavaScript, а не з C#.

Версія допоміжного тегу виглядає так:

```html
<label class="customer" asp-for="PetName">Pet Name:</label>
```
Вони видають однаковий результат, але помічники тегів, завдяки своїй інтеграції в теги HTML, дозволяють вам залишатися «в розмітці».
Існує багато вбудованих помічників тегів, і вони призначені для використання замість відповідних помічників HTML. Однак не всі помічники HTML мають пов'язаний помічник тегів. У таблиці перелічено найчастіше використовувані помічники тегів, їхні відповідні помічники HTML та доступні атрибути. Вони будуть детально розглянуті в решті цього розділу.

|Tag Helper|HTML Helper|Доступні атрибути|
|----------|-----------|-----------------|
|Form|Html.BeginForm Html.BeginRouteForm Html.AntiForgeryToken|asp-route — для іменованих маршрутів (не можна використовувати з атрибутами контролера або дії). asp-antiforgery — чи слід додати захист від підробок (за замовчуванням true). asp-area—the name of the area. asp-controller—the name of the controller. asp-action — назва дії. asp-route-<ParameterName> — додає параметр до маршруту, наприклад, asp-route-id="1". asp-all-route-data — словник для додаткових значень маршруту.|
|Form Action(button or input type=image)|N/A|asp-route — для іменованих маршрутів (не можна використовувати з атрибутами контролера або дії). asp-antiforgery — чи слід додати захист від підробок (за замовчуванням true). asp-area—the name of the area. asp-controller—the name of the controller. asp-action — назва дії. asp-route-<ParameterName> — додає параметр до маршруту, наприклад, asp-route-id="1". asp-all-route-data — словник для додаткових значень маршруту.|
|Anchor|Html.ActionLink|asp-route — для іменованих маршрутів (не можна використовувати з атрибутами контролера або дії). asp-area — назва області. asp-controller — визначає контролер. asp-action — визначає дію. asp-protocol—HTTP or HTTPS. asp-fragment — фрагмент URL-адреси. asp-host—the host name. asp-route-<ParameterName> — додає параметр до маршруту, наприклад, asp-route-id="1". asp-all-route-data — словник для додаткових значень маршруту.|
|Input|Html.TextBox/TextBoxFor Html.Editor/EditorFor|asp-for — властивість моделі. Можна переміщатися по моделі (Customer.Address.AddressLine1) та використовувати вирази (asp-for='@localVariable'). Атрибути id та name генеруються автоматично. Будь-які атрибути data-val та type HTML5 генеруються автоматично. asp-format – рядок форматування, який використовується для форматування результату asp-for|
|TextArea|Html.TextAreaFor|asp-for — властивість моделі. Можна переміщатися по моделі (Customer.Address.Description) та використовувати вирази (asp-for='@localVariable'). Атрибути id та name генеруються автоматично. Будь-які атрибути data-val та type HTML5 генеруються автоматично.|
|Label|Html.LabelFor|asp-for — властивість моделі. Можна переміщатися по моделі (Customer.Address.AddressLine1) та використовувати вирази (asp-for='@localVariable'). Відображає значення атрибута Display, якщо він існує; інакше використовується назва властивості.|
|Partial|Html.Partial(Async)Html.RenderPartial(Async)|name — шлях та назва часткового представлення. for—model expression on current form to be the model in the partial. model — об'єкт, який буде моделлю в частковому перегляді. view-data — ViewData для часткової об'єкта. fallback-name — вигляд, який потрібно завантажити, якщо іменований вигляд не вдається знайти. optional — якщо optional, і перегляд не знайдено, часткова операція завершиться без виконання, а не викличе помилку.|
|Select|Html.DropDownListFor Html.ListBoxFor|asp-for — властивість моделі. Можна переміщатися по моделі (Customer.Address.AddressLine1) та використовувати вирази (asp-for='@localVariable'). asp-items — визначає елементи options. Автоматично генерує атрибут selected='selected'. Атрибути id та name генеруються автоматично. Будь-які атрибути data-val HTML5 генеруються автоматично.|
|Validation Message (span)|Html.ValidationMessageFor|asp-validation-for — властивість моделі. Можна переміщатися по моделі (Customer.Address.AddressLine1) та використовувати вирази (asp-for='@localVariable'). Додає атрибут data-valmsg-for до діапазону.|
|Validation Summary (div)|Html.ValidationSummaryFor|asp-validation-summary — виберає один із варіантів: All (Усі), ModelOnly (Тільки модель) або None (Жоден). Додає атрибут data-valmsg-summary до div.|
|Link|N/A|asp-append-version — додає хеш файлу як індикатор версії до імені файлу (як рядок запиту) для кешування. href — адреса джерела для доставки мережевої версії  контенту. asp-fallback-href — резервний файл, який використовуватиметься, якщо основний недоступний; зазвичай використовується з джерелами CDN. asp-fallback-href-include — глобальний список файлів, які потрібно включити під час резервного тестування. asp-fallback-href-exclude — глобальний список файлів, які потрібно виключити під час резервного тестування. asp-fallback-test-* — властивості, що використовуються під час резервного тесту. asp-suppress-fallback-integrity – вказує, чи слід проводити перевірку цілісності резервного файлу. За замовчуванням – true. asp-href-include – шаблон глобального файлу, що включає файли. asp-href-exclude – шаблон глобального файлу, що виключає файли.|
|Script|N/A|asp-append-version — додає хеш файлу як індикатор версії до імені файлу (як рядок запиту) для кешування. src — адреса версії джерела для мережі доставки контенту (CDN). asp-fallback-src — резервний файл, який використовуватиметься, якщо основний недоступний; зазвичай використовується з джерелами CDN. asp-fallback-src-include — глобальний список файлів, які потрібно включити під час резервного відновлення. asp-fallback-src-exclude — глобальний список файлів, які потрібно виключити під час резервного використання. asp-fallback-test — метод скрипта для використання в резервному тесті. asp-suppress-fallback-integrity — вказує, чи слід проводити перевірку цілісності резервного файлу. За замовчуванням — true. asp-src-include — глобальний шаблон файлів, які потрібно включити. asp-src-exclude — глобальний шаблон файлів, які потрібно виключити.|
|Image|N/A|asp-append-version — додає хеш файлу як індикатор версії до імені файлу (як рядок запиту) для очищення кешу.|
|Environment|N/A|names — єдина назва середовища хоста або список назв, розділених комами, для запуску візуалізації вмісту (ігнорує регістр). include — єдина назва середовища хоста або список назв, розділених комами, для запуску візуалізації вмісту (ігнорує регістр). exclude — єдина назва середовища хоста або список назв, розділених комами, для виключення з візуалізації вмісту (ігнорує регістр). Перевизначення та дублікати, знайдені в include або names|

## Увімкнення Tag Helpers
Помічники тегів мають бути ввімкнені у вашому проєкті, оскільки вони є функцією, що вимагається за бажанням. Файл _ViewImports.html зі стандартного шаблону вже містить наступний рядок для ввімкнення вбудованих у фреймворк ASP.NET Core помічників тегів:

```cshtml
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```
Це робить усі помічники тегів у збірці Microsoft.AspNetCore.Mvc.TagHelpers (яка містить усі вбудовані помічники тегів) доступними для всіх представлень на рівні каталогу файлу _ViewImports.cshtml або нижче.

## Form Tag Helper

Допоміжний елемент тегу <form> замінює HTML-допоміжні елементи Html.BeginForm та Html.BeginRouteForm. Наприклад, щоб створити форму, яка надсилає дані до версії HTTP Post дії EditAsync на CarsController з одним параметром (Id), використовуйте наступний код і розмітку:

```html
<form method="post" asp-controller="Cars" asp-action="Edit" asp-route-id="@Model.Id" >
<!-- Omitted for brevity -->
</form>
```
З точки зору виключно HTML, тег \<form\> працюватиме без допоміжних атрибутів тегу. Якщо жоден з атрибутів допоміжних функцій тегу відсутній, то це просто звичайна HTML-форма, і токен захисту від підробки потрібно додавати вручну. Однак, після додавання одного з допоміжних тегів, токен захисту від підробки додається до форми. Токен захисту від підробок можна вимкнути, додавши asp-antiforgery='false' до тегу форми.

## Форма Cars Create

Форма створення для сутності Car надсилається до методу дії Create контролера CarsController. Додайте нове пусте представлення Razor з назвою Create.cshtml у каталозі Views\Cars. Оновіть вигляд до наступного:

```html
@model Car

@{
    ViewData["Title"] = "Create";
}

<h1>Create a New Car</h1>
<hr/>
<div class="row">
    <div class="col-md-4">
        <form asp-controller="Cars" asp-action="Create">
        </form>
    </div>
</div>
```
Це не повний огляд, але його достатньо, щоб показати те, що ми розглянули досі, плюс допоміжний елемент тегу форми. Для порівняння, перший рядок суворо типізує вигляд до класу сутності Car. Блок Razor встановлює заголовок сторінки, специфічний для перегляду. Тег HTML <form> має атрибути asp-controller та asp-action, які виконуються на стороні сервера для формування тегу, а також додавання токена захисту від підробки. Тепер запустіть програму та перейдіть за адресою https://localhost:5001/Cars/Create.
Перевірка вихідного коду покаже, що форма має атрибут action на основі asp-контролера та asp-action, метод налаштовано на post, а __RequestVerificationToken було додано як прихований вхідний код форми.

```html
<form action="/Cars/Create" method="post">
        <input name="__RequestVerificationToken" type="hidden" value="CfDJ8CGu_LUjAd1DsAayMZvoMcWsYHW2dDQLZbksei2wmZW-9HLQUOG1Pis-Yl6AQa5FvuUe56Q8REyrGIkVrZRdbCQRXuFdpYdi9g_UQZBmssgytBVjlgYEgV81XvLUQANPTxCZmRtW6UrCkHb3SqQ8qq4">
</form>
```
Тепер оновіть тег <form>, щоб вимкнути токен захисту від підробки:

```cshtml
        <form asp-controller="Cars" asp-action="Create" asp-antiforgery="false">
        </form>
```
Якщо запустити програму та перевірити вихідний код сторінки, токена там більше немає. Якщо користувач спробує надіслати цю форму назад, запит не вдасться, оскільки метод CreateAsync() має атрибут [ValidateAntiForgeryToken]. Запит виглядає як міжсайтовий запит і блокується. Щоб вручну знову додати токен, змініть значення asp-antiforgery на true, повністю видаліть атрибут (значення за замовчуванням – true) або скористайтеся допоміжною функцією Html.AntiForgeryToken() для додавання токена:

```cs
        <form asp-controller="Cars" asp-action="Create" asp-antiforgery="false">
            @Html.AntiForgeryToken()
        </form>
```
Знову переглянувши згенерований HTML-код, ви побачите, що токен повернувся на місце. Рекомендований метод отримання токена у формі – використовувати налаштування за замовчуванням, повністю пропускаючи атрибут asp-antiforgery.

## Тег хелпер дії форми для Button/Image

Допоміжний тег дії форми використовується для кнопок та зображень, щоб змінити дію для форми, яка їх містить. Наприклад, наступна кнопка, додана до форми редагування, призведе до того, що запит на публікацію перейде до кінцевої точки Create поточного контролера:

```html
<form method="post">

    // ...

    <button type="submit" asp-controller="TestBaseCrud" asp-action="Create">Create</button>
</form>
```

## Тег хелпер Anchor

Тег хелпер <anchor> замінює Html.ActionLink. Він використовує багато тих самих тегів маршрутизації, що й допоміжний тег <form>. Наприклад в Views\Home\Index.cshtml , щоб створити посилання для представлень, використовуйте наступний код:

```html

     <a class="link-info" asp-area="" asp-controller="Home" asp-action="RazorSyntax">RazorSyntax</a>
    <br/>
    <a class="link-info" asp-area="" asp-controller="Cars" asp-action="Templates">Car templates</a>

```
Щоб додати елемент меню навігації для сторінки RazorSyntax, оновіть _Menu.cshtml до наступного, додавши новий елемент меню між елементами меню «Home» та «Privacy» (теги <li> навколо тегів <anchor> потрібні під час використання меню Bootstrap):

```html
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="RazorSyntax">RazorSyntax <i class="fas fa-cut"></i></a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
                </li>
```
Розмітка після тексту «Razor Syntax» — це значок FontAwesome, який прикрашає меню. 
Тег хелпер можна поєднати з моделлю представлення. Наприклад, використовуючи екземпляр Car у представлені RazorSyntax, наступний тег прив'язки перенаправляє до методу Details, передаючи Id як параметр маршруту:

```html
<<a asp-controller="TestBaseCrud" asp-action="Details" asp-route-id="@Model.Id">@Model.PetName</a>
```
## Тег хелпер Input

Допоміжний засіб тегів \<input\> є одним із найуніверсальніших допоміжних засобів тегів. Окрім автоматичної генерації атрибутів HTML id та name, а також будь-яких атрибутів перевірки даних HTML5, допоміжний засіб тегів створює відповідну розмітку HTML на основі типу даних цільової властивості. У таблиці перелічено тип HTML, створений на основі типу .NET властивості.

Типи HTML, згенеровані з типів .NET за допомогою допоміжного елемента тегів введення
|.NET Type|Згенерований тип HTML|
|---------|---------------------|
|Bool|type="checkbox"|
|String|type="text"|
|DateTime|type="datetime"|
|Byte, Int, Single, Double|type="number"|

Крім того, допоміжний елемент тегу \<input\> додасть атрибути типу HTML5 на основі анотацій даних. У таблиці наведено деякі з найпоширеніших анотацій та згенеровані атрибути типу HTML5.

Атрибути типу HTML5, згенеровані з анотацій даних .NET
|.NET Data Annotation|Згенерований атрибут типу HTML5|
|--------------------|-------------------------------|
|EmailAddress|type="email"|
|Url|type='url'|
|HiddenInput|type='hidden'|
|Phone|type='tel'|
|DataType(DataType.Password)|type='password'|
|DataType(DataType.Date)|type='date'|
|DataType(DataType.Time)|type='time'|

Шаблон редагування Car.cshtml містить теги \<input\> для властивостей PetName та Color. Як нагадування,, що
тут перелічені лише ці теги:

```cshtml
    <input asp-for="Color" class="form-control"/>
    <input asp-for="PetName" class="form-control" />
```
Допоміжний елемент тегу \<input\> додає атрибути name та id до відображеного тегу, існуюче значення властивості (якщо воно є) та атрибути перевірки HTML5. Обидва поля є обов'язковими та мають обмеження довжини рядка 50. Ось відображена розмітка для цих двох властивостей:

```html
<input class="form-control valid" type="text" data-val="true" data-val-length="The field Color must be a string with a maximum length of 50." data-val-length-max="50" data-val-required="The Color field is required." id="Color" maxlength="50" name="Color" value="Black" aria-describedby="Color-error" aria-invalid="false">
```
```html
<input class="form-control valid" type="text" data-val="true" data-val-length="The field Pet Name must be a string with a maximum length of 50." data-val-length-max="50" data-val-required="The Pet Name field is required." id="PetName" maxlength="50" name="PetName" value="Zippy" aria-describedby="PetName-error" aria-invalid="false">
```

## Тег хелпер TextArea

Допоміжний елемент тегу \<textarea\> автоматично додає атрибути id та name, а також будь-які теги перевірки HTML5, визначені для властивості. Наприклад, наступний рядок створює тег \<textarea\> для властивості Description:

```html
<textarea asp-for="Description"></textarea>
```

## Тег хелпер Select

Допоміжний інструмент тегу \<select\> створює вхідні теги \<select\> з властивості моделі та колекції. Як і у випадку з допоміжним елементом тегу \<input\>, id та name додаються до розмітки, а також будь-які атрибути data-val HTML5. Якщо значення властивості моделі збігається з одним зі значень елемента списку вибору, цей параметр додає вибраний атрибут до розмітки.
Наприклад, візьмемо модель, яка має властивість під назвою Country та SelectList під назвою Countries, зі списком, визначеним наступним чином:

```cs
public List<SelectListItem> Countries { get; } = new List<SelectListItem>
{
  new SelectListItem { Value = 'MX', Text = 'Mexico' },
  new SelectListItem { Value = 'CA', Text = 'Canada' },
  new SelectListItem { Value = 'US', Text = 'USA'  },
};
```
Наступна розмітка відобразить тег select з відповідними опціями:

```html
<select asp-for='Country' asp-items="@ViewBag.LookupValues"></select>
```
Якщо значення властивості Country встановлено на CA, у представленні буде виведено наступну повну розмітку:

```html
<select id="Country" name="Country">
  <option value="MX">Mexico</option>
  <option selected="selected" value="CA">Canada</option>
  <option value="US">USA</option>
</select>
```

## Тег хелпер Validation

Допоміжні засоби тегів повідомлення перевірки та підсумку перевірки точно повторюють допоміжні засоби HTML Html.ValidationMessageFor та Html.ValidationSummaryFor. Перший застосовується до HTML-проміжку для певної властивості моделі, а другий застосовується до тегу div і представляє всю модель. У зведенні перевірки є варіанти «All», «ModelOnly» (за винятком помилок у властивостях моделі) або «None».
Згадайте тег хелпери перевірки з EditorTemplate файлу Car.cshtml (показано жирним шрифтом):

```cshtml
@model Car
<div asp-validation-summary="All" class="text-danger"></div>
<div>
    <label asp-for="MakeId" class="col-form-label"></label>
    <select asp-for="MakeId" class="form-control" asp-items="@ViewBag.LookupValues"></select>
    <span asp-validation-for="MakeId" class="text-danger"></span>
</div>
<div>
    <label asp-for="Color" class="col-form-label"></label>
    <input asp-for="Color" class="form-control"/>
    <span asp-validation-for="Color" class="text-danger"></span>
</div>
```
Ці помічники відображатимуть помилки ModelState, отримані внаслідок зв'язування та перевірки.
Зайдіть за посиланням https://localhost:5001/TestBaseCrud/Edit/7 зробіль пустими поля і нажміть Edit.

## Тег хелпер Environment 

Допоміжний тег <environment> зазвичай використовується для умовного завантаження файлів JavaScript та CSS (або будь-якої розмітки, якщо на те пішло) на основі середовища, в якому працює сайт. Відкрийте фрагмент _Head.cshtml та оновіть розмітку до наступного:

```html
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1.0" />
<title>@ViewData["Title"] - AutoLot.Mvc</title>
<environment include="Development">
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.css" />
    <link rel="stylesheet" href="~/lib/font-awesome/css/all.css" />
</environment>
<environment exclude="Development">
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/lib/font-awesome/css/all.min.css" />
</environment>
<link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
<link rel="stylesheet" href="~/AutoLot.Mvc.styles.css" asp-append-version="true" />
```
Перший помічник тегу <environment> використовує атрибут include=”Development” для включення файлів, що містяться в середовищі, коли середовище встановлено на Development. У попередньому коді завантажуються немінімізовані версії Bootstrap та Font Awesome. Другий помічник тегів використовує exclude=”Development” для включення файлів, що містяться в середовищі, коли середовище не є Development, та завантажує мініфіковані версії. Останні два файли мінімізуються WebOptimizer, який вже налаштований на перевірку середовища, тому вони перелічені поза допоміжним елементом тегу \<environment\>.

Також оновіть _JavaScriptFiles.cshtml до наступного (зверніть увагу, що файли в розділі «Development» більше не мають розширень .min):

```html
<environment include="Development">
    <script src="~/lib/jquery/jquery.js"></script>
    <script src="~/lib/bootstrap/js/bootstrap.bundle.js"></script>
</environment>
<environment exclude="Development">
    <script src="~/lib/jquery/jquery.min.js"></script>
    <script src="~/lib/bootstrap/js/bootstrap.bundle.min.js"></script>
</environment>
<script src="~/js/site.js" asp-append-version="true"></script>
```

## Тег хелпер Link

Допоміжний тег \<link\> має атрибути, що використовуються як для локальних, так і для віддалених файлів CSS. Атрибут asp-append-version, який використовується з локальними файлами, додає хеш файлу як параметр рядка запиту до URL-адреси, що надсилається до браузера. Коли файл змінюється, хеш змінюється, оновлюючи URL-адресу, що надсилається до браузера. Оскільки посилання змінилося, браузер очищає кеш цього файлу та перезавантажує його.
Оновіть усі теги посилань у файлі _Head.cshtml до наступного (зверніть увагу, що теги посилань site.css та AutoLot.Mvc.styles.css вже мають атрибут asp-append-version):

```html
<environment include="Development">
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/lib/font-awesome/css/all.css" asp-append-version="true" />
</environment>
<environment exclude="Development">
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.min.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/lib/font-awesome/css/all.min.css" asp-append-version="true" />
</environment>
```
Посилання, яке надсилається до браузера для файлу site.css, тепер має такий вигляд (ваш хеш буде відрізнятися):
```html
<link rel="stylesheet" href="/AutoLot.Mvc.styles.css?v=c0q1pDJs2As7AtKAgl-1nEWPC6s1kTpJd4xkQzat-3U&amp;v=c0q1pDJs2As7AtKAgl-1nEWPC6s1kTpJd4xkQzat-3U">
```

Під час завантаження CSS-файлів з мережі доставки контенту (CDN) допоміжні засоби тегів надають механізм тестування, щоб переконатися, що файл завантажено правильно. Тест шукає певне значення (абсолютне) для властивості (позиції) у певному класі CSS (лише для SR), і якщо властивість не збігається, допоміжний засіб тегів завантажить резервний файл. Для параметра «Cross origin» має бути встановлено значення «anonymous», щоб файли cookie автентифікації не надсилалися до CDN, а перевірка цілісності гарантує, що саме очікуваний файл – це той, що завантажується. Якщо хеш CDN-файлу не пройде перевірку цілісності, допоміжний засіб тегів завантажить резервний файл. Оновіть теги посилань, що містяться в помічнику тегів \<environment exclude=”Development”\>, щоб вони відповідали наступному:

```html
<environment exclude="Development">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.1.3/css/bootstrap.min.css"
          asp-fallback-href="~/lib/bootstrap/css/bootstrap.min.css"
          asp-append-version="true"
          asp-fallback-test-class="sr-only"
          asp-fallback-test-property="position"
          asp-fallback-test-value="absolute"
          asp-suppress-fallback-integrity="true"
          crossorigin="anonymous"
          integrity="sha512-GQGU0fMMi238uA+a/bdWJfpUGKUkBdgfFdgBm72SUQ6BeyWjoY/ton0tEjH+OSH9iP4Dfh+7HM0I9f5eR0L/4w=="/>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css"
          asp-fallback-href="~/lib/font-awesome/css/all.min.css"
          asp-append-version="true"
          asp-fallback-test-class="fab"
          asp-fallback-test-property="display"
          asp-fallback-test-value="inline-block"
          asp-suppress-fallback-integrity="true"
          crossorigin="anonymous"
          integrity="sha512-1ycn6IcaQQ40/MKBW2W4Rhis/DbILU74C1vSrLJxCq57o941Ym01SwNsOMqvEBFlcgUa6xLiPY/NS5R+E6ztJQ=="/>
</environment>
```

Важливе зауваження, особливо якщо ви розробляєте/розгортаєте у Windows, полягає в тому, що перевірка цілісності застосовується як до CDN-файлу, так і до локального файлу. Якщо ваш локальний файл змінить закінчення рядків, то хеш локального файлу не збігатиметься з хешем в атрибуті цілісності, і ресурс буде заблоковано. Цю проблему можна вирішити, не проводячи перевірку цілісності резервного файлу, додавши asp-suppress-fallback-integrity='true' до допоміжних функцій тегу посилання:

Якщо ви вирішите пропустити перевірку цілісності вашого локального файлу, переконайтеся, що виконано інші перевірки, щоб переконатися, що локальний файл справді є правильним файлом і не був підроблений або зламаний.

## Тег хелпер Script

Допоміжний елемент тегу script схожий на допоміжний елемент тегу \<link\> з налаштуваннями кеш-бастингу та резервного CDN. Атрибут asp-append-version працює так само для скриптів, як і для пов'язаних таблиць стилів.
Атрибути asp-fallback-* також використовуються з джерелами файлів CDN. asp-fallback-test просто перевіряє достовірність JavaScript і, якщо не вдається, завантажує файл із резервного джерела. 
Оновіть код _JavaScriptFiles.cshtml, щоб використовувати можливості кешування та резервного CDN (зверніть увагу, що тег скрипта site.js вже має атрибут asp-append-version):

```html
<environment exclude="Development">
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js"
            asp-append-version="true"
            asp-fallback-src="~/lib/jquery/jquery.min.js"
            asp-fallback-test="window.jQuery"
            asp-suppress-fallback-integrity="true" 
            crossorigin="anonymous"
            integrity="sha512-894YE6QWD5I59HgZOGReFYm4dnWc1Qt5NtvYSaNcOP+u1T9qYdvdihz0PPSiiqn/+/3e7Jo4EaG7TubfWGUrMQ==">
    </script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/js/bootstrap.bundle.min.js"
            asp-append-version="true"
            asp-fallback-src="~/lib/bootstrap/js/bootstrap.bundle.min.js"
            asp-fallback-test="window.jQuery && window.jQuery.fn && window.jQuery.fn.modal"
            asp-suppress-fallback-integrity="true"
            crossorigin="anonymous"
            integrity="sha384-ka7Sk0Gln4gmtz2MlQnikT1wXgYsOg+OMhuP+IlRH9sENBO0LRn5q+8nbTov4+1p">
    </script>
</environment>
```
## Тег хелпер Image

Допоміжний засіб тегу image надає атрибут asp-append-version, який працює так само, як описано в допоміжних засобах тегів  link та script.

# Кастомний тег хелпер

Як і вбудовані помічники тегів, можна розробити користувацькі тег хелпери, які допоможуть формувати HTML-теги або створювати спеціальні теги. Під час створення інтерфейсу користувача на основі CRUD кожен екран зазвичай має набір посилань для переходу на інші екрани в програмі. Наприклад, на сторінці деталей є посилання для редагування, видалення та повернення до списку. Для AutoLot.Mvc спеціальні тег хелпери замінять HTML-код, який використовується для навігації по екранах CRUD для Car, зменшуючи кількість повторюваного коду та забезпечуючи узгоджений зовнішній вигляд. 

## Встановлення основи
Користувацьки тег хелпери використовують UrlHelperFactory та IActionContextAccessor для створення посилань на основі маршрутизації. 

## Оновлення Program.cs

Щоб створити екземпляр UrlFactory з класу, що не походить від Controller, до колекції services потрібно додати IActionContextAccessor. Викличте наступний рядок у Program.cs, щоб додати IActionContextAccessor до колекції служб:

```cs
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
```

## Створення базового класу

Створіть нову папку з назвою TagHelpers у кореневому каталозі проекту AutoLot.Mvc. У цій папці створіть нову папку з назвою Base, а в цій папці створіть клас з назвою ItemLinkTagHelperBase.cs, зробіть клас публічним та абстрактним і успадкуйте його від TagHelper:

```cs
namespace AutoLot.Mvc.TagHelpers.Base;

public abstract class ItemLinkTagHelperBase :TagHelper
{

}
```
Додайте конструктор, який приймає екземпляри IActionContextAccessor та IUrlHelperFactory. Використайте UrlHelperFactory разом з ActionContextAccessor для створення екземпляра IUrlHelper та збереження його у змінній рівня класу:

```cs
    protected readonly IUrlHelper UrlHelper;
    protected ItemLinkTagHelperBase(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
    {
        UrlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
    }

```
У конструкторі використовуйте екземпляр contextAccessor, щоб отримати поточний контролер та призначити його полю рівня класу:

```cs
    private readonly string? _controllerName;
    protected ItemLinkTagHelperBase(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
    {
        UrlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
        _controllerName = contextAccessor.ActionContext.ActionDescriptor.RouteValues["controller"];
    }
```
Додайте захищену властивість, щоб похідні класи могли вказувати назву дії для маршруту:

```cs
protected string ActionName { get; set; }
```
Додайте одну публічну властивість для зберігання ідентифікатора елемента, наступним чином:

```cs
public int? ItemId { get; set; }
```
Публічні властивості на користувацьких тег хелперах відображаються як атрибути HTML на тегу. Згідно з домовленістю про іменування, назва властивості перетворюється на нижній регістр літер. Це означає, що кожна велика літера пишеться малою, а перед кожною літерою, яка змінюється на малу (окрім першої), вставляються тире (-). Це перетворює ItemId на item-id (як слова на шашлику).
Коли викликається тег хелпер, викликається метод Process(). Метод Process() приймає два параметри: TagHelperContext та TagHelperOutput. TagHelperContext використовується для отримання будь-яких інших атрибутів тегу та словника об'єктів, що використовуються для зв'язку з іншими помічниками тегів, що орієнтовані на дочірні елементи. TagHelperOutput використовується для створення відрендереного виводу. 
Оскільки це базовий клас, ми додамо метод під назвою BuildContent(), який похідні класи зможуть викликати з методу Process(). Додайте наступний метод і код:

```cs
    protected void BuildContent(TagHelperOutput output, string cssClassName, string displayText, string fontAwesomeName)
    {
        output.TagName = "a"; // Replaces <item-> with <a> tag
        var target = (ItemId.HasValue)
            ? UrlHelper.Action(ActionName, _controllerName, new { id = ItemId })
            : UrlHelper.Action(ActionName, _controllerName);
        output.Attributes.SetAttribute("href", target);
        output.Attributes.Add("class", cssClassName);
        output.Content.AppendHtml($@"{displayText} <i class=""fas fa-{fontAwesomeName}""></i>");
    }
```

Перший рядок змінює тег з тегу, що використовується в розмітці, на тег прив'язки. Наступний рядок використовує статичний метод UrlAction.Action() для генерації маршруту, включаючи параметр маршруту, якщо такий існує. Наступні два встановлюють HREF тегу прив'язки на згенерований маршрут і додають назву класу CSS. В останньому рядку додано текст для відображення та шрифт Font Awesome як текст, який відображається користувачеві.
Як останній крок, додайте наступний глобальний оператор using до файлу GlobalUsings.cs:

```cs
global using AutoLot.Mvc.TagHelpers.Base;
```

## Тег хелпер для елемента Details

Створіть новий клас з назвою ItemDetailsTagHelper.cs у папці TagHelpers. Зробіть клас публічним та успадкуйте його від ItemLinkTagHelperBase.

```cs
namespace AutoLot.Mvc.TagHelpers;

public class ItemDetailsTagHelper : ItemLinkTagHelperBase
{
    public ItemDetailsTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory) : base(contextAccessor, urlHelperFactory)
    {
    }
}
```
Додайте конструктор для прийняття необхідних екземплярів об'єктів та передачі їх базовому класу. Конструктору також потрібно призначити ActionName:

```cs
    public ItemDetailsTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory) : base(contextAccessor, urlHelperFactory)
    {
        ActionName = nameof(CarsController.DetailsAsync).RemoveAsyncSuffix();
    }
```
Примітка щодо імені контролера, яке використовується в методі nameof(): нагадаємо, що метод повертає ім'я методу (DetailsAsync), а ім'я контролера не виводиться. Для кожного з цих користувацьких помічників тегів базовий клас повинен знати назву методу дії, а всі основні дії CRUD знаходяться на базовому контролері. Однак, використання nameof() із загальним BaseCrudController трохи заплутане, тому ми замінюємо реалізацію BaseCrudController на використання методу nameof().
Перевизначте метод Process(), викликавши метод BuildContent() у базовому класі.

```cs
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        //<a asp-action="Details" asp-route-id="@item.Id" class="text-info">Details <i class="fas fa-info-circle"></i></a>
        BuildContent(output, "text-info", "Details", "info-circle");
    }
```
Це створює посилання «Details» за допомогою класу CSS text-info, тексту «Details» та зображення Font Awesome info.
Під час виклику допоміжних функцій тегів суфікс TagHelper пропускається, а решта назви класу починається з малої літери. У цьому випадку тег HTML — <item-details>. Значення asp-route-id походить з атрибута item-id на допоміжному механізмі тегів:

```html
<item-details item-id='@item.Id'></item-details>
```

## Зробимо видимими користувацькі тег хелпери.

Щоб зробити видимими користувацькі тег хелпери, команду @addTagHelper потрібно виконати для будь-яких перегляду, які використовують помічники тегів або додаються до файлу _ViewImports.cshtml. Відкрийте файл _ViewImports.cshtml і додайте наступний рядок:

```cshtml
@addTagHelper *, AutoLot.Mvc
```

Перевіримо тег хелпер ItemDetails в файлі Views\TestBaseCrud\Index.cshtml

```html
<h1>Tag Helpers</h1>
<hr/>

<item-details item-id="3"></item-details>
```

## Тег хелпер для елемента Delete

Створіть новий клас з назвою ItemDeleteTagHelper.cs у папці TagHelpers. Зробіть клас публічним та успадковуйте його від ItemLinkTagHelperBase. Додайте конструктор для прийняття потрібних екземплярів об'єктів та встановіть ActionName, використовуючи назву методу DeleteAsync():

```cs
namespace AutoLot.Mvc.TagHelpers;

public class ItemDeleteTagHelper : ItemLinkTagHelperBase
{
    public ItemDeleteTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory) : base(contextAccessor, urlHelperFactory)
    {
        ActionName = nameof(CarsController.DeleteAsync).RemoveAsyncSuffix();
    }
}
```
Перевизначте метод Process(), викликавши метод BuildContent() у базовому класі.

```cs
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        //<a asp-action="Delete" asp-route-id="@item.Id" class="text-danger">Delete <i class="fas fa-trash"></i></a>
        BuildContent(output,"text-danger","Delete","trash");
    }
```

Це створює посилання «Delete» із зображенням сміттєвого кошика Font Awesome.


## Тег хелпер для елемента Edit 

Створіть новий клас з назвою ItemEditTagHelper.cs у папці TagHelpers. Зробіть клас публічним, успадкуйте його від ItemLinkTagHelperBase та додайте конструктор, який призначає Edit як ActionName:

```cs
namespace AutoLot.Mvc.TagHelpers;

public class ItemEditTagHelper : ItemLinkTagHelperBase
{
    public ItemEditTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory) : base(contextAccessor, urlHelperFactory)
    {
        ActionName = nameof(CarsController.EditAsync).RemoveAsyncSuffix();
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        //<a asp-action="Edit" asp-route-id="@item.Id" class="text-warning">Delete <i class="fas fa-trash"></i></a>

        BuildContent(output, "text-warning", "Edit", "edit");
    }
}
```
Це створює посилання «Edit» із зображенням олівця Font Awesome:


## Тег хелпер для елемента Create

Створіть новий клас з назвою ItemCreateTagHelper.cs у папці TagHelpers. Зробіть клас публічним, успадкуйте його від ItemLinkTagHelperBase та додайте конструктор, який призначає Create як ActionName:

```cs
namespace AutoLot.Mvc.TagHelpers;

public class ItemCreateTagHelper : ItemLinkTagHelperBase
{
    public ItemCreateTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory) : base(contextAccessor, urlHelperFactory)
    {
        ActionName = nameof(CarsController.CreateAsync).RemoveAsyncSuffix();
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output, "text-success", "Create New", "plus");
    }
}
```
Це створює посилання «Create» із зображенням Font Awesome plus.

## Тег хелпер для елемента List

Створіть новий клас з назвою ItemListTagHelper.cs у папці TagHelpers. Зробіть клас публічним, успадкуйте його від ItemLinkTagHelperBase та додайте конструктор, який призначає Index як ActionName:

```cs
namespace AutoLot.Mvc.TagHelpers;

public class ItemListTagHelper : ItemLinkTagHelperBase
{
    public ItemListTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory) : base(contextAccessor, urlHelperFactory)
    {
        ActionName = nameof(CarsController.IndexAsync).RemoveAsyncSuffix();
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output,"text-default","Back to List","list");
    }
}
```
Це створює посилання Index із зображенням списку Font Awesome

## Перевіримо всі створені тег хелпери

Змінимо файл Views\TestBaseCrud\Index.cshtml

```html
<h1>Tag Helpers</h1>
<hr />

<item-details item-id="1"></item-details>
<item-delete item-id="1"></item-delete>
<item-edit item-id="1"></item-edit>
<item-create></item-create>
<item-list></item-list>
```

# HTML хелпер

Хоча помічники тегів замінили багато помічників HTML, все ще існує кілька таких, для яких немає еквівалента. У таблиці перелічені деякі, які досі широко використовуються (і використовуються в цьому розділі).

Часто використовувані HTML хелпери

|HTML Helper|Використання|
|-----------|------------|
|Html.DisplayFor()|Відображає об'єкт, визначений виразом|
|Html.DisplayForModel()|Відображає модель, використовуючи шаблон за замовчуванням або власний шаблон|
|Html.DisplayNameFor()|Отримує відображуване ім'я, якщо воно існує, або ім'я властивості, якщо відображуване ім'я відсутнє.|
|Html.EditorFor()|Відображає поле редагування для об'єкта, визначеного виразом|
|Html.EditorForModel()|Відображає редактор для моделі, використовуючи шаблон за замовчуванням або власний шаблон|

## HTML хелпер  DisplayFor 

Допоміжний метод DisplayFor() відображає об'єкт, визначений виразом. Якщо для відображаного типу існує шаблон відображення, він буде використаний для створення HTML-коду елемента. Наприклад, у перегляді RazorSyntax інформацію про марку автомобіля у поданні можна відобразити за допомогою цього коду:

```cshtml
@Html.DisplayFor(x=>x.MakeNavigation)
```
Оскільки для класу Make немає доступного власного шаблону (у папці DisplayTemplates поточного каталогу контролера або каталогу Shared), механізм перегляду використав рефлексію для генерації наступного HTML-коду:

```html
<div class="display-label">Name</div>
<div class="display-field">BMW</div>
<div class="display-label">Id</div>
<div class="display-field">5</div>
```
Якщо існує та доступне представлення з назвою Make.cshtml, то це представлення буде використано для відображення HTML (пам’ятайте, що пошук назви шаблону базується на типі об’єкта, а не на назві його властивості). Якщо існує представлення з назвою ShowMake.cshtml (наприклад), його можна використовувати для візуалізації об'єкта за допомогою наступного виклику:

```cs
@Html.DisplayFor(x=>x.MakeNavigation, "ShowMake");
```
Якщо шаблон не вказано і для назви класу немає шаблону, для створення HTML-коду для відображення використовується рефлексія.

## HTML хелпер DisplayForModel

Допоміжний метод DisplayForModel() відображає модель для представлення. Якщо для відображаного типу існує шаблон відображення, він буде використаний для створення HTML-коду для елемента. Додайте наступний код внизу представлення RazorSyntax:

```cshtml
@Html.DisplayForModel();
```
Як і у випадку з допоміжною функцією DisplayFor(), якщо існує шаблон відображення з назвою для типу, він буде використаний. Існує шаблон відображення Car.cshtml, але він знаходиться в структурі каталогів Cars, а не в структурі каталогу Home, де знаходиться представлення RazorSyntax. Щоб використати шаблон з іншої структури каталогів, потрібно вказати не лише назву представлення, але й повний шлях і розширення файлу:

```cshtml
@Html.DisplayForModel("../Cars/DisplayTemplates/Car.cshtml")
```
Також можна використовувати іменовані шаблони. Наприклад, щоб відобразити автомобіль за допомогою шаблону відображення CarWithColors.html, використовуйте наступний виклик (припускаючи, що шаблон знаходиться в тій самій структурі каталогів або в Shared каталозі):

```cshtml
@Html.DisplayForModel("CarWithColors.cshtml")
```

## HTML хелпери EditorFor та EditorForModel

Помічники EditorFor() та EditorForModel() функціонують так само, як і відповідні помічники відображення. Різниця полягає в тому, що пошук шаблонів здійснюється в каталозі EditorTemplates, а замість представлення об'єкта лише для читання відображаються HTML-редактори.