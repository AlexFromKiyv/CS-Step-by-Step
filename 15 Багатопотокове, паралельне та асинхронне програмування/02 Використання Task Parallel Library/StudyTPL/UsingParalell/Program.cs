void SimpleUsingInvoke()
{
    Parallel.Invoke(
        () => {
            Console.WriteLine($"Task:{Task.CurrentId}");
            Thread.Sleep(3000);
            Console.WriteLine("Hi");

        },        
        ()=>SayWithDelay("girl",5000),
        
        SayGoodbay       
        );

    void SayGoodbay()
    {
        SayWithDelay("Goodbay", 8000);
    }
    
    void SayWithDelay(string phrase, int delay) 
    {
        AboutTask();
        Thread.Sleep(delay);
        Console.WriteLine(phrase);
    }

    void AboutTask()
    {
        Console.WriteLine($"Task:{Task.CurrentId}");
    }
}
SimpleUsingInvoke();


