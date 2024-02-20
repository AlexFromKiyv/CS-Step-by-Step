
using CarLibrary;

void TestDrive()
{
    SportCar sportCar = new SportCar("MyBird", 100, 200);
    sportCar.TurboBoost();

    MiniVan miniVan = new();
    miniVan.TurboBoost();
}
//TestDrive();

void ViewInternaalClass()
{
    var iCar = new InternalCar();
    iCar.TurboBoost();
}
ViewInternaalClass();

