# Додаваня пакетів в проект.

Створимо проект UsingPackages. Додамо додадкові пакети.

1. В Solution Explorer на назві проекту правий клік.
2. Manage NuGet Packages.
3. Перейти на закладку Browse.
4. Ввести Microsoft.Extensions.Configuration.
5. Вибрати пакет Microsoft.Extensions.Configuration нажати Install.

Також можна додати:
 - Microsoft.Extensions.Configuration.Binder 
 - Microsoft.Extensions.Configuration.FileExtensions
 - Microsoft.Extensions.Configuration.Json

 Перейдіть на закладку Installed і зверніть увагу шо аби побачити всі встанвлені файли требо шоб вікно пошуку було порожне.

 Для встановлення в Visual Studio Code в папцв проекту треба виконати наступні команди.
 ```
 dotnet add package Microsoft.Extensions.Configuration
 dotnet add package Microsoft.Extensions.Configuration.Binder
 dotnet add package Microsoft.Extensions.Configuration.FileExtensions
 dotnet add package Microsoft.Extensions.Configuration.Json

 ```   

 Після додавання в файлі проекту буде 
 ```
   <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration" Version="7.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Binder" Version="7.0.3" />
    <PackageReference Include="Microsoft.Extensions.Configuration.FileExtensions" Version="7.0.0" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="7.0.0" />
  </ItemGroup>
 ```

 ## Використання

1. Додати в проект файл appsettings.json
 ```json
{
  "PacktSwitch": {
    "Level": "Warning"
  }
}
 ```
2. Якшо проект запускаеться в Visual Studio властивість файлу appsettings.json Copy to Output Directory : Copy always. (Правиц клік на файлі Properties)
3. В Program.cs
```cs
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

UsingAppsettingsJson();
void UsingAppsettingsJson()
{
Console.WriteLine($"Reading from appsettings.json in {Directory.GetCurrentDirectory()}");
ConfigurationBuilder builder = new();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile(
    "appsettings.json",
    optional: true, 
    reloadOnChange: true);

IConfigurationRoot configuration = builder.Build();
TraceSwitch ts = new(
  displayName: "PacktSwitch",
  description: "This switch is set via a JSON config.");

configuration.GetSection("PacktSwitch").Bind(ts);

Trace.WriteLineIf(ts.TraceError, "Trace error");
Trace.WriteLineIf(ts.TraceWarning, "Trace warning");
Trace.WriteLineIf(ts.TraceInfo, "Trace information");
Trace.WriteLineIf(ts.TraceVerbose, "Trace verbose");

}
```
```
Trace error
Trace warning
```
