
namespace EmployeeApp;
// Managers need to know their number of stock options.
class Manager6 :Employee6
{
    public int StockOptions { get; set; }
    public Manager6(int id, string name, float pay, int age, string ssn, 
         int stockOptions) 
        : base(id, name, pay, age, ssn, EmployeePayTypeEnum.Salaried)
    {
        StockOptions = stockOptions;
    }
    public Manager6()
    {
    }

    //public Manager6(int id, string name, float pay, int age, string ssn, 
    //    EmployeePayTypeEnum payType, int stockOptions )
    //{
    //    // This property is defined by the Manager class.
    //    StockOptions = stockOptions;

    //    // Assign incoming parameters using the
    //    // inherited properties of the parent class.
    //    Id = id;
    //    Name = name;
    //    Pay = pay;
    //    Age = age;
    //    // OOPS! This would be a compiler error,
    //    // if the SSN property were read-only! 
    //    _SSN = ssn;
    //    PayType = payType;
    //}










}
