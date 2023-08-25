using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericDelegate;

// This generic delegate can represent any method
// returning void and taking a single parameter of type T.
public delegate void MyDenericDelegate<T>(T arg); 
