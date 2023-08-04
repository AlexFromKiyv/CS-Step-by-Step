# Анонімні типи

Для створення моделі реального світу ми створюемо новий тип, наприклад в вигляді класу з властивостами конструкторами і методами.
Іноді потрібно створити клас для данних без общирної функціональної бази (методи, події та інше). Крім того можливо використання таких данних потрібне для декількох методів. 
Чи потрібно для таких випадкив створення класу? Адже цого ше треба буде і підтримувати.
Для таких даних користно створити тип на льоту і в цому допамагають анонімні типи. Наприклад необхідно зробити спеціальний метод який отримує вхідні параметри і робить з ними незначни дії.  

```cs
void DefiningAnonymousType()
{
    var girl = new { Name = "Alice", Age = 25 };
    Console.WriteLine(girl+"\n"+ girl.GetType()+"\n\n");


    Console.WriteLine(GetCar("VW", "Käfer", 1938));

    static string GetCar(string manufacturer, string model, int year)
    {
        var car = new { Manufacturer = manufacturer, Model = model, Year = year };
        Console.Write(car+"\n"+ car.GetType()+"\n\n"); 

        string carString = JsonSerializer.Serialize(car);
        return carString;
    }
}

DefiningAnonymousType();
```
```
{ Name = Alice, Age = 25 }
<>f__AnonymousType0`2[System.String,System.Int32]


{ Manufacturer = VW, Model = Kafer, Year = 1938 }
<>f__AnonymousType1`3[System.String,System.String,System.Int32]

{"Manufacturer":"VW","Model":"K\u00E4fer","Year":1938}
```

Коли ми створюємо анонімний тип ми використовуємо ключове слово var в поєднанні з синтаксисом ініціалізації об'єкта. Компілятор сам створить визначення класу для такого об'єкту. Синтаксис ініціалізатора вказує створити приватні тілки для читання властивості для новостворенного типу.
Таким чином анонімні типи дозволяють швидко моделювати "форму" данних зменьшими затратами. Це трохі більше ніж створення типу данних на льоту, якій підтримує основу інкапсуляції за допомогою властивостей та діє на основі відповідного сміслу отриманих значен.

## Внутрешне представлення анонімних типів.

Всі анонімни типи автоматично походять від System.Object. 

```cs
void ReflectObjectContent(object @object)
{
    Type type = @object.GetType();

    Console.WriteLine($"\nObject is instance of {type.Name}");
    Console.WriteLine($"Base class of {type.Name} is {type.BaseType}");
    Console.WriteLine($"object.ToString() == {@object}");
    Console.WriteLine($"@object.GetHashCode() == {@object.GetHashCode()}");
}

void ExplorationAnonimusTypes()
{
    var girl = new { Name = "Julia", Age = 35 };

    ReflectObjectContent(girl);

    // girl.Name = "Olga"; It isn't works. it is readonly
}

ExplorationAnonimusTypes();
```
```
Object is instance of <>f__AnonymousType0`2
Base class of <>f__AnonymousType0`2 is System.Object
object.ToString() == { Name = Julia, Age = 35 }
@object.GetHashCode() == 366485669
```
Об'єкт має тип <>f__AnonymousType0`2 який призначає компілятор і не доступне в коді. Всі властивості в цьому класі readonly. Як видно з прикладу при створені класу компілятор перевизначає метод ToString. Так само превизначаються Equals(), GetHashCode(). 

## Як працює Equals.

GetHashCode обчислює хещ-значеня виходячи з кожного значеня поля як вхідні данні для типу System.Collections.Generic.EqualityComparer<T>. Ця реалізація передбачає два таки обєкта еквівалентні якшо співпадають всі їх значення. Анонімні типи добре підходять для розміщеня в контейнері Hashtable.

```cs
void MethodEqualsIntoAnonymousType()
{
    var girl1 = new { Name = "Olga", Age = 35 };
    var girl2 = new { Name = "Olga", Age = 35 };

    ReflectObjectContent(girl1);
    ReflectObjectContent(girl2);

    Console.WriteLine();

    Console.WriteLine($"girl1.Equals(girl2): {girl1.Equals(girl2)} ");
    Console.WriteLine($"girl1 == girl2: {girl1 == girl2}");

}
MethodEqualsIntoAnonymousType();
```
```

Object is instance of <>f__AnonymousType0`2
Base class of <>f__AnonymousType0`2 is System.Object
object.ToString() == { Name = Olga, Age = 35 }
@object.GetHashCode() == -522252693

Object is instance of <>f__AnonymousType0`2
Base class of <>f__AnonymousType0`2 is System.Object
object.ToString() == { Name = Olga, Age = 35 }
@object.GetHashCode() == -522252693

girl1.Equals(girl2): True
girl1 == girl2: False

```
По перше ми бачено компілятор використовує один і той самий тип для створення об'єктів. Тобто якшо анонімний клас по визначенню типів підходить він використовує його а не робить новий. По друге метод Equals порівнює об'єкти по значенням а не по посиланню(використовує семантіку на основі значень).
Аноанімні типи не отримуєть перезавантажені методи == тому в цьому випадку порівнюються посилання.



