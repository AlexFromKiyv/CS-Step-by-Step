using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AccessModifiers
{
    class AllKindModifiers
    {
        //private only for this class
        string VarWithEpmtyModifier = "VarWithEpmtyModifier";

        //private only for this class
        private string VarWithPrivate = "VarWithPrivate";
        
        // for this and inheritance classes from this project
        protected private string VarWithProtectedPrivate = "VarWithProtectedPrivate";

        // for this and inheritance classes 
        protected string VarWithProtected = "VarWithProtected";

        // for this project
        internal string VarWithInternal = "VarWithInternal";

        // for this project and inheritance classes other project
        protected internal string VarWithProtectedInternal = "VarWithProtectedInternal";

        //for all
        public string VarWithPublic = "VarWithPublic";


        //this method is private only for this class
        void MethodWithEpmtyModifier() => Console.WriteLine("MethodWithEpmtyModifier");

        //private only for this class
        private void MethodWithPrivate() => Console.WriteLine("MethodWithPrivate");

        //for this and inheritance classes from this project
        protected private void MethodWithProtectedPrivate() => Console.WriteLine("MethodWithProtectedPrivate");

        // for this and inheritance classes 
        protected void MethodWithProtected() => Console.WriteLine("MethodWithProtected");

        // for this project
        internal void MethodWithInternal() => Console.WriteLine("MethodWithInternal");
        
        // for this project and inheritance classes other project
        protected internal void MethodWithProtectedInternal() => Console.WriteLine("MethodWithProtectedInternal");

        // for all
        public void MethodWithPublic() => Console.WriteLine("MethodWithPublic");
    }
}
