# Enum

Додамо в рішення проект Enums.

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
    Ice = -2,
    Snow = -1,
    Liquid = 1, 
    Par = 2
}
```




