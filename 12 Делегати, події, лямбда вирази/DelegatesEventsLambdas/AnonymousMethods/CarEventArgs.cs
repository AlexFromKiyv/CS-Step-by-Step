﻿
namespace AnonymousMethods;

public class CarEventArgs : EventArgs
{
    public readonly string message;

    public CarEventArgs(string message)
    {
        this.message = message;
    }
}