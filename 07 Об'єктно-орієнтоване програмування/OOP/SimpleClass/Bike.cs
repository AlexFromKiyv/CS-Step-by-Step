using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace SimpleClass
{
    class Bike
    {
        public string ownerName;
        public int currentSpeed;

        //Overload default constructor
        public Bike()
        {
            ownerName = "Noname";
            currentSpeed = 2;
        }

        // currentSpeed = default // construtor can be one-line method
        public Bike(string ownerName) => this.ownerName = ownerName;

        // caller will set all
        public Bike(string ownerName, int currentSpeed)
        {
            this.ownerName = ownerName;
            this.currentSpeed = currentSpeed;
        }

        // with out parameter
        public Bike(string ownerName, int currentSpeed, out bool moving)
        {
            this.ownerName = ownerName;
            this.currentSpeed = currentSpeed;
            
            if (currentSpeed > 0)
            {
                moving = true;
            }
            else
            {
                moving = false;
            }

        }
 
        public void StateToConsol() => Console.WriteLine($"Bicycler: {ownerName} is driving at speed: {currentSpeed}");
        public void SpeedUp(int speedChange) => currentSpeed += speedChange;

        //this
        public void SetOwnerName(string ownerName) => ownerName = ownerName;
        public void SetOwnerNameWithThis(string ownerName) => this.ownerName = ownerName;




    }
}
