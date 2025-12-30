using ObservableCollections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

void WorkWithObservableCollection()
{
    // Make a collection to observe and add a few Person objects.
    ObservableCollection<Person> people = new ObservableCollection<Person>()
    {
        new Person {FirstName = "Peter", LastName = "Murphy", Age = 52},
        new Person {FirstName = "Kevin", LastName = "Key", Age = 48},
    };

    // Wire up the CollectionChanged event.
    people.CollectionChanged += people_CollectionChanged;

    ShowCollection();

    // Now add a new item.
    people.Add(new Person("Fred", "Smith", 32));
    Console.WriteLine();

    ShowCollection();

    // Remove an item.
    people.RemoveAt(0);
    Console.WriteLine();

    ShowCollection();

    void ShowCollection()
    {
        // All Collection
        foreach (var p in people)
        {
            Console.WriteLine($"\t{p.ToString()}");
        }
        Console.WriteLine();
    }
}
WorkWithObservableCollection();


void people_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
{
    // What was the action that caused the event?
    Console.WriteLine($"Action for this event: {e.Action}");

    // They removed something.
    if (e.Action == NotifyCollectionChangedAction.Remove && e.OldItems != null)
    {
        Console.WriteLine("Here are the OLD items:");
        foreach (Person p in e.OldItems)
        {
            Console.WriteLine(p.ToString());
        }
        Console.WriteLine();
    }

    // They added something.
    if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
    {
        // Now show the NEW items that were inserted.
        Console.WriteLine("Here are the NEW items:");
        foreach (Person p in e.NewItems)
        {
            Console.WriteLine(p.ToString());
        }
    }
}


