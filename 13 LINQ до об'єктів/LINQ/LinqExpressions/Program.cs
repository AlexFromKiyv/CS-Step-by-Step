// This array will be the basis of our testing...
using LinqExpressions;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;

ProductInfo[] itemsInStock = new[] 
{
    new ProductInfo{ Name = "Mac's Coffee", Description = "Coffee with TEETH", NumberInStock = 24},
    new ProductInfo{ Name = "Milk Maid Milk", Description = "Milk cow's love", NumberInStock = 100},
    new ProductInfo{ Name = "Pure Silk Tofu", Description = "Bland as Possible", NumberInStock = 120},
    new ProductInfo{ Name = "Crunchy Pops", Description = "Cheezy, peppery goodness", NumberInStock = 2},
    new ProductInfo{ Name = "RipOff Water", Description = "From the tap to your wallet", NumberInStock = 100},
    new ProductInfo{ Name = "Classic Valpo Pizza", Description = "Everyone loves pizza!",  NumberInStock = 73}
};

void CollectionToConsole<T>(IEnumerable<T>? collection)
{
    if (collection == null) return;

    foreach (var item in collection)
    {
        Console.WriteLine(item);
    }
    Console.WriteLine();
}
//CollectionToConsole(itemsInStock);

void SelectAllContainer()
{
    SelectEverything(itemsInStock);

    void SelectEverything(ProductInfo[] products)
    {
        //Get everything.
        var allProducts = from p in products select p;
        CollectionToConsole(allProducts);
    }       
}

//SelectAllContainer();

void SelectAllNames()
{
    SelectEveryNames(itemsInStock);

    void SelectEveryNames(ProductInfo[] products)
    {
        //Get names.
        var allNames = from p in products select p.Name;
        CollectionToConsole(allNames);
    }
}

//SelectAllNames();

void UseOperatorWhere()
{
    SelectOverstock(itemsInStock,25);

    void SelectOverstock(ProductInfo[] products, int quantity)
    {
        var overstock =
            from p in products
            where p.NumberInStock > quantity
            select p;

        CollectionToConsole(overstock);
    }
}

//UseOperatorWhere();

void UseOperatorWhereWithComplexClause()
{
    SelectWithMilk(itemsInStock, 0);

    void SelectWithMilk(ProductInfo[] products, int quantity)
    {
        var overstock =
            from p in products
            where p.NumberInStock > quantity && p.Name.Contains("Milk")
            select p;

        CollectionToConsole(overstock);
    }
}
//UseOperatorWhereWithComplexClause();

void UseWhereForComplexObject()
{
    List<Person> people = new()
    {
        new("Fedja",25,new(){"Russian"}),
        new("Anna",40,new(){"Russian","Deutch"}),
        new("Julia",30,new(){"Russian","Ukraine","English" }),
        new("Sava",28,new(){"Russain","Deutch"}),
        new("Olga",25,new(){ "Ukrainian", "English","Russian"}),
        new("Mikola",25,new(){ "Ukrainian", "English"}),
        new("Alex",30,new(){ "Ukrainian", "English","Russian","C#"})
    };

    var selected = from person in people
                   from language in person.Languages
                   where person.Age < 26
                   where language == "English"
                   select person.Name;

    CollectionToConsole(selected);

}
//UseWhereForComplexObject();


void UseTake()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithTake(itemsInStock, 2);

    void SelectWithTake(ProductInfo[] products, int quantity)
    {
        var query = from p in products select p;
        var result = query.Take(quantity); //!!!
        CollectionToConsole(result);
    }
}
//UseTake();

void UseTakeWhile()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithTakeWhile(itemsInStock, 20);

    void SelectWithTakeWhile(ProductInfo[] products, int quantityProduct)
    {
        var query = from p in products select p;
        var result = query.TakeWhile(sp => sp.NumberInStock > quantityProduct);//!!!
        CollectionToConsole(result);
    }
}
//UseTakeWhile();

void UseTakeLast()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithTakeLast(itemsInStock, 2);

    void SelectWithTakeLast(ProductInfo[] products, int count)
    {
        var query = from p in products select p;
        var result = query.TakeLast(count);//!!!
        CollectionToConsole(result);
    }
}

//UseTakeLast();

void UseSkip()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithTake(itemsInStock, 2);

    void SelectWithTake(ProductInfo[] products, int quantity)
    {
        var query = from p in products select p;
        var result = query.Skip(quantity); //!!!
        CollectionToConsole(result);
    }
}

//UseSkip();

void UseSkipWhile()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithSkipWhile(itemsInStock, 20);

    void SelectWithSkipWhile(ProductInfo[] products, int quantityProduct)
    {
        var query = from p in products select p;
        var result = query.SkipWhile(sp => sp.NumberInStock > quantityProduct);//!!!
        CollectionToConsole(result);
    }
}

//UseSkipWhile();

void UseSkipLast()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithSkipLast(itemsInStock, 2);

    void SelectWithSkipLast(ProductInfo[] products, int count)
    {
        var query = from p in products select p;
        var result = query.SkipLast(count);//!!!
        CollectionToConsole(result);
    }
}

//UseSkipLast();


void UseSkipAndTake()
{
    CollectionToConsole(itemsInStock);
    Console.WriteLine("\n");

    SelectWithSkipAndTake(itemsInStock, 2, 2);

    void SelectWithSkipAndTake(ProductInfo[] products, int skip, int take)
    {
        var query = from p in products select p;
        var result = query.Skip(skip).Take(take);//!!!
        CollectionToConsole(result);
    }
}

//UseSkipAndTake();


void PagingWithRanges()
{
    IEnumerable<ProductInfo> selectedProducts;
    var queryForSelectedProduct = from p in itemsInStock select p;


    selectedProducts = queryForSelectedProduct.Take(..3);
    WriteResult("The first three item",selectedProducts);

    selectedProducts = queryForSelectedProduct.Take(3..);
    WriteResult("Skippint the first three", selectedProducts);

    selectedProducts = queryForSelectedProduct.Take(3..5);
    WriteResult("Skip three take two", selectedProducts);

    selectedProducts = queryForSelectedProduct.Take(^2..);
    WriteResult("The last two", selectedProducts);

    selectedProducts = queryForSelectedProduct.Take(..^2);
    WriteResult("Skip the last two", selectedProducts);

    void WriteResult(string message, IEnumerable<ProductInfo> products)
    {
        Console.Clear();
        Console.WriteLine("\tAll product");
        CollectionToConsole(itemsInStock);

        Console.WriteLine("\n\t"+message);
        CollectionToConsole(products);
        Console.ReadLine();
    }
}

//PagingWithRanges();


void PagingWithChunks()
{
    var queryForSelectedProduct = from p in itemsInStock select p;

    IEnumerable<ProductInfo[]> chunks = queryForSelectedProduct.Chunk(2);

    var counter = 1;
    foreach (var item in chunks)
    {
        WriteResult($"Chunk {counter}", item);
        counter++;
    }

    void WriteResult(string message, IEnumerable<ProductInfo> products)
    {
        Console.Clear();
        Console.WriteLine("\tAll product");
        CollectionToConsole(itemsInStock);

        Console.WriteLine("\n\t" + message);
        CollectionToConsole(products);
        Console.ReadLine();
    }
}

//PagingWithChunks();

void ProjectingNewDataType()
{
    GetNameAndDescription(itemsInStock);

    void GetNameAndDescription(ProductInfo[] products)
    {
        var prodeuctNameAndDescription =
            from p in products
            select new { p.Name, p.Description };

        CollectionToConsole(prodeuctNameAndDescription);
        Console.WriteLine("\n"+prodeuctNameAndDescription.GetType());
    }
}
//ProjectingNewDataType();

void ReturnProjection()
{
    var arrayOfNewType = GetNameAndDescription(itemsInStock);

    foreach (var item in arrayOfNewType)
    {
        Console.WriteLine(item);
    }

    Array GetNameAndDescription(ProductInfo[] products)
    {
        var productNameAndDescription =
            from p in products
            select new { p.Name, p.Description };

        return productNameAndDescription.ToArray();

    }
}

//ReturnProjection();

void ProjectionWithProductNameDescription()
{
    CollectionToConsole( GetNameAndDescription(itemsInStock) );

    

    IEnumerable<ProductNameDescription> GetNameAndDescription(ProductInfo[] products)
    {
        var productNameAndDescription =
            from p in products
            select new ProductNameDescription { Name = p.Name, Description = p.Description };
        return productNameAndDescription;
    }
}

//ProjectionWithProductNameDescription();

void UseLet()
{
    List<Car> garage = new List<Car>
    {
        new("VW","T2",1995),
        new("VW","Caddy",2001),
        new("VW","LT",2001),
        new("Mercedes","Sprinter",1998),
        new("Mercedes","Vaito",2000)
    };

    CollectionToConsole(garage);
    Console.WriteLine();

    var otherGarage = from c in garage
                      let model = $"{c.Manufacturer} {c.Name}"
                      let age = DateTime.Now.Year - c.Year
                      where age > 23
                      select new
                      {
                          Model = model,
                          Age = age
                      };
    CollectionToConsole(otherGarage);
}
//UseLet();


void UseEnumerableMethodCount()
{
    string[] games = { "Morrowind", "Uncharted 2", "Fallout 3", "Daxter", "System Shock 2" };

    var queryNameBigerThan6 =
        from g in games
        where g.Length > 6
        select g;

    CollectionToConsole(games);
    Console.WriteLine("\n");

    CollectionToConsole(queryNameBigerThan6);
    Console.WriteLine("\n");

    Console.WriteLine(GetCount(queryNameBigerThan6));

    int GetCount<T>(IEnumerable<T> collection)
    {
        return collection.Count();
    }
}
//UseEnumerableMethodCount();

void UseTryGetNonEnumeratedCount()
{
    WriteCount(itemsInStock);

    void WriteCount(ProductInfo[] products)
    {
        var query = from p in products select p;
        bool result = query.TryGetNonEnumeratedCount(out int count);
        if (result)
        {
            Console.WriteLine(count);
        }
        else
        {
            Console.WriteLine("Try get count failed."  );
        }
    }

}
//UseTryGetNonEnumeratedCount();

void NoWorkTryGetNonEnumeratedCount()
{
    var collection = GetProducts(itemsInStock);

    bool result = collection.TryGetNonEnumeratedCount(out int count);

    if (result)
    {
        Console.WriteLine(count);
    }
    else
    {
        Console.WriteLine("Try get count failed.");
    }

    Console.WriteLine(collection.Count());

    static IEnumerable<ProductInfo> GetProducts(ProductInfo[] products)
    {
        for (int i = 0; i < products.Length; i++)
        {
           yield return products[i];
        }
    }
}

//NoWorkTryGetNonEnumeratedCount();

void UseReverse()
{
    CollectionToConsole(itemsInStock);

    Console.WriteLine("\n");

    CollectionToConsole( SelectWithRevers(itemsInStock) );


    IEnumerable<ProductInfo> SelectWithRevers(ProductInfo[] products)
    {
        var queryForAllProduct =
            from p in products
            select p;

        return queryForAllProduct.Reverse();
    }
}

//UseReverse();


void UseOrderByName()
{
    CollectionToConsole(itemsInStock);

    Console.WriteLine("\n");

    CollectionToConsole(SelectWithOrderby(itemsInStock));

    IEnumerable<ProductInfo> SelectWithOrderby(ProductInfo[] products)
    {
       var queryForAllWithSorting =
            from p in products
            orderby p.Name
            select p;

        return queryForAllWithSorting;
    }
}

//UseOrderByName();


void UseOrderByNameDescending()
{
    CollectionToConsole(itemsInStock);

    Console.WriteLine("\n");

    CollectionToConsole(SelectWithOrderby(itemsInStock));


    IEnumerable<ProductInfo> SelectWithOrderby(ProductInfo[] products)
    {
        var queryForAllWithSorting =
             from p in products
             orderby p.Name descending
             select p;

        return queryForAllWithSorting;
    }
}
//UseOrderByNameDescending();

List<string> myCars = new List<string> { "Yugo", "Aztec", "BMW" };
List<string> yourCars = new List<string> { "BMW", "Saab", "Aztec" };

void ListCarToConsole<T>(IEnumerable<T> collection, string note = "Collection")
{
    Console.Write($"{note}\t:\t ");
    foreach (var item in collection)
    {
        Console.Write(item +"\t"  );
    }
    Console.WriteLine();
}


void SelectionFromTwoSource()
{
    List<Place> places = new()
    {
        new("Job"),
        new("Home")
    };

    List<Person> people = new()
    {
        new("Valja",25,null),
        new("Fedja",30,null)
    };

    var regularLife = from person in people
                      from place in places
                      select new { Person = person.Name, Place = place.Name };

    CollectionToConsole(regularLife);
}
//SelectionFromTwoSource();

void UseExcept()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars,"My    ");
    ListCarToConsole(queryYourCars, "Your  ");

    Console.WriteLine();

    var carDiff = queryMyCars.Except(yourCars);//!!!
    ListCarToConsole(carDiff, "Except");
}
//UseExcept();

void UseIntersect()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars, "My      ");
    ListCarToConsole(queryYourCars, "Your    ");

    Console.WriteLine();

    var carIntersect = queryMyCars.Intersect(yourCars);//!!!
    ListCarToConsole(carIntersect, "Intersect");
}
//UseIntersect();

void UseUnion()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars, "My  ");
    ListCarToConsole(queryYourCars, "Your ");

    Console.WriteLine();

    var carIntersect = queryMyCars.Union(yourCars);//!!!
    ListCarToConsole(carIntersect, "Union");
}
//UseUnion();

void UseConcat()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars, "My  ");
    ListCarToConsole(queryYourCars, "Your ");

    Console.WriteLine();

    var carConcat = queryMyCars.Concat(yourCars);//!!!
    ListCarToConsole(carConcat, "Concat");
}

//UseConcat();

void CollectionToConsoleInLine<T>(IEnumerable<T> collection, string aboutCollection)
{
    Console.Write(aboutCollection);
    foreach (var item in collection)
    {
        Console.Write(item + "\t");
    }
    Console.WriteLine();
}


void UseExceptWithSelector()
{
    var first = new (string Name, int Age)[] { ("Francis", 20), ("Lindsey", 30), ("Ashley", 40) };
    var second = new (string Name, int Age)[] { ("Claire", 30), ("Pat", 30), ("Drew", 33) };

    CollectionToConsoleInLine(first, "First     :");
    CollectionToConsoleInLine(second, "Second    :");

    var exceptBy = first.ExceptBy(second.Select(x => x.Age), fp => fp.Age); //!!!
    CollectionToConsoleInLine(exceptBy, "ExceptBy  :");

}
//UseExceptWithSelector();

void UseIntersectByWithSelector()
{
    var first = new (string Name, int Age)[] { ("Francis", 20), ("Lindsey", 30), ("Ashley", 40) };
    var second = new (string Name, int Age)[] { ("Claire", 30), ("Pat", 30), ("Drew", 33) };

    CollectionToConsoleInLine(first, "First     :");
    CollectionToConsoleInLine(second, "Second    :");

    var intersectBy = first.IntersectBy(second.Select(x => x.Age), fp => fp.Age); //!!!
    CollectionToConsoleInLine(intersectBy, "IntersectBy:");

}
//UseIntersectByWithSelector();

void UseDistinct()
{
    var queryMyCars =
        from c in myCars
        select c;
    var queryYourCars =
        from c in yourCars
        select c;
    ListCarToConsole(queryMyCars, "My          ");
    ListCarToConsole(queryYourCars, "Your        ");

    Console.WriteLine();

    var carConcat = queryMyCars.Concat(yourCars);
    ListCarToConsole(carConcat, "Concat     ");

    var carConacatDistinct = carConcat.Distinct();//!!! 
    ListCarToConsole(carConacatDistinct, "Concat Distinct");
}

//UseDistinct();

void UseDistinctWithSelector()
{
    var first = new (string Name, int Age)[] { ("Francis", 20), ("Lindsey", 30), ("Ashley", 40) };
    var second = new (string Name, int Age)[] { ("Claire", 30), ("Pat", 30), ("Drew", 33) };

    
    CollectionToConsoleInLine(first,"First     :");
    CollectionToConsoleInLine(second, "Second    :");

    var concat = first.Concat(second);
    CollectionToConsoleInLine(concat, "Concat    :");

    var concatDistinctBy = concat.DistinctBy(x => x.Age);
    CollectionToConsoleInLine(concatDistinctBy,"DistinctBy:");

}
//UseDistinctWithSelector();


void UseAggregateOperations()
{
    double[] winterTemperatures = { 2.0, -21.3, 8, -4, 0, 8.2 };

    PrintAll(winterTemperatures);

    var quertAllTemperatures = from t in winterTemperatures select t;

    double max = quertAllTemperatures.Max();
    PrintResult(max, "Max");

    double min = quertAllTemperatures.Min();
    PrintResult(min, "Min");

    double average = quertAllTemperatures.Average();
    PrintResult(average, "Average");

    double sum = quertAllTemperatures.Sum();
    PrintResult(sum, "Sum");


    void PrintResult(double value, string note)
    {
        Console.WriteLine($"{note}\t:{value}");
    }

    void PrintAll(double[] collection)
    {
        foreach (var item in collection)
        {
            Console.Write($"{item}\t");
        }
        Console.WriteLine();
    }
}
//UseAggregateOperations();


void UseAggregateOperationsWithSelector()
{

    CollectionToConsole(itemsInStock);

    Console.WriteLine("\n");

    Console.WriteLine("Product with maximum instock" );
    Console.WriteLine(SelectWithMaxBy(itemsInStock));
    Console.WriteLine("Product with minimum instock");
    Console.WriteLine(SelectWithMinBy(itemsInStock));

    ProductInfo? SelectWithMaxBy(ProductInfo[] products)
    {
       return products.MaxBy(p => p.NumberInStock);
    }
    ProductInfo? SelectWithMinBy(ProductInfo[] products)
    {
        return products.MinBy(p => p.NumberInStock);
    }

}
//UseAggregateOperationsWithSelector();


void UseGroupBy()
{
    Car[] garage =
    {
        new("VW","e-UP",2015),
        new("Mercedes","W164",2005),
        new("VW","Käfer",1937),
        new("ЗАЗ","ЗАЗ-1102 Таврія",1992),
        new("Mercedes","W123",1981),
        new("ЗАЗ","ЗАЗ-965",1965),
        new("VW","Golf",1975),
        new("ЗАЗ","Lanos",2010),
    };

    var carGroups = from car in garage
                    group car by car.Manufacturer;

    foreach (var group in carGroups)
    {
        Console.WriteLine("\t"+group.Key);
        CollectionToConsole(group);
        Console.WriteLine();
    }

    // same with method
    var carGroupByMethod = garage.GroupBy(c => c.Manufacturer);
}
//UseGroupBy();

void GroupingWithNewObjects()
{
    Car[] garage =
    {
        new("VW","e-UP",2015),
        new("Mercedes","W164",2005),
        new("VW","Käfer",1937),
        new("ЗАЗ","ЗАЗ-1102 Таврія",1992),
        new("Mercedes","W123",1981),
        new("ЗАЗ","ЗАЗ-965",1965),
        new("VW","Golf",1975),
        new("ЗАЗ","Lanos",2010),
    };

    var manufacturers = from car in garage
                    group car by car.Manufacturer into carGroup
                    select new { Manufacturer = carGroup.Key, Count = carGroup.Count() };
    
    CollectionToConsole(manufacturers);

    //same with method

    var manufacturerByMethod = garage
        .GroupBy(c => c.Manufacturer)
        .Select(g => new { Manufacturer = g.Key, Count = g.Count() });
}

//GroupingWithNewObjects();

void NestedQuery()
{
    Car[] garage =
    {
        new("VW","e-UP",2015),
        new("Mercedes","W164",2005),
        new("VW","Käfer",1937),
        new("ЗАЗ","ЗАЗ-1102 Таврія",1992),
        new("Mercedes","W123",1981),
        new("ЗАЗ","ЗАЗ-965",1965),
        new("VW","Golf",1975),
        new("ЗАЗ","Lanos",2010),
    };

    var garageByManufacturer = from car in garage
                               group car by car.Manufacturer into carGroup
                               select new
                               {
                                   Manufacturer = carGroup.Key,
                                   Count = carGroup.Count(),
                                   Cars = from c in carGroup select c
                               };

    foreach (var group in garageByManufacturer)
    {
        Console.WriteLine($"\tManufacturer:{group.Manufacturer} Count:{group.Count}");
        CollectionToConsole(group.Cars);
        Console.WriteLine();
    }

    //same with method
    var garageByManufacturerWithMethod = garage
        .GroupBy(c => c.Manufacturer)
        .Select(g => new
        {
            Manufacturer = g.Key,
            Count = g.Count(),
            Cars = g.Select(car => car)
        });
}

//NestedQuery();

void UseOperatorJoin()
{
    Product[] products =
    {
        new(1,"Jacket",100),
        new(2,"Shirt",15),
        new(3,"Head",20),
        new(4,"Toothbrash",2),
        new(5,"Eggs",2.5),
        new(6,"Bread",0.5)
    };

    List<Cart_item> cart = new()
    {
        new(1,3,1),
        new(2,5,1),
        new(3,6,2),
        new(4,10,1)
    };

    CollectionToConsole(cart);

    var cartAndProduct = from item in cart
                         join product in products
                         on item.Product_Id equals product.Id
                         select new
                         {
                             Item = item,
                             Product = product
                         };

    CollectionToConsole(cartAndProduct); Console.WriteLine();

    var purshase = from item in cart
                   join product in products
                   on item.Product_Id equals product.Id
                   select new
                   {
                       N = item.Id,
                       Name = product.Name,
                       Price = product.Price,
                       Quantity = item.Quantyty,
                       Amount = item.Quantyty * product.Price
                   };

    CollectionToConsole(purshase);

    var purshaseSum = purshase.Sum(i => i.Amount);
    Console.WriteLine(purshaseSum);
}
//UseOperatorJoin();

void UseMethodJoin()
{
    Product[] products =
    {
        new(1,"Jacket",100),
        new(2,"Shirt",15),
        new(3,"Head",20),
        new(4,"Toothbrash",2),
        new(5,"Eggs",2.5),
        new(6,"Bread",0.5)
    };

    List<Cart_item> cart = new()
    {
        new(1,3,1),
        new(2,5,1),
        new(3,6,2),
        new(4,10,1)
    };

    CollectionToConsole(cart);

    var purshase = cart.Join(
        products,
        item => item.Product_Id,
        product => product.Id,
        (item, product) => new
        {
            N = item.Id,
            Name = product.Name,
            Price = product.Price,
            Quantity = item.Quantyty,
            Amount = item.Quantyty * product.Price

        }
        );

    CollectionToConsole(purshase);

    Console.WriteLine(purshase.Sum(p=>p.Amount));
}
//UseMethodJoin();

void UseGroupJoin()
{
    Driver petja = new("Petro", 5);
    Driver viktor = new("Viktor", 3);
    Driver olga = new("Olga", 2);

    List<Driver> drivers = new() { petja, viktor, olga };

    List<Vehile> garage = new()
    {
        new("Mersedes Sprinter",petja),
        new("VW Caddy", viktor),
        new("Peugeot Partner", olga),
        new("Mersedes Vito",viktor),
        new("VW Transorter",petja)
    };

    var query = drivers.GroupJoin(
        garage,
        driver => driver,
        vehile => vehile.Owner,
        (d, vehileCollection) => new
        {
            Driver = d,
            Cars = vehileCollection.Select(c => c.Name)
        });

    foreach (var item in query)
    {
        Console.WriteLine(item.Driver);
        CollectionToConsole(item.Cars);
    }

    //same with operator
    var queryO = from person in drivers
                 join car in garage
                 on person equals car.Owner into g
                 select new
                 {
                     Driver = person,
                     Cars = from c in g select c.Name
                 };
}
//UseGroupJoin();

void UseAll()
{
    List<Person> people = new()
    {
        new("Olja",28,new()),
        new("Petja",20,new())
    };

    bool moreThanEighteen = people.All(p => p.Age >18);

    Console.WriteLine(moreThanEighteen);

    bool lengthNameIs3 = people.All(p => p.Name.Length == 3);

    Console.WriteLine(lengthNameIs3);

}
//UseAll();

void UseAny()
{
    List<Person> people = new()
    {
        new("Olja",18,new()),
        new("Petja",20,new())
    };

    bool IsSomeoneOlderEighteen = people.Any(p => p.Age > 18);

    Console.WriteLine(IsSomeoneOlderEighteen);

    bool IsShortName = people.Any(p => p.Name.Length == 3);

    Console.WriteLine(IsShortName);
}
//UseAny();

void UseConains()
{
    Person girl1 = new("Olga", 25, new());
    Person girl2 = new("Julia", 30, new());
    Person boy1 = new("Vova", 30, new());
    Person boy2 = new("Vitja", 28, new());

    List<Person> meeting = new()
    {
        girl1,girl2,boy1
    };

    bool IsSheOnMeeting = meeting.Contains(girl2);
    Console.WriteLine(IsSheOnMeeting);

    bool IsHeOnMeeting = meeting.Contains(boy2);
    Console.WriteLine(IsHeOnMeeting);
}
//UseConains();

void UseFirstAndFirstOrDefault()
{
    List<Person> people = new()
    {
        new("Ira",27,new()),
        new("Petro",32,new()),
        new("Mikola",62,new()),
        new("Olga",30,new()),
        new("Marina",35,new())
    };
    
    CollectionToConsole(people);

    Console.WriteLine("people.First()");
    Console.WriteLine(people.First());  Console.WriteLine("\n");

    Console.WriteLine("people.First(p => p.Age == 30)");
    Console.WriteLine(people.First(p => p.Age == 30)); Console.WriteLine("\n");

    Console.WriteLine("people.FirstOrDefault(p => p.Age == 40)"); 
    Console.WriteLine($"Default is null:"+(people.FirstOrDefault(p => p.Age == 40) is null)); Console.WriteLine("\n");

    Console.WriteLine("people.FirstOrDefault(p => p.Age == 40, new(\"Someone\", 40, new()))"); 
    Console.WriteLine(people.FirstOrDefault(p=>p.Age == 40,new("Someone",40,new()))); Console.WriteLine("\n");


    Console.WriteLine("people.First(p => p.Age == 40)");
    Console.WriteLine(people.First(p => p.Age == 40));

}
//UseFirstAndFirstOrDefault();