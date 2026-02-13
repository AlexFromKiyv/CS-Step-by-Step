using CommonSnappableTypes;

namespace MyModule;
[CompanyInfo(CompanyName = "MySoft", CompanyUrl = "www.MySoft.com")]
public class MyModule : IAppFunctionality
{
    public void DoIt()
    {
        Console.WriteLine("I want to do it different."  );
    }
}
