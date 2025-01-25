namespace EmployeeApp;

partial class Employee7
{
    // Field data
    protected string name = null!;
    protected string ssn = null!;
    protected BenefitPackage employeeBenefits = new();



    // Properties
    public int Id { get; set; }
    public string Name
    {
        get => name;
        set
        {
            if (value.Length > 15)
            {
                Console.WriteLine("Error! Name length exceeds 15 characters!");
            }
            else
            {
                name = value;
            }
        }
    }
    public float Pay { get; set; }
    public int Age { get; set; }
    public string SSN => ssn;
    public EmployeePayTypeEnum PayType {  get; set; }

    public BenefitPackage Benefits 
    { 
        get => employeeBenefits; 
        set => employeeBenefits = value; 
    }

    // Class member

    public class BenefitPackage
    {
        public enum BenefitPackageLevel
        {
            Standard, Gold, Platinum
        }
        public double ComputePayDeduction()
        {
            return 125.0;
        }
    }

    //Constructors
    public Employee7(int id, string name, float pay, int age, string ssn,
        EmployeePayTypeEnum payType)
    {
        Id = id;
        Name = name;
        Pay = pay;
        Age = age;
        this.ssn = ssn;
        PayType = payType;
    }
    public Employee7(int id, string name, float pay) :
        this(id, name, pay, 0, "", EmployeePayTypeEnum.Salaried)
    { }
    public Employee7() { }
}
