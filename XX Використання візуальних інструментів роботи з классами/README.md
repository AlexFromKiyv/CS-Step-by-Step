Visual Studio має візуальні інструменти проектування класів.

# Перевірка установки Class Designer

Перед використанням треба виконати преревірку. 

1. Запустіть Visual Studio Instaler.
2. Modify
3. Individual component
4. В розділі Code Tools повинна бути выдмічено Class Designer

# Додавання класу в проект

1. Створимо рішення з проектом ClassDesigner типу консольний проект.
2. На значку проекта правий клік Add > New Item > Class Diagram 
3. Назву поміняйте на VehiclesDiagram.cd 
5. Add 
6. На значку проекта правий клік Add > New Item > Class 
7. Назву поміняйте на Vehile.cs
8. Add
9. Заменіть Vehile.cs
```cs
    public class Vehile
    {
        public string Name { get; set; }
        public string Producer { get; set; }

        public Vehile(string name, string producer)
        {
            Name = name;
            Producer = producer;
        }
    }
```
10. Претягніть клас Vehile.cs на поле VehiclesDiagram.cd 

Натисканючи на подвійній стілці ви можете побачити складові класу.
При натискані на Toolbox можна побачити типи які можна добавити а також звязки між типами.

# Успадкування.

1. З панелі Toolbox перетягніть Class
2. Введить назву Car.cs
3. В панелі Toolbox віберіть Inheritance.
4. Натисніть на верхню частину зображення класу Car
5. Натисніть на верхню частину зображення класу Vehile(ви повині побачите звя'зок між класами)
7. В Class Details додайте метод Name:GetInfo, Type:string. 
6. Замініть клас Car.cs
```cs
    public class Car : Vehile
    {
        public Car(string name, string producer) : base(name, producer)
        {
        }

        public int Price { get; set; }

        public string GetInfo()
        {
            return $"Car:{Producer} - {Name} Price:{Price}";
        }
    }
```
7.Заменіть класс проекту Program.cs
```cs

var car = new Car("Golf II", "VW");
car.Price= 400;

Console.WriteLine(car.GetInfo());

```

Таким чином можно визуально працювати з класами.
