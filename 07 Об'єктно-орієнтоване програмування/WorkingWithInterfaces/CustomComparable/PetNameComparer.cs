using System.Collections;

namespace CustomComparable;

public class PetNameComparer : IComparer
{
    public int Compare(object? x, object? y)
    {
        if (x is Car car1 && y is Car car2)
        {
            return string.Compare(car1.PetName, car2.PetName,
                StringComparison.OrdinalIgnoreCase);
        }
        else
        {
            throw new ArgumentException("Parameter in not type Car");
        }
    }
}
