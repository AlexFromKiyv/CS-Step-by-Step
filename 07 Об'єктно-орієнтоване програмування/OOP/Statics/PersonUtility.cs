using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statics
{
    static class PersonUtility
    {
        public static string IsWeightGood(Person person)
        {
            string result;

            double? bodyMassIndex = BodyMassIndex(person);
            
            ArgumentNullException.ThrowIfNull(bodyMassIndex);

            if(bodyMassIndex > 0 && bodyMassIndex < 18.5)
            {
                result = $"Body mass index:{bodyMassIndex:0.##}\nWeight is less than required";
            
            }else if(bodyMassIndex > 18.5 &&  bodyMassIndex < 24.9)
            {
                result = $"Body mass index:{bodyMassIndex:0.##}\nWeight is normal";
            
            }else if(bodyMassIndex > 25)
            {
                result = $"Body mass index:{bodyMassIndex:0.##}\nWeight is more than normal";
            }
            else
            {
                result = "Could not be calculated.";
            }

            return result;
        }

        public static double? BodyMassIndex(Person person)
        {
 
            return person.weight / ( person.height/100 * person.height/100 );
        }
    }
}
