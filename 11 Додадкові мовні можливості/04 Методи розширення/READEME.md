# Методи розширення

Методи розширення дозволяють збільшити функціональні можливості класа або структури не роблячи прямих змін в орігінальному визначені типу.Цеможе бути корисним в декількох випадках. 
Коли клас знаходиться в реальній роботі і стає зрозуміло шо потрібно додавати нові члени. Коли ви змінюєте клас на пряму ви ризикуєте порушити зворотню сумістність з базовими класами оскільки вони можуть бути скопільовані не з останім визначенням класу.
Одним из способів забезпечення зворотньої сумісності є створення похідного класую Однак тепер є два класи для підтримки.
Якшо у вас є стркутура або запечатаний клас і ви хочите зробити шоб у нього були поліморфна поведінка для вашої системи. Оскілки це можно зробити тільки додавши члени знову виникає ризик порушеня сумісності.
Використовуючи методи розширення можна модіфікуваим тип без підкалсу та без безпосередньої його зміни.
Нова функціональність пропонується лише в поточному проекті.

## Визначення.

Накладаються обмеження на методи розширеня шо вони повині бути визначені у статичному класі і бути статичними оскільки працюють на рівні типу. 

```cs
    public static class MyExtensions
    {
        public static void DisplayDefiningAssembly(this object obj)
        {
            Console.WriteLine($"\n" +
                $"{obj.GetType()} " +
                $"lives here: " +
                $"{Assembly.GetAssembly(obj.GetType())?.GetName().Name}");
        } 

        public static int ReverseDigits(this int i)
        {
            char[] chars = i.ToString().ToCharArray();

            Array.Reverse(chars);

            string newStringGigit = new string(chars);

            return int.Parse(newStringGigit);
        }

        public static string ReverseChars(this string s)
        {
            char[] chars = s.ToCharArray();

            Array.Reverse(chars);

            string newString = new string(chars);

            return newString;
        }
    }
```
```cs
void InvokeExtentionMethod()
{
    int myInt = 123;
    myInt.DisplayDefiningAssembly();

    Console.WriteLine(myInt.ReverseDigits());

    string? myString = "Hi girl";

    myString.DisplayDefiningAssembly();

    Console.WriteLine(myString.ReverseChars());

    // myString.ReverseDigits(); have no for string

    System.Data.DataSet ds = new();
    ds.DisplayDefiningAssembly();
}

InvokeExtentionMethod()
```
```

System.Int32 lives here: System.Private.CoreLib
321

System.String lives here: System.Private.CoreLib
lrig iH

System.Data.DataSet lives here: System.Data.Common

```
this представляє елемент який розширюється. Це ключове слово слугує модіфікаторм для першого (і тілки першого ) параметра методу.
Перший метод використовувати для будь якого об'єкту. Він відображає назву збірки шо містить відповідний тип. Оскільки object є батьком для всіх типів то цей метод з'явится для будьякої змінної.
Другий метод дозволяє для будь якого int перевернути цифри.Зверніть увагу як this визначено як модіфікатор перед типом параметра. Перший параметр представляє тип що розширюється. 
Метод розширення може мати декілька параметрів але лише перший визначає тип до якого він буде використаний.







