# System.Collections.ObjectModel

Аби зрозуміти механізми цього класу, крім розуміння узагальнень,потібно знати як працюють делегати, події та як використовується клас EventArgs. Аби краще зрозуміти цю тему краше до неї підійти після ознайомлення з розділом "Делегати, події, лямбда вирази".

Простір імен System.Collections.ObjectModel невеликий і містить декілька класів орієнтованих на колекції. Два класи про які безумовно слід знати.

    ObservableCollection<T> : Представляє динамічну колекцію даних, яка надає сповіщеня, коли елементи додаються, видаляються або коли весь список оновлюється.    

    ReadOnlyObservableCollection<T> : Представляє read-only версію ObservableCollection<T>.

Клас ObservableCollection<T> корисний тим, що він має можливість інформувати зовнішні об'єкти коли в його колеції відбуваються зміни.(ReadOnlyObservableCollection<T> має такіж можливості але тільки для читання).

## Робота з ObservableCollection<T> .

В більшості робота з ObservableCollection<T> аналогічна роботі з List<T>, вразовуючи шо основні з реалізованих інтерфейсів співпадають. Що робить клас ObservableCollection<T> унікальним, так це те, що він підтримує подію CollectionChanged. Ця подія виникає завжди коли відувається додавання, видалення елемента або бульяка модіфікація колекції. 
Як і будь яка подія CollectionChanged визначається в термінах делегату, який в цьому випадку є NotifyCollectionChangedEventHandler. Цей делегат може визвати будь який метод який приймає об'єкт як перший параметр і NotifyCollectionChangedEventArgs як другий.
Припустимо ми маємо клас.
```cs
    internal class Person
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public int Age { get; set; }

        public Person(string firstName = "", string lastName = "", int age = 0)
        {
            FirstName = firstName;
            LastName = lastName;
            Age = age;
        }

        public override string? ToString()
        {
            return $"{FirstName}\t{LastName}\t{Age}\t";
        }
    }
```
Також ми маємо допоміжний метод
```cs
// Auxiliary method to print a collection.
void CollectionToConsole(IList<Person> collection)
{
    Console.WriteLine("\n\tCollection"  );
    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
} 
```
Використаємо їх для роботи та дослідження колекцією .
```cs
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using WorkingWithObservableCollection;

void UseObservableCollection()
{
    ObservableCollection<Person> people = new()
    {
        new("Sveta","Kulik",25),
        new("Vera","Tuliy",29),
    };

    CollectionToConsole(people);

    people.CollectionChanged += People_CollectionChanged;

    people.Add(new("Olga", "Homenko", 30));
    people.RemoveAt(1);
    people.Move(0, 1);
    people.Move(0, 1);
    CollectionToConsole(people);
    people.Clear();
    CollectionToConsole(people);
}

UseObservableCollection();

// Event handler
void People_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
{
    Console.WriteLine($"\n\tAction:{e.Action}");

    if (e.Action == NotifyCollectionChangedAction.Add)
    {
        if (e.NewItems != null)
        {
            Console.WriteLine("\tNew items");
            foreach (var item in e.NewItems)
            {
                Console.WriteLine(item);
            }
        }
    }

    if (e.Action == NotifyCollectionChangedAction.Remove)
    {
        if (e.OldItems != null)
        {
            Console.WriteLine("\tRemove items");
            foreach (var item in e.OldItems)
            {
                Console.WriteLine(item);
            }
        }
    }

    if (e.Action == NotifyCollectionChangedAction.Reset)
    {
        Console.WriteLine("The list is cleared.");
    }
}
```
```

        Collection
Sveta   Kulik   25
Vera    Tuliy   29

        Action:Add
        New items
Olga    Homenko 30

        Action:Remove
        Remove items
Vera    Tuliy   29

        Action:Move

        Action:Move

        Collection
Sveta   Kulik   25
Olga    Homenko 30

        Action:Reset
The list is cleared.

        Collection
```

В прикладі колекція заповнюється як звичайний List<T>. Далі назначається обробник події CollectionChanged. 
Вхідний параметр обробника NotifyCollectionChangedEventArgs визначає дві важливі властивості, OldItems та NewItems, що нададуть вам список елементів які були до події та нових елементів залучених до зміни. Подія CollectionChanged може запускатися, коли елементи додаються, видаляються, переміщуються або скидаються. Щоб дізнатися яка подія виникнула можна скористатися властивістю Action об'екта NotifyCollectionChangedEventArgs.

```cs
    //
    // Summary:
    //     Describes the action that caused a System.Collections.Specialized.INotifyCollectionChanged.CollectionChanged
    //     event.
    public enum NotifyCollectionChangedAction
    {
        //
        // Summary:
        //     An item was added to the collection.
        Add = 0,
        //
        // Summary:
        //     An item was removed from the collection.
        Remove = 1,
        //
        // Summary:
        //     An item was replaced in the collection.
        Replace = 2,
        //
        // Summary:
        //     An item was moved within the collection.
        Move = 3,
        //
        // Summary:
        //     The contents of the collection changed dramatically.
        Reset = 4
    }

```
