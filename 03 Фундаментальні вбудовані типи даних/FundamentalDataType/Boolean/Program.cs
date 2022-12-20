ExplorationOfBooleanType();

static void ExplorationOfBooleanType()
{
    bool myBool = default;
    Console.WriteLine($"Default: {myBool}");
    Console.WriteLine($"Type in: {myBool.GetType()}");
    Console.WriteLine($"Representation to string: {bool.TrueString}, {bool.FalseString} ");
}

