
void GetPaths()
{
    var list = AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES").ToString().Split(';');

    Console.WriteLine("\nTRUSTED_PLATFORM_ASSEMBLIES:");
    foreach (var path in list)
    {
        Console.WriteLine(path);
    }

    Console.WriteLine($"\nPLATFORM_RESOURCE_ROOTS: {AppContext.GetData("PLATFORM_RESOURCE_ROOTS")}");
    Console.WriteLine($"\nNATIVE_DLL_SEARCH_DIRECTORIES: {AppContext.GetData ("NATIVE_DLL_SEARCH_DIRECTORIES")}");
    Console.WriteLine($"\nAPP_PATHS: {AppContext.GetData("APP_PATHS")}");
    Console.WriteLine($"\nAPP_NI_PATHS: {AppContext.GetData("APP_NI_PATHS")}");

}
GetPaths();