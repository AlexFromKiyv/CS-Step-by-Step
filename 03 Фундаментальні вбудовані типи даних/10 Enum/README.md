# Enum

Додамо в рішення проект Enums.

## Створення

Коли ми робимо систему в якій враховуеться стан води простіше оперувати словами Ice, Snow, Liquid, Par ніж 0,1,2,3

```cs
static void SimpleEnum()
{
    WaterStateEnum myEnum = WaterStateEnum.Ice;

    Console.WriteLine(myEnum);
 
    Console.WriteLine(myEnum == WaterStateEnum.Ice);
    Console.WriteLine(myEnum == WaterStateEnum.Liquid);
}
SimpleEnum();

enum WaterStateEnum
{
    Ice,    //0
    Snow,   //1
    Liquid, //2 
    Par     //3
}
```
```
Ice
True
False
```

Для читабільності в кінці добаляють суфікс Enum. В top-lavel operators визначення типів повино бути в кінці файлу за операторами. 

Іменовні константи в перерахуванні відповідають числа з 0 з подальшою прогрессією n+1. Це можна змінити.

```cs
enum WaterState1Enum
{
    Ice = 100,    
    Snow,   //101
    Liquid, //102 
    Par     //103
}
```
Послідовність не обовязкова.

```cs
enum WaterState2Enum
{
    Ice = -1,
    Snow = -1,
    Liquid = 1,
    Par = 2
}
```

За замовчуванням тип зберігання який використовується для зберігання int. Але це можна змінити на byte

```cs
enum WaterState3Enum : byte
{
    Ice,
    Snow,
    Liquid,
    Par
}
```
Це може бути корисним для  пристрої з малим об’ємом пам’яті та потребує економії пам’яті, де це можливо.

## Використання

```cs
static void UsingEnum()
{
    WaterStateEnum  enumNow = WaterStateEnum.Liquid;

    Console.WriteLine(enumNow);
    Console.WriteLine((int)enumNow);

    IsItCold(enumNow);

    enumNow = WaterStateEnum.Ice;

    IsItCold(enumNow);

    //enumNow = Ice;                    don't work
    //enumNow = WaterStateEnum.Boiling; 

    Console.WriteLine(enumNow is ValueType); //True

    static void IsItCold(WaterStateEnum waterStateEnum)
    {
        switch(waterStateEnum)
        {
            case WaterStateEnum.Snow:
            case WaterStateEnum.Ice:
                Console.WriteLine("Yes. It's cold");
                break;               
            case WaterStateEnum.Liquid:
            case WaterStateEnum.Par:
                Console.WriteLine("No. It is'nt.");
                break;
            default:
                Console.WriteLine("I do'nt know yet.");
                break;
        }
    }

}
UsingEnum();

enum WaterStateEnum
{
    Ice,    //0
    Snow,   //1
    Liquid, //2 
    Par     //3
}
```
```
Liquid
2
No. It is'nt.
Yes. It's cold
True

```

Як бачите використорування именованих значень робить код читабільнішим. Для отримання числового значеня зміну можно привети до базового типу перерахування. 

Клас System.Enum має метод для визначення типу в якому зберігаються значення змінної та інше. 

```cs
static void UsingSystemEnum()
{

    EnumInfo(WaterStateEnum.Ice);
    EnumInfo(DayOfWeek.Monday);
    EnumInfo(ConsoleColor.Red);


    static void EnumInfo(System.Enum enumElement)
    {
        Console.WriteLine(enumElement);

        Type enumType = enumElement.GetType();

        Console.WriteLine($"Type:{enumType}");
        Console.WriteLine($"Is enum:{enumType.IsEnum}");
        Console.WriteLine($"Underlying type:{Enum.GetUnderlyingType(enumType)}");
        foreach (var item in Enum.GetValues(enumType))
        {
            Console.WriteLine($"{item} - {item:D}");
        }
        Console.WriteLine();
    }
}
UsingSystemEnum();
```
```
Tuesday - 2
Wednesday - 3
Thursday - 4
Friday - 5
Saturday - 6

Red
Type:System.ConsoleColor
Is enum:True
Underlying type:System.Int32
Black - 0
DarkBlue - 1
DarkGreen - 2
DarkCyan - 3
DarkRed - 4
DarkMagenta - 5
DarkYellow - 6
Gray - 7
DarkGray - 8
Blue - 9
Green - 10
Cyan - 11
Red - 12
Magenta - 13
Yellow - 14
White - 15
```

В прикладі використовуеться флаг форматування :D.

## Побітові операції і Enum

```cs
UsingBitwiseOperations();
static void UsingBitwiseOperations(){
    Console.WriteLine($"6 = {Convert.ToString(6, 2)}");
    Console.WriteLine($"4 = {Convert.ToString(4, 2)}");
    Console.WriteLine($"{Convert.ToString(6, 2)} & {Convert.ToString(4, 2)} = {Convert.ToString(6 & 4, 2)} = {6&4} ");
    Console.WriteLine($"{Convert.ToString(6, 2)} | {Convert.ToString(4, 2)} = {Convert.ToString(6 | 4, 2)} = {6 | 4}  ");
    Console.WriteLine($"{Convert.ToString(6, 2)} ^ {Convert.ToString(4, 2)} = {Convert.ToString(6 ^ 4, 2)} = {6 ^ 4} ");
    Console.WriteLine($"{Convert.ToString(6, 2)} >> 1 = {Convert.ToString(6 >> 1, 2)} = {6 >> 1}  ");
    Console.WriteLine($"{Convert.ToString(6, 2)} << 1 = {Convert.ToString(6 << 1, 2)} = {6 << 1}  ");
    Console.WriteLine($"~{Convert.ToString(6, 2)} = {Convert.ToString(~6, 2)} = {~6}");
    Console.WriteLine($"Int.MaxValue =  {Convert.ToString(int.MaxValue, 2)}");
}
```
Результат:

```
6 = 110
4 = 100
110 & 100 = 100 = 4
110 | 100 = 110 = 6
110 ^ 100 = 10 = 2
110 >> 1 = 11 = 3
110 << 1 = 1100 = 12
~110 = 11111111111111111111111111111001 = -7
Int.MaxValue =  1111111111111111111111111111111
```
Побітові оператори швидкий механізм. Разом з Enum можна рішати деякі завдання.

```cs
static void UsingBitwiseOperationsWithEnum()
{
    ContactPreferenceEnum contactsJulia = ContactPreferenceEnum.Email | ContactPreferenceEnum.Phone;

    Console.WriteLine(contactsJulia.ToString());
    Console.WriteLine();

    Console.WriteLine("None - {0}", (contactsJulia | ContactPreferenceEnum.None) == contactsJulia);
    Console.WriteLine("Email - {0}", (contactsJulia | ContactPreferenceEnum.Email) == contactsJulia);
    Console.WriteLine("Phone - {0}", (contactsJulia | ContactPreferenceEnum.Phone) == contactsJulia);
    Console.WriteLine("Ukrposhta - {0}", (contactsJulia | ContactPreferenceEnum.Ukrposhta) == contactsJulia);
}
UsingBitwiseOperationsWithEnum();

[Flags]
enum ContactPreferenceEnum
{
    None = 1,
    Email = 2,
    Phone = 4,
    Ukrposhta = 8
}
```
```
Email, Phone

None - False
Email - True
Phone - True
Ukrposhta - False
```
Ше один приклад використання Enum.

Types.cs
```cs
class Person
{
    public string? Name { get; set; }

    public DayOfTheWeek TrainingDays;
}

[Flags]
public enum DayOfTheWeek : byte
{
    Sunday    = 0b_0000_0000, // i.e. 0
    Monday    = 0b_0000_0001, // i.e. 1
    Tuesday   = 0b_0000_0010, // i.e. 2
    Wednesday = 0b_0000_0100, // i.e. 4
    Thursday  = 0b_0000_1000, // i.e. 8
    Friday    = 0b_0001_0000, // i.e. 16
    Saturday  = 0b_0010_0000, // i.e. 32
}
```
```cs
UsingEnumForPerson();
void UsingEnumForPerson()
{
    Person girl = new Person();
    girl.Name = "Vikotry";
    girl.TrainingDays = DayOfTheWeek.Monday | DayOfTheWeek.Wednesday | DayOfTheWeek.Friday;

    Console.WriteLine($"{girl.Name} is training {girl.TrainingDays}");
}
```
```
Vikotry is training Monday, Wednesday, Friday
```
Робіть тип enum похідним від byte коли кількість варіантів не бульше 8, ushort для 16, uint для 32, ulong для 64.
