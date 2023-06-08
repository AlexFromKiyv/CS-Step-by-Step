using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccessModifiers
{
    internal class CallerAllKindModifiers
    {
        public void AttemptingToCall()
        {
            AllKindModifiers allDataAndMemeber = new();

            // inaccessible
            // allDataAndMemeber.VarWithEpmtyModifier 
            // allDataAndMemeber.VarWithPrivate
            // allDataAndMemeber.VarWithProtectedPrivate
            // allDataAndMemeber.VarWithProtected
            Console.WriteLine(allDataAndMemeber.VarWithInternal);
            Console.WriteLine(allDataAndMemeber.VarWithProtectedInternal);
            Console.WriteLine(allDataAndMemeber.VarWithPublic);

            // inaccessible
            //allDataAndMemeber.MethodWithEpmtyModifier();
            //allDataAndMemeber.MethodWithPrivate();
            //allDataAndMemeber.MethodWithProtected();
            allDataAndMemeber.MethodWithInternal();
            allDataAndMemeber.MethodWithProtectedInternal();
            allDataAndMemeber.MethodWithPublic();
        }
    }
}
