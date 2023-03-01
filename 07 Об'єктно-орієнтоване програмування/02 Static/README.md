# Static.

```cs
void BadInvokeStatic()
{
    int a = 0;
    int b;
    //b = a.Parse("1"); //cannot be accessed with an instance reference;
    //                  //qualify it with a type name instied
    b = int.Parse("1");

    //Console console = new Console(); // Cannot create instance of the static class
}
```
На члени класу які визначені як static (статичні) можна посилатися як на можливість класу, а не конкретного об'єкту. Тобто аби викликати статичний метод треба використовувати клас. Також не можна створити об'екта статичного класу.

Статичні члени класу можна разглядати як службові і виконують загальні речі. За визначенням, службовий клас — це клас, який не підтримує жодного стану на рівні об’єкта та не створюється за допомогою ключового слова new. Але статичними можуть бути визнеачення буль якого класу. Ключеве слово статіс може бути застосовано до:

- даних класу
- методів класу
- властивостей класу
- до конструкторів
- до цілого класу
- до using

## Статичні данні.

Кожний окремий об'єкт зберігає дані просвій стан як свою власну копію данних. Коли визначаються статичні дані класу вони відносяться для всіх обї'ктів.

```cs
    class Person
    {
        public string name;
        public double? height;
        public double? weight;

        public Person(string name, double? height, double? weight)
        {
            this.name = name;
            this.height = height;
            this.weight = weight;
        }

        public Person(string name)
        {
            this.name = name;
        }

        public void ToConsole() => Console.WriteLine($"Name:{name} Height:{height} Weight:{weight}");
    }
```
```cs
CreateSomePerson();

void CreateSomePerson()
{
    Person girl_1 = new("Ira");
    girl_1.height = 65.0;
    girl_1.ToConsole();

    Person boy_1 = new("Igor");
    boy_1.ToConsole();
}
```
```
Name:Ira Height: Weight:
Name:Igor Height: Weight:
```
При створені об'ектів для кожного виділяеться окрема пам'ять. Зміни одного не впривають на інші. Додамо статичне поле.

```cs
        public static double maxWeight = 635;
        public static double avarageWeight = 81.5;
```
```cs
UsingStaticData();

void UsingStaticData()
{
    Person girl = new("Maria");
    girl.weight = 55;

    Person man = new("Max");
    man.weight = 90;

    Console.WriteLine(girl.weight < Person.avarageWeight);
    Console.WriteLine(man.weight < Person.avarageWeight);

}
```
```
True
False
```
Для статичні данних пам'ять виділяються один раз і ці дані можуть бути використовувана всіма об'єктами. Статични методи також відносяться до усьго класу.

```cs
        // Static method
        public static double GetAvarageWeight() => avarageWeight;
        public static void SetAvarageWeight(double newAvarageWeight) => avarageWeight = newAvarageWeight;
```
```cs

UsingStaticMethods();

void UsingStaticMethods()
{
    Console.WriteLine(Person.GetAvarageWeight());
    Person.SetAvarageWeight(77.2);
    Console.WriteLine(Person.GetAvarageWeight());
}
```
```
81,5
77,2
```
Таким чином якшо дані або закономірності мають місце для всього класу то поля і методи можна робити статичними. Якби avarageWeight не було б статичним полем тоді для всіх об'єктів треба булоб встановлювати це значення і міняти.
