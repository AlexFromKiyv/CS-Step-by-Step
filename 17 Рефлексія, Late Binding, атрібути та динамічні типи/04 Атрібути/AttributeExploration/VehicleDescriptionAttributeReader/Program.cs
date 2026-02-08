using AttributedCarLibrary;
static void ReflectOnAttributesUsingEarlyBinding() 
{
    // Get a Type representing the Winnebago.
    Type type = typeof(Winnebago);

    object[] customAttributes = type.GetCustomAttributes(false);

    // Print the description.
    foreach (VehicleDescriptionAttribute customAttribute in customAttributes)
    {
        Console.WriteLine($"{type}\t{customAttribute.Description}");
    }
}
ReflectOnAttributesUsingEarlyBinding();
