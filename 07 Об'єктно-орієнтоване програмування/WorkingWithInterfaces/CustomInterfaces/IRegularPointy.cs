namespace CustomInterfaces;

interface IRegularPointy : IPointy
{
    int SideLength { get; set; }
    int NumberOfSides { get; set; }

    int Perimeter => SideLength * NumberOfSides;

    //Static members are also allowed 
    static string? ExampleProperty { get; set; }

    static IRegularPointy()
    {
        ExampleProperty = "Something";
    }
}
