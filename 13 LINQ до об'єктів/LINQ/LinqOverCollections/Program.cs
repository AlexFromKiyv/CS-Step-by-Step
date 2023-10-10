using LinqOverCollections;

List<Car> myCar = new()
{
    new Car{ PetName = "Henry", Color = "Silver", Speed = 100, Make = "BMW"},
    new Car{ PetName = "Daisy", Color = "Tan", Speed = 90, Make = "BMW"},
    new Car{ PetName = "Mary", Color = "Black", Speed = 55, Make = "VW"},
    new Car{ PetName = "Clunker", Color = "Rust", Speed = 5, Make = "Yugo"},
    new Car{ PetName = "Melvin", Color = "White", Speed = 43, Make = "Ford"}
};

void CollectionToConsole<T>(IEnumerable<T>? collection)
{
    if (collection == null) return;

    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
}

void UseLinqForComplexObject()
{
    GetFastCars(myCar);

    void GetFastCars(List<Car> cars)
    {
        var queryFastCars = from c in cars where c.Speed > 55 select c;

        CollectionToConsole(queryFastCars);
    }
}

//UseLinqForComplexObject();

void UseLinqForComplexObjectWithComplexCriteria()
{
    GetFastCars(myCar);

    void GetFastCars(List<Car> cars)
    {
        var queryFastCars = from c in cars where c.Speed > 90 && c.Make == "BMW" select c;

        CollectionToConsole(queryFastCars);
    }
}

UseLinqForComplexObjectWithComplexCriteria();