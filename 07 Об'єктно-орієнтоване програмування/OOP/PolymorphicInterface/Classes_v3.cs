using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace PolymorphicInterface
{
    abstract class Shape_v3
    {
        protected string _name;
        public string Name 
        { 
            get => _name;
            set 
            {
                if(value.Length < 10)
                {
                    _name = value;
                }
                else
                {
                    _name = "";
                } 
                
            } 
        }
        protected Shape_v3(string name = ""):this() 
        {
            Name = name;
        }
        protected Shape_v3()
        {
            _name = "";
        }

        public abstract void Draw();
    }

    class Circle_v3 : Shape_v3
    {
        public Circle_v3(string name = "") : base(name)
        {
        }

        public override void Draw() => Console.WriteLine($"Circle({Name})");
    }

    class ThreeDCircle_v3 : Circle_v3
    {
        public new string Name { get; set; }
        public ThreeDCircle_v3(string name)
        {
            Name = name;
        }
        public new void Draw() => Console.WriteLine($"Drawing 3D Circle -> {Name}");
    }

}
