using System.Reflection;

void ReflectAttributesUsingLateBinding()
{
	try
	{
		Assembly assembly = Assembly.LoadFrom("AttributedCarLibrary");

		Type? vehicleDesctiption = assembly.GetType("AttributedCarLibrary.VehicleDescriptionAttribute");
		Console.WriteLine(vehicleDesctiption);

        PropertyInfo? propertyInfoVehileDesc = vehicleDesctiption?.GetProperty("Description");
        Console.WriteLine(propertyInfoVehileDesc);

        Type[] types = assembly.GetTypes();
		foreach (Type type in types)
		{
			if (vehicleDesctiption == null) { return;}

			object[] objects = type.GetCustomAttributes(vehicleDesctiption, false);
			foreach (object obj in objects)
			{
                Console.WriteLine($"{type} - {propertyInfoVehileDesc?.GetValue(obj,null)}");
            }
        }
	}
	catch (Exception ex)
	{
        Console.WriteLine(ex.Message);
    }
}
ReflectAttributesUsingLateBinding();
