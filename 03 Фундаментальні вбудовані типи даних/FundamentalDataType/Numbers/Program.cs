ExplorationOfNumbers();
static void ExplorationOfNumbers()
{
    Console.WriteLine("int --------------");
    int i = 1_000;
    Console.WriteLine(i);
    Console.WriteLine(i.GetType());
    Console.WriteLine("Min: "+int.MinValue);
    Console.WriteLine("Max: "+int.MaxValue);

    Console.WriteLine("long --------------");
    long l = 100_000_000_000L;
    Console.WriteLine(l);
    Console.WriteLine(l.GetType());
    Console.WriteLine("Min: " + long.MinValue);
    Console.WriteLine("Max: " + long.MaxValue);

    Console.WriteLine("float -------------");
    float f = 100_000.12345F;
    Console.WriteLine(f);
    Console.WriteLine(f.GetType());
    Console.WriteLine("Min: " + float.MinValue);
    Console.WriteLine("Max: " + float.MaxValue);

    Console.WriteLine("double -------------");
    double d = 100_000.12345;
    Console.WriteLine(d);
    Console.WriteLine(d.GetType());
    Console.WriteLine("Min: " + double.MinValue);
    Console.WriteLine("Max: " + double.MaxValue);
    Console.WriteLine(double.PositiveInfinity);
    Console.WriteLine(double.NegativeInfinity);


    Console.WriteLine("decimal -------------");
    decimal m = 100_000.12345M;
    Console.WriteLine(m);
    Console.WriteLine(m.GetType());
    Console.WriteLine("Min: " + decimal.MinValue);
    Console.WriteLine("Max: " + decimal.MaxValue);
}
