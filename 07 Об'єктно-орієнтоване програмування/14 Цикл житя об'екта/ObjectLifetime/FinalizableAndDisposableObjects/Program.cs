
using FinalizableAndDisposableObjects;
UsingDisposalPattern();
void UsingDisposalPattern()
{
    goodResourseWrapper wrapper1 = new goodResourseWrapper();
    wrapper1.Resource = "Im work";
    Console.WriteLine(wrapper1.Resource);
    wrapper1.Dispose();
    wrapper1.Dispose();

    goodResourseWrapper wrapper2 = new goodResourseWrapper();
    wrapper2.Resource = "Im work again";
    Console.WriteLine(wrapper2.Resource);
}

