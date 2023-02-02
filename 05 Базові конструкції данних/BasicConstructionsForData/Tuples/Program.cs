
//CreateTuples();
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

ComparationTuples();
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