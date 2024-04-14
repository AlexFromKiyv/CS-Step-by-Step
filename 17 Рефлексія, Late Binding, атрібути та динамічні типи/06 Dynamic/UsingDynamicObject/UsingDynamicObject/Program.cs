using UsingDynamicObject;

void TestOurClass()
{

    dynamic person = new Person();

    person.Name = "George";
    person.Age = 25;

    Func<int,int> increaseAge  = (int y) => { person.Age += y; return person.Age; };
    person.IncreaseAge = increaseAge;


    person.IncreaseAge(10);

    Console.WriteLine($"{person.Name} {person.Age}");

}
TestOurClass();
