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

        //Change default constructor
        public Bike()
        {
            ownerName = "Noname";
            currentSpeed = 2;
        }

        public void StateToConsol() => Console.WriteLine($"Bicycler: {ownerName} is driving at speed: {currentSpeed}");
        public void SpeedUp(int speedChange) => currentSpeed += speedChange;
    }
}
