namespace CustomInterfaces;

// This interface defines the behavior of 'having points.'
public interface IPointy
{
    // Implicitly public and abstract.
    //byte GetNumbetOfPoints();
    byte Points { get; }
}
