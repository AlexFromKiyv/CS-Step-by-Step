namespace MyClassLibrary;

public class MyClass
{
    public static void PublicMethodInLibrary()
    {
        Console.WriteLine("PublicMethodInLibrary");
        PrivateMethodInLibrary();
    }
    private static void PrivateMethodInLibrary() 
    {
        Console.WriteLine("PrivateMethodInLibrary");
        File.OpenText("bad path to file");
    }
}