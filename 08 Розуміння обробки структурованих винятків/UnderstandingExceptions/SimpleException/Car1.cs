﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleException;

class Car1
{
    // Constant for maximum speed.
    public const int MaxSpeed = 100;

    // Car properties.
    public int CurrentSpeed { get; set; }
    public string PetName { get; set; } = "NoName";

    // Is the car still operational?
    private bool _carIsDead;

    private readonly Radio _radio = new Radio();

    public Car1()
    {
    }
    public Car1(string petName, int currentSpeed)
    {
        CurrentSpeed = currentSpeed;
        PetName = petName;
    }

    public void CrankTunes(bool state)
    {
        // Delegate request to inner object.
        _radio.TurnOn(state);
    }
    //Change current speed.
    public void Accelerate(int delta)
    {
        if (_carIsDead)
        {
            Console.WriteLine($"{PetName} is out of order...");
        }
        else
        {
            CurrentSpeed += delta;
            if (CurrentSpeed > MaxSpeed)
            {
                CurrentSpeed = 0;
                _carIsDead = true;

                // Use the 'throw' keyword to raise an exception.
                throw new Exception($"{PetName} has overheated!")
                {
                    HelpLink = "https://group.mercedes-benz.com/en/",
                    Data =
                    {
                        {"Timestamp",DateTime.Now},
                        {"Cause","You have a lead foot." }
                    }
                };
                
            }
            Console.WriteLine($"\tCurrentSpeed = {CurrentSpeed}");
        }
    }
}
