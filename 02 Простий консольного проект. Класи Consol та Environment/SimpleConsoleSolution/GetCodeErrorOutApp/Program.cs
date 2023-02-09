Console.Write("Does this program work well?:(Y/N)");

string? enteredString = Console.ReadLine();
if (enteredString == "Y" || enteredString == "y")
{
    return 0;
}
else
{
    // Bad work 
    return -1; 
}

