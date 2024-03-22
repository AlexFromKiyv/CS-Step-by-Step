using AttributedCarLibrary;

void ReflectOnAttributesUsingEarlyBinding()
{
    Type type = typeof(Winnebago);
    Console.WriteLine($"We have type {type}\n");

    object[] customAttributes = type.GetCustomAttributes(false);

    foreach (VehicleDescriptionAttribute item in customAttributes)
    {
        Console.WriteLine(item.Description);
    }

}
ReflectOnAttributesUsingEarlyBinding();