# Основні можливості обробки винятків

Кажучи про якість програмного забезпеченя відрізняють:

Bugs - помилка яку заклав програміст.
User errors - дії користувача які не передбачив програміст.
Exeption - це ситуації які виникають в процесі роботи і вважаються неприйнятними для корректної роботи і які є можливість передбачити.

Навіть коли програміст заклав помилку та непередбачив помилку користувача среда виконання генерує відповідний виняток. В бібліотеці базових класів визначені класи винятків FormatException, IndexOutOfRangeException, FileNotFoundException, ArgumentOutOfRangeException та інші.

## Роль обробки винятків в .Net

Помилки це звічайна річ і вони виникають постійно. Підходи коли помилкам давали числові значення не ідеальні, трохи застарілі але ще працюють. Кілкість чисел-помилок може в проекти дуже збільшуватися.

Аби команди не придумували свої варіанти, .Net дає свою стандартну струкруровану обробку винятків. Це уніфікований підхід до обробки помилок. Iншою перевагою структурованої обробки винятків .Net є шо замість загадкового числа створюється об'єкт помилки в якому міститься тип , опис а також детальний знімок стеку викликів якій ініціював проблему. Також користувач може отримати додадтоку інформацію як можна вирішити проблему у вигляді спеціальних даних або посилання.

Аби обробити виняток треба використати:
- Тип класу шо представляє виняток.
- Член класу який видає виняток за обставин.
- Блок коду який викликає члена у видаку різних обставин.
- Блок коду який перехоплює виняток. 

Об'єкт шо представляє проблему яка винилка є єкземпляром класу шо розширюється від  System.Exception або його нащадків.

## System.Exception

Всі винятки ініційовані в .Net остаточно походять від System.Exception. Рішення Exeptions, проект BaseClassExeption.

```cs
    //
    // Summary:
    //     Represents errors that occur during application execution.
    public class Exception : ISerializable
    {
        public Exception();
        public Exception(string? message);
        public Exception(string? message, Exception? innerException);
        protected Exception(SerializationInfo info, StreamingContext context);

        public virtual string? StackTrace { get; }
        public virtual string? Source { get; set; }
        public virtual string Message { get; }
        public Exception? InnerException { get; }
        public int HResult { get; set; }
        public virtual IDictionary Data { get; }
        public MethodBase? TargetSite { get; }
        public virtual string? HelpLink { get; set; }

        ...

    }
```
Це основна частина визначення класу.

## Виняткова ситуація.

Розглянемо ситуацію коли виняток предбавуваний . Файл Classes_v1.cs
```cs
    class Car_v1
    {
        public const int MAXSPEED = 140;


        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }


        private bool _carIsDead;

        public Car_v1(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }

        public Car_v1()
        {
        }

        public void Accelerate(int delta)
        {

            if (_carIsDead)
            {
                Console.WriteLine($"{Name} is out of order ..."); 
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MAXSPEED)
                {
                    Console.WriteLine($"{Name} has overheated!");
                    CurrentSpeed = 0;
                    _carIsDead = true;
                }
                else
                {
                    Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
                }
            }
        }
    }
```
```cs
ExplorationTheOccurationOfAnException();
void ExplorationTheOccurationOfAnException()
{
    Car_v1 car = new("Nissan Leaf", 35);

	for (int i = 0; i < 10; i++)
	{
		car.Accelerate(20);
	}

}
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135
Nissan Leaf has overheated!
Nissan Leaf is out of order ...
Nissan Leaf is out of order ...
Nissan Leaf is out of order ...
Nissan Leaf is out of order ...
```
В цьому прикладі ми будемо створювати структурований виняток за відомими обставинами. Логіка класу проста коли відбувається збільшення швидкості(Accelerate) відбуваеться превірка масимальної швидкості. Коли шіидкість перевишує масимально допустиму двигун прегрівається шо фіксуеться в приватній змінній.(В цому випадку краще б було замість константи використовувати властивість тільки для читаня але це не змінює суті). У цьому прикладі знаючи всі обмеженя ми самі конролює мо ситуацію. Поточна реалізація просто показує прегрів але можна можна покрашити ситуацію запобігши цьому.

## throw і загальний виняток

```cs
    class Car_v2
    {
        public const int MAXSPEED = 140;
        public string Name { get; set; } = "";
        public int CurrentSpeed { get; set; }

        private bool _carIsDead;

        public Car_v2(string name, int currentSpeed)
        {
            Name = name;
            CurrentSpeed = currentSpeed;
        }
        public Car_v2()
        {
        }

        public void Accelerate(int delta)
        {

            if (_carIsDead)
            {
                Console.WriteLine($"{Name} is out of order ...");
            }
            else
            {
                CurrentSpeed += delta;
                if (CurrentSpeed > MAXSPEED)
                {
                    CurrentSpeed = 0;
                    _carIsDead = true;
                    throw new Exception($"{Name} has overheated!");
                }
                else
                {
                    Console.WriteLine($"Current speed {Name}:{CurrentSpeed}");
                }
            }
        }
    }
```
```cs
ExplorationThrow();
void ExplorationThrow()
{
    Car_v2 car = new("Nissan Leaf", 35);

    for (int i = 0; i < 10; i++)
    {
        car.Accelerate(20);
    }
}
```
```
Current speed Nissan Leaf:55
Current speed Nissan Leaf:75
Current speed Nissan Leaf:95
Current speed Nissan Leaf:115
Current speed Nissan Leaf:135
Unhandled exception. System.Exception: Nissan Leaf has overheated!
```
В цьому прикладі створюється екземпляр класу Exeption з використанням конструктора який заповнює read-only поле. Аби викинути виняток використовується throw.  





