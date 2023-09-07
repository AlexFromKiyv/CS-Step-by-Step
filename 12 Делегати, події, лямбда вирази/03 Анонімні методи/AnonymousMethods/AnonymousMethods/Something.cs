using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnonymousMethods
{
    internal class Something
    {
        //State
        public string? Name { get; set; }
        public int Speed { get; set; }
        public void ToConsole() => Console.WriteLine($"\tName: {Name}\tSpeed: {Speed}");

        public Something(string name, int speed)
        {
            Name = name;
            Speed = speed;
        }

        public Something()
        {
        }

        //Events
        public event EventHandler<SomethingEventArgs>? SpeedUp;
        public event EventHandler<SomethingEventArgs>? SpeedDown;
    
        public void ChangeSpeed(int speed)
        {
            Speed += speed;

            if (speed > 0)
            {
                SpeedUp?.Invoke(this, new SomethingEventArgs("Speed increased"));
            }
            else if(speed < 0) 
            {
                SpeedDown?.Invoke(this, new SomethingEventArgs("Speed dropped"));
            }
        }
    }
}
