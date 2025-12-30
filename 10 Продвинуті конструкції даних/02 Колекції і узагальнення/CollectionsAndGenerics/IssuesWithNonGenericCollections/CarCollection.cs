using System.Collections;

namespace IssuesWithNonGenericCollections;

public class CarCollection : IEnumerable
{
    private ArrayList arCars = new ArrayList();

    public IEnumerator GetEnumerator() => arCars.GetEnumerator();

    // Cast for caller.
    public Car GetCar(int pos) => (Car)arCars[pos];
    // Insert only Car objects.
    public void AddCar(Car c)
    {
        arCars.Add(c);
    }
    public void ClearCars()
    {
        arCars.Clear();
    }
    public int Count => arCars.Count;
}
