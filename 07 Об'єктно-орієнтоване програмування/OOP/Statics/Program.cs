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


//UsingStaticMethods();

void UsingStaticMethods()
{
    Console.WriteLine(Person.GetAvarageWeight());
    Person.SetAvarageWeight(77.2);
    Console.WriteLine(Person.GetAvarageWeight());
}


//UsingStaticConstructor();
void UsingStaticConstructor()
{
    Person tankman_1 = new("Jak");
    tankman_1.ToConsole();
    
    Person tankman_2 = new("Bob");
    tankman_2.ToConsole();
    
    Console.WriteLine(Person.desiredHeight);
}

UsingStaticClass();
void UsingStaticClass()
{
    Person jack = new("Jack", 190, 110);
    jack.ToConsole();
    Console.WriteLine(PersonUtility.IsWeightGood(jack));
    
    Console.WriteLine();

    Person maria = new("Maria", 165, 55);
    maria.ToConsole();
    Console.WriteLine(PersonUtility.IsWeightGood(maria));
}