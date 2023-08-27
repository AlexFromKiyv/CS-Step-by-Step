using GenericDelegate;

void UseMyGenericDelegate()
{
    MyDenericDelegate<string> workWithString = new(StringTarget);
    workWithString += StringTarget; //work
    workWithString("Hi");

    MyDenericDelegate<int> workWithInt = new(IntTarget);
    workWithInt += IntTarget;
 // workWithInt += StringTarget;  // don't work 
    workWithInt(3);

    void StringTarget(string text)
    {
        Console.WriteLine(text.ToUpper());
    }

    void IntTarget(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Console.WriteLine(i);
        }
    }
}

UseMyGenericDelegate();