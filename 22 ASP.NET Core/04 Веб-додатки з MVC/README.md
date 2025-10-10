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

Механізм перегляду Razor був розроблений як покращення порівняно з механізмом перегляду веб-форм та використовує Razor як основну мову.
