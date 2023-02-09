//CreateTuples();


using System.Security.Cryptography.X509Certificates;

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

//ComparationTuples();
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

//UsingTuples();
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

//UsingTupleInSwitch();
void UsingTupleInSwitch()
{
    string result = GetNaturalConditions(("high", "low"));
    Console.WriteLine(result);

    result = GetNaturalConditions(("no", "no"));
    Console.WriteLine(result);

    result = GetNaturalConditions(("high", "no"));
    Console.WriteLine(result);


    string GetNaturalConditions((string wind, string snow) values)
    {
        return values switch
        {
            ("high", "high") => "Worst",
            ("high", "low") => "Bad",
            ("low", "no") => "Normal",
            ("no","no") => "Good",
            (_, _) => "Did not understand the data"
        };
    }
}

//DeconstructingTuple();
void DeconstructingTuple(){

    (string Name, int? Height, int? Weight) girl = ("Olga", 180, 80);
    Console.WriteLine(girl);

    string name;
    int? height;
    int? weight;

    (name, height, weight) = girl;

    Console.WriteLine($"{name} {height} {weight}");


    (string yourName,int? yourHeight, _) = girl;

    Console.WriteLine($"{yourName} {yourHeight}");
}


DeconstructingTupleWithStruct();
void DeconstructingTupleWithStruct()
{
    Point point = new Point(10, 5);

    int a;
    int b;
    (a, b) = point.Deconstruct();
    Console.WriteLine($"{a} {b}");

    int c1;
    int c2;
    (c1, c2) = point;
    Console.WriteLine($"{c1} {c2}");

}

struct Point
{
    public int X;
    public int Y;
    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public (int x, int y) Deconstruct() => (X, Y);

    internal void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }
}
