
//SimpleEnum();
static void SimpleEnum()
{
    WaterStateEnum myEnum = WaterStateEnum.Ice;

    Console.WriteLine(myEnum);
 
    Console.WriteLine(myEnum == WaterStateEnum.Ice);
    Console.WriteLine(myEnum == WaterStateEnum.Liquid);
}

//UsingEnum();
static void UsingEnum()
{
    

    WaterStateEnum  enumNow = WaterStateEnum.Liquid;

    Console.WriteLine(enumNow);
    Console.WriteLine((int)enumNow);

    IsItCold(enumNow);

    enumNow = WaterStateEnum.Ice;

    IsItCold(enumNow);

    //enumNow = Ice;                    don't work
    //enumNow = WaterStateEnum.Boiling; 

    Console.WriteLine(enumNow is ValueType); //True

    static void IsItCold(WaterStateEnum waterStateEnum)
    {
        switch(waterStateEnum)
        {
            case WaterStateEnum.Snow:
            case WaterStateEnum.Ice:
                Console.WriteLine("Yes. It's cold");
                break;               
            case WaterStateEnum.Liquid:
            case WaterStateEnum.Par:
                Console.WriteLine("No. It is'nt.");
                break;
            default:
                Console.WriteLine("I do'nt know yet.");
                break;
        }
    }

}

//UsingSystemEnum();
static void UsingSystemEnum()
{

    EnumInfo(WaterStateEnum.Ice);
    EnumInfo(DayOfWeek.Monday);
    EnumInfo(ConsoleColor.Red);


    static void EnumInfo(System.Enum enumElement)
    {
        Console.WriteLine(enumElement);

        Type enumType = enumElement.GetType();

        Console.WriteLine($"Type:{enumType}");
        Console.WriteLine($"Is enum {enumType.IsEnum}");
        Console.WriteLine($"Underlying type:{Enum.GetUnderlyingType(enumType)}");
        foreach (var item in Enum.GetValues(enumType))
        {
            Console.WriteLine($"{item} - {item:D}");
        }
        Console.WriteLine();
    }
}



//UsingBitwiseOperations();
static void UsingBitwiseOperations(){
    Console.WriteLine($"6 = {Convert.ToString(6, 2)}");
    Console.WriteLine($"4 = {Convert.ToString(4, 2)}");
    Console.WriteLine($"{Convert.ToString(6, 2)} & {Convert.ToString(4, 2)} = {Convert.ToString(6 & 4, 2)} = {6&4} ");
    Console.WriteLine($"{Convert.ToString(6, 2)} | {Convert.ToString(4, 2)} = {Convert.ToString(6 | 4, 2)} = {6 | 4}  ");
    Console.WriteLine($"{Convert.ToString(6, 2)} ^ {Convert.ToString(4, 2)} = {Convert.ToString(6 ^ 4, 2)} = {6 ^ 4} ");
    Console.WriteLine($"{Convert.ToString(6, 2)} >> 1 = {Convert.ToString(6 >> 1, 2)} = {6 >> 1}  ");
    Console.WriteLine($"{Convert.ToString(6, 2)} << 1 = {Convert.ToString(6 << 1, 2)} = {6 << 1}  ");
    Console.WriteLine($"~{Convert.ToString(6, 2)} = {Convert.ToString(~6, 2)} = {~6}");
    Console.WriteLine($"Int.MaxValue =  {Convert.ToString(int.MaxValue, 2)}");
}

UsingBitwiseOperationsWithEnum();
static void UsingBitwiseOperationsWithEnum()
{
    ContactPreferenceEnum contactsJulia = ContactPreferenceEnum.Email | ContactPreferenceEnum.Phone;
    Console.WriteLine("Julia contacts:");
    Console.WriteLine("None - {0}", (contactsJulia | ContactPreferenceEnum.None) == contactsJulia);
    Console.WriteLine("Email - {0}", (contactsJulia | ContactPreferenceEnum.Email) == contactsJulia);
    Console.WriteLine("Phone - {0}", (contactsJulia | ContactPreferenceEnum.Phone) == contactsJulia);
    Console.WriteLine("Ukrposhta - {0}", (contactsJulia | ContactPreferenceEnum.Ukrposhta) == contactsJulia);

}



enum WaterStateEnum
{
    Ice,    //0
    Snow,   //1
    Liquid, //2 
    Par     //3
}

enum WaterState1Enum
{
    Ice = 100,
    Snow,   //101
    Liquid, //102 
    Par     //103
}

enum WaterState2Enum
{
    Ice = -1,
    Snow = -1,
    Liquid = 1,
    Par = 2
}

enum WaterState3Enum : byte
{
    Ice,
    Snow,
    Liquid,
    Par
}


[Flags]
enum ContactPreferenceEnum
{
    None = 1,
    Email = 2,
    Phone = 4,
    Ukrposhta = 8
}