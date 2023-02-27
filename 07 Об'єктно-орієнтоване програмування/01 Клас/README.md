# Клас

Клас це визначений користувачем тип який складаеться з полів данних і членами які працюють з ними. Набір данних полів є "станом" екземпляра класу якій називають об'єктом. Це поєднання полів і методів дозволяє створити модель об'єктів реального світу.

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

        public void StateToConsol() => Console.WriteLine($"{ownerName} is driving at speed {currentSpeed}");
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
    Bike bike = new();

    bike.ownerName = "Mark";
    bike.currentSpeed = 5;

    for (int i = 0; i < 5; i++)
    {
        bike.SpeedUp(1);
        bike.StateToConsol();
    }
}
```
```
Mark is driving at speed 6
Mark is driving at speed 7
Mark is driving at speed 8
Mark is driving at speed 9
Mark is driving at speed 10
```

