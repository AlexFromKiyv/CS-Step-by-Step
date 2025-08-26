namespace AutoLot.Services.DataServices.Dal;

public class MakeDalDataService : DalDataServiceBase<Make>, IMakeDataService
{
    public MakeDalDataService(IMakeRepo mainRepo) : base(mainRepo)
    {
    }
}
