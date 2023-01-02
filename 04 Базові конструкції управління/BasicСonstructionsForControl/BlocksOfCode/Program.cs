//BadCode();
static void BadCode()
{
    string enteredString = "";
    while (enteredString != "y")
        Console.Write("Do you want to exit ? (Y/N):");
        enteredString = Console.ReadLine();

}

ItWork();
static void ItWork()
{
    string enteredString = "";
    while (enteredString.ToLower() != "y")
    {
        Console.Write("Do you want to exit ? (Y/N):");
        enteredString = Console.ReadLine();
    }


}


class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }

    public string GetPhone()
    {
        return Phone;
    }
}