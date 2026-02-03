using CarLibrary;
using ExamineTypeClass;
using System.Reflection;

void ObtainTypeUseObjectGetType()
{
    SportCar sportCar = new();
    Type type = sportCar.GetType();

    Console.WriteLine(type);
    Console.WriteLine(type.GetType());
   
}
//ObtainTypeUseObjectGetType();

void ObtainTypeUseTypeOf()
{
    Type type = typeof(SportCar);

    Console.WriteLine(type);
    Console.WriteLine(type.GetType());
}
ObtainTypeUseTypeOf();

void ObtainTypeUseTypeGetType()
{

    Type? type = Type.GetType("ExamineTypeClass.Person", true,true);

    Console.WriteLine(type);
    Console.WriteLine(type?.GetType());

    type = Type.GetType("CarLibrary.SportCar, Carlibrary", true, true);

    Console.WriteLine(type);
    Console.WriteLine(type?.GetType());

}
//ObtainTypeUseTypeGetType();


void UseGetMemebers()
{
    Type type = typeof(Person);

    var members = type.GetMembers();

    foreach (var member in members)
    {
        Console.WriteLine($"{member.DeclaringType}   {member.MemberType}   {member.Name}");
    }
}
//UseGetMemebers();

void UseGetMemebersWithBindingFlags()
{
    Type type = typeof(Person);

    var members = type.GetMembers(
        BindingFlags.DeclaredOnly | 
        BindingFlags.Instance | 
        BindingFlags.NonPublic | 
        BindingFlags.Public);

    foreach (var member in members)
    {
        Console.WriteLine($"{member.DeclaringType}   {member.MemberType}   {member.Name}");
    }
}
//UseGetMemebersWithBindingFlags();

void GetOneMember()
{
    Type type = typeof(Person);

    var changeSaleries = type.GetMember("ChangeSelary");
    foreach (var member in changeSaleries)
    {
        Console.WriteLine($"{member.DeclaringType}   {member.MemberType}   {member.Name}");
    }
}
//GetOneMember();
