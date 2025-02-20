using MiInterfaceHierarchy;

void MultipleInheritance1()
{
    Rectangle rectangle = new Rectangle();
    rectangle.Draw();
}
//MultipleInheritance1();

void MultipleInheritance2()
{
    Square square = new();

    ((IDrawable) square).Draw();

    ((IPrintable) square).Draw();

}
MultipleInheritance2();
