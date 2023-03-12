# Partial class.

Клас можна розділити на різні файли. Розділення класу не перезаписує код а скоріше слугує розділенням класу на логічні частини для потреб розуміння і керування або для генерації коду. Можна розділити код на той шо є шаблонним і той якій більш складніший і корисніший. Проект Partial

```cs
    partial class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }

        public Person(int id, string name, double height, double weight)
        {
            Id = id;
            Name = name;
            Height = height;
            Weight = weight;
        }
        public Person() 
        {
            Name = "Name is no known";
        }
    }
```
```cs
    partial class Person
    {
        public void ToConsole()
        {
            Console.WriteLine($"Id:{Id}\nName:{Name}\n");
        }
        public double BodyMassIndex() => Weight / (Height * Height);

        public void BodyMassIndexToConsol() => Console.WriteLine(BodyMassIndex());    

    }
```
```cs
using Partial;

Person girl = new(1,"Julia",1.65,65);

girl.ToConsole();
girl.BodyMassIndexToConsol();
```
```
Id:1
Name:Julia

23,875114784205696
```

Перoш за все зверніть увагу на ключеве слово partial в обох файлах класу. Вимога назва типу в класах повина співпадати і було в одному просторі імен. Таким чином клас поділено на ту частину яка білше змінюється і яка майже не змінюеться.



