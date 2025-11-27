# Веб-застосунки Razor Pages

Цей розділ базується на тому, що ви вивчили в попередній главі, і завершує розробку програми на основі Razor Page на AutoLot.Web. Базова архітектура застосунків на основі Razor Page дуже схожа на застосунки в стилі MVC, з головною відмінністю в тому, що вони базуються на сторінках, а не на контролерах. У цьому розділі буде висвітлено відмінності під час створення застосунку AutoLot.Web, і припускається, що ви прочитали попередні розділи про ASP.NET Core.

    Ви можете продовжувати роботу з рішенням, яке ви розпочали в попередніх розділах ASP.NET Core.

# Анатомія сторінки Razor

На відміну від програм у стилі MVC, представлення в програмах на основі Razor Page є частиною сторінки. Для демонстрації додайте нову порожню сторінку Razor з назвою RazorSyntax, клацнувши правою кнопкою миші на каталозі Pages у проекті AutoLot.Web у Visual Studio, виберіть Add  ➤ Razor Page та виберіть шаблон Razor Page – Empty. Ви побачите два створені файли: RazorSyntax.cshtml та RazorSyntax.cshtml.cs. Файл RazorSyntax.cshtml – це представлення сторінки, а файл RazorSyntax.cshtml.cs – це файл коду для представлення. 
Перш ніж продовжити, додайте такі оператори using до файлу GlobalUsings.cs у проекті AutoLot.Web:

```cs
global using AutoLot.Models.Entities;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using AutoLot.Services.DataServices.Interfaces;
global using Microsoft.Build.Framework;
```

## Razor Page PageModel класи та методи обробника сторінки

Код файлу для Razor Page походить від базового класу PageModel та має назву з суфіксом Model, як-от RazorSyntaxModel. Базовий клас PageModel, як і базовий клас Controller у застосунках у стилі MVC, надає багато допоміжних методів, корисних для створення вебзастосунків. На відміну від класу Controller, Razor Pages прив'язані до одного представлення, мають маршрути на основі структури каталогів та єдиний набір методів обробника сторінок для обслуговування HTTP-запитів get (OnGet()/OnGetAsync()) та post (OnPost()/OnPostAsync()). 
Змініть сформований клас RazorSyntaxModel так, щоб метод обробника сторінок OnGet() був асинхронним, та оновіть назву на OnGetAsync(). Далі додайте метод обробника сторінок async OnPostAsync() для HTTP-запитів Post:

```cs
namespace AutoLot.Web.Pages;
public class RazorSyntaxModel : PageModel
{
    public async Task OnGetAsync()
    {
    }

    public async Task OnPostAsync()
    {
    }
}
```

    Імена за замовчуванням можна змінити. Це буде розглянуто пізніше.


Зверніть увагу, що методи обробника сторінок не повертають значення, як їхні аналоги методів дій. Коли метод обробника сторінки повертає значення, сторінка неявно повертає вигляд, з яким пов'язана ця сторінка. Методи обробника сторінок Razor також підтримують повернення IActionResult, що потім вимагає явного повернення IActionResult. Якщо метод має повернути вигляд класу, повертається результат методу Page(). Метод також може перенаправляти на іншу сторінку.

```cs
    public async Task<IActionResult> OnGetAsync()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        return RedirectToPage("Index");
    }
```
Похідні класи PageModel підтримують як впровадження методів, так і впровадження конструкторів. Під час використання ін'єкції методу параметр має бути позначений атрибутом [FromService], ось так:

```cs
public async Task OnGetAsync([FromServices] ICarDataService carDataService)
{
  //Get a car instance
}
```
Оскільки класи PageModel зосереджені на одному представленні, частіше використовують ін'єкцію конструктора замість ін'єкції методу. Оновіть клас RazorSyntaxModel, додавши конструктор, який приймає екземпляр ICarDataService та призначає його полю рівня класу:

```cs
    private readonly ICarDataService _carDataService;
    public RazorSyntaxModel(ICarDataService carDataService)
    {
        _carDataService = carDataService;
    }
```
Якщо ви перевірите метод Page(), то побачите, що немає перевантаження, яке приймає об'єкт. Хоча пов'язаний метод View() у MVC використовується для передачі моделі до представлення, Razor Pages використовує властивості класу PageModel для надсилання даних до представлення. Додайте нову публічну властивість з назвою Entity типу Car до класу RazorSyntaxModel:

```cs
   public Car Entity { get; set; }
```
Тепер скористайтеся сервісом даних, щоб отримати запис Car та призначити його публічній властивості (якщо прапорець UseApi в appsettings.Development.json встановлено на true, переконайтеся, що AutoLot.Api запущено):

```cs
    public async Task<IActionResult> OnGetAsync()
    {
        Entity = await _carDataService.FindAsync(6);
        return Page();
    }
```
Razor Pages може використовувати неявне зв'язування для отримання даних з представлення, так само як і методи дій MVC:

```cs
    public async Task<IActionResult> OnPostAsync(Car entity)
    {
        //do something interesting
        return RedirectToPage("Index");
    }
```
Razor Pages також підтримує явне зв'язування:

```cs
    public async Task<IActionResult> OnPostAsync()
    {
        var newCar = new Car();
        if (await TryUpdateModelAsync(newCar, "Entity",
            c => c.Id,
            c => c.TimeStamp,
            c => c.PetName,
            c => c.Color,
            c => c.IsDrivable,
            c => c.MakeId,
            c => c.Price
          ))
        {
            //do something interesting
        }
        return RedirectToPage("Index");
    }
```

Однак, поширеною практикою є оголошення властивості, яка використовується методом HTTP get, як BindProperty:

```cs
    [BindProperty]
    public Car Entity { get; set; }
```
Ця властивість потім буде неявно прив'язана під час HTTP-запитів post, а методи OnPost()/OnPostAsync() використовуватимуть прив'язану властивість:

```cs
    public async Task<IActionResult> OnPostAsync()
    {
        await _carDataService.UpdateAsync(Entity);
        return RedirectToPage("Index");
    }
```

## Перегляди сторінок Razor
Перегляди сторінок Razor є специфічними для Razor PageModel, починаються з директиви @page та вводяться в код за файлом, ось так для зібраної сторінки RazorSyntax:

```html
@page
@model AutoLot.Web.Pages.RazorSyntaxModel
@{
}
```
Зверніть увагу, що представлення не прив’язане до BindProperty (якщо таке існує), а до похідного класу PageModel. Властивості похідного класу PageModel (наприклад, властивість Entity на сторінці RazorSyntax) є продовженням @Model. Щоб створити форму, необхідну для тестування різних сценаріїв зв'язування, додайте наступний код до подання RazorSyntax.cshtml:

```html
<h1>Razor Syntax</h1>
<form asp-page="RazorSyntaxModel">
    <input type="hidden" asp-for="@Model.Entity.Id" />
    <input type="hidden" asp-for="@Model.Entity.TimeStamp" />
    <input asp-for="@Model.Entity.PetName" /><br />
    <input asp-for="@Model.Entity.Color" /><br />
    <input asp-for="@Model.Entity.IsDrivable" /><br />
    <input asp-for="@Model.Entity.MakeId" /><br />
    <input asp-for="@Model.Entity.Price" /><br />
    <input asp-for="@Model.Entity.DateBuilt"/><br />
    <button type="submit">Submit</button>
</form>
```
Запустіть програму та перейдіть за адресою https://localhost:5021/RazorSyntax.
Зверніть увагу, що властивість не обов'язково має бути BindProperty для доступу до значень у представленні. Вона має бути BindProperty лише для того, щоб метод HTTP post неявно зв'язував значення.
Як і у випадку з додатками на основі MVC, HTML, CSS, JavaScript та Razor працюють разом у представленнях сторінок Razor. Весь базовий синтаксис Razor, розглянутий у попередньому розділі, підтримується у представленнях сторінок Razor, включаючи помічники тегів та помічники HTML. Єдина відмінність полягає у посиланні на властивості моделі, як було продемонстровано раніше. Щоб підтвердити це, оновіть представлення, додавши наступний код з прикладу попередньої глави, зі змінами (повне обговорення синтаксису дивіться в попередній главі):

```html
<h3>
    @for (int i = 0; i < 13; i++)
    {
        @i
        @:&nbsp;
    }
</h3>
<br />

@{
    //Code Block
    var foo = "Foo";
    var bar = "Bar";
    var htmlList = "<ul><li>one</li><li>two</li></ul>";
}

@foo
<br />
@foo.@bar
<br />
@htmlList
<br />


@foo.ToUpper()

<br />
@Html.Raw(htmlList)
<br />

@{
    @:Straight Text
    <div>Value:@Model.Entity.PetName</div>
    <text>
        Lines without HTML tag
    </text>
    <br />
}
<hr />

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

@*
    Multiline Comments
    Hi.
*@

@functions {
    public static IList<string> SortList(IList<string> strings)
    {
        var list = from s in strings orderby s select s;
        return list.ToList();
    }
}

@{
    var myList = new List<string> { "C", "A", "Z", "F" };
    var sortedList = SortList(myList);
}
@foreach (string s in sortedList)
{
    @s

    @:&nbsp;
}

@* @{
    Func<dynamic, object> b = @<strong>@item</strong>;
}
This will be bold: @b("Foo") *@

<hr />
The Car named @Model.Entity.PetName is a <span style="color:@Model.Entity.Color">@Model.Entity.Color</span> @Model.Entity.MakeNavigation.Name
<hr />
<h3>Display For examples</h3>
@Html.DisplayFor(x => x.Entity.MakeNavigation)
<hr />
@Html.DisplayFor(c => c.Entity)
<hr />
@Html.EditorFor(c => c.Entity)
<hr />
```
Зверніть увагу на зміну в останніх двох рядках. Методи _DisplayForModel()/EditorForModel() поводяться по-різному в Razor Pages, оскільки представлення прив'язане до PageModel, а не до entity/viewmodel представлення, як у MVC-застосунках.

# Перегляди застосунку

Представлення Razor у стилі MVC (без похідного класу PageModel як коду) та часткові представлення також підтримуються в застосунках Razor Page. Це включає файли _ViewStart.cshtml, _ViewImports.cshtml (обидва в каталозі \Pages) та _Layout.cshtml, розташовані в каталозі Pages\Shared. Усі три забезпечують ту саму функціональність, що й у застосунках на основі MVC.

## Представлення _ViewStart та _ViewImports

Файл _ViewStart.cshtml виконує свій код перед відображенням будь-якого іншого представлення сторінки Razor та використовується для встановлення макета за замовчуванням.

```html
@{
    Layout = "_Layout";
}
```
Файл _ViewImports.cshtml використовується для імпорту спільних директив, таких як оператори @using. Його вміст застосовується до всіх представлень в одному каталозі або підкаталозі файлу _ViewImports. Цей файл є еквівалентом представлення файлу GlobalUsings.cs для коду C#.

```html
@using AutoLot.Web
@namespace AutoLot.Web.Pages
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers

```
Оголошення @namespace визначає простір імен за замовчуванням, де розташовані сторінки застосунку.

## Каталог Shared
Shared каталог у розділі Pages містить часткові подання, шаблони відображення та редагування, а також макети, доступні для всіх сторінок Razor.

## Папка DisplayTemplates

Шаблони відображення працюють однаково в MVC та Razor Pages. Вони розміщуються в каталозі з назвою DisplayTemplates і керують тим, як відображаються типи під час виклику методу DisplayFor(). Шлях пошуку починається в каталозі Pages\\{CurrentPageRoute}\DisplayTemplates, і якщо його не знайдено, то пошук здійснюється в папці Pages\Shared\DisplayTemplates. Як і у випадку з MVC, рушій шукає шаблон з такою ж назвою, як і тип, що відтворюється, або шаблон, назва якого відповідає назві, переданій у метод.

## Шаблон відображення дати та часу

Створіть нову папку з назвою DisplayTemplates у папці Pages\Shared. Очистіть весь згенерований код та коментарі та замініть їх наступним:

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

Зверніть увагу, що директива @model, яка суворо типізує представлення, використовує малу літеру m. Під час посилання на призначене значення моделі в Razor використовується велика літера M. У цьому прикладі визначення моделі може мати значення null. Якщо значення моделі, передане у представлення, дорівнює null, шаблон відображає слово Unknown. В іншому випадку дата відображається у форматі Short Date, використовуючи властивість Value типу, що допускає значення null, або саму модель. З цим шаблоном, якщо ви запустите програму та перейдете на сторінку RazorSyntax, ви побачите, що значення BuiltDate відформатоване як коротка дата.

## Шаблон відображення Car
Створіть новий каталог з назвою Cars у каталозі Pages та додайте каталог з назвою DisplayTemplates у каталог Cars. Додайте нове представлення з назвою Car.cshtml до цієї папки. Очистіть весь згенерований код і коментарі та замініть їх наступним кодом, який відображає сутність Car:

```html
@model AutoLot.Models.Entities.Car
<dl class="row">
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
HTML-хелпер DisplayNameFor() відображає назву властивості, якщо тільки властивість не оформлена атрибутом Display(Name='') або DisplayName(''), і в цьому випадку використовується значення відображення. Метод DisplayFor() відображає значення властивості моделі, зазначеної у виразі. Зверніть увагу, що властивість navigation для MakeNavigation використовується для отримання назви виробника. 
Щоб використовувати шаблон з іншої структури каталогів, потрібно вказати назву представлення, а також повний шлях і розширення файлу. Щоб використовувати цей шаблон у представленні RazorSyntax, оновіть метод DisplayFor() до наступного:

```html
@Html.DisplayFor(c=>c.Entity,"Cars/DisplayTemplates/Car.cshtml")
```
Інший варіант – перемістити шаблони відображення до каталогу Pages\Shared\DisplayTemplates

### Шаблон відображення Car з кольором

Скопіюйте представлення Car.cshtml до іншого представлення з назвою CarWithColors.cshtml у каталозі Cars\DisplayTemplates. Різниця полягає в тому, що цей шаблон змінює колір тексту Color на основі значення властивості Color моделі. Оновіть тег <dd> нового шаблону для кольору наступним чином:

```html
    <dd class="col-sm-10" style="color:@Model.Color">
        @Html.DisplayFor(model => model.Color)
    </dd>
```
## Шаблони редагування 

Папка EditorTemplates працює так само, як і папка DisplayTemplates, за винятком того, що шаблони використовуються для редагування.

## Шаблон редагування Car

Створіть новий каталог з назвою EditorTemplates у каталозі Pages\Cars. Додайте нове представлення з назвою Car.cshtml до цієї папки. Очистіть весь згенерований код і коментарі та замініть їх наступним кодом, який представляє розмітку для редагування сутності Car:

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
Шаблони редакторів викликаються за допомогою допоміжної функції HTML EditorFor(). Щоб використовувати її зі сторінкою RazorSyntax, оновіть виклик EditorFor() наступним чином:

```html
@Html.EditorFor(c=>c.Entity,"Cars/EditorTemplates/Car.cshtml")
```

## Ізоляцію CSS переглядів.

Razor Pages також підтримує ізоляцію CSS. Клацніть правою кнопкою миші на каталозі \Pages та виберіть Add ➤ New Item,, перейдіть до ASP.NET Core/Web/Content у лівій панелі, виберіть Style Sheet та назвіть її Index.cshtml.css. Оновіть вміст до наступного:

```css
h1 {
    background-color: azure;
}
```
Ця зміна робить тег \<h1\> на сторінці Index, але не впливає на інші сторінки. У Razor Pages застосовуються ті самі правила, що й у MVC з ізоляцією CSS перегляду: файл CSS генерується лише під час запуску в режимі розробки або під час публікації сайту. Щоб побачити CSS в інших середовищах, вам потрібно ввімкнути цю функцію:

```cs
//Enable CSS isolation in a non-deployed session
if (!builder.Environment.IsDevelopment())
{
    builder.WebHost.UseWebRoot("wwwroot");
    builder.WebHost.UseStaticWebAssets();
}
```

## Макети

Макети в Razor Pages функціонують так само, як і в MVC-застосунках, за винятком того, що вони розташовані в Pages\Shared, а не Views\Shared. _ViewStart.cshtml використовується для визначення макета за замовчуванням для структури каталогів, а перегляди Razor Page можуть явно визначати свій макет за допомогою блоку Razor:

```html
@{
    Layout = '_MyCustomLayout';
}
```

## Внессеня данних

Додайте наступний код на початок файлу _Layout.cshtml, який вводить IWebHostEnvironment:

```html
@inject IWebHostEnvironment _env
```
Далі оновіть нижній колонтитул, щоб відобразити середовище, в якому зараз працює програма:

```html
            &copy; 2025 - AutoLot.Web - @_env.EnvironmentName - <a asp-area="" asp-page="/Privacy">Privacy</a>
```

## Часткові представлення

Основна відмінність часткових представлень у Razor Pages полягає в тому, що представлення Razor Page не може бути відображено як часткове представлення. У Razor Pages вони використовуються для інкапсуляції елементів інтерфейсу та завантажуються з іншого представлення або компонента представлення. Далі ми розділимо макет на часткові елементи, щоб полегшити підтримку розмітки.

## Створення часткових елементів.

Створіть новий каталог з назвою Partials у каталозі Shared. Клацніть правою кнопкою миші на новому каталозі та виберіть Add ➤ New Item. Введіть Razor View у поле пошуку та виберіть Razor View -Empty. Створіть три порожні представлення з назвами _Head.cshtml, _JavaScriptFiles.cshtml та _Menu.cshtml.
Виріжте вміст макета, що знаходиться між тегами \<head\>\</head\>, та вставте його у файл _Head.cshtml. У _Layout.cshtml замініть видалену розмітку викликом для відображення нового часткового коду:

```html
<head>
    <partial name="Partials/_Head" />
</head>
```
Для частини меню виріжте всю розмітку між тегами \<header\>\</header\> та вставте її у файл _Menu.cshtml. Оновіть _Layout, щоб відобразити частину меню.

```html
        <partial name="Partials/_Menu" />
```
Останнім кроком на цьому етапі є вирізання тегів \<script\> (після закриваючого тегу \</footer\>) для файлів JavaScript та вставка їх у частковий JavaScriptFiles. Обов’язково залиште тег RenderSection на місці.

```html
    <partial name="Partials/_JavaScriptFiles" />
```

## Надсилання даних до часткових пердставлень

Властивість похідної PageModel можна передати в часткове представлення за допомогою допоміжного тегу \<partial\>, як показано тут:

```html
<partial name='Partials/_CarListPartial' model='@Model.Entities'/>
```


## ViewBag, ViewData та TempData

Сторінки Razor також підтримують об'єкти ViewBag, ViewData та TempData. Нагадаємо, що частина \<head\> представлення _Layout.cshtml (тепер у _Head.cshtml) використовує ViewData для встановлення заголовка сторінки:

```html
<title>@ViewData["Title"] - AutoLot.Web</title>
```
За допомогою Razor Pages ви можете посилатися на ViewData у блоці Razor у перегляді:

```html
@page
@model AutoLot.Web.Pages.RazorSyntaxModel
@{
    ViewData["Title"] = "Razor Syntax";
}
```
Ви також можете встановити властивості ViewData, прикрасивши властивості PageModel атрибутом [ViewData]. Наведений нижче код дає той самий результат, що й блок view Razor, показаний у попередньому прикладі коду:

```cs
    [ViewData]
    public string Title => "Razor Syntax";
```
Тепер, коли ви запустите програму та перейдете за адресою https://localhost:5021/Home/RazorSyntax, ви побачите заголовок «Razor Syntax – AutoLot.Web» у вкладці браузера. 
Це працює так само для TempData.

Додамо SelectList.

```html
@page
@model AutoLot.Web.Pages.RazorSyntaxModel
@{
    ViewData["Title"] = "Razor Syntax";
    ViewData["LookupValues"] = new List<SelectListItem>
    {
        new SelectListItem { Value = "1", Text = "VW" },
        new SelectListItem { Value = "2", Text = "BMW" },
        new SelectListItem { Value = "3", Text = "ZAZ" },
        new SelectListItem { Value = "4", Text = "Ford" },
    };
}
```

# Додавання клієнтських бібліотек до AutoLot.Web

Шаблон за замовчуванням завантажив бібліотеки CSS та JavaScript у каталог wwwroot\lib. Щоб перейти до керування бібліотеками за допомогою LibraryManager, почніть із видалення всього каталогу lib і всіх каталогів і файлів, які він містить.

## Додайте файл libman.json

Файл libman.json контролює, що встановлюється, з яких джерел та куди встановлюються файли.

### У Visual Studio

Якщо ви використовуєте Visual Studio, клацніть правою кнопкою миші на проекті AutoLot.Web і виберіть Manage Client-Side Libraries. Це додає файл libman.json до кореневого каталогу проекту. У Visual Studio також є опція для інтеграції Library Manager з процесом MSBuild. Якщо ви не встановили пакет NuGet Microsoft.Web.LibraryManager.Build перед додаванням файлу libmon.json (що ми зробили під час збирання проектів у розділі 30), клацніть правою кнопкою миші на файлі libman.json і виберіть Enable restore on build. Це запропонує вам дозволити додавання пакета NuGet до проєкту. Дозвольте встановлення пакета, якщо буде запропоновано. Якщо спочатку встановити пакет, проєкт автоматично налаштується на відновлення під час збірки.

### У командному рядоку

Створіть новий файл libman.json за допомогою наступної команди (це встановлює постачальника за замовчуванням на cdnjs.com):

```console
libman init --default-provider cdnjs
```
Якщо ви не встановлюватимете постачальника за замовчуванням за допомогою командного рядка, вам буде запропоновано вибрати його, за замовчуванням буде встановлено cdnjs.

## Оновіть файл libman.json

Після додавання всіх файлів, необхідних для цієї програми, повний файл libman.json відображається тут:

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
Після збереження файлу (у Visual Studio) файли будуть завантажені в папку wwwroot\lib проекту. Якщо ви запускаєте програму з командного рядка, введіть таку команду, щоб перезавантажити всі файли:

```console
libman restore
```
Доступні додаткові параметри командного рядка. Введіть libman -h, щоб переглянути всі параметри.

## Оновіть посилання на JavaScript та CSS

Зі зміною розташування бібліотек JavaScript та CSS необхідно оновити часткові перегляди. Також буде додано помічники тегів <environment> та <link>. Спочатку оновіть файл _Head.cshtml до наступного:

```html
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1.0" />
<title>@ViewData["Title"] - AutoLot.Mvc</title>
<environment include="Development">
    <link rel="stylesheet" href="~/lib/bootstrap/css/bootstrap.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/lib/font-awesome/css/all.css" asp-append-version="true" />
</environment>
<environment exclude="Development">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap/5.1.3/css/bootstrap.min.css"
          asp-fallback-href="~/lib/bootstrap/css/bootstrap.min.css"
          asp-append-version="true"
          asp-fallback-test-class="sr-only"
          asp-fallback-test-property="position"
          asp-fallback-test-value="absolute"
          asp-suppress-fallback-integrity="true"
          crossorigin="anonymous"
          integrity="sha512-GQGU0fMMi238uA+a/bdWJfpUGKUkBdgfFdgBm72SUQ6BeyWjoY/ton0tEjH+OSH9iP4Dfh+7HM0I9f5eR0L/4w==" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css"
          asp-fallback-href="~/lib/font-awesome/css/all.min.css"
          asp-append-version="true"
          asp-fallback-test-class="fab"
          asp-fallback-test-property="display"
          asp-fallback-test-value="inline-block"
          asp-suppress-fallback-integrity="true"
          crossorigin="anonymous"
          integrity="sha512-1ycn6IcaQQ40/MKBW2W4Rhis/DbILU74C1vSrLJxCq57o941Ym01SwNsOMqvEBFlcgUa6xLiPY/NS5R+E6ztJQ==" />
</environment>
<link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
<link rel="stylesheet" href="~/AutoLot.Mvc.styles.css" asp-append-version="true" />
```
Далі оновіть _JavaScriptFiles.cshtml, щоб змінити розташування, та додайте допоміжні функції тегів \<environment\> та \<script\>.

```html
<environment include="Development">
    <script src="~/lib/jquery/jquery.js"></script>
    <script src="~/lib/bootstrap/js/bootstrap.bundle.js"></script>
</environment>
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
<script src="~/js/site.js" asp-append-version="true"></script>
```
Остання зміна полягає в оновленні розташування jquery.validation та додаванні допоміжних елементів тегів \<environment\> та \<script\> у частковому поданні _ValidationScriptsPartial.cshtml.

```html
<environment include="Development">
    <script src="~/lib/jquery-validation/jquery.validate.js" asp-append-version="true"></script>
    <script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.js" asp-append-version="true"></script>
    <script src="~/js/validationCode.js"></script>
</environment>
<environment exclude="Development">
    <script src="~/js/validationCode.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.19.3/jquery.validate.min.js"
            asp-fallback-src="~/lib/jquery-validation/jquery.validate.min.js"
            asp-fallback-test="window.jQuery && window.jQuery.validator"
            asp-append-version="true"
            crossorigin="anonymous"
            integrity="sha512-37T7leoNS06R80c8Ulq7cdCDU5MNQBwlYoy1TX/WUsLFC2eYNqtKlV0QjH7r8JpG/S0GUMZwebnVFLPd6SU5yg==">
    </script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery-validation-unobtrusive/3.2.12/jquery.validate.unobtrusive.min.js"
            asp-fallback-src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"
            asp-fallback-test="window.jQuery && window.jQuery.validator && window.jQuery.validator.unobtrusive"
            asp-append-version="true"
            crossorigin="anonymous"
            integrity="sha512-o6XqxgrUsKmchwy9G5VRNWSSxTS4Urr4loO6/0hYdpWmFUfHqGzawGxeQGMDqYzxjY9sbktPbNlkIQJWagVZQg==">
    </script>
</environment>
```

## Додавання та налаштування WebOptimizer

Відкрийте файл Program.cs у проекті AutoLot.Web та додайте наступний рядок (безпосередньо перед викликом app.UseStaticFiles()):

```cs
app.UseWebOptimizer();
```

Наступний крок – налаштувати, що мінімізувати та об’єднувати в пакет. Бібліотеки з відкритим кодом вже мають мінімізовані версії, завантажені через Library Manager, тому мінімізувати потрібно лише файли, специфічні для проекту, включаючи згенерований CSS-файл, якщо ви використовуєте ізоляцію CSS. У файлі Program.cs додайте наступний блок коду перед var app = builder.Build():

```cs
if (builder.Environment.IsDevelopment() || builder.Environment.IsEnvironment("Local"))
{
    //builder.Services.AddWebOptimizer(false,false);
    builder.Services.AddWebOptimizer(options =>
    {
        options.MinifyCssFiles("AutoLot.Web.styles.css");
        options.MinifyCssFiles("/css/site.css"); 
        options.MinifyJsFiles("/js/site.js"); 

        //options.AddJavaScriptBundle("/js/validationCode.js", "/js/validations/**/*.js");
        options.AddJavaScriptBundle("/js/validationCode.js", "js/validations/validators.js", "js/validations/errorFormatting.js");
    });
}
else
{
    builder.Services.AddWebOptimizer(options =>
    {
        //options.MinifyCssFiles(); //Minifies all CSS files
        //options.MinifyJsFiles(); //Minifies all JS files
        //options.MinifyCssFiles("css/site.cs"); //Minifies the site.css file
        //options.MinifyCssFiles("lib/**/*.cs"); //Minifies all CSS files
        //options.MinifyJsFiles("js/site.js"); //Minifies the site.js file
        //options.MinifyJsFiles("lib/**/*.js"); //Minifies all JavaScript file under the wwwroot/lib directory

        //options.MinifyJsFiles("js/site.js");
        //options.MinifyJsFiles("lib/**/*.js");

        options.MinifyCssFiles("AutoLot.Web.styles.css");
        options.MinifyCssFiles("cs/site.cs");
        options.MinifyJsFiles("js/site.js");

        //options.AddJavaScriptBundle("js/validations/validationCode.js", "js/validations/**/*.js");
        options.AddJavaScriptBundle("/js/validationCode.js", "js/validations/validators.js", "js/validations/errorFormatting.js");
    });
}

```
У блоці Development код налаштовано для comment/uncomment різних опцій, щоб ви могли відтворити робоче середовище без переходу до робочого середовища. Останній крок – додати до системи помічники тегів WebOptimizer. Додайте наступний рядок у кінець файлу _ViewImports.cshtml:

```html
@addTagHelper *, WebOptimizer.Core
```

# Тег-хелпери

Представлення Razor Pages (а також представлення макета та часткові представлення) також підтримують помічники тегів. Будь-який тег-хелпер, що бере участь у маршрутизації, використовує атрибути, орієнтовані на сторінку, замість атрибутів, орієнтованих на контролер. У таблиці перелічено тег-хелпери, які використовують маршрутизацію, їхній відповідний помічник HTML та доступні атрибути сторінки Razor. Відмінності будуть детально розглянуті після таблиці.

|Тег-хелпер|Html-хелпер|Доступні атрибути|
|----------|-----------|-----------------|
|Form|Html.BeginForm Html.BeginRouteForm Html.AntiForgeryToken|asp-route — для іменованих маршрутів (не можна використовувати з атрибутами контролера, сторінки або дії). asp-antiforgery — якщо слід додати захист від підробки (за замовчуванням true). asp-area — назва області. asp-route-\<ParameterName\> — додає параметр до маршруту, наприклад, asp-route-id="1".asp-page — назва сторінки Razor. asp-page-handler — назва обробника сторінки Razor. asp-all-route-data — словник для додаткових значень маршруту.|
|Form Action
(button or input type=image)|N/A|asp-route — для іменованих маршрутів (не можна використовувати з атрибутами контролера, сторінки або дії). asp-antiforgery — якщо слід додати захист від підробки (за замовчуванням true). asp-area — назва області. asp-route-\<ParameterName\> — додає параметр до маршруту, наприклад, asp-route-id="1".asp-page — назва сторінки Razor. asp-page-handler — назва обробника сторінки Razor. asp-all-route-data — словник для додаткових значень маршруту|
|Anchor|Html.ActionLink|asp-route — для іменованих маршрутів (не можна використовувати з атрибутами контролера, сторінки або дії). asp-area — назва області. asp-protocol — HTTP або HTTPS. asp-fragment — фрагмент URL-адреси. asp-host — ім'я хоста. asp-route-\<ParameterName\> — додає параметр до маршруту, наприклад, asp-route-id='1'. asp-page — назва сторінки Razor. asp-page-handler — назва обробника сторінки Razor. asp-all-route-data — словник для додаткових значень маршруту.|

## Увімкнення тег-хелперів
Тег-хелпери необхідно ввімкнути у вашому проєкті у файлі _ViewImports.html, додавши наступний рядок (який був доданий шаблоном за замовчуванням):

```html
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
```

## Тег-хелпер form

У Razor Pages тег-хелпер <form> використовує asp-page замість asp-controller та asp-action:

```html
<form method="post" asp-page="Edit" asp-route-id="@Model.Entity.Id" >
<!-- Omitted for brevity -->
</form>
```
Іншим доступним варіантом є вказати назву методу обробника сторінки. Під час змінення назви необхідно дотримуватися формату On\<Verb\>\<CustomName\>() (OnPostCreateNewCar()) або On\<Verb\>\<CustomName\>Async() (OnPostCreateNewCarAsync()). Після перейменування методу HTTP post, метод обробника визначається так:

```html
<form method="post" asp-page="Create" asp-page-handler="CreateNewCar">
<!-- Omitted for brevity -->
</form>
```
Усі методи обробника HTTP-post Razor Page автоматично перевіряють наявність токена захисту від підробок, який додається щоразу, коли використовується тег-хелпер \<from\>.

## Тег-хелпер form action Button/Image

Допоміжний тег дії форми використовується для кнопок і зображень, щоб змінити дію для форми, яка їх містить, і підтримує атрибути asp-page та asp-page-handler так само, як і тег-хелпер \<form\>.

```html
<button type="submit" asp-page="Create">Index</button>
```
## Тег-хелпер Anchor

Тег-хелпер \<anchor\> замінює HTML-хелпер Html.ActionLink та використовує багато тих самих тегів маршрутизації, що й допоміжний елемент тегу \<form\>. Наприклад, щоб створити посилання для представлення RazorSyntax, використовуйте наступний код:

```html
<a asp-area="" asp-page="/RazorSyntax">Razor Syntax</a>
```
Щоб додати елемент меню навігації для сторінки RazorSyntax, оновіть _Menu.cshtml до наступного, додавши новий елемент меню між елементами меню  Home та Privacy:

```html
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-page="/RazorSyntax">RazorSyntax</a>
                </li>
```
Тег-хелпер \<anchor\> можна поєднати з моделлю переглядів. Наприклад, використовуючи екземпляр Car на сторінці RazorSyntax, наступний тег прив'язки перенаправляє на сторінку Details, передаючи Id як параметр маршруту:

```html
<a asp-page="Cars/Details" asp-route-id="@Model.Entity.Id">@Model.Entity.PetName</a>
```

Якшо цей код додати зараз він не буде створювати відповідне посилання, бо немає сторінки Details.

# Кастомні тег-хелпери

Створення спеціальних тег-хелперів для Razor Pages дуже схоже на їх створення для програм MVC. Обидва вони успадковують від TagHelper і повинні реалізовувати метод Process(). Для AutoLot.Web відмінність від версії MVC полягає в тому, як створюються посилання, оскільки маршрутизація відрізняється. Перш ніж розпочати, додайте наступний оператори using до файлу GlobalUsings.cs:

```cs
global using Microsoft.AspNetCore.Mvc.Infrastructure;
global using Microsoft.AspNetCore.Mvc.Routing;
global using Microsoft.AspNetCore.Razor.TagHelpers;
global using Microsoft.Extensions.DependencyInjection.Extensions;
```
## Оновлення Program.cs

Нам знову потрібно використовувати UrlHelperFactory та IActionContextAccessor для створення посилань на основі маршрутизації. Знову ж таки, нам потрібно використовувати UrlHelperFactory та IActionContextAccessor для створення посилань на основі маршрутизації. Щоб створити екземпляр UrlFactory з класу, що не походить від PageModel, до колекції services потрібно додати IActionContextAccessor. Викличте наступний рядок у Program.cs, щоб додати IActionContextAccessor до колекції служб:

```cs
builder.Services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
```

## Створення базового класу

Створіть нову папку з назвою TagHelpers у кореневому каталозі проекту AutoLot.Web. У цій папці створіть нову папку з назвою Base, а в цій папці створіть клас з назвою ItemLinkTagHelperBase.cs, зробіть клас публічним та абстрактним і успадкуйте його від TagHelper:

```cs
namespace AutoLot.Web.TagHelpers.Base;

public abstract class ItemLinkTagHelperBase : TagHelper
{
}
```
Додайте конструктор, який приймає екземпляри IActionContextAccessor та IUrlHelperFactory.
Використайте UrlHelperFactory разом з ActionContextAccessor для створення екземпляра IUrlHelper та збереження його у змінній рівня класу:

```cs
    protected ItemLinkTagHelperBase(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
    {
        UrlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
    }
```
У конструкторі використовуйте екземпляр contextAccessor, щоб отримати поточну сторінку Page та призначити її полю рівня класу. Значення маршруту сторінки має вигляд \<directory\>/\<PageName\>, подібно до Cars/Index: Рядок розділяється, щоб отримати лише назву каталогу:

```cs
    private readonly string _pageName;

    protected ItemLinkTagHelperBase(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
    {
        UrlHelper = urlHelperFactory.GetUrlHelper(contextAccessor.ActionContext);
        _pageName = contextAccessor.ActionContext.ActionDescriptor.RouteValues["page"]?.Split("/", StringSplitOptions.RemoveEmptyEntries)[0];
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
Нагадуємо, що публічні властивості кастомних тег-хелперів відображаються як атрибути HTML тегу. Згідно з домовленістю про іменування, назва властивості перетворюється на lower-kabob-casing. Це означає, що кожна велика літера пишеться з малої, а перед кожною літерою, яка змінюється на малу (окрім першої), вставляються тире (-). Це перетворює ItemId на item-id (як слова на шашлику).
Метод BuildContent() викликається похідними класами для побудови HTML-коду, який відображатиметься, замість тег-хелпера:

```cs
    protected void BuildContent(TagHelperOutput output,
        string cssClassName, string displayText, string fontAwesomeName)
    {
        output.TagName = "a";
        var target = (ItemId.HasValue) ? UrlHelper.Page($"/{_pageName}/{ActionName}", new { id = ItemId }) : UrlHelper.Page($"/{_pageName}/{ActionName}");
        output.Attributes.SetAttribute("href", target);
        output.Attributes.Add("class", cssClassName);
        output.Content.AppendHtml($@"{displayText} <i class=""fas fa-{fontAwesomeName}""></i>");
    }
```
Перший рядок змінює тег на тег anchor. Наступний рядок використовує статичний метод UrlHelper.Page() для генерації маршруту, включаючи параметр маршруту, якщо такий існує. Наступні два встановлюють HREF тегу anchor на згенерований маршрут і додають назву класу CSS. В останньому рядку додається текст, що відображається, та шрифт Font Awesome як текст, який відображається користувачеві. Як останній крок, додайте наступний глобальний оператор using до файлу GlobalUsings.cs:

```cs
global using AutoLot.Web.TagHelpers.Base;
```

## Тег-хелпер Item Details

Створіть новий клас з назвою ItemDetailsTagHelper.cs у папці TagHelpers. Зробіть клас публічним та успадкуйте його від ItemLinkTagHelperBase.

```cs
namespace AutoLot.Web.TagHelpers;

public class ItemDetailsTagHelper : ItemLinkTagHelperBase
{

}

```
Додайте конструктор для прийняття необхідних екземплярів об'єктів та передачі їх базовому класу. Конструктору також потрібно призначити ActionName:

```cs
    public ItemDetailsTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
        : base(contextAccessor, urlHelperFactory)
    {
        ActionName = "Details";
    }
```
Перевизначте метод Process(), викликавши метод BuildContent() у базовому класі.

```cs
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output, "text-info", "Details", "info-circle");
    }
```
Під час виклику тег-хелпера суфікс TagHelper пропускається, а решта назви класу починається з малої літери шашликом. У цьому випадку тегом HTML є \<item-details\>. Значення asp-route-id походить з атрибута item-id тег-хелпера:

```html
<item-details item-id='@item.Id'></item-details>
```
Це створює посилання Details за допомогою класу CSS text-info, тексту Details та зображення Font Awesome info. Один із варіантів такий:

```html
<a asp-page="Cars/Details" asp-route-id="5" class="text-info">Details <i class="fas fa-info-circle"></i></a>
```

## Тег-хелпер Item Delete

Створіть новий клас з назвою ItemDeleteTagHelper.cs у папці TagHelpers. Додайте конструктор для прийняття потрібних екземплярів об'єктів та встановіть ActionName, використовуючи назву методу :

```cs
namespace AutoLot.Web.TagHelpers;

public class ItemDeleteTagHelper : ItemLinkTagHelperBase
{
    public ItemDeleteTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
        : base(contextAccessor, urlHelperFactory)
    {
        ActionName = "Delete";
    }
}
```
Перевизначте метод Process(), викликавши метод BuildContent() у базовому класі.

```cs
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output, "text-danger", "Delete", "trash");
    }
```

## Тег-хелпер Item Edit

Створіть новий клас з назвою ItemEditTagHelper.cs у папці TagHelpers. Зробіть клас публічним, успадкуйте від ItemLinkTagHelperBase та додайте конструктор, який призначає Edit як ActionName:

```cs
namespace AutoLot.Web.TagHelpers;

public class ItemEditTagHelper : ItemLinkTagHelperBase
{
    public ItemEditTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
        : base(contextAccessor, urlHelperFactory)
    {
        ActionName = "Edit";

    }
}
```
Перевизначте метод Process(), викликавши метод BuildContent() у базовому класі.

```cs
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output, "text-warning", "Edit", "edit");
    }
```

## Тег-хелпер Item Create

Створіть новий клас з назвою ItemCreateTagHelper.cs у папці TagHelpers. Зробіть клас публічним, успадкуйте його від ItemLinkTagHelperBase та додайте конструктор, який призначає Create як ActionName:

```cs
namespace AutoLot.Web.TagHelpers;

public class ItemCreateTagHelper : ItemLinkTagHelperBase
{
    public ItemCreateTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
        : base(contextAccessor, urlHelperFactory)
    {
        ActionName = "Create";
    }
}
```
Перевизначте метод Process(), викликавши метод BuildContent() у базовому класі.

```cs
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output, "text-success", "Create New", "plus");
    }
```

## Тег-хелпер Item List

Створіть новий клас з назвою ItemListTagHelper.cs у папці TagHelpers. Зробіть клас публічним, успадкуйте його від ItemLinkTagHelperBase та додайте конструктор, який призначає Index як ActionName:

```cs
namespace AutoLot.Web.TagHelpers;

public class ItemListTagHelper : ItemLinkTagHelperBase
{
    public ItemListTagHelper(IActionContextAccessor contextAccessor, IUrlHelperFactory urlHelperFactory)
        : base(contextAccessor, urlHelperFactory)
    {
        ActionName = "Index";

    }

}
```
Перевизначте метод Process(), викликавши метод BuildContent() у базовому класі.

```cs
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        BuildContent(output, "text-default", "Back to List", "list");
    }

```
У дії Index немає параметра маршруту:

```html
<item-list></item-list>
```

## Відображення користувацьких тег-хелперів
Щоб відобразити користувацькі помічники тегів, необхідно виконати команду @addTagHelper для будь-яких представлень, які використовують помічники тегів або додаються до файлу _ViewImports.cshtml. Відкрийте файл _ViewImports.cshtml у кореневому каталозі папки Views та додайте наступний рядок:

```html
@addTagHelper *, AutoLot.Web
```

# Cars Razor Pages

Далі ми створимо базовий клас, який оброблятиме спільний код на всіх сторінках. Перш ніж почати, додайте наступний оператор using до файлу GlobalUsings.cs:

```cs
global using AutoLot.Models.Entities.Base;
```

# Клас BasePageModel

Додайте новий каталог з назвою Base в каталог Pages. У цьому новому каталозі додайте новий клас з назвою BasePageModel. Зробіть його абстрактним та узагальненим (візьміть тип сутності для доступу до даних та тип класу для логування) та успадкуйте від PageModel:

```cs
namespace AutoLot.Web.Pages.Base;

public abstract class BasePageModel<TEntity,TPageModel> : PageModel
    where TEntity : BaseEntity, new()
    where TPageModel : PageModel
{

}
```
Далі додайте захищений конструктор, який приймає екземпляр IAppLogging<TPageModel>, екземпляр IDataServiceBase<TEntity> та рядок для заголовка сторінки. Екземпляри інтерфейсу призначаються захищеним полям класу, а рядок призначається властивості Title ViewData.

```cs
    protected readonly IAppLogging<TPageModel> AppLoggingInstance;
    protected readonly IDataServiceBase<TEntity> DataService;

    [ViewData]
    public string Title { get; init; }

    protected BasePageModel(IAppLogging<TPageModel> appLogging, IDataServiceBase<TEntity> dataService, string pageTitle)
    {
        AppLoggingInstance = appLogging;
        DataService = dataService;
        Title = pageTitle;
    }
```
Базовий клас має три публічні властивості. Екземпляр TEntity, який є BindProperty, SelectList для значень пошуку та властивість Error для відображення повідомлення у вигляді банера помилки у перегляді:

```cs
    [BindProperty]
    public TEntity Entity { get; set; }
    public SelectList LookupValues { get; set; }
    public string Error { get; set; }
```

Далі додайте метод, який приймає екземпляр IDataServiceBase, імена властивостей dataValue та dataText та створює SelectList:

```cs
    protected async Task GetLookupValuesAsync<TLookupEntity>(
        IDataServiceBase<TLookupEntity> lookupService, string lookupKey, string lookupDisplay)
        where TLookupEntity : BaseEntity, new()
    {
        LookupValues = new(await lookupService.GetAllAsync(), lookupKey, lookupDisplay);
    }
```
Перш ніж продовжити будувати базовий клас протестуємо шо в ньому вже є. Ми вже можемо використвоуючи його щоб вивести всі записи Car.

Додамо в GlobalUsings

```cs
global using AutoLot.Web.Pages.Base;
```

## Код Razor Page Index для Cars

На сторінці Index буде показано список записів Car та посилання на інші сторінки CRUD. Список міститиме або всі записи, або лише ті, що мають певне значення Make. Нагадаємо, що в маршрутизації Razor Page сторінка Index Razor є сторінкою за замовчуванням для каталогу, до якої можна дістатися з URL-адрес /Cars та /Cars/Index, тому додаткова маршрутизація не потрібна (на відміну від версії MVC). 
Почніть з додавання порожньої сторінки Razor з назвою Index.cshtml до каталогу Pages\Cars. Перевизначте клас IndexModel. Додайте конструктор, який отримує екземпляри IAppLogging<IndexModel> та ICarDataService та пердає в батьківський клас:

```cs
namespace AutoLot.Web.Pages.Cars;
public class IndexModel : BasePageModel<Car, IndexModel>
{
    public IndexModel(IAppLogging<IndexModel> appLogging, ICarDataService dataService) : base(appLogging, dataService, "Index")
    {
    }
}

```
Додайте три публічні властивості до класу. Два містять властивості MakeName та MakeId, що використовуються списком автомобілів за маркою, а третій містить фактичний список записів Car.

```cs
public string MakeName { get; set; }
public int? MakeId { get; set; }
public IEnumerable<Car> CarRecords { get; set; }
```
Метод HTTP get приймає додаткові параметри для makeId та makeName, а потім встановлює значення цих параметрів для публічних властивостей (навіть якщо вони дорівнюють null). Параметри є частиною маршруту, який буде оновлено разом із переглядом. Потім він викликає метод GetAllByMakeIdAsync() служби даних, який поверне всі записи, якщо makeId має значення null, інакше він поверне лише записи Car для цього Marke:

```cs
    public async Task OnGetAsync(int? makeId, string makeName)
    {
        MakeId = makeId;
        MakeName = makeName;
        CarRecords = await ((ICarDataService)DataService).GetAllByMakeIdAsync(makeId);
    }
```
## Частковий перегляд списку автомобілів

Для сторінки Index доступні два режими перегляду. В одному показано всі автомобілі, а в іншому — список автомобілів за маркою. Оскільки інтерфейс користувача той самий, списки будуть відображатися з використанням часткового представлення. Це часткове представлення таке ж, як і для MVC-застосунку, демонструючи підтримку часткових представлень між фреймворками.
Створіть новий каталог з назвою Partials у каталозі Pages\Cars. У цьому каталозі додайте нове представлення з назвою _CarListPartial.cshtml та видаліть наявний код. Встановіть тип IEnumerable<Car> та додайте блок Razor, щоб визначити, чи слід відображати марки.
Коли цей частковий елемент використовується всім списком інвентарю, повинні відображатися марки. Якщо відображається лише один бренд, поле Make слід приховати, оскільки воно буде в заголовку сторінки.

```html
@model IEnumerable<Car>
@{
    var showMake = true;
    if (bool.TryParse(ViewBag.ByMake?.ToString(), out bool byMake))
    {
        showMake = !byMake;
    }
}
```
У наступній розмітці використовується ItemCreateTagHelper для створення посилання на метод Create HTTP Get (нагадаємо, що допоміжні функції тегів пишуться з малого регістру шашликом під час використання у представленнях Razor).

```html
<p>
    <item-create></item-create>
</p>
```
Додамо таблицю.

```html
<table class="table">
</table>
```
У заголовках таблиць використовується HTML-хелпер Razor для отримання DisplayName для кожної властивості. У цьому розділі використовується блок Razor для відображення інформації Make на основі змінної рівня перегляду, встановленої раніше.

```html
    <thead>
    <tr>
        <th>@Html.DisplayNameFor(model => model.Id)</th>
        @if (showMake)
        {
            <th>@Html.DisplayNameFor(model => model.MakeId)</th>
        }
        <th>@Html.DisplayNameFor(model => model.Color)</th>
        <th>@Html.DisplayNameFor(model => model.PetName)</th>
        <th>@Html.DisplayNameFor(model => model.Price)</th>
        <th>@Html.DisplayNameFor(model => model.DateBuilt)</th>
        <th>@Html.DisplayNameFor(model => model.IsDrivable)</th>
        <th></th>
    </tr>
    </thead>
```
В тілі таблиці перебираються записи та відображаються записи таблиці за допомогою HTML-хелпера DisplayFor Razor. Цей блок також використовує кастомні тег-хелпери item-edit, item-details та item-delete. 

```html
    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>@Html.DisplayFor(modelItem => item.Id)</td>
                @if (showMake)
                {
                    <td>@Html.DisplayFor(modelItem => item.MakeNavigation.Name)</td>
                }
                <td>@Html.DisplayFor(modelItem => item.Color)</td>
                <td>@Html.DisplayFor(modelItem => item.PetName)</td>
                <td>@Html.DisplayFor(modelItem => item.Price)</td>
                <td>@Html.DisplayFor(modelItem => item.DateBuilt)</td>
                <td>@Html.DisplayFor(modelItem => item.IsDrivable)</td>
                <td>
                    <item-edit item-id="@item.Id"></item-edit> |
                    <item-details item-id="@item.Id"></item-details> |
                    <item-delete item-id="@item.Id"></item-delete>
                </td>
            </tr>
        }
    </tbody>
```

## Перегляд Razor Page Index для Cars

З використанням часткового елемента _CarListPartial, представлення сторінки Index Razor досить мале, що демонструє перевагу використання часткових представлень для зменшення повторюваної розмітки. Перший крок – оновити інформацію про маршрут, додавши два необов’язкові токени маршруту до директиви @page:

```html
@page "{makeId?}/{makeName?}"
@model AutoLot.Web.Pages.Cars.IndexModel
```
Наступний крок – визначити, чи має значення MakeId, що може мати значення null, у IndexModel. Нагадаємо, що MakeId та MakeName оновлюються в методі OnGetAsync() на основі параметрів маршруту. Якщо є значення, відобразіть MakeName у заголовку та створіть новий ViewDataDictionary, що містить властивість ByMake. Якщо є значення, відобразіть MakeName у заголовку та створіть новий ViewDataDictionary, що містить властивість ByMake. Потім це передається в частковий вигляд разом із властивістю моделі CarRecords, обидві з яких використовуються частковим виглядом _CarListPartial.Якщо MakeId не має значення, викличте частковий вигляд _CarListPartial з властивістю CarRecords, але без ViewDataDictionary:

```html
@{ 
    if (Model.MakeId.HasValue)
    {
        <h1>Vehicle Inventory for @Model.MakeName</h1>
        var mode = new ViewDataDictionary(ViewData) { { "ByMake", true } };
        <partial name="Partials/_CarListPartial" model="@Model.CarRecords" view-data="@mode" />
    }
    else
    {
        <h1>Vehicle Inventory</h1>
        <partial name="Partials/_CarListPartial" model="@Model.CarRecords" />
    }
}
```
Щоб побачити цей вигляд у дії, запустіть програму та перейдіть за посиланням https://localhost:5021/Cars/ або https://localhost:5021/Cars/Index, щоб переглянути повний список транспортних засобів. Щоб переглянути список автомобілів BMW, перейдіть за посиланням https://localhost:5021/Cars/5/BMW (або https://localhost:5021/Cars/Index/5/BMW).

Додамо в частковий перегляд _Menu.cshtml

```html
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-page="/Cars/Index">Cars</a>
                </li>
```

## Метод GetOneAsync() класу BasePageModel

Повернемося до побудови базового класу. Метод GetOneAsync() намагається отримати запис TEntity за Id. Якщо параметр id має значення null або запис не вдається знайти, встановлюється властивість Error. В іншому випадку запис присвоюється властивості Entity BindProperty:

```cs
    protected async Task GetOneAsync(int? id)
    {
        if (!id.HasValue)
        {
            Error = "Invalid request";
            Entity = null;
            return;
        }

        Entity = await DataService.FindAsync(id.Value);
        if (Entity == null)
        {
            Error = "Not found";
            return;
        }

        Error = string.Empty;
    }
```

## Код Razor Page Details для Cars

Сторінка Details використовується для відображення одного запису Car під час виклику із HTTP-запитом get. В каталог Pages\Cars додайте Razor Page Details. Маршрут розширюється необов’язковим значенням id. Оновіть код до наступного, який використовує методи батьківського класу BasePageModel:

```cs
public class DetailsModel : BasePageModel<Car, DetailsModel>
{
    public DetailsModel(IAppLogging<DetailsModel> appLogging,
        ICarDataService dataService) : base(appLogging, dataService, "Details")
    {
    }

    public async Task OnGetAsync(int? id)
    {
        await GetOneAsync(id);
    }
}
```
Конструктор приймає екземпляри IAppLogging\<T\> та ICarDataService і передає їх базовому класу разом із заголовком сторінки. Метод обробника сторінок OnGetAsync() приймає необов'язковий параметр маршруту, а потім викликає базовий метод GetOneAsync(). Оскільки метод не повертає IActionResult, представлення відтворюється після завершення роботи методу.

## Перегляд Razor Page Cars Details

Перший крок – оновити маршрут, включивши необов’язковий токен маршруту id та додати заголовок:

```html
@page "{id?}"
@model AutoLot.Web.Pages.Cars.DetailsModel
```
Якщо у властивості Errors є значення, то повідомлення потрібно відобразити у банері. Якщо значення помилки немає, скористайтеся шаблоном відображення автомобіля, щоб відобразити інформацію про записи. На закінчені створіт посилання за допомогою кастомних тег-хелперів:

```html
@if (!string.IsNullOrEmpty(Model.Error))
{
    <div class="alert alert-danger" role="alert">
        @Model.Error
    </div>
}
else
{
    <h1>Details for @Model.Entity.PetName</h1>
    @Html.DisplayFor(m => m.Entity)
    @*@Html.DisplayFor(m => m.Entity, "CarWithColors")*@

    <div>
        <item-edit item-id="@Model.Entity.Id"></item-edit> |
        <item-delete item-id="@Model.Entity.Id"></item-delete> |
        <item-list></item-list>
    </div>
}
```
Запустимо додаток і перейдемо за посиланням https://localhost:5021/Cars/Details/7

Рядок @Html.DisplayFor() можна замінити на @Html.DisplayFor(m=>m.Entity,"CarWithColors") для відображення шаблону, який використовує колір при відображенні.

## Метод SaveOneAsync() та SaveWithLookupAsync() класу BasePageModel

Продовжимо будувати базовий клас BasePageModel. Метод SaveOneAsync() перевіряє валідність ModelState, а потім намагається зберегти або оновити запис. Якщо ModelState недійснє, дані відображаються у перегляді для виправлення користувачем. Якщо під час виклику збереження/оновлення виникає виняток, повідомлення про виняток додається до властивості Error та ModelState, а потім представлення повертається користувачеві. Метод приймає Func\<TEntity, bool, Task\<TEntity\>\>, тому його можна викликати як для AddAsync(), так і для UpdateAsync():

```cs
    protected virtual async Task<IActionResult> SaveOneAsync(Func<TEntity, bool, Task<TEntity>> persistenceTask)
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            await persistenceTask(Entity, true);
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            ModelState.AddModelError(string.Empty, ex.Message);
            AppLoggingInstance.LogAppError(ex, "An error occurred");
            return Page();
        }

        return RedirectToPage("./Details", new { id = Entity.Id });
    }
```
Цей метод буде протестован в розділі Area.

Метод SaveWithLookupAsync() виконує той самий процес, що й SaveOneAsync(), але також повторно заповнює SelectList за потреби. Для отримання даних для значень пошуку використовується служба даних, а для створення списку вибору (SelectList) – імена властивостей dataValue та dataText:

```cs
    protected virtual async Task<IActionResult> SaveWithLookupAsync<TLookupEntity>(
        Func<TEntity, bool, Task<TEntity>> persistenceTask, 
        IDataServiceBase<TLookupEntity> lookupService,string lookupKey, string lookupDisplay)
        where TLookupEntity : BaseEntity, new()
    {
        if (!ModelState.IsValid)
        {
            await GetLookupValuesAsync(lookupService, lookupKey, lookupDisplay);
            return Page();
        }

        try
        {
            await persistenceTask(Entity, true);
        }
        catch (Exception ex)
        {
            Error = ex.Message;
            ModelState.AddModelError(string.Empty, ex.Message);
            await GetLookupValuesAsync(lookupService, lookupKey, lookupDisplay);
            AppLoggingInstance.LogAppError(ex, "An error occurred");
            return Page();
        }

        return RedirectToPage("./Details", new { id = Entity.Id });
    }

```

Маючи ці методи в базовому класі ми можемо їх протестувати в дочірніх класах.

## Код Razor Page Create для Cars

Створіть в каталозі Cars Razor Page з назвою Create. Сторінка успадковується від BasePageModel. Видаліть сформований код та замініть його наступним:

```cs
namespace AutoLot.Web.Pages.Cars;

public class CreateModel : BasePageModel<Car, CreateModel>
{


}
```
Окрім IAppLogging\<T\> та ICarDataService для базового класу, конструктор приймає екземпляр IMakeDataService та призначає його полю рівня класу:

```cs
    private readonly IMakeDataService _makeService;
    public CreateModel(
        IAppLogging<CreateModel> appLogging, 
        ICarDataService carService, IMakeDataService makeService) 
        : base(appLogging, carService, "Create")
    {
        _makeService = makeService;
    }
```
Метод обробника HTTP get заповнює властивість LookupValues. Оскільки повертаного значення немає, представлення відтворюється після завершення методу:

```cs
    public async Task OnGetAsync()
    {
        await GetLookupValuesAsync(_makeService, nameof(Make.Id), nameof(Make.Name));
    }
```
Метод обробника HTTP-post використовує базовий метод SaveWithLookupAsync(), а потім повертає IActionResult з базового методу. Зверніть увагу на нестандартну назву методу. Це буде вирішено у формі перегляду за допомогою тег-хелпера \<form\>:

```cs
    public async Task<IActionResult> OnPostCreateNewCarAsync()
    {
        return await SaveWithLookupAsync(
            DataService.AddAsync,
            _makeService, 
            nameof(Make.Id),
            nameof(Make.Name));
    }
```
## Перегляд Razor Page Create для Cars

Представлення використовує базовий маршрут, тому не потрібно вносити жодних змін до директиви @page. Додайте заголовок та блок помилки:

```html
@page
@model AutoLot.Web.Pages.Cars.CreateModel
<h1>Create a New Car</h1>
<hr/>
@if (!string.IsNullOrEmpty(Model.Error))
{
    <div class="alert alert-danger" role="alert">
        @Model.Error
    </div>
}
else
{
    
}
```
Тег \<form\> використовує два тег-хелпери. Тег-хелпер asp-page встановлює дію форми на відправку даних назад до маршруту Create (у поточному каталозі, яким є Cars). Тег-хелпер asp-page-handler вказує назву методу без префікса OnPost та суфікса Async. Пам’ятайте, що якщо використовуються тег-хелпери \<form\>, токен захисту від підробки автоматично додається до даних форми:

```html
    <form asp-page="Create" asp-page-handler="CreateNewCar">

    </form>
```
Вміст форми здебільшого складається шаблонно. Два рядки, на які варто звернути увагу, це допоміжний засіб тегу asp-validation-summary та  HTML EditorFor(). Метод EditorFor() викликає шаблон редактора для класу Car. Другий параметр додає SelectList до ViewBag. У зведенні перевірки відображаються лише помилки на рівні моделі, оскільки шаблон редактора показує помилки на рівні поля:


```html
        <div class="row">
            <div class="col-md-4">
                <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                @Html.EditorFor(x => x.Entity, new { LookupValues = Model.LookupValues })
                <input type="hidden" asp-for="Entity.TimeStamp" value=" "/> 
            </div>
        </div>
        <div class="d-flex flex-row mt-3">
            <button type="submit" class="btn btn-success">Create <i class="fas fa-plus"></i></button>&nbsp;&nbsp;|&nbsp;&nbsp;
            <item-list></item-list>
        </div>
```
Останнім оновленням є додавання частини _ValidationScriptsPartial до розділу Scripts. Нагадаємо, що у макеті цей розділ з'являється після завантаження jQuery. Шаблон розділів допомагає забезпечити завантаження належних залежностей перед вмістом розділу:

```html
@section Scripts {
    <partial name="_ValidationScriptsPartial"/>
}
```
Перш ніж запускати додаток на перевірку треба в файлі _ValidationScriptsPartial.cshtml закоментувати посилання на файл який ми створимо пізніше.

```html
    @* <script src="~/js/validationCode.js"></script> *@
```
Тепер можна запустити додаток и додати запис.

Таким чином ми перевірили роботу метода SaveWithLookupAsync базового класу при додавані нового запису з допомогою DataService.AddAsync. Оскільки метод базового класу SaveWithLookupAsync буде оновлювати записи,  xтворимо сторінку для оновлення.

## Код Razor Page Edit для Cars

Сторінка редагування Razor дотримується того ж шаблону, що й сторінка створення Razor. Вона успадковується від BasePageModel. Очистіть сформований код і замініть його наступним:

```cs
namespace AutoLot.Web.Pages.Cars;

public class EditModel : BasePageModel<Car,EditModel>
{

}

```
Окрім IAppLogging\<T\> та ICarDataService для базового класу, конструктор приймає екземпляр IMakeDataService та призначає його полю рівня класу:

```cs
    public EditModel(IAppLogging<EditModel> appLogging, 
        ICarDataService carService, 
        IMakeDataService makeService) 
        : base(appLogging, carService, "Edit")
    {
        _makeService = makeService;
    }
```
Метод обробника HTTP get заповнює властивість LookupValues ​​та намагається отримати сутність. Оскільки повертаного значення немає, представлення відтворюється після завершення методу:

```cs
    public async Task OnGetAsync(int? id)
    {
        await GetLookupValuesAsync(_makeService, nameof(Make.Id), nameof(Make.Name));
        await GetOneAsync(id);
    }
```
Метод обробника HTTP-post використовує базовий метод SaveWithLookupAsync(), а потім повертає IActionResult з базового методу:

```cs
    public async Task<IActionResult> OnPostAsync()
    {
        return await SaveWithLookupAsync(
            DataService.UpdateAsync,
            _makeService, 
            nameof(Make.Id),
            nameof(Make.Name));
    }
```
## Перегляд Razor Page Edit для Cars

Представлення приймає необов'язковий ідентифікатор як токен маршруту, який додається до директиви @page.

```html
@page "{id?}"
```
Додайте блок помилки:

```html
@if (!string.IsNullOrEmpty(Model.Error))
{
    <div class="alert alert-danger" role="alert">
        @Model.Error
    </div>
}
else
{
    
}
```
Тег \<form\> використовує два тег-хелпери. Тег-хелпер asp-page встановлює дію форми на відправку назад до маршруту Edit (у поточному каталозі, яким є Cars). Тег-хелпер asp-route-id визначає значення параметра id маршруту:

```html
@if (!string.IsNullOrEmpty(Model.Error))
{
    //...
}
else
{
    <form asp-page="Edit" asp-route-id="@Model.Entity.Id">
    </form>
}
```

Вміст форми здебільшого складається з макета. Чотири рядки, на які варто звернути увагу, це тег-хелпер asp-validation-summary, HTML-хелпер EditorFor() та два приховані теги input. Метод EditorFor() викликає шаблон редагування для класу Car. Другий параметр додає SelectList до ViewBag. У зведенні перевірки відображаються лише помилки на рівні моделі, оскільки шаблон редактора показує помилки на рівні поля. Два приховані теги введення містять значення властивостей Id та TimeStamp, які необхідні для процесу оновлення, але не мають значення для користувача:

```html
    <form asp-page="Edit" asp-route-id="@Model.Entity.Id">
        <div class="row">
            <div class="col-md-4">
                <div asp-validation-summary="ModelOnly"></div>
                @Html.EditorFor(x => x.Entity, new { LookupValues = Model.LookupValues })
                <input type="hidden" asp-for="Entity.Id"/>
                <input type="hidden" asp-for="Entity.TimeStamp"/>
            </div>
        </div>
        <div class="d-flex flex-row mt-3">
            <button type="submit" class="btn btn-primary">Save <i class="fas fa-save"></i></button>&nbsp;&nbsp;|&nbsp;&nbsp;
            <item-list></item-list>
        </div>
    </form>
```
Останнім оновленням є додавання частки  _ValidationScriptsPartial до розділу Scripts. Нагадаємо, що у макеті цей розділ з'являється після завантаження jQuery. Шаблон розділів допомагає забезпечити завантаження належних залежностей перед вмістом розділу:

```html
@section Scripts {
    @{ await Html.RenderPartialAsync("_ValidationScriptsPartial"); }
}
```
Тепер можна первірить роботу сторінки.

## Метод DeleteOneAsync() класу BasePageModel

Продовжимо будувати базовий клас BasePageModel. Метод DeleteOneAsync() функціонує так само, як і метод Delete() HTTP-Post у MVC-версії AutoLot. Представлення оптимізовано таким чином, щоб надсилати лише значення, необхідні EF Core для видалення запису, а саме Id та TimeStamp. Якщо видалення з якоїсь причини не вдається, ModelState очищується, ChangeTracker скидається, сутність витягується, а властивість Error встановлюється на повідомлення про виняток:

```cs
    public async Task<IActionResult> DeleteOneAsync(int? id)
    {
        if (!id.HasValue || id.Value != Entity.Id)
        {
            Error = "Bad Request";
            return Page();
        }

        try
        {
            await DataService.DeleteAsync(Entity);
            return RedirectToPage("./Index");
        }
        catch (Exception ex)
        {
            ModelState.Clear();
            DataService.ResetChangeTracker();
            Entity = await DataService.FindAsync(id.Value);
            Error = ex.Message;
            AppLoggingInstance.LogAppError(ex, "An error occurred");
            return Page();
        }
    }
```
Протестуемо метод відповідною сторінкою.

## Код Razor Page Delete для Cars

Сторінка Delete успадковується від BasePageModel і має конструктор, який приймає два необхідні параметри:

```cs
namespace AutoLot.Web.Pages.Cars;

public class DeleteModel : BasePageModel<Car, DeleteModel>
{
    public DeleteModel(
    IAppLogging<DeleteModel> appLogging,
    ICarDataService carService) : base(appLogging, carService, "Delete")
    {
    }
}
```

Представлення сторінки не використовує значення SelectList, тому метод обробника HTTP get просто отримує сутність. Оскільки метод не повертає значення, представлення відображається після завершення роботи методу:

```cs
    public async Task OnGetAsync(int? id)
    {
        await GetOneAsync(id);
    }
```
Метод обробника HTTP-popstвикористовує базовий метод DeleteOneAsync(), а потім повертає IActionResult з базового методу:

```cs
    public async Task<IActionResult> OnPostAsync(int? id)
    {
        return await DeleteOneAsync(id);
    }
```

## Перегляд Razor Page Delete для Cars

Представлення приймає необов'язковий ідентифікатор як токен маршруту, який додається до директиви @page.

```html

```
Додайте блок помилки:

```html
@if (!string.IsNullOrEmpty(Model.Error))
{
    <div class="alert alert-danger" role="alert">
        @Model.Error
    </div>
}
else
{
}
```
Тег \<form\> використовує два тег-хелпери. Тег-хелпер asp-page встановлює дію форми на відправку назад до маршруту Delete (у поточному каталозі, яким є Cars). Тег-хелпер asp-route-id визначає значення параметра id маршруту:

```html
@if (!string.IsNullOrEmpty(Model.Error))
{
    //...
}
else
{
    <h1>Delete @Model.Entity.PetName</h1>
    <h3>Are you sure you want to delete this car?</h3>
    <div>
        <form asp-page="Delete" asp-route-id="@Model.Entity.Id">
        </form>
    </div>
}
```
Представлення використовує шаблон відображення Car поза формою та приховані поля для властивостей Id та TimeStamp всередині форми. Підсумок перевірки відсутній, оскільки будь-які помилки в процесі видалення відображатимуться в банері помилки:

```html
@if (!string.IsNullOrEmpty(Model.Error))
{
    //...
}
else
{
    <h1>Delete @Model.Entity.PetName</h1>
    <h3>Are you sure you want to delete this car?</h3>
    <div>
        @Html.DisplayFor(c=>c.Entity)
        <form asp-page="Delete" asp-route-id="@Model.Entity.Id">
            <input type="hidden" asp-for="Entity.Id"/>
            <input type="hidden" asp-for="Entity.TimeStamp"/>
            <button type="submit" class="btn btn-danger">Delete <i class="fas fa-trash"></i></button>&nbsp;&nbsp;|&nbsp;&nbsp;
            <item-list></item-list>
        </form>
    </div>
}
```
Частковий вираз _ValidationScriptsPartial не потрібен, тому перегляд сторінки видалення завершено.
Тепер можна перевірити всі операції CRUD.

# View Components (Компоненти перегляду)

Компоненти перегляду в застосунках на основі Razor Page створюються та функціонують так само, як і в застосунках у стилі MVC. Основна відмінність полягає в тому, де мають бути розташовані часткові види. Щоб розпочати роботу в проекті AutoLot.Web, додайте наступний оператор до GlobalUsings.cs

```cs
global using Microsoft.AspNetCore.Mvc.ViewComponents;
```

Створіть нову папку з назвою ViewComponents у кореневому каталозі. Додайте до цієї папки новий файл класу з назвою MenuViewComponent.cs та оновіть код до наступного (той самий, що був створений у попередній главі).

## Створення часткового перегляду

У Razor Pages елементи меню повинні використовувати тег-хелпер asp-page замість asp-controller та asp-action.
Створіть нову папку з назвою Components в папці Pages\Shared. У цій новій папці створіть ще одну нову папку з назвою Menu. У цій папці створіть часткове представлення з назвою MenuView.cshtml. Очистіть існуючий код та додайте наступну розмітку:

```html
@model IEnumerable<AutoLot.Models.Entities.Make>
<div class="dropdown-menu">
<a class="dropdown-item text-dark" asp-page="/Cars/Index" asp-route-makeId="" asp-route-makeName="">All</a>

@foreach (var item in Model)
{
    <a class="dropdown-item text-dark" asp-page="/Cars/Index" asp-route-makeId="@item.Id" asp-route-makeName="@item.Name">@item.Name</a>
}
</div>
```
Щоб викликати компонент перегляду з синтаксисом тег-хелперів, в файлі _ViewImports.cshtml потрібно щоб був рядок(доданий нами раніше):

```html
@addTagHelper *, AutoLot.Web
```

Нарешті, відкрийте фрагмент _Menu.cshtml змінить наявну ромітку для сторінок Cars та інші:

```html
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-page="/Index">Home <i class="fa fa-home"></i></a>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-page="/RazorSyntax">Razor Syntax  <i class="fas fa-cut"></i></a>
                </li>
                <li class="nav-item dropdown">
                    <a class="nav-link dropdown-toggle text-dark" data-bs-toggle="dropdown">Cars <i class="fa fa-car"></i></a>
                    <vc:menu></vc:menu>
                </li>
                <li class="nav-item">
                    <a class="nav-link text-dark" asp-area="" asp-page="/Privacy">Privacy <i class="fa fa-user-secret"></i></a>
                </li>
```

Тепер, коли ви запустите програму, ви побачите меню Cars з пунктами підменю Makes.

# 