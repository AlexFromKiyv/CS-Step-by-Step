using Pointers;

unsafe
{
    // Work with pointers.
}


void UseUnsafeMethod()
{
    static unsafe void SquareIntPointer(int* myIntPointer)
    {
        *myIntPointer *= *myIntPointer;
    }

    unsafe
    {
        int myInt = 10;
        SquareIntPointer(&myInt);
        Console.WriteLine(myInt);
    }

    int otherInt = 20; 

    //SquareIntPointer(&otherInt); // ...may only be used in an unsafe context
}

//UseUnsafeMethod();

unsafe void UsePointersOperators()
{

    static unsafe void PrintValueAndAddress()
    {
        int myInt;

        // Define an int pointer, and
        // assign it the address of myInt.
        int* myIntPointer = &myInt;

        Console.WriteLine($"myInt: {myInt}");
        Console.WriteLine(*myIntPointer);
        Console.WriteLine($"Adress: {(int)&myIntPointer:X}");

        // Assign value of myInt using pointer indirection.
        *myIntPointer = 123; 

        Console.WriteLine($"myInt: {myInt}");
        Console.WriteLine(*myIntPointer);
        Console.WriteLine($"Adress: {(int)&myIntPointer:X}");
    }

    PrintValueAndAddress();
}

//UsePointersOperators();


unsafe void SafeAndUnsafe()
{
    int i = 5, j = 10;
    PrintTwoInts(i, j);

    SafeSwap(ref i, ref j);
    PrintTwoInts(i, j);

    unsafe
    {
        UnSafeSwap(&i, &j);
    }
    PrintTwoInts(i, j);

    unsafe static void UnSafeSwap(int* x, int* y)
    {
        int temp = *x;
        *x = *y;
        *y = temp;
    }

    static void SafeSwap(ref int x, ref int y )
    {
        int temp = x;
        x = y;
        y = temp;
    }

    void PrintTwoInts(int x, int y)
    {
        Console.WriteLine($"x:{x} y:{y}");
    }

}

//SafeAndUnsafe();


unsafe void AccessToPropertyViaPointer()
{
    Point point = new();
    Console.WriteLine(point);
    Point* pointPointer = &point;
    pointPointer->x = 10;
    pointPointer->y = 20;
    Console.WriteLine(pointPointer->ToString());
 
    Console.WriteLine(*pointPointer);
    
    (*pointPointer).x = 100;
    (*pointPointer).y = 200;
    Console.WriteLine(point);

}

//AccessToPropertyViaPointer();


unsafe void UseStackalloc()
{
    Console.WriteLine(UnsafeStackAlloc());

    static unsafe string UnsafeStackAlloc()
    {
        char* p = stackalloc char[58];
        for (int k = 0; k < 58; k++)
        {
            p[k] = (char)(k + 65);
        }
        return new string(p);
    }
}

//UseStackalloc();

unsafe static void UseFixed()
{
    PointRef point = new PointRef { x = 1, y = 2 };


    // Pin point in place so it will not
    // be moved or GC-ed.
    fixed (int* pointerOnX = &point.x)
    {
        Console.WriteLine(point.x);
        point.x = 2;
        *pointerOnX = 3;
    }
    // point is now unpinned, and ready to be GC-ed once
    // the method completes.
    Console.WriteLine(point);
}

//UseFixed();


void UseSizeOf()
{
    Console.WriteLine(sizeof(short));
    Console.WriteLine(sizeof(int));
    Console.WriteLine(sizeof(decimal));
}

//UseSizeOf();

unsafe void UseSizeOfForYourTypes()
{
    unsafe
    {
        Console.WriteLine(sizeof(PointRef));
    }
}

UseSizeOfForYourTypes();