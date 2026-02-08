using System.Reflection;

static void ReflectAttributesUsingLateBinding()
{
    try
    {
        // Load the local copy of AttributedCarLibrary.
        Assembly asm = Assembly.LoadFrom("AttributedCarLibrary");

        // Get type info of VehicleDescriptionAttribute.
        Type vehicleDesc = asm.GetType("AttributedCarLibrary.VehicleDescriptionAttribute");

        // Get type info of the Description property.
        PropertyInfo propDesc = vehicleDesc.GetProperty("Description");

        // Get all types in the assembly.
        Type[] types = asm.GetTypes();

        foreach (var type in types)
        {
            object[] objects = type.GetCustomAttributes(vehicleDesc, true);
            // Iterate over each VehicleDescriptionAttribute and print
            // the description using late binding.
            foreach (object o in objects)
            {
                Console.WriteLine($"{type.Name}: {propDesc?.GetValue(o, null)}\n");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

}
ReflectAttributesUsingLateBinding();