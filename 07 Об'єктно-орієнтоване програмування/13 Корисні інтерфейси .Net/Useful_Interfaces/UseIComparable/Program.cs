
using UseIComparable;

//ExplorationArraySort_1();
void ExplorationArraySort_1()
{
    Car[] cars = new Car[5];

    cars[0] = new Car(1, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car(3, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car(4, "VOLKSWAGEN Beetle 2011-2016", 180);
    cars[4] = new Car(5, "VOLKSWAGEN Beetle 2016-2019", 180);

    Array.Sort(cars);

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }
}

//ExplorationArraySort_2();
void ExplorationArraySort_2()
{
    Car_v1[] cars = new Car_v1[5];

    cars[0] = new Car_v1(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v1(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v1(1, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v1(4, "VOLKSWAGEN Beetle 2011-2016", 180);
    cars[4] = new Car_v1(5, "VOLKSWAGEN Beetle 2016-2019", 180);

    Array.Sort(cars);

    foreach (Car_v1 car in cars)
    {
        Console.WriteLine(car);
    }
}

//ExplorationArraySort_3();
void ExplorationArraySort_3()
{
    Car_v2[] cars = new Car_v2[5];

    cars[0] = new Car_v2(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v2(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v2(1, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v2(4, "VOLKSWAGEN Beetle 2011-2016", 180);
    cars[4] = new Car_v2(5, "VOLKSWAGEN Beetle 2016-2019", 180);

    Array.Sort(cars);

    foreach (Car_v2 car in cars)
    {
        Console.WriteLine(car);
    }
}


//ExplorationArraySort_4();
void ExplorationArraySort_4()
{
    Car_v3[] cars = new Car_v3[5];

    cars[0] = new Car_v3(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v3(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v3(5, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v3(4, "VOLKSWAGEN Beetle 2016-2019", 180);
    cars[4] = new Car_v3(1, "VOLKSWAGEN Beetle 2011-2016", 180);


    Array.Sort(cars);

    foreach (Car_v3 car in cars)
    {
        Console.WriteLine(car);
    }
}

//ExplorationArraySort_5();
void ExplorationArraySort_5()
{
    Car_v3[] cars = new Car_v3[5];

    cars[0] = new Car_v3(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v3(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v3(5, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v3(4, "VOLKSWAGEN Beetle 2016-2019", 180);
    cars[4] = new Car_v3(1, "VOLKSWAGEN Beetle 2011-2016", 180);

    Console.WriteLine("Sort by MaxSpeed");

    Array.Sort(cars);

    foreach (Car_v3 car in cars)
    {
        Console.WriteLine(car);
    }

    Console.WriteLine("\nSort by Name");
    Array.Sort(cars,new Car_v3NameComparer());

    foreach (Car_v3 car in cars)
    {
        Console.WriteLine(car);
    }
}


//ExplorationArraySort_6();
void ExplorationArraySort_6()
{
    Car[] cars = new Car[5];

    cars[0] = new Car(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car(5, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car(4, "VOLKSWAGEN Beetle 2016-2019", 180);
    cars[4] = new Car(1, "VOLKSWAGEN Beetle 2011-2016", 180);

    Console.WriteLine("\nSort by Name");
    Array.Sort(cars, new CarNameComparer());

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }

    Console.WriteLine("\nSort by Id");
    Array.Sort(cars, new CarIdComparer());

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }

    Console.WriteLine("\nSort by MaxSpeed");
    Array.Sort(cars, new CarMaxSppedComparer());

    foreach (Car car in cars)
    {
        Console.WriteLine(car);
    }
}

ExplorationArraySort_7();
void ExplorationArraySort_7()
{
    Car_v4[] cars = new Car_v4[5];

    cars[0] = new Car_v4(3, "VOLKSWAGEN Beetle 1945-2003", 100);
    cars[1] = new Car_v4(2, "VOLKSWAGEN Beetle 2001-2002", 225);
    cars[2] = new Car_v4(5, "VOLKSWAGEN Beetle 1998-2005", 161);
    cars[3] = new Car_v4(4, "VOLKSWAGEN Beetle 2016-2019", 180);
    cars[4] = new Car_v4(1, "VOLKSWAGEN Beetle 2011-2016", 180);

    Array.Sort(cars,Car_v4.SortByName);

    foreach (Car_v4 car in cars)
    {
        Console.WriteLine(car);
    }
}