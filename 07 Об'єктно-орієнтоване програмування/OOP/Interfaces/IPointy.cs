using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{


    /// <summary>
    /// This interface defines the behavior of 'having points.'
    /// </summary>
    internal interface IPointy
    {
        //public int NumberOfPoints; // Error. Cannot contain instance fields

        //public IPointy() { } // Error. Cannot contain instance constructor

        //byte GetNumberOfPoint(); //This method is the same as property Points

        public int Points { get; } // Interface types are able to define any number of property

    }

    /// <summary>
    /// To represent a regular polygon
    /// </summary>
    interface IRegularPointy : IPointy
    {
        int SideLength { get; set; }
        int NumberOfSide { get; set; }
        int Perimeter => SideLength * NumberOfSide;


        //Static constructor and member
        static string Inscription { get; set; }
        static IRegularPointy() => Inscription = "No inscription";
    }

}
