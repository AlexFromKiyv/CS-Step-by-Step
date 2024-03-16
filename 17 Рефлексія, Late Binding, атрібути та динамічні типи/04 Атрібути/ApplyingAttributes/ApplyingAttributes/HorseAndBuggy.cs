using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ApplyingAttributes;

//[XmlRoot(Namespace = "http://www.MyCompany.com"), Obsolete("Use another vehicle!")]
[XmlRoot(Namespace = "http://www.MyCompany.com")]
[Obsolete("Use another vehicle!")]
internal class HorseAndBuggy
{
    //...
}
