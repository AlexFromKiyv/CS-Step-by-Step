# IEnumerable, IEnumerator

# IEnumerable (Перелічуваний)

Виходячи з назви вміст обє'кту класу можна по черзі пройти і перелічити.
Наприклад в масиві можна пройтий по кожному його єлементу.

```cs
ArrayIsEnumerable();
void ArrayIsEnumerable()
{
    int[] temperatures = { 4, 6, 8 };

    foreach (int item in temperatures)
    {
        Console.WriteLine(item);
    }
}
```
```
4
6
8
```
Досить багато ситуацій коли корисно пройтись по елементам колекції тобто використати оператор foreach. Можливість використовувати foreach є у об'єктів ваших кастомних класах. Для цього тип має мати реалізаціє метода під назвою GetEnumerator() який формальзований інтерфейсом IEnumerable.
```cs
namespace System.Collections
{
    //
    // Summary:
    //     Exposes an enumerator, which supports a simple iteration over a non-generic collection.
    public interface IEnumerable
    {
        //
        // Summary:
        //     Returns an enumerator that iterates through a collection.
        //
        // Returns:
        //     An System.Collections.IEnumerator object that can be used to iterate through
        //     the collection.
        IEnumerator GetEnumerator();
    }
}
```
Метод повертає посилання на об'єкт інтерфейсного типу IEnumerator

```cs
namespace System.Collections
{
    //
    // Summary:
    //     Supports a simple iteration over a non-generic collection.
    public interface IEnumerator
    {
        //
        // Summary:
        //     Gets the element in the collection at the current position of the enumerator.
        //
        // Returns:
        //     The element in the collection at the current position of the enumerator.
        object Current { get; }

        //
        // Summary:
        //     Advances the enumerator to the next element of the collection.
        //
        // Returns:
        //     true if the enumerator was successfully advanced to the next element; false if
        //     the enumerator has passed the end of the collection.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The collection was modified after the enumerator was created.
        bool MoveNext();
        //
        // Summary:
        //     Sets the enumerator to its initial position, which is before the first element
        //     in the collection.
        //
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     The collection was modified after the enumerator was created.
        void Reset();
    }
}
``` 
Цей інтерфейс забезпечуває інфраструктуру проходу по елементам які зберігаються в IEnumerable-сумісному контейнері.

Спробуемо цю поведінку 
```cs
    class Car
    {
        public string Name { get; set; } = "";
        public int MaxSpeed { get; set; }

        public Car(string name, int maxSpeed)
        {
            Name = name;
            MaxSpeed = maxSpeed;
        }

        public Car()
        {
        }
    }

    class Garage 
    {
        private Car[] carArray = new Car[4];
        public Garage()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerator GetEnumerator() =>  carArray.GetEnumerator();
    }
```
```cs
IEnumerableFromArray();
void IEnumerableFromArray()
{
    Garage garage = new();

    foreach (var item in garage)
    {
        Console.WriteLine($"{((Car)item).Name}");
    }

    Console.WriteLine();

    var ie = garage.GetEnumerator();

    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}");
    Console.Write($"{((Car)ie.Current).Name}\t");
    Console.WriteLine($"The method Current returned : {ie.Current}\t");

    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}");
    ie.Reset();
    Console.WriteLine($"Reset");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
    Console.WriteLine($"The method MoveNext returned : {ie.MoveNext()}\t{((Car)ie.Current).Name} ");
}
```
```
VW Golf 1
VW Golf 2
VW Golf 3
VW Golf 4

The method MoveNext returned : True
VW Golf 1       The method Current returned : IEnumerable_IEnumerator.Car
The method MoveNext returned : True     VW Golf 2
The method MoveNext returned : True     VW Golf 3
The method MoveNext returned : True     VW Golf 4
The method MoveNext returned : False
Reset
The method MoveNext returned : True     VW Golf 1
The method MoveNext returned : True     VW Golf 2
The method MoveNext returned : True     VW Golf 3
The method MoveNext returned : True     VW Golf 4
Unhandled exception. System.InvalidOperationException: Enumeration already finished.
   at System.ArrayEnumerator.get_Current()
   at Program.<<Main>$>g__IEnumerableFromArray|0_1() in D:\MyWork\CS-Step-by-Step\07 Об'єктно-ор?єнтоване програмування\13 Корисн? ?нтерфейси .Net\Useful_Interfaces\IEnumerable_IEnumerator\Program.cs:line 43
   at Program.<Main>$(String[] args) in D:\MyWork\CS-Step-by-Step\07 Об'єктно-ор?єнтоване програмування\13 Корисн? ?нтерфейси .Net\Useful_Interfaces\IEnumerable_IEnumerator\Program.cs:line 15
```
Для реалізації інтерфейсу в класі Garage можна було підти більш довгим шляхом і реалізувати методи самостійно. Оскільки клас Array(як і інші коллекції) вже має реалізовані IEnumerable і IEnumerator запит на виконання поведінки ми делегуємо йому. Тобто наш клас веде себе як массив.
В прикладі показано шо клас безпечно використовувати з foreach. Але якщо потрібно використати методи інтерфейсу IEnumerator то потрібно враховувати на те шо коли метод MoveNext повертає false то це значить шо коллекція повністью пройдена і Current ні на що не вказує.

```cs
    class Garage_v1 : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage_v1()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        IEnumerator IEnumerable.GetEnumerator() => carArray.GetEnumerator();
    }
```
```cs
IEnumerableFromArray_2();
void IEnumerableFromArray_2()
{
    Garage_v1 garage = new();

    //var enumerator = garage.GetEnumerator(); //class cannot contain definition GetEnumerator  

    foreach (Car item in garage)
    {
        Console.WriteLine($"{item.Name} {item.MaxSpeed}");
    }
}
```
```
VW Golf 1 145
VW Golf 2 167
VW Golf 3 168
VW Golf 4 192
```
В цьому прикладі можна відзначити дві корисні речі. 
По перше при явній реалізації метод  GetEnumerator він стає приватним і недоступний на рівні об'єкта для користувачів а для foreach буде доступним. Таким чином зменьшується можливість використати його з винятком.
По друге в foreach відбуваеться приведеня від Оbject до Car. Тут можливо краще зробити додадкову перевірку.

## Метод ітератора за допомогою yield.

Існує альтернативний варіант побудови типів які мають колекцію і дозволяють використати ітерації по цій коллекції. Ітератор це член що вказує як повині повертатись внутрішні елементи контейнера при роботі foreach.

```cs
    class Garage_v2 : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage_v2()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerator GetEnumerator()
        {
            yield return carArray[0];
            yield return carArray[1];
            yield return carArray[2];
            yield return carArray[3];

            foreach (Car car in carArray)
            {
                yield return car; 
            }

            yield return new Car("VW Polo",160);
            yield return new Car("VW Passat",180);
        }
    }
```
```cs
ExamineIEnumerableWithYield();
void ExamineIEnumerableWithYield()
{
    Garage_v2 garage = new();

    foreach (Car item in garage)
    {
        Console.WriteLine($"{item.Name} {item.MaxSpeed}");
    }
}
```
```
VW Golf 1 145
VW Golf 2 167
VW Golf 3 168
VW Golf 4 192
VW Golf 1 145
VW Golf 2 167
VW Golf 3 168
VW Golf 4 192
VW Polo 160
VW Passat 180
```
Ключове слово yield використовується для вказівки значеня яке буде повернуто конструкції foreach. Коли досягнуто оператора yield return розташування в контейнері зберігаеться і виконання презапескаеться при наступному виклику визову ітератора. Цей синтаксис може бути корисним, якщо ви хочете повернути локальні дані з методу для обробки за допомогою синтаксису foreach.

## Особливості GetEnumerator()

```cs
    class Garage_v3 : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage_v3()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerator GetEnumerator()
        {
            Console.WriteLine("Without MoveNext, it does not work.");
            yield return carArray[0];
            yield return carArray[1];
        }
    }
```
```cs
ExplorationGetEnumerator();
void ExplorationGetEnumerator()
{
    Garage_v3 garage = new();

    var enumerator = garage.GetEnumerator();

    Console.WriteLine("After called GetEnumerator");

    enumerator.MoveNext();
}
```
```
After called GetEnumerator
Without MoveNext, it does not work.
```
При такому використанні yeild код методу виконуеться тільки після першої ітерації яка може виникнути при визові MoveNext. До операторів yeild може бути бангато інструкцій наприклад підключеня до БД і помилка виникне лише під час ітерацій. 

```cs
    class Garage_v4 : IEnumerable
    {
        private Car[] carArray = new Car[4];
        public Garage_v4()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerator GetEnumerator()
        {
            Console.WriteLine("Work in GetEnumerator (For example connect to DB)");

            return ActualImplementation();

            IEnumerator ActualImplementation()
            {
                yield return carArray[0];
                yield return carArray[1];
                yield return carArray[2];
            }
        }
    }
```
```cs
ExplorationGetEnumeratorWithFunction();
void ExplorationGetEnumeratorWithFunction()
{
    Garage_v4 garage = new();

    var enumerator = garage.GetEnumerator();

    Console.WriteLine("After called GetEnumerator");

    enumerator.MoveNext();
}
```
```
Work in GetEnumerator (For example connect to DB)
After called GetEnumerator
```
Таким чином все шо стосується елементів конетйнера для ітерацій виделено в окрему функцію. І це дозволяє протестувати код шо находиться перед процесом створеня інфраструктури ітерації за допомогою yeild.

## Іменований ітератор.
yeild може бути використовуватися і в інших методах які називають іменованими ітераторами. Ці методи повертають тип IEnumerable 

```cs
    class Garage_v5 
    {
        private Car[] carArray = new Car[4];
        public Garage_v5()
        {
            carArray[0] = new Car("VW Golf 1", 145);
            carArray[1] = new Car("VW Golf 2", 167);
            carArray[2] = new Car("VW Golf 3", 168);
            carArray[3] = new Car("VW Golf 4", 192);
        }

        public IEnumerable GetTheCars(bool returnReversed = false)
        {

            return ActualImplementation();

            IEnumerable ActualImplementation()
            {
                if (returnReversed)
                {
                    for (int i = carArray.Length - 1; i >= 0; i--)
                    {
                        yield return carArray[i];
                    }
                }
                else
                {
                    for (int i = 0; i < carArray.Length; i++)
                    {
                        yield return carArray[i];
                    }
                }
            }
        }
    }
```
```cs
ExplorationNamedEnumerator();
void ExplorationNamedEnumerator()
{
    Garage_v5 garage = new();

    foreach (Car item in garage.GetTheCars(true))
    {
        Console.WriteLine($"{item.Name} {item.MaxSpeed}");
    }

    foreach (Car item in garage.GetTheCars())
    {
        Console.WriteLine($"{item.Name} {item.MaxSpeed}");
    }
}
```
```
VW Golf 4 192
VW Golf 3 168
VW Golf 2 167
VW Golf 1 145
VW Golf 1 145
VW Golf 2 167
VW Golf 3 168
VW Golf 4 192
```
Тут реалізована інфраструктура яка реалізовує прохід в прямому так і в зворотньому порядках. Метод можна застосовувати окремо від реалізації інтерфейсу IEnumerable.
Таким чином іменовані ітератори можна викороистовувати для отримання різних варіантів послідовностей.

Таким чином аби об'єкти вашого типу можна було використовувати в конструкції foreach треда шоб була можливість отримати IEnumerable-сумісний контейнер з послідовністю. Це можна досягти або делегуючи існуючий метод GetEnumerator або створити власний використовуючи yeild.