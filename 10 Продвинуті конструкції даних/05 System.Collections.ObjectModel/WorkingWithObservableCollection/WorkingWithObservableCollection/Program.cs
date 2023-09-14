// See https://aka.ms/new-console-template for more information
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

    if (e.Action == NotifyCollectionChangedAction.Move)
    {
        if (e.OldItems != null)
        {
            Console.WriteLine("\tRemove items");
            foreach (var item in e.OldItems)
            {
                Console.WriteLine(item);
            }
        }
        if (e.NewItems != null)
        {
            Console.WriteLine("\tNew items");
            foreach (var item in e.NewItems)
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

// Auxiliary method to print a collection.
void CollectionToConsole(IList<Person> collection)
{
    Console.WriteLine("\n\tCollection"  );
    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
} 



