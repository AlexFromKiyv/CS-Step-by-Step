using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamineTypeClass
{
    internal class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        private double _selary;

        public void ChangeSelary(double selary)
        {
            _selary = selary;
        }
    }
}
