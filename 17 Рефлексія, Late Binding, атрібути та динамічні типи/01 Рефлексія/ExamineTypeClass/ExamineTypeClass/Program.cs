
using CarLibrary;
using ExamineTypeClass;

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
    SportCar sportCar = new();
    Type type =typeof(SportCar);

    Console.WriteLine(type);
    Console.WriteLine(type.GetType());
}
//ObtainTypeUseTypeOf();

void ObtainTypeUseTypeGetType()
{

    Type? type = Type.GetType("ExamineTypeClass.Person", true,true);

    Console.WriteLine(type);
    Console.WriteLine(type?.GetType());

    type = Type.GetType("CarLibrary.SportCar, Carlibrary", true, true);

    Console.WriteLine(type);
    Console.WriteLine(type?.GetType());

}
ObtainTypeUseTypeGetType();
