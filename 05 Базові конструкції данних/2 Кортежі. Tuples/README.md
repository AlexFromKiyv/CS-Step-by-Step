# Кортежі. Tuples

Додамо проект Tuples

## Створення

Кортежі корисні коли декілька полів данних треба передати або отримати як одне ціле
```cs


CreateTuples();
void CreateTuples()
{
    var temperatures = (-2, -1, 0, 1, -1);
    Console.WriteLine(temperatures);

    (string, int) girl = ("Julia", 65);
    Console.WriteLine(girl);
    Console.WriteLine(girl.Item1);
    Console.WriteLine($"Weight:{girl.Item2}");

    (string Name, int Age) boy = ("John", 13);
    Console.WriteLine(boy);
    Console.WriteLine(boy.Name);
    Console.WriteLine(boy.Item1);

    //(string, int) boy = (Name: "Jhon", Age: 12); // don't work
    //Console.WriteLine(boy.Name);

    var emploeer = (Name: "Jerry", Age: 32);
    Console.WriteLine(emploeer.Name);

    var superGirl = ("Kira", (90, 60, 90));
    Console.WriteLine(superGirl);

    var terminator = new { Model = "101", Power = 2800 };
    var otherTerminator = (terminator.Model, terminator.Power);
    Console.WriteLine(otherTerminator);
}
```
Значення в кортежі не обов'язково повинні бути однакового типу.
Поля кортежів додадково неперевірені(not validated) і ви не можете для них зробити додадкові методи. Тобто вони слугуюдь тільки як механізм передачі данних.

## Порівняння кортежів

```cs

ComparationTuples();
void ComparationTuples()
{
    (int? a, int? b) tuple1 = (160, 60);
    var tuple2 = (a:160, b:60);

    Console.WriteLine(tuple1 == tuple2);

    (long, int?) tuple3 = (160, 60);
    Console.WriteLine(tuple2 == tuple3);

    (int, (int, int, int)) tuple4 = (35, (90, 60, 90));
    var tuple5 = (35, (90, 60, 90));
    Console.WriteLine(tuple4 == tuple5);
}
```
```
True
True
True
```
Під час порівнювання виконуються можливи неявні приведеня типів.

## Кортеж як значення шо повертає метод.

Для використання классів і структур для того шоб метод повернув декілька значень потрібно дододково розроляти їх. Крім того меньш єфективно пертворювати кілька значень в об'єкт а потім знову і значення. Для цілей транспортування декількох данних гарно підходить кортеж. 

```cs
UsingTuples();
void UsingTuples()
{

    var result1 = GetPersonCharacteristic(10);

    Console.WriteLine(result1);
    Console.WriteLine(result1.Name);
    Console.WriteLine(result1.Height);
    Console.WriteLine(result1.Weight);

    var (_,name,_,_) = GetPersonCharacteristic(10);
    Console.WriteLine(name);


    (int? Id, string? Name, int? Height, int? Weight) GetPersonCharacteristic(int? Id)
    {
        //get data from db
        return (Id, "Jerry", 170, 85);
    }
}
```
```
(10, Jerry, 170, 85)
Jerry
170
85
Jerry
```


