namespace MiInterfaceHierarchy;
// Multiple interface inheritance.
interface IShape : IDrawable,IPrintable
{
    int GetNumberOfSide();
}
