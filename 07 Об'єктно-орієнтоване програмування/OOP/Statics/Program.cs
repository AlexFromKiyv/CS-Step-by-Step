using Statics;

void BadInvokeStatic()
{
    int a = 0;
    int b;
    //b = a.Parse("1"); //cannot be accessed with an instance reference;
    //                  //qualify it with a type name instied
    b = int.Parse("1");

    //Console console = new Console(); // Cannot create instance of the static class
}


//CreateSomePerson();

void CreateSomePerson()
{
    Person girl_1 = new("Ira");
    girl_1.height = 65.0;
    girl_1.ToConsole();

    Person boy_1 = new("Igor");
    boy_1.ToConsole();
}


//UsingStaticData();

void UsingStaticData()
{
    Person girl = new("Maria");
    girl.weight = 55;

    Person man = new("Max");
    man.weight = 90;

    Console.WriteLine(girl.weight < Person.avarageWeight);
    Console.WriteLine(man.weight < Person.avarageWeight);

}


UsingStaticMethods();

void UsingStaticMethods()
{
    Console.WriteLine(Person.GetAvarageWeight());
    Person.SetAvarageWeight(77.2);
    Console.WriteLine(Person.GetAvarageWeight());
}