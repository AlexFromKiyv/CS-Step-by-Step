
using CustomTypeConversions;

static void NumericConversions()
{
    int a = 123;
    long b = a;       // Implicit conversion from int to long.
    int c = (int)b;  // Explicit conversion from long to int.
    Console.WriteLine(c);
}
//NumericConversions();

static void ConversionRectangleTopSquare()
{
    // Make a Rectangle.
    Rectangle r = new Rectangle(15, 4);
    Console.WriteLine(r.ToString());
    r.Draw();

    Console.WriteLine();

    // Convert r into a Square,
    // based on the height of the Rectangle.
    Square s = (Square)r;
    Console.WriteLine(s.ToString());
    s.Draw();
}
//ConversionRectangleTopSquare();

static void ConversionRectangleTopSquare_1()
{
    // This method requires a Square type.
    static void DrawSquare(Square sq)
    {
        Console.WriteLine(sq.ToString());
        sq.Draw();
    }

    // Convert Rectangle to Square to invoke method.
    Rectangle rect = new Rectangle(10, 5);
    DrawSquare((Square)rect);

}
//ConversionRectangleTopSquare_1();

static void ConversionRectangleTopSquare_2()
{
    // Converting an int to a Square.
    Square square = (Square)90;
    Console.WriteLine($"square = {square}");

    // Converting a Square to an int.
    int side = (int)square;
    Console.WriteLine($"Side length of square = {side}");
}
//ConversionRectangleTopSquare_2();

static void Conversions()
{
    // Implicit cast OK!
    Square square = new Square { Length = 7 };
    Rectangle rectangle = square;
    Console.WriteLine($"rectangle = {rectangle}");
    
    // Explicit cast syntax still OK!
    square = new Square { Length = 3 };
    rectangle = (Rectangle)square;
    Console.WriteLine($"rectangle = {rectangle}");
}
Conversions();