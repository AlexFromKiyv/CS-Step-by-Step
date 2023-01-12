//SimpleMethod();
static void SimpleMethod()
{
    double height = 176;

    double result = MaxGoodWeight(height);

    Console.WriteLine(result);


    static double MaxGoodWeight(double height)
    {
        return (height / 100) * (height / 100) * 24.9;
    }
}


//SimpleMethodWithLambda();

static void SimpleMethodWithLambda()
{

    Console.WriteLine(MaxGoodWeight(176));

    static double MaxGoodWeight(double height) => (height / 100) * (height / 100) * 24.9;
}

SimpleMethodWithValidation();
static void SimpleMethodWithValidation()
{
    for (int height = 164; height < 192; height+=2)
    {
        Console.WriteLine(MaxGoodWeightWithValidation(height));
    }
   
    //Console.WriteLine(MaxGoodWeight(320)); it do not work

    static string MaxGoodWeightWithValidation(double height)
    {

        if (height > 130 && height < 280 )
        {
            return $"Max good weight for {height} cm is {MaxGoodWeight(height)} "; 
        }
        else
        {
            return $"Bad input height {height} ";
        }

        // Local function
        static double MaxGoodWeight(double height) => (height/100)*(height/100)*24.9;
        
    }
}
