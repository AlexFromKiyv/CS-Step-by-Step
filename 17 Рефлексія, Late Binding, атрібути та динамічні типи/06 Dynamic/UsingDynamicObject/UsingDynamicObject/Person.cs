using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsingDynamicObject;

internal class Person : DynamicObject
{

    Dictionary<string, object> members = new Dictionary<string, object>();

    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        if (value is not null)
        {
            members[binder.Name] = value;
            return true;
        }
        return false;
    }

    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        result = null;
        if (members.ContainsKey(binder.Name))
        {
            result = members[binder.Name];
            return true;
        }
        return false;
    }

    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        result = null;
        if (args?[0] is int number)
        {
            dynamic method = members[binder.Name];
            result = method(number);
        }
        return result != null;
    }
}
