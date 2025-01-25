namespace EmployeeApp;

partial class Employee6
{
    // Field data
    private string _name = null!;
    private string _SSN = null!;

    // Properties
    public int Id { get; set; }
    public string Name
    {
        get => _name;
        set
        {
            if (value.Length > 15)
            {
                Console.WriteLine("Error! Name length exceeds 15 characters!");
            }
            else
            {
                _name = value;
            }
        }
    }
    public float Pay { get; set; }
    public int Age { get; set; }
    public string SSN => _SSN;
    public EmployeePayTypeEnum PayType {  get; set; }

    //Constructors
    public Employee6(int id, string name, float pay, int age, string ssn,
        EmployeePayTypeEnum payType)
    {
        Id = id;
        Name = name;
        Pay = pay;
        Age = age;
        _SSN = ssn;
        PayType = payType;
    }
    public Employee6(int id, string name, float pay) :
        this(id, name, pay, 0, "", EmployeePayTypeEnum.Salaried)
    { }
    public Employee6() { }
}
