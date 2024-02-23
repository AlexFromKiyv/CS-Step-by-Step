using CarLibrary;

void TestDrive()
{
    SportCar sportCar = new SportCar("MyBird", 100, 200);
    sportCar.TurboBoost();

    MiniVan miniVan = new();
    miniVan.TurboBoost();

    InternalCar internalCar = new();
    internalCar.TurboBoost();
}
TestDrive();
