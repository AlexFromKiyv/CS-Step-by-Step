namespace EmployeeApp;
public class Employee5
{
    // Field data
    private int _id;
    private string _name = null!;
    private float _pay;
    private int _age;
    private string _SSN = null!;
    private EmployeePayTypeEnum _payType;
    private DateTime _hireDate;

    // Properties
    public int Id { get => _id; set => _id = value; }
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
    public float Pay { get => _pay; set => _pay = value; }
    public int Age { get => _age; set => _age = value; }
    public string SSN => _SSN;

    public EmployeePayTypeEnum PayType
    {
        get => _payType;
        set => _payType = value;
    }

    public DateTime HireDate
    {
        get => _hireDate;
        set => _hireDate = value;
    }

    //Constructors
    public Employee5(int id, string name, float pay, int age, string ssn,
        EmployeePayTypeEnum payType)
    {
        Id = id;
        Name = name;
        Pay = pay;
        Age = age;
        // OOPS! This is no longer possible if the property is read only.
        //SSN = ssn;
        _SSN = ssn;
        PayType = payType;
    }

    public Employee5(int id, string name, float pay) :
        this(id, name, pay, 0, "",EmployeePayTypeEnum.Salaried )
    { }

    public Employee5() { }

    // Mathods
    public void GiveBonus(float amount)
    {
        Pay = this switch
        {
            { Age: >= 18, PayType: EmployeePayTypeEnum.Commission, HireDate.Year : > 2020 } 
            => Pay += 0.10F * amount,
            { Age: >= 18, PayType: EmployeePayTypeEnum.Hourly, HireDate.Year: > 2020 }
            => Pay += 40F * amount / 2080F,
            { Age: >= 18, PayType: EmployeePayTypeEnum.Salaried, HireDate.Year: > 2020 }
            => Pay += amount,
            _ => Pay += 0
        };
    }

    public void DisplayStatus()
    {
        Console.WriteLine($"{Id}\t{Name}\t{Age}\t{Pay}");
    }

}
