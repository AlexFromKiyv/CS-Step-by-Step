//using System.Runtime.CompilerServices;
//[assembly:InternalsVisibleTo("CSharpCarClient")]
namespace CarLibrary;
public abstract class Car
{
    public string PetName { get; set; } = string.Empty; 
    public int MaxSpeed { get; set; }
    public int CurrentSpeed { get; set; }

    protected EngineStateEnum State = EngineStateEnum.EngineAlive;
    public EngineStateEnum EngineState => State;
    public abstract void TurboBoost();

    protected Car() { }
    protected Car(string name, int maxSpeed, int currentSpeed)
    {
        PetName = name;
        MaxSpeed = maxSpeed;
        CurrentSpeed = currentSpeed;
    }
}
