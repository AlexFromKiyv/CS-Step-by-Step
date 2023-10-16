// This array will be the basis of our testing...
using LinqExpressions;

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

ProjectionWithProductNameDescription();