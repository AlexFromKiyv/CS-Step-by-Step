namespace AutoLot.Services.DataServices.Api.Examples;

public class ApiServiceWrapperExample : IApiServiceWrapperExample
{
    protected readonly HttpClient Client;
    public ApiServiceWrapperExample(HttpClient client)
    {
        Client = client;
        //common client configuration goes here
    }
    //interesting methods implemented here
}
