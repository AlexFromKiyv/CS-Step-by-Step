# Рядки

System.String скорочено string допомогає працювати з рядками. Додамо проект Strings

## Створення
```cs
ExplorationOfStrings_1();

static void ExplorationOfStrings_1()
{
    string myString = "Hi girl!";

    Console.WriteLine(myString);
    Console.WriteLine($"String is ValueType: {myString is ValueType}");
    Console.WriteLine($"Length:{myString.Length}");
    Console.WriteLine($"Compare with \"HI girl!\":{string.Compare(myString,"HI girl!")}");
    Console.WriteLine($"Contains \"girl\":{myString.Contains("girl")}");
    Console.WriteLine($"To uppper:{myString.ToUpper()}");
    Console.WriteLine($"Replace \" girl!\":{myString.Replace(" girl!"," !")}");
    Console.WriteLine(myString);
}
```
Як ми бачимо тип не є ValueType і тому зним ведется робота як з об'єктом. Крім того треба зазначити шо метод <em>Replace</em> не заминює <em>myString</em> а створює нову.

## Інтерполяція
```cs
Interpolation();
static void Interpolation()
{
    string name = "Julia";
    int weight = 65;

    string myString = string.Format("Name:{0} Weight:{1}",name,weight);
    Console.WriteLine(myString);

    myString = $"Name:{name} Weight:{weight}";
    Console.WriteLine(myString);

    myString = $"Name:{name.ToUpper()} Weight:{weight+=3}";
    Console.WriteLine(myString);
}
```
Таким чином можна вносити зміні і вирази в рядок. Зверніть увагу після методу ToUpper() нема ; Область в дужка не може бути використана для великої кількості коду. Для методу чи простого виразу.

## Конкатинація
```cs
Concatination();
static void Concatination()
{
    string myString1 = "Hi";
    string myString2 = "everybody";
    string myString3 = myString1 + " " + myString2;
    myString3 += "!";
    Console.WriteLine(myString3);
}
``` 
Таким чином можна об'єднувати рядки.

## Символи екранування

```cs
Escapes();
static void Escapes()
{
    Console.WriteLine("Code\tName\tPrice");
    Console.WriteLine("D:\\Documents\\template.doc");
    Console.WriteLine("Text\n\n");
    Console.WriteLine("Text{0}{0}",Environment.NewLine);
    Console.WriteLine("\"New text\"");
    Console.WriteLine("\a");
}
```
Escape символи дозволяють по різному виводити текст а також додати ситемны звуки. Оскільки перехіду на нову строку відповідають різні символи в різних ОС иноді краше викорасти Environment.NewLine.

## Радок як є.
```cs
Verbatim();

static void Verbatim()
{
    string myString = @"D:\Documents\";
    Console.WriteLine(myString);

    myString =@"How      
    are
       you?";
    Console.WriteLine(myString);
}
```
Додаваня <em>@</em> виключає escape символи і робить рядок таким як він є. Це корисно наприклад для шляху до теки. 

## Порівняння

Хоча рядки відносяться до reference(посилання) типів і в стеку зберігаеться посилання на об'єкт в купі оператори порівняння не порівнюють посилання а порівнюють складових об'єктів рядків.
Тобто для рядків оператори <em> == , != </em> перевизначені.
```cs
//StringsComparison();

static void StringsComparison()
{
    string string1 = "Hi";
    string string2 = "HI";
    Console.WriteLine($"string1:{string1} string2:{string2}");

    Console.WriteLine($" string1 == string2 {string1 == string2} ");
    Console.WriteLine($" string1 == \"Hi\"  {string1 == "Hi"}");
    Console.WriteLine($" string1 == \"HI\"  {string1 == "HI"}");
    Console.WriteLine($" string1 == \"hi\"  {string1 == "HI"}");
    Console.WriteLine($" Hi.Equals(string1) {"Hi".Equals(string1)}");
    Console.WriteLine($" string1.Equals(string2) {string1.Equals(string2)}");
}
```
Об'єкти рядків порівнються посимвольно з урахуванням регистру і культури.


```cs
ChangeStringsBeforeComparison();

static void ChangeStringsBeforeComparison()
{
    string myString = "MEN";
    string enteredString = "men";

    Console.WriteLine(myString.ToUpper() == enteredString.ToUpper());
}
```
Коли регистр не обовязково враховоуовати при порівнянні можно перевести рядок в верхній регістр. Але це може понизити продуктивність при великих рядках і невдачу при різних культурах.

Крашим варіантом робити програму не чутливою для регістра і культури використати перегружені варіант методів порівняння Equals і IndexOf

```cs
ComparationWithCustomize();

static void ComparationWithCustomize()
{
    string s1 = "girl";
    string s2 = "GIRL";

    Console.WriteLine($"s1:{s1} s2:{s2} \n\r");
    Console.WriteLine($"s1.Equals(s2) : {s1.Equals(s2)}");
    Console.WriteLine($"s1.Equals(s2,StringComparison.OrdinalIgnoreCase) : {s1.Equals(s2,StringComparison.OrdinalIgnoreCase)}");
    Console.WriteLine($"s1.Equals(s2,StringComparison.InvariantCultureIgnoreCase) : {s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase)}");

    Console.WriteLine($"s1.Equals(s2, StringComparison.OrdinalIgnoreCase): {s1.Equals(s2, StringComparison.OrdinalIgnoreCase)}");
    Console.WriteLine($"s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase): {s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase)}");
    Console.WriteLine();
    Console.WriteLine($"s1.IndexOf(\"I\"): {s1.IndexOf("I")}");
    Console.WriteLine($"s1.IndexOf(\"I\",StringComparison.OrdinalIgnoreCase)}}: {s1.IndexOf("I",StringComparison.OrdinalIgnoreCase)}");
    Console.WriteLine($"s1.IndexOf(\"I\",StringComparison.InvariantCultureIgnoreCase)}}: {s1.IndexOf("I", StringComparison.InvariantCultureIgnoreCase)}");
}
```
Таким чином можно зробити незалежність від регістру і культури.

# Коли string не кращий варіант.

Тип string добре підходить для збереженя наприклад призвища чи серійного номера. Але коли мова іде про великі тексти то тут цей тип може бути не єффективним.

```cs
StringInHeap();
static void StringInHeap()
{
    string myString = "Hi girl!"; // first object in heap
    Console.WriteLine(myString);
    
    Console.WriteLine(myString.ToUpper()); // second object in heap
    Console.WriteLine(myString);

    myString = "Hi"; // third object in heap
    Console.WriteLine(myString);
}
```
Кожного разу коли ми присвоюєм нове значення існуючій змінній створюється новий об'єкт а не міняються символи в існуючому. Теж саме відбуваеться коли ми викликаємо методи типа ToUpper.Тому коли ми захочему об'єднувати великі текстові дані рядкових змінних да й ше в циклі то це приведе до великої кількості об'єктів в heap. Тобто змінні типу string паганий варіант для програм обробки великіх текстів.

## Клас System.Text.StringBuilder

Цей клас при використані схожий на string.

```cs

UsingStringBuilder();

static void UsingStringBuilder()
{
    StringBuilder mySB = new StringBuilder("Product list:",256);
    mySB.Append(Environment.NewLine);
    mySB.AppendLine("Apple");
    mySB.AppendLine("Garlic");
    mySB.AppendLine("Tomato");
    mySB.AppendLine("Bread");
    mySB.AppendLine("Milk");
    mySB.Replace("Milk", "Kefir");
    Console.WriteLine(mySB);
    Console.WriteLine(mySB.Length);
  
}
```

Коли ви додасте в код тип StringBuilder переконайтесь шо додалось імпортування простору імен в початку файлу using System.Text.

Особливістю використання класу є те шо коли ми робимо маніпуляцію то ми звінюемо внутрішніні символьні дані, а не отримуемо нову копію як з класом string. за замовчуванням створюеться рядок з 16 символів. Але можна зразу вказати 256.

## Використовуваня рядків для передачи сирих байтів.

```cs
UsingBase64encoding();
void UsingBase64encoding()
{

    byte[] binaryObject = new byte[128];
    Random.Shared.NextBytes(binaryObject);
    for (int i = 0; i < binaryObject.Length; i++)
    {
        Console.Write($"{binaryObject[i]:X}");
    }
    Console.WriteLine();
    Console.WriteLine("------Send to network------------");

    string encoded = Convert.ToBase64String(binaryObject);
    Console.WriteLine(encoded);

    Console.WriteLine();
    Console.WriteLine("------Get from network------------");
    byte[] newBinaryObject = Convert.FromBase64String(encoded);
    for (int i = 0; i < newBinaryObject.Length; i++)
    {
        Console.Write($"{newBinaryObject[i]:X}");
    }
}
```
```
69155CDAA39DF93C584CFA7DE3FCBDE69946F837F131C4E64FCD62D5F141E07D491568BB7D23F82C875D33ED864AB57D5C9B1F48D6C05F73AB331D339326722965E6A593D719B639C24C4C96C5BA0AF918BE8BB2BE75B2BD2598C90A8AAFA02DB626F2AE59CF1D15D6F8EB1B2CBD01B1AC2C4E11FA7A9CEC568
------Send to network------------
aQFVzao53wk8WEz6B94/y95pCUb4N/ExDATmT81i1fFB4H1JFWi7fSP4AsgHXTPthkoLVw1cmx9I1sBfc6szHTOTJnICll5qWT1xm2OcJMTJbFugr5GL6Lsr51sr0lmMkAqKr6AC22JvKuWc8dFdb46xssvQGxrCxOEfp6nOxWg=

------Get from network------------
69155CDAA39DF93C584CFA7DE3FCBDE69946F837F131C4E64FCD62D5F141E07D491568BB7D23F82C875D33ED864AB57D5C9B1F48D6C05F73AB331D339326722965E6A593D719B639C24C4C96C5BA0AF918BE8BB2BE75B2BD2598C90A8AAFA02DB626F2AE59CF1D15D6F8EB1B2CBD01B1AC2C4E11FA7A9CEC568
```
При передачі данних в мережі не відомо як відреагує мережа та ОС на ці данні тому безпечно використовувати методи ToBase64String і FromBase64String які претвотюють байти в рядок і навпаки. 



