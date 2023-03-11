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




