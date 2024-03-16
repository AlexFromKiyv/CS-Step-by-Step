using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace CarLibrary;

public abstract class Car
{
    public string Name { get; set; } = string.Empty;
    public int CurrentSpeed { get; set; }
    public int MaxSpeed { get; set; }


    protected EngineStateEnum State = EngineStateEnum.EngineAlive;
    protected EngineStateEnum EngineState => State;

    protected Car() { }
    protected Car(string name, int currentSpeed, int maxSpeed)
    {
        Name = name;
        CurrentSpeed = currentSpeed;
        MaxSpeed = maxSpeed;
    }

    public abstract void TurboBoost();

    public void TurnOnRadio(bool isTurnOn,MusicMediaEnum musicMedia) => 
        Console.WriteLine(isTurnOn ? $"Jamming {musicMedia}" : "Quiet time..."); 
}
