using Conversions;

void ImplicitAndExplicitConversionsNumeric()
{
    int a = 1;
    long b = a;      // Implicit conversion
    int c = (int)b;  // Explicit conversion
}

void ImplicitAndExplicitConversionsClasses()
{
    Base baseThing = new Derived() {Id = 1, Name="Drone" }; // Implicit conversion

    Console.WriteLine(baseThing.Id);
    // baseThing have no Name

    Derived derivedThing = (Derived)baseThing; // Explicit conversion
    Console.WriteLine(derivedThing.Id);
    Console.WriteLine(derivedThing.Name);

    Base otherThing = new() { Id = 2 };
    Console.WriteLine(otherThing.Id);

    //Unhandled exception. System.InvalidCastException
    //Derived derivedOtherThing = (Derived)otherThing; 
    //Console.WriteLine(derivedOtherThing.Id);
    //Console.WriteLine(derivedOtherThing.Name);

    Console.WriteLine(otherThing as Derived is null);
    
}

//ImplicitAndExplicitConversionsClasses();


void UseCustomExplicitConversion()
{
    Rectangle rectangle = new(10, 20);

    Console.WriteLine(rectangle);

    Console.WriteLine((Square)rectangle);
}

//UseCustomExplicitConversion();

void UseAdditionExplicitConversion()
{
    Rectangle rectangle = new(25,4);
   
    Console.WriteLine(rectangle);

    Square_v1 square = (Square_v1)rectangle;

    Console.WriteLine(square);

    double length = (double)square;

    Console.WriteLine(length);

    Console.WriteLine((Square_v1)length);
}

//UseAdditionExplicitConversion();

void UseImlicitConversion()
{
    Square square = new(10);

    Rectangle_v1 rectangle = square;

    Console.WriteLine(rectangle);

    Rectangle_v1 rectangle1 = (Rectangle_v1)square;
    
    Console.WriteLine(rectangle1);

}

UseImlicitConversion();