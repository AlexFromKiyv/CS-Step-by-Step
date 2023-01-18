# Enum

Додамо в рішення проект Enums.

## Створення

Коли ми робимо систему в якій враховуеться стан води простіше оперувати словами Ice, Snow, Liquid, Par ніж 0,1,2,3

```cs
SimpleEnum();
static void SimpleEnum()
{
    WaterStateEnum myEnum = WaterStateEnum.Ice;

    Console.WriteLine(myEnum);
 
    Console.WriteLine(myEnum == WaterStateEnum.Ice);
    Console.WriteLine(myEnum == WaterStateEnum.Liquid);
}

enum WaterStateEnum
{
    Ice,    //0
    Snow,   //1
    Liquid, //2 
    Par     //3
}
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
UsingEnum();
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

enum WaterStateEnum
{
    Ice,    //0
    Snow,   //1
    Liquid, //2 
    Par     //3
}
```

Як бачите використорування именованих значень робить код читабільнішим.Для отримання числового значеня зміну можно привети до базового типу перерахування. 

Клас System.Enum має метод для визначення типу в якому зберігаються значення змінної та інше. 

```cs
UsingSystemEnum();
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
```
В прикладі використовуеться флаг форматування :D.







