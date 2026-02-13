using AttributedCarLibrary;
static void ReflectOnAttributesUsingEarlyBinding()
{
    // Get a Type representing the Winnebago.
    Type type = typeof(HorseAndBuggy);

    object[] customAttributes = type.GetCustomAttributes(false);

    // Print the description.
    foreach (var customAttribute in customAttributes)
    {
        Console.Write(customAttribute);

        if (customAttribute is VehicleDescriptionAttribute vehicleDescriptionAttribute)
        {
            Console.Write($"\t{vehicleDescriptionAttribute.Description}");
        }
        else
        {
            Console.WriteLine();
        }
    }
}
ReflectOnAttributesUsingEarlyBinding();
