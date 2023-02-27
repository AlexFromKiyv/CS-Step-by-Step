# Клас

Клас це визначений користувачем тип який складаеться з полів данних і членами які працюють з ними. Набір данних полів є "станом" екземпляра класу якій називають об'єктом. Це поєднання полів і методів дозволяє створити модель об'єктів реального світу.

## Створення

1. Створимо рішеня OOP з консольним проектом SimpleClass.
2. На проекті правий-клік Add > Class.
3. Name: Bike.cs
4. Замініть контент цього класу
```cs
namespace SimpleClass
{
    class Bike
    {
        public string ownerName;
        public int currentSpeed;

        public void StateToConsol() => Console.WriteLine($"Bicycler: {ownerName} is driving at speed: {currentSpeed}");
        public void SpeedUp(int speedChange) => currentSpeed += speedChange;
    }
}
```
Тут члени-змінні визначають стан а методи поведінку. Члени оголошено з модіфікаторм public. Це означає коли буде створено об'єкт то до такіх членів буде безпосередній доступ. Гарною практикою є прості поля класу робити або приватними або захищеними.  

5. Замініть контент Program.cs
```cs
using SimpleClass;

UsingClassBike();
void UsingClassBike()
{
    Bike bike;
   // bike.StateToConsol(); don't work
    bike = new();
    bike.StateToConsol();

    bike.ownerName = "Mark";
    bike.currentSpeed = 5;
    bike.StateToConsol();


    for (int i = 0; i < 5; i++)
    {
        bike.SpeedUp(1);
        bike.StateToConsol();
    }
}
```
```
Bicycler:  is driving at speed: 0
Bicycler: Mark is driving at speed: 5
Bicycler: Mark is driving at speed: 6
Bicycler: Mark is driving at speed: 7
Bicycler: Mark is driving at speed: 8
Bicycler: Mark is driving at speed: 9
Bicycler: Mark is driving at speed: 10
```
В строчці Bike bike; створюється об'єкт якій ше не визначено. Наступна bike = new(); присваює посилання на об'єкт який створено з використанням конструктора за замовчуванням.

## Конструктори.

Оскільки об'єкти мають стан є бажання мати механізм його втановленя при створенні.
Конструктор — це спеціальний метод класу, який викликається опосередковано під час створення об’єкта за допомогою ключового слова new. Вони мають таку саму назву як клас і нічно не повертають.

Як видно з попереднього прикладу при створені об'єкта поля отримали значення default. Це робота конструктора за замовчуванням. Цю поведінку можна перевизначити.

```cs
class Bike
    {

        public string ownerName;
        public int currentSpeed;

        //Change default constructor
        public Bike()
        {
            ownerName = "Noname";
            currentSpeed = 2;
        }

        public void StateToConsol() => Console.WriteLine($"Bicycler: {ownerName} is driving at speed: {currentSpeed}");
        public void SpeedUp(int speedChange) => currentSpeed += speedChange;
    }
```
```
Bicycler: Noname is driving at speed: 2
Bicycler: Mark is driving at speed: 5
Bicycler: Mark is driving at speed: 6
Bicycler: Mark is driving at speed: 7
Bicycler: Mark is driving at speed: 8
Bicycler: Mark is driving at speed: 9
Bicycler: Mark is driving at speed: 10
```
Конструктор за замовчуванням ніколи не приймає аргументів.
