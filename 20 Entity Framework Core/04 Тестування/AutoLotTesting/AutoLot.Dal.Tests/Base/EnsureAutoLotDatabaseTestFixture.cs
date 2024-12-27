
namespace AutoLot.Dal.Tests.Base;

public sealed class EnsureAutoLotDatabaseTestFixture : IDisposable
{
    public EnsureAutoLotDatabaseTestFixture()
    {
        var configuration = TestHelpers.GetConfiguration;
        var context = TestHelpers.GetContext(configuration);
        SampleDataInitializer.ClearAndSeedData(context);
        context.Dispose();
    }
    public void Dispose()
    {
    }
}