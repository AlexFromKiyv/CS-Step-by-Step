namespace AutoLot.Services.ApiWrapper.Models;

public class ApiServiceSettings
{
    public ApiServiceSettings() { }
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Uri { get; set; } = null!;
    public string CarBaseUri { get; set; } = null!;
    public string MakeBaseUri { get; set; } = null!;
    public int MajorVersion { get; set; } 
    public int MinorVersion { get; set; }
    public string Status { get; set; } = null!;
    public string ApiVersion => $"{MajorVersion}.{MinorVersion}" + (!string.IsNullOrEmpty(Status) ? $"-{Status}" : string.Empty);
}