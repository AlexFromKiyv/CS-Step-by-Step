namespace AutoLot.Services.DataServices.Api.Examples;

public class BasicUsageWithIHttpClientFactory
{
    private readonly IHttpClientFactory _httpClientFactory;
    public BasicUsageWithIHttpClientFactory(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task DoSomethingAsync()
    {
        var client = _httpClientFactory.CreateClient();
        Console.WriteLine(client.BaseAddress);

        //do something interesting with the client
    }
}
