
void CreateDynamicObject()
{
    dynamic girl = new System.Dynamic.ExpandoObject();

    // properties
    girl.Name = "Lucy";
    girl.Age = 31;
    girl.Languages = new List<string> { "ukrainian", "russian" };
    
    //method
    girl.IncreaseAge = (Action<int>)(a =>  girl.Age += a);



    //invoke
    girl.IncreaseAge(2);

    //write
    Console.WriteLine($"{girl.Name} {girl.Age}");
    foreach (string language in girl.Languages)
    {
        Console.WriteLine($"\t{language}");
    }
}
CreateDynamicObject();