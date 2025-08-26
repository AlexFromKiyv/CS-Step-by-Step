namespace AutoLot.Services.DataServices.Api.Examples;

public class TypedUsageWithIHttpClientFactory
{
    private readonly IApiServiceWrapperExample _serviceWrapper;
    public TypedUsageWithIHttpClientFactory(IApiServiceWrapperExample serviceWrapper)
    {
        _serviceWrapper = serviceWrapper;
    }
    public async Task DoSomethingAsync()
    {
        //do something interesting with the service wrapper
    }
}