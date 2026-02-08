using CommonSnappableTypes;

namespace CSharpSnapIn;
[CompanyInfo(CompanyName = "FooSoft", CompanyUrl = "www.FooSoft.com")]
public class CSharpModule : IAppFunctionality
{
    void IAppFunctionality.DoIt()
    {
        Console.WriteLine("You have just used the C# snap-in!");
    }
}
