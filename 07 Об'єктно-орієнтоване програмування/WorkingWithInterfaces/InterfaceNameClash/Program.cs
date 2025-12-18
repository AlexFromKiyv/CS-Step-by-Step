using InterfaceNameClash;

void ExplicitInterfaceImplementation1()
{
    Octagon octagon = new Octagon();

    octagon.Draw();

    // Both of these invocations call
    // the same Draw() method!

    ((IDrawToForm)octagon).Draw();
    
    if (octagon is IDrawToPrinter)
    {
        octagon.Draw();
    }
}
//ExplicitInterfaceImplementation1();

void ExplicitInterfaceImplementation2()
{
    Octagon1 octagon1 = new Octagon1();

    //octagon1.Draw();

    // We now must use casting to access the Draw()
    // members.
    ((IDrawToForm)octagon1).Draw();

    ((IDrawToMemory)octagon1).Draw();

    if (octagon1 is IDrawToPrinter printerOctagon1)
    {
        printerOctagon1.Draw();
    }
}
//ExplicitInterfaceImplementation2();