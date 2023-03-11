# Const та read-only поля.

Іноді в класі потрібно зберігати константи та дані які потрібні тільки для читання.

## Const
Константи ніколи не змінюються після їх початкового призначення.
```cs
    internal class BodyMassIndex
    {
        public const double LESS_THEN_NORM = 18.5;
        public const double OVER_THEN_NORM = 25; 

        public void Greeting()
        {
            const string hello = "Hello\n";
            const string greeting = $"{hello}This is Body mass index calculator \n\n";
            Console.WriteLine(greeting);
        }

    }
```
```cs
UsingConstants();
void UsingConstants()
{
    //BodyMassIndex.LESS_THEN_NORM = 17; // don't work for const
    Console.WriteLine(BodyMassIndex.OVER_THEN_NORM);
}
```
Константи є неявно статичними данними. Їх також можна використовувати в середині методів. Початкове значеня призначається при визначенні. Значеня констант повино бути відомо під час компіляції тому в конструкторах константи не призначаються.

## Read-only та static read-only поля.

Відмінісь полів read-only від констант в тому шо вони можуть бути визначені підчас виконання і можуть бути визначені тільки в конструкторі. Це може бути корисним коли параметри всієї програми треба прочитати з файлу і установити.
```cs
    internal class BodyMassIndex_v1
    {
        public readonly double LESS_THEN_NORM;
        public readonly double OVER_THEN_NORM;

        public BodyMassIndex_v1()
        {
            LESS_THEN_NORM = 18.5;
            OVER_THEN_NORM = 25;
        }
    }
```
```cs
UsingReadOnly();
void UsingReadOnly()
{
    BodyMassIndex_v1 bodyMassIndex_V1 = new BodyMassIndex_v1();

    //bodyMassIndex_V1.LESS_THEN_NORM = 18; don't work for read-only
    Console.WriteLine(bodyMassIndex_V1.LESS_THEN_NORM);

}
```
На відміну від констант read-only поля не є статичними. Якшо треба аби ці поля використовивались на рівні не окремого об'єкта а на рівні класу їх явно треба зробити static.
```cs
    internal class BodyMassIndex_v2
    {
        public static readonly double LESS_THEN_NORM = 18.5;
        public static readonly double OVER_THEN_NORM;

        static BodyMassIndex_v2()
        {
            OVER_THEN_NORM = 25;
        }
    }
```
Тут показани два дваріанти ініціалізації. Конструктор використовується коли такі дані відомі під час виконання і зчитуються зовні. 



