# Власні узагальнення (Generic).

Крім того шо можна використовувати готові узагальнені класи з бібліотеки, можна створювати власні класи. 

## Методи

Створення узагальнених методів це посилена версія перзавантаженя методу. Перезавантаженя звичайних методів це визначеня метода з одной і тою назвою але з різною кількістю або типом параметрів. Перезавантаженя корисна річ але може винукнити ситуація коли треба робити багато методів яки посуті виконують одне й те саме і відрізняються лише типом параметрів.

Generics\CustomGenericMethods\Types.cs
```cs
    static class SwapFunctions
    {
        static void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        static void Swap(ref Person personA,ref Person personB)
        { 
            Person tempPerson = personA;
            personA = personB;
            personB = tempPerson;
        }
    }
``` 
В цьому прикладі створені методи які міняють місцями дві частини даних за допомогою простого переписуваня. Наче все добре але методів з такою самою логікою може бути нароблено для дуже багатоєкількості типів. Обслуговування великої кількості методі може стати кошмаром.

Створення методу з використанням System.Object не дуже добре, оскілки тягне з собою проблеми упаковки/розпаковки та безпечності типів. 
Багато схожих перезавантажених методів спонукає використати узагальнення.
```cs
        static void Swap<T>(ref T a,  ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
```
В цому методі замість типу вказано параметр(заповнювач) <T> перед списком параметрів. Це означає шо метод оперує типом Т який буде визначений при використані.

```cs
void UseGenericMethod()
{
    int a = 35, b = 50;
    Console.WriteLine($"{a} {b}");
    SwapFunctions.Swap<int>(ref a,ref b);
    Console.WriteLine($"{a} {b}\n");

    string vechile1 = "Bus", vechile2 = "eCar";
    Console.WriteLine($"{vechile1} {vechile2}");
    SwapFunctions.Swap<string>(ref vechile1,ref vechile2);
    Console.WriteLine($"{vechile1} {vechile2}\n");

    Person person1= new Person(1,"Alex"), person2 = new(2,"Ira");
    Console.WriteLine($"{person1} {person2}");
    SwapFunctions.Swap<Person>(ref person1,ref person2);
    Console.WriteLine($"{person1} {person2}");

}

UseGenericMethod();
```
```
35 50

I am swaping two System.Int32

50 35

Bus eCar

I am swaping two System.String

eCar Bus

1 Alex Undefined 0       2 Ira Undefined 0

I am swaping two CustomGenericMethods.Person

2 Ira Undefined 0        1 Alex Undefined 0
```
Основан перевага створення узагальненного методу є шо увас є одна версія методу для підтримки, яка може працювати з єлементами потрібного вам типу з безпечним для типів способом.

