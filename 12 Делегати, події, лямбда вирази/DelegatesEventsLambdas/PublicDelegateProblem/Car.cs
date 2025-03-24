using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublicDelegateProblem;

public class Car
{
    public delegate void CarEngineHandler(string messageForCaller);

    // Now a public member!
    public CarEngineHandler? ListOfHandler;

    // Just fire out the Exploded notification.
    public void Accelerate(int delta)
    {
        if (ListOfHandler != null)
        {
            ListOfHandler("Sorry, this car is dead...");
        } 
    }
}
