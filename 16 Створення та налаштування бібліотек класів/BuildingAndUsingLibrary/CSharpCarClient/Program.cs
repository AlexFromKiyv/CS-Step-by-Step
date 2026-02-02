using CarLibrary;

void UsingTypeFromCarLibrary()
{
    // Make a sports car.
    SportsCar viper = new SportsCar("Viper", 240, 40);
    viper.TurboBoost();

    // Make a minivan.
    MiniVan mv = new MiniVan();
    mv.TurboBoost();
}
//UsingTypeFromCarLibrary();

void UsingInternalType()
{
    var internalClassInstance = new MyInternalClass();
    internalClassInstance.Hi();
}
UsingInternalType();

