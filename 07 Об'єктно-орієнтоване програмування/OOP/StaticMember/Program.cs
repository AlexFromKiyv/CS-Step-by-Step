void BadInvokeStatic()
{
    int a = 0;
    int b;
    //b = a.Parse("1"); //cannot be accessed with an instance reference;
    //                  //qualify it with a type name instied
    b = int.Parse("1");

    //Console console = new Console(); // Cannot create instance of the static class
}
