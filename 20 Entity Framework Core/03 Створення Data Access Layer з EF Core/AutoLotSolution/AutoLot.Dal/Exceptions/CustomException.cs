﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoLot.Dal.Exceptions;

public class CustomException : Exception
{
    public CustomException() { }
    public CustomException(string? message) : base(message) { }
    public CustomException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
